using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : BaseTrap
{
    
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            TakeHit(collision.collider);
        }
    }
}
