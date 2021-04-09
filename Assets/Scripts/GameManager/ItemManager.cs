using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    enum Item { WindItem, AttckItem , HeartItem ,ReloadSpeedItem, HealthmaxItem, ResurrectionItem }
    Item Wind = Item.WindItem;
    Item Attck = Item.AttckItem;
    Item Heart = Item.HeartItem;
    Item ReloadSpeed = Item.ReloadSpeedItem;
    Item Healthmax = Item.HealthmaxItem;
    Item Resurrection = Item.ResurrectionItem;
    public void ItemActive(string Itemname, GameObject ItemObject) {

       // Debug.Log(Itemname);
       // Destroy(ItemObject);
        if (Itemname.Equals(Wind.ToString())) {
            Debug.Log("바람 아이템");
            Destroy(ItemObject);
        }
        if (Itemname.Equals(Attck.ToString()))
        {
            Debug.Log("공격 아이템");
            Destroy(ItemObject);
        }
        if (Itemname.Equals(Heart.ToString()))
        {
            Debug.Log("체력회복 아이템");
            Destroy(ItemObject);
        }
        if (Itemname.Equals(ReloadSpeed.ToString()))
        {
            Debug.Log("장전속도 아이템");
            Destroy(ItemObject);
        }
        if (Itemname.Equals(Healthmax.ToString()))
        {
            Debug.Log("피 영구증가 아이템");
            Destroy(ItemObject);
        }
        if (Itemname.Equals(Resurrection.ToString()))
        {
            Debug.Log("부활 아이템");
            Destroy(ItemObject);
        }
    }
}
