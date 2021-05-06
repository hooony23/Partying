using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

namespace ItemManager
{
    public class ItemManager : ItemManagerController
    {
        public GameObject UserUi;
        public Image iconFill;
        private string ItemIcon = "GameUi/IconPrefabs/";
        public void Update()
        {
            var num = -1;
            //if(!NewtworkInfo.ItemRespawn.Equal(""))
            // foreach (EItem item in Enum.GetValues(typeof(EItem))) { if (item.ToString().Equals("itemName")) { num = (int)item; break; } }
            // Instantiate(Resources.Load(ItemLocation[num]) as GameObject);
        }
        public void AddBuffIcon(string name, float time = 0)
        {
            name = name.Split(new string[] { "Item" }, StringSplitOptions.None)[0];
            Debug.Log(name);
            if (itemSet.Contains(name)) {
                return;
            }
            itemSet.Add(name); //Wind
            UserUi = GameObject.Find("UserUi(Clone)").transform.Find("UserInfoUi").transform.Find("PlayerBuff").gameObject;
            string iconName = ItemIcon + name+ "UiIcon"; //GameUi/IconPrefabs/Wind
            Debug.Log(iconName);
            GameObject ItemIconObject = Instantiate(Resources.Load(iconName)) as GameObject;
            ItemIconObject.name = Resources.Load(iconName).name; //WindUiIcon
            ItemIconObject.transform.SetParent(UserUi.transform);
            iconFill = ItemIconObject.transform.GetChild(0).GetComponent<Image>();
            StartCoroutine(Cooltime(ItemIconObject, time)); //WindUiIcon
        }
        IEnumerator Cooltime(GameObject gameObject, float time)
        {
            Debug.Log(time);
            float coolTime = time;
            while (iconFill.fillAmount > 0)
            {
                iconFill.fillAmount -= 1 * Time.smoothDeltaTime / coolTime;
                yield return null;
            }
            Destroy(gameObject);
            itemSet.Remove(gameObject.name.Replace("UiIcon",""));
            yield break;
        }
    }
}