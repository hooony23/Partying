using UnityEngine;
using Util;
using System;
using System.IO;
using Communication.GameServer.API;
public class Player : PlayerUtil
{

    void Awake()
    {
        UserUuid = Config.userUuid;
        CameraArm = GameObject.Find("CameraArm").transform;
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();

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
        MoveChangeSend("Labylinth");
    }
    private void FixedUpdate() // default : 50fps
    {

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