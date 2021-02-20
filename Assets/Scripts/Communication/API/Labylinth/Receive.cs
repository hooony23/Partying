using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Util;


namespace Communication.API.Labylinth
{
    public class Receive : Controller
    {
        public void CreateMap(string response)
        {
            MapInfo mapInfo = JsonConvert.DeserializeObject<MapInfo>(response);
            Map mapObject = null;
            
            //TODO 더 좋은 방법 찾아야함
            try{
            Config.mapInfo = mapInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }
        public void SyncPackit(string response)
        {
            JObject responseJson = JObject.Parse(response);
            JObject[] datas = responseJson.Value<JObject[]>("data");
            foreach (var data in datas)
            {
                GameObject gameObject = GameObject.Find(data.Value<string>("uuid"));
                Player player = gameObject.GetComponent<Player>();
                player.PInfo = data.ToObject<PlayerInfo>();
            }

        }
    }
}