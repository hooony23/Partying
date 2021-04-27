using UnityEngine;
using Communication.GameServer.API;
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
        GetItem();
        Dodge();
        PlayerStateUpdate();
    }

    private void FixedUpdate() // default : 50fps
    {
        FreezeRotation();
        StopToWall();
    }
}
