using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using project;


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
    [SerializeField] private GameObject nextScreen;

    private bool idValid = false;


    //서버 테스트
    string userID = null;

    private void Start()
    {
        

        
    }

    private void OnApplicationQuit()
    {
        AsynchronousClient.ConnectedExit();
    }

    // 아이디 중복확인 클릭
    public void OnClickIdCheck()
    {
        // 서버 테스트
        Debug.Log("네트워킹 테스트"+userID);

        string id = idInput.text;

        if (id.Equals(""))
        {
            warning.text = "아이디를 입력해 주세요";
        }

        idValid = true;
        
    }

    // 회원가입 클릭
    public void OnClickRegister()
    {
        string name = nameInput.text;
        string id = idInput.text;
        string pw = pwInput.text;
        string mobile = mobileInput.text;

        // json 형태로 서버 전송
        SignUpInfo sInfo = new SignUpInfo();
        string jsonData = sInfo.ObjectToJson(sInfo);
        Debug.Log(jsonData);
        AsynchronousClient.Send(jsonData);
        // 서버에서 받아온 Json 출력
        // 다음화면으로 넘어감

        // 서버로부터 uuid 받아옴
        /* string response = AsynchronousClient.Connected();
        JObject responseJson = JObject.Parse(response);
        userID = responseJson["data"].Value<string>("uuid"); */
    }

}
