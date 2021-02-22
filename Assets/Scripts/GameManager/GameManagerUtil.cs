using UnityEngine;
using Newtonsoft.Json.Linq;
using Util;

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
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"),Vector3.zero,Quaternion.identity) as GameObject;
            playerCamera.name = Resources.Load("Player/CameraArm").name;
            
            // 순찰 npc
            GameObject AIPatrol = Instantiate(Resources.Load("Patrol/Patrol"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            AIPatrol.name = Resources.Load("Patrol/Patrol").name;

            // 플레이어, 맵, 함정, patrol point 생성
            GameObject Map = Instantiate(Resources.Load("Map/Map"),new Vector3(0,0,0),Quaternion.identity) as GameObject;
            Map.name = Resources.Load("Map/Map").name;
            
        }
    }
}