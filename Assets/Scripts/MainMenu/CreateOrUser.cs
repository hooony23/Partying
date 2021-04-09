using Communication.MainServer;
using UnityEngine.UI;
public class CreateOrUser : BaseMainMenu, IMainMenu
{
    void Start()
    {
        SetUp();

        //TODO: 속도 느림 개선 필요
        new WebSocketModule().Start();
    }
    public void SetUp()
    {
        UINum = 3;
        nextUINum = 5;
        this.transform.Find("Button Back").gameObject.GetComponent<Button>().onClick.AddListener(delegate {BackUI();});
        this.transform.Find("RoomSetting").Find("Button RoonSetting").gameObject.GetComponent<Button>().onClick.AddListener(delegate {NextUI();});
        this.transform.Find("Lobby").Find("Button Lobby").gameObject.GetComponent<Button>().onClick.AddListener(delegate {SelectUI(nextUINum);});
    }
}