using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviourPun
{
    private Animator anim = null;
    private bool isThrowingAnim = false;
    private bool isThrowable = false;
    private const float throwCoolTime = 0f;
    private float throwCoolTimer = 0f;

    private GameObject ballGo = null;
    private float throwForce = 10f; // 던지는 힘의 크기
    private float throwAngle = 0f;
    private Rigidbody ballRb = null;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        isThrowable = true;
    }

    private IEnumerator ThrowBallCoroutine(int _ballId, Vector3 _dir)
    {
        yield return null;
    }
    private void Update()
    {
        if (!photonView.IsMine) return;
        if (!GameManager.isGameStart) return;
        throwCoolTimer += Time.deltaTime;

        if (Input.GetKey(KeyCode.W) && !isThrowingAnim && isThrowable)
        {
            isThrowingAnim = true;

            anim.SetBool("IsThrowing", true);
            Invoke("ThrowBall", 0.7f);
            Invoke("StopThrowingAnim", 1f);

        }

        // cooltime 과 UI
        if (throwCoolTimer < throwCoolTime)
        {
            isThrowable = false;
        }
        else if (throwCoolTimer >= throwCoolTime)
        {
            isThrowable = true;
            throwCoolTimer = throwCoolTime;
        }
        UIManager.instance.SetThrowUI(throwCoolTimer);
    }
    private void ThrowBall()
    {

        // 애니메이션 현재 프레임에서 손의 위치 계산
        Vector3 handPosition = anim.GetBoneTransform(HumanBodyBones.RightHand).position;
        if (this.gameObject.CompareTag("BluePlayer"))
        {
            ballGo = PhotonNetwork.Instantiate("P_Waterball", handPosition, Quaternion.identity);
        }
        else if (this.gameObject.CompareTag("RedPlayer"))
        {
            ballGo = PhotonNetwork.Instantiate("P_Fireball", handPosition, Quaternion.identity);
        }
        ballRb = ballGo.GetComponent<Rigidbody>();
        ballRb.useGravity = false;
        // 공 던짐
        Vector3 throwDir = Quaternion.Euler(0f, throwAngle, 0f) * transform.forward;
        Vector3 throwVelocity = throwDir * throwForce;
        ballRb.AddForce(throwVelocity, ForceMode.Impulse);
        //ballRb.useGravity = true; 
    }
    private void StopThrowingAnim()
    {
        isThrowingAnim = false;
        anim.SetBool("IsThrowing", false);
        throwCoolTimer = 0f;
       
    }

} // end of class
