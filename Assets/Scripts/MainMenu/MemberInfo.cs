using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// memberInfo API
// 각 방의 Uuid로 해당하는 방의 멤버 아이디 리스트 반환
public class MemberInfo
{
    public static List<string> Get(string roomUuid)
    {
        List<string> memList = new List<string>();

        string memInfoUri = "api/v1/rooms/" + roomUuid;
        string response;


        response = MServer.Communicate(memInfoUri, "GET");
        JObject json = JObject.Parse(response);

        Debug.Log(response);

        JToken arrData = json["data"]["memberInfo"];
        JArray jsonArray = (JArray)arrData;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            memList.Add(jsonArray[i]["nickname"].ToString());
        }

        return memList;
    }
}
