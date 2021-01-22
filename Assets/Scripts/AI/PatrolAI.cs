using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using project;

/* 순찰자의 순찰, 인식, 추격 기능 
 * Target을 Player로 둘수 있고, 다른것을 쫒게 할 수도 있음
 * 추격할 타겟의 태그를 "Player", LayerMask를 "Player"라고 설정해야함
 */

public class PatrolAI : MonoBehaviour
{
    // 시야각 에 따른 순찰
    [SerializeField] float viewAngle = 0f; // 시야각
    [SerializeField] float detect_distance = 0f; // 반경
    [SerializeField] LayerMask layerMask_player = 0; // OverlapSphere : LayerMask를 통해 인식함
    [SerializeField] LayerMask layerMask_ppoint = 0; // ppoint : PatrolPoint

    // 주변의 패트롤포인트를 인식하고 인식된 포인트에서만 순찰
    Transform last_ppoint;
    Transform nearestPlayer = null;
    float patrol_distance = 17; // 순찰지역 인식 거리
    bool isPatrol;

    // 추격
    NavMeshAgent patrol = null;
    Transform target; // 타켓이 정해지면 움직임 ( target : player, ppoint ...)


    //@@@ 서버 @@@
    AIMove aiMove = new AIMove();

    private void Awake()
    {
        patrol = GetComponent<NavMeshAgent>();

        // @@@ 서버 @@@
        //AsynchronousClient.Connected();
        
    }
    void Start()
    {
        last_ppoint = transform;
        InvokeRepeating("FindPatrolPoint", 0f, 1.5f);

        

    }

    private void OnApplicationQuit()
    {
        // @@@ 서버 @@@
        //AsynchronousClient.ConnectedExit();
    }

    void Update()
    {
        UpdatePatrolTarget();
        Move(); // 순찰, 추격, 위험지역 확인
        
        
    }

    private void FixedUpdate()
    {
        // AI정보 서버에 보냄
        //string aiUuid = "aiUuid-123123-123123";
        //Vector3 moveVec = Vector3.zero;
        //aiMove.UpdateAiInfo(aiUuid, transform.position, moveVec, target);
        //string jsonData = aiMove.ObjectToJson(aiMove);
        //Debug.Log(jsonData);
        //AsynchronousClient.Send(jsonData);

    }

    // 다음 정찰지역으로 이동시켜줌

    void UpdatePatrolTarget()
    {
        
        Collider[] cols = Physics.OverlapSphere(transform.position, detect_distance, layerMask_player); // (중심, 반경, layer)
        List<Collider> cols_visible = new List<Collider>();
        if (cols.Length > 0) // 주변에 감지된 Player콜라이더 1개이상이면
        {
            Debug.Log("플레이어 검출됨");

            // 1. cols 중 시야 안에 들어온 것만 배열에 추가
            for (int i = 0; i < cols.Length; i++)
            {
                Vector3 target_visible = cols[i].transform.position;
                Vector3 targetDirection = (target_visible - transform.position).normalized;
                float targetAngle = Vector3.Angle(targetDirection, transform.forward);
                if (targetAngle < viewAngle * 0.5f)
                {
                    cols_visible.Add(cols[i]);
                }
            }

            // 2. cols_visible 은 distance안에 들어왔고 시야각 안에 노출된 플레이어들이다
            // 그 중에서 가장 거리가 가까운 플레이어만 본다
            if (cols_visible.Count > 0)
            {
                int minIndex = 0;
                float minDistance = (transform.position - cols_visible[0].transform.position).magnitude;
                for (int i = 0; i < cols_visible.Count; i++)
                {
                    // 추출된 콜라이더(플레이어위치) 와 순찰자 위치 비교하여 제일 가까운 플레이어를 타겟으로
                    if ((transform.position - cols[i].transform.position).magnitude < minDistance)
                    {
                        minIndex = i;
                        minDistance = (transform.position - cols[i].transform.position).magnitude;
                    }
                }
                nearestPlayer = cols_visible[minIndex].transform;

                // 주변에 플레이어 감지되었고, 시야각 안에 들어옴
                Debug.Log("플레이어 발견");
                isPatrol = false;
                target = nearestPlayer;
            }

            // 주변에 플레이어 감지 되었고, 시야각에서 사라짐
            else
            {
                Debug.Log("플레이어 놓침");
                isPatrol = true;
                
            }
        }

        // 주변에 플레이어 없음
        Debug.Log("순찰 재시작");
        isPatrol = true;
 
    }

    public void Move()
    {
        // 레이저로 순찰지역 인식 distance 표시
        Debug.DrawRay(transform.position, transform.forward * detect_distance, Color.red);
        //Debug.DrawRay(transform.position, transform.forward * patrol_distance, Color.blue);
        if(target)
            patrol.SetDestination(target.position);
        
    }

    // 위험 지역에 플레이어가 들어왔는지 확인, 확인되면 기존 순찰을 취소 후 플레이어 추적
    public void CheckDanger(Transform dangerTarget)
    {
        target = dangerTarget;
    }

    // 최초 순찰지역 인식 기능
    void FindPatrolPoint()
    {
        if (patrol.velocity == Vector3.zero || isPatrol == true)
        {
            // 주변 패트롤 포인트 인식
            Collider[] cols = Physics.OverlapSphere(transform.position, patrol_distance, layerMask_ppoint); // (중심, 반경, layer)
            List<Collider> points = new List<Collider>(cols);

            if (cols.Length > 0)
            {
                // 인식된 PatrolPoints 들중 1개만 랜덤으로 선택
                int ridx = Random.Range(0, points.Count);
                target = points[ridx].transform;
                last_ppoint = target;
            }
            else // 주변에 PatrolPoint 가 없는 Patrol은
            {
                // 플레이어 올때까지 기다린다
                target = null;
            }
        }     

    }   
}
