using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
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

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            BossPattern.Instance.BodySlam();

        }



    }
}
