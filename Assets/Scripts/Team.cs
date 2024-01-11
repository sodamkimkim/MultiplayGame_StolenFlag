using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Team :MonoBehaviourPun
{

    public abstract string GetTeamName();
    public abstract List<string> GetTeamMemList();
    public abstract int GetTeamScore();
    public abstract void SetTeamMemList(string _playerNickName);
    public abstract void SetTeamScore(int _score);
    [PunRPC]
    public abstract void SetUp(string _playerColor);
} // end of class
