namespace Item
{
    public class ReloadSpeed : BaseItem
    {
        private Player _player = null;
        public void Awake()
        {
            WaitTime = 10f;
        }
        public override void ItemApply(Player player, float time = 0)
        {

            _player = player;
            player.ShotSpeed = Util.Config.shotSpeed * 3;
            base.ItemApply(player, time);
            itemManager.AddBuffIcon(this.gameObject.name, time);
        }
        public override void DisAppear()
        {
            _player.ShotSpeed = Util.Config.shotSpeed;
        }
    }
}