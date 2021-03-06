using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PauseControl : MonoBehaviour
{
    public static PauseControl pauseMenu;
    private bool IsUiPoen = false;
    [SerializeField]
    private GameObject PauseObject;
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject OptionMenu;

    private void Awake()
    {
        if (pauseMenu == null) //pauseMenu 없을시
        {
            if (Config.LodingSence != 0) //메인메뉴가 아닐때만 작동
            {
                pauseMenu = this;
                DontDestroyOnLoad(gameObject); // 씬 변경후에도 메뉴를 관리하도록 함
                                               // 환경설정 슬라이더에 초기값 대입
            }
        }
        else
        { //씬 이동후 pauseMenu 중복방지를 위한 기존 pauseMenu 파괴
            Destroy(gameObject);
        }
    }
    void Update()
    {
        if (Input.GetButtonDown("pause"))
        {
            if (!IsUiPoen)
            {
                PauseObject.SetActive(true);
                IsUiPoen = true;
                
                
                Debug.Log(IsUiPoen+"========================================================");
            }
            else if (IsUiPoen)
            {
                Debug.Log("종료실행 ======================================================== ");
                if (OptionMenu.activeSelf == true) // 초기메뉴 화면으로 가기위함
                {
                    MainMenu.SetActive(true);
                    OptionMenu.SetActive(false);
                }
                PauseObject.SetActive(false);
                IsUiPoen = false;
            }
        }
    }
}