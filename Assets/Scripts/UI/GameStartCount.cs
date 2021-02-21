using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameStartCount : MonoBehaviour
{
    [SerializeField]
    public Text CountDownDisplay;

    private void Start()
    {
        Debug.Log("start");
        StartCoroutine(CountDownToStart());
    }

    IEnumerator CountDownToStart() {
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(5f);
        CountDownDisplay.gameObject.SetActive(true);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        while (Config.CountDownTime > 0) {
            CountDownDisplay.text = Config.CountDownTime.ToString();
            yield return new WaitForSecondsRealtime(1f);
            Debug.Log("doing");
            Config.CountDownTime--;
        }
        CountDownDisplay.text = "Start!";
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        Debug.Log("end");
        Config.StartGame = true;
        CountDownDisplay.gameObject.SetActive(false);
    }
}
