using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    private bool IsUiPoen = false;
    [SerializeField]
    private GameObject PauseObject;
    void Update()
    {
        if (Input.GetButtonDown("pause"))
        {
            if (!IsUiPoen)
            {
                PauseObject.SetActive(true);
                IsUiPoen = true;
                Debug.Log(IsUiPoen+"========================================================");
            }
            else if (IsUiPoen)
            {
                Debug.Log("종료실행 ======================================================== ");
                PauseObject.SetActive(false);
                IsUiPoen = false;
            }
        }
    }
}