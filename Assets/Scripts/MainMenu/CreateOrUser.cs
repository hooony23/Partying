using System.Net.NetworkInformation;
using Communication.MainServer;
using Communication;
using Util;
using UnityEngine.UI;
using UnityEditor.Build.Content;
using UnityEngine;

public class CreateOrUser : BaseMainMenu, IMainMenu
{
    void Start()
    {
        SetUp();

        //TODO: 속도 느림 개선 필요
        new WebSocketModule().Start();
    }
    void OnEnable()
    {
        UINum = 4;
        nextUINum = 6;
    }
    public void SetUp()
    {
        this.transform.Find("Button Back").gameObject.GetComponent<Button>().onClick.AddListener(delegate {BackUI();});
        this.transform.Find("RoomSetting").Find("Button RoomSetting").gameObject.GetComponent<Button>().onClick.AddListener(delegate {NextUI();});
        this.transform.Find("Lobby").Find("Button Lobby").gameObject.GetComponent<Button>().onClick.AddListener(delegate {SelectUI(nextUINum);});
        var chatUi = Instantiate(Resources.Load("Chat/ChatUI")) as GameObject;
        chatUi.transform.SetParent(GameObject.Find("Main Menu Canvas").transform);
    }
    private void OnApplicationQuit() {
        MServer.Communicate("GET","api/v1/session/signOut",$"userUuid={Util.Config.userUuid}");    
    }
    protected override void BackUI()
    {
        MServer.Communicate("GET","api/v1/session/signOut",$"userUuid={Config.userUuid}");
        SelectUI(2);
    }
    protected override void SelectUI(int selectUINum)
    {
        if(NetworkInfo.connectionId.Equals(""))
        {
            SetwarningText("연결이 확실치 않습니다 잠시만 기다려 주세요.");
            return;
        }
        base.SelectUI(selectUINum);
    }
    protected override void NextUI()
    {
        if(NetworkInfo.connectionId.Equals(""))
        {
            SetwarningText("연결이 확실치 않습니다 잠시만 기다려 주세요.");
            return;
        }
        base.NextUI();
    }
}