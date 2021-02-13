using System.Reflection;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Lib;
using Communication.JsonFormat;


namespace Communication.API.Labylinth
{
    public class Controller
    {
        private string _server = "Labylinth";
        public void Move(string request)
        {
            JObject requestJson = JObject.Parse(request);
            Connection.Send(BaseJsonFormat.ObjectToJson(MethodBase.GetCurrentMethod().Name, _server, requestJson.Value<string>("uuid"), request));
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