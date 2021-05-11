using System.Security.AccessControl;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Communication;
using Communication.JsonFormat;
using Communication.MainServer;
// 1. 사용자의 ID와 PW를 입력받고
// 2. 값이 유효한지 확인후 다음화면으로 넘어간다

public class Login : BaseMainMenu, IMainMenu
{
    // ID와 PW 가 유효한지 확인후 nextScreen 활성화 하기 위함
    private InputField idInput = null;  // SerializeField : private 변수를 인스펙터에서 다룰 수 있게함
    private InputField pwdInput = null;

    // 서버 통신용
    protected override void Awake()
    {
        base.Awake();
        SetUp();
    }
    public void OnEnable()
    {
        
        UINum = 2;
        nextUINum = 4;
    }
    
    protected override void OnApplicationQuit(){}
    public void SetUp()
    {
        // Initialize variable
        var Menu = this.transform.Find("Menu");

        // Set GUIObject
        idInput = this.transform.Find("ID").Find("InputField ID").GetComponent<InputField>();
        pwdInput = this.transform.Find("Password").Find("InputField Password").GetComponent<InputField>();

        // Set Button Event
        Menu.Find("Button SignIn").gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnClickLogin(); }); // 로비, 방만들기 선택창 이동
        Menu.Find("Button SignUp").gameObject.GetComponent<Button>().onClick.AddListener(delegate { NextUI(); });       // 회원가입 창 이동
        this.transform.Find("Button Back").gameObject.GetComponent<Button>().onClick.AddListener(delegate { BackUI(); });// 게임 시작, 종료 화면 이동
    }
    public void OnClickLogin()
    {
        // ID PW 입력 확인
        var id = idInput.text;
        var pw = pwdInput.text;
        JObject json = null;
        // 서버에 로그인 요청

        if (id.Equals("") || pw.Equals(""))
        {
            SetwarningText("입력 값을 확인해 주세요");
            return;
        }
        // TODO : id, pw, 정규식 필요

        // 서버에 전송
        string response = "";

        SignInInfo info = new SignInInfo();
        info.UpdateInfo(id, pw);
        response = MServer.SignIn(info);

        json = JObject.Parse(response);
        serverMsg = json["data"]["isSuccess"].ToString();

        if (!serverMsg.Equals("True"))
        {
            SetwarningText(json["data"]["errorMsg"].ToString());
            return;
        }
        //TODO: Test용으로 추후 jwt로 변경해야함
        // var temp = Lib.Common.GetData(response);
        var temp = new JObject();
        temp["nickname"] = json["data"]["nickname"].ToString();

        NetworkInfo.myData = new MemberInfo(json["data"]["userUuid"].ToString(), temp["nickname"].ToString());
        SetwarningText("로그인에 성공하였습니다 잠시 기다려 주세요");
        SelectUI(nextUINum);
    }


    protected override void NextUI()
    {
        ClearInputField();
        base.NextUI();
    }

    protected override void SelectUI(int UINum)
    {
        ClearInputField();
        base.SelectUI(UINum);
    }
    private void ClearInputField()
    {
        idInput.text = "";
        pwdInput.text = "";
    }
}
