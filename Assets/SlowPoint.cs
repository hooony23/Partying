using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPoint : MonoBehaviour
{
    [SerializeField] Player player = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.speed = player.speed * 0.2f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.speed = player.speed * 5f;
        }
    }
}
