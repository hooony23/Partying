using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// 유저, 방 안에 들어옴 : 방 Uuid로 방 입장, 현재 방의 상태(멤버 리스트, 방제목 등) 필요
// 방장, 최초 방 생성시 방 들어옴 : 방 Uuid를 생성하고 생성한 Uuid의 방으로 입장, 현재 방의 상태 필요

public class Room : MonoBehaviour
{
    [SerializeField] private Text title = null;
    [SerializeField] private Button startButton = null;
    [SerializeField] private Text playerCount = null;


    // 서버 통신용
    private string getMemInfoMsg;
    private List<string> users = new List<string>();

    // 현재 방의 유저 UI
    [SerializeField] GameObject[] players = null;

    // 현재 방의 정보
    public static string roomName = "";
    public static string roomUuid = "";
    public static string roomMemberCount = "";

    private void Start()
    {

    }

    // OnEnable : Hierachy 에서 활성화 될 때마다 실행
    private void OnEnable()
    {
        
        SetupRoom();
        users = MemberInfo.Get(roomUuid); // InvokeRepeating 으로 바꿔야 될 수도 있음
    }

    private void FixedUpdate()
    {
        
        if (users.Count > 0)
        {
            ActiveStartButton();

            UpdatePlayerBannerList();
        }
    }
    public void SetupRoom()
    {
        title.text = roomName;
        playerCount.text = roomMemberCount;
    }

    // 플레이어가 들어오면 PLAYER1, PLAYER2, PLAYER3, PLAYER4 UI 업데이트
    private void UpdatePlayerBannerList()
    {
        Text pText;
        Image pImage;

        for (int i = 0; i < users.Count; i++)
        {
            // USER(1~4) UI 초기화
            pText = players[i].GetComponentInChildren<Text>();
            pImage = players[i].GetComponentInChildren<Image>();

            pText.text = users[i];
            pImage.color = Color.white;
        }
    }

    // 멤버가 다 차면 시작 버튼 활성화
    // 방장이 시작 버튼을 누르면 멤버 전부 화면 이동
    private void ActiveStartButton()
    {
        if (users.Count == 4)
            startButton.interactable = true;
        else
            startButton.interactable = false;
    }


     public void OnClickGameStart()
    {
        SceneManager.LoadScene("LodingScene");
        // Game Scene 으로 넘어감
    }
 
}


