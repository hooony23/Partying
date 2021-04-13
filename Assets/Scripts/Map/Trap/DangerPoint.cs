using UnityEngine;

public class DangerPoint : MonoBehaviour
{
    // 위험지역으로 달려갈 Enemy를 지정
    PatrolAI enemy = null;
    private void Start()
    {
        enemy = GameObject.Find("Patrol").GetComponent<PatrolAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.CheckDanger(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.CheckDanger(null);
        }
    }

}
