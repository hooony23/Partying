using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapClearItem : MonoBehaviour
{
    // enum 선언으로 열거형 타입으로 아이템 분류
    public enum MapClear { ClearItem };
    public MapClear ItemType;
    [SerializeField]
    private GameObject GameClearUi;
    private bool UserClear = false;
    private bool UserallClear = false;
    [SerializeField] private Button ContinueButton = null;
    private void Start()
    {
        //GameClearUi = GameObject.Find("GameClearUi");
    }
    private void Update()
    {
        if (Config.GameClear) {
            StartCoroutine(ClearUi());
        }
        if (UserClear && UserallClear) {
            StartCoroutine(SceneChange());
        }
    }
    public void UserClearButton() {
        Debug.Log("button Test");
        UserClear = true;
        Config.GameClear = false;
        ContinueButton.interactable = false;
    }

    public void isGameClear() {
        GameClearUi.SetActive(true);
    }
    IEnumerator ClearUi() {
        yield return new WaitForSeconds(5f);
        isGameClear();
    }
    IEnumerator SceneChange()
    {
        yield return new WaitForSeconds(5f);
        GameClearUi.SetActive(true);
        Debug.Log(UserClear);
            if (UserClear)
            {
               yield return new WaitForSeconds(2f);
                if (UserallClear)
                {
                    Config.GameClear = false;
                    SceneManager.LoadScene("First Map");
                }
                UserallClear = true;
            }
        
    }
}

