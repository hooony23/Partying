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
        turnSpeed = 0.5f;
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
        if (IsMyCharacter())
        {
            // 피격 처리
            if (Config.defaultStage==2)
            {
                Mat = transform.Find("큐브").gameObject.GetComponent<SkinnedMeshRenderer>().material;
            }

            //CameraArm = GM.GetComponent<GameManager.GameManager>().PlayerCamera.transform;
            CameraMain = GameObject.Find("Main Camera").GetComponent<Camera>();
            CameraArm = GameObject.Find("CameraArm2").GetComponent<CMController>();

            CmFollowTarget = this.transform.Find("CM Follow Target").GetComponent<Transform>();

            ShotPoint = CameraMain.transform.Find("Shot Point").GetComponent<Transform>();

            UserScore = GM.GetComponent<UserScore>();
        }

        // 플레이어 공격
        MusslePoint = transform.Find("Mussle Point").GetComponent<MusslePoint>();
    }

 

    void Update()
    {
        if (IsMyCharacter())
        {
            GetInput();
            CameraTurn();
            Aim();
            Move();
        }
        else
        {
            GetNetWorkInput();
            NetworkMove();
        }
        Attack();
        Dodge();
        GetItem();
        UpdatePInfo();
        if (IsMyCharacter())
        {
            MoveChangeSend();    
            // 피격 처리
            CheckHP();
        }
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
        if (IsMyCharacter())
            NearbyObject(other);
    }

    private void OnTriggerExit(Collider other) //플레이어 범위에 아이템이 벗어났는지 확인
    {
        if (IsMyCharacter())
            MoveAnyFromObject(other);
    }
}