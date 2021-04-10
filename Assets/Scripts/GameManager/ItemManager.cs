using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    enum Item { WindItem, AttckItem, HeartItem, ReloadSpeedItem, HealthmaxItem, ResurrectionItem }
    Item Wind = Item.WindItem;
    Item Attck = Item.AttckItem;
    Item Heart = Item.HeartItem;
    Item ReloadSpeed = Item.ReloadSpeedItem;
    Item Healthmax = Item.HealthmaxItem;
    Item Resurrection = Item.ResurrectionItem;
    public void ItemActive(string Itemname, GameObject ItemObject, GameObject PlayUser)
    {

        RaidPlayerUtil User = PlayUser.gameObject.GetComponent<RaidPlayerUtil>();
        GameObject Itemactivetime = ItemObject.transform.Find("Itemmodle").gameObject;
        float UserHealth = User.PlayerHealth;

        if (Itemname.Equals(Wind.ToString()))
        {
            Debug.Log("바람 아이템");
            StartCoroutine(Itembuff(Itemname, User, ItemObject, 30f));
            Itemactivetime.SetActive(false);
        }
        if (Itemname.Equals(Attck.ToString()))
        {
            Debug.Log("공격 아이템");
            StartCoroutine(Itembuff(Itemname, User, ItemObject, 10f));
            Itemactivetime.SetActive(false);
        }
        if (Itemname.Equals(ReloadSpeed.ToString()))
        {
            Debug.Log("장전속도 아이템");
            StartCoroutine(Itembuff(Itemname, User, ItemObject, 10f));
            Itemactivetime.SetActive(false);
        }
        if (Itemname.Equals(Heart.ToString()))
        {
            Debug.Log("체력회복 아이템");
            StartCoroutine(Itemapply(Itemname, User, ItemObject));
            Itemactivetime.SetActive(false);
        }
        if (Itemname.Equals(Healthmax.ToString()))
        {
            StartCoroutine(Itemapply(Itemname, User, ItemObject));
            Debug.Log("피 영구증가 아이템");
            Itemactivetime.SetActive(false);
        }
        if (Itemname.Equals(Resurrection.ToString()))
        {
            StartCoroutine(Itemapply(Itemname, User, ItemObject));
            Debug.Log("부활 아이템");
            Itemactivetime.SetActive(false);
        }
    }
    public IEnumerator Itembuff(string Itemname2, RaidPlayerUtil User, GameObject ItemObject, float time)
    {
        if (Itemname2.Equals(Wind.ToString())) {//이동속도 2배, 30초
            User.PlayerSpeed = 42f;
            yield return new WaitForSeconds(time);
            User.PlayerSpeed = 14f;
            Destroy(ItemObject);
        }
        if (Itemname2.Equals(Attck.ToString())) {// 공격력 2배,10초
           //플레이어 데미지 증가 [스크립트 끌어오기(미구현)] 
            yield return new WaitForSeconds(time);
            //플레이어 데미지 감소 [스크립트 끌어오기(미구현)] 
            Destroy(ItemObject);
        }
        if (Itemname2.Equals(ReloadSpeed.ToString())) { //공격속도 3배, 10초
            //플레이어 공격속도 증가 [스크립트 끌어오기(미구현)] 
            yield return new WaitForSeconds(time);
            //플레이어 공격속도 감소 [스크립트 끌어오기(미구현)] 
            Destroy(ItemObject);
        }
    }
    public IEnumerator Itemapply(string Itemname2, RaidPlayerUtil User, GameObject ItemObject)
    {
        if (Itemname2.Equals(Heart.ToString())) {// 플레이어 체력 회복
            User.RaidplayerHealth += 100f;
            Debug.Log(User.RaidplayerHealth);
            yield return new WaitForSeconds(0.1f);
            Destroy(ItemObject);
        }
        if (Itemname2.Equals(Attck.ToString())) {// 플레이어 체력 영구증가
            //아이템 오브젝트와 충돌한 플레이어 최대체력 증가 [스크립트 끌어오기(미구현)] 
            yield return new WaitForSeconds(0.1f);
            Destroy(ItemObject);
        }
        if (Itemname2.Equals(ReloadSpeed.ToString())) { //플레이어 랜덤 부활
            //아이템 오브젝트와 충돌한 플레이어외 랜덤플레이어 부활 [스크립트 끌어오기(미구현)] 
            yield return new WaitForSeconds(0.1f);
            Destroy(ItemObject);
        }
    }
}
