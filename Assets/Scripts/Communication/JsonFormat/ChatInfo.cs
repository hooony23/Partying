using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Communication.JsonFormat
{
public class ChatInfo {
        public string Nickname { get; set; } = "";
        public string Message { get; set; } = "";
        public double Time { get; set; } = Lib.Common.ConvertToUnixTimestamp(DateTime.Now);
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}