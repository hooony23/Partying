using Lib;
using Communication.GameServer.API;
namespace GameManager
{
    public class RaidGameManager : GameManagerUtil
    {
        void Awake()
        {
            // TODO: Test시 주석 지울 것
            Common.SetUserUuid(System.Guid.NewGuid().ToString());
            APIController.SendController("Connected");
            APIController.SendController("InitStage2");

            InitializeRaid();
        }
        
        protected override void Update()
        {
            UpdateUserList();
            base.Update();
        }
    }
}