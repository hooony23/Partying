using Util;

namespace Communication.JsonFormat
{
    public class InitStage2
    {
        private static InitStage2 value;
        public BossInfo BossInfo {get; set;}
        public CellInfo[] PlayerLocs {get; set;}
        public static InitStage2 GetInitStage2()
        {
            return value;
        }
        public static void SetInitStage2(InitStage2 initStage2 = null)
        {
            value = initStage2;
        }
    }
}