﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Util;

public class PauseControl : MonoBehaviour
{
    public static PauseControl pauseMenu;
    private bool IsUiPoen = false;
    private GameObject PauseObject, MainMenu, OptionMenu;
    public AudioMixer audioMixer;
    public Slider UiSliderSfx;
    public Slider UiSliderBgm;
    private void Awake()
    {
        PauseObject = GameObject.Find("PauseMenu").transform.Find("Preferences").gameObject;
        MainMenu = PauseObject.transform.Find("MainMenu").gameObject;
        OptionMenu = PauseObject.transform.Find("OptionMenu").gameObject;
        UiSliderBgm = OptionMenu.transform.Find("BGMSlider").GetComponent<Slider>();
        UiSliderSfx = OptionMenu.transform.Find("SFXSlider").GetComponent<Slider>();
        if (pauseMenu == null) //pauseMenu 없을시
        {
            //if (Config.defaultStage != 0) //메인메뉴가 아닐때만 작동
            //{
                Cursor.visible = false;
                pauseMenu = this;
                DontDestroyOnLoad(gameObject); // 씬 변경후에도 메뉴를 관리하도록 함
                                               // 환경설정 슬라이더에 초기값 대입
            //}
        }
        else
        { //씬 이동후 pauseMenu 중복방지를 위한 기존 pauseMenu 파괴
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        audioMixer = SoundManager.instance.audioMixer;
        UiSliderBgm.value = Config.Bgmvol;
        UiSliderSfx.value = Config.Sfxvol;
    }
    void Update()
    {
        if (Input.GetButtonDown("pause"))
        {
            if (Config.defaultStage != 0)
            {
                if (!IsUiPoen)
                {
                    Cursor.visible = true;
                    PauseObject.SetActive(true);
                    IsUiPoen = true;
                    // Debug.Log(IsUiPoen + "========================================================");
                }
                else if (IsUiPoen)
                {
                    Cursor.visible = false;
                    // Debug.Log("종료실행 ======================================================== ");
                    if (OptionMenu.activeSelf == true) // 초기메뉴 화면으로 가기위함
                    {
                        MainMenu.SetActive(true);
                        OptionMenu.SetActive(false);
                    }
                    PauseObject.SetActive(false);
                    IsUiPoen = false;
                }
            }
        }
    }
    public void SFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        Config.Sfxvol = volume;
    }
    public void BGMVolume(float volume)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
        Config.Bgmvol = volume;
    }
}