using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Communication.MainServer;

public class FirstScreen : BaseMainMenu, IMainMenu
{
    public void Start()
    {
        SetUp();
        Debug.Log($"uuid : {Util.Config.userUuid}");
    }
    void OnEnable()
    {
        UINum = 1;
    }
    public void SetUp()
    {        
        // Set User Uuid
        Lib.Common.SetUserUuid(System.Guid.NewGuid().ToString());
        
        // Communication Test
        try
        {
            MServer.Communicate("GET", "api/v1/util/pingpong");
        }
        catch
        {
            SetwarningText("서버와 통신 할 수 없습니다. 다시 접속해주세요.");
            Invoke("OnClickQuit", 3f);
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
        // 에디터 편집상황이면 게임정지, 어플리케이션 실행상황이면 어플리케이션 종료
<<<<<<< HEAD
#if UNITY_EDITOR
=======
>>>>>>> origin/dev-SungyuHwang
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
