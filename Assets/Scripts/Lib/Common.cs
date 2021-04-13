using System;
using System.Reflection;
using System.Collections;
using UnityEngine;
<<<<<<< HEAD
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Communication.GameServer.API.Labylinth;
using Util;
=======

>>>>>>> origin/dev-SungyuHwang

namespace Lib
{
    public static class Common
    {
<<<<<<< HEAD

        public static void SetUserUuid(string uuid)
        {
            Config.userUuid = uuid;
        }
        public static JObject GetData(object request, string name = "data")
        {
            return JObject.Parse(request.ToString())[name] as JObject;
        }
        public static void CallAPI(string server, string tranferFlag, string APIName, params object[] list)
=======
        public static IEnumerator WaitThenCallback(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback();
        }
        public static void CallAPI(string tranferFlag, string APIName, params object[] list)
>>>>>>> origin/dev-SungyuHwang
        {
            /// <summary>
            /// param : "namespace.className"
            /// returm : Type
            /// </summary>
<<<<<<< HEAD
            Type controller = CallClass(server, tranferFlag);
            CallMethod(controller, APIName, list);
        }

        public static Type CallClass(string server, string tranferFlag)
=======
            Type controller = CallClass(tranferFlag);
            CallMethod(controller, APIName, list);
        }

        public static Type CallClass(string tranferFlag)
>>>>>>> origin/dev-SungyuHwang
        {
            /// <summary>
            /// param : "namespace.className"
            /// returm : Type
            /// </summary>
            /// <returns></returns>
<<<<<<< HEAD
            Type type = Type.GetType($"Communication.GameServer.API.{server}.{tranferFlag}");
=======
            Type type = Type.GetType($"Communication.API.{tranferFlag}");
>>>>>>> origin/dev-SungyuHwang
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

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            return origin.AddSeconds(timestamp);
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }
}