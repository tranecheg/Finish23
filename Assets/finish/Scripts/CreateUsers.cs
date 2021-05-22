using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CreateUsers : MonoBehaviour
{
    public InputField loginField, passField;
    private string[] users;
    private List<string> logins = new List<string>();

    void Start()
    {
        StartCoroutine(getData());
    }
    IEnumerator getData()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            UnityWebRequest items = UnityWebRequest.Get("https://finish230.000webhostapp.com/index.php");
            yield return items.SendWebRequest();
            
            string data = items.downloadHandler.text;

            data = data.Remove(data.Length - 1);
            users = data.Split(';');

            for (int i = 0; i < users.Length; i++)
            {
                logins.Add(getValue(users[i], "Login:"));
            }
        }
            

        
        
    }
    string getValue(string data, string index)
    {
        string val = data.Substring(data.IndexOf(index) + index.Length);
        val = val.Remove(val.IndexOf("|"));
        return val;
    }

    public void Add()
    {
        for (int i = 0; i < logins.Count; i++)
        {
            if (logins[i] == loginField.text)
                return;
        }
        if (loginField.text != "" && passField.text != "")
        {
            AddUser(loginField.text, passField.text);
            
        }
            


    }
    
        void AddUser(string login, string pass)
        {
        //loginField.text = "";
        //passField.text = "";

        WWWForm form = new WWWForm();
        form.AddField("login",login);
        form.AddField("password", pass);

        UnityWebRequest.Post("https://finish230.000webhostapp.com/reg.php", form).SendWebRequest();
       

        }
}
