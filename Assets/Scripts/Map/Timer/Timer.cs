using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeText;
    private float time;
    private int Notime = 100;
    private bool TimeOver = false;
    private bool Mintime = false;
    [SerializeField]
    private string BGMSound;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        time = Config.Timer;
        
    }
    private void Start()
    {
        SoundManager.instance.IsPlaySound(BGMSound);
    }
    private void Update()
    {
        if (Config.StartGame)
        {
            timeText.gameObject.SetActive(true);
            Config.StartGame = false;
        }
        if (time > 0)
        {
            CountDownTimer();
        }
        if (Mintime && time>99.995f) {
            Debug.Log("실행중");
            SoundManager.instance.StopAllSound();
            audioSource.Play();
        }
        if (time < 0 && TimeOver) {
            TimeOver = false;
            Debug.Log("게임끝");
            //Application.Quit();

        }
    }
    private void CountDownTimer() {
        time -= Time.deltaTime;
        timeText.text = string.Format("{0:D2}:{1:D2}", (int)(time/60%60), (int)(time % 60));
        if (time < 0) {
            TimeOver = true;
        }
        if (time > 99&&time<100) {
            Mintime = true;
        }
    }
}
