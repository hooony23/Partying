using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Communication;
using Communication.MainServer;
using Util;
// memberInfo API
// 각 방의 Uuid로 해당하는 방의 멤버 아이디 리스트 반환
public class MemberInfo
{
    public static JArray Get(string roomUuid)
    {

        string memInfoUri = "api/v1/rooms/" + roomUuid;
        string response;

        
        if(NetworkInfo.connectionId.Equals(""))
            throw new Exception("not found connectionId");
        response = MServer.Communicate("GET", memInfoUri, $"userUuid={Config.userUuid}&connectionId={NetworkInfo.connectionId}");
        JObject json = JObject.Parse(response);

        Debug.Log(response);

        JToken arrData = json["data"]["memberInfo"];
        JArray jsonArray = (JArray)arrData;

        return jsonArray;
    }
}
