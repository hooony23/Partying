using UnityEngine;
using Newtonsoft.Json.Linq;
using Partying.Assets.Scripts.Util;


public class NetWorkManager : MonoBehaviour
{
    
    void Awake()
    {
        // 서버로부터 uuid 받아옴
        string response = Connection.Connected();
        JObject responseJson = JObject.Parse(response);
        Config.userUuid = responseJson["data"].Value<string>("uuid");
    }

    

    private void OnApplicationQuit()
    {
        /* 서버 연결 해제 */
        Connection.ConnectedExit();
        
    }
}