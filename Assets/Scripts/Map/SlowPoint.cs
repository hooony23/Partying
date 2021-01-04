﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 함정의 Collider 에 충돌한 other 를 느려지게함
 * (추가?) Patrol 도 느려지게 할 지의 여부
 */
public class SlowPoint : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.speed = player.speed * 0.2f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.speed = player.speed * 5f;
        }
    }
}