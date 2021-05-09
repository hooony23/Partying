using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Communication;
using Communication.JsonFormat;
namespace Boss
{
    // 작동순서
    // 1. 플레이어 인식(최초 혹은 플레이어 죽을 시에 1번 발생)
    // 2. 보스는 패턴을 선택 패턴은 플레이어 1명 조준을 포함한 에임 패턴과 전체 패턴으로 나뉨
    // 3. 패턴 실행시 플레이어에게 데미지
    public class BossUtil : BossController
    {

        // 모든 자식 파티클 시스템 정지상태로 초기화
        public void InitParticleSystem()
        {
            ParticleSystem[] pc;
            pc = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < pc.Length; i++)
            {
                pc[i].Stop();

            }
        }

        // 패턴 선택, 드론 Wakeup(10초) 이후에 시작
        public void Think()
        {
            if (!PatternActivated)
            {
                Debug.Log(System.Enum.GetName(typeof(BossInfo.Patterns), Pattern));

                Debug.Log("현재패턴 :" + Pattern);
                switch (Pattern)
                {
                    case BossInfo.Patterns.CHANGINGELAGER:
                        PatternActivated = true;
                        StartCoroutine(ChargingLaser());
                        break;
                    case BossInfo.Patterns.OCTALASER:
                        PatternActivated = true;
                        StartCoroutine(OctaLaser());
                        break;
                    case BossInfo.Patterns.BODYSLAM:
                        PatternActivated = true;
                        StartCoroutine(BodySlam());
                        break;

                }
            }
        }

        public IEnumerator Aim()
        {
            Transform targetPlayer = GM.GetPlayerGameObject(Target).transform;
            float rotationSpeed = 100f;

            Quaternion targetRotation = Quaternion.identity;
            do
            {
                Vector3 targetDirection = targetPlayer.position - this.transform.position;
                targetDirection = targetDirection - new Vector3(0, 0, 0);
                targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion nextRotation = Quaternion.RotateTowards(
                        this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                this.transform.rotation = nextRotation;
                yield return null;
            } while (Quaternion.Angle(this.transform.rotation, targetRotation) > 0.01f);
        }

        public IEnumerator AimReset()
        {
            float rotationSpeed = 100f;

            Quaternion targetRotation = Quaternion.identity;
            do
            {
                targetRotation = Quaternion.Euler(0, 0, 0);

                Quaternion nextRotation = Quaternion.RotateTowards(
                    this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                this.transform.rotation = nextRotation;
                yield return null;
            } while (Quaternion.Angle(this.transform.rotation, targetRotation) > 0.01f);

        }

        // 보스 패턴 //
        public IEnumerator WakeUp()
        {
            AnimController.SetTrigger("WakeUp");
            yield return new WaitForSeconds(10f);
            AnimController.SetTrigger("Idle");
            BossCollider.enabled = true;
            yield return new WaitForSeconds(1f);

            PatternActivated = false;
        }

        public IEnumerator ChargingLaser()
        {
            StartCoroutine(Aim());
            yield return new WaitForSeconds(1f);

            ChargingL.Play();
            yield return new WaitForSeconds(8f);
            
            StartCoroutine(AimReset());
            yield return new WaitForSeconds(2f);
            PatternActivated = false;
        }

        public IEnumerator OctaLaser()
        {
            OctaL.Play();                           // 파티클 시스템 플레이
            AnimController.SetTrigger("OctaLaser1");      // 레이저 총구 각도 변환 애니메이션
            yield return new WaitForSeconds(8f);

            PatternActivated = false;
        }

        public IEnumerator BodySlam()
        {
            if (!Target.Equals(""))
            {

                WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
                float duration = 0.5f; // 타겟에 도달하는 시간, 짧을수록 패턴이 빨라짐
                float distanceRatio = 30f; // 플레이어를 넘어 지나가는 거리

                AnimController.SetTrigger("BodySlam1");
                BossCollider.enabled = false;
                yield return new WaitForSeconds(2.5f);

                Transform target = GameObject.Find(Target.ToString()).GetComponent<Transform>();
                Vector3 targetPos = new Vector3(target.position.x, this.transform.position.y, target.position.z);

                // 플레이어를 넘어 지나가도록 함
                Vector3 addDistance = (targetPos - this.transform.position).normalized * distanceRatio;
                targetPos += addDistance;
                // 추격 루틴
                while (duration > 0.0f)
                {
                    duration -= Time.deltaTime;

                    this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime / duration);

                    yield return waitForEndOfFrame;
                }

                yield return new WaitForSeconds(7.5f);
                BossCollider.enabled = true;
                PatternActivated = false;
            }
        }
        
        public IEnumerator Destroyed()
        {
            AnimController.Play("Destroyed");
            BossCollider.enabled = false;
            yield return new WaitForSeconds(1f);
        }

        // BodySlam 랜덤 타겟 선택 
        public int GetRanPlayerIdx()
        {
            int idx = Random.Range(0, GM.PlayerList.Count);

            return idx;
        }

        protected void UpdateBossInfo()
        {
            if (NetworkInfo.bossInfo !=null)
            {
                //this.transform.position = NetworkInfo.bossInfo.GetLocToVector3();
                this.GetComponent<Rigidbody>().velocity = NetworkInfo.bossInfo.GetVecToVector3();
                // if(boss.GetComponent<Boss.Boss>().BossHP!= NetworkInfo.bossInfo.BossHP)
                // {
                //     boss.Animator.SetTriger("Hit");
                // }
                Target = NetworkInfo.bossInfo.Target;
                Pattern = NetworkInfo.bossInfo.pattern;
                BossHP = NetworkInfo.bossInfo.BossHP;
                NetworkInfo.bossInfo = null;
            }
        }
        protected void InitBossInfo()
        {
            this.transform.position = NetworkInfo.bossInfo.GetLocToVector3();
            UpdateBossInfo();
        }
        public void CheckHP()
        {
            //TODO: 보스 패턴이 죽음으면 클리어.
            if (BossHP <= 0)
            {
                InitParticleSystem();
                StartCoroutine(Destroyed());
            }
        }

        // 보스 물리적 충돌 시 떨림 제거
        public void FreezeVelocity()
        {
            BossRigid.velocity = Vector3.zero;
            BossRigid.angularVelocity = Vector3.zero;
        }

        // Destroyed2 애니메이션 안에 포함됨
        public void GameOver()
        {
            var boss = GameObject.Find("Boss");
            var bossInfo = boss.GetComponent<Boss>();
            if (bossInfo.Pattern == Communication.JsonFormat.BossInfo.Patterns.DIE)
            {
                var gameManager = GameObject.Find("GameManager").GetComponent<GameManager.RaidGameManager>();
                gameManager.GameClear = true;
            }
        }

    }
}