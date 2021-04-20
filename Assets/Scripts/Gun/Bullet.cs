using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;

// 

public class Bullet : MonoBehaviour, IDamageable
{
    public int Damage { get; set; } = 1;
    public string BulletID { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("보스 맞음");
            TakeHit(other.GetComponent<Collider>(), Damage);
        }
    }

    // 보스에게 데미지를 입힘
    public void TakeHit(Collider collider, float damage)
    {
        var boss = collider.gameObject.GetComponent<Boss.Boss>();
        boss.BossHP -= damage;
        Debug.Log(boss.BossHP);
        Debug.Log(BulletID);
        Debug.Log(Damage);
    }

    private void OnEnable()
    {
        Invoke("DestroyBullet", 2f);
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
