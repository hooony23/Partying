using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timeText;
    private float time;
    private bool TimeOver = false;

    private void Awake()
    {
        time = Config.Time;
    }
    private void Update()
    {
        if (time > 0)
        {
            CountDownTimer();
        }
        if (time < 0 && TimeOver) {
            TimeOver = false;
            Debug.Log("게임끝");
            Application.Quit();

        }
    }
    private void CountDownTimer() {
        time -= Time.deltaTime;
        timeText.text = string.Format("{0:D2}:{1:D2}", (int)(time/60%60), (int)(time % 60));
        if (time < 0) {
            TimeOver = true;
        }
    }
}
