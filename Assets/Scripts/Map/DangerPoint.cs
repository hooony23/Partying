using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerPoint : MonoBehaviour
{
    // 위험지역으로 달려갈 Enemy를 지정
    [SerializeField] PatrolAI enemy = null;

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
            enemy.Move();
        }
    }

}
