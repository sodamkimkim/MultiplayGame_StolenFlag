using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMesh : MonoBehaviour
{
    private SkinnedMeshRenderer mr = null;
    private Texture2D[] playerTextures = null;
    private void Awake()
    {
        mr = GetComponent<SkinnedMeshRenderer>();
        playerTextures = Resources.LoadAll<Texture2D>("Textures");
        mr.material.mainTexture = playerTextures[1];
    }
    public void setMaterial(string _playerColor)
    {
        if (_playerColor.Equals(ScoreManager.eTeamName.BlueTeam))
        {
            // blue team
            mr.material.mainTexture = playerTextures[0];
        }
        else
        {
            mr.material.mainTexture = playerTextures[1];
            // red team

        }
    }
}
