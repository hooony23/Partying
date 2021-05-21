using UnityEngine;

namespace Item
{
    public class Heart : BaseItem
    {
        public override void ItemApply(Player player, float time = 0)
        {
            var collider = gameObject.GetComponent<SphereCollider>();
            Destroy(collider);
            player.PlayerHealth += 1;
            player.SyncHp();
            base.ItemApply(player, time);
        }
    }
}