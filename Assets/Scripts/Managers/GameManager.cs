using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }
            return m_instance;
        }
    }
    private static GameManager m_instance;
    [SerializeField]
    private GameObject bluePlayerPrefab;
    [SerializeField]
    private GameObject redPlayerPrefab;
    [SerializeField]
    private GameObject cvs_start;
    [SerializeField]
    private Button btnStart;
    [SerializeField]
    private Button btnLeave;
    [SerializeField]
    private GameObject flagGo;
    // private GameObject flagGoPrefab;
    private Vector3 flagOriginPos = new Vector3(0f, 5.863f, 0f);

    public int redScore { get; set; }
    public int blueScore { get; set; }
    private int gameEndRunTime = 0;
    private float gameNowtime;
    private Vector3 redPlayerOriginPos = new Vector3(-0.33f, 8.33f, -6.35f);
    private Vector3 bluePlayerOriginPos = new Vector3(0.33f, 8.33f, 6.35f);
    private bool isGameover = false;
    public static bool isGameStart = false;
    private List<GameObject> playerGoList = new List<GameObject>();

    private int playerIdx;


    private void Awake()
    {
        btnStart.onClick.AddListener(OnClickBtnStart);
        btnLeave.onClick.AddListener(OnClickBtnLeave);
        if (instance != this)
        {
            Destroy(gameObject);
        }
        gameNowtime = 0f;
        playerIdx = 0;
        gameEndRunTime = 1000;
        UIManager.instance.SetEndTime(gameEndRunTime);
        isGameStart = false;
        SetActiveCvsStart(true);

        SetInteractableBtnStart(false);

    }
    public void SetInteractableBtnStart(bool _active)
    {
        btnStart.interactable = _active;
    }
    /// <summary>
    /// canvas start
    /// </summary>
    /// <param name="_active"></param>
    [PunRPC]
    public void SetActiveCvsStart(bool _active)
    {
        cvs_start.SetActive(_active);

    }
    public void OnClickBtnStart()
    {
        photonView.RPC("SetActiveCvsStart", RpcTarget.All, false);
        //  SetActiveCvsStart(false);
        GameManager.isGameStart = true;
    }
    public void OnClickBtnLeave()
    {
        PhotonNetwork.LeaveRoom();

    }
    private void Start()
    {
        CreatePlayer();
    }
    private void CreatePlayer()
    {
        if (bluePlayerPrefab != null && redPlayerPrefab != null)
        {
            Vector3 playerPos = new Vector3(0, 0, 0);
            GameObject playerGo = null;
            int playerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
            if (playerCnt % 2 == 1)
            {
                playerGo = PhotonNetwork.Instantiate(bluePlayerPrefab.name, bluePlayerOriginPos, Quaternion.identity);
                playerGo.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                photonView.RPC("ApplyPlayerList", RpcTarget.All);
                UIManager.instance.SetNickName(playerGo.GetPhotonView().name);

                Debug.LogError("2:" + playerGo.name);
            }
            else
            {
                playerGo = PhotonNetwork.Instantiate(redPlayerPrefab.name, redPlayerOriginPos, Quaternion.identity);
                //playerIdx++;
                photonView.RPC("ApplyPlayerList", RpcTarget.All);

                UIManager.instance.SetNickName(playerGo.GetPhotonView().name);

                Debug.LogError("1:" + playerGo.name);
            }
        }
    }

    [PunRPC]
    public void ApplyPlayerIdx(int _playerIdx)
    {
        playerIdx = _playerIdx;
        UIManager.instance.SetplayerIdx(playerIdx);
    }
    [PunRPC]
    public void ApplyPlayerList()
    {
        int playerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
        if (playerCnt == playerGoList.Count) return;
        Debug.LogError("CurrentRoom PlayerCount: " + playerCnt);
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        playerGoList.Clear();
        for (int i = 0; i < playerCnt; ++i)
        {
            int key = i + 1;
            for (int j = 0; j < photonViews.Length; ++j)
            {
                if (photonViews[j].isRuntimeInstantiated == false) continue;
                if (PhotonNetwork.CurrentRoom.Players.ContainsKey(key) == false) continue;

                int viewNum = photonViews[j].Owner.ActorNumber;
                int playerNum = PhotonNetwork.CurrentRoom.Players[key].ActorNumber;
                if (viewNum == playerNum)
                {
                    photonViews[j].gameObject.name = "Player_" + photonViews[j].Owner.NickName;
                    playerGoList.Add(photonViews[j].gameObject);
                }
            }
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            SetInteractableBtnStart(true);
        }
        else
        {
            SetInteractableBtnStart(false);
        }
        if (!isGameover && isGameStart)
        {

            gameNowtime += Time.deltaTime;

            UIManager.instance.SetNowTime(gameNowtime);

            if (gameNowtime >= gameEndRunTime)
            {
                gameNowtime = gameEndRunTime;
                UIManager.instance.SetNowTime(gameNowtime);

                EndGame();
            }

        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PhotonNetwork.LeaveRoom();
        }

        UIManager.instance.SetplayerIdx(playerGoList.Count);


    }
    private void EndGame()
    {
        isGameover = true;
        isGameStart = false;
        if (blueScore > redScore)
        { // # blue∞° ¿Ã±Ë
            UIManager.instance.SetActiveBlueTeamWinUI(true);
            UIManager.instance.SetActiveRedTeamWinUI(false);
        }
        else if (blueScore < redScore)
        { // # red∞° ¿Ã±Ë
            UIManager.instance.SetActiveBlueTeamWinUI(false);
            UIManager.instance.SetActiveRedTeamWinUI(true);
        }
        else
        { // # ∫Ò±Ë
            UIManager.instance.SetActiveBlueTeamWinUI(false);
            UIManager.instance.SetActiveRedTeamWinUI(false);
        }
        UIManager.instance.SetActiveCvsGameOverUI(true);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

            stream.SendNext(gameEndRunTime);
            stream.SendNext(redScore);
            stream.SendNext(blueScore);
            stream.SendNext(gameNowtime);
            stream.SendNext(playerGoList.Count);
            stream.SendNext(isGameStart);
        }
        else
        {
            gameEndRunTime = (int)stream.ReceiveNext();
            redScore = (int)stream.ReceiveNext();
            blueScore = (int)stream.ReceiveNext();
            gameNowtime = (float)stream.ReceiveNext();
            playerIdx = (int)stream.ReceiveNext();
            isGameStart = (bool)stream.ReceiveNext();
            UIManager.instance.SetEndTime(gameEndRunTime);
            UIManager.instance.SetNowTime(gameNowtime);
            //  UIManager.instance.SetplayerIdx(playerIdx);

        }
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }

} // end of class
