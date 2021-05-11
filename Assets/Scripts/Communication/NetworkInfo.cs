using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Util;
namespace Communication
{
    public static class NetworkInfo
    {
        //TODO: 가능하면 값 저장 없이 Receive단에서 바로 처리할 수 있도록 바꿔야함
        public static double startTime = 0d;
        public static double finishTime = 0d;
        public static MapInfo mapInfo = null;
        public static Dictionary<string, PlayerInfo> playersInfo = new Dictionary<string, PlayerInfo>();//new Dictionary<string, PlayerInfo>(){{"Player",null}};
        public static Queue<string> connectedExitQueue = new Queue<string>();
        public static Queue<string> GetItemUserQueue = new Queue<string>();
        public static Queue<string> deathUserQueue = new Queue<string>();
        public static string connectionId = "";
        public static JArray memberInfo = new JArray();
        public static JArray roomList = new JArray();
        // TODO:회원가입 또는 로그인 시 jwt로 가져온 사용자 정보를 저장해야함.
        public static MemberInfo myData = new MemberInfo();
        public static RoomInfo roomInfo = new RoomInfo();
        public static AiInfo aiInfo = null;
        public static BossInfo bossInfo = null;
    }
}