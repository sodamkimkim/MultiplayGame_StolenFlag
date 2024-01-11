using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class GoalPoint : MonoBehaviourPun
{
    [SerializeField]
    private GameObject blueCelebration = null;
    [SerializeField]
    private GameObject redCelebration = null;

    private const int flagScore = 1;
    private void OnCollisionEnter(Collision _other)
    {
        if (_other.gameObject.CompareTag("BluePlayer") || _other.gameObject.CompareTag("RedPlayer"))
        {
            // 다른팀에 점수 주는 거 허용x
            Flag flag = _other.gameObject.GetComponentInChildren<Flag>();
            if (flag != null)
            {
                UserInfo userInfo = _other.gameObject.GetComponent<UserInfo>();
                Debug.Log("깃발가진 player다!");

                if (this.gameObject.CompareTag(ScoreManager.eTeamName.BlueTeam.ToString())) // bluteam post일 때 여기 실행
                {
                    if (_other.gameObject.CompareTag("BluePlayer"))
                    {
                        Team blueTeam = _other.gameObject.GetComponent<BlueTeam>();
                        flag.SetFlagRandomPos();
                        GetScoreActions(blueTeam, userInfo);
                    }
                    else
                    {
                        Debug.Log("여기는 BlueTeam post다!!!");
                    }

                }
                if (this.gameObject.CompareTag(ScoreManager.eTeamName.RedTeam.ToString())) // redteam post일 때 여기 실행
                {
                    if (_other.gameObject.CompareTag("RedPlayer"))
                    {
                        Team redTeam = _other.gameObject.GetComponent<RedTeam>();
                        flag.SetFlagRandomPos();
                        GetScoreActions(redTeam, userInfo);

                    }
                    else
                    {
                        Debug.Log("여기는 RedTeam post다!!!");
                    }
                }

            }
            else
            {
                Debug.Log("Player인데 깃발은 없네");
            }

        }

    }
    private void GetScoreActions(Team _team, UserInfo _userInfo)
    {
        Transform userTr = _userInfo.gameObject.GetComponent<Transform>();

        Debug.LogError(_team.GetTeamName() + " 1점 획득!");
        // Team정보,  player정보랑 점수 ScoreManager에 던져줌
        ScoreManager.instance.SetScore(_team, _userInfo, flagScore);
        LaunchCelebration(_team, userTr);
        Debug.Log(ScoreManager.instance.ToString());
    }
    private void LaunchCelebration(Team _team, Transform _playerTr)
    {
        Debug.LogError(_team.GetTeamName());
        GameObject go = null;
        if (_team.GetTeamName().Equals(ScoreManager.eTeamName.BlueTeam.ToString()))
        {
            go = PhotonNetwork.Instantiate(blueCelebration.name, _playerTr.position, Quaternion.identity);
        }
        else
        {
            go = PhotonNetwork.Instantiate(redCelebration.name, _playerTr.position, Quaternion.identity);
        }
        go.transform.SetParent(_playerTr);
        Destroy(go, 3);
    }

} // end of class
