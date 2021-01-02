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

    bool eDown; // E키를 누르는 것을 표시
    bool getItem = false; // 클리어 아이템 획득 표시
    GameObject nearObject; // 아이템 습득로직을 위한 오브젝트 정의

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        GetItem();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        eDown = Input.GetKeyDown(KeyCode.E); //E키를 통한 아이템 습득
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moveVec * speed * Time.deltaTime;
    }

    void Turn()
    {
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
    private void OnTriggerStay(Collider other) //플레이어 범위에 아이템이 인식할 수 있는지 확인
    {
        if (other.CompareTag("Item"))
        {
            nearObject = other.gameObject;
            if (getItem == true)
            {
                SceneManager.LoadScene("First Map"); //getItem이 true일경우 다음 맵으로 이동
                Destroy(nearObject);// 이동과 동시에 아이템 오브젝트가 사라짐
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
