using System.Collections;
using UnityEngine;

public class SpikeTrap : BaseTrap
{
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!IsActive)
        {
            TrapEvent(other);
            IsActive = true;
        }
    }
    public override void TrapEvent(Collider col, params object[] obj)
    {        // holeActive 가 false 상태(처음으로 함정을 밟음) 이면 함정을 발동

        StartCoroutine(HitPlayer(col));
    }

    private IEnumerator HitPlayer(Collider col)
    {
        anim.SetTrigger("doActivate");
        yield return new WaitForSeconds(1.1f);
        if (col.CompareTag("Player"))
        {
            TakeHit(col);
        }
    }

    // SpikeUpDown 애니메이션 내부에서 실행됨
    public void SpikeOff()
    {
        IsActive = false;
    }
}