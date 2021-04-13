namespace Item
{
    public class Heart : BaseItem
    {
        public override void ItemApply(Player player, float time = 0)
        {
            player.PlayerHealth += 1;
            base.ItemApply(player, time);
        }
    }
}