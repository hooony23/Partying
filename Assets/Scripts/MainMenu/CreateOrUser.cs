using Communication.MainServer;
using Communication;
using UnityEngine.UI;
using UnityEngine;
using Chatting;
public class CreateOrUser : BaseMainMenu, IMainMenu
{
    protected override void Awake()
    {
        base.Awake();
        ChatModule.GetChatModule().Start();
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
        //TODO: 로그인 화면으로 돌아가면 비활성화. 채팅창 여부 바꾸기
        var chatUi = Instantiate(Resources.Load("GameUi/Chat/ChatUi")) as GameObject;
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
            SoundManager.instance.IsPlaySfxSound("WrongMessageSound");
            return;
        }
        SoundManager.instance.IsPlaySfxSound("ButtonClickSound");
        base.SelectUI(selectUINum);
    }
    protected override void NextUI()
    {
        if(NetworkInfo.connectionId.Equals(""))
        {
            SetwarningText("서버와 통신이 원활하지 않습니다. 잠시만 기다려 주세요.");
            SoundManager.instance.IsPlaySfxSound("WrongMessageSound");
            base.NextUI();
            return;
        }
        SoundManager.instance.IsPlaySfxSound("ButtonClickSound");
        base.NextUI();
    }
}