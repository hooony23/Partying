using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Communication;
using Communication.MainServer;
using Util;
public class Lobby : BaseMainMenu
{
    // SerializeField : 인스펙터에서만 접근 가능
    // 방목록 동적생성
    [SerializeField] private GameObject roomPrefab = null;
    [SerializeField] private Transform content = null;
    [SerializeField] private GameObject nextScreen = null; // RoomUI
    [SerializeField] private Button refresh = null;


    // 비밀번호 입력 창
    [SerializeField] private GameObject popup = null;
    [SerializeField] private Text message = null;
    [SerializeField] private InputField inputPassword = null;


    // 선택한 방의 정보
    private RoomInfo clickRoomInfo = null;

    // 서버 통신용
    
    private List<RoomInfo> rooms = new List<RoomInfo>();
    private string preRoomList = ""; // 리스트 정보가 변경되었을 때만 업데이트 하기 위함

    private void OnEnable()
    {
        GetRoomsList();
    }
    void Update()
    {
        OnUpdateRoomList(NetworkInfo.roomList);
    }
    public void OnUpdateRoomList(JArray roomList)
    {
        
        if (!this.preRoomList.Equals(roomList.ToString()))
        {
            Debug.Log($"preRoomList : {preRoomList.ToString()}");
            Debug.Log($"NetworkRoomList : {roomList.ToString()}");
            //Debug.Log("방 목록을 갱신합니다");

            // 받은 서버의 정보 갱신
            UpdateRoomsList(roomList);

            // 스크롤뷰의 방 버튼 갱신
            SetRoomButtons();

            // 다음에 수행될 때 현재 방의 정보와 이전 방의 정보를 비교
            this.preRoomList = roomList.ToString();

        }
    }
    public void GetRoomsList()
    {
        string roomsListUri = "api/v1/rooms/main";
        string response = "";
        JArray roomList;
        if(NetworkInfo.connectionId.Equals(""))
            throw new Exception("not found connectionId");
        response = MServer.Communicate("GET", roomsListUri, $"userUuid={Config.userUuid}&connectionId={NetworkInfo.connectionId}");
        Debug.Log($"lobby response: {response}");
        JObject json = JObject.Parse(response);

        serverMsg = json["data"]["isSuccess"].ToString();
        roomList = json["data"]["roomList"] as JArray;
        if(serverMsg.Equals("True"))
        {
            OnUpdateRoomList(roomList);
        }
        else
        {
            SetwarningText(json["data"]["errorMsg"].ToString());
        }


    }

    // 서버에 받아온 roomList 정보를 rooms배열에 갱신
    private void UpdateRoomsList(JArray roomList)
    {
        // 받은 JSON 의 roomList 데이터

        // 이전에 받아둔 방 목록들 제거
        rooms.Clear();

        // roomList 배열 파싱
        for (int i = 0; i < roomList.Count; i++)
        {
            RoomInfo info = new RoomInfo();
            info.RoomUuid = roomList[i]["roomUuid"].ToString();
            info.RoomName = roomList[i]["roomName"].ToString();
            info.RoomAdmin = roomList[i]["admin"].ToString();
            info.MemberCount = roomList[i]["memberCount"].ToString();
            info.Pwd = roomList[i]["pwd"].ToString();

            rooms.Add(info);
        }
    }

    // 방 목록 리스트 데이터를 프리팹(방 버튼)화 시키는 기능
    // 방 버튼 프리펩(roomPrefab) 의 각 속성에 roomUuid, roomName, admin, pwd, memberCount 순으로 입력하여
    private void SetRoomButtons()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            RoomInfo currentRoom = rooms[i];

            // 1. 방 버튼 프리펩에 넣을 정보를 가져옴
            string roomUuid = rooms[i].RoomUuid;
            string roomName = rooms[i].RoomName;
            string admin = rooms[i].RoomAdmin;
            string Pwd = rooms[i].Pwd;
            string memberCount = rooms[i].MemberCount;

            // 2. 방 버튼 프리펩 정보 구성
            // 방 제목
            Text roomTitle = roomPrefab.transform.Find("Text Title").gameObject.GetComponent<Text>();
            roomTitle.text = roomName;

            // 방장
            Text roomMaster = roomPrefab.transform.Find("Text Master").gameObject.GetComponent<Text>();
            roomMaster.text = admin;

            // 방 비밀번호(자물쇠 UI)
            Image locker = roomPrefab.transform.Find("Image Locker").gameObject.GetComponent<Image>();
            if (!Pwd.Equals(""))
                locker.enabled = true;
            else
                locker.enabled = false;

            // 인원수
            Text headcount = roomPrefab.transform.Find("Text Headcount").gameObject.GetComponent<Text>();
            headcount.text = memberCount;

            // 3. 방 버튼을 인스턴스화
            GameObject instance = Instantiate(roomPrefab);
            // 각 방 버튼에 리스너 추가
            Button button = instance.GetComponent<Button>();
            button.onClick.AddListener(() => { OnClickRoom(currentRoom); });

            // 꽉 찬 방은 비활성화
            if (int.Parse(memberCount) == 4)
            {
                button.interactable = false;
            }

            // 스크롤 뷰 안에 동적 생성한 버튼을 세팅
            button.transform.SetParent(content);

        }
    }


    public void OnClickRefresh()
    {
        // GetRoomsList();
    }

    private void OnClickRoom(RoomInfo currentRoom)
    {
        this.clickRoomInfo = currentRoom;
        NetworkInfo.memberInfo = MemberInfo.Get(currentRoom.RoomUuid);
        // 해당 방의 인원 정보 재확인
        List<MemberInfo> roomMemberList = NetworkInfo.memberInfo.ToObject<List<MemberInfo>>();
        if (roomMemberList.Count >= 4)
        {
            SetwarningText("해당 방의 인원수가 초과하였습니다");
            return;
        }

        // 비공개/공개 방 입장
        if (!currentRoom.Pwd.Equals(""))
        {
            Debug.Log(currentRoom.Pwd);
            popup.SetActive(true);
        }
        else
        {
            // 들어갈 방의 정보 세팅
            Room.roomName = currentRoom.RoomName;
            Room.roomUuid = currentRoom.RoomUuid;
            Room.roomMemberCount = (int.Parse(currentRoom.MemberCount) + 1).ToString();

            GoNextScreen();
        }
    }

    private void GoNextScreen()
    {
        // 다음화면으로
        this.gameObject.SetActive(false);
        nextScreen.SetActive(true);
    }
    private void GoBackScreen()
    {
        
    }
    // 비밀번호 팝업 메뉴 확인 버튼
    public void OnClickCheck()
    {
        if (inputPassword.text.Equals(clickRoomInfo.Pwd))
        {
            message.text = "비밀번호를 확인하였습니다";

            // 들어갈 방의 정보 세팅
            Room.roomName = this.clickRoomInfo.RoomName;
            Room.roomUuid = this.clickRoomInfo.RoomUuid;
            Room.roomMemberCount = (int.Parse(this.clickRoomInfo.MemberCount) + 1).ToString();

            GoNextScreen();
            inputPassword.text = "";
            popup.SetActive(false);
        }
        else
        {
            message.text = "비밀번호가 틀렸습니다";
        }
        Invoke("SetBasicMessage", 2f);
    }

    // 비밀번호 팝업 메뉴 취소 버튼
    public void OnClickCancel()
    {
        inputPassword.text = "";
        popup.SetActive(false);
    }

    private void SetBasicMessage()
    {
        message.text = "비밀번호를 입력해주세요";
    }
}
