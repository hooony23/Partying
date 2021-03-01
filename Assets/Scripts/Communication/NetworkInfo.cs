using System.Collections.Generic;
using Communication.JsonFormat;

namespace Communication
{
    public static class NetworkInfo
    {

        public static MapInfo mapInfo = null;
        public static Dictionary<string,PlayerInfo> playersInfo = new Dictionary<string,PlayerInfo>();
    }
}