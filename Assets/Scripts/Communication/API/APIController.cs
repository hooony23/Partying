using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Lib;
using Util;


namespace Communication.API
{
    public class APIController : MonoBehaviour
    {


        public static void SendController(string server, String type, params String[] requestJson)
        {
            if (Enum.IsDefined(typeof(Config.SendAPINames),type))
                Common.CallAPI(server,"Send", type, requestJson);
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

            string type = responseJson.Value<string>("type");
            type = Common.ToPascalCase(type);
            string server = responseJson.Value<string>("server");
            string data = JsonConvert.SerializeObject((JObject)responseJson["data"]);
            if (Enum.IsDefined(typeof(Config.ReceiveAPINames),type))
                Common.CallAPI(server, "Receive",type, data);
        }
    }
}