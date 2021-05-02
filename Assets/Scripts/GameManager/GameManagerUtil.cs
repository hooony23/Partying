using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Communication;
using Communication.MainServer;
using Communication.GameServer;
using Communication.GameServer.API;
using Util;

namespace GameManager
{
    public class GameManagerUtil : GameManagerController
    {
        protected void Start()
        {
            var ClearObject = Instantiate(Resources.Load("GameUi/GameClearUi")) as GameObject;
            GameClearUi = ClearObject.transform.Find("ClearUi").gameObject;
            ContinueButton = GameClearUi.transform.Find("GameClearButton").GetComponent<Button>();
            ContinueButton.onClick.AddListener(UserClearButton); 
            APIController.SendController("SyncStart");
        }
        protected void InitializeLabylinth()
        {   
            // 플레이어에게 부착할 카메라 생성
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"), Vector3.zero, Quaternion.identity) as GameObject;
            playerCamera.name = Resources.Load("Player/CameraArm").name;

            // 플레이어, 맵, 함정, 순찰 npc, patrol point 생성
            GameObject Map = Instantiate(Resources.Load("Labyrinth/Map/Map"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            Map.name = Resources.Load("Labyrinth/Map/Map").name;

        }
        protected void InitializeRaid()
        {
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"), Vector3.zero, Quaternion.identity) as GameObject;
            playerCamera.name = Resources.Load("Player/CameraArm").name;

            //TODO: Test용 소환 삭제해야함
            GameObject player = Instantiate(Resources.Load("Player/Player"), new Vector3(150,0,150), Quaternion.identity) as GameObject;
            player.name = Resources.Load("Player/Player").name;
            
            GameObject Map = Instantiate(Resources.Load("Raid/Map/RaidTerrain"), Vector3.zero, Quaternion.identity) as GameObject;
            Map.name = Resources.Load("Raid/Map/RaidTerrain").name;

            GameObject ItemManager = Instantiate(Resources.Load("Raid/Item/ItemManager"), Vector3.zero, Quaternion.identity) as GameObject;
            ItemManager.name = Resources.Load("Raid/Item/ItemManager").name;
            
            InitUserList();
            GameObject Boss = Instantiate(Resources.Load("Raid/Boss/BossPrefab/Boss"), new Vector3(150,26,150), Quaternion.identity) as GameObject;
            Boss.name = Resources.Load("Raid/Boss/BossPrefab/Boss").name;
        }
        protected void DelUser()
        {
            while (NetworkInfo.connectedExitQueue.Count != 0)
            {
                string userUuid = NetworkInfo.connectedExitQueue.Dequeue();
                Destroy(GameObject.Find(userUuid));
            }
        }
        protected void DeathUser()
        {
            while (NetworkInfo.deathUserQueue.Count > 0)
            {
                string userUuid = NetworkInfo.deathUserQueue.Dequeue();
                GameObject player = GameObject.Find(userUuid);
                player.GetComponent<Player>().IsDead = true;
                Debug.Log($"{userUuid} 가 죽었습니다!");
                player.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
            }
        }
        protected void ClearGame()
        {
            switch(currentStage)
            {
                case 1:
                    Stage1Clear();
                    break;
                case 2:
                    Stage2Clear();
                    break;
            }
        }
        protected void Stage1Clear()
        {
            
            if (NetworkInfo.GetItemUserQueue.Count != 0)
            {
                gameClear = true;
                string userUuid = NetworkInfo.GetItemUserQueue.Dequeue();
                IsGameClear();
                // Lib.Common.WaitThenCallback(1f,);
                Debug.Log("Game Clear");

            }
        }
        protected void Stage2Clear()
        {
            
                if (GameObject.Find("Boss")==null)
                {
                    gameClear=true;
                    IsGameClear();
                }
        }
        protected void InitUserList()
        {
            if (NetworkInfo.playersInfo.Count > 0)
            {
                foreach (var playerUuid in NetworkInfo.playersInfo.Keys)
                {
                    Debug.Log(playerUuid);
                    PlayerList.Add(GameObject.Find(playerUuid));
                }
            }
        }
        protected void UpdateUserList()
        {
            if (NetworkInfo.deathUserQueue.Count > 0)
            {
                foreach (var deathUserUuid in NetworkInfo.deathUserQueue)
                {
                    PlayerList.Remove(GameObject.Find(deathUserUuid));
                    DeathPlayerList.Add(GameObject.Find(deathUserUuid));
                }
            }
        }
        protected void UpdateBossInfo()
        {
            if (NetworkInfo.bossInfo !=null)
            {
                var boss = GameObject.Find("Boss");//.GetComponent<Boss.Boss>()
                boss.transform.position = NetworkInfo.bossInfo.GetLocToVector3();
                boss.GetComponent<Rigidbody>().velocity = NetworkInfo.bossInfo.GetVecToVector3();
                // if(boss.GetComponent<Boss.Boss>().BossHP!= NetworkInfo.bossInfo.BossHP)
                // {
                //     boss.Animator.SetTriger("Hit");
                // }
                var bossInfo = boss.GetComponent<Boss.Boss>();
                bossInfo.Pattern = NetworkInfo.bossInfo.pattern;
                bossInfo.BossHP =NetworkInfo.bossInfo.BossHP;
                NetworkInfo.bossInfo = null;
            }
        }
    //게임 클리어 UI 활성화
    public void IsGameClear()
    {
        GameClearUi.SetActive(true);
    }
    public void UserClearButton()
    {
        ContinueButton.interactable = false;
        SceneManager.LoadScene("LodingScene");
    }
        protected virtual void Update()
        {
            ClearGame();
        }

        protected virtual void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            MServer.LeaveRoom(NetworkInfo.roomInfo.RoomUuid);
            MServer.SignOut();
            Connection.ConnectedExit();
        }
    }
}