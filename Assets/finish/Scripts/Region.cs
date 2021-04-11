using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Region : MonoBehaviour
{
    public string region;

    private void Start()
    {
        
    }
    public void InputMenu(int value)
    {
        PhotonNetwork.Disconnect();
        region = GetComponent<Dropdown>().options[value].text;
        PhotonNetwork.ConnectToRegion(region);

    }
}
