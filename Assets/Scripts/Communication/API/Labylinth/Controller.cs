using Newtonsoft.Json.Linq;
using Lib;


namespace Communication.API.Labylinth
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