using Util;
namespace Communication.JsonFormat
{
    public class ItemInfo
    {
        public static ItemInfo value;
        public Division2 Loc {get;set;}
        public int Name {get; set;} = -1;
        public double LifeTime {get; set;}
        public static ItemInfo GetItemInfo()
        {
            var itemInfo = value;
            ItemInfo.value = null;
            return itemInfo;
        }
    }
}