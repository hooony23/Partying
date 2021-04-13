using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class MapClearItem : MonoBehaviour
{
    // enum 선언으로 열거형 타입으로 아이템 분류
    public enum MapClear { ClearItem };
    public MapClear ItemType;

    //게임클리어 Ui
    private GameObject GameClearUi;
    private Button ContinueButton = null;
    private Animator animator;
    private bool isanimation = false;

    private bool UserClear = false;
    private bool DestrotyBox = false;
    private bool UserallClear = false;
    private void Awake()
    {
        GameObject ClearObject = Instantiate(Resources.Load("GameUi/GameClearUi")) as GameObject;
        GameClearUi = ClearObject.transform.Find("ClearUi").gameObject;
        ContinueButton = GameClearUi.transform.Find("GameClearButton").GetComponent<Button>();
        animator = transform.Find("ChestBox").GetComponent<Animator>();
    }
    private void Start()
    {
        ContinueButton.onClick.AddListener(UserClearButton);
    }
    private void Update()
    {
        //TODO: 만약 아이템이 추가되면 이 부분 수정 필요.
        if (Communication.NetworkInfo.GetItemUserQueue.Count > 0)
            Config.GameClear = true;
        if (Config.GameClear && !DestrotyBox)
        {
            StartCoroutine(ClearUi());
        }
        if (UserClear && UserallClear)
        {
            StartCoroutine(SceneChange());
        }
    }
    //유저가 누르는 씬전환 확인 버튼
    public void UserClearButton()
    {
        Debug.Log("button Test");
        UserClear = true;
        Config.GameClear = false;
        ContinueButton.interactable = false;
    }

    //게임 클리어 UI 활성화
    public void isGameClear()
    {
        GameClearUi.SetActive(true);
    }
    //5초뒤 클리어 확인(애니메이션 추가예정)
    IEnumerator ClearUi()
    {
        Debug.Log("animation");
        isanimation = animator.GetBool("IsBoxOpen");
        animator.SetBool("IsBoxOpen", !isanimation);
        DestrotyBox = true;
        yield return new WaitForSeconds(5f);
        isGameClear();
    }
    //게임클리어UI활성화 및 씬전환(4명이 전부 확인버튼 누르면 씬전환 추가예정)
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(5f);
        GameClearUi.SetActive(true);
        Debug.Log(UserClear);
        if (UserClear)
        {
            yield return new WaitForSeconds(2f);
            if (UserallClear)
            {
                Config.GameClear = false;
                SceneManager.LoadScene("First Map");
            }
            UserallClear = true;
        }

    }
}

