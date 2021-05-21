using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource[] audioSourceSFX;
    public Sound[] audioclip;
    public void start(int soundLength)
    {
        for (int i = 0; i <= soundLength; i++)
        {
            audioSourceSFX[i].clip = audioclip[i].clip;
        }
    }
}
