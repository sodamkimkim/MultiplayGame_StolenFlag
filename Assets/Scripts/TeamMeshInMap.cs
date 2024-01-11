using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMeshInMap : MonoBehaviour
{
    private MeshRenderer mr = null;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }
    public void SetMaterial(string _playerColor)
    {
        if (_playerColor.Equals(ScoreManager.eTeamName.BlueTeam))
        {
            mr.material.color = new Color(0, 0, 255);
        }
        else
        {
            mr.material.color = new Color(255, 0, 0);
        }
    }
} // end of class
