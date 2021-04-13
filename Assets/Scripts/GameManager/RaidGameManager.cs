using Lib;
using Communication.JsonFormat;
using Communication.GameServer;
using Communication.GameServer.API;
namespace GameManager
{
    public class RaidGameManager : GameManagerUtil
    {
        void Awake()
        {
            Common.SetUserUuid(System.Guid.NewGuid().ToString());
            APIController.SendController("Connected");
            APIController.SendController("InitStage2");
            InitializeRaid();
        }
        
        void Update()
        {
            UpdateUserList();
            ClearGame();
        }

        void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            Connection.ConnectedExit();
        }
    }
}