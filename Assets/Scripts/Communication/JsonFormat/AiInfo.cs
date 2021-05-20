using UnityEngine;
using Util;
using Newtonsoft.Json.Linq;
namespace Communication.JsonFormat
{
    public sealed class AiInfo
    {
        public string Uuid {get;set;} = "Patrol";
        public string Target {get;set;} = "Player";
        public AiInfo(string uuid,string target){
            this.Uuid = uuid;
            this.Target = target;
        }
    }
}