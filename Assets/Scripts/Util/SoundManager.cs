using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
}
public class SoundManager : MonoBehaviour
{
    /* public AudioSource bgSound;
     public AudioSource audioSource;
     public AudioMixer audioMixer;
     public AudioClip[] bglist;
     public Slider UiSliderSfx;
     public Slider UiSliderBgm;
     
        //최초 실행할 때 초기값을 설정
        Config.Bgmvol = 0.5f; 
        Config.Sfxvol = 0.5f;
     */


    public static SoundManager instance;
    public AudioSource[] audioSourceSFX;
    public AudioSource audioSourceBGM;

    public string[] playSoundName;

    public Sound[] EffectSound;
    public Sound[] BgmSound;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //UiSliderBgm.value = Bgmvol;
           // UiSliderSfx.value = Sfxvol;
        }
        else {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        playSoundName = new string[audioSourceSFX.Length];
    }
    public void PlaySE(string name) {
        for (int i = 0; i < EffectSound.Length; i++)
        {
            if (name.Equals(BgmSound[i].name))
            {
                Debug.Log(name);
                audioSourceBGM.clip = BgmSound[i].clip;
                audioSourceBGM.Play();
            }
            if (name.Equals(EffectSound[i].name)) {
                for (int j = 0;  j < audioSourceSFX.Length;  j++)
                {
                    if (!audioSourceSFX[j].isPlaying) {
                        playSoundName[j] = EffectSound[i].name;
                        audioSourceSFX[j].clip = EffectSound[i].clip;
                        audioSourceSFX[j].Play();
                        return;
                    }
                }
                Debug.Log("모든 효과오디오 소스 사용중");
                return;
            }
        }
        Debug.Log(name+"사운드에 등록되지 않거나 이름이 틀림");
        return;
    }

    public void StopAllSE() { //실행중인 음원들 모두 정지
        for (int i = 0; i < audioSourceSFX.Length; i++)
        {
            audioSourceSFX[i].Stop();
        }
    }
    public void StopSE(string name) { //해당 이름과 일치하는 음원 정지
        
        for (int i = 0; i < audioSourceSFX.Length; i++)
        {
            if (playSoundName.Equals(name))
            {
                Debug.Log(playSoundName + " 소리 중지");
                audioSourceSFX[i].Stop();
                return;
            }
           /*else { //합칠 수 있을 경우 위 함수 제거후 추가
                audioSourceSFX[i].Stop();
            }*/
        }
        Debug.Log("재생중인" + name + "사운드가 없습니다.");
    }
    /*
    private void Start()
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(UiSliderBgm.value) * 20);
        audioMixer.SetFloat("BGM", Mathf.Log10(UiSliderSfx.value) * 20);
    }

    public void SFXPlay(AudioClip clip) {
        audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();
    }
    public void BgSoundPlay(AudioClip clip) {
        bgSound.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.5f;
        bgSound.Play();
    }
    
    public void SetVolume(float volume) {
        audioMixer.SetFloat("Volume", Mathf.Log10(volume) * 20);
    }
    public void SFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
    public void BGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }*/

}
