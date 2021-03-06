using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.IO;


// Signup : 회원가입

public class Signup : MonoBehaviour
{
    [SerializeField] private Text warning = null;
    [SerializeField] private Button idCheck = null;

    [SerializeField] private InputField nameInput = null;
    [SerializeField] private InputField idInput = null;
    [SerializeField] private InputField pwInput = null;
    [SerializeField] private InputField mobileInput = null;

    [SerializeField] private Button signup = null;
    [SerializeField] private GameObject nextScreen; // 로그인 화면

    private bool idValid = false;


    // 서버 통신용
    private string serverMsg = "";

    private void Start()
    {
        

        
    }

    // 아이디 중복확인 클릭
    public void OnClickIdCheck()
    {

        string id = idInput.text;

        if (id.Equals(""))
        {
            warning.text = "아이디를 입력해 주세요";
        }
        else
        {
            idValid = true; // 유효한 아이디를 입력했음을 표시
            warning.text = "유효한 아이디 입니다";
        }
    }
    
    // 회원가입 클릭
    public void OnClickRegister()
    {
        string name = nameInput.text;
        string id = idInput.text;
        string pw = pwInput.text;
        string mobile = mobileInput.text;
        string response;

        // 인풋값 확인(id 중복 확인 했으면 idValid = True)
        if (name.Equals("") || id.Equals("") || pw.Equals("") || mobile.Equals("") || !idValid)
        {
            warning.text = "입력값을 확인해 주세요";
            if (idValid == false)
                warning.text = "아이디 중복확인을 해주세요";
        }
        else
        {

            // 입력받은 값을 JSON 형태로
            signUpInfo info = new signUpInfo();
            info.UpdateInfo(id, pw, mobile, name);

            // json 형태로 서버에 전송
            string signupUri = "api/v1/userSession/signUp";
            response = MServer.Communicate(signupUri, "POST", info);
            JObject json = JObject.Parse(response);

            // 서버로 부터 받은 메세지 
            serverMsg = json["data"]["isSuccess"].ToString();

        }

        if (serverMsg.Equals("True"))
        {
            warning.text = "회원가입에 성공하셨습니다";
            Invoke("GoNextScreen", 2.5f);
        }

    }
    private void GoNextScreen()
    {
        // 입력값 지우고 다음화면으로
        nameInput.text = "";
        idInput.text = "";
        pwInput.text = "";
        mobileInput.text = "";
        warning.text = "";

        // 로그인 화면으로
        this.gameObject.SetActive(false);
        nextScreen.SetActive(true);
    }
    
}
