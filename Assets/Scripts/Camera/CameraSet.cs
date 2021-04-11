using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSet : MonoBehaviour
{
    [SerializeField] private Camera[] cameras = null;
    private int funcKeysNum = 2;                    // 입력받는 f 키 활성화 개수
    private int funcKey;                            // 0 : f1, 1 : f2 ...

    private void Update()
    {
        GetInput();
        CameraChange();
    }

    void GetInput()
    {

        /*funcKeys[0] = Input.GetButtonDown("f1");
        funcKeys[1] = Input.GetButtonDown("f2");
        funcKeys[2] = Input.GetButtonDown("f3");*/

        for (int i = 0; i < funcKeysNum; i++)
        {
            bool idx = Input.GetButtonDown("f" + (i + 1).ToString()); // f1 은 0번 인덱스로
            if (idx)
            {
                Debug.Log(idx);
                funcKey = i;
            }
        }

    }

    void CameraChange()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            if (funcKey == i) cameras[funcKey].gameObject.SetActive(true);
            else
            {
                cameras[i].gameObject.SetActive(false);
            }
        }
    }
}
