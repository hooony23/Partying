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
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"),Vector3.zero,Quaternion.identity) as GameObject;
            playerCamera.name = Resources.Load("Player/CameraArm").name;
            GameObject player = Instantiate(Resources.Load("Player/Player"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            player.name = Config.userUuid;
            GameObject AIPatrol = Instantiate(Resources.Load("Patrol/Patrol"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            AIPatrol.name = Resources.Load("Patrol/Patrol").name;
            GameObject patrolPoint = new GameObject("PatrolPoint");
            patrolPoint.AddComponent<BoxCollider>();
            // MazeCell playerSpawnLocation = GameObject.Find("Map").GetComponent<Map>().Grid[0,0];
            patrolPoint.layer = LayerMask.NameToLayer("PatrolPoint");
            Vector3 firstGrid = new Vector3( - 0.5f, 1 + player.transform.localScale.y/2, 0.5f);
            player.transform.position = firstGrid;
            patrolPoint.transform.position = firstGrid + new Vector3(UnityEngine.Random.Range(0,20)*Config.labylinthOnSpaceSize/2,-UnityEngine.Random.Range(0,20)*Config.labylinthOnSpaceSize/2);
        }
    }
}