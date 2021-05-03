using System.Net.NetworkInformation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameUi;

public class BossLaser : MonoBehaviour, IDamageable
{
    private void OnParticleCollision(GameObject other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log("파티클에서 플레이어 충돌");
            TakeHit(other.GetComponent<Collider>(), 1);

        }
    }
    public void TakeHit(Collider collider, float damage)
    {
        UserScore aa =GameObject.Find("GameManager").GetComponent<UserScore>();
        var player = collider.gameObject.GetComponent<Player>();
        var reactVec = (collider.transform.position - this.transform.position).normalized;
        if (!player.IsBeatable)
            return;
        player.PlayerHealth -= damage;
        StartCoroutine(player.OnAttacked(reactVec));
        aa.PlayerHeart(player);
    }


}
