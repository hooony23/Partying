using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIView : MonoBehaviour
{
    [SerializeField] float viewAngle = 0f; // AI시야각
    [SerializeField] float distance = 0f; // 시야의 거리
    [SerializeField] LayerMask layerMask = 0; // LayerMask를 통해 인식함



    // Update is called once per frame
    void Update()
    {
        Sight();
    }

    void Sight()
    {
        // OverlapSphere : 주변에 있는 layerMask의 콜라이더 검출, 검출된 객체의 트랜스폼 정보를 가져올 수 있음
        Collider[] cols = Physics.OverlapSphere(transform.position, distance, layerMask);

        if (cols.Length > 0) // 감지됨
        {
            Transform tfplayer = cols[0].transform; // 플레이어 1명기준 검출되면 cols[0] 이 플레이어 콜라이더

            // AI의 방향과 플레이어가있는 위치의 각도차이가 시야각보다 작은지 확인, 작으면 chase
            Vector3 targetDirection = (tfplayer.position - transform.position).normalized; // AI기준 타겟의 방향
            float targetAngle = Vector3.Angle(targetDirection, transform.forward); // AI의 정면과 타켓의 각도

            if (targetAngle < viewAngle * 0.5f)
            {
                // 플레이어에게 Ray 를 쏴서 장애물이 없는지 확인, Ray에 닿은 것이 Player이면 chase
                if (Physics.Raycast(transform.position, targetDirection, out RaycastHit targetHit, distance))
                {
                    if (targetHit.transform.tag == "Player")
                    {
                        // 장애물 없음, 몬스터가 점점 가까이 감
                        transform.position = Vector3.Lerp(transform.position, targetHit.transform.position, 0.02f);
                        
                    }
                }
            }
        }
    }

    
}
