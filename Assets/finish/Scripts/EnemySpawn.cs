using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyPrefab, TeamA, TeamB;
    
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TeamA = GameObject.FindGameObjectsWithTag("Player");
        createTeamA();
        TeamB = GameObject.FindGameObjectsWithTag("Enemy");
        createTeamB();


    }
    void createTeamA()
    {
        if (TeamA.Length < 5)
        {
            if (PhotonNetwork.IsMasterClient)
            {

                GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab[Random.Range(0, 3)].name,
                    new Vector3(Random.Range(-20,20), 1, -55), Quaternion.identity);
                
                enemy.name = "PlayerA " + TeamA.Length;
                enemy.tag = "Player";

            }
        }
    }
    void createTeamB()
    {
        if (TeamA.Length < 5)
        {
            if (PhotonNetwork.IsMasterClient)
            {

                GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab[Random.Range(0, 3)].name,
                    new Vector3(Random.Range(-20, 20), 1, 55), Quaternion.identity);
                
                enemy.name = "PlayerB " + TeamB.Length;
                enemy.tag = "Enemy";

            }
        }
    }
}
