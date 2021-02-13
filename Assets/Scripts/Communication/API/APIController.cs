using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Lib;


namespace Communication.API
{
    public class APIController : MonoBehaviour
    {
        public enum SendAPINames 
        {
            "Move"
        };
        public enum ReceiveAPINames
        {
            "SyncPackit"
        };

        public static void SendController(String type, String requestJson)
        {
            if (SendAPINames.Contains(type))
                Common.CallAPI(type, requestJson);
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
            }
            // RegularExpression.jsonValidation(responseJson);

            string type = responseJson.Value<string>("type");
            if (ReceiveAPINames.Contains(type))
                Common.CallAPI(type);
        }
    }
}