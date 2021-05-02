using UnityEngine;
using System.Collections.Generic;

public class DangerPoint : BaseTrap
{
    // 위험지역으로 달려갈 Enemy를 지정
    [SerializeField] List<PatrolAI> enemys;
    public void Awake()
    {
        enemys = new List<PatrolAI>();
        GameObject AIs = GameObject.Find("AIs");
        for(var i =0;i<AIs.transform.childCount;i++)
            enemys.Add(AIs.transform.GetChild(i).GetComponent<PatrolAI>());
    }
    public void OnTriggerEnter(Collider other)
    {
        TrapEvent(other, other.transform);
    }

    private void OnTriggerExit(Collider other)
    {

        TrapEvent(other, null);
    }
    public override void TrapEvent(Collider other, params object[] obj)
    {
        Transform transform = null;
        if(obj != null)
            transform = (Transform)obj[0];
        if (other.CompareTag("Player"))
        {
            foreach(var enemy in enemys)
                enemy.CheckDanger(transform);
        }
    }
}