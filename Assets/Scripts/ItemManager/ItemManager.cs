using System;
using UnityEngine;
using Communication;
using Item;
using Lib;
namespace ItemManager
{
    public class ItemManager : ItemManagerController
    {
        public void Update()
        {
            if(NetworkInfo.itemRespawn.Name != -1)
            {
                var itemInfo = NetworkInfo.itemRespawn;
                GameObject spawnedItem = Instantiate(Resources.Load(ItemLocation[itemInfo.Name]),new Vector3(NetworkInfo.itemRespawn.Loc.X,2,NetworkInfo.itemRespawn.Loc.Y),Quaternion.identity) as GameObject;
                spawnedItem.GetComponent<BaseItem>().RemoveTime = Common.ConvertFromUnixTimestamp(itemInfo.LifeTime);
                NetworkInfo.itemRespawn.Name = -1;
            }
        }
    }
}