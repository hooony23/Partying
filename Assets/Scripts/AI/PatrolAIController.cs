using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Util;


public class PatrolAIController
{

    // 시야각 에 따른 순찰
    public LayerMask LayerMaskPlayer { get; set; } = 0; // OverlapSphere : LayerMask를 통해 인식함
    public Transform NearestPlayer { get; set; }
    public float ViewAngle { get; set; } = Config.patrolVisionAngle; // 시야각
    public float DetectDistance { get; set; } = Config.playerDetectDistance; // 반경
    
    // 주변의 패트롤포인트를 인식하고 인식된 포인트에서만 순찰
    public LayerMask LayerMaskPpoint { get; set; } = 0; // Ppoint LayerMask
    public Transform LastPpoint { get; set; }
    public float PatrolDistance { get; set; }= Config.patrolPointFindDistance; // 순찰지역 인식 거리
    public bool IsPatrol { get; set ; }
    
    // 추격
    public NavMeshAgent Patrol { get; set ; }
    public Transform Target { get; set; } // 타켓이 정해지면 움직임 ( target : player, ppoint ...)
}
