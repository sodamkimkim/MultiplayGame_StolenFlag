using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private readonly string gameVersion = "v1.0";
    private string userId = "sodam";
    public TMP_InputField userIdText;
    public TMP_InputField roomNameText;

    // �� ��� �����ϱ� ���� ��ųʸ� �ڷ���
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    public GameObject roomPrefab;
    public Transform scrollContent;

    private void Awake()
    {
        // ������ ȥ�� ���� �ε��ϸ�, ������ ������� �ڵ����� ��ũ ��
        PhotonNetwork.AutomaticallySyncScene = true;
        // ���� ���� ����
        PhotonNetwork.GameVersion = gameVersion;
        // ���� ����
        PhotonNetwork.ConnectUsingSettings();
    }
    private void Start()
    {
        Debug.Log("00. ���� �Ŵ��� ����");
        userId = PlayerPrefs.GetString("USER_ID", $"USER_{Random.Range(0, 100):000}");
        userIdText.text = userId;
        PhotonNetwork.NickName = userId;
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("01. ���� ������ ����");
        // �κ� ����
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("02. �κ� ����");
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� �� ���� ����");
        // �� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 4;
        roomNameText.text = $"Room_{Random.Range(1, 100):000}";
        // �� ���� > �ڵ� ����
        PhotonNetwork.CreateRoom("room_1", ro);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("03. �� ���� �Ϸ�");
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("04. �� ���� �Ϸ�");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("Main");
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;
        foreach (var room in roomList)
        {
            // ���� ������ ���
            if (room.RemovedFromList == true)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            // �� ������ ����(����)�� ���
            else
            {
                // ���� ó�� ������ ���
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject _room = Instantiate(roomPrefab, scrollContent);
                    _room.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, _room);
                }
                // �� ������ �����ϴ� ���
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }
    }
    #region UI_BUTTON_CALLBACK
    // Random ��ư Ŭ��
    public void OnRandomBtn()
    {
        // Id��ǲ�ʵ� ���������
        if(string.IsNullOrEmpty(userIdText.text))
        {
            // �������̵� �ο�
            userId = $"USER_{Random.Range(0, 100):00}";
            userIdText.text = userId;
        }
        PlayerPrefs.SetString("USER_ID", userIdText.text);
        PhotonNetwork.NickName = userIdText.text;
        PhotonNetwork.JoinRandomRoom();
    }

    // Room ��ưŬ�� �� (�� ����)
    public void OnMakeRoomClick()
    {
        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;
        ro.IsVisible = true;
        ro.MaxPlayers = 2;
        // ��ǲ�ʵ尡 ���������
        if (string.IsNullOrEmpty(roomNameText.text))
        {
            roomNameText.text = $"ROOM_{Random.Range(1, 100):000}";
        }
        PhotonNetwork.CreateRoom(roomNameText.text, ro);
    }
    #endregion
} // end of class
