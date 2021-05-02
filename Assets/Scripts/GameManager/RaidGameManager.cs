using Lib;
using Communication.GameServer.API;
namespace GameManager
{
    public class RaidGameManager : GameManagerUtil
    {
        void Awake()
        {
            // TODO: Test시 주석
            APIController.SendController("InitStage2");

            InitializeRaid();
        }
        
        protected override void Update()
        {
            UpdateUserList();
            UpdateBossInfo();
            base.Update();
        }
    }
}