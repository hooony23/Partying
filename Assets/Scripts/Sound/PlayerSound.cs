using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSound : MonoBehaviour
{
    public AudioSource[] audioSourceSFX;
    public Sound[] audioclip;
    public void start() {
        for (int i = 0; i <= audioSourceSFX.Length; i++)
        {
            audioSourceSFX[i].clip = audioclip[i].clip;
            Debug.Log(audioSourceSFX[i].clip);
        }
    }
    public void RightWalkSound()
    {
        audioSourceSFX[0].Play();
        Debug.Log("aaa");
    }
    public void LeftWalkSound()
    {
        audioSourceSFX[1].Play();
        Debug.Log("bbb");
    }
    public void DeadSound() 
    {
        audioSourceSFX[2].Play();
        Debug.Log("ccc");
    }
    public void DodgeSound()
    {
        audioSourceSFX[3].Play();
        Debug.Log("ddd");
    }
    public void FireSound()
    {
        audioSourceSFX[4].Play();
        Debug.Log("eee");
    }
    public void HitSound()
    {
        audioSourceSFX[5].Play();
        Debug.Log("fff");
    }


}
