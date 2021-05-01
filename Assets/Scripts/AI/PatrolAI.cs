using UnityEngine;
using UnityEngine.AI;


public class PatrolAI : PatrolAIUtil
{
    /// <summary>
    /// 순찰자의 순찰, 인식, 추격 기능 
    /// Target을 Player로 둘수 있고, 다른것을 쫒게 할 수도 있음
    /// 추격할 타겟의 태그를 "Player", LayerMask를 "Player"라고 설정해야함
    /// </summary>
    private void Awake()
    {
        LayerMaskPlayer = LayerMask.GetMask("Player");
        LayerMaskPpoint = LayerMask.GetMask("PatrolPoint");
        Patrol = gameObject.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        LastPpoint = transform;

        InvokeRepeating("FindPatrolPoint", 0f, 1.5f);
    }

    void Update()
    {
        
        if(!Lib.Common.IsAdmin())
        {
            NetworkSync();
        }
        else{
            UpdatePatrolTarget();
            MoveChangeSend();
        }
        Move(); // 순찰, 추격, 위험지역 확인


    }


}
