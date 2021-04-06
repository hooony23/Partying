using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Communication.MainServer;
public class PopupSystem : BaseMainMenu
{
    // 팝업할 객체는 해당하는 스크립트에서 PopupSystem.instance.OpenPopUp() 으로 UI를 팝업 시킨다
    // (팝업할 객체는 PopupAnimator 컴포넌트를 가지고 있어야 한다)

    private GameObject popup;
    private Animator anim;


    public static PopupSystem instance { get; set; }

    private void Awake()
    {
        instance = this;
    }

    public void OpenPopUp(GameObject forPopup)
    {
        forPopup.SetActive(true);
    }

    public void ClosePopUp(GameObject forPopup)
    {
        anim = forPopup.GetComponent<Animator>();
        anim.SetTrigger("Close");
        StartCoroutine("SetFalseDelay", forPopup);
    }

    IEnumerator SetFalseDelay(GameObject forPopup)
    {
        // 0.15초 뒤에 게임오브젝트를 비활성화를 해야 애니메이션이 보입니다
        yield return new WaitForSeconds(0.15f);
        forPopup.SetActive(false);
    }
    
    private void OnApplicationQuit() {
        Communication.GameServer.Connection.ConnectedExit();
    }
}
