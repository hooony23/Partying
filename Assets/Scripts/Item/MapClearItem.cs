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
    private bool DestrotyBox = false;
    private void Awake()
    {
        GameObject ClearObject = Instantiate(Resources.Load("GameUi/GameClearUi")) as GameObject;
        GameClearUi = ClearObject.transform.Find("ClearUi").gameObject;
        //ContinueButton = GameClearUi.transform.Find("GameClearButton").GetComponent<Button>();
        animator = transform.Find("ChestBox").GetComponent<Animator>();
    }
    private void Start()
    {
        //ContinueButton.onClick.AddListener(UserClearButton);
    }
    private void Update()
    {
        if (Config.GameClear && !DestrotyBox)
        {
            StartCoroutine(ClearUi());
        }
    }
    //유저가 누르는 씬전환 확인 버튼
    public void UserClearButton()
    {
        Debug.Log("button Test");
        Config.GameClear = false;
        ContinueButton.interactable = false;
    }

    //TODO: ClearUI 버튼 없애고 타이머 보여주는것으로 싱크 해결

    //게임 클리어 UI 활성화
    public void isGameClear()
    {
        GameClearUi.SetActive(true);
        StartCoroutine(SceneChange());
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
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(1f);
        GameClearUi.SetActive(true);
        yield return new WaitForSeconds(3f);
        Config.GameClear = false;
        Config.LodingSence = 2;
        SceneManager.LoadScene("LodingScene");
        

    }
}

