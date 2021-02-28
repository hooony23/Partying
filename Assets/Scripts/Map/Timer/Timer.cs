using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField]
    private Text timeText;
    private float time;
    private bool TimeOver = false;
    private bool mintime = false;
    private bool isTimerStart = true;
    [SerializeField]
    private string BGMSound;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        time = Config.Timer;
        
    }
    private void Update()
    {
            TimeCount();
    }
    private void TimeCount() {
        if (Config.StartGame&& isTimerStart)
        {
            SoundManager.instance.IsPlaySound(BGMSound);
            timeText.gameObject.SetActive(true);
            isTimerStart = false;
        }
        if (time > 0)
        {
            CountDownTimer();
        }
        if (mintime)
        {
            if (!audioSource.isPlaying)
            {
                mintime = false;
                SoundManager.instance.IsStopSound(BGMSound);
                audioSource.Play();
            }
        }
        if (time < 0 && TimeOver)
        {
            TimeOver = false;
            audioSource.Stop();
            //Application.Quit();

        }
    }
    private void CountDownTimer() {
        time -= Time.deltaTime;
        timeText.text = string.Format("{0:D2}:{1:D2}", (int)(time/60%60), (int)(time % 60));
        //Debug.Log(timeText.text);
        if (time < 0) {
            TimeOver = true;
        }
        if (time<60&&time>59.9) {
            mintime = true;
        }
    }
}
