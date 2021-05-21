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

        protected virtual void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            APIController.SendController("ConnectedExit");
            MServer.LeaveRoom(NetworkInfo.roomInfo.RoomUuid);
            MServer.SignOut();
        }
        protected void SetGameUi()
        {
            var ClearObject = Instantiate(Resources.Load("GameUi/GameClearUi")) as GameObject;
            GameClearUi = ClearObject.transform.Find("ClearUi").gameObject;
            GameOverUi = Instantiate(Resources.Load("GameUi/OverUi")) as GameObject;
            GameOverUi.name = Resources.Load("GameUi/OverUi").name;
            ContinueButton = GameClearUi.transform.Find("GameClearButton").GetComponent<Button>();
            ContinueButton.onClick.AddListener(UserClearButton);
        }
        protected void SetChatting()
        {
            var chatUi = Instantiate(Resources.Load("GameUi/Chat/ChatUi")) as GameObject;
        }
        protected void SetCurrentStage(int currentStage)
        {
            this.currentStage = currentStage;
        }
        protected void SpawnCamera()
        {
            PlayerCamera = Instantiate(Resources.Load("Player/CameraArm2"), Vector3.zero, Quaternion.identity) as GameObject;
            PlayerCamera.name = Resources.Load("Player/CameraArm2").name;
        }
        protected void InitializeLabylinth()
        {
            // 플레이어에게 부착할 카메라 생성
            GameObject Map = Instantiate(Resources.Load("Labyrinth/Map/Map"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            Map.name = Resources.Load("Labyrinth/Map/Map").name;
        }
        protected void InitializeRaid()
        {
            var stage2Info = Communication.JsonFormat.InitStage2.GetInitStage2();
            NetworkInfo.bossInfo = stage2Info.BossInfo;

            GameObject Map = Instantiate(Resources.Load("Raid/Map/Raidmap"), Vector3.zero, Quaternion.identity) as GameObject;
            Map.name = Resources.Load("Raid/Map/RaidTerrain").name;
            GameObject Boss = Instantiate(Resources.Load("Raid/Boss/BossPrefab/Boss"), stage2Info.BossInfo.GetLocToVector3(), Quaternion.identity) as GameObject;
            Boss.name = Resources.Load("Raid/Boss/BossPrefab/Boss").name;
            GameObject ItemManager = Instantiate(Resources.Load("Raid/Item/ItemManager"), Vector3.zero, Quaternion.identity) as GameObject;
            ItemManager.name = Resources.Load("Raid/Item/ItemManager").name;
            SpawnPlayer(stage2Info.PlayerLocs);
        }
        protected void SpawnPlayer(CellInfo[] players)
        {
            foreach (var playerInfo in players)
            {
                GameObject player = Instantiate(Resources.Load("Player/Player"), new Vector3(playerInfo.col, 5, playerInfo.row), Quaternion.identity) as GameObject;
                player.name = playerInfo.data.ToString();
                player.GetComponent<Player>().UserUuid = player.name;
            }
        }
        protected void DelUser()
        {
            while (NetworkInfo.connectedExitQueue.Count != 0)
            {
                string userUuid = NetworkInfo.connectedExitQueue.Dequeue();
                var exitUser = GameObject.Find(userUuid);
                if (PlayerList.Contains(exitUser))
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
                Debug.Log($"remain death user count : {NetworkInfo.deathUserQueue.Count}");
                string userUuid = NetworkInfo.deathUserQueue.Dequeue();
                var deathUser = GameObject.Find(userUuid);
                deathUser.GetComponent<Player>().IsDead = true;
                PlayerList.Remove(deathUser);
                DeathPlayerList.Add(deathUser);
                Debug.Log($"{userUuid} 가 죽었습니다!");
                Debug.Log($"Remain User Count : {PlayerList.Count}");
                Debug.Log($"Death User Count : {DeathPlayerList.Count}");
            }
            if(PlayerList.Count<=0)
                GameOver = true;
        }
        protected void ClearGame()
        {
            if (!GameClear)
                return;
            Cursor.visible = true;
            GameClearUi.SetActive(true);
            Debug.Log("Game Clear");
            SoundManager.instance.IsPlaySfxSound("StageClearSound");
            GameClear = false;
        }
        protected void OverGame()
        {
            if(!GameOver)
                return;
            Cursor.visible = true;
            Debug.Log("Game Over");
            GameOverUi.GetComponent<OverUi>().GameOverUi();
        }
        protected void InitUserList()
        {
            if (NetworkInfo.playersInfo.Count > 0)
            {
                foreach (var playerUuid in NetworkInfo.playersInfo.Keys)
                {
                    Debug.Log($"{playerUuid} is find? : {GameObject.Find(playerUuid)!=null}");
                    PlayerList.Add(GameObject.Find(playerUuid));
                }
                NetworkInfo.playersInfo.Clear();
            }
        }
        public void UserClearButton()
        {
            ContinueButton.interactable = false;
            SoundManager.instance.StopBgmSound();
            SceneManager.LoadScene("LodingScene");
        }
        public GameObject GetPlayerGameObject(string userUuid)
        {
            foreach (var gameObject in PlayerList)
            {
                if (gameObject.name.Equals(userUuid))
                {
                    return gameObject;
                }
            }
            return null;
        }
    }
}