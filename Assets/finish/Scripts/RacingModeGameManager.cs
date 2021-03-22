using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RacingModeGameManager : MonoBehaviourPunCallbacks
{
    public GameObject[] PlayerPrefabs;
    public Transform[] InstantiatePositions;

    public Text TimeUIText;
    public GameObject[] FinishOrderUIGameObjects;

    public List<GameObject> lapTriggers = new List<GameObject>();

    //Singeleton Implementation
    public static RacingModeGameManager instance = null;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;



    }


    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerRacingGame.PLAYER_SELECTION_NUMBER, out playerSelectionNumber ))
            {
                Debug.Log((int)playerSelectionNumber);

                int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                Vector3 instantiatePosition = InstantiatePositions[actorNumber-1].position;

                PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name,instantiatePosition,Quaternion.identity);

            }


        }


        foreach (GameObject gm in FinishOrderUIGameObjects)
        {
            gm.SetActive(false);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
