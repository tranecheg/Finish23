using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGuns : MonoBehaviourPun
{
    public GameObject[] guns;
    void Start()
    {
        if (!GetComponent<PhotonView>().Controller.IsLocal)
            Destroy(GetComponent<CreateGuns>());

        if (photonView.IsMine && PhotonNetwork.PlayerList.Length > 1)
            photonView.RPC("GunCreate", RpcTarget.Others);
        else if(PhotonNetwork.PlayerList.Length <= 1)
            GunCreate();

    }

    [PunRPC]
    public void GunCreate()
    {
        switch (GunSelected.selectionNumber)
        {
            case 0:
                GameObject gun = PhotonNetwork.Instantiate(guns[0].name, GunSelected.selectionPos, Quaternion.Euler(GunSelected.selectionRot));
                gun.name = "LaserGun";
                gun.transform.SetParent(transform.GetChild(1));

                break;
            case 1:
                GameObject gun1 = PhotonNetwork.Instantiate(guns[1].name, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                gun1.name = "MachineGun";
                gun1.transform.SetParent(transform.GetChild(1));
                break;
            case 2:
                GameObject gun2 = PhotonNetwork.Instantiate(guns[2].name, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                gun2.name = "RocketLauncher";
                gun2.transform.SetParent(transform.GetChild(1));
                break;

        }
    }

}
