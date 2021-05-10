using Communication.JsonFormat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Communication
{
    public class ChattingMassegeInfo
    {
        public static ChatQueue<ChatInfo> Allchat = new ChatQueue<ChatInfo>();
        public static ChatQueue<ChatInfo> Everychat = new ChatQueue<ChatInfo>();
        public static ChatQueue<ChatInfo> Groupchat = new ChatQueue<ChatInfo>();
    }
}