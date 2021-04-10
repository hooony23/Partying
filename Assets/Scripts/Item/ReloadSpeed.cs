namespace Item
{
    public class ReloadSpeed : BaseItem
    {
        private Player _player = null;
        public void Start()
        {
            WaitTime = 10f;
        }
        public override void ItemApply(Player player,float time = 0)
        {
            
            _player = player;
            player.ShotSpeed = Util.Config.shotSpeed * 3;
            Invoke("DisAppear",WaitTime);
            base.ItemApply(player,time);
        }
        public void DisAppear()
        {
            _player.ShotSpeed = Util.Config.shotSpeed;
        }
    }
}