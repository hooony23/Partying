﻿using UnityEngine;
using Communication.API;
using Communication.JsonFormat;

public class OtherPlayer : PlayerUtil
{
    
    void Awake()
    {
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();

    }

    // 에티터 플레이버튼, 앱의 종료 -> 생명주기 종료

    // Update is called once per frame
    void Update()
    {
        GetNetWorkInput();
        Move();
        Turn();
        IsGetItem();
        Dodge();
        PlayerStateUpdate();
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