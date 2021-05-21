using System.Net.NetworkInformation;
using UnityEngine;
using Util;
using Communication;
using Boss;
using Weapon;
using GameUi;

public class Player : PlayerUtil
{
    void Start()
    {
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
        PlayerSound = GetComponent<PlayerSound>();
        Mat = transform.Find("큐브").gameObject.GetComponent<SkinnedMeshRenderer>().material;
        //CameraArm = GM.GetComponent<GameManager.GameManager>().PlayerCamera.transform;
        CameraMain = GameObject.Find("Main Camera").GetComponent<Camera>();
        CameraArm = GameObject.Find("CameraArm2").GetComponent<CMController>();

        CmFollowTarget = this.transform.Find("CM Follow Target").GetComponent<Transform>();

        Debug.Log("CmFollowTarget유무 : " + CmFollowTarget);

        ShotPoint = CameraMain.transform.Find("Shot Point").GetComponent<Transform>();

        UserScore = GM.GetComponent<UserScore>();
    

        UserScore = GM.GetComponent<UserScore>();
        // 플레이어 공격
        MusslePoint = transform.Find("Mussle Point").GetComponent<MusslePoint>();
    }


    void Update()
    {
        GetInput();
        CameraTurn();
        Aim();
        Move();
        Attack();
        Dodge();
        GetItem();
        UpdatePInfo();
        MoveChangeSend();
        // 피격 처리
        CheckHP();
        AnimationStart();
        CheckDeath();

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