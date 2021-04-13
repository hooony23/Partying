using Util;
namespace Communication.JsonFormat
{
    public class ItemInfo
    {
        public Division2 Loc {get;set;}
        public int Name {get; set;}
        public double LifeTime {get; set;}
        public ItemInfo()
        {
            Name = -1;
        }
    }
}