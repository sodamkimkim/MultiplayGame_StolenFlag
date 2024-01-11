using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    private string userId;
    private string userNicName;

    // player �̱�Ƚ��
    private int cntWinning;
    
    // player �� Ƚ��
    private int cntLose; 

    // tot(myscore / teamscore) /100
    private int scoreRate;

    // (cntWin/cntWin+cntLose) *scoreRate
    private int nowRank;

    // ���� ������ �� 0. ���� �������� ���ӿ����� score
    private int inGamePersonalScore;

    public void SetInGamePersonalScore(int _score)
    {
        inGamePersonalScore += _score;
    }
} // end of class
