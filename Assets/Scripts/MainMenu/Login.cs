using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
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
    public void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        // Initialize variable
        UINum = 2;
        nextUINum = 4;
        var Menu = this.transform.Find("Menu");
        
        // Set GUIObject
        idInput = this.transform.Find("ID").Find("InputField ID").GetComponent<InputField>();
        pwdInput = this.transform.Find("Password").Find("InputField Password").GetComponent<InputField>();
        
        // Set Button Event
        Menu.Find("Button SignIn").gameObject.GetComponent<Button>().onClick.AddListener(delegate {SelectUI(nextUINum);});
        Menu.Find("Button SignUp").gameObject.GetComponent<Button>().onClick.AddListener(delegate {NextUI();});
        this.transform.Find("Button Back").gameObject.GetComponent<Button>().onClick.AddListener(delegate {BackUI();});
    }
    public void OnClickLogin()
    {
        // ID PW 입력 확인
        var id = idInput.text;
        var pw = pwdInput.text;
        JObject json = null;
        // 서버에 로그인 요청
        if (id != "" && pw != "") // TODO : id, pw, 정규식 필요
        {
            // MServer SignIn API,uri : api/v1/session/signIn ,method : POST
            // 서버에 전송
            string signInUri = "api/v1/session/signIn";
            string response = "";

            SignInInfo info = new SignInInfo();
            info.UpdateInfo(id, pw);
            var requestJson = BaseJsonFormat.ObjectToJson("signIn", "center_server", info);
            response = MServer.Communicate("POST", signInUri, requestJson);
            Debug.Log(response);
            json = JObject.Parse(response);
            serverMsg = json["data"]["isSuccess"].ToString();
            

        }
        else
        {
            SetwarningText("입력 값을 확인해 주세요");
        }

        if (serverMsg.Equals("True"))
        {
            SetwarningText("로그인에 성공하였습니다 잠시 기다려 주세요");
            Invoke("GoNextScreen", 2.5f);
        }
        if (serverMsg.Equals("False"))
        {
            SetwarningText(json["data"]["errorMsg"].ToString());
        }


    }


    // 화면 넘어가기
    protected override void NextUI()
    {
        idInput.text = "";
        pwdInput.text = "";
        base.NextUI();
    }
    protected override void SelectUI(int UINum)
    {
        idInput.text = "";
        pwdInput.text = "";
        base.SelectUI(UINum);
    }
}
