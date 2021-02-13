using UnityEngine;
using Util;
using System;
using System.IO;
using Communication.API;
public class Player : PlayerUtil
{
    [SerializeField] private GameObject cameraArm;

    void Awake()
    {

        CameraArm = cameraArm.transform;
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        // UserUuid = Config.userUuid;

    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        IsGetItem();
        CameraTurn();
        Dodge();
        PlayerStateUpdate();
        
    }
    private void FixedUpdate() // default : 50fps
    {
        /* 서버 전송 */
        // CharacterInfo 에 현재 플레이어의 상태 입력
        // CharacterInfo 를 서버로 전송
        // PInfo.SetInfo(transform.position, MoveDir, PlayerState, UserUuid); 
        // APIController.SendController("Move",PInfo.ObjectToJson());
        FreezeRotation();
        StopToWall();
    }



    private void OnTriggerStay(Collider other) //플레이어 범위에 아이템이 인식할 수 있는지 확인
    {
        IsClear(other);

    }
    private void OnTriggerExit(Collider other) //플레이어 범위에 아이템이 벗어났는지 확인
    {
        IsGetItem(other);
    }
}