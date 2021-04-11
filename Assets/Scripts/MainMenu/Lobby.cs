using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    // SerializeField : 인스펙터에서만 접근 가능
    // 방목록 동적생성
    [SerializeField] private GameObject roomPrefab = null;
    [SerializeField] private Transform content = null;
    [SerializeField] private GameObject nextScreen = null; // RoomUI



    // 예시) 받아올 방들의 정보
    List<RoomInfo> rooms;
    RoomInfo room1 = new RoomInfo(0, "안녕안녕", "hooony23", "1234", 1);
    RoomInfo room2 = new RoomInfo(1, "드루와드루와", "skine134", "", 3);
    RoomInfo room3 = new RoomInfo(2, "자신있는 사람만", "prawn12", "", 4);
    RoomInfo room4 = new RoomInfo(3, "아무나", "sunkyu", "", 1);
    RoomInfo room5 = new RoomInfo(4, "신세계", "newt", "", 3);
    RoomInfo room6 = new RoomInfo(5, "아모르파티", "dynamic", "", 2);
    RoomInfo room7 = new RoomInfo(6, "바보바보바보", "hasse", "", 1);

    // 비밀번호 입력 창
    [SerializeField] private GameObject popup = null;
    [SerializeField] private Text message = null;
    [SerializeField] private InputField inputPassword = null;
    [SerializeField] private Button check = null;
    [SerializeField] private Button cancel = null;



    // 선택한 방의 정보
    private RoomInfo clickroomInfo = null;


    private void Awake()
    {

    }

    void Start()
    {
        // 예시
        rooms = new List<RoomInfo>();
        rooms.Add(room1);
        rooms.Add(room2);
        rooms.Add(room3);
        rooms.Add(room4);
        rooms.Add(room5);
        rooms.Add(room6);
        rooms.Add(room7);

        AddElement(); // 방 목록 갱신

        popup.SetActive(false); // 팝업 메뉴 비활성화로 초기화


    }

    void FixedUpdate()
    {
        // 받아온 배열을 통해 방 목록 동기화

    }

    private void AddElement()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            // 모든 방의 정보를 받아옴
            int index = rooms[i].RoomIndex;
            string title = rooms[i].RoomTitle;
            string master = rooms[i].RoomMaster;
            string password = rooms[i].RoomPassword;
            int headcount = rooms[i].Headcount;

            // roomPrefab 의 자식 컴포넌트에 정보를 넣는 과정
            Text roomTitle = roomPrefab.transform.Find("Text Title").gameObject.GetComponent<Text>();
            roomTitle.text = title;

            Text roomMaster = roomPrefab.transform.Find("Text Master").gameObject.GetComponent<Text>();
            roomMaster.text = master;

            Text roomPassword = roomPrefab.transform.Find("Text Password").gameObject.GetComponent<Text>();
            roomPassword.text = password;

            Text roomHeadcount = roomPrefab.transform.Find("Text Headcount").gameObject.GetComponent<Text>();
            roomHeadcount.text = headcount.ToString();



            // 받아온 방 개수만큼 방 버튼 동적 생성 과정
            GameObject instance = Instantiate(roomPrefab); // 인스턴스화
            Button button = instance.GetComponent<Button>(); // Button형식으로 바꿔줌
            button.onClick.AddListener(() => { OnClickRoom(index); }); // 생성된 Button마다 Listener 추가
            button.transform.SetParent(content); // Scroll View > Viewport > Content 안에 동적 생성

            // 인원 꽉 찼을 시 비활성화
            if (headcount == 4)
            {
                button.interactable = false;
            }
        }

    }

    // 비밀번호 없는 방
    void OnClickRoom(int index)
    {
        this.clickroomInfo = rooms[index];

        // 비밀번호가 "" 인 공개방 입장
        if (clickroomInfo.RoomPassword.Equals(""))
        {
            Debug.Log(clickroomInfo.RoomTitle + " 방을 입장합니다");
            this.gameObject.SetActive(false);
            nextScreen.SetActive(true);


        }
        // 비밀번호가 "" 가 아닌 비밀방 클릭
        else
        {
            // 비밀번호 입력 창 팝업시킴
            message.text = "비밀번호를 입력해주세요";
            PopupSystem.instance.OpenPopUp(popup);

        }
    }

    // Popup 메뉴 확인버튼(비밀번호 있는 방)
    public void OnClickCheck()
    {

        // 확인 클릭
        // 비밀번호 일치 시 방 입장 처리, RoomUI화면
        // 비밀번호 불일치시 "틀렸습니다" 출력

        if (inputPassword.text.Equals(clickroomInfo.RoomPassword))
        {
            Debug.Log("비밀번호 일치합니다");
            popup.SetActive(false); // 팝업 바로 비활성화
            inputPassword.text = ""; // 팝업 입력했던 비밀번호 재설정

            // 다음 화면으로
            this.gameObject.SetActive(false);
            nextScreen.SetActive(true);

        }
        else
        {
            message.text = "틀렸습니다";
            inputPassword.text = "";
        }

    }

    public void OnClickCancel()
    {
        PopupSystem.instance.ClosePopUp(popup);
    }
}
