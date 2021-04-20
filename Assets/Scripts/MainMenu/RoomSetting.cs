using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Communication;
using Communication.MainServer;
using Communication.JsonFormat;
// 방만들기

public class RoomSetting : BaseMainMenu, IMainMenu
{

    private InputField roomTitleInput = null;
    private InputField roomPasswordInput = null;
    private string Title = "";
    private string password = "";


    protected override void Awake()
    {
        base.Awake();
        SetUp();
    }
    private void OnEnable() 
    {
        UINum = 5;
        nextUINum = 7;    
    }
    public void SetUp()
    {
        // Set GUI Object 
        roomTitleInput = this.transform.Find("Name").Find("InputField RoomTitle").gameObject.GetComponent<InputField>();
        roomPasswordInput = this.transform.Find("Password").Find("InputField Password").gameObject.GetComponent<InputField>();

        // Set Button Event
        this.transform.Find("Button Back").gameObject.GetComponent<Button>().onClick.AddListener(delegate { BackUI(); });
        this.transform.Find("Button Create").gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnClickCreate(); });
    }

    public void OnClickCreate()
    {
        // 방제목(입력필수), 암호 확인 , roomInfo 클래스 객체 이용하여, Lobby의 rooms배열에 추가
        // 방제목 미입력 시 생성하기 못함, 경고 활성화

        Title = roomTitleInput.text;
        password = roomPasswordInput.text;

        if (Title.Equals(""))
        {
            SetwarningText("방 제목을 입력해주세요");
            return;
        }
        SetwarningText("방을 생성하였습니다");

        // [createRoom API] uri : api/v1/rooms/createRoom , method : POST
        string response = "";

        CreateRoomInfo info = new CreateRoomInfo();
        info.UpdateInfo(Title, password);
        if (NetworkInfo.connectionId.Equals(""))
            throw new Exception("not found connectionId");
        info.ConnectionId = NetworkInfo.connectionId;
        response = MServer.CreateRoom(info);
        JObject json = JObject.Parse(response);
        serverMsg = json["data"]["isSuccess"].ToString();

        if (!serverMsg.Equals("True"))
            throw new Exception("서버와 통신중 장애가 발생 했습니다.");
        // 방 진입
        NetworkInfo.roomInfo = ((JObject)json["data"]["roomInfo"]).ToObject<RoomInfo>();
        NetworkInfo.memberInfo = json["data"]["memberInfo"] as JArray;
        SelectUI(nextUINum);


    }

    protected override void SelectUI(int selectUINum)
    {
        // 입력값 초기화
        roomTitleInput.text = "";
        roomPasswordInput.text = "";

        // 다음화면으로
        base.SelectUI(selectUINum);
    }
}
