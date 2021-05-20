using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Communication;
using Util;
using System.Globalization;
public class Timer : MonoBehaviour
{

    public Text CountDownDisplay;
    CultureInfo cultureInfo = new CultureInfo("ko-KR");
    //타이머 Text
    private Text timeText;
    private GameManager.GameManager GM = null;
    //타이머 관리를 위한 변수
    public double Time;
    private double finishTime;

    //BGM 실행
    [SerializeField] private string stageBgm = "Stage1";
    AudioSource audioSource;
    bool timerStartFlag = false;
    bool musicFlag = true;
    private void Start()
    {
        GM=GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
        GameObject startCountingObject = Instantiate(Resources.Load("GameUi/CountDownUi")) as GameObject;
        CountDownDisplay = startCountingObject.transform.Find("CountDownText").GetComponent<Text>();
        GameObject TimerObject = Instantiate(Resources.Load("GameUi/TimerUi")) as GameObject;
        timeText = TimerObject.transform.Find("TimeText").GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        GameStartCounting();
        TimeCount();
    }
    private void GameStartCounting()
    {
        
        if(NetworkInfo.startTime!=0d){
            var seconds = NetworkInfo.startTime - Lib.Common.ConvertToUnixTimestamp(System.DateTime.Now);
            NetworkInfo.startTime = 0;
            StartCoroutine(CountDownToStart((int)seconds));
        }
    }
    private void SetTimer()
    {
        Debug.Log($"finish time : {NetworkInfo.finishTime - Lib.Common.ConvertToUnixTimestamp(System.DateTime.Now)}");
        finishTime = NetworkInfo.finishTime;
        Time = (finishTime - Lib.Common.ConvertToUnixTimestamp(System.DateTime.Now))<=0?Config.Timer:(finishTime - Lib.Common.ConvertToUnixTimestamp(System.DateTime.Now));
        timerStartFlag = true;
        GM.GameStart = true;
    }
    
    IEnumerator CountDownToStart(int seconds)
    {
        yield return new WaitForSecondsRealtime(2f);
        CountDownDisplay.gameObject.SetActive(true);

        //1초마다 넘어가며 카운트 다운
        while (seconds > 0)
        {
            CountDownDisplay.text = seconds.ToString();
            yield return new WaitForSecondsRealtime(1f);
            seconds--;
        }

        //start 화면과 함께 게임시작
        //Time.timeScale = 1.0f;
        CountDownDisplay.text = "Start!";
        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
        yield return new WaitForSecondsRealtime(1f);
        CountDownDisplay.gameObject.SetActive(false);
        if(Config.defaultStage==1)
            SetTimer();
    }
    private void TimeCount()
    {
        
        if(!timerStartFlag)
            return;
        //게임시작과 함께 BGM실행
        if(musicFlag)
        {
            SoundManager.instance.IsPlaySound(stageBgm);
            musicFlag=false;
        }
        timeText.gameObject.SetActive(true);
        //타이머의 시간의 흐름관리
        if (Time > 0)
        {
            CountDownTimer();
        }
        //시간이 종료될때
        if (Time <= 0)
        {
            audioSource.Stop();
            GM.GameOver=true;
        }
    }

    //타이머 작동
    private void CountDownTimer()
    {
        Time = NetworkInfo.finishTime - Lib.Common.ConvertToUnixTimestamp(System.DateTime.Now);
        timeText.text = string.Format("{0:D2}:{1:D2}", (int)(Time / 60 % 60), (int)(Time % 60));
    }
}
