using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

namespace Communication.JsonFormat
{
public class ChatInfo {
        public string ChatType {get; set; } = "";
        public string Nickname { get; set; } = "";
        public string Message { get; set; } = "";
        public double Time { get; set; } = Lib.Common.ConvertToUnixTimestamp(DateTime.Now);
        public string C {get; set; } = "";
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        public void SetChatInfo(string nickname,string message)
        {
            Nickname = nickname;
            Message = message;
        }
        public void SetChatInfo(ChatInfo chatInfo)
        {
            Nickname = chatInfo.Nickname;
            Message = chatInfo.Message;
        }
    }
}