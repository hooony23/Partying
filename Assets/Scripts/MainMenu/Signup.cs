using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Signup : MonoBehaviour
{
    [SerializeField] private Text warning = null;
    [SerializeField] private Button idCheck = null;
    [SerializeField] private InputField idInput = null;
    [SerializeField] private InputField pwInput = null;
    [SerializeField] private InputField eaInput = null; // email address
    [SerializeField] private Button signup = null;
    [SerializeField] private GameObject nextScreen = null;

    private bool idValid = false;

    private void Start()
    {
        
    }

    // 아이디 중복확인 클릭
    public void OnClickIdCheck()
    {
        // 데이터베이스에서 중복된 아이디 있는지 확인
        // ~~~~

        warning.color = Color.white;
        warning.text = "중복확인 완료";
        idValid = true;
    }

    // 회원가입 클릭
    public void OnClickRegister()
    {
        string id = idInput.text;
        string pw = pwInput.text;
        string email = eaInput.text;

        // 내용이 다 써져있지 않다면 warning
        if (id.Equals("") || pw.Equals("") || email.Equals(""))
        {
            warning.text = "입력을 확인해주세요";
            warning.color = Color.red;
        }
        else if (idValid == false)
        {
            warning.text = "아이디 중복확인을 해주세요";
            warning.color = Color.red;
        }
        // 내용이 다 써져있다면 register
        else
        {
            // 등록절차 ~~

            // 입력내용 초기화
            idInput.text = "";
            pwInput.text = "";
            eaInput.text = "";
            warning.text = "";
            warning.color = Color.black;
            
            // 다음화면으로
            this.gameObject.SetActive(false);
            nextScreen.SetActive(true);
        }
    }

}
