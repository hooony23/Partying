using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

/* 순찰자의 순찰, 인식, 추격 기능 
 * Target을 Player로 둘수 있고, 다른것을 쫒게 할 수도 있음
 * 추격할 타겟의 태그를 "Player", LayerMask를 "Player"라고 설정해야함
 */

public class PatrolAI : PatrolAIUtil
{


    //@@@ 서버 @@@
    AIMove aiMove = new AIMove();


    NavMeshAgent navPatrol;
    private void Awake()
    {
        pac.LayerMaskPlayer = LayerMask.GetMask("Player");
        pac.LayerMaskPpoint = LayerMask.GetMask("PatrolPoint");
        pac.Patrol = gameObject.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        pac.LastPpoint = transform;

        InvokeRepeating("FindPatrolPoint", 0f, 1.5f);
    }

    void Update()
    {
        // Debug.Log(pac.Patrol.);
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


}
