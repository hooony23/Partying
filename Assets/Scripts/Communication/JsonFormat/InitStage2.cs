using Util;

namespace Communication.JsonFormat
{
    public class InitStage2
    {
        public static InitStage2 value;
        public BossInfo BossInfo {get; set;}
        public CellInfo[] PlayerLocs {get; set;}
        public static InitStage2 GetInitStage2()
        {
            return value;
        }
    }
}