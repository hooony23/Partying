using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Communication.MainServer;
// Signup : 회원가입

public class Signup : BaseMainMenu, IMainMenu
{
    private Button idCheck = null;

    private InputField nameInput = null;
    private InputField nickNameInput = null;
    private InputField pwInput = null;
    private InputField idInput = null;

    private Button signup = null;

    private bool idValid = false;


    // 서버 통신용

    protected override void Awake()
    {
        base.Awake();
        SetUp();
    }
    private void OnEnable()
    {
        UINum = 3;
    }
    public void SetUp()
    {
        // Initialize Variable
        var id = this.transform.Find("ID");
        var password = this.transform.Find("Password");
        var name = this.transform.Find("Name");
        var nickname = this.transform.Find("NickName");

        // Set GUI Object
        idInput = id.Find("InputField ID").gameObject.GetComponent<InputField>();
        pwInput = password.Find("InputField Password").gameObject.GetComponent<InputField>();
        nameInput = name.Find("InputField Name").gameObject.GetComponent<InputField>();
        nickNameInput = nickname.Find("InputField NickName").gameObject.GetComponent<InputField>();
        idCheck = id.Find("Button IDCheck").gameObject.GetComponent<Button>();
        signup = this.transform.Find("Button SignUp").gameObject.GetComponent<Button>();
        

        // Set Button Event
        idCheck.onClick.AddListener(delegate {OnClickIdCheck();});
        signup.onClick.AddListener(delegate {OnClickSignUp();});
        this.transform.Find("Button Back").gameObject.GetComponent<Button>().onClick.AddListener(delegate {BackUI();});
    }
    // 아이디 중복확인 클릭
    public void OnClickIdCheck()
    {
        //TODO: idCheck api를 만들어서, 포커싱이 풀리면 자동으로 idcheck하도록 할 것.
        string id = nickNameInput.text;

        if (id.Equals(""))
        {
            SetwarningText("아이디를 입력해 주세요");
        }
        else
        {
            idValid = true; // 유효한 아이디를 입력했음을 표시
            SetwarningText("유효한 아이디 입니다");
        }
    }

    // 회원가입 클릭
    public void OnClickSignUp()
    {
        string name = nameInput.text;
        string id = nickNameInput.text;
        string pw = pwInput.text;
        string mobile = idInput.text;
        string response;

        // 인풋값 확인(id 중복 확인 했으면 idValid = True)
        if (name.Equals("") || id.Equals("") || pw.Equals("") || mobile.Equals("") || !idValid)
        {
            SetwarningText("입력값을 확인해 주세요");
            if (idValid == false)
                SetwarningText("아이디 중복확인을 해주세요");
        }
        else
        {

            // 입력받은 값을 JSON 형태로
            SignUpInfo info = new SignUpInfo();
            info.UpdateInfo(id, pw, mobile, name);

            // json 형태로 서버에 전송
            response = MServer.SignUp(info);
            JObject json = JObject.Parse(response);

            //TODO: Test용으로 추후 jwt로 변경해야함
            // var temp = Lib.Common.GetData(response);
            var temp = new JObject();
            temp["nickname"] = "";
            Communication.NetworkInfo.myData = new MemberInfo(Util.Config.userUuid,temp["nickname"].ToString());
            // 서버로 부터 받은 메세지 
            serverMsg = json["data"]["isSuccess"].ToString();
        }

        if (serverMsg.Equals("True"))
        {
            SetwarningText("회원가입에 성공하셨습니다");
            Invoke("NextUI",2f);
        }

    }
    protected override void NextUI()
    {
        nameInput.text = "";
        nickNameInput.text = "";
        pwInput.text = "";
        idInput.text = "";
        ResetMessage();
        base.NextUI();
    }
    protected override void OnApplicationQuit(){}

}
