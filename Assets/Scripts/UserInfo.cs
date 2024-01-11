using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    private string userId;
    private string userNicName;

    // player 이긴횟수
    private int cntWinning;
    
    // player 진 횟수
    private int cntLose; 

    // tot(myscore / teamscore) /100
    private int scoreRate;

    // (cntWin/cntWin+cntLose) *scoreRate
    private int nowRank;

    // 게임 시작할 때 0. 현재 진행중인 게임에서의 score
    private int inGamePersonalScore;

    public void SetInGamePersonalScore(int _score)
    {
        inGamePersonalScore += _score;
    }
} // end of class
