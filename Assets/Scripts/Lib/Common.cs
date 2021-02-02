using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Partying.Assets.Scripts.API.Labylinth;


namespace Partying.Assets.Scripts.Lib
{
    public class Common
    {
        public static void CallAPI(string APIName,params object[] list)
        {
            //TODO 
            Controller controller = CallClass("Labylinth");
            CallMethod(controller, APIName, list);
        }

        public static Controller CallClass(string nameSpaceName)
        {
            /// <summary>
            /// param : "namespace.className"
            /// returm : Type
            /// </summary>
            /// <returns></returns>
            Type type = Type.GetType($"Partying.Assets.Scripts.API.{nameSpaceName}.Controller");
            Controller instance = Activator.CreateInstance(type) as Controller;
            return instance;
        }

        public static void CallMethod(object obj, String APIName,params object[] list)
        {
            /// <summary>
            /// param : obj : classInstance , APIName : APIName
            /// process : Call Method
            /// returm : None
            /// </summary>
            /// <returns></returns>
            Type type = obj.GetType();
            // private, protected, public에 관계없이 취득한다.
            MethodInfo info = type.GetMethod(APIName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            info.Invoke(obj, list);
        }
        public static string GetRequestData(string type, string server, string uuid, string data){
            JObject requestJson = Config.requestForm;
            requestJson["type"] = type;
            requestJson["server"] = server;
            requestJson["uuid"] = uuid;
            requestJson["data"] = data;
            return requestJson.ToString();
        }
    }
}