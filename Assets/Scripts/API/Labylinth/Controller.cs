using Newtonsoft.Json.Linq;
using Partying.Assets.Scripts.Lib;


namespace Partying.Assets.Scripts.API.Labylinth
{
    public class Controller
    {

        public void Move(string request)
        {
            JObject requestJson = JObject.Parse(request);
            Connection.Send(Common.GetRequestData("Move", "Labylinth", requestJson.Value<string>("uuid"), request));
        }
    }
}