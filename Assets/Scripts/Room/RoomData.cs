using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour {
    private TMP_Text roomInfoText;
    private RoomInfo _roomInfo;
    public TMP_InputField userIdText;

    public RoomInfo RoomInfo
    {
        get
        {
            return _roomInfo;
        }
        set
        {
            _roomInfo = value;
            // ex) room_02 (1/2)
            roomInfoText.text = $"{_roomInfo.Name}({_roomInfo.PlayerCount}/{_roomInfo.MaxPlayers})";
            // 버튼의 클릭 이벤트에 함수를 연결
            GetComponent<Button>().onClick.AddListener(()=>OnEnterRoom(_roomInfo.Name));
        }
    }
    private void Awake()
    {
        roomInfoText = GetComponentInChildren<TMP_Text>();
        userIdText = GameObject.Find("InputField (TMP) - NickName").GetComponent<TMP_InputField>();
    }
    void OnEnterRoom(string roomName)
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;

        PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinOrCreateRoom(roomName, ro, TypedLobby.Default);
    }
} // end of class
