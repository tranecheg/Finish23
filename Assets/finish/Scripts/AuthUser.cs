using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AuthUser : MonoBehaviour
{
    public InputField loginField, passField;
   
    public void AuthClick()
    {
        if (loginField.text != "" && passField.text != "")
            StartCoroutine(Auth(loginField.text, passField.text));


    }

    IEnumerator Auth(string login, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", login);
        form.AddField("password", pass);

        UnityWebRequest www = UnityWebRequest.Post("https://finish230.000webhostapp.com/auth.php", form); 
        yield return www.SendWebRequest();
        string data = www.downloadHandler.text;
        if (data == "Done")
        {
            Debug.Log("Success!!!");
            NetworkManager.OnLoginButtonClicked(login);
           
        }
        else
            Debug.Log("Error!!!");

    }
}
