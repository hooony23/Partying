using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Util;
namespace Boss
{
    public class BossController : MonoBehaviour
    {
        public float RadarRange { get; set; } = Config.radarRange;
        public float BossHP { get; set; } = Config.bossHP;
        public GameManager.GameManager GM { get; set; }
        public List<Transform> TargetList { get; set; }
        public ParticleSystem ChargingL { get; set; }
        public ParticleSystem OctaL { get; set; }
        public Animator Animator { get; set; }
        public LayerMask PlayerMask { get; set; } = 0;
        public NavMeshAgent NavMeshAgent { get; set; }

        // public void UpdateTargetList()
        // {
        //     Collider[] cols = Physics.OverlapSphere(transform.position, RadarRange, PlayerMask);

        //     GM.PlayerList.Clear();
        //     TargetList.Clear();

        //     if (cols.Length == 0)
        //     {
        //         /// 게임 종료 ///
        //     }

        //     else if (cols.Length > 5)
        //     {
        //         /// 초대받지 않은 손님이 있다!! ///
        //     }

        //     else
        //     {
        //         if (cols.Length > 0 && cols.Length < 5)
        //         {
        //             for (int i = 0; i < cols.Length; i++)
        //             {
        //                 // 추후 UUID 값 리스트로 받기 : Add(cols[i].transform.name);
        //                 TargetList.Add(GM.PlayerList[i].transform.Find("Boss Aim"));
        //             }
        //         }
        //     }
        // }

    }
}

