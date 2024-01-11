using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLauncher : MonoBehaviourPunCallbacks
{
    private string gameVersion = "1";
    private byte maxPlayerPerRoom = 4;
    [SerializeField] private string nickName = string.Empty;
    [SerializeField] private Button connectButton = null;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Start()
    {
        connectButton.interactable = true;
    }
    public void Connect()
    {
        if (string.IsNullOrEmpty(nickName))
        {
            return;
        }
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    } // end of Connect()
    public void OnValueChangedNickName(string _nickName)
    {
        nickName = _nickName;
        PhotonNetwork.NickName = nickName;
    }
    public override void OnConnectedToMaster()
    {
        connectButton.interactable = false;
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("Disconnected: {0}", cause);
        connectButton.interactable = true;

        // ���� �����ϸ� OnJoinedRoom ȣ��
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        // �����Ͱ� PhotonNetwork.LoadLevel()�� ȣ���ϸ�,
        // ��� �÷��̾ ������ ������ �ڵ����� �ε�
        // PhotonNetwork.LoadLevel("Room"); // PhotonNetwork.LoadLevel()�� ������ Ŭ���̾�Ʈ������ ȣ��Ǿ�� �ϹǷ� isMasterclient�� �̿��� üũ�Ѵ�.
        // ���⼭�� �����Ͱ� ���ÿ� ������ �����ϰ� �ϴ� ������ �ƴϱ� ������ ���� ���� �θ��� ��.
        SceneManager.LoadScene("Main"); // Scenes in build�� ��ϵ� ���� Load����
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    { // �������� ���԰�, ó���� �� �����ϱ� ����� ���´�.
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);
        Debug.Log("Create Room");
        // random�̶� roomname null
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom }); // createRoom �Ǹ� -> joinRoom 
    }
} // end of class
