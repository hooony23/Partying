using Newtonsoft.Json;


namespace Communication.JsonFormat
{
    public class BaseJsonFormat
    {
        private string type;
        private string uuid;
        private object data;
        public string ObjectToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static string ObjectToJson(string type, string uuid, object data)
        {
            /// <summary>
            /// 현재 오브젝트를 json 형식의 string으로 반환합니다.
            /// </summary>
            /// <returns>
            /// 
            /// {
            ///     "uuid":"",
            ///     "type:"",
            ///     "data":{
            ///         "object"
            ///     }
            /// }
            /// </returns>
            string _type = type;
            string _uuid = uuid;
            object _data = data;
            return JsonConvert.SerializeObject(new { type = _type, uuid = _uuid, _data = data });
        }
        public void SetValues(string type, string uuid, object data)
        {
            this.type = type;
            this.Uuid = uuid;
            this.Data = data;
        }
        public string Type { get => type; set => type = value; }
        public string Uuid { get => uuid; set => uuid = value; }
        public object Data { get => data; set => data = value; }
    }
}