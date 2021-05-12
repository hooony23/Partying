using System.Diagnostics;
using System.Text;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Util;
using Communication.JsonFormat;
// JSON 정보를 서버로 보내는 클래스 입니다
namespace Communication.MainServer
{

    public class MServer : MonoBehaviour
    {
        public static string json = "";
        private static string basicURL = Config.mainServerDNS;

        /// <summary>
        /// POST, PUT, DELETE 메서드를 사용할 경우 이용합니다
        /// </summary>
        /// <param name="uri">MServer 클래스의 basicURL을 확인, 해당부분을 제외한 나머지 주소 입력</param>
        /// <param name="method">"POST", "PUT", "DELETE"</param>
        /// <param name="param">JSON으로 변환될 클래스</param>
        /// <returns>JSON -> String 값</returns>
        public static String Communicate(string method, string uri, object param = null)
        {
            uri = basicURL + "/" + uri;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            // 요청과 보낼 주소 세팅
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = method;
            request.ContentType = "application/json";

            // GET이 아닐 경우.
            if (!method.Equals("GET"))
            {

                // 데이터(body) 직렬화
                string str = JsonConvert.SerializeObject(param);
                var bytes = System.Text.Encoding.UTF8.GetBytes(str);

                request.ContentLength = bytes.Length;
                // TODO: Server Logging 추가되면 삭제
                UnityEngine.Debug.Log($"request : {uri} {param.ToString()}");
                // Stream 형식으로 데이터를 보냄
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                    stream.Close();
                }
            }
            else
            {
                if (param != null)
                    uri = $"{uri}?{param.ToString()}";
                // TODO: Server Logging 추가되면 삭제
                UnityEngine.Debug.Log($"request : {uri}");
                request = (HttpWebRequest)WebRequest.Create(uri);
            }
            // StreamReader 로 역질렬화, 응답 데이터를 받음
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            json = reader.ReadToEnd();
            UnityEngine.Debug.Log($"response : {json}");
            return json;
        }

        // ValidationCheckFunction
        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
        public static string SignUp(object requestJson)
        {
            var requestString = BaseJsonFormat.ObjectToJson("signUp", requestJson);
            return Communicate("POST", "api/v1/session/signUp", requestString);
        }
        public static string SignIn(object requestJson)
        {
            var requestString = BaseJsonFormat.ObjectToJson("signIn", requestJson);
            return Communicate("POST", "api/v1/session/signIn", requestString);
        }
        public static string SignOut()
        {
            return Communicate("GET", "api/v1/session/signOut", $"userUuid={Util.Config.userUuid}");
        }
        public static string Pingpong()
        {
            return Communicate("GET", "api/v1/util/pingpong");
        }

        public static string CreateRoom(object requestJson)
        {
            var requestString = BaseJsonFormat.ObjectToJson("creatRoom", requestJson);
            var response = Communicate("POST", "api/v1/rooms/createRoom", requestString);
            var roomInfo = (Lib.Common.GetData(response)["roomInfo"] as JObject).ToObject<RoomInfo>();
            Chatting.ChatModule.GetChatModule().AddToGroup(roomInfo.RoomUuid);
            return response;
        }

        public static JArray GetMemberInfo(string roomUuid)
        {

            string memberInfoUri = $"api/v1/rooms/{roomUuid}";
            if (NetworkInfo.connectionId.Equals(""))
                throw new Exception("not found connectionId");
            var response = Communicate("GET", memberInfoUri, $"userUuid={Config.userUuid}&connectionId={NetworkInfo.connectionId}");
            Chatting.ChatModule.GetChatModule().AddToGroup(roomUuid);
            return JObject.Parse(response)["data"]["memberInfo"] as JArray;
        }
        public static void LeaveRoom(string roomUuid)
        {
            Communicate("GET", $"api/v1/rooms/{roomUuid}/leave", $"userUuid={Config.userUuid}");
            Chatting.ChatModule.GetChatModule().RemoveFromGroup(roomUuid);
        }
        public static string GetRoomsList()
        {
            return Communicate("GET", "api/v1/rooms/main", $"userUuid={Config.userUuid}&connectionId={NetworkInfo.connectionId}");
        }
        public static JArray ReturnRoom(string roomUuid)
        {
            var response = Communicate("GET", $"api/v1/rooms/{roomUuid}/returnRoom");
            return JObject.Parse(response)["data"]["memberInfo"] as JArray;
        }
        public static void Ready(string roomUuid, bool isReady)
        {
            Communicate("GET", $"api/v1/rooms/{roomUuid}/ready", $"userUuid={Config.userUuid}&ready={isReady}");
        }

    }

    // // ValidationCheckClass
    // public class WebRequestCert : UnityEngine.Networking.CertificateHandler
    // {
    //     protected override bool ValidateCertificate(byte[] certificateData)
    //     {
    //         //return base.ValidateCertificate(certificateData);
    //         return true;
    //     }
    // }

}