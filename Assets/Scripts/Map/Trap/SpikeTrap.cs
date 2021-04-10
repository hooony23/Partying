using UnityEngine;

public class SpikeTrap : BaseTrap
{
    public void OnTriggerEnter(Collider other) 
    {
        TrapEvent(other);
    }
    public override void TrapEvent(Collider col, params object[] obj)
    {        // holeActive 가 false 상태(처음으로 함정을 밟음) 이면 함정을 발동
    
        if (col.CompareTag("Player"))
        {
            TakeHit(col);
            anim.SetTrigger("open");
        }
    }
}