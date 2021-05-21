using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSound : MonoBehaviour
{
    public AudioSource[] audioSourceSFX;
    public Sound[] audioclip;
    public void Start() {
        for (int i = 0; i < audioSourceSFX.Length; i++)
        {
            audioSourceSFX[i].clip = audioclip[i].clip;
            Debug.Log(audioSourceSFX[i].clip);
        }
    }
    public void RightWalkSound()
    {
        audioSourceSFX[0].Play();
        Debug.Log($"Play Player Sfx : {audioclip[0].clip.name}");
    }
    public void LeftWalkSound()
    {
        audioSourceSFX[1].Play();
        Debug.Log($"Play Player Sfx : {audioclip[1].clip.name}");
    }
    public void DeadSound() 
    {
        audioSourceSFX[2].Play();
        Debug.Log($"Play Player Sfx : {audioclip[2].clip.name}");
    }
    public void DodgeSound()
    {
        audioSourceSFX[3].Play();
        Debug.Log($"Play Player Sfx : {audioclip[3].clip.name}");
    }
    public void FireSound()
    {
        audioSourceSFX[4].Play();
        Debug.Log($"Play Player Sfx : {audioclip[4].clip.name}");
    }
    public void HitSound()
    {
        audioSourceSFX[5].Play();
        Debug.Log($"Play Player Sfx : {audioclip[5].clip.name}");
    }


}
