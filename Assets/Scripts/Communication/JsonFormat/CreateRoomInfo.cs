﻿namespace Communication.JsonFormat
{
    public class CreateRoomInfo
    {
        public string roomName;
        public string pwd;

        public CreateRoomInfo()
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
}
