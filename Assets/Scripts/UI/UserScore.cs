
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace GameUi
{
    public class UserScore : MonoBehaviour
    {
        //스코어 목록 동적생성
        private GameObject infoPanel, textObject, scrollPanel;
        private static GameObject playerHp;
        Player player;

        //유저 스코어창 오픈 애니메이션
        private Animator animator;
        private bool userInfoOpen = false;

        //예시) 받아올 유저들의 이름과 점수
        private string playername = "sunsub";
        List<UserScoreInfo> userInfo = new List<UserScoreInfo>();
        UserScoreInfo userInfo1 = new UserScoreInfo("kevin", 0);
        UserScoreInfo userInfo2 = new UserScoreInfo("sunsub", 0);
        UserScoreInfo userInfo3 = new UserScoreInfo("tarios", 100);
        UserScoreInfo userInfo4 = new UserScoreInfo("resviosa", 0);

        public void Awake()
        {
            GameObject UserScoreInfoObject = Instantiate(Resources.Load("GameUi/UserUi")) as GameObject;
            infoPanel = UserScoreInfoObject.transform.Find("UsersScoreUi").gameObject;
            textObject = Resources.Load("GameUi/UserText") as GameObject;
            GameObject UserInfoScrollView = infoPanel.transform.Find("UserInfoScroll View").transform.Find("Viewport").gameObject;
            scrollPanel = UserInfoScrollView.transform.Find("Content").gameObject;
            GameObject UserUi = UserScoreInfoObject.transform.Find("UserInfoUi").gameObject;
            playerHp = UserUi.transform.Find("PlayerHp").gameObject;
            animator = infoPanel.GetComponent<Animator>();

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
            //TODO: 테스트용이 아닌 현재 사용중인 유저로 변경할것.
            player = GameObject.Find("Player").GetComponent<Player>();

            PlayerHeart(player);
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
        //TODO: 유저 스코어 키 설정, 현재 체력 갱신하며 화면.
        public void OpenPanel()//User Score의 약자의 U를 사용
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                userInfoOpen = animator.GetBool("UserInfoOpen");
                animator.SetBool("UserInfoOpen", userInfoOpen);

            }
            if (Input.GetKeyUp(KeyCode.U))
            {
                userInfoOpen = animator.GetBool("UserInfoOpen");
                animator.SetBool("UserInfoOpen", !userInfoOpen);

            }
        }
        public void  PlayerHeart(Player player)
        {
            Player playUser = player.gameObject.GetComponent<Player>();
            float currntHp = playUser.PlayerHealth;
            float maxHp = playUser.PlayerMaxHealth;
            if (0 < playerHp.transform.childCount){
                for (int i = 0; i < playerHp.transform.childCount; i++) {
                    Destroy(playerHp.transform.GetChild(i).gameObject);
                    Debug.Log("하트 초기화");
                }
            }
            for (int i = 0; i < maxHp; i++)
            {
                if (i < currntHp)
                {
                    GameObject Heart = Instantiate(Resources.Load("GameUi/Heart"),playerHp.transform)as GameObject;
                    Debug.Log("하트생성");
                }
                else
                {
                    GameObject EmptyHeart = Instantiate(Resources.Load("GameUi/EmptyHeart"), playerHp.transform) as GameObject;
                    Debug.Log("빈하트 생성");
                }
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

                //유저 스코어 동적생성
                GameObject ScoreTextObject = Instantiate(textObject, scrollPanel.transform);
                Text ScoreText = ScoreTextObject.GetComponent<Text>();
                ScoreText.text = ScoreResult;
            }
        }
    }
}
