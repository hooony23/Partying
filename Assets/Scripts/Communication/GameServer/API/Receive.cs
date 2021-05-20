using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Util;
using System.Globalization;


namespace Communication.GameServer.API
{
    public class Receive : Controller
    {
        public void CreateMap(string response)
        {
            MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(response);
            foreach(var playerInfo in mapInfo.playerLocs)
            {
                NetworkInfo.playersInfo[playerInfo.data.ToString()] = null;
            }
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
        public void SyncAiPacket(string response)
        {
            Debug.Log(response);
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
            NetworkInfo.aiInfo = ((JObject)responseJson["data"]).ToObject<AiInfo>();

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
            string exitUserUuid = responseJson.Value<string>("uuid");
            NetworkInfo.connectedExitQueue.Enqueue(exitUserUuid);
        }
        public void GetItem(string response)
        {
            Debug.Log(response);
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
        public void SpawnItem(string response)
        {
            Debug.Log(response);
            ItemInfo responseJson = null;
            try
            {
                responseJson = JsonConvert.DeserializeObject<ItemInfo>(response);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
            ItemInfo.InitItemInfo(responseJson);
        }
        public void SyncStart(string response)
        { 
            Debug.Log(response);
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
            if(Config.defaultStage==1)
            {
                NetworkInfo.finishTime = responseJson.Value<double>("finishTime");
            }
            NetworkInfo.startTime = responseJson.Value<double>("startTime");
        }
        public void SyncBoss(string response)
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
            NetworkInfo.bossInfo = responseJson.ToObject<BossInfo>();
        }
        public void InitStage2(string response)
        {
            Debug.Log(response);
            Communication.JsonFormat.InitStage2 responseJson = null;
            try
            {
                responseJson = JsonConvert.DeserializeObject<Communication.JsonFormat.InitStage2>(response);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
            Communication.JsonFormat.InitStage2.SetInitStage2(responseJson);
            NetworkInfo.bossInfo = responseJson.BossInfo;
            foreach(var playerInfo in responseJson.PlayerLocs)
            {
                NetworkInfo.playersInfo[playerInfo.data.ToString()] = null;
            }
        }
    }
}