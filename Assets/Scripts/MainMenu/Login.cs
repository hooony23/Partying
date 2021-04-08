using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Communication.JsonFormat;
using Communication.MainServer;
// 1. 사용자의 ID와 PW를 입력받고
// 2. 값이 유효한지 확인후 다음화면으로 넘어간다

public class Login : BaseMainMenu
{
    // ID와 PW 가 유효한지 확인후 nextScreen 활성화 하기 위함
    [SerializeField] private GameObject nextScreen; // HostOrUserUI
    [SerializeField] private InputField idInput = null;  // SerializeField : private 변수를 인스펙터에서 다룰 수 있게함
    [SerializeField] private InputField pwInput = null;
    private string id = "";
    private string pw = "";

    // 서버 통신용

    public void OnClickLogin()
    {
        // ID PW 입력 확인
        id = idInput.text;
        pw = pwInput.text;
        UINum = 1;
        nextUINum = 3;
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
    private void GoNextScreen()
    {
        // 화면 초기화
        idInput.text = "";
        pwInput.text = "";
        ResetMessage();

        // 다음 화면으로
        this.gameObject.SetActive(false);
        nextScreen.SetActive(true);
    }
}
