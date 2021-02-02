using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;



public class PlayerInfo
{
    public string type = "move";
    public Dictionary<string, object> data = new Dictionary<string, object>();
    public string movement = "";
    public string uuid = "";
    

    public void UpdateInfo(Vector3 location, Vector3 moveVec, string playerEvent, string userID)
    {
        Dictionary<string, object> loc = new Dictionary<string, object>();
        Dictionary<string, object> vec = new Dictionary<string, object>();
        float tx, ty, tz; // t : transform(location)
        float vx, vy, vz; // v : vector

        // 플레이어의 정보를 얻어온다
        tx = location.x;
        ty = location.y;
        tz = location.z;
        vx = moveVec.x;
        vy = moveVec.y;
        vz = moveVec.z;

        // 각각의 키 값에 얻어온 정보를 저장한다

        // "loc"
        loc["x"] = tx;
        loc["y"] = ty;
        loc["z"] = tz;

        // "vec"
        vec["x"] = vx; /* 추후 움직임 수정에 변경 가능성 있음 */
        vec["y"] = vy;
        vec["z"] = vz;
        
        // 딕셔너리를 값으로 가지는 키(loc, vec, ...) 에 정보를 받아온 딕셔너리를 추가한다
        data["loc"] = loc;
        data["vec"] = vec;

        // event
        data["movement"] = playerEvent;

        // uuid
        data["uuid"] = userID;


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
