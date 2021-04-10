using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Communication;
using Communication.API;

public class SetDeadBoundary : MonoBehaviour
{
    
    // DeadBoundary 에 Player 닿으면 죽음 처리
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == Config.userUuid)
        {
            APIController.SendController("Death");
        }
    }

}
