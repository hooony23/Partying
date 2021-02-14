using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Lib;
using Util;


namespace Communication.API
{
    public class APIController : MonoBehaviour
    {


        public static void SendController(string server, String type, String requestJson)
        {
            if (Enum.IsDefined(typeof(Config.SendAPINames),type))
                Common.CallAPI(server, type, requestJson);
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
            string server = responseJson.Value<string>("server");
            string data = responseJson.Value<string>("data");
            if (Enum.IsDefined(typeof(Config.ReceiveAPINames),type))
                Common.CallAPI(server, type, responseJson);
        }
    }
}