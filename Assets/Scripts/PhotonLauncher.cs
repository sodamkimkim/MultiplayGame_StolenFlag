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

        // 방을 생성하면 OnJoinedRoom 호출
        Debug.Log("Create Room");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        // 마스터가 PhotonNetwork.LoadLevel()을 호출하면,
        // 모든 플레이어가 동일한 레벨을 자동으로 로드
        // PhotonNetwork.LoadLevel("Room"); // PhotonNetwork.LoadLevel()은 마스터 클라이언트에서만 호출되어야 하므로 isMasterclient를 이용해 체크한다.
        // 여기서는 마스터가 동시에 게임을 시작하게 하는 구조가 아니기 때문에 각자 씬을 부르면 됨.
        SceneManager.LoadScene("Main"); // Scenes in build에 등록된 씬만 Load가능
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    { // 서버에는 들어왔고, 처음엔 방 없으니까 여기로 들어온다.
        Debug.LogErrorFormat("JoinRandomFailed({0}): {1}", returnCode, message);
        Debug.Log("Create Room");
        // random이라서 roomname null
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayerPerRoom }); // createRoom 되면 -> joinRoom 
    }
} // end of class
