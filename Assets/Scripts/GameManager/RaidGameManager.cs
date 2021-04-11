using Communication;

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