using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{

    // 기본 움직임(w,a,s,d, spacebar)
    float hAxis; 
    float vAxis;
    Vector3 moveVec, moveDir;
    
    bool jDown; // sacebar키 입력 여부

    // 움직임 상태
    public float player_speed; // 플레이어 스피드
    bool isDodge; // 회피동작 상태 여부
    bool isStun = false;  // 플레이어 스턴 상태 여부

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

    //TODO
    // 0. 플레이어 시점, 회피 움직임
    /* 서버와 상호작용 */
    /* 
     * 1. 클라이언트 - > 서버 : 최초 상황 동기화, 플레이어 입력 전송(움직임, 회피 등)
     * 2. 서버 -> 클라이언트 : 서버에서 보내주는 패킷을 통해 클라이언트에서 처리(움직임, 회피, 등)
     */


    // Start is called before the first frame update
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        /* 서버 전송 */
        

        Move();
        Turn();
        GetItem();
        Dodge();

        CameraTurn();
        
        
    }

    private void FixedUpdate()
    {
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
    }

    void Move()
    {
        Vector2 moveInput = new Vector2(hAxis, vAxis).normalized; // TPS 움직임용 vector
        Vector3 moveVec = new Vector3(hAxis, 0f, vAxis).normalized; // Dodge 방향용 vector(필요없음)

        if (isStun == false && isDodge == false)
        {
            // transform.position += moveVec * speed * Time.deltaTime;
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;

            transform.forward = lookForward;
            transform.position += moveDir * Time.deltaTime * player_speed;
        }
        else if(isStun == false && isDodge == true)
        {

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

    }

    void Turn()
    {
        transform.LookAt(transform.position + moveDir);
    }

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
            if (nearObject.tag == ("Item"))
            {
                getItem = true;
            }
        }
    }

    void Dodge() // 플레이어 회피
    {
        if (jDown && isDodge == false && moveDir != Vector3.zero)
        {

            /* 수정 필요
             */
            isDodge = true;
            player_speed *= 2;

            anim.SetTrigger("doDodge");

            Invoke("DodgeOut", 1f); // 회피중인 시간, 후에 원래대로 돌아가는 DodgeOut 실행 
        }
    }
    void DodgeOut() // 플레이어 회피 동작 이후 원래상태로 복구
    {
        isDodge = false;
        player_speed *= 0.5f;
    }

    // 플레이어 스턴 기능
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
