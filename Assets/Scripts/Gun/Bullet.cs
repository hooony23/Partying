using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;

public class Bullet : MonoBehaviour, IDamageable
{
    private int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            Debug.Log("보스 맞음");
            TakeHit(other.GetComponent<Collider>(), damage);
        }
    }

    public void TakeHit(Collider collider, float damage)
    {
        var boss = collider.gameObject.GetComponent<Boss.Boss>();
        boss.BossHP -= damage;
        Debug.Log(boss.BossHP);
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
