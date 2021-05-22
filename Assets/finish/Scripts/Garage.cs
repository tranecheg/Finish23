using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garage : MonoBehaviour
{
    public GameObject carChoise;
    private bool isActive;
    public GameObject cam, upgrade;
    public GameObject[] lobby;

    void Update()
    {
        carChoise.SetActive(isActive);
    }
    public void ChangeCar()
    {
        isActive = !isActive;
    }
    public void ChangeGun()
    {
        cam.GetComponent<CamFocus>().enabled = true;
        for(int i =0; i < lobby.Length; i++)
        {
            lobby[i].SetActive(false);
        }
        upgrade.SetActive(true);
    }
    public void Lobby()
    {
        for (int i = 0; i < lobby.Length; i++)
        {
            lobby[i].SetActive(true);
        }
        upgrade.SetActive(false);
        GameObject.Find("Cam").GetComponent<CamFocus>().enabled = false;
        GameObject.Find("Cam").transform.position = new Vector3(17.3f, 13, 2);
        GameObject.Find("Cam").transform.GetChild(0).localPosition = new Vector3(-25f, 6f, 29);
        GameObject.Find("Cam").transform.GetChild(0).eulerAngles = new Vector3(12, 146.2f, 0);
    }
    

}
