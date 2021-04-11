using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;



public class AIMove
{
    public string type = "aiMove";
    public string uuid = "room_masteruuid-8285-48b9-b0cd-017df4ed029b"; // 서버로부터 받아오는 로직 추가
    public Dictionary<string, object> data = new Dictionary<string, object>();


    public void UpdateAiInfo(string aiUuid, Vector3 location, Vector3 moveVec, Transform target)
    {
        Dictionary<string, object> loc = new Dictionary<string, object>();
        Dictionary<string, object> vec = new Dictionary<string, object>();
        Dictionary<string, object> targetPoint = new Dictionary<string, object>();
        float tx, ty, tz; // t : transform(location)
        float vx, vy, vz; // v : vector(Vector3)
        float tgx, tgy, tgz; // tg : target(location)

        // Vector 정보 받아옴
        tx = location.x;
        ty = location.y;
        tz = location.z;
        vx = moveVec.x;
        vy = moveVec.y;
        vz = moveVec.z;
        tgx = target.position.x;
        tgy = target.position.y;
        tgz = target.position.z;

        // 각각의 키 값에 얻어온 정보를 저장한다

        // "loc"
        loc["x"] = tx;
        loc["y"] = ty;
        loc["z"] = tz;

        // "vec"
        vec["x"] = vx;
        vec["y"] = vy;
        vec["z"] = vz;

        // "targetPoint"
        targetPoint["x"] = tgx;
        targetPoint["y"] = tgy;
        targetPoint["z"] = tgz;

        // 최종 data 정보
        data["aiUuid"] = aiUuid;
        data["loc"] = loc;
        data["vec"] = vec;
        data["targetPoint"] = targetPoint;

    }

    public string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    public T JsonToOject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(jsonData);
    }


}
