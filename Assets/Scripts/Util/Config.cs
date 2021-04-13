using UnityEngine;
using Communication.JsonFormat;

namespace Util
{

    public static class Config
    {
        
        public static string defaultDNS = "localhost";//"skine134.iptime.org";
        public static int mainServerPort = 1215;//42450
        public static int chatServerPort = 42460;
        public static int gameServerPort = 11000;
        public static string mainServerDNS = $"https://{defaultDNS}:{mainServerPort}";
        public static string chatServerDNS = $"htpps://{defaultDNS}:{chatServerPort}";

        // boss
        public static float radarRange = 60f;
        public static float bossHP = 6000f;
        // player
        public static float playerMoveVector = 3f;
        public static float playerSpeed = 14f;
        public static float playerHealth = 3;
        public static float playerAttackDamage = 1f;
        public static float shotSpeed = 1f;
        public static string userUuid = "";
        // patrol
        public static float patrolVisionAngle = 110f;        // 패트롤 시야각
        public static float playerDetectDistance = 10f;      // 플레이어 viewAngle 안에 들어왔을시 사정거리
        public static float patrolPointFindDistance = 2000f;  // 순찰지역 랜덤 인식을 위한 사정거리
        // map
        public static int ROW = 20;
        public static int COL = 20;
        // 
        public static int defaultStage = 2;
        public static float labylinthOnSpaceSize = 0f;

        public enum InputKey
        {
            A = KeyCode.A,
            S = KeyCode.S,
            D = KeyCode.D,
            W = KeyCode.W,
            E = KeyCode.E,
            Space = KeyCode.Space,
        }

        public enum SendAPINames
        {
            Connected,
            ConnectedExit,
            CreateMap,
            Death,
            Move,
            GetItem,
        }
        public enum ReceiveAPINames
        {
            Connected,
            CreateMap,
            ConnectedExit,
            Death,
            SyncPacket,
            GetItem,
        }
        public static int LodingSence = 1; //씬 로드 순서파악을 위한 숫자

        //Sound
        public static float Bgmvol = 0.5f;
        public static float Sfxvol = 0.5f;

        public static float Timer = 65f;
        public static int CountDownTime = 3;
        public static bool StartGame = false;

        public static bool GameClear = false;
    }

}


