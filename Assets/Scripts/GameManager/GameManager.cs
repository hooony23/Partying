using Communication;
using Communication.API;


namespace GameManager
{

    public class GameManager : GameManagerUtil
    {

        void Awake()
        {
            APIController.SendController("Labylinth", "CreateMap");
            Connection.receiveDone.WaitOne();
            InitializeLabylinth();
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