using System.Net;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Util
{

    public static class Config
    {
        public static string serverIP = "skine134.iptime.org";
        public static int serverPort = 11000;

        // player
        public static float playerMoveVector = 3f;
        public static float playerSpeed = 14f;
        public static float playerHealth = 100;
        public static string userUuid = "";
        // patrol
        public static float patrolVisionAngle = 110f;        // 패트롤 시야각
        public static float playerDetectDistance = 10f;      // 플레이어 viewAngle 안에 들어왔을시 사정거리
        public static float patrolPointFindDistance = 20f;  // 순찰지역 랜덤 인식을 위한 사정거리
        public static JObject requestForm = JObject.Parse(@"{'type' : '', 'uuid' : '','server':'','data':{}}");
        // map
        public static int ROW = 20;
        public static int COL = 20;

        public enum InputKey
        {
            A = KeyCode.A,
            S = KeyCode.S,
            D = KeyCode.D,
            W = KeyCode.W,
            E = KeyCode.E,
            Space = KeyCode.Space,

        }
    }
}
