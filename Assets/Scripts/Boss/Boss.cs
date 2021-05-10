using UnityEngine;
using UnityEngine.AI;
using Communication;
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
    public class Boss : BossUtil, IDamageable
    {

        void Start()
        {
            InitBossInfo();

            GM = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
            AnimController = GetComponent<Animator>();

            ChargingL = GameObject.Find("Charging Laser").GetComponent<ParticleSystem>();
            OctaL = GameObject.Find("Octa Laser").GetComponent<ParticleSystem>();

            PlayerMask = LayerMask.GetMask("Player");

            InitParticleSystem();
            if (GM.PlayerList.Count > 0)
            {
                StartCoroutine(WakeUp());
            }

            BossRigid = GetComponent<Rigidbody>();
            BossCollider = GetComponentInChildren<SphereCollider>();
            
        }
        void Update()
        {
            UpdateBossInfo();
            CheckHP();
            Think();

            FreezeVelocity();
        }

        // 보스 플레이어 충돌시(BodySlam, 지나가다가 충돌)
        private void OnCollisionEnter(Collision collision)
        {
            // 총알과 구분하기 위해 Tag 와 name 비교
            if (collision.transform.CompareTag("Player") && collision.transform.name.Equals(Target))
            {
                // 플레이어에게 공격
                TakeHit(collision.collider, 1);
            }

        }

        public void TakeHit(Collider collider, float damage)
        {
            Debug.Log("플레이어를 공격함");
            var player = collider.gameObject.GetComponent<Player>();
            var reactVec = (collider.transform.position - this.transform.position).normalized;
            if (!player.IsBeatable)
                return;
            player.PlayerHealth -= damage;
            StartCoroutine(player.OnAttacked(reactVec));
        }
    }

}