using System.Collections.Generic;

[System.Serializable]
public class createRoomInfo
{
    public string roomName;
    public string pwd;

    public createRoomInfo()
    {
        roomName = "";
        pwd = "";
    }

    public void UpdateInfo(string roomName, string pwd)
    {
        this.roomName = roomName;
        this.pwd = pwd;
    }

}
