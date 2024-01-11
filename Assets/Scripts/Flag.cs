using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviourPunCallbacks
{
    private Transform flagTr = null;
    private Vector3 flagOriginPos = new Vector3(0f, 5.863f, 0f);
    [SerializeField]
    private GameObject[] flagSpawnPosArr = null;
    private void Awake()
    {
        flagTr = GetComponent<Transform>();
    }
    public Vector3 GetPos()
    {
        return transform.position;
    }
    private void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.CompareTag("BluePlayer") || _other.gameObject.CompareTag("RedPlayer"))
        {
            Debug.Log(_other.tag.ToString() + "가 flag를 차지했다!");
            //   flagTr.SetParent(playerTr);
            // 플레이어 오브젝트의 PhotonView ID 가져옴
            int playerId = _other.GetComponent<PhotonView>().ViewID;
            photonView.RPC("SetFlagParent", RpcTarget.All, playerId);
        }
    }
    [PunRPC]
    private void SetFlagParent(int _playerId)
    {
        // 플레이어 ID로 플레이어 오브젝트를 가져옴
        GameObject playerGo = PhotonView.Find(_playerId).gameObject;
        // 플레이어 오브젝트를 부모로 설정
        transform.SetParent(playerGo.transform);
        // 부모에 맞게 flag transform 변경해주는 함수
        photonView.RPC("SetFlagWithPlayer", RpcTarget.All);
    }
    [PunRPC]
    // player와 flag가 충돌했을 때 flag의 위치, 사이즈, rot변화
    private void SetFlagWithPlayer()
    {
        Transform parentTr = this.transform.parent;
        //Vector3 newPos = _parentTr.position;
        if (parentTr != null)
        {
            Vector3 newPos = Vector3.zero;
            newPos.x += 0.362f;
            newPos.y += 1.028f;
            newPos.z += -0.093f;
            flagTr.localPosition = newPos;


            flagTr.localScale = new Vector3(0.3f, 0.4f, 0.3f);
            Vector3 eulerAngle = parentTr.rotation.eulerAngles;
            eulerAngle.x += 90f;
            eulerAngle.z += 90f;
            flagTr.rotation = Quaternion.Euler(eulerAngle);
        }
    }

    // flag 골 후, random 위치로 배치
    public void SetFlagRandomPos()
    {
        this.gameObject.transform.SetParent(null);

        //Vector3 newPos = flagOriginPos;
        Vector3 newPos = flagSpawnPosArr[Random.Range(0, flagSpawnPosArr.Length - 1)].transform.position;
        flagTr.position = newPos;

        flagTr.localScale = new Vector3(1f, 1f, 1f);
        flagTr.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    /// <summary>
    /// 플레이어가 공에 맞았을 때 flag가 떨어지는 위치
    /// </summary>

    public void SetFlagPlayerArea(Vector3 _flagPos)
    {
        photonView.RPC("SetFlagPosPlayerArea", RpcTarget.All, _flagPos);
    }
    [PunRPC]
    private void SetFlagPosPlayerArea(Vector3 _flagPos)
    {
        //// 반경 설정
        //float radius = 3f;

        //// 반경 내의 무작위 위치 계산
        //Vector3 randomPosition = Random.insideUnitSphere * radius;

        // XZ 평면에서만 위치 조정
        //randomPosition.y = 0f;

        // 깃발 이동
        transform.position = _flagPos + Vector3.up *0.5f;
        //flagTr.localScale = new Vector3(1f, 1f, 1f);
        //flagTr.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

} // end of calss
