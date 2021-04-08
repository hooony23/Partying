using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using Communication;
using Communication.MainServer;
using Communication.JsonFormat;
// 방만들기

public class RoomSetting : BaseMainMenu
{
    public GameObject nextScreen; // RoomUI

    [SerializeField] private InputField roomTitleInput = null;  
    [SerializeField] private InputField roomPasswordInput = null;
    private string title = "";
    private string password = "";

    // 서버 통신용
    

    public string Title { get => title; set => title = value; }

    private void Start()
    {

    }

    public void onClickCreate()
    {
        // 방제목(입력필수), 암호 확인 , roomInfo 클래스 객체 이용하여, Lobby의 rooms배열에 추가
        // 방제목 미입력 시 생성하기 못함, 경고 활성화

        title = roomTitleInput.text;
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
                Room.roomName = title;
                NetworkInfo.memberInfo = json["data"]["memberInfo"] as JArray;
                Room.roomMemberCount = "1";
                SetwarningText(Room.roomName);
                GoNextScreen();
            }

        }
        else
        {
            SetwarningText("방 제목을 입력해주세요");
        }
    }

    private void GoNextScreen()
    {
        // 입력값 초기화
        roomTitleInput.text = "";
        roomPasswordInput.text = "";

        // 다음화면으로
        this.gameObject.SetActive(false);
        nextScreen.SetActive(true);
    }
}
