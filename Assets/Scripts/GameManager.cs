using UnityEngine;
using Newtonsoft.Json.Linq;
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
            MazeCell playerSpawnLocation = GameObject.Find("Maze").GetComponent<Maze>().Grid[0,0];

        }



        void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            Connection.ConnectedExit();
        }
    }
}