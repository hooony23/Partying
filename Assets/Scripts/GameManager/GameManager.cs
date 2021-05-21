using Lib;
using Communication.JsonFormat;
using Communication.GameServer;
using Communication.GameServer.API;
using Util;
using UnityEngine;

namespace GameManager
{

    public class GameManager : GameManagerUtil
    {
        protected void Awake()
        {
            //SpawnCamera();
            SetCurrentStage(Config.defaultStage);
            //UI는 원래 Start에서 설정해야하지만, 채팅은 모든 오브젝트가 소환되기전에 설정되야하므로, Awake에서 동작.
            SetChatting();
            switch (Config.defaultStage)
            {
                case 1:
                    InitializeLabylinth();
                    break;
                case 2:
                    InitializeRaid();
                    break;
            }
            APIController.SendController("SyncStart");
            Communication.GameServer.Connection.receiveDone.WaitOne();
        }
        
        protected virtual void Start()
        {
            SetGameUi();
            InitUserList();
        }
        protected virtual void Update()
        {
            DelUser();
            DeathUser();
            ClearGame();
            OverGame();
        }

    }
}