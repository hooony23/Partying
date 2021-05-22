using UnityEngine;

public class HoleTrap : BaseTrap
{
    private bool holeActivate = false;
    private Collider playerColl;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void OnTriggerStay(Collider other)
    {
        if (IsActive)
        {
            TrapEvent(other, holeActivate);
            return;
        }
        anim.SetTrigger("open");
        IsActive = true;
    }
    public override void TrapEvent(Collider other, params object[] obj)
    {
        bool holestate = (bool)obj[0];

        // 발동된 함정은 holeActive 가 true 상태
        if (other.CompareTag("Player") && holestate)
        {
            playerColl = other.GetComponent<Collider>();
            playerColl.enabled = false;
            // 플레이어 3초 동안 못움직이게 함
            Player player = other.GetComponent<Player>();
            player.Stun(3f);

            // 플레이어 구멍에 빠진 뒤 콜라이더 원상복구(부활, 죽음, ... etc)
            Invoke("EnableColl", 2f);
        }
    }
    // HoleOpen애니메이션 에서 HoleOpen(), HoleClose() 실행함
    public void HoleOpen()
    {
        holeActivate = true;
    }

    public void HoleClose()
    {
        holeActivate = false;
        IsActive = false;
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
