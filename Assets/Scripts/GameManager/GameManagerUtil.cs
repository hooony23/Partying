using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Communication;
using Communication.JsonFormat;
using Communication.MainServer;
using Communication.GameServer;
using Communication.GameServer.API;
using Util;
using GameUi;

namespace GameManager
{
    public class GameManagerUtil : GameManagerController
    {
        protected void Start()
        {
            currentStage=Config.defaultStage;
            
            Debug.Log($"current stage : {currentStage}");
            var ClearObject = Instantiate(Resources.Load("GameUi/GameClearUi")) as GameObject;
            GameClearUi = ClearObject.transform.Find("ClearUi").gameObject;
            var OverObject = Instantiate(Resources.Load("GameUi/OverUi")) as GameObject;
            ContinueButton = GameClearUi.transform.Find("GameClearButton").GetComponent<Button>();
            ContinueButton.onClick.AddListener(UserClearButton); 
            APIController.SendController("SyncStart");
        }
        protected void InitializeLabylinth()
        {   
            // 플레이어에게 부착할 카메라 생성
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"), Vector3.zero, Quaternion.identity) as GameObject;
            playerCamera.name = Resources.Load("Player/CameraArm").name;
            GameObject Map = Instantiate(Resources.Load("Labyrinth/Map/Map"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            InitUserList();
            Map.name = Resources.Load("Labyrinth/Map/Map").name;
            this.gameObject.AddComponent<UserScore>();

        }
        protected void InitializeRaid()
        {
            var stage2Info = Communication.JsonFormat.InitStage2.GetInitStage2();
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"), Vector3.zero, Quaternion.identity) as GameObject;
            playerCamera.name = Resources.Load("Player/CameraArm").name;
            NetworkInfo.bossInfo = stage2Info.BossInfo;
            //TODO: Test용 소환 삭제해야함
            foreach(var playerInfo in stage2Info.PlayerLocs)
            {
                GameObject player = Instantiate(Resources.Load("Player/Player"), new Vector3(playerInfo.col,5,playerInfo.row), Quaternion.identity) as GameObject;
                player.name = playerInfo.data.ToString();
            }
            
            GameObject Map = Instantiate(Resources.Load("Raid/Map/Raidmap"), Vector3.zero, Quaternion.identity) as GameObject;
            Map.name = Resources.Load("Raid/Map/RaidTerrain").name;

            GameObject ItemManager = Instantiate(Resources.Load("Raid/Item/ItemManager"), Vector3.zero, Quaternion.identity) as GameObject;
            ItemManager.name = Resources.Load("Raid/Item/ItemManager").name;
            
            InitUserList();
            GameObject Boss = Instantiate(Resources.Load("Raid/Boss/BossPrefab/Boss"), stage2Info.BossInfo.GetLocToVector3(), Quaternion.identity) as GameObject;
            Boss.name = Resources.Load("Raid/Boss/BossPrefab/Boss").name;
        }
        protected void DelUser()
        {
            while (NetworkInfo.connectedExitQueue.Count != 0)
            {
                string userUuid = NetworkInfo.connectedExitQueue.Dequeue();
                var exitUser = GameObject.Find(userUuid);
                if(PlayerList.Contains(exitUser))
                    PlayerList.Remove(exitUser);
                else
                    DeathPlayerList.Remove(exitUser);
                Destroy(exitUser);
            }
        }
        protected void DeathUser()
        {
            while (NetworkInfo.deathUserQueue.Count > 0)
            {
                string userUuid = NetworkInfo.deathUserQueue.Dequeue();
                var deathUser = GameObject.Find(userUuid);
                deathUser.GetComponent<Player>().IsDead=true;
                PlayerList.Remove(deathUser);
                DeathPlayerList.Add(deathUser);
                Debug.Log($"{userUuid} 가 죽었습니다!");
            }
        }
        protected void ClearGame()
        {
           if(GameClear)
           {
               
                IsGameClear();
                // Lib.Common.WaitThenCallback(1f,);
                Debug.Log("Game Clear");
                GameClear=false;
           }
        }
        protected void InitUserList()
        {
            PlayerList.Add(GameObject.Find(Config.userUuid));
            if (NetworkInfo.playersInfo.Count > 0)
            {
                foreach (var playerUuid in NetworkInfo.playersInfo.Keys)
                {
                    Debug.Log(playerUuid);
                    PlayerList.Add(GameObject.Find(playerUuid));
                }
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
    public GameObject GetPlayerGameObject(string userUuid)
    {
        foreach(var gameObject in PlayerList)
        {
            if(gameObject.name.Equals(userUuid))
            {
                return gameObject;
            }
        }
        return null;
    }
        protected virtual void Update()
        {
            DelUser();
            DeathUser();
            ClearGame();
        }

        protected virtual void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            APIController.SendController("ConnectedExit");
            MServer.LeaveRoom(NetworkInfo.roomInfo.RoomUuid);
            MServer.SignOut();
        }
    }
}