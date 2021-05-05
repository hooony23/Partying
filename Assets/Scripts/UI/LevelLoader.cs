using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public int ChangeScne;
    private bool gameLondingDone = false;
    public void Awake()
    {
        ChangeScne = Config.LodingSence;
    }
    public void Start()
    {
        LoadLevel(ChangeScne);
    }
    public void LoadLevel(int scenceIndex)
    {
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
            if (operation.progress >= 0.9f&& !gameLondingDone) {
                Debug.Log(operation.isDone);
                Debug.Log(operation.progress);
                gameLondingDone = true;
                yield return new WaitForSeconds(3f);
                Debug.Log("active");
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

}
