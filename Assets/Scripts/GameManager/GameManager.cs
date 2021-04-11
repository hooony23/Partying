using System;
using UnityEngine;
using Util;
using Communication;
using Communication.API;


namespace GameManager
{

    public class GameManager : GameManagerUtil
    {
        void Awake()
        {
            SetUserUuid(Connection.Connected());
            APIController.SendController("CreateMap");
            Connection.receiveDone.WaitOne();
            InitializeLabylinth();
        }
        
        void Update()
        {
            DelUser();
            DeathUser();
            ClearGame();
        }

        void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            Connection.ConnectedExit();
        }

    }
}