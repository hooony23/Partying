using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;
using Communication;
using Communication.JsonFormat;
using Communication.GameServer;
using Communication.GameServer.API;
using Lib;
using Util;

/* 순찰자의 순찰, 인식, 추격 기능 
 * Target을 Player로 둘수 있고, 다른것을 쫒게 할 수도 있음
 * 추격할 타겟의 태그를 "Player", LayerMask를 "Player"라고 설정해야함
 */

public class PatrolAI : PatrolAIUtil
{
    NavMeshAgent navPatrol;
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
        var currentVec = Patrol.desiredVelocity;
        if(this.AiInfo.GetVecToVector3()!= currentVec){
            this.AiInfo.Vec=new Division3(currentVec);
            this.AiInfo.Loc = new Division3(this.gameObject.transform.position);
            APIController.SendController("AiMove",new Division3(currentVec));
        }
        UpdatePatrolTarget();
        if(!Lib.Common.IsAdmin())
        {
            NetworkSync();
        }
        Move(); // 순찰, 추격, 위험지역 확인


    }


}
