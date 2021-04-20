using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace signalR_Test_Client
{
    public class JsonHandler
    {
        public string JsonToString(JObject JsonObject)
        {
            return JsonObject.ToString();
        }

        public JObject StringToJson(string data)
        {
            return JObject.Parse(data);
        }
    }
}
