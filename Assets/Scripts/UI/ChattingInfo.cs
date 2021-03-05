using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ChattingInfo
{
    private string username;
    private string chatText;
    private string chatMessage;
    private Text text;
    public ChattingInfo(string name, string chatText)
    {
        this.username = name;
        this.chatText = chatText;
    }
    public string Username { get => username; set => username = value; }
    public string ChatText { get => chatText; set => chatText = value; }
    public string ChatMessage { get => chatMessage; set => chatMessage = value; }
    public Text Text { get => text; set => text = value; }
}
