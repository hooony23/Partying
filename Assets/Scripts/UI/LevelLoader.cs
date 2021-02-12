using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public void Awake()
    {
        Config.LodingSence = 2;
    }
    public void Start()
    {
        LoadLevel(Config.LodingSence);
}
    public void LoadLevel(int scenceIndex) {
        StartCoroutine(LoadAsynchronously(scenceIndex));
    }
    IEnumerator LoadAsynchronously(int scenceIndex) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scenceIndex);
        //operation.allowSceneActivation = false; //로딩이 다되어도 미실행함, 실행원할시 true
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";
            Debug.Log(progress);
            yield return null;
        }
        }

}
