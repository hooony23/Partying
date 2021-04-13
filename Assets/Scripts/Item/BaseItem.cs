using System;
using UnityEngine;
using System.Collections.Generic;
namespace Item
{
    public class BaseItem : MonoBehaviour, IItem
    {
        public float WaitTime { get; set; }
        public DateTime RemoveTime { get; set; }

        public void FixedUpdate()
        {
            transform.Rotate(new Vector3(0, 20f, 0) * Time.deltaTime, Space.World);
            RemoveItem();
        }


        private void OnTriggerEnter(Collider other) //아이템과 플레이어가 충돌시 발생하는 이벤트
        {
            if (other.CompareTag("Player"))
            {
                Player player = other.transform.gameObject.GetComponent<Player>();
                ItemApply(player, WaitTime);


            }
        }
        private void RemoveItem()
        {
            if(DateTime.Now>RemoveTime)
            {
                StartCoroutine(Lib.Common.WaitThenCallback(0, () => { Destroy(this.gameObject); }));
            }
        }
        public virtual void DisAppear() { }
        public virtual void ItemApply(Player player, float time = 0)
        {
            Invoke("DisAppear", time);
            // 아이템 이벤트 실행보다 먼저 사라지지 않도록 하기 위해 0.5f 추가.
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Destroy(this.transform.GetChild(i).gameObject);
            }
            StartCoroutine(Lib.Common.WaitThenCallback(time + 0.5f, () => { Destroy(this.gameObject); }));
        }
    }
}