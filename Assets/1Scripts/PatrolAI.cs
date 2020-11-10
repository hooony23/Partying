using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolAI : MonoBehaviour
{
    NavMeshAgent enemy = null;

    // 정찰 위치들을 담을 배열 선언
    [SerializeField] Transform[] tfWayPoints = null;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        // 시작하자마자 2초 마다 MoveToNextWayPoint 반복
        InvokeRepeating("MoveToNextWayPoint", 0f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        // 타겟이 있다면 타겟 위치로 이동
        if (target != null)
        {
            enemy.SetDestination(target.position);
        }
    }

    // 다음 정찰지역으로 이동시켜줌
    void MoveToNextWayPoint()
    {
        if (target == null)
        {
            // AI 속도가 0이 되면 다음 지역으로 순찰 시작
            if (enemy.velocity == Vector3.zero)
            {
                enemy.SetDestination(tfWayPoints[count++].position); // count 를 증가시키며 다음지역

                if (count >= tfWayPoints.Length)
                    count = 0;
            }
        }
    }

    Transform target = null;
    public void SetTarget(Transform playertarget)
    {
        // 기존 순찰 취소
        CancelInvoke();
        target = playertarget;
        enemy.speed = 20f;
    }

    public void RemoveTarget()
    {
        enemy.speed = 3.5f;
        target = null;
        InvokeRepeating("MoveToNextWayPoint", 0f, 2f);
    }
}
