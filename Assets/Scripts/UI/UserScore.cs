using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UserScore : MonoBehaviour
{
    //스코어 목록 동적생성
    private GameObject InfoPanel, textObject, scrollPanel;
    Text IsPlayerScore;


    //유저 스코어창 오픈 애니메이션
    private Animator animator;
    private bool userInfoOpen = false;

    //예시) 받아올 유저들의 이름과 점수
    private string playername = "sunsub";
    [SerializeField] List<UserScoreInfo> userInfo = new List<UserScoreInfo>();
    UserScoreInfo userInfo1 = new UserScoreInfo("kevin", 0);
    UserScoreInfo userInfo2 = new UserScoreInfo("sunsub", 0);
    UserScoreInfo userInfo3 = new UserScoreInfo("tarios", 100);
    UserScoreInfo userInfo4 = new UserScoreInfo("resviosa", 0);

    public void Awake()
    {
        GameObject UserScoreInfoObject = Instantiate(Resources.Load("GameUi/UserUi")) as GameObject;
        InfoPanel = UserScoreInfoObject.transform.Find("UsersScoreUi").gameObject;
        textObject = Resources.Load("GameUi/UserText") as GameObject;
        GameObject UserInfoScrollView = GameObject.Find("UserInfoScroll View").transform.Find("Viewport").gameObject;
        scrollPanel = UserInfoScrollView.transform.Find("Content").gameObject;
        GameObject UserUi = UserScoreInfoObject.transform.Find("UserInfoUi").gameObject;
        IsPlayerScore = UserUi.transform.Find("IsPlayerScore").GetComponent<Text>();
        animator = InfoPanel.GetComponent<Animator>();
    }
    public void Start()
    {
        Debug.Log(userInfo1);
        //예시
        userInfo.Add(userInfo1);
        userInfo.Add(userInfo2);
        userInfo.Add(userInfo3);
        userInfo.Add(userInfo4);

        //유저 스코어 갱신
        SendUserScore();

    }
    public void Update()
    {
        //게임 시작시 오픈가능
        if (Config.StartGame)
        {
            OpenPanel();
        }
    }
    //Tap키를 이용한 유저스코어창 오픈
    public void OpenPanel()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            userInfoOpen = animator.GetBool("UserInfoOpen");
            animator.SetBool("UserInfoOpen", userInfoOpen);

        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            userInfoOpen = animator.GetBool("UserInfoOpen");
            animator.SetBool("UserInfoOpen", !userInfoOpen);

        }
    }
    public void SendUserScore()

    {
        for (int i = 0; i < userInfo.Count; i++)
        {
            //각 유저들의 정보를 받아옴
            string UserName = userInfo[i].Text;
            int userScore = userInfo[i].Score;
            string ScoreResult = string.Format("{0} : {1}", UserName, userScore);

            //현재 플레이중인 유저 스코어 갱신
            if (UserName.Equals(playername))
            {
                IsMyScore(UserName, userScore);
            }
            //유저 스코어 동적생성
            GameObject ScoreTextObject = Instantiate(textObject, scrollPanel.transform);
            Text ScoreText = ScoreTextObject.GetComponent<Text>();
            ScoreText.text = ScoreResult;
        }
    }
    public void IsMyScore(string name, int score)
    {
        string ScoreResult = string.Format("{0} \nScore : {1}", name, score);
        IsPlayerScore.text = ScoreResult;
    }
}

