using System.Collections.Generic;
using Newtonsoft.Json;

[System.Serializable]
public class signUpInfo
{
    public string type = "signIn";
    public string uuid = "b2a6938e-8285-48b9-b0cd-017df4ed029b";
    public Dictionary<string, string> data = new Dictionary<string, string>();

    public signUpInfo()
    {
        data["nickname"] = "hong134";
        data["pwd"] = "asdf123$";
        data["cellphone"] = "01012341234";
        data["name"] = "김성훈";
    }
   
    public void UpdateInfo(string id, string password, string mobile, string name)
    {
        data["nickname"] = id;
        data["pwd"] = password;
        data["cellphone"] = mobile;
        data["name"] = name;
    }

}
