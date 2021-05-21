using UnityEngine;
using Communication;
using GameManager;
namespace Item
{
    public class Resurection : BaseItem
    {
        public GameManager.GameManager gameManager = null;
        public void OnEnable()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
        }
        public override void ItemApply(Player player, float time = 0)
        {
            var collider = gameObject.GetComponent<SphereCollider>();
            Destroy(collider);
            if (gameManager.DeathPlayerList.Count > 0)
            {
                GameObject[] deathUser = gameManager.DeathPlayerList.ToArray();
                int peek = Random.Range(0, 3);
                var resurectionUser = deathUser[peek];
                resurectionUser.GetComponent<Player>().IsDead = false;
            }
            base.ItemApply(player, time);
        }
    }
}