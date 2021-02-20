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
            APIController.SendController("Labylinth", "CreateMap");
            
            Connection.receiveDone.WaitOne();
            InitializeLabylinth();
        }



        void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            Connection.ConnectedExit();
        }

    }
}