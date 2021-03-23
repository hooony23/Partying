using System.Collections.Generic;
using Communication.JsonFormat;

namespace Communication
{
    public static class NetworkInfo
    {

        public static MapInfo mapInfo = null;
        public static Dictionary<string,PlayerInfo> playersInfo = new Dictionary<string,PlayerInfo>();
        public static Queue<string> connectedExitQueue = new Queue<string>();
        public static Queue<string> GetItemUserQueue = new Queue<string>();
        public static Queue<string> deathUserQueue = new Queue<string>();
    }
}