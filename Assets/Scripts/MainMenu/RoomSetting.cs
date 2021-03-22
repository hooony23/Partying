using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

// 방만들기

public class RoomSetting : MonoBehaviour
{
    public GameObject warning;
    public GameObject nextScreen; // RoomUI

    [SerializeField] private InputField roomTitleInput = null;  
    [SerializeField] private InputField roomPasswordInput = null;
    private string title = "";
    private string password = "";

    // 서버 통신용
    private string serverMsg = "";

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
            Debug.Log("방을 생성하였습니다");

            // [createRoom API] uri : api/v1/rooms/createRoom , method : POST
            string createRoomUri = "api/v1/rooms/createRoom";
            string response = "";

            createRoomInfo info = new createRoomInfo();
            info.UpdateInfo(Title, password);
            var requestJson = Communication.JsonFormat.BaseJsonFormat.ObjectToJson("creatRoom", "center_server", info);

            response = MServer.Communicate(createRoomUri, "POST", requestJson);
            Debug.Log(response);
            JObject json = JObject.Parse(response);
            serverMsg = json["data"]["isSuccess"].ToString();

            if (serverMsg.Equals("True"))
            {
                // 방 진입
                // 방의 Uuid를 생성하는 과정이 필요
                Room.roomName = title;
                Room.roomUuid = json["data"]["roomUuid"].ToString();
                Room.roomMemberCount = "1";
                Debug.Log(Room.roomName);
                GoNextScreen();
            }

        }
        else
        {
            Debug.Log("방 제목을 입력해주세요");
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
