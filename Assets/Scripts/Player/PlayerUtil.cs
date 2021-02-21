using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;


public class PlayerUtil : MonoBehaviour
{
    public PlayerController playerController = new PlayerController();
    [Range(0.01f, 10)] public float mouseSensitivity = 1;

    public void GetInput()
    {

        playerController.HAxis = Input.GetAxis("Horizontal");
        playerController.VAxis = Input.GetAxis("Vertical");
        playerController.EDown = Input.GetKeyDown(KeyCode.E); //E키를 통한 아이템 습득
        playerController.JDown = Input.GetButtonDown("Jump"); // GetButtonDown : (일회성) 점프, 회피    GetButton : (차지) 모으기
        playerController.MouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 마우스를 통해 플레이어 화면 움직임
        playerController.MoveInput = new Vector2(playerController.HAxis, playerController.VAxis).normalized; // TPS 움직임용 vector


    }

    // 플레이어 상태를 프레임마다 업데이트(네트워크 애니메이션 연계 용도)
    public void PlayerStateUpdate()
    {
        // 애니메이션 : run, dodge 의 bool값 확인후 true 가 되면 "상태" 전송
        if (playerController.IsMove == true)
            playerController.PlayerState = "Move";
        if (playerController.IsDodge == true)
            playerController.PlayerState = "Dodge";
    }

    public void Move()
    {
        playerController.MoveVec = new Vector3(playerController.MoveInput.x, 0f, playerController.MoveInput.y).normalized; // Dodge 방향용 vector

        if (playerController.IsStun == false)
        {
            playerController.IsMove = true;
            Debug.DrawRay(playerController.CameraArm.position, playerController.CameraArm.forward, Color.red);

            Vector3 lookForward = new Vector3(playerController.CameraArm.forward.x, 0f, playerController.CameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(playerController.CameraArm.right.x, 0f, playerController.CameraArm.right.z).normalized;
            // 마우스로 바라보고 있는 벡터를 방향벡터로 바꿈
            playerController.MoveDir = (lookForward * playerController.MoveInput.y + lookRight * playerController.MoveInput.x).normalized;

            transform.forward = lookForward; // 마우스가 바라보는 방향을 캐릭터가 바라보도록 함
            transform.position += playerController.MoveDir * Time.deltaTime * playerController.PlayerSpeed;
        }

        if (playerController.IsStun == true)
        {
            playerController.MoveDir *= 0.1f;
        }

        if (playerController.IsBorder == true)
        {
            Debug.Log("벽 충돌!!");
            // 벽앞에서 멈추는거 필요, moveVec 을 0으로 하는건 안됨
        }

        // run 애니메이션
        playerController.Anim.SetBool("isRun", playerController.MoveInput != Vector2.zero); // 움직이는 상태 -> isRun 애니메이션 실행

        if (playerController.MoveInput == Vector2.zero)
        {
            playerController.IsMove = false;
        }

    }
    
    public void Turn()
    {
        transform.LookAt(transform.position + playerController.MoveDir);
    }
    
    /// <summary>
    /// 마우스 이동에 의한 카메라 각도 제한
    /// </summary>
    public void CameraTurn()
    {
        // transform 의 z축을(z : 앞뒤, x : 좌우, y : 상하) vector 가 생기는 방향쪽으로 바라보게 함
        // transform.LookAt(transform.position + moveVec);

        Vector3 camAngle = playerController.CameraArm.rotation.eulerAngles;
        float x = camAngle.x - playerController.MouseDelta.y * mouseSensitivity;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        playerController.CameraArm.rotation = Quaternion.Euler(x, camAngle.y + playerController.MouseDelta.x * mouseSensitivity, camAngle.z);

    }


    public void GetItem() // 아이템 획득을 위한 로직
    {
        if (playerController.EDown && playerController.NearObject != null)
        { //아이템을 먹었을 때 실행하는 문장
            if (playerController.NearObject.CompareTag("Item"))
            {
                playerController.GetItem = true;
            }
        }
    }
    /*public void OpenPreferences() {
        if (playerController.PDown) {
        }
    }*/

    public void Dodge() // 플레이어 회피
    {
        if (playerController.JDown && playerController.IsDodge == false && playerController.MoveDir != Vector3.zero)
        {
            playerController.IsDodge = true;
            playerController.PlayerSpeed *= 2;

            playerController.Anim.SetTrigger("doDodge");

            Invoke("DodgeOut", 0.4f); // 회피중인 시간, 후에 원래대로 돌아가는 DodgeOut 실행
        }
    }
    public void DodgeOut() // 플레이어 회피 동작 이후 원래상태로 복구
    {
        playerController.IsDodge = false;
        playerController.PlayerSpeed *= 0.5f;
    }

    // 플레이어 스턴
    public void Stun(float time) // 구멍함정은 타임을 3초로 줄 것
    {
        StartCoroutine("StunForSec", time);
    }
    public IEnumerator StunForSec(float time)
    {
        playerController.IsStun = true;
        yield return new WaitForSeconds(time);
        playerController.IsStun = false;

    }

    // 플레이어 죽음
    public void Dead()
    {
        // isDead == true 일때
        Debug.Log("플레이어가 죽었습니다!!");
    }


    /*@@@ 충돌해결, 콜리전 처리 등 @@@*/
    // 물리 충돌 해결 -> FixedUpdate
    // 충돌로 인한 회전력 발생 무력화
    public void FreezeRotation()
    {
        playerController.Rigid.angularVelocity = Vector3.zero;
    }
    public void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 1f, Color.green);
        playerController.IsBorder = Physics.Raycast(transform.position, transform.forward, 1f, LayerMask.GetMask("Wall"));
    }

    public void IsClear(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            playerController.NearObject = other.gameObject;
            if (playerController.GetItem == true)
            {
                Destroy(playerController.NearObject,3f); // 이동과 동시에 아이템 오브젝트가 사라짐
                Config.GameClear = true;
            }
        }
    }
   

    public void IsGetItem(Collider other)
    {

        if (other.CompareTag("Item"))
        {
            playerController.NearObject = null;
            Config.GameClear = true;
        }
    }
}
