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
                    // TODO: Test시 주석 지울 것
                    // Util.Config.userUuid = System.Guid.NewGuid().ToString();
                    // string temp= System.IO.File.ReadAllText(@".\Assets\Scripts\temp.json");
                    // var temp2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Communication.JsonFormat.MapInfo>(temp);
                    // temp2.playerLocs[0].data = Util.Config.userUuid;
                    // Communication.NetworkInfo.mapInfo = temp2;
                    //**************************************
                    InitializeLabylinth();
                    break;
                case 2:
                    // // // TODO: Test시 주석 지울 것
                    // Util.Config.userUuid = System.Guid.NewGuid().ToString();
                    // APIController.SendController("Connected");
                    // APIController.SendController("GetItem");
                    // Communication.GameServer.Connection.receiveDone.WaitOne();
                    //****************************************
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
        }

    }
}