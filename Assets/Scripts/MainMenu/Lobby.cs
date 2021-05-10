using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Communication;
using Communication.JsonFormat;
using Communication.MainServer;
using Util;
public class Lobby : BaseMainMenu, IMainMenu
{
    // SerializeField : 인스펙터에서만 접근 가능
    // 방목록 동적생성
    [SerializeField]private GameObject roomPrefab = null;
    private Transform content = null;

    // 비밀번호 입력 창
    private GameObject popup = null;
    private InputField inputPassword = null;
    [SerializeField]Text roomTitle = null;
    [SerializeField]Text roomMaster = null;
    [SerializeField]Image locker = null;
    [SerializeField]Text headcount = null;


    // 선택한 방의 정보
    private RoomInfo clickRoomInfo = null;

    // 서버 통신용

    private List<RoomInfo> rooms = new List<RoomInfo>();
    private string preRoomList = ""; // 리스트 정보가 변경되었을 때만 업데이트 하기 위함
    protected override void Awake()
    {
        base.Awake();
        SetUp();
    }    
    private void OnEnable()
    {
        GetRoomsList();
    }
    void Update()
    {
        OnUpdateRoomList(NetworkInfo.roomList);
    }
    public void SetUp()
    {
        // Initialize variable
        backUINum = 4;
        UINum = 6;

        // Set GUIObject
        content = this.transform.Find("Scroll View RoomList").Find("Viewport").Find("Content");

        popup = this.transform.Find("Popup Password").gameObject;
        inputPassword = popup.transform.Find("InputField Password").gameObject.GetComponent<InputField>();

        // Set Button Event
        this.transform.Find("Button Back").gameObject.GetComponent<Button>().onClick.AddListener(delegate { SelectUI(backUINum); });
        popup.transform.Find("Button Check").gameObject.GetComponent<Button>().onClick.AddListener(delegate {OnClickCheck();});
        popup.transform.Find("Button Cancel").gameObject.GetComponent<Button>().onClick.AddListener(delegate {OnClickCancel();});

    }
    public void OnUpdateRoomList(JArray roomList)
    {

        if (!this.preRoomList.Equals(roomList.ToString()))
        {
            Debug.Log($"preRoomList : {preRoomList.ToString()}");
            Debug.Log($"NetworkRoomList : {roomList.ToString()}");

            // 받은 서버의 정보 갱신
            UpdateRoomsList(roomList);

            // 스크롤뷰의 방 버튼 갱신
            SetRoomButtons();

            // 현재 방의 정보와 이전 방의 정보를 동기화
            this.preRoomList = roomList.ToString();

        }
    }
    public void GetRoomsList()
    {
        string response = "";
        JArray roomList;
        if (NetworkInfo.connectionId.Equals(""))
            throw new Exception("not found connectionId");
        response = MServer.GetRoomsList();
        JObject json = JObject.Parse(response);
        serverMsg = json["data"]["isSuccess"].ToString();
        roomList = json["data"]["roomList"] as JArray;
        if (serverMsg.Equals("True"))
        {
            NetworkInfo.roomList = roomList;
            OnUpdateRoomList(roomList);
        }
        else
        {
            SetwarningText(json["data"]["errorMsg"].ToString());
        }


    }

    // 서버에 받아온 roomList 정보를 rooms배열에 갱신
    private void UpdateRoomsList(JArray roomArray)
    {

        // 이전에 받아둔 방 목록들 제거
        ClearRoomList();

        // 받은 JSON 의 roomList 데이터
        rooms.AddRange(roomArray.ToObject<List<RoomInfo>>());
        Debug.Log($"room count : {rooms.Count},rooms : {roomArray.ToString()}");
    }

    // 방 목록 리스트 데이터를 프리팹(방 버튼)화 시키는 기능
    // 방 버튼 프리펩(roomPrefab) 의 각 속성에 roomUuid, roomName, admin, pwd, memberCount 순으로 입력하여
    private void SetRoomButtons()
    {
        foreach (RoomInfo room in rooms)
        {
            roomTitle = roomPrefab.transform.GetChild(0).gameObject.GetComponent<Text>();
            roomMaster = roomPrefab.transform.GetChild(1).gameObject.GetComponent<Text>();
            locker = roomPrefab.transform.GetChild(2).gameObject.GetComponent<Image>();
            headcount = roomPrefab.transform.GetChild(3).Find("Text PlayerCount").gameObject.GetComponent<Text>();
            // 방 제목
            roomTitle.text = room.RoomName;

            // 방장

            roomMaster.text = room.Admin.Nickname;
            // 방 비밀번호(자물쇠 UI)
            if(room.Pwd == null)
                throw new Exception("password is null");
            if (!room.Pwd.Equals(""))
                locker.enabled = true;
            else
                locker.enabled = false;

            // 인원수
            headcount.text = room.MemberCount.ToString();

            // 3. 방 버튼을 인스턴스화
            GameObject instance = Instantiate(roomPrefab);
            // 각 방 버튼에 리스너 추가
            Button button = instance.GetComponent<Button>();
            button.onClick.AddListener(() => { OnClickRoom(room); });

            // 꽉 찬 방은 비활성화
            if (room.MemberCount == 4)
            {
                button.interactable = false;
            }

            // 스크롤 뷰 안에 동적 생성한 버튼을 세팅
            button.transform.SetParent(content);

        }
    }

    private void OnClickRoom(RoomInfo selectRoom)
    {
        this.clickRoomInfo = selectRoom;

        // 비공개/공개 방 입장
        if (!selectRoom.Pwd.Equals(""))
        {
            Debug.Log(selectRoom.Pwd);
            popup.SetActive(true);
            return;
        }
        NetworkInfo.memberInfo = MServer.GetMemberInfo(clickRoomInfo.RoomUuid);
        NetworkInfo.roomInfo = selectRoom;
        NextUI();
    }
    // 비밀번호 팝업 메뉴 확인 버튼
    public void OnClickCheck()
    {
        if (inputPassword.text.Equals(""))
        {
            SetwarningText("비밀번호를 입력해주세요");
            return;
        }
        else if(!inputPassword.text.Equals(clickRoomInfo.Pwd))
        {
            SetwarningText("비밀번호가 틀렸습니다");
            return;
        }
        inputPassword.text = "";
        popup.SetActive(false);
        NetworkInfo.memberInfo = MServer.GetMemberInfo(clickRoomInfo.RoomUuid);
        // 해당 방의 인원 정보 재확인
        List<MemberInfo> roomMemberList = NetworkInfo.memberInfo.ToObject<List<MemberInfo>>();
        if (roomMemberList.Count >= 4)
        {
            SetwarningText("해당 방의 인원수가 초과하였습니다");
            return;
        }
        SetwarningText("비밀번호를 확인하였습니다");
        NextUI();
    }
    private void ClearRoomList()
    {
        rooms=new List<RoomInfo>();
        for(var i =0;i<content.childCount;i++)
            Destroy(content.GetChild(i).gameObject);
    }
    // 비밀번호 팝업 메뉴 취소 버튼
    public void OnClickCancel()
    {
        inputPassword.text = "";
        popup.SetActive(false);
    }
    protected override void SelectUI(int selectUINum)
    {
        NetworkInfo.roomList = new JArray();
        base.SelectUI(selectUINum);
    }
}
