using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1. 사용자의 ID와 PW를 입력받고
// 2. 값이 유효한지 확인후 다음화면으로 넘어간다

public class Login : MonoBehaviour
{
    // ID와 PW 가 유효한지 확인후 nextScreen 활성화 하기 위함
    public GameObject nextScreen; // HostOrUserUI
    public GameObject warning;

    [SerializeField] private InputField idInput = null;  // SerializeField : private 변수를 인스펙터에서 다룰 수 있게함
    [SerializeField] private InputField pwInput = null;
    private string id = "";
    private string pw = "";

    public void OnClickLogin()
    {
        // ID PW 입력 확인
        id = idInput.text;
        pw = pwInput.text;
        Debug.Log("ID :" + id);
        Debug.Log("PW :" + pw);

        // 입력이 정상적이면 다음화면 (회원인지 확인과정 추가 필요)
        if (id != "" && pw != "")
        {
            // 서버 세션에 사용자 정보 보냄(id, pw ...등)


            // 화면 넘어가기
            this.gameObject.SetActive(false);
            nextScreen.SetActive(true);
        }
        else
        {
            Debug.Log("입력하신 정보가 올바르지 않습니다");
            warning.SetActive(true);
        }
    }
}
