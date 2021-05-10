using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Communication;
using Communication.MainServer;
using Communication.GameServer.API;
using UnityEngine.UI;
using Util;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public void Start()
    {
        LoadLevel(++Config.defaultStage);
    }
    public void LoadLevel(int scenceIndex)
    {
        if(Config.defaultStage==2)
        {
            APIController.SendController("InitStage2");
            Communication.GameServer.Connection.receiveDone.WaitOne();
        }
        else if(SceneManager.GetActiveScene().buildIndex <= scenceIndex)
        {
            NetworkInfo.memberInfo = MServer.GetMemberInfo(NetworkInfo.roomInfo.RoomUuid);
            scenceIndex = 0;
        }
        StartCoroutine(LoadAsynchronously(scenceIndex));
    }
    IEnumerator LoadAsynchronously(int scenceIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenceIndex);
        //operation.allowSceneActivation = false; //로딩이 다되어도 미실행함, 실행원할시 true
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            Debug.Log($"current stage : {Config.defaultStage}");
            yield return null;
        }
    }

}