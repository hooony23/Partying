using UnityEngine;
using Newtonsoft.Json.Linq;
using Lib;
using Util;

namespace Communication
{

    public class GameManager : MonoBehaviour
    {

        void Awake()
        {
            string response = Connection.Connected();
            JObject responseJson = JObject.Parse(response);
            Config.userUuid = responseJson["data"].Value<string>("uuid");
            MazeCell playerSpawnLocation = GameObject.Find("Map").GetComponent<Map>().Grid[0,0];
            GameObject Map = Instantiate(Resources.Load("Map/Map"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            GameObject AIPatrol = Instantiate(Resources.Load("Patrol/Patrol"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            GameObject player = Instantiate(Resources.Load("Player/Player"),new Vector3(0,3,0),Quaternion.identity) as GameObject;
            GameObject patrolPoint = new GameObject("PatrolPoint");
            Map.name = Resources.Load("Map/Map").name;
            AIPatrol.name = Resources.Load("Patrol/Patrol").name;
            player.name = Resources.Load("Player/Player").name;
            patrolPoint.AddComponent<BoxCollider>();
            patrolPoint.layer = LayerMask.NameToLayer("PatrolPoint");
            Vector3 firstGrid = new Vector3( - 0.5f, 1 + player.transform.localScale.y/2, 0.5f);
            player.transform.position = firstGrid;
            patrolPoint.transform.position = firstGrid + new Vector3(UnityEngine.Random.Range(0,20)*Config.labylinthOnSpaceSize/2,-UnityEngine.Random.Range(0,20)*Config.labylinthOnSpaceSize/2);
        }

        

        void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            Connection.ConnectedExit();
        }
    }
}