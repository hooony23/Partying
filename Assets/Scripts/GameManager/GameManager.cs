using Lib;
using Communication.JsonFormat;
using Communication.GameServer;
using Communication.GameServer.API;
using Util;

namespace GameManager
{

    public class GameManager : GameManagerUtil
    {
        void Awake()
        {
            switch (Config.defaultStage)
           { 
               case 1:
                // TODO: Test시 주석 지울 것
                // Util.Config.userUuid = System.Guid.NewGuid().ToString();
                // string temp= System.IO.File.ReadAllText(@".\Assets\Scripts\temp.json");
                // var temp2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Communication.JsonFormat.MapInfo>(temp);
                // temp2.playerLocs[0].data = Util.Config.userUuid;
                // Communication.NetworkInfo.mapInfo = temp2;
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
        }

    }
}