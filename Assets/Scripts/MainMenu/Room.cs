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
    // 서버 통신용
    private string getMemInfoMsg;
    private Transform playerGrid;
    private JArray users;
    private Color readyColor;
    // 현재 방의 유저 UI
    GameObject[] players = null;
    GameObject[] defaultGridSet = null;
    private HashSet<string> readyUserSet = null;
    // 현재 방의 정보
    public string roomUuid = "";
    protected override void Awake()
    {
        base.Awake();
        SetUp();
        Communication.NetworkInfo.mapInfo = null;
    }
    // OnEnable : Hierachy 에서 활성화 될 때마다 실행
    public void OnEnable()
    {
        roomUuid = NetworkInfo.roomInfo.RoomUuid;
        readyUserSet = new HashSet<string>();
        UINum = 7;
        SetText();
    }

    private void FixedUpdate()
    {
        OnUpdateMemberInfo();
        CheckReadyUser();
        if(Lib.Common.IsAdmin())
            ActiveStartButton();
        if (Communication.NetworkInfo.mapInfo != null)
            GameStart();
    }
    public void SetUp()
    {
        //Initialize Variable
        readyColor = new Color(0.7f,0.4f,0.3f);
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
        startButton = startButtonTransfom.GetComponent<Button>();

        // Set Button Event
        this.transform.Find("Button Back").GetComponent<Button>().onClick.AddListener(delegate { BackUI(); });
        startButton.onClick.AddListener(delegate { OnClickGameStart(); });
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


    // 플레이어가 들어오면 PLAYER1, PLAYER2, PLAYER3, PLAYER4 UI 업데이트
    private void UpdatePlayerBannerList()
    {
        Text pText;
        Image pImage;
        ClearPlayerGrid();

        //ready 초기화.
        readyUserSet = new HashSet<string>();
        ReadyReset();
        for (int i = 0; i < users.Count; i++)
        {
            var user = users[i].ToObject<MemberInfo>();
            if(i==0){
                NetworkInfo.roomInfo.Admin = user;
            }
            // USER(1~4) UI 초기화
            pText = players[i].GetComponentInChildren<Text>();
            pImage = players[i].GetComponentInChildren<Image>();

            pText.text = user.Nickname;
            pImage.color = Color.white;

            if (Lib.Common.IsAdmin())
            {
                startButton.transform.Find("Text").GetComponent<Text>().text = "Game Start";
            }
            else
            {
                startButton.transform.Find("Text").GetComponent<Text>().text = "Game Ready";
            }
        }
    }
    private void ClearPlayerGrid()
    {
        for (int i = 0; i < 4; i++)
        {
            players[i].GetComponentInChildren<Text>().text = $"PLAYER{i + 1}";
            players[i].GetComponentInChildren<Text>().color = new Color(0,0,0);
            players[i].GetComponentInChildren<Image>().color = new Color(0.45f, 0.45f, 0.45f);
        }
    }
    // 멤버가 다 차면 시작 버튼 활성화
    // 방장이 시작 버튼을 누르면 멤버 전부 화면 이동
    private void ActiveStartButton()
    {
        if (readyUserSet.Count >= users.Count-1)
            SetStartButtonActive(true);
        else
            SetStartButtonActive(false);
    }
    public void OnClickGameStart()
    {
        //레디
        if(startButton.interactable)
        {
            APIController.SendController("Connected");
            MServer.Ready(true);
            SetStartButtonActive(false);
        }
        else
        {
            //레디 해제
            APIController.SendController("ConnectedExit");
            MServer.Ready(false);
            SetStartButtonActive(true);
        }
        if (Lib.Common.IsAdmin())
        {
            APIController.SendController("CreateMap");
            // Game Scene 으로 넘어감
            GameStart();
        }
    }
    private void SetStartButtonActive(bool isActive)
    {
        startButton.interactable = isActive;
        if (startButton.interactable)
            startButton.transform.GetChild(0).GetComponent<Text>().color = new Color(1f, 1f, 1f);
        else
            startButton.transform.GetChild(0).GetComponent<Text>().color = new Color(0.45f, 0.45f, 0.45f);
    }
    public void GameStart()
    {
        SceneManager.LoadScene("LodingScene"); //Coroutine을 이용해 시간 딜레이 추가 여부 상의 필요
    }
    public void ReadyReset()
    {
        if(!startButton.interactable&&!Lib.Common.IsAdmin())
        {
            APIController.SendController("ConnectedExit");
            SetStartButtonActive(true);
        }
    }
    public void CheckReadyUser()
    {
        var readyUserInfo = ReadyUserInfo.GetReadyUserInfo();
        var color = Color.black;
        if(readyUserInfo ==null)
            return;
        
        if(readyUserInfo.Ready){
            readyUserSet.Add(readyUserInfo.Player);
            color = readyColor;
        }
        else
            readyUserSet.Remove(readyUserInfo.Player);
        SetUserColor(readyUserInfo.Player,color);
    }

    protected void SetUserColor(string userUuid,Color color)
    {
        
        
        for(var i=0 ;i< users.Count;i++)
        {
            var memberInfo = users[i].ToObject<MemberInfo>();
            if(userUuid.Equals(memberInfo.UserUuid))
            {
                players[i].GetComponentInChildren<Text>().color = color;
                break;
            }
        }
    }
    protected override void BackUI()
    {
        ReadyReset();
        MServer.Ready(false);
        NetworkInfo.roomInfo = new RoomInfo();
        MServer.LeaveRoom(roomUuid);
        base.BackUI();
    }
    protected override void OnApplicationQuit()
    {
        ReadyReset();
        MServer.Ready(false);
        MServer.LeaveRoom(roomUuid);
        base.OnApplicationQuit();
    }
}


