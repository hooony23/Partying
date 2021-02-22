using Util.Division;

namespace Communication.JsonFormat
{
    public class MapInfo
    {
        public int[,,] labylinthArray;
        public int[,] patrolPoints;
        public string[,] trap;
        public string[,] playerLocs;
        public Division2 clearItem;
    }
}