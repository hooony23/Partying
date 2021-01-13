using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrap : MonoBehaviour
{
    private bool hole_activate = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && hole_activate)
        {
            Player player = other.GetComponent<Player>();
            CapsuleCollider playerColl = player.GetComponent<CapsuleCollider>();
            playerColl.enabled = false;

            // 플레이어 3초 동안 못움직이게 함
            player.Stun(3f);

            // 플레이어 구멍에 빠진 뒤 원상복구(부활, 죽음, ... etc)

            
        }
    }

   

    // HoleOpen애니메이션 에서 HoleOpen() 실행함
    private void HoleOpen()
    {
        hole_activate = true;
        Invoke("HoleClose" , 3f);
        
    }

    private void HoleClose()
    {
        hole_activate = false;
       
    }
    
}
