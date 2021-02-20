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
            GameObject player = Instantiate(Resources.Load("Player/Player"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            GameObject playerCamera = Instantiate(Resources.Load("Player/CameraArm"),Vector3.zero,Quaternion.identity) as GameObject;
            GameObject AIPatrol = Instantiate(Resources.Load("Patrol/Patrol"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            GameObject patrolPoint = new GameObject("PatrolPoint");
            AIPatrol.name = Resources.Load("Patrol/Patrol").name;
            player.name = Resources.Load("Player/Player").name;
            playerCamera.name = Resources.Load("Player/CameraArm").name;
            patrolPoint.AddComponent<BoxCollider>();
            // MazeCell playerSpawnLocation = GameObject.Find("Map").GetComponent<Map>().Grid[0,0];
            patrolPoint.layer = LayerMask.NameToLayer("PatrolPoint");
            Vector3 firstGrid = new Vector3( - 0.5f, 1 + player.transform.localScale.y/2, 0.5f);
            player.transform.position = firstGrid;
            patrolPoint.transform.position = firstGrid + new Vector3(UnityEngine.Random.Range(0,20)*Config.labylinthOnSpaceSize/2,-UnityEngine.Random.Range(0,20)*Config.labylinthOnSpaceSize/2);
        }
    }
}