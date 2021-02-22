using System.Collections.Generic;

[System.Serializable]
public class signInInfo
{
    public string type = "signUp";
    public string uuid = "b2a6938e-8285-48b9-b0cd-017df4ed029b";
    public Dictionary<string, string> data = new Dictionary<string, string>();

    public signInInfo()
    {
        data["cellphone"] = "01012341234";
        data["pwd"] = "asdf123$";
    }

    public void UpdateInfo(string cellphone, string pwd)
    {
        data["cellphone"] = cellphone;
        data["pwd"] = pwd;
    }
}
