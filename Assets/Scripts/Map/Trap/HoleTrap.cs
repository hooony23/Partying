using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrap : MonoBehaviour
{
    private bool holeActivate = false;
    private Collider playerColl;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        // holeActive 가 false 상태(처음으로 함정을 밟음) 이면 함정을 발동
        if (other.CompareTag("Player") && !holeActivate)
        {
            Debug.Log("함정 발동");
            anim.SetTrigger("open");
        }

        // 발동된 함정은 holeActive 가 true 상태
        if (other.CompareTag("Player") && holeActivate)
        {
            Player player = other.GetComponent<Player>();
            playerColl = player.GetComponent<Collider>();
            playerColl.enabled = false;

            // 플레이어 3초 동안 못움직이게 함
            player.Stun(3f);

            // 플레이어 구멍에 빠진 뒤 콜라이더 원상복구(부활, 죽음, ... etc)
            Invoke("EnableColl", 2f);
            
        }
    }

    // HoleOpen애니메이션 에서 HoleOpen(), HoleClose() 실행함
    private void HoleOpen()
    {
        holeActivate = true;
    }

    private void HoleClose()
    {
        holeActivate = false;
    }

    // 구멍에 빠지게 하기 위해 Player 콜라이더 n초뒤 활성화
    private void EnableColl()
    {
        playerColl.enabled = true;
    }

    private void returnToIdle()
    {
        anim.SetTrigger("close");
    }
    
}
