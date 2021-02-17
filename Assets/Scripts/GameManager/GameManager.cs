using Communication;


namespace GameManager
{

    public class GameManager : GameManagerUtil
    {

        void Awake()
        {
            SetUserUuid(Connection.Connected());
            InitializeLabylinth();
        }

        

        void OnApplicationQuit()
        {
            /* 서버 연결 해제 */
            Connection.ConnectedExit();
        }

    }
}