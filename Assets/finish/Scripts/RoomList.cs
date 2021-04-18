using Photon.Pun;
using Photon.Realtime;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomList : MonoBehaviourPunCallbacks
{
    public string room;
    public List<RoomInfo> roomsList;
    RoomInfo info;

    private void Update()
    {
       
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {

        room = roomList[0].Name;


    }

    

    public void GetName(RoomInfo _info)
    {
        info = _info;
        room = info.Name;
    }

}
