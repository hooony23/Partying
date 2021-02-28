using Util;

namespace Communication.JsonFormat
{
    public class MapInfo
    {
        public int[,,] labylinthArray;
        public CellInfo[] patrolPoints;
        public CellInfo[] trap;
        public CellInfo[] playerLocs;
        public Division2 clearItem;
    }
}