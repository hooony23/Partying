using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpandDown : MonoBehaviour
{
    public float startTime;
    public float minY, maxY;

    [Range(1, 20)] public float moveSpeed;

    private int sign = -1;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= startTime)
        {
            if (transform.position.y <= minY || transform.position.y >= maxY)
            {
                sign *= -1;

            }
            transform.position += new Vector3(0, moveSpeed * Time.deltaTime * sign, 0);


        }
    }

}
