using System.Reflection;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Lib;


namespace Communication.GameServer.API.Labylinth
{
    public class Send : Controller
    {
        public void Move(string request)
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name), _server, request));
        }
        public void CreateMap()
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name), _server));
        }
        public void GetItem()
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name), _server));
        }
        public void Death()
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name), _server));
        }
        public void Connected()
        {
            string request = BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name), _server);
            Connection.Connected(request);
        }
    }
}