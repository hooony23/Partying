using System.Net.NetworkInformation;
using Communication.MainServer;
using Communication;
using Util;
using UnityEngine.UI;
using UnityEngine;
public class CreateOrUser : BaseMainMenu, IMainMenu
{
    protected override void Awake()
    {
        base.Awake();
        SetUp();
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
        //TODO: 로그인 화면으로 돌아가면 비활성화.
        var chatUi = Instantiate(Resources.Load("GameUi/Chat/ChatUi")) as GameObject;
        chatUi.transform.SetParent(GameObject.Find("Main Menu Canvas").transform);
    }
    protected override void BackUI()
    {
        MServer.SignOut();
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
            SetwarningText("서버와 통신이 원활하지 않습니다. 잠시만 기다려 주세요.");
            return;
        }
        base.NextUI();
    }
}