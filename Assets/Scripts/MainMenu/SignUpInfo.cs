using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class SignUpInfo
{
    public string type = "signIn";
    public string uuid = "b2a6938e-8285-48b9-b0cd-017df4ed029b";
    public Dictionary<string, string> data = new Dictionary<string, string>();

    public SignUpInfo()
    {
        data["nickname"] = "hong134";
        data["pwd"] = "asdf123$";
        data["cellphone"] = "01012341234";
        data["name"] = "김성훈";
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
