using System.Dynamic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Communication;
using Communication.GameServer.API;
using Communication.JsonFormat;
using Util;
public class PlayerUtil : PlayerController
{
    private Vector3 preMoveDir = Vector3.zero;
    private bool sendFlag = true;

    [Range(0.01f, 20f)] public float turnSpeed;
    public float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;
    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;
    public void GetInput()
    {
        if (IsDead||!GM.GameStart)
            return;
        HAxis = 0f;
        VAxis = 0f;
        InputEvent(GetInputKeys());
        MoveInput = new Vector2(HAxis, VAxis).normalized; // TPS 움직임용 vector
        MouseClickInput = Input.GetMouseButton(0);
        MoveVec = new Vector3(MoveInput.x, 0f, MoveInput.y).normalized; // Dodge 방향용 vector

    }
    public void GetNetWorkInput()
    {
        if (NetworkInfo.playersInfo.ContainsKey(this.gameObject.name) && !IsDead)
            if (NetworkInfo.playersInfo[this.gameObject.name] != null && NetworkInfo.playersInfo[this.gameObject.name] != PInfo)
            {
                PInfo = NetworkInfo.playersInfo[this.gameObject.name];
                this.gameObject.transform.position = new Vector3(PInfo.loc.X, PInfo.loc.Y, PInfo.loc.Z);
            }
        MoveDir = new Vector3(PInfo.vec.X, PInfo.vec.Y, PInfo.vec.Z);
        PlayerState = PInfo.movement;
    }
    public bool IsMyCharacter()
    {
        return this.UserUuid == Config.userUuid;
    }
    public void InputEvent(IEnumerable<KeyCode> keyArray)
    {
        if(keyArray.Contains(KeyCode.A))
            HAxis = -1f;
        if(keyArray.Contains(KeyCode.D))
            HAxis = 1f;
        if(keyArray.Contains(KeyCode.W))
            VAxis = 1f;
        if(keyArray.Contains(KeyCode.S))
            VAxis = -1f;
        if(keyArray.Contains(KeyCode.E))
            EDown = Input.GetKeyDown(KeyCode.E); //E키를 통한 아이템 습득
        if(keyArray.Contains(KeyCode.Space))
            JDown = Input.GetKeyDown(KeyCode.Space); // GetButtonDown : (일회성) 점프, 회피    GetButton : (차지) 모으기
    }
    public void MoveChangeSend()
    {
        if ((((MoveDir == preMoveDir)&&sendFlag)|| PlayerState == Movement.Shot) && !IsDead)
        {
            APIController.SendController("Move", PInfo);
            sendFlag=false;
        }
        preMoveDir = MoveDir;
    }
    public bool IsKeyInput()
    {
        var inputData = from input in Config.InputKey.GetValues(typeof(Config.InputKey))
                        .Cast<Config.InputKey>()
                        .Select(k => (KeyCode)k)
                        .ToList()
                        where Input.GetKeyDown(input) || Input.GetKeyUp(input)
                        select input;
        if (inputData.Count() <= 0)
        {
            return false;
        }
        return true;
    }
    public IEnumerable<KeyCode> GetInputKeys()
    {
        var inputData = from input in Config.InputKey.GetValues(typeof(Config.InputKey))
                        .Cast<Config.InputKey>()
                        .Select(k => (KeyCode)k)
                        .ToList()
                        where Input.GetKey(input)
                        select input;
        return inputData;
    }
    public void Move()
    {

        if (MoveVec.magnitude >= 0.1f && !IsStun && !MouseClickInput && !IsAttack)
        {
            Cursor.visible = false;

            IsMove = true;
            // 만약 현재 플레이어가 조정하고 있는 캐릭터라면 마우스가 바라보는 방향을 캐릭터가 바라보도록 함
            if (IsMyCharacter())
            {
                // 키보드 움직임 
                float targetAngle = Mathf.Atan2(MoveVec.x, MoveVec.z) * Mathf.Rad2Deg 
                    + CameraMain.transform.eulerAngles.y;               // 카메라가 플레이어를 보는기준으로 플레이어 방향 정함
                
                float angle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle,
                    ref turnSmoothVelocity, turnSmoothTime);            // 플레이어의 방향전환을 부드럽게 함
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                MoveDir = moveDir;
                if(MoveDir != preMoveDir)
                    sendFlag = true;
            }
            if (!IsBorder) // 벽에 부딛힌 경우 위치는 옮기지 않는다(애니메이션은 작동)
            {
                this.transform.position += MoveDir.normalized * Time.deltaTime * PlayerSpeed;
            }
        }

        if (IsStun == true)
        {
            MoveDir *= 0.1f;
        }

        if (MoveInput == Vector2.zero)
        {
            IsMove = false;
        }
        PlayerState = Movement.Run;
    }
    public void UpdatePInfo()
    {

        if (IsMyCharacter())
        {
            PInfo = new PlayerInfo(this.transform.position, MoveDir, PlayerState, UserUuid);
            if (PlayerState == Movement.Shot)
                PInfo.SetAngle(ShotPoint.position);
        }
    }
    public void CameraTurn()
    {
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        CmFollowTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);
    }

    public void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CameraArm.Aim();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            CameraArm.AimOut();
        }
    }

    public void Dodge() // 플레이어 회피
    {
        if (!IsMyCharacter())
        {
            if (PlayerState == Movement.Dodge)
            {
                DodgeIn();
            }

        }
        else
        {
            if (JDown && IsDodge == false && MoveDir != Vector3.zero)
            {
                DodgeIn();
                PlayerState = Movement.Dodge;
            }
        }
    }
    public void DodgeIn()
    {
        IsDodge = true;
        PlayerSpeed = Config.playerSpeed * 2;
        Invoke("DodgeOut", 0.4f); // 회피중인 시간, 후에 원래대로 돌아가는 DodgeOut 실행
    }
    public void DodgeOut() // 플레이어 회피 동작 이후 원래상태로 복구
    {
        IsDodge = false;
        PlayerSpeed = Config.playerSpeed;
    }
    // 플레이어 스턴
    public void Stun(float time) // 구멍함정은 타임을 3초로 줄 것
    {
        StartCoroutine("StunForSec", time);
    }
    public IEnumerator StunForSec(float time)
    {
        IsStun = true;
        yield return new WaitForSeconds(time);
        IsStun = false;

    }

    /*@@@ 충돌해결, 콜리전 처리 등 @@@*/
    // 물리 충돌 해결 -> FixedUpdate
    // 충돌로 인한 회전력 발생 무력화
    public void FreezeRotation()
    {
        Rigid.angularVelocity = Vector3.zero;
    }

    public void StopToWall()
    {
        float raydis = 1.5f;
        Debug.DrawRay(transform.position, transform.forward * raydis, Color.green);
        IsBorder = Physics.Raycast(transform.position, transform.forward, raydis, LayerMask.GetMask("Wall"));
    }
    public void IsClear(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            NearObject = other.gameObject;
            if (HaveItem == true)
            {
            }
        }
    }

    public void GetItem()
    {
        if (EDown && NearObject != null)
            if (NearObject.CompareTag("Item"))
            {
                Debug.Log("IsItem");
                NearObject = null;
                HaveItem = true;
                APIController.SendController("GetItem");
            }
    }
    // 플레이어의 HP를 프레임별로 확인
    private bool isSendDeath = false;
    public void CheckHP()
    {
        if (PlayerHealth <= 0 && !isSendDeath)
        {
            APIController.SendController("Death");
            isSendDeath = true;
        }
    }
    public void CheckDeath()
    {
        if (IsDead)
        {
            Stun(4f);
            this.gameObject.layer = default; // 보스가 인식 못함
            Anim.Play("dead");
            Destroy(this.gameObject, 4f); // 4초 뒤 플레이어 오브젝트 제거
        }
    }

    public IEnumerator OnAttacked(Vector3 reactVec)
    {
        IsBeatable = false;
        Debug.Log("플레이어가 공격받음");
        Debug.Log(PlayerHealth);
        SyncHp();
        StartCoroutine(Blink(5));
        PlayerSound.HitSound();
        KnockBack(reactVec, 8f);

        yield return new WaitForSeconds(2f);
        IsBeatable = true;
    }
    public void SyncHp()
    {
        UserScore.PlayerHeart(GetComponent<Player>());
    }


    // 공격을 당하면 플레이어 메테리얼을 깜빡거리게 함
    public IEnumerator Blink(int count)
    {
        if (PlayerHealth > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Mat.color = Color.red;
                yield return new WaitForSeconds(0.1f);
                Mat.color = Color.white;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            Mat.color = Color.gray;
        }
    }
    public void KnockBack(Vector3 reactVec, float force)
    {
        reactVec += Vector3.up;
        Rigid.AddForce(reactVec * force, ForceMode.Impulse);
    }
    public void NearbyObject(Collider other)
    {
        Debug.Log(other.gameObject.name);
        NearObject = other.gameObject;
    }

    public void MoveAnyFromObject(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (NearObject == other.gameObject)
            NearObject = null;
    }
    public void Attack()
    {
        if (!IsMyCharacter())
        {
            if (PlayerState == Movement.Shot)
            {
                AttackEvent();
            }
            return;
        }

        if (MouseClickInput && !IsAttack && !IsDodge)
        {
            AttackEvent();
            PlayerState = Movement.Shot;
        }
    }
    public void AttackEvent()
    {
        // SoundManager.instance.IsPlaySound("Attack");
        transform.LookAt(ShotPoint);
        IsAttack = true;
        Pistol.Shot();
    }
    public void AnimationStart()
    {
        if ((int)PlayerState == (int)Movement.Run)
        {
            Anim.SetBool(System.Enum.GetName(typeof(Movement), PlayerState), IsMove && !IsAttack);
            return;
        }
        if (System.Enum.IsDefined(typeof(Movement), PlayerState))
            Anim.SetTrigger(System.Enum.GetName(typeof(Movement), PlayerState));
        else
        {
            Anim.SetBool(System.Enum.GetName(typeof(Movement), PlayerState), false);
        }
        PlayerState = Movement.Idle;
        if(!IsMyCharacter())
        {
            PInfo.movement=0;
        }
    }
}
