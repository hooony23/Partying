using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Communication;
using Util;

public class Timer : MonoBehaviour
{
    //타이머 Text
    private Text timeText;

    //타이머 관리를 위한 변수
    private double Time;
    private bool TimeOver = false;
    private bool Mintime = false;
    private bool isTimerStart = true;

    //BGM 실행
    [SerializeField] private string BGMSound;
    AudioSource audioSource;
    private void Start()
    {
        GameObject TimerObject = Instantiate(Resources.Load("GameUi/TimerUi")) as GameObject;
        timeText = TimerObject.transform.Find("TimeText").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        Time = (NetworkInfo.finishTime - Lib.Common.ConvertToUnixTimestamp(System.DateTime.Now))<=0?Config.Timer:(NetworkInfo.finishTime - Lib.Common.ConvertToUnixTimestamp(System.DateTime.Now));
    }
    private void Update()
    {
        TimeCount();
    }
    private void TimeCount()
    {
        //게임시작과 함께 BGM실행
        if (Config.StartGame && isTimerStart)
        {
            SoundManager.instance.IsPlaySound(BGMSound);
            timeText.gameObject.SetActive(true);
            isTimerStart = false;
        }
        //타이머의 시간의 흐름관리
        if (Time > 0)
        {
            CountDownTimer();
        }
        //60초 이하로 남았을때
        if (Mintime)
        {
            //현재 audioSource가 동작중이지 않을 때 실행
            if (!audioSource.isPlaying)
            {
                Mintime = false;
                SoundManager.instance.IsStopSound(BGMSound);
                audioSource.Play();
            }
        }
        //시간이 종료될때
        if (Time < 0 && TimeOver)
        {
            TimeOver = false;
            audioSource.Stop();
            //Application.Quit();

        }
    }

    //타이머 작동
    private void CountDownTimer()
    {
        Time -= UnityEngine.Time.deltaTime;
        timeText.text = string.Format("{0:D2}:{1:D2}", (int)(Time / 60 % 60), (int)(Time % 60));
        if (Time < 0)
        {
            TimeOver = true;
        }
        if (Time < 60 && Time > 59.9)
        {
            Mintime = true;
        }
    }
}
