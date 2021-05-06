using Lib;
using Communication.GameServer.API;
namespace GameManager
{
    public class RaidGameManager : GameManagerUtil
    {
        void Awake()
        {
            // // TODO: Test시 주석 지울 것
            // Util.Config.userUuid = System.Guid.NewGuid().ToString();
            // APIController.SendController("Connected");
            // APIController.SendController("GetItem");
            // Communication.GameServer.Connection.receiveDone.WaitOne();
            //****************************************
            InitializeRaid();
        }
        
    }
}