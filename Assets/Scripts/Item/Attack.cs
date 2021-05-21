using UnityEngine;

namespace Item
{
    public class Attack : BaseItem
    {

        private Player _player = null;
        public void Awake()
        {
            WaitTime = 10f;
        }
        public override void ItemApply(Player player, float time = 0)
        {
            var collider = gameObject.GetComponent<SphereCollider>();
            Destroy(collider);
            _player = player;
            player.AttackDamage = Util.Config.playerAttackDamage * 2;
            base.ItemApply(player, time);
            itemManager.AddBuffIcon(this.gameObject.name, time);
        }
        public override void DisAppear()
        {
            _player.AttackDamage = Util.Config.playerAttackDamage;
        }
    }
}