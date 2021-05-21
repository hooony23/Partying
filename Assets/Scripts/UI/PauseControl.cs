using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Util;

public class PauseControl : MonoBehaviour
{
    public static PauseControl pauseMenu;
    private bool uiOpenFlag = false;
    private GameObject preferences, optionMenu, volumeMenu;
    private Button volumeButton, volumeBack, menuQuit;
    public AudioMixer audioMixer;
    public Slider uiSliderSfx;
    public Slider uiSliderBgm;
    public GameManager.GameManager GM;
    private void Awake()
    {
        preferences = GameObject.Find("PauseMenu").transform.Find("Preferences").gameObject;
        optionMenu = preferences.transform.Find("MainMenu").gameObject;
        volumeButton = optionMenu.transform.Find("Volume").GetComponent<Button>();
        menuQuit = optionMenu.transform.Find("Quit").GetComponent<Button>();
        volumeMenu = preferences.transform.Find("OptionMenu").gameObject;
        volumeBack = volumeMenu.transform.Find("Back").GetComponent<Button>();
        uiSliderBgm = volumeMenu.transform.Find("BGMSlider").GetComponent<Slider>();
        uiSliderSfx = volumeMenu.transform.Find("SFXSlider").GetComponent<Slider>();
        SetUp();
        if (pauseMenu == null) //pauseMenu 없을시
        {
            if (Config.defaultStage != 0) //메인메뉴가 아닐때만 작동
            {
                pauseMenu = this;
                DontDestroyOnLoad(gameObject); // 씬 변경후에도 메뉴를 관리하도록 함
                                               // 환경설정 슬라이더에 초기값 대입
            }
            /*else if(Config.defaultStage == 0)
            {
                Destroy(gameObject);
            }*/
        }
        else
        { //씬 이동후 pauseMenu 중복방지를 위한 기존 pauseMenu 파괴
            Destroy(gameObject);
        }
    }
    public void SetUp()
    {
        volumeButton.onClick.AddListener(OpenVolumeMenu);
        menuQuit.onClick.AddListener(delegate { QuitMenu(); });
        volumeBack.onClick.AddListener(delegate { BackOptionMenu(); });
        uiSliderSfx.onValueChanged.AddListener(SFXVolume);
        uiSliderBgm.onValueChanged.AddListener(BGMVolume);
    }
    public void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
        var soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        audioMixer = soundManager.audioMixer;
        uiSliderBgm.value = Config.Bgmvol;
        uiSliderSfx.value = Config.Sfxvol;
    }
    void Update()
    {
        if (Input.GetButtonDown("pause"))
        {
            if (Config.defaultStage == 0||!GM.GameStart)
                return;
            else if(GM.GameClear|| GM.GameOver) {
                QuitMenu();
                return;
            }
                if (!uiOpenFlag)
                {
                OptionMenuOpen();
                }
                else if (uiOpenFlag)
                {
                    if (volumeMenu.activeSelf == true) // 초기메뉴 화면으로 가기위함
                    {
                    BackOptionMenu();
                    return;
                    }
                QuitMenu();
                }
        }
    }
    void OptionMenuOpen() {
        Cursor.visible = true;
        preferences.SetActive(true);
        optionMenu.SetActive(true);
        volumeMenu.SetActive(false);
        uiOpenFlag = true;
        GM.PauseOpen = true;
    }
    void QuitMenu()
    {
        Cursor.visible = false;
        preferences.SetActive(false);
        optionMenu.SetActive(true);
        volumeMenu.SetActive(false);
        uiOpenFlag = false;
        GM.PauseOpen = false;
    }
    void OpenVolumeMenu() {
        volumeMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    void BackOptionMenu() {
        volumeMenu.SetActive(false);
        optionMenu.SetActive(true);
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