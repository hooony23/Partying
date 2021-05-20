using UnityEngine;
using Util;
using Newtonsoft.Json.Linq;
namespace Communication.JsonFormat
{
    public sealed class AiInfo
    {
        private static AiInfo value {get;set;}
        public string Uuid {get;set;} = "Patrol";
        public string Target {get;set;} = "Player";
        private AiInfo(string uuid,string target){
            this.Uuid = uuid;
            this.Target = target;
        }
        public static void SetValue(string uuid,string target)
        {
            value = new AiInfo(uuid,target);
        }
        public static AiInfo GetInstance(string uuid,string target)
        {
            return new AiInfo(uuid,target);
        }
        public static void SetValue(JObject json)
        {
            value = json.ToObject<AiInfo>();
        }
        public static AiInfo GetValue()
        {
            return value;
        }
        public static void ClearValue()
        {
            value = null;
        }
    }
}