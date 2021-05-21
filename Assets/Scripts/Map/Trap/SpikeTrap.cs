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
    {
        // holeActive 가 false 상태(처음으로 함정을 밟음) 이면 함정을 발동
        StartCoroutine(OnTrap(col));
    }

    private IEnumerator OnTrap(Collider col)
    {
        yield return new WaitForSeconds(1f);
        anim.SetTrigger("doActivate");
    }


    // SpikeUpDown 애니메이션 내부에서 실행됨
    public void OffTrap()
    {
        IsActive = false;
    }
}