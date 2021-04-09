using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Util;


namespace Communication.GameServer.API.Labylinth
{
    public class Receive : Controller
    {
        public void CreateMap(string response)
        {
            MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(response);

            //TODO 더 좋은 방법 찾아야함
            NetworkInfo.mapInfo = mapInfo;

        }
        public void SyncPacket(string response)
        {
            JObject responseJson = null;
            try
            {
                responseJson = JObject.Parse(response);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
            JArray usersInfo = responseJson.Value<JArray>("usersInfo");
            foreach (JObject data in usersInfo)
            {
                PlayerInfo userInfo = data.ToObject<PlayerInfo>();
                if (!userInfo.uuid.Equals(Config.userUuid))
                {
                    try
                    {
                        NetworkInfo.playersInfo.Add(userInfo.uuid, userInfo);
                    }
                    catch (ArgumentException)
                    {
                        NetworkInfo.playersInfo[userInfo.uuid] = userInfo;
                    }

                }
            }

        }
        public void Death(string response)
        {

            JObject responseJson = null;
            try
            {
                responseJson = JObject.Parse(response);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
            string deathUserUuid = responseJson.Value<string>("uuid");
            NetworkInfo.deathUserQueue.Enqueue(deathUserUuid);
        }
        public void Connected(string response)
        {
            
        }
        public void ConnectedExit(string response)
        {

            JObject responseJson = null;
            try
            {
                responseJson = JObject.Parse(response);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
            string deathUserUuid = responseJson.Value<string>("uuid");
            NetworkInfo.connectedExitQueue.Enqueue(deathUserUuid);
        }
        public void GetItem(string response)
        {

            JObject responseJson = null;
            try
            {
                responseJson = JObject.Parse(response);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
            string isGetUserUuid = responseJson.Value<string>("uuid");
            NetworkInfo.GetItemUserQueue.Enqueue(isGetUserUuid);
        }
    }
}