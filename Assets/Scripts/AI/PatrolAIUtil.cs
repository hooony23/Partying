using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Communication;
using Communication.JsonFormat;

public class PatrolAIUtil :PatrolAIController
{
    public void UpdatePatrolTarget()
    {
        Collider[] cols = Physics.OverlapSphere(Patrol.transform.position, DetectDistance, LayerMaskPlayer); // (중심, 반경, layer)
        List<Collider> cols_visible = new List<Collider>();
        if (cols.Length > 0) // 주변에 감지된 Player콜라이더 1개이상이면
        {
            Debug.Log("플레이어 검출됨");

            // 1. cols 중 시야 안에 들어온 것만 배열에 추가
            for (int i = 0; i < cols.Length; i++)
            {
                Vector3 target_visible = cols[i].transform.position;
                Vector3 targetDirection = (target_visible - Patrol.transform.position).normalized;
                float targetAngle = Vector3.Angle(targetDirection, Patrol.transform.forward);

                // 0.5f : 시야각 forward 기준으로 한쪽방향 * 2
                if (targetAngle < ViewAngle * 0.5f)
                {
                    cols_visible.Add(cols[i]);
                }
            }

            // 2. cols_visible 은 distance안에 들어왔고 시야각 안에 노출된 플레이어들이다
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
                NearestPlayer = cols_visible[minIndex].transform;

                // 주변에 플레이어 감지되었고, 시야각 안에 들어옴
                Debug.Log("플레이어 발견");
                IsPatrol = false;
                Target = NearestPlayer;
            }

            // 주변에 플레이어 감지 되었고, 시야각에서 사라짐
            else
            {
                Debug.Log("플레이어 놓침");
                IsPatrol = true;

            }
        }

        // 주변에 플레이어 없음
        // Debug.Log("순찰 재시작");
        IsPatrol = true;

    }

    public void Move()
    {
        if(!Lib.Common.IsAdmin()){
            NetworkMove();
            return;
        }
        // 레이저로 순찰지역 인식 distance 표시
        Debug.DrawRay(Patrol.transform.position, Patrol.transform.forward * DetectDistance, Color.red);
        //Debug.DrawRay(transform.position, transform.forward * patrol_distance, Color.blue);
        if (Target)
            Patrol.SetDestination(Target.position);

    }
    public void NetworkSync()
    {
        if(NetworkInfo.aiInfo != null)
        {
            AiInfo = NetworkInfo.aiInfo;
            this.gameObject.transform.position = AiInfo.GetLocToVector3();
            NetworkInfo.aiInfo=null;
        }
    }
    public void NetworkMove()
    {
        this.gameObject.transform.position += AiInfo.GetVecToVector3();
    }

    // 위험 지역에 플레이어가 들어왔는지 확인, 확인되면 기존 순찰을 취소 후 플레이어 추적
    public void CheckDanger(Transform dangerTarget)
    {
        if(dangerTarget==null)
            FindPatrolPoint();
        else
            Target = dangerTarget;
    }

    // 최초 순찰지역 인식 기능
    public void FindPatrolPoint()
    {
        if (Patrol.velocity == Vector3.zero || IsPatrol == true)
        {
            // 주변 패트롤 포인트 인식
            Collider[] cols = Physics.OverlapSphere(Patrol.transform.position, PatrolDistance, LayerMaskPpoint); // (중심, 반경, layer)
            List<Collider> points = new List<Collider>(cols);
            if (cols.Length > 0)
            {
                // 인식된 PatrolPoints 들중 1개만 랜덤으로 선택
                int ridx = Random.Range(0, points.Count);
                Target = points[ridx].transform;
                LastPpoint = Target;
            }
            else // 주변에 PatrolPoint 가 없는 Patrol은
            {
                // 플레이어 올때까지 기다린다
                Target = null;
            }
        }

    }
}
