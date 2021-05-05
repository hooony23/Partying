using UnityEngine;
using Newtonsoft.Json.Linq;
using Communication;
using Util;
using GameUi;

namespace GameManager
{
    public class GameManagerUtil : GameManagerController
    {
        protected void SetUserUuid(string response)
        {
            JObject responseJson = JObject.Parse(response);
            Config.userUuid = responseJson["data"].Value<string>("uuid");
        }
        protected void InitializeLabylinth()
        {   
            // 플레이어에게 부착할 카메라 생성
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"), Vector3.zero, Quaternion.identity) as GameObject;
            playerCamera.name = Resources.Load("Player/CameraArm").name;
            /*
            // 순찰 npc
            GameObject AIPatrol = Instantiate(Resources.Load("Labyrinth/Patrol/Patrol"), new Vector3(0, 3, 0), Quaternion.identity) as GameObject;
            AIPatrol.name = Resources.Load("Labyrinth/Patrol/Patrol").name;
            */
            // 플레이어, 맵, 함정, patrol point 생성
            GameObject Map = Instantiate(Resources.Load("Labyrinth/Map/Map"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            Map.name = Resources.Load("Labyrinth/Map/Map").name;
            this.gameObject.AddComponent<UserScore>();

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
            while (NetworkInfo.deathUserQueue.Count != 0)
            {
                string userUuid = NetworkInfo.deathUserQueue.Dequeue();
                PlayerList.Remove(GameObject.Find(userUuid));
                DeathPlayerList.Add(GameObject.Find(userUuid));
                GameObject player = GameObject.Find(userUuid);
                Debug.Log($"{userUuid} 가 죽었습니다!");
                player.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
            }
        }
        protected void ClearGame()
        {
            while (NetworkInfo.GetItemUserQueue.Count != 0)
            {
                string userUuid = NetworkInfo.GetItemUserQueue.Dequeue();
                Debug.Log("Game Clear");
                Application.Quit(0);
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
    }
}