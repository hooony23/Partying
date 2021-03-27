using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDrone : MonoBehaviour
{
    void Start()
    {
        
    }
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            BossPattern.Instance.ChargingLaser();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            BossPattern.Instance.OctaLaser();

        }

    }
}
