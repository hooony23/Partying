using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 함정의 Collider 에 충돌한 other 를 느려지게함
 * (추가?) Patrol 도 느려지게 할 지의 여부
 */
public class SlowPoint : BaseTrap
{
    private void OnTriggerEnter(Collider other)
    {
        TrapEvent(other, 0.2f);
    }
    private void OnTriggerExit(Collider other)
    {
        TrapEvent(other, 1f);
    }
    public override void TrapEvent(Collider other, params object[] obj)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().PlayerSpeed = (float)Util.Config.playerSpeed * (float)obj[0];
        }
    }
}
