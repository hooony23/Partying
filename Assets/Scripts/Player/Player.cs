using System.Net.NetworkInformation;
using UnityEngine;
using Util;
using Communication;
using Boss;
public class Player : PlayerUtil
{
    void Awake()
    {
        // 피격 처리
        if (NetworkInfo.currentStage == 2)
        {
            CurrentStage = "Raid";
            Mat = transform.Find("큐브").gameObject.GetComponent<SkinnedMeshRenderer>().material;
        }
        UserUuid = Config.userUuid;
        CameraArm = GameObject.Find("CameraArm").transform;
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();

    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        GetItem();
        CameraTurn();
        Dodge();
        PlayerStateUpdate();
        MoveChangeSend();

        // 피격 처리
        CheckHP();
    }

    private void FixedUpdate() // default : 50fps
    {
        FreezeRotation();
        StopToWall();
    }

    private void OnTriggerEnter(Collider other) //플레이어 범위에 아이템이 인식할 수 있는지 확인
    {
        NearbyObject(other);
    }

    private void OnTriggerExit(Collider other) //플레이어 범위에 아이템이 벗어났는지 확인
    {
        MoveAnyFromObject(other);
    }
}