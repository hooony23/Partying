using System;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Communication.API.Labylinth;
using Util;

namespace Lib
{
    public static class Common
    {
        public static void CallAPI(string server, string APIName, params object[] list)
        {
            /// <summary>
            /// param : "namespace.className"
            /// returm : Type
            /// </summary>
            Type controller = CallClass(server);
            CallMethod(controller, APIName, list);
        }

        public static Type CallClass(string server)
        {
            /// <summary>
            /// param : "namespace.className"
            /// returm : Type
            /// </summary>
            /// <returns></returns>
            Type type = Type.GetType($"Communication.API.{server}.Controller");
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
            Type type = classType.GetType();
            // private, protected, public에 관계없이 취득한다.
            MethodInfo info = type.GetMethod(APIName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            info.Invoke(classType, list);
        }

    }
}