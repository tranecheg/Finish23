using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerParams : MonoBehaviour
{
    private string[] param;
    public Text login, nick, coins, level;
    public Slider exp;
    public static string loginDb;
    public static int levelDb, expDb, coinsDb;
    void Start()
    {
        if (login.text == "" || login.text==null)
            login.text = loginDb;
        StartCoroutine(getData(login.text));
        
    }
    IEnumerator getData(string login)
    {
        //loginField.text = "";
        //passField.text = "";

        WWWForm form = new WWWForm();
        form.AddField("login", login);
        UnityWebRequest www = UnityWebRequest.Post("https://finish230.000webhostapp.com/params.php", form);
        yield return www.SendWebRequest();
        string data = www.downloadHandler.text;
        data = data.Remove(data.Length - 1);
        param = data.Split(';');

        
        nick.text = getValue(param[0], "Login:");
        level.text = getValue(param[0], "Level:");
        coins.text = "Coins: " + getValue(param[0], "Coins:");
        exp.value = Convert.ToInt32(getValue(param[0], "Exp:"));


        if (nick.text != "NickName")
        {
            loginDb = nick.text;
            levelDb = Convert.ToInt32(getValue(param[0], "Level:"));
            coinsDb = Convert.ToInt32(getValue(param[0], "Coins:"));
            expDb = Convert.ToInt32(getValue(param[0], "Exp:"));
        }
        
        
    }
    string getValue(string data, string index)
    {
        string val = data.Substring(data.IndexOf(index) + index.Length);
        if(val.Contains("|"))
            val = val.Remove(val.IndexOf("|"));
        return val;
    }

    
}
