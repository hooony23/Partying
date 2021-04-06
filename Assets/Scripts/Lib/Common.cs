using System;
using System.Reflection;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Communication.GameServer.API.Labylinth;
using Util;

namespace Lib
{
    public static class Common
    {

        public static void SetUserUuid(string uuid)
        {
            Config.userUuid = uuid;
        }
        public static JObject GetData(object request, string name = "data")
        {
            return JObject.Parse(request.ToString())[name] as JObject;
        }
        public static void CallAPI(string server, string tranferFlag, string APIName, params object[] list)
        {
            /// <summary>
            /// param : "namespace.className"
            /// returm : Type
            /// </summary>
            Type controller = CallClass(server, tranferFlag);
            CallMethod(controller, APIName, list);
        }

        public static Type CallClass(string server, string tranferFlag)
        {
            /// <summary>
            /// param : "namespace.className"
            /// returm : Type
            /// </summary>
            /// <returns></returns>
            Type type = Type.GetType($"Communication.API.{server}.{tranferFlag}");
            return type;
        }

        public static void CallMethod(Type classType, String APIName, params object[] list)
        {
            /// <summary>
            /// param : classInstance : classInstance , APIName : APIName, list : API params
            /// process : Call Method
            /// returm : None
            /// </summary>
            /// <returns></returns>
            Type type = classType;

            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
            object classObject = constructor.Invoke(new object[] { });
            // private, protected, public에 관계없이 취득한다.
            MethodInfo info = type.GetMethod(APIName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            try
            {
                info.Invoke(classObject, list);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static string ToPascalCase(string str)
        {
            // If there are 0 or 1 characters, just return the string.
            if (str == null) return str;
            if (str.Length < 2) return str.ToUpper();

            // Split the string into words.
            string[] words = str.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }
        public static string ToCamelCase(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 1)
            {
                return char.ToLowerInvariant(str[0]) + str.Substring(1);
            }
            return str;
        }
    }
}