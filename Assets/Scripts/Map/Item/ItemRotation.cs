using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotation : ItemManager
{

    void Update()
    {
        transform.Rotate(new Vector3(0, 20f, 0) * Time.deltaTime, Space.World);
    }

    private void OnTriggerStay(Collider other) //아이템과 플레이어가 충돌시 발생하는 이벤트
    { 
        if (other.CompareTag("Player"))
        {
            GameObject BuffItem = this.gameObject;
            string BuffItemname = this.gameObject.name;
            ItemActive(BuffItemname, BuffItem);

        }

    }
}
