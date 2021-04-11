using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Communication;
using Communication.API;
using Communication.JsonFormat;
using Util;
public class PlayerUtil : PlayerController
{
    [Range(0.01f, 10)] public float mouseSensitivity = 1;
    [SerializeField]
    private string BGMSound;
    public void GetInput()
    {
        if (!IsDead)
        {
            HAxis = 0f;
            VAxis = 0f;
            foreach (var key in GetInputKeys())
            {
                InputEvent(key);
            }
            MouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 마우스를 통해 플레이어 화면 움직임
            MoveInput = new Vector2(HAxis, VAxis).normalized; // TPS 움직임용 vector
        }
    }
    public void GetNetWorkInput()
    {
        if (NetworkInfo.playersInfo.ContainsKey(this.gameObject.name) && !IsDead)
            // if (NetworkInfo.playersInfo[this.gameObject.name].loc.X != PInfo.loc.X||
            //     NetworkInfo.playersInfo[this.gameObject.name].loc.Y != PInfo.loc.Y||
            //     NetworkInfo.playersInfo[this.gameObject.name].loc.Z != PInfo.loc.Z)
            if (NetworkInfo.playersInfo[this.gameObject.name] != PInfo)
            {
                PInfo = NetworkInfo.playersInfo[this.gameObject.name];
                this.gameObject.transform.position = new Vector3(PInfo.loc.X, PInfo.loc.Y, PInfo.loc.Z);
            }
        MoveDir = new Vector3(PInfo.vec.X, PInfo.vec.Y, PInfo.vec.Z);
    }
    public void InputEvent(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.A:
                HAxis = -1f;
                break;
            case KeyCode.D:
                HAxis = 1f;
                break;
            case KeyCode.W:
                VAxis = 1f;
                break;
            case KeyCode.S:
                VAxis = -1f;
                break;
            case KeyCode.E:
                EDown = Input.GetKeyDown(key); //E키를 통한 아이템 습득
                break;
            case KeyCode.Space:
                JDown = Input.GetKeyDown(key); // GetButtonDown : (일회성) 점프, 회피    GetButton : (차지) 모으기
                break;
        }
    }
    public void MoveChangeSend()
    {
        //TODO 만약 플레이어가 하나의 키를 누른채로 마우스만 이동시키며 
        //     움직일 경우 동기화 불가능함 추후 수정필요
        if (IsKeyInput() && !IsDead)
        {
            APIController.SendController("Move", PInfo.ObjectToJson());
        }
    }
    public bool IsKeyInput()
    {
        var inputData = from input in Config.InputKey.GetValues(typeof(Config.InputKey))
                        .Cast<Config.InputKey>()
                        .Select(k => (KeyCode)k)
                        .ToList()
                        where Input.GetKeyDown(input) == true || Input.GetKeyUp(input)
                        select input;
        if (inputData.Count() <= 0)
            return false;
        return true;
    }
    public IEnumerable<KeyCode> GetInputKeys()
    {
        var inputData = from input in Config.InputKey.GetValues(typeof(Config.InputKey))
                        .Cast<Config.InputKey>()
                        .Select(k => (KeyCode)k)
                        .ToList()
                        where Input.GetKey(input) == true
                        select input;
        return inputData;
    }
    // 플레이어 상태를 프레임마다 업데이트(네트워크 애니메이션 연계 용도)
    public void PlayerStateUpdate()
    {
        // 애니메이션 : run, dodge 의 bool값 확인후 true 가 되면 "상태" 전송
        if (IsMove == true)
            PlayerState = "Move";
        else if (IsDodge == true)
            PlayerState = "Dodge";
    }
    public void Move()
    {
        MoveVec = new Vector3(MoveInput.x, 0f, MoveInput.y).normalized; // Dodge 방향용 vector

        if (IsStun == false)
        {
            IsMove = true;
            // 만약 현재 플레이어가 조정하고 있는 캐릭터라면 마우스가 바라보는 방향을 캐릭터가 바라보도록 함
            if (this.UserUuid.Equals(Config.userUuid))
            {
                Debug.DrawRay(CameraArm.position, CameraArm.forward, Color.red);

                Vector3 lookForward = new Vector3(CameraArm.forward.x, 0f, CameraArm.forward.z).normalized;
                Vector3 lookRight = new Vector3(CameraArm.right.x, 0f, CameraArm.right.z).normalized;
                transform.forward = lookForward;

                // 마우스로 바라보고 있는 벡터를 방향벡터로 바꿈
                MoveDir = (lookForward * MoveInput.y + lookRight * MoveInput.x).normalized;
            }
            if (!IsBorder)
            {
                this.gameObject.transform.position += MoveDir * Time.deltaTime * PlayerSpeed;
            }
        }

        if (IsStun == true)
        {
            MoveDir *= 0.1f;
        }

        // run 애니메이션
        Anim.SetBool("isRun", MoveDir != Vector3.zero); // 움직이는 상태 -> isRun 애니메이션 실행

        if (MoveInput == Vector2.zero)
        {
            IsMove = false;
        }
        if (this.UserUuid.Equals(Config.userUuid))
            PInfo = new PlayerInfo(this.transform.position, MoveDir, PlayerState, UserUuid);
    }
    public void Turn()
    {
        transform.LookAt(transform.position + MoveDir);
    }
    public void CameraTurn()
    {
        ///<summary>
        ///
        /// 마우스 이동에 의한 카메라 각도 제한
        /// 
        /// <summary>
        // transform 의 z축을(z : 앞뒤, x : 좌우, y : 상하) vector 가 생기는 방향쪽으로 바라보게 함
        // transform.LookAt(transform.position + moveVec);
        Vector3 camAngle = CameraArm.rotation.eulerAngles;
        float x = camAngle.x - MouseDelta.y * mouseSensitivity;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        CameraArm.transform.position = this.transform.position;
        CameraArm.rotation = Quaternion.Euler(x, camAngle.y + MouseDelta.x * mouseSensitivity, camAngle.z);

    }
    public void IsGetItem() // 아이템 획득을 위한 로직
    {
        if (EDown && NearObject != null)
        { //아이템을 먹었을 때 실행하는 문장
            if (NearObject.CompareTag("Item"))
            {
                GetItem = true;
            }
        }
    }
    public void Dodge() // 플레이어 회피
    {
        if (JDown && IsDodge == false && MoveDir != Vector3.zero)
        {
            IsDodge = true;
            PlayerSpeed *= 2;

            Anim.SetTrigger("doDodge");

            Invoke("DodgeOut", 0.4f); // 회피중인 시간, 후에 원래대로 돌아가는 DodgeOut 실행
        }
    }
    public void DodgeOut() // 플레이어 회피 동작 이후 원래상태로 복구
    {
        IsDodge = false;
        PlayerSpeed *= 0.5f;
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
            if (GetItem == true)
            {
                Destroy(NearObject, 7f); // 이동과 동시에 아이템 오브젝트가 사라짐
                Config.GameClear = true;
            }
        }
    }
    public void IsGetItem(Collider other)
    {

        if (other.CompareTag("Item"))
        {
            NearObject = null;
        }
    }
    // 플레이어의 HP를 프레임별로 확인
    public void CheckHP()
    {
        if (PlayerHealth <= 0)
        {
            IsDead = true;
            CheckDeath();
        }
    }
    public void CheckDeath()
    {
        if (IsDead)
        {
            this.gameObject.layer = default; // 보스가 인식 못함
            Anim.Play("dead");
            Destroy(this.gameObject, 4f); // 4초 뒤 플레이어 오브젝트 제거
        }
    }

    public IEnumerator OnAttacked(Vector3 reactVec)
    {
        Debug.Log("플레이어가 공격받음");

        if (IsBeatable == false)
        {
            IsBeatable = true;
            StartCoroutine(Blink(5));
            KnockBack(reactVec, 8f);
        }

        yield return new WaitForSeconds(2f);
        IsBeatable = false;

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

    /// 플레이어 피격 처리 ///
}
