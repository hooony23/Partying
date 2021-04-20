using UnityEngine;
using UnityEngine.AI;
using GameManager;

// ++ : 패턴은 8초 이후에 동작하도록
// 0. 플레이어 피격 처리
// 1. 패턴은 8초 이후에 랜덤으로 일으킴
// 2. 플레이어 인식하고 플레이어 에임패턴과, 전체 공격 패턴으로 나눔
// 3. 아이들 애니메이션 새로 만들기(낮은)
// 4. 모든 애니메이션 수정


// 보스의 체력, 상태, 패턴 등을 기록
// 보스 패턴 중 선택
// 플레이어 데미지 입힘
namespace Boss
{
    public class Boss : BossUtil
    {

        void Start()
        {
            GM = GameObject.Find("GameManager").GetComponent<RaidGameManager>();
            Animator = GetComponent<Animator>();

            ChargingL = GameObject.Find("Charging Laser").GetComponent<ParticleSystem>();
            OctaL = GameObject.Find("Octa Laser").GetComponent<ParticleSystem>();

            PlayerMask = LayerMask.GetMask("Player");

            NavMeshAgent = GetComponent<NavMeshAgent>();

            InitParticleSystem();
            if (GM.PlayerList.Count > 0)
            {
                StartCoroutine(WakeUp());
            }

            BossCollider = GetComponent<SphereCollider>();

        }
        void Update()
        {
            TargetList.Clear();
            foreach (var player in GM.PlayerList) { TargetList.Add(player.GetComponent<Transform>()); }

            CheckHP();
        }


    }

}