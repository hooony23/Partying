using Communication.MainServer;

public class CreateOrUser : BaseMainMenu
{
    void Start()
    {
        UINum = 3;
        //TODO: 속도 느림 개선 필요
        new WebSocketModule().Start();
    }
}