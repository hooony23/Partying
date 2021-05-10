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

    // map
    public static int mapSize = 20;

    public static int ROW = 20;
    public static int COL = 20;
}
