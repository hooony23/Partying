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
    private bool uiActive = false;
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
        //TODO: Test할때는 아래, 사용시는 위의 주석을 지울것.
        //player = GameObject.Find(Config.userUuid.ToString()).GetComponent<Player>();
        player = GameObject.Find(Config.userUuid).GetComponent<Player>();
    }
    private void Update()
    {
        if (GM.PlayerList.Count<=0&&!uiActive) {
            uiActive = true;
            Invoke("GameOverUi", 2f);
        }
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
        SoundManager.instance.IsStopSound("stage"+(Config.defaultStage).ToString());
        Config.defaultStage=99;
        SceneManager.LoadScene("LodingScene");
    }
}
