using Communication.GameServer;

namespace GameManager
{
    public class RaidGameManager : GameManagerUtil
    {
        void Awake()
        {
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