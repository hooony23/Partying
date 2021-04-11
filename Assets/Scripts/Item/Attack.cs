namespace Item
{
    public class Attack : BaseItem
    {

        private Player _player = null;
        public void Start()
        {
            WaitTime = 10f;
        }
        public override void ItemApply(Player player,float time = 0)
        {
            
            _player = player;
            player.AttackDamage = Util.Config.playerAttackDamage * 2;
            base.ItemApply(player,time);
        }
        public override void DisAppear()
        {
            _player.AttackDamage = Util.Config.playerAttackDamage;
        }
    }
}