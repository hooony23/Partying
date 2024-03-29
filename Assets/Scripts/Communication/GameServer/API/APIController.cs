using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Lib;
using Util;


namespace Communication.GameServer.API
{
    public class APIController : MonoBehaviour
    {

        public static void SendController(String type, params object[] requestJson)
        {
            Common.CallAPI("Send", type, requestJson);
        }
        
        public static void ReceiveController(String json)
        {
            JObject responseJson = null;
            try
            {
                responseJson = JObject.Parse(json);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
            // RegularExpression.jsonValidation(responseJson);

            string type = Common.ToPascalCase(responseJson.Value<string>("type"));
            string data = responseJson["data"].ToString();
            Common.CallAPI("Receive", type, data);
        }
    }
}