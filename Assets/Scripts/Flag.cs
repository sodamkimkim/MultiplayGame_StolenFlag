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
            Debug.Log(_other.tag.ToString() + "�� flag�� �����ߴ�!");
            //   flagTr.SetParent(playerTr);
            // �÷��̾� ������Ʈ�� PhotonView ID ������
            int playerId = _other.GetComponent<PhotonView>().ViewID;
            photonView.RPC("SetFlagParent", RpcTarget.All, playerId);
        }
    }
    [PunRPC]
    private void SetFlagParent(int _playerId)
    {
        // �÷��̾� ID�� �÷��̾� ������Ʈ�� ������
        GameObject playerGo = PhotonView.Find(_playerId).gameObject;
        // �÷��̾� ������Ʈ�� �θ�� ����
        transform.SetParent(playerGo.transform);
        // �θ� �°� flag transform �������ִ� �Լ�
        photonView.RPC("SetFlagWithPlayer", RpcTarget.All);
    }
    [PunRPC]
    // player�� flag�� �浹���� �� flag�� ��ġ, ������, rot��ȭ
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

    // flag �� ��, random ��ġ�� ��ġ
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
    /// �÷��̾ ���� �¾��� �� flag�� �������� ��ġ
    /// </summary>

    public void SetFlagPlayerArea(Vector3 _flagPos)
    {
        photonView.RPC("SetFlagPosPlayerArea", RpcTarget.All, _flagPos);
    }
    [PunRPC]
    private void SetFlagPosPlayerArea(Vector3 _flagPos)
    {
        //// �ݰ� ����
        //float radius = 3f;

        //// �ݰ� ���� ������ ��ġ ���
        //Vector3 randomPosition = Random.insideUnitSphere * radius;

        // XZ ��鿡���� ��ġ ����
        //randomPosition.y = 0f;

        // ��� �̵�
        transform.position = _flagPos + Vector3.up *0.5f;
        //flagTr.localScale = new Vector3(1f, 1f, 1f);
        //flagTr.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

} // end of calss
