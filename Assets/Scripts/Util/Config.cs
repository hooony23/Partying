using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static string serverIP = "skine134.iptime.org";
    public static int serverPort = 11000;

    // player
    public static float playerSpeed = 14f;
    public static float playerHealth = 100;

    // patrol
    public static float patrolVisionAngle = 110f;        // 패트롤 시야각
    public static float playerDetectDistance = 10f;      // 플레이어 viewAngle 안에 들어왔을시 사정거리
    public static float patrolPointFindDistance = 20f;  // 순찰지역 랜덤 인식을 위한 사정거리

    // map
    public static int ROW = 20;
    public static int COL = 20;
    public static int LodingSence = 1; //씬 로드 순서파악을 위한 숫자

    //Sound
    public static float Bgmvol = 0.5f;
    public static float Sfxvol = 0.5f;

    public static float Time = 50f;


}
