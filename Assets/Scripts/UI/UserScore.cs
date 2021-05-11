
using Communication;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace GameUi
{
    public class UserScore : MonoBehaviour
    {
        //스코어 목록 동적생성
        private GameObject infoPanel, textObject, scrollPanel, otherUserLive;
        private static GameObject playerHp, playerBuff;
        private Player player;

        //유저 스코어창 오픈 애니메이션
        private Animator animator;
        private bool userInfoOpen = false;

        //예시) 받아올 유저들의 이름과 점수
        private string playerName = "Player";
        private Dictionary<string, string> UserLive = new Dictionary<string, string>();

        //TODO: 스코어 관련 여부 보류
        //List<UserScoreInfo> userInfo = new List<UserScoreInfo>();
        GameManager.GameManager gameManager;


        public void Awake()
        {

            GameObject UserScoreInfoObject = Instantiate(Resources.Load("GameUi/UserUi")) as GameObject;
            UserScoreInfoObject.name = Resources.Load("GameUi/UserUi").name;
            infoPanel = UserScoreInfoObject.transform.Find("UsersScoreUi").gameObject;
            textObject = Resources.Load("GameUi/UserText") as GameObject;
            GameObject UserInfoScrollView = infoPanel.transform.Find("UserInfoScroll View").transform.Find("Viewport").gameObject;
            scrollPanel = UserInfoScrollView.transform.Find("Content").gameObject;
            GameObject UserUi = UserScoreInfoObject.transform.Find("UserInfoUi").gameObject;
            playerHp = UserUi.transform.Find("PlayerHp").gameObject;
            playerBuff = UserUi.transform.Find("PlayerBuff").gameObject;
            otherUserLive = UserScoreInfoObject.transform.Find("OtherUser").gameObject;
            animator = infoPanel.GetComponent<Animator>();


        }
        public void Start()
        {

            //Debug.Log(userInfo1);
            //예시UserLive.Add("kevin", "live");
            UserLive.Add("Player", "live");
            UserLive.Add("tarios", "live");
            UserLive.Add("resviosa", "live");

            //유저 스코어 갱신
            //TODO: 스코어 관련 여부 보류
            //SendUserScore();
            Debug.Log($"My uuid :{Config.userUuid}");
            player = GameObject.Find(Config.userUuid).GetComponent<Player>();
            Config.LodingSence = 2;
            if (Config.defaultStage == 2 && Config.LodingSence == 2)
            {
                Debug.Log(Config.defaultStage);
                PlayerHeart(player);
            }

            gameManager = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();

            foreach (KeyValuePair<string, string> kvp in UserLive)
            {
                string UserName = kvp.Key;
                //Debug.Log(UserLive.ContainsKey(kvp.Key));
                if (!UserName.Equals(playerName))
                {
                    GameObject UserliveInfo = Instantiate(Resources.Load("GameUi/UserLive"), otherUserLive.transform) as GameObject;
                    UserliveInfo.name = UserName;
                    Text UserliveText = UserliveInfo.transform.Find("Text").GetComponent<Text>();
                    UserliveText.text = UserName;
                }

            }
        }
        public void Update()
        {
            //게임 시작시 오픈가능
            if (Config.StartGame)
            {
                OpenPanel();
            }
            //OtherUserlive();
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
        public void PlayerHeart(Player player)
        {
            Player playUser = player.gameObject.GetComponent<Player>();
            float currntHp = playUser.PlayerHealth;
            float maxHp = playUser.PlayerMaxHealth;
            if (0 < playerHp.transform.childCount)
            {
                for (int i = 0; i < playerHp.transform.childCount; i++)
                {
                    Destroy(playerHp.transform.GetChild(i).gameObject);
                    Debug.Log("하트 초기화");
                }
            }
            for (int i = 0; i < maxHp; i++)
            {
                if (i < currntHp)
                {
                    GameObject Heart = Instantiate(Resources.Load("GameUi/IconPrefabs/Heart"), playerHp.transform) as GameObject;
                    Debug.Log("하트생성");
                }
                else
                {
                    GameObject EmptyHeart = Instantiate(Resources.Load("GameUi/IconPrefabs/EmptyHeart"), playerHp.transform) as GameObject;
                    Debug.Log("빈하트 생성");
                }
            }
        }
        //TODO: 스코어 사용 여부에 따라 삭제
        /*public void SendUserScore()
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
        }*/
        public void OtherUserlive()
        {
            if (NetworkInfo.deathUserQueue.Count != 0)
            {
                Debug.Log("죽음 출력");
                foreach (GameObject name in gameManager.DeathPlayerList)
                {
                    if (UserLive.ContainsKey(name.name))
                    {
                        Debug.Log("죽은 유저 확인");
                        UserLive[name.name] = ("dead");

                    }
                }
                if (UserLive.ContainsValue("dead"))
                {
                    foreach (KeyValuePair<string, string> a in UserLive)
                    {
                        Image User = GameObject.Find(a.Key).GetComponent<Image>();
                        if (a.Value.Equals("dead"))
                        {
                            Debug.Log("죽은 유저 확인2222");
                            User.color = new Color32(130, 130, 130, 255);
                        }
                        else
                        {
                            Debug.Log("생존 확인");
                            User.color = new Color32(0, 0, 0, 255);
                        }
                    }
                }
            }
        }
    }
}
