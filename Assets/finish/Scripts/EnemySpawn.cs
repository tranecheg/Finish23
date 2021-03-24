using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
   
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(enemyPrefab.name, transform.position, Quaternion.identity);
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
