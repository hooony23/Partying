using UnityEngine;

public class DangerPoint : BaseTrap
{
    // 위험지역으로 달려갈 Enemy를 지정
    [SerializeField] PatrolAI enemy = null;

    public void OnTriggerEnter(Collider other)
    {
        TrapEvent(other,other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        
        TrapEvent(other,null);
    }
    public override void TrapEvent(Collider other, params object[] obj)
    {
        Transform transform = obj[0] != null ? (Transform)obj[0] : null;
        if (other.CompareTag("Player"))
        {
            enemy.CheckDanger(transform);
        }
    }
}
