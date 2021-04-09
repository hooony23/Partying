using UnityEngine;
using Util;
using System;
using System.IO;
using Communication.API;
public class RaidPlayer : RaidPlayerUtil
{

    void Awake()
    {
        UserUuid = Config.userUuid;
        CameraArm = GameObject.Find("CameraArm").transform;
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();

        // 피격 처리 
        Mat = transform.Find("큐브").gameObject.GetComponent<SkinnedMeshRenderer>().material;
        BossController = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>();

    }

    private void Start()
    {
        
    }

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

        // 피격 처리
        CheckHP();
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