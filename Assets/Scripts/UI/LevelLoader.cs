using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using Communication;
using Communication.MainServer;
using UnityEngine.UI;
using Util;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
<<<<<<< HEAD
    public void Start()
    {
        LoadLevel(++Config.defaultStage);
=======
    public int ChangeScne;
    private bool gameLondingDone = false;
    public void Awake()
    {
        ChangeScne = Config.LodingSence;
    }
    public void Start()
    {
        LoadLevel(ChangeScne);
>>>>>>> origin/dev-SungyuHwang
    }
    public void LoadLevel(int scenceIndex)
    {
        if(SceneManager.GetActiveScene().buildIndex <= scenceIndex)
        {
            NetworkInfo.memberInfo = MServer.GetMemberInfo(NetworkInfo.roomInfo.RoomUuid);
            scenceIndex = 0;
        }
        StartCoroutine(LoadAsynchronously(scenceIndex));
    }
    IEnumerator LoadAsynchronously(int scenceIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenceIndex);
        operation.allowSceneActivation = false; //로딩이 다되어도 미실행함, 실행원할시 true
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
<<<<<<< HEAD
            Debug.Log(Config.defaultStage);
=======
            if (operation.progress >= 0.9f&& !gameLondingDone) {
                Debug.Log(operation.isDone);
                Debug.Log(operation.progress);
                gameLondingDone = true;
                yield return new WaitForSeconds(3f);
                Debug.Log("active");
                operation.allowSceneActivation = true;
            }
>>>>>>> origin/dev-SungyuHwang
            yield return null;
        }
    }

}
