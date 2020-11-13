using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset; // 멀티로 변경시, Start 에서 플레이어 offset(x, y, z) 을 받아와야 함

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
