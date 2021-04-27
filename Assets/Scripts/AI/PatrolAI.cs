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


    //@@@ 서버 @@@
    AIMove aiMove = new AIMove();

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
        /*
        "aiUuid": "af2b16e7-b829-4721-8c8d-e85e4381a4fd",
        "loc": "{}",
        "vec": "{}",
        "targetPoint": "{x,y,z}"
        */

        /*
                      "x": 12,
            "y": 1,
            "z": 8
        },
        "vec": {
            "x": 1.0,
            "y": 0,
            "z": -0.5
        },
        "movement": "Move",
        "uuid": "af2b16e7-b829-4721-8c8d-e85e4381a4fd",
        "isDetected": true,
        "targe": "b2a6938e-8285-48b9-b0cd-017df4ed029b"
        */
        var currentVec = Patrol.desiredVelocity;
        if(this.AiInfo.GetVecToVector3()!= currentVec){
            this.AiInfo.Vec=new Division3(currentVec);
            this.AiInfo.Loc = new Division3(this.gameObject.transform.position);
            APIController.SendController("AiMove",new Division3(currentVec));
        }
        UpdatePatrolTarget();
        if(!NetworkInfo.roomInfo.Admin.UserUuid.Equals(Config.userUuid))
        {
            NetworkSync();
            NetworkMove();
        }
        else
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
