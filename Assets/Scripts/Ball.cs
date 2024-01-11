using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviourPun
{
    private const float ballDuration = 2f;
    private void OnTriggerEnter(Collider _other)
    {
        if (this.gameObject.CompareTag("Waterball"))
        {

            if (_other.CompareTag("RedPlayer"))
            {
                int playerId = _other.GetComponent<PhotonView>().ViewID;
                photonView.RPC("DestroyBallByPlayerAttack", RpcTarget.All, playerId);
            }
            else
            {
                photonView.RPC("DestroySelf", RpcTarget.All);
            }
        }
        else if (this.gameObject.CompareTag("Fireball"))
        {
            if (_other.CompareTag("BluePlayer"))
            {
                int playerId = _other.GetComponent<PhotonView>().ViewID;
                photonView.RPC("DestroyBallByPlayerAttack", RpcTarget.All, playerId);
            }
            else
            {
                photonView.RPC("DestroySelf", RpcTarget.All);
            }
        }

    }
    [PunRPC]
    public void DestroySelf()
    {
        Destroy(this.gameObject, ballDuration);
    }
    [PunRPC]
    public void DestroyBallByPlayerAttack(int _playerId)
    {
        GameObject playerGo = PhotonView.Find(_playerId).gameObject;


        Flag flag = playerGo.GetComponentInChildren<Flag>();
        if (flag != null)
        {
            int flagId = flag.GetComponent<PhotonView>().ViewID;
            Vector3 flagPos = flag.GetPos();
            photonView.RPC("FlagSetParentNull", RpcTarget.All, flagId, flagPos);
        }
        Destroy(this.gameObject);
        Debug.LogError(playerGo.tag + "¸Â¾Ò´Ù.");
    }

    [PunRPC]
    public void FlagSetParentNull(int _flagId, Vector3 _flagPos)
    {
        GameObject flagGO = PhotonView.Find(_flagId).gameObject;
        flagGO.transform.SetParent(null);
        Flag flag = flagGO.GetComponent<Flag>();
        flag.SetFlagPlayerArea(_flagPos);
        
    }
} // end of class
