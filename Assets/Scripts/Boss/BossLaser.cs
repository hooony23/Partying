using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaser : MonoBehaviour
{
    Player raidPlayer;
    private void OnParticleCollision(GameObject other)
    {
        if (other.tag.Equals("Player"))
        {
            Debug.Log("파티클에서 플레이어 충돌");

            Vector3 reactVec = (other.transform.position - this.transform.position).normalized;
            raidPlayer = other.GetComponent<Player>();
            raidPlayer.TakeAttack(reactVec);

        }
    }


}
