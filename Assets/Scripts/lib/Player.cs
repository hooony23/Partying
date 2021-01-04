using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Player : MonoBehaviour
{

    // 기본 움직임
    float hAxis; 
    float vAxis;
    Vector3 moveVec;
    public float speed;
    bool jDown; // sacebar키 입력 여부

    // 움직임 상태
    bool isDodge; // 회피동작 상태 여부

    // 상호작용
    bool eDown; // E키 입력 여부
    bool getItem = false; // 클리어 아이템 획득 표시
    GameObject nearObject; // 아이템 습득로직을 위한 오브젝트 정의

    // 애니메이션
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        GetItem();
        Dodge();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        eDown = Input.GetKeyDown(KeyCode.E); //E키를 통한 아이템 습득
        jDown = Input.GetButtonDown("Jump"); // GetButtonDown : (일회성) 점프, 회피    GetButton : (차지) 모으기
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * speed * Time.deltaTime;

        // 애니메이션
        anim.SetBool("isRun", moveVec != Vector3.zero); // 움직이는 상태 -> isRun 애니메이션 실행
    }

    void Turn()
    {
        // transform 의 z축을(z : 앞뒤, x : 좌우, y : 상하) vector 가 생기는 방향쪽으로 바라보게 함
        transform.LookAt(transform.position + moveVec);
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
        if (jDown && isDodge == false && moveVec != Vector3.zero)
        {
            isDodge = true;
            speed *= 2;
            anim.SetTrigger("doDodge");

            Invoke("DodgeOut", 0.4f); // 회피중인 시간 0.4초, 0.4초후에 원래대로 돌아가는 DodgeOut 실행
        }
    }

    
    void DodgeOut() // 플레이어 회피 동작 이후 원래상태로 복구
    {
        isDodge = false;
        speed *= 0.5f;
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
