using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
using Communication;
using GameManager;

public class OverUi : MonoBehaviour
{
    private GameObject uiObject;
    private GameManager.GameManager GM;
    private Button overUiButton;
    private Text buttonText, OveruiText;
    public bool UiActive{get;set;} = false;
    Player player;
    public void Awake()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
        uiObject = this.transform.GetChild(0).gameObject;
        OveruiText = uiObject.transform.Find("OverUiText").GetComponent<Text>();
        overUiButton = uiObject.transform.Find("Button").GetComponent<Button>();
        buttonText = overUiButton.transform.GetChild(0).GetComponent<Text>();
    }
    public void Start()
    {
        player = GameObject.Find(Config.userUuid).GetComponent<Player>();
    }
    public void UesrDeadUi() {
        Cursor.visible = true;
        uiObject.SetActive(true);
        OveruiText.color = new Color32(243, 102, 77, 255);
        OveruiText.text = "Game Over";
        buttonText.text = "OtherPlayer";
        overUiButton.onClick.AddListener(UserDeadButton);
    }
    public void GameOverUi() {
        Cursor.visible = true;
        uiObject.SetActive(true);
        OveruiText.color = new Color32(50, 173, 221, 255);
        OveruiText.text = "- Game Over -";
        buttonText.text = "BackRoom";
        overUiButton.onClick.AddListener(GameOverButton);
    }
    public void UserDeadButton() {
        Destroy(this);
        //TODO: 유저 시점 변환하는 변수나 함수 참조필요
    }
    public void GameOverButton() {
        Config.defaultStage=99;
        overUiButton.interactable=false;
        SoundManager.instance.StopBgmSound();
        SceneManager.LoadScene("LodingScene");
    }
}
