using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance
    {
        get
        {
            if(m_instance ==null)
            {
                m_instance = FindObjectOfType<ScoreManager>();
            }
            
            return m_instance;
        }
    }

    private static ScoreManager m_instance;
    public enum eTeamName { BlueTeam, RedTeam }

    // Blue goal post obj를 넣어주기
    [SerializeField]
    Team blueTeam = null;
    // Red goal post obj를 넣어주기
    [SerializeField]
    Team redTeam = null;
    [SerializeField]
    private GameObject cvsCelemony = null;
    

    private void Awake()
    {
        cvsCelemony.gameObject.SetActive(false);
        if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void SetScore(Team _team, UserInfo _userInfo, int _score)
    {
        if (_team.GetTeamName().Equals(blueTeam.GetTeamName()))
        { // blueteam goal == blueteam player
            blueTeam.SetTeamScore(_score);
            GameManager.instance.blueScore= _score;
            UIManager.instance.UpdateBlueteamScore(blueTeam.GetTeamScore());
            cvsCelemony.gameObject.SetActive(true);
            Invoke("DeActiveCelemonyCvs", 2f);
        }
        if (_team.GetTeamName().Equals(redTeam.GetTeamName()))
        { // redteam goal == redteam player
            redTeam.SetTeamScore(_score);
            GameManager.instance.redScore = _score;
            UIManager.instance.UpdateRedteamScore(redTeam.GetTeamScore());
            cvsCelemony.gameObject.SetActive(true);
            Invoke("DeActiveCelemonyCvs", 2f);

        }
        // 개인 점수 정보도 업데이트해줌
        _userInfo.SetInGamePersonalScore(_score);
    }
    private void DeActiveCelemonyCvs()
    {
        cvsCelemony.gameObject.SetActive(false);
    }
    public override string ToString()
    {
        return "BlueTeam: " + blueTeam.GetTeamScore() + " RedTeam: " + redTeam.GetTeamScore();
    }
} // end of class
