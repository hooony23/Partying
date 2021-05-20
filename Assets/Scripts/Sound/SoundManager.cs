﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;
/*
//다른 스크립트에서 선언시
     [SerializeField]
    private string BGMSound;*/

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}
/// <summary>
/// 게임의 효과음, 배경음을 관리함
/// </summary>

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance; //전역변수로 설정하여 어디서든 참조 
    public AudioSource[] audioSourceSFX;
    public AudioSource audioSourceBGM;
    public AudioMixer audioMixer;

    public string[] playSoundName;
    public string[] playBgmSoundName;

    public Sound[] EffectSound;
    public Sound[] BgmSound;
    public Vector3 Vec;
    //public Slider UiSliderSfx;
   // public Slider UiSliderBgm;

    private void Awake()
    {
        if (instance == null) //SoundManager 없을시
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬 변경후에도 소리를 관리하도록 함
            //환경설정 슬라이더에 초기값 대입
            /*
            UiSliderBgm.value = Config.Bgmvol;
            UiSliderSfx.value = Config.Sfxvol;*/

        }
        else
        { //씬 이동후 SoundManager 중복방지를 위한 기존 SoundManger 파괴
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        Vec = this.gameObject.transform.position;
        //효과음의 갯수와 동일화
        playSoundName = new string[audioSourceSFX.Length];
        playBgmSoundName = new string[BgmSound.Length];
        if (Config.defaultStage == 0)
        {
            IsPlaySound2d("Main");
        }
        audioMixer.SetFloat("SFX", Mathf.Log10(Config.Bgmvol) * 20); // 오디오 설정에 슬라이더 값 대입
        audioMixer.SetFloat("BGM", Mathf.Log10(Config.Sfxvol) * 20);
    }
    public void IsPlaySound2d(string name) { //Ui, BGM 사운드 관리
        for (int i = 0; i < BgmSound.Length; i++)
        {
            for (int j = 0; j < BgmSound.Length; j++)
            {
                //Debug.Log("Test" + j+" // "+ i);
                if (name.Equals(BgmSound[i].name)) //BGM재생
                {
                    if (!audioSourceBGM.isPlaying)
                    {
                        playBgmSoundName[j] = BgmSound[i].name;
                        audioSourceBGM.clip = BgmSound[i].clip;
                        //Debug.Log(BgmSound[i].name);
                        Debug.Log(playBgmSoundName[j]);
                        audioSourceBGM.Play();
                        return;
                    }
                }
            }
        }
        Debug.Log(name + "사운드에 등록되지 않거나 이름이 틀림");
        return;
    }
    public void IsPlaySound3d(string name,Vector3 objectVec) //실시간이 아닌 3d 오브젝트 사운드 관리
    { //음원재생
        for (int i = 0; i < audioSourceSFX.Length; i++)
        {
            for (int j = 0; j < audioSourceSFX.Length; j++)
            {
                if (name.Equals(EffectSound[i].name))
                {//SFX재생
                    if (!audioSourceSFX[j].isPlaying)
                    {
                        playSoundName[j] = EffectSound[i].name;
                        audioSourceSFX[j].clip = EffectSound[i].clip;
                        AudioSource.PlayClipAtPoint(EffectSound[i].clip, objectVec,Config.Sfxvol);
                        return;
                    }
                    Debug.Log("모든 효과오디오 소스 사용중");
                    return;
                }
            }
        }
        Debug.Log(name + "사운드에 등록되지 않거나 이름이 틀림");
        return;
    }

    public void StopAllSfxSound()
    { //실행중인 효과음들 모두 정지
        for (int i = 0; i < audioSourceSFX.Length; i++)
        {
            audioSourceSFX[i].Stop();
        }
    }
    public void StopBgmSound()
    { //실행중인 배경음 정지
                audioSourceBGM.Stop();
                return;
    }
    public void IsStopSound(string name)
    { //해당 이름과 일치하는 음원 정지

        for (int i = 0; i < audioSourceSFX.Length; i++)
        {
            if (playSoundName[i].Equals(name))
            {
                Debug.Log(playSoundName + " 소리 중지");
                Debug.Log(playSoundName[i].ToString() + " 소리 중지");
                audioSourceSFX[i].Stop();
                return;
            }
            /*else { //StopAllSound와 합칠 수 있을 경우 제거후 추가
                 audioSourceSFX[i].Stop();
             }*/
        }
        Debug.Log("재생중인" + name + "사운드가 없습니다.");
    }
    // 환경설정의 스크롤바 조절
   
}
