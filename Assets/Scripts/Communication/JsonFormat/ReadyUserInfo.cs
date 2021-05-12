namespace Communication.JsonFormat
{
    public class ReadyUserInfo
    {
        private static ReadyUserInfo value;
        public string Player{get;set;} = "";
        public bool Ready{get;set;} = false;
        public static void SetReadyUserInfo(ReadyUserInfo readyUser)
        {
            value  = readyUser;
        }
        
        public static ReadyUserInfo GetReadyUserInfo()
        {
            var readyUser = value;
            value = null;
            return readyUser;
        }
    }
}