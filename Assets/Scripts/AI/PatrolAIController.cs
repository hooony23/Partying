using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Partying.Assets.Scripts.Util;


public class PatrolAIController
{
    // 시야각 에 따른 순찰
    private LayerMask layerMaskPlayer = 0; // OverlapSphere : LayerMask를 통해 인식함
    private Transform nearestPlayer;
    private float viewAngle = Config.patrolVisionAngle; // 시야각
    private float detectDistance = Config.playerDetectDistance; // 반경

    // 주변의 패트롤포인트를 인식하고 인식된 포인트에서만 순찰
    private LayerMask layerMaskPpoint = 0; // Ppoint LayerMask
    private Transform lastPpoint;
    private float patrolDistance = Config.patrolPointFindDistance; // 순찰지역 인식 거리
    private bool isPatrol;

    // 추격
    private NavMeshAgent patrol;
    private Transform target; // 타켓이 정해지면 움직임 ( target : player, ppoint ...)

    public LayerMask LayerMaskPlayer { get => layerMaskPlayer; set => layerMaskPlayer = value; }
    public Transform NearestPlayer { get => nearestPlayer; set => nearestPlayer = value; }
    public float ViewAngle { get => viewAngle; set => viewAngle = value; }
    public float DetectDistance { get => detectDistance; set => detectDistance = value; }
    public LayerMask LayerMaskPpoint { get => layerMaskPpoint; set => layerMaskPpoint = value; }
    public Transform LastPpoint { get => lastPpoint; set => lastPpoint = value; }
    public float PatrolDistance { get => patrolDistance; set => patrolDistance = value; }
    public bool IsPatrol { get => isPatrol; set => isPatrol = value; }
    public NavMeshAgent Patrol { get => patrol; set => patrol = value; }
    public Transform Target { get => target; set => target = value; }
}
