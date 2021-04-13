using System.Collections;
using UnityEngine;
using System.Collections.Generic;
namespace Item
{
    public class Wind : BaseItem
    {
        private Player _player = null;
        public void Start()
        {
            WaitTime = 30f;
        }
        public override void ItemApply(Player player, float time = 0)
        {

            _player = player;
            player.PlayerSpeed = Util.Config.playerSpeed * 3;
            base.ItemApply(player, time);
        }
        public override void DisAppear()
        {
            _player.PlayerSpeed = Util.Config.playerSpeed;
        }

    }
}