namespace Communication.JsonFormat
{
    public class MemberInfo
    {
        public string UserUuid { get; set; }
        public string Nickname { get; set; }
        public MemberInfo():this("",""){}
        public MemberInfo(string userUuid,string nickname)
        {
            UserUuid = userUuid;
            Nickname = nickname;
        }
    }
}
