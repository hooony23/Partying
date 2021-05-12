using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Communication;
using Communication.MainServer;
using Communication.JsonFormat;
using Communication.GameServer.API;
using Util;
// 유저, 방 안에 들어옴 : 방 Uuid로 방 입장, 현재 방의 상태(멤버 리스트, 방제목 등) 필요
// 방장, 최초 방 생성시 방 들어옴 : 방 Uuid를 생성하고 생성한 Uuid의 방으로 입장, 현재 방의 상태 필요

public class Room : BaseMainMenu, IMainMenu
{
    private Text title = null;
    private Button startButton = null;
    private Text playerCount = null;
    // 서버 통신용
    private string getMemInfoMsg;
    private Transform playerGrid;
    private JArray users;

    // 현재 방의 유저 UI
    GameObject[] players = null;
    GameObject[] defaultGridSet = null;

    // 현재 방의 정보
    public string roomUuid = "";
    protected override void Awake()
    {
        base.Awake();
        SetUp();
        Communication.NetworkInfo.mapInfo = null;
    }
    public void OnEnable()
    {
        roomUuid = NetworkInfo.roomInfo.RoomUuid;
        UINum = 7;
        SetText();
    }
    public void Update()
    {
        // if()
        // ActiveGameStartButton()
    }
    public void SetUp()
    {
        //Initialize Variable
        playerGrid = this.transform.Find("Player Grid");
        var startButtonTransfom = this.transform.Find("Button Start");
        users = NetworkInfo.memberInfo;
        
        // Set GUI Object
        players = new GameObject[]{
                                    playerGrid.GetChild(0).gameObject,
                                    playerGrid.GetChild(1).gameObject,
                                    playerGrid.GetChild(2).gameObject,
                                    playerGrid.GetChild(3).gameObject};
        defaultGridSet = players;
        title = this.transform.Find("RoomTitle").Find("Text RoomTitle").gameObject.GetComponent<Text>();
        playerCount = this.transform.Find("PlayerCount").Find("Text PlayerCount").GetComponent<Text>();
        startButton = startButtonTransfom.GetComponent<Button>();
        
        // Set Button Event
        this.transform.Find("Button Back").GetComponent<Button>().onClick.AddListener(delegate {BackUI();});
        startButton.onClick.AddListener(delegate {OnClickGameStart();});
    }
    private void SetText()
    {
        Debug.Log($"RoomName : {NetworkInfo.roomInfo.RoomName}");
        title.text = NetworkInfo.roomInfo.RoomName;
        UpdatePlayerBannerList();
    }
    private void OnUpdateMemberInfo()
    {
        if (!users.ToString().Equals(NetworkInfo.memberInfo.ToString()))
        {
            Debug.Log($"membersInfo : {users.ToString()}");
            Debug.Log($"network.memberInfo : {NetworkInfo.memberInfo.ToString()}");
            users = NetworkInfo.memberInfo;
            UpdatePlayerBannerList();
        }
    }
    // OnEnable : Hierachy 에서 활성화 될 때마다 실행

    private void FixedUpdate()
    {
        OnUpdateMemberInfo();
        if (users.Count > 0)
        {
            ActiveStartButton();
        }
        if (Communication.NetworkInfo.mapInfo != null)
            GameStart();
    }

    // 플레이어가 들어오면 PLAYER1, PLAYER2, PLAYER3, PLAYER4 UI 업데이트
    private void UpdatePlayerBannerList()
    {
        Text pText;
        Image pImage;
        List<string> usersName = new List<string>();
        playerCount.text = NetworkInfo.roomInfo.MemberCount.ToString();
        ClearPlayerGrid();
        foreach(JObject item in users)
        {
            usersName.Add(item["nickname"].ToString());
            Debug.Log($"user Name:{item["nickname"].ToString()}");
        }
        for (int i = 0; i < users.Count; i++)
        {
            // USER(1~4) UI 초기화
            pText = players[i].GetComponentInChildren<Text>();
            pImage = players[i].GetComponentInChildren<Image>();

            pText.text = usersName[i];
            pImage.color = Color.white;
            
            if(Lib.Common.IsAdmin())
            {
                startButton.transform.Find("Text").GetComponent<Text>().text = "Game Start";
                startButton.gameObject.GetComponent<Button>().interactable = false;
            }
            else
            {
                startButton.transform.Find("Text").GetComponent<Text>().text = "Game Ready";
                startButton.gameObject.GetComponent<Button>().interactable = false;
            }
        }
        playerCount.text = NetworkInfo.roomInfo.MemberCount.ToString();
    }
    private void ClearPlayerGrid()
    {
        for (int i=0;i<4;i++)
        {
            players[i].GetComponentInChildren<Text>().text = $"PLAYER{i+1}";
            players[i].GetComponentInChildren<Image>().color = new Color(0.45f,0.45f,0.45f);
        }
    }
    // 멤버가 다 차면 시작 버튼 활성화
    // 방장이 시작 버튼을 누르면 멤버 전부 화면 이동
    private void ActiveStartButton()
    {
        if (users.Count >=1 )
            startButton.interactable = true;
        else
            startButton.interactable = false;
    }
     public void OnClickGameStart()
    {
        APIController.SendController("Connected");
        if(Lib.Common.IsAdmin())
        {
            APIController.SendController("CreateMap");
            // Game Scene 으로 넘어감
            GameStart();
        }
    }
    public void GameStart()
    {
        SceneManager.LoadScene("LodingScene"); //Coroutine을 이용해 시간 딜레이 추가 여부 상의 필요
    }
    protected override void BackUI()
    {
        NetworkInfo.roomInfo = new RoomInfo();
        MServer.LeaveRoom(roomUuid);
        base.BackUI();
    }
    protected override void OnApplicationQuit()
    {
        MServer.LeaveRoom(roomUuid);
        base.OnApplicationQuit();
    }
}


