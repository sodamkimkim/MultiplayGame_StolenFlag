using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }
    private static UIManager m_instance;

    [SerializeField]
    private TextMeshProUGUI redteamScoreTxt;

    [SerializeField]
    private TextMeshProUGUI blueteamScoreTxt;

    [SerializeField]
    private GameObject cvsGameOverUI; // 게임 오버시 활성화할 UI
    [SerializeField]
    private GameObject blueteamWinUI;
    [SerializeField]
    private GameObject redteamWinUI;
    [SerializeField]
    private TextMeshProUGUI endTimeTxt;
    [SerializeField]
    private TextMeshProUGUI nowTimeTxt;

    [SerializeField]
    private TextMeshProUGUI playerIdxTxt;

    [SerializeField]
    private TextMeshProUGUI  playerNickText;

    // skill UI
    [SerializeField]
    private GameObject dashUIGo = null;
    private TMP_Text dashCooltimeText = null;
    [SerializeField]
    private GameObject throwUIGo = null;
    private TMP_Text throwCooltimeText = null;

    private void Awake()
    {
        if(instance != this)
        {
            Destroy(gameObject);
        }
        SetActiveCvsGameOverUI(false);
        dashCooltimeText = dashUIGo.GetComponentInChildren<TMP_Text>();
        throwCooltimeText = throwUIGo.GetComponentInChildren<TMP_Text>();
    }
    public void SetDashUI(float _coolTime)
    {
        dashCooltimeText.text = ""+Mathf.Floor(_coolTime*10f)/10f;
    }
    public void SetThrowUI(float _coolTime)
    {
        throwCooltimeText.text = "" + Mathf.Floor(_coolTime * 10f) / 10f;

    }
    public void UpdateRedteamScore(int _newScore)
    {
        redteamScoreTxt.text = "" + _newScore;
    }
    public void UpdateBlueteamScore(int _newScore)
    {
        blueteamScoreTxt.text = "" + _newScore;
    }
    public void SetActiveBlueTeamWinUI(bool _active)
    {
        blueteamWinUI.SetActive(_active);
    }
    public void SetActiveRedTeamWinUI(bool _active)
    {
        redteamWinUI.SetActive(_active);
    }

    public void SetActiveCvsGameOverUI(bool _active)
    {
        cvsGameOverUI.SetActive(_active);
    }
    public void SetNowTime(float _nowTime)
    {
        nowTimeTxt.text = ""+ (int)_nowTime;
    }
    public void SetEndTime(int _endTime)
    {
        endTimeTxt.text = "" + _endTime;
    }
    public void SetplayerIdx(int _playerIdx)
    {
        playerIdxTxt.text = "" + _playerIdx;
    }
    public void SetNickName(string _nickName)
    {
        playerNickText.text = "" + _nickName;
    }
} // end of class
