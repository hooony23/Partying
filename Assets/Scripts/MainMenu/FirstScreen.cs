using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirstScreen : BaseMainMenu
{

    public void Start()
    {
        //TODO: Test시 주석 처리 할 것
        Lib.Common.SetUserUuid(System.Guid.NewGuid().ToString());
        Debug.Log(MServer.Communicate("api/v1/util/pingpong", "GET"));
        Debug.Log($"uuid : {Util.Config.userUuid}");
    }
    public void OnClickStart()
    {
        Debug.Log("게임을 시작하였습니다. 로그인 화면으로 갑니다.");
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        // 에디터 편집상황이면 게임정지, 어플리케이션 실행상황이면 어플리케이션 종료
        UnityEditor.EditorApplication.isPlaying = false; 
#else
        Application.Quit(); 
#endif
    }

}
