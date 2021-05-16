using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class CMController : MonoBehaviour
{
    public GameObject targetplayer;
    public GameObject shotpoint;

    // cmFreeLook, cmAim 세팅값 설정 변경용
    private Cinemachine.CinemachineFreeLook freelook; 
    private Cinemachine.CinemachineFreeLook aim; 

    private GameObject cmFreeLook;
    private GameObject cmAim;
    private Camera maincam;

    private void Start()
    {
        maincam = Camera.main;                           // 각 씬의 메인 카메라 이름으로 변경
        cmFreeLook = this.transform.Find("CM FreeLook").gameObject;
        cmAim = this.transform.Find("CM Aim").gameObject;


    }

    void Update()
    {

    }
}
