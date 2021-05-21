using System.Net.NetworkInformation;
using UnityEngine;
using Util;
using Communication;
using Boss;
using Weapon;
using GameUi;
public class OtherPlayer : PlayerUtil
{
    void Start()
    {
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
        Mat = transform.Find("큐브").gameObject.GetComponent<SkinnedMeshRenderer>().material;
        // 플레이어 공격
        MusslePoint = transform.Find("Mussle Point").GetComponent<MusslePoint>();
    }

 

    void Update()
    {
        GetNetWorkInput();
        NetworkMove();
        Attack();
        Dodge();
        GetItem();
        AnimationStart();
        CheckDeath();

    }

    private void FixedUpdate() // default : 50fps
    {
        FreezeRotation();
        StopToWall();
    }
}