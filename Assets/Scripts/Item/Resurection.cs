using UnityEngine;
using Communication;

namespace Item
{
    public class Resurection : BaseItem
    {
        public override void ItemApply(Player player, float time = 0)
        {
            if(NetworkInfo.deathUserQueue.Count > 0)
            {   
                string[] deathUser = NetworkInfo.deathUserQueue.ToArray();
                int peek = Random.Range(0,3);
                var resurectionUser = GameObject.Find(deathUser[peek]);
                resurectionUser.GetComponent<Player>().IsDead = false;
            }
            base.ItemApply(player, time);
        }
    }
}