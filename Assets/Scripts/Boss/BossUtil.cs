using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Boss
{
    // 작동순서
    // 1. 플레이어 인식(최초 혹은 플레이어 죽을 시에 1번 발생)
    // 2. 보스는 패턴을 선택 패턴은 플레이어 1명 조준을 포함한 에임 패턴과 전체 패턴으로 나뉨
    // 3. 패턴 실행시 플레이어에게 데미지
    public class BossUtil : BossController
    {

        private enum Patterns { CHANGINGELAGER, OCTALASER, BODYSLAM,IDLE }

        // 주변의 Layer : Player 인 오브젝트 4개 검출

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
        public IEnumerator Think()
        {
            yield return new WaitForSeconds(0.1f);

            if (GM.PlayerList.Count <= 0)
            {
                Animator.SetTrigger("Idle");
                yield return null;
            }
            else
            {
                int ranAction = Random.Range(0, 3);
                switch (ranAction)
                {
                    case (int)Patterns.CHANGINGELAGER:
                        StartCoroutine(ChargingLaser());
                        break;
                    case (int)Patterns.OCTALASER:
                        StartCoroutine(OctaLaser());
                        break;
                    case (int)Patterns.BODYSLAM:
                        StartCoroutine(BodySlam());
                        break;
                }
            }
        }

        public IEnumerator Aim()
        {
            Transform targetPlayer = GM.PlayerList[GetRanPlayerIdx()].transform;
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
                targetRotation = Quaternion.Euler(0, this.transform.rotation.y, this.transform.rotation.z);

                Quaternion nextRotation = Quaternion.RotateTowards(
                    this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                this.transform.rotation = nextRotation;
                yield return null;
            } while (Quaternion.Angle(this.transform.rotation, targetRotation) > 0.01f);

        }

        // 보스 패턴 //
        public IEnumerator WakeUp()
        {
            Animator.SetTrigger("WakeUp");
            yield return new WaitForSeconds(7f);
            Animator.SetTrigger("Idle");
            BossCollider.enabled = true;
            yield return new WaitForSeconds(3f);

            StartCoroutine(Think());
        }

        public IEnumerator ChargingLaser()
        {
            StartCoroutine(Aim());
            yield return null;

            ChargingL.Play();
            yield return new WaitForSeconds(8f);

            StartCoroutine(AimReset());
            StartCoroutine(Think());
        }

        public IEnumerator OctaLaser()
        {
            OctaL.Play();                           // 파티클 시스템 플레이
            Animator.SetTrigger("OctaLaser1");      // 레이저 총구 각도 변환 애니메이션
            yield return new WaitForSeconds(8f);

            StartCoroutine(Think());
        }

        public IEnumerator BodySlam()
        {
            float atkSpeed = 2f;
            int targetIdx = GetRanPlayerIdx();
 
            Animator.SetTrigger("BodySlam1");
            BossCollider.enabled = false;           // 보스 지면으로 내려옴, 지면에서 보스 무적
            yield return new WaitForSeconds(1.5f);

            // 추격 루틴

            yield return new WaitForSeconds(8);
            BossCollider.enabled = true;

           

            StartCoroutine(Think());
        }

        public IEnumerator Destroyed()
        {
            Animator.Play("Destroyed");
            BossCollider.enabled = false;
            yield return new WaitForSeconds(10f);

            // 보스 게임오브젝트 제거
            Destroy(this.gameObject);
        }

        // BodySlam 랜덤 타겟 선택 
        public int GetRanPlayerIdx()
        {
            int idx = Random.Range(0, GM.PlayerList.Count);

            return idx;
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

    }
}