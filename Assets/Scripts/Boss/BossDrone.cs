using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDrone : MonoBehaviour
{
    ParticleSystem chargingL, octaL;

    void Start()
    {
        GameObject chargingLaser = GameObject.Find("Charging Laser");
        chargingL = chargingLaser.GetComponent<ParticleSystem>();
        chargingL.Stop();

        GameObject octaLaser = GameObject.Find("Octa Laser");
        octaL = octaLaser.GetComponent<ParticleSystem>();
        octaL.Stop();
    }
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            chargingL.Play();
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            octaL.Play();
        }

    }
}
