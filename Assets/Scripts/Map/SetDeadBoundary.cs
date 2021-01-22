using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDeadBoundary : MonoBehaviour
{

    // DeadBoundary 에 Player 닿으면 죽음 처리
    private void OnCollisionEnter(Collision collision)
    {
        Player player = collision.rigidbody.GetComponent<Player>();

        Debug.Log(ErrorMessage.clientMessage["dead"] + player.getPlayerController().PlayerHealth);
    }

}
