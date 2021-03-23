using Communication;
using Communication.API;


namespace GameManager
{

    public class GameManager : GameManagerUtil
    {

        void Awake()
        {
            
            //TODO: Test시 주석 처리 할 것
            
            // TODO: Test시 주석 지울 것
            // Util.Config.userUuid = System.Guid.NewGuid().ToString();
            // string temp= System.IO.File.ReadAllText(@".\Assets\Scripts\temp.json");
            // var temp2 = Newtonsoft.Json.JsonConvert.DeserializeObject<Communication.JsonFormat.MapInfo>(temp);
            // temp2.playerLocs[0].data = Util.Config.userUuid;
            // Communication.NetworkInfo.mapInfo = temp2;

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