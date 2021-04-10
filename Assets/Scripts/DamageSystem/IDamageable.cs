using UnityEngine;

public interface IDamageable
{
    void TakeHit(Collider colider,float damage);
}