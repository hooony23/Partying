using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/* 순찰자의 순찰, 인식, 추격 기능 
 * Target을 Player로 둘수 있고, 다른것을 쫒게 할 수도 있음
 * 추격할 타겟의 태그를 "Player", LayerMask를 "Player"라고 설정해야함
 */

public class PatrolAI : MonoBehaviour
{
    // 시야각 에 따른 순찰
    [SerializeField] float viewAngle = 0f; // 시야각
    [SerializeField] float distance = 0f; // 반경
    [SerializeField] LayerMask layerMask = 0; // OverlapSphere : LayerMask를 통해 인식함


    // 주변의 패트롤포인트를 인식하고 인식된 포인트에서만 순찰
    static int find_point = 2;
    Transform[] tfWayPoints = new Transform[find_point]; // (test)랜덤으로 근처 2개만 포인트 인식
    int count = 0;
    float detect_distance = 30;

    NavMeshAgent enemy = null;
    Transform target = null; // 타켓이 정해지면 추격함

    private void Awake()
    {
        enemy = GetComponent<NavMeshAgent>();
        

    }
    void Start()
    {
        InvokeRepeating("Patrol", 0f, 2f);
        FindPatrolPoint();
    }

    void Update()
    {
        UpdateTarget();
        Chase();
        
    }

    // 다음 정찰지역으로 이동시켜줌
    void Patrol()
    {
        // 포인트 돌아가면서 순찰
        if (enemy.velocity == Vector3.zero)
        {
            enemy.SetDestination(tfWayPoints[count++].position);
            if (count >= tfWayPoints.Length)
                count = 0;
        }
    }

    void UpdateTarget()

    /* 
     * 추적해야할 플레이어들의 타겟을 업데이트
     * 타겟의 layerMask는 "Player"로 해야 인식 가능
     * 인식된 Player들중 가장 가까운 것을 타겟으로 설정
     */

    {
        Collider[] cols = Physics.OverlapSphere(transform.position, distance, layerMask); // (중심, 반경, layer)
        if (cols.Length > 0) // 주변에 1개이상 콜라이더 검출되면
        {
            Debug.Log("플레이어 검출됨");
            int minIndex = 0;
            float minDistance = (transform.position - cols[0].transform.position).magnitude;
            for (int i = 0; i < cols.Length; i++)
            {
                // 추출된 콜라이더(플레이어위치) 와 순찰자 위치 비교하여 제일 가까운 플레이어를 타겟으로
                if ((transform.position - cols[i].transform.position).magnitude < minDistance)
                {
                    minIndex = i;
                    minDistance = (transform.position - cols[i].transform.position).magnitude;
                }
            }
            target = cols[minIndex].transform;
            
        }
    }

    public void Chase()
    {

        // 타겟이 있고 > 시야각 안에 들어왔으면 > 순찰 취소 > 추격
        // 타겟이 있고 > 시야각 안에 없으면 > 순찰
        if (target != null)
        {
            Vector3 targetDirection = (target.position - transform.position).normalized;
            float targetAngle = Vector3.Angle(targetDirection, transform.forward);

            if (targetAngle < viewAngle * 0.5f) // 범위 안에 있고 시야각 안에 있음
            {
                CancelInvoke();
                enemy.SetDestination(target.position);


            }
            else
            {
                InvokeRepeating("Patrol", 0f, 2f);
            }
        }
        else if (target == null) // 범위에 없음
        {
            InvokeRepeating("Patrol", 0f, 2f);
        }
    }

    // 위험 지역에 플레이어가 들어왔는지 확인, 확인되면 기존 순찰을 취소 후 플레이어 추적
    public void CheckDanger(Transform dangerTarget)
    {
        CancelInvoke();
        enemy.SetDestination(dangerTarget.position);
    }

    // 최초 순찰지역 인식 기능
    void FindPatrolPoint()
    {
        // 레이저로 순찰지역 인식 distance 표시
        Debug.DrawRay(transform.position, transform.forward * detect_distance, Color.red);

        Collider[] p_points = Physics.OverlapSphere(transform.position, detect_distance, layerMask); // (중심, 반경, layer)
        Debug.Log(p_points.Length);


    }
          
    
    
    
}
