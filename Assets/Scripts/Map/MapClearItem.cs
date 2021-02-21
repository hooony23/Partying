using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapClearItem : MonoBehaviour
{
    // enum 선언으로 열거형 타입으로 아이템 분류
    public enum MapClear { ClearItem };
    public MapClear ItemType;
    private void Update()
    {
        if (Config.GameClear) {
            Config.GameClear = false;
            Debug.Log("게임클리어");
            Invoke("SceneChange", 5);
        }
    }
    public void SceneChange()
    {
        SceneManager.LoadScene("First Map");
    }
}

