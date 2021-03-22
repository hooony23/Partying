using UnityEngine;
using Newtonsoft.Json.Linq;
using Communication;
using Util;

namespace GameManager
{
    public class GameManagerUtil : GameManagerController
    {
        protected void InitializeLabylinth()
        {
            // 플레이어에게 부착할 카메라 생성
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"),Vector3.zero,Quaternion.identity) as GameObject;
            playerCamera.name = Resources.Load("Player/CameraArm").name;
            
            // 순찰 npc
            GameObject AIPatrol = Instantiate(Resources.Load("Patrol/Patrol"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            AIPatrol.name = Resources.Load("Patrol/Patrol").name;

            // 플레이어, 맵, 함정, patrol point 생성
            GameObject Map = Instantiate(Resources.Load("Map/Map"),new Vector3(0,0,0),Quaternion.identity) as GameObject;
            Map.name = Resources.Load("Map/Map").name;
            
        }
        protected void DelUser()
        {
                while(NetworkInfo.connectedExitQueue.Count != 0)
                {
                    string userUuid = NetworkInfo.connectedExitQueue.Dequeue();
                    Destroy(GameObject.Find(userUuid));
                }
        }
        protected void DeathUser()
        {
                while(NetworkInfo.deathUserQueue.Count != 0)
                {
                    string userUuid = NetworkInfo.deathUserQueue.Dequeue();
                    GameObject player = GameObject.Find(userUuid);
                    if (Config.userUuid == userUuid)
                        player.GetComponent<Player>().IsDead = true;
                    else
                        player.GetComponent<OtherPlayer>().IsDead = true;
                    Debug.Log($"{userUuid} 가 죽었습니다!");
                    player.transform.rotation = Quaternion.Euler(new Vector3(45,0,0));
                }
        }
        protected void ClearGame()
        {
                while(NetworkInfo.GetItemUserQueue.Count != 0)
                {
                    string userUuid = NetworkInfo.GetItemUserQueue.Dequeue();
                    Debug.Log("Game Clear");
                    Application.Quit(0);
                }
        }
    }
}