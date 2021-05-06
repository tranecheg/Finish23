using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathRaceGameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] PlayerPrefabs;
    public Text score;
    public int scoreForWin;

    // Start is called before the first frame update
    void Start()
    {
        
        if (PhotonNetwork.IsConnectedAndReady)
        {

            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            {
                int randomPosition = Random.Range(-30,30);

                PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name,new Vector3(randomPosition,0,randomPosition), Quaternion.identity);

            }

        }
        TakeDamage.scoreA = 0;
        TakeDamage.scoreB = 0;

        StartCoroutine(Leave());




    }
    private void Update()
    {
        score.text = "TeamA: " + TakeDamage.scoreA + "\nTeamB: " + TakeDamage.scoreB;
    }
    IEnumerator Leave()
    {
        while (true)
        {
            if (TakeDamage.scoreA > scoreForWin || TakeDamage.scoreB > scoreForWin)
                OnQuitMatchButtonClicked();

            yield return new WaitForSeconds(1f);
        }



    }


    public void OnQuitMatchButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("LobbyScene");

    }

    

   
}
