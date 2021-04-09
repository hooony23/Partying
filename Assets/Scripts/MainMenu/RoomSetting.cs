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
    private string Title {get;set;}= "";
    private string password = "";


    private void Start()
    {
        SetUp();
    }
    public void SetUp()
    {
        // Initialize Variable
        UINum = 5;
        nextUINum = 7;
        
        // Set GUI Object 
        roomTitleInput = this.transform.Find("Name").Find("InputField RoomTitle").gameObject.GetComponent<InputField>();
        roomPasswordInput = this.transform.Find("Password").Find("InputField Password").gameObject.GetComponent<InputField>();
        
        // Set Button Event
        this.transform.Find("Button Back").gameObject.GetComponent<Button>().onClick.AddListener(delegate {BackUI();});
        this.transform.Find("Button Create").gameObject.GetComponent<Button>().onClick.AddListener(delegate {SelectUI(nextUINum);});
    }

    public void onClickCreate()
    {
        // 방제목(입력필수), 암호 확인 , roomInfo 클래스 객체 이용하여, Lobby의 rooms배열에 추가
        // 방제목 미입력 시 생성하기 못함, 경고 활성화

        Title = roomTitleInput.text;
        password = roomPasswordInput.text;

        if (!Title.Equals(""))
        {
            SetwarningText("방을 생성하였습니다");

            // [createRoom API] uri : api/v1/rooms/createRoom , method : POST
            string createRoomUri = "api/v1/rooms/createRoom";
            string response = "";

            CreateRoomInfo info = new CreateRoomInfo();
            info.UpdateInfo(Title, password);
            if(NetworkInfo.connectionId.Equals(""))
                throw new Exception("not found connectionId");
            info.ConnectionId = NetworkInfo.connectionId;
            var requestJson = Communication.JsonFormat.BaseJsonFormat.ObjectToJson("creatRoom", "center_server", info);

            response = MServer.Communicate("POST", createRoomUri, requestJson);
            SetwarningText(response);
            JObject json = JObject.Parse(response);
            serverMsg = json["data"]["isSuccess"].ToString();

            if (serverMsg.Equals("True"))
            {
                // 방 진입
                // 방의 Uuid를 생성하는 과정이 필요
                Room.roomName = Title;
                NetworkInfo.memberInfo = json["data"]["memberInfo"] as JArray;
                Room.roomMemberCount = "1";
                SetwarningText(Room.roomName);
                SelectUI(nextUINum);
            }

        }
        else
        {
            SetwarningText("방 제목을 입력해주세요");
        }
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
