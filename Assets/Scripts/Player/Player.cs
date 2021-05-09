using System.Net.NetworkInformation;
using UnityEngine;
using Util;
using Communication;
using Boss;
using Weapon;
using GameUi;
public class Player : PlayerUtil
{
    void Awake()
    {
        // TODO: 스테이지 1에서 총 안보이게.
        // if(Config.defaultStage==1)
        // {
        //     Destroy(this.gameObject.transform.Find("아마튜어").Find("spine").Find("handgun").gameObject);
        // }
        UserUuid = Config.userUuid;
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        if (IsMyCharacter())
        {
            // 피격 처리
            if (Config.defaultStage==2)
            {
                Mat = transform.Find("큐브").gameObject.GetComponent<SkinnedMeshRenderer>().material;
            }
            CameraArm = GameObject.Find("CameraArm").transform;
        }
        GM = GameObject.Find("GameManager");
        UserScore = GM.GetComponent<UserScore>();
        // 플레이어 공격
        Pistol = transform.Find("Mussle Point").GetComponent<MusslePoint>();
        ShotPoint = CameraArm.Find("Shot Point").transform;

    }

    void Update()
    {
        if (IsMyCharacter())
            GetInput();
        else
            GetNetWorkInput();
        Move();
        Turn();
        Attack();
        Dodge();
        GetItem();
        CameraTurn();
        AnimationStart();
        if (IsMyCharacter())
            MoveChangeSend();

        // 피격 처리
        CheckHP();
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