using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;
using UnityEngine.UIElements;

public class GameStartCount : MonoBehaviour
{
    public Text CountDownDisplay;
    //현재시각을 이용하여 CountDown을 해보았으나, timescale이 0일때 발생하는 오류가 다수발생.
    /* private int firsttime = (int)System.DateTime.Now.TimeOfDay.TotalSeconds+5;
     private int endtime = (int)System.DateTime.Now.TimeOfDay.TotalSeconds;
     private int CountDownTime;*/
    private void Start()
    {
        GameObject TimerObject = Instantiate(Resources.Load("GameUi/CountDownUi")) as GameObject;
        CountDownDisplay = TimerObject.transform.Find("CountDownText").GetComponent<Text>();
        StartCoroutine(CountDownToStart());
    }
    //Coroutine을 이용하여 카운트 다운을 실행함.
    //timeScale이 0이 되면 Update문이 실행불가.
    //WaitForSecondsRealtime을 통해 현재 초만큼 움직이도록 실행.
    IEnumerator CountDownToStart()
    {
        //현재 게임의 배속을 0으로 만들고 2초뒤 실행, 물리적 시간도 함께 조정
        Time.timeScale = 0.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(2f);
        CountDownDisplay.gameObject.SetActive(true);

        //1초마다 넘어가며 카운트 다운
        while (Config.CountDownTime > 0)
        {
            CountDownDisplay.text = Config.CountDownTime.ToString();
            yield return new WaitForSecondsRealtime(1f);
            Config.CountDownTime--;
        }

        //start 화면과 함께 게임시작
        Time.timeScale = 1.0f;
        CountDownDisplay.text = "Start!";
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        Config.StartGame = true;
        yield return new WaitForSecondsRealtime(1f);
        CountDownDisplay.gameObject.SetActive(false);
    }
}
