using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Communication;
using Communication.MainServer;
using Util;

public class FirstScreen : BaseMainMenu, IMainMenu
{
    protected override void Awake()
    {
        base.Awake();
        SetUp();
        Debug.Log($"uuid : {Util.Config.userUuid}");
        if(Config.defaultStage>=2)
        {
            Config.defaultStage=0;
            SelectUI(7);
            //TODO: BGM, 채팅창, 설정할것
            SoundManager.instance.IsPlaySound("Main");
        }
    }
    void OnEnable()
    {
        UINum = 1;
    }
    public void SetUp()
    {        
        
        // Communication Test
        try
        {
            MServer.Pingpong();
            if(NetworkInfo.connectionId.Equals(""))
                new WebSocketModule().Start();
        }
        catch
        {
            SetwarningText("서버와 통신 할 수 없습니다. 다시 접속해주세요.");
            Invoke("OnQuit", 3f);
        }
        // Set Button Event
        this.transform.Find("Button Start").GetComponent<Button>().onClick.AddListener(delegate {OnClickStart();});
        this.transform.Find("Button Quit").GetComponent<Button>().onClick.AddListener(delegate {OnClickQuit();});
    }
    public void OnClickStart()
    {
        SetwarningText("게임을 시작하였습니다. 로그인 화면으로 갑니다.");
        Invoke("NextUI",3f);
    }

    public void OnClickQuit()
    {
        OnQuit();
    }
    protected override void OnApplicationQuit() {}
}
