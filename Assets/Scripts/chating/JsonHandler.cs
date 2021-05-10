using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Chatting
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
