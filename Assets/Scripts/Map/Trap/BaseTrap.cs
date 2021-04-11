using UnityEngine;

public class BaseTrap : MonoBehaviour, ITrapEvent, IDamageable
{
    protected Animator anim { get; set; }
    public virtual void TrapEvent(Collider other, params object[] obj) { }
    public virtual void TakeHit(Collider col, float damage = 999)
    {
        col.gameObject.GetComponent<Player>().PlayerHealth -= damage;
    }
}