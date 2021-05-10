namespace Communication.JsonFormat
{
    public class CreateRoomInfo
    {
        public string RoomName { get; set; } = "";
        public string Pwd { get; set; } = "";
        public string ConnectionId { get; set; } = "";

        public void UpdateInfo(string roomName, string pwd)
        {
            RoomName = roomName;
            Pwd = pwd;
        }

    }
}
