
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Communication;
using Communication.JsonFormat;
using Communication.GameServer.API;
using Lib;
using Util;

public class PatrolAIUtil : PatrolAIController, IDamageable
{
    private bool isReport = false;
    public void SearchPlayers()
    {
        Collider[] cols = Physics.OverlapSphere(Patrol.transform.position, DetectDistance, LayerMaskPlayer); // (중심, 반경, layer)
        List<Collider> cols_visible = new List<Collider>();
        // cols_visible 은 distance안에 들어왔고 시야각 안에 노출된 플레이어들이다
        // 그 중에서 가장 거리가 가까운 플레이어만 본다
        if (cols_visible.Count > 0)
        {
            int minIndex = 0;
            float minDistance = (Patrol.transform.position - cols_visible[0].transform.position).magnitude;
            for (int i = 0; i < cols_visible.Count; i++)
            {
                // 추출된 콜라이더(플레이어위치) 와 순찰자 위치 비교하여 제일 가까운 플레이어를 타겟으로
                if ((Patrol.transform.position - cols[i].transform.position).magnitude < minDistance)
                {
                    minIndex = i;
                    minDistance = (Patrol.transform.position - cols[i].transform.position).magnitude;
                }
            }
            IsDetectedPlayer(cols_visible[minIndex].transform);
        }

        // 주변에 플레이어 감지 되었고, 시야각에서 사라짐
        else
        {
            Debug.Log("플레이어 놓침");
            APIController.SendController("UnDetected", AiInfo.GetInstance(this.gameObject.name,Target.gameObject.name));
        }
    }
    public void IsDetectedPlayer(Transform targetPlayerTransform)
    {


        // 주변에 플레이어 감지되었고, 시야각 안에 들어옴
        var nearestPlayer = targetPlayerTransform;
        Debug.Log("플레이어 발견");
        IsPatrol = false;
        isReport=false;
        Target = nearestPlayer;
        APIController.SendController("IsDetected", AiInfo.GetInstance(this.gameObject.name,Target.gameObject.name));
    }
    public void Move()
    {
        // 레이저로 순찰지역 인식 distance 표시
        Debug.DrawRay(Patrol.transform.position, Patrol.transform.forward * DetectDistance, Color.red);
        //Debug.DrawRay(transform.position, transform.forward * patrol_distance, Color.blue);
        if (Target)
            Patrol.SetDestination(Target.position);
    }

    public void FinishSearch()
    {
        if (this.transform == Target && !isReport)
        {
            APIController.SendController("AIFinishSearch", AiInfo.GetInstance(this.gameObject.name,Target.gameObject.name));
            isReport = true;
        }
    }

    // 위험 지역에 플레이어가 들어왔는지 확인, 확인되면 기존 순찰을 취소 후 플레이어 추적
    public void CheckDanger(Transform dangerTarget)
    {
        if (dangerTarget == null)
            FindPatrolPoint();
        else
            Target = dangerTarget;
    }
    public void SetTargetPoint()
    {
        var aiInfo = AiInfo.GetValue();
        if(aiInfo.Uuid.Equals(this.gameObject.name))
        {
            TargetPoint = aiInfo.Target;
            AiInfo.ClearValue();
            isReport=false;
        }
    }
    // 최초 순찰지역 인식 기능
    public void FindPatrolPoint()
    {
        if (Patrol.velocity == Vector3.zero || IsPatrol == true)
        {
            // 주변 패트롤 포인트 인식
            Collider[] cols = Physics.OverlapSphere(Patrol.transform.position, PatrolDistance, LayerMaskPpoint); // (중심, 반경, layer)
            var data = (from patrolPoint in cols where patrolPoint.gameObject.name.Equals(TargetPoint) select patrolPoint).FirstOrDefault();
            if (data != null)
            {
                // 인식된 PatrolPoints 들중 1개만 랜덤으로 선택
                Target = data.transform;
                LastPpoint = Target;
            }
            else // 주변에 PatrolPoint 가 없는 Patrol은
            {
                // 플레이어 올때까지 기다린다
                Target = null;
            }
        }

    }
    public void TakeHit(Collider collider, float damage = 999)
    {
        if (collider.transform.CompareTag("Player"))
            collider.gameObject.GetComponent<Player>().PlayerHealth -= damage;
    }
}
