using Util;
namespace Communication.JsonFormat
{
    public class ItemInfo
    {
        private static ItemInfo value;
        public Division2 Loc {get;set;}
        public int Name {get; set;} = -1;
        public double LifeTime {get; set;}
        public static ItemInfo GetItemInfo()
        {
            var itemInfo = value;
            return itemInfo;
        }
        public static void InitItemInfo(ItemInfo itemInfo =null)
        {
            value = itemInfo;
        }
    }
}