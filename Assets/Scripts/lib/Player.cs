using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;
using project;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Player : MonoBehaviour
{

    // 기본 움직임(w,a,s,d, spacebar)
    float hAxis; 
    float vAxis;
    Vector3 moveVec, moveDir;
    Vector2 moveInput;
    
    bool jDown; // sacebar키 입력 여부

    // 움직임 상태, 플레이어 상태
    string player_state; // 플레이어 이벤트, 상태(run, dodge, ...)
    public float player_speed = config.player_speed; 
    public float player_health = config.player_health;
    bool isMove;
    bool isDodge; // 회피동작 상태 여부
    bool isStun = false;
    bool isDead = false; 

    // 상호작용
    bool eDown; // E키 입력 여부
    bool getItem = false; // 클리어 아이템 획득 표시
    GameObject nearObject; // 아이템 습득로직을 위한 오브젝트 정의

    // 애니메이션
    Animator anim;

    // 물리효과
    Rigidbody rigid;
    bool isBorder;

    // 캐릭터 시점
    public Transform cameraArm;
    Vector2 mouseDelta;

    /*@@@ 서버 @@@*/
    string userID;
    PlayerInfo pInfo = new PlayerInfo();
    


    
    void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        /* 서버 연결 */
        // AsynchronousClient.Connected();   

        // 서버로부터 uuid 받아옴
        /* string response = AsynchronousClient.Connected();
        JObject responseJson = JObject.Parse(response);
        userID = responseJson["data"].Value<string>("uuid"); */

    }

    private void Start()
    {
        

        
    }


    // 에티터 플레이버튼, 앱의 종료 -> 생명주기 종료
    private void OnApplicationQuit()
    {
        /* 서버 연결 해제 */
        // AsynchronousClient.ConnectedExit();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        GetItem();
        Dodge();
        PlayerStateUpdate();


        CameraTurn();
        
    }

    private void FixedUpdate() // default : 50fps
    {
        /* 서버 전송 */
        // CharacterInfo 에 현재 플레이어의 상태 입력
        // CharacterInfo 를 서버로 전송
        pInfo.UpdateInfo(transform.position, moveDir, player_state, userID); 
        string jsonData = pInfo.ObjectToJson(pInfo);
        // AsynchronousClient.Send(jsonData);


        FreezeRotation();
        StopToWall();
    }

    void GetInput()
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");
        eDown = Input.GetKeyDown(KeyCode.E); //E키를 통한 아이템 습득
        jDown = Input.GetButtonDown("Jump"); // GetButtonDown : (일회성) 점프, 회피    GetButton : (차지) 모으기
        mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); // 마우스를 통해 플레이어 화면 움직임
        moveInput = new Vector2(hAxis, vAxis).normalized; // TPS 움직임용 vector

        
    }

    // 플레이어 상태를 프레임마다 업데이트(네트워크 애니메이션 연계 용도)
    void PlayerStateUpdate()
    {
        // 애니메이션 : run, dodge 의 bool값 확인후 true 가 되면 "상태" 전송
        if (isMove == true)
            player_state = "Move";
        if (isDodge == true)
            player_state = "Dodge";
    }

    void Move()
    {
        moveVec = new Vector3(moveInput.x, 0f, moveInput.y).normalized; // Dodge 방향용 vector

        if (isStun == false )
        {
            isMove = true;
            Debug.DrawRay(cameraArm.position, cameraArm.forward, Color.red);
            
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            // 마우스로 바라보고 있는 벡터를 방향벡터로 바꿈
            moveDir = (lookForward * moveInput.y + lookRight * moveInput.x).normalized;

            transform.forward = lookForward; // 마우스가 바라보는 방향을 캐릭터가 바라보도록 함
            transform.position += moveDir * Time.deltaTime * player_speed;
        }
        
        if (isStun == true)
        {
            moveDir *= 0.1f;
        }

        if (isBorder == true)
        {
            Debug.Log("벽 충돌!!");
            // 벽앞에서 멈추는거 필요, moveVec 을 0으로 하는건 안됨
        }

        // run 애니메이션
        anim.SetBool("isRun", moveInput != Vector2.zero); // 움직이는 상태 -> isRun 애니메이션 실행

        if (moveInput == Vector2.zero)
        {
            isMove = false;
        }

    }

    void Turn()
    {
        transform.LookAt(transform.position + moveDir);
    }

    // 마우스 이동에 의한 카메라 각도 제한
    void CameraTurn()
    {
        // transform 의 z축을(z : 앞뒤, x : 좌우, y : 상하) vector 가 생기는 방향쪽으로 바라보게 함
        // transform.LookAt(transform.position + moveVec);

        
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);

    }


    void GetItem() // 아이템 획득을 위한 로직
    {
        if (eDown && nearObject != null)
        { //아이템을 먹었을 때 실행하는 문장
            if (nearObject.CompareTag("Item"))
            {
                getItem = true;
            }
        }
    }

    void Dodge() // 플레이어 회피
    {
        if (jDown && isDodge == false && moveDir!= Vector3.zero)
        {
            isDodge = true;
            player_speed *= 2;

            anim.SetTrigger("doDodge");

            Invoke("DodgeOut", 0.4f); // 회피중인 시간, 후에 원래대로 돌아가는 DodgeOut 실행 
        }
    }
    void DodgeOut() // 플레이어 회피 동작 이후 원래상태로 복구
    {
        isDodge = false;
        player_speed *= 0.5f;
    }

    // 플레이어 스턴
    public void Stun(float time) // 구멍함정은 타임을 3초로 줄 것
    {
        StartCoroutine("StunForSec", time);
    }
    IEnumerator StunForSec(float time)
    {
        isStun = true;
        yield return new WaitForSeconds(time);
        isStun = false;

    }

    // 플레이어 죽음
    void Dead()
    {
        // isDead == true 일때
        // 
    }


    /*@@@ 충돌해결, 콜리전 처리 등 @@@*/
    // 물리 충돌 해결 -> FixedUpdate
    // 충돌로 인한 회전력 발생 무력화
    void FreezeRotation()
    {
        rigid.angularVelocity = Vector3.zero;
    }
    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 1f, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 1f, LayerMask.GetMask("Wall"));
    }

    private void OnTriggerStay(Collider other) //플레이어 범위에 아이템이 인식할 수 있는지 확인
    {
        if (other.CompareTag("Item"))
        {
            nearObject = other.gameObject;
            if (getItem == true)
            {
                SceneManager.LoadScene("First Map"); //getItem이 true일경우 다음 맵으로 이동
                Destroy(nearObject); // 이동과 동시에 아이템 오브젝트가 사라짐
            }
        }

    }
    private void OnTriggerExit(Collider other) //플레이어 범위에 아이템이 벗어났는지 확인
    {
        if (other.CompareTag("Item"))
        {
            nearObject = null;
        }
    }
}
