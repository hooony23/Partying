using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
    ParticleSystem chargingL, octaL;
    Animator anim;
    private static BossPattern instance = null;

    public static BossPattern Instance
    {
        get
        {
            if (null == instance) return null;
            return instance;
        }
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    
    void Start()
    {
        anim = GetComponent<Animator>();

        GameObject chargingLaser = GameObject.Find("Charging Laser");
        chargingL = chargingLaser.GetComponent<ParticleSystem>();
        chargingL.Stop();

        GameObject octaLaser = GameObject.Find("Octa Laser");
        octaL = octaLaser.GetComponent<ParticleSystem>();
        octaL.Stop();
    }

    public void ChargingLaser()
    {
        chargingL.Play();                   // 파티클 시스템 플레이
    }

    public void OctaLaser()
    {
        octaL.Play();                       // 파티클 시스템
        anim.SetTrigger("OctaLaser1");      // 레이저 총구 각도 변환 애니메이션
    }

    public void BodySlam()
    {
        anim.SetTrigger("BodySlam1");
    }
}
