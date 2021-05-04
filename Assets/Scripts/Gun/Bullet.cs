using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Boss;
using Communication.GameServer.API;

// 
namespace Weapon
{
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
        if(BulletID.Equals(Util.Config.userUuid))
            APIController.SendController("AttackBoss",new {damage=damage});
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
}