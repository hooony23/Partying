using System.Collections.Generic;

[System.Serializable]
public class createRoomInfo
{
    public string type = "createRoom";
    public string uuid = "b2a6938e-8285-48b9-b0cd-017df4ed029b";
    public Dictionary<string, string> data = new Dictionary<string, string>();

    public createRoomInfo()
    {
        data["roomName"] = "";
        data["pwd"] = "";
    }

    public void UpdateInfo(string roomName, string pwd)
    {
        data["roomName"] = roomName;
        data["pwd"] = pwd;
    }

}
