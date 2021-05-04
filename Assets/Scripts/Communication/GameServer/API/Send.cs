using System.Reflection;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Lib;


namespace Communication.GameServer.API
{
    public class Send : Controller
    {
        public void Move(object request)
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name), request));
        }
        public void CreateMap()
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name)));
            Connection.receiveDone.WaitOne();
        }
        public void GetItem()
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name)));
        }
        public void Death()
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name)));
        }
        public void Connected()
        {
            string request = BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name));
            Connection.Connected(request);
        }
        public void ConnectedExit()
        {
            string request = BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name));
            Connection.ConnectedExit(request);
        }
        public void AiMove(object request)
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name), request));
        }
        public void InitStage2()
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name)));   
        }
        public void SyncStart()
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name)));   
        }
        public void AttackBoss(object request)
        {
            Connection.Send(BaseJsonFormat.ObjectToJson(Common.ToCamelCase(MethodBase.GetCurrentMethod().Name), request));
        }
    }
}