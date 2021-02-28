using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUi : MonoBehaviour
{
    private bool emptychat = false;
    Color color;
    Image image1, image2;

    [SerializeField]
    private string username;
    [SerializeField]
    private int maxMessages = 25;
    [SerializeField]
    GameObject chatPanel, isTextBox, isInputChatBox;
    [SerializeField]
    private InputField chatBox;
    [SerializeField]
    List<Message> messagesList = new List<Message>();
   // public int count=0;
    void Start()
    {
        isInputChatBox = GameObject.Find("ChatScroll View");
        image1 = isInputChatBox.GetComponent<Image>();
        image2 = chatBox.GetComponent<Image>();
    }
    void Update()
    {
        chatactive();
    }
    /// <summary>
    /// 채팅을 서버로 보내는 부분
    /// </summary>
    /// <param name="text">채팅의 내용을 서버로 전송하는 부분 구현예정. chatBox에 채팅내용은 서버수신부분 구현 후 수정예정</param>
    public void SendMessageToChat(string text) {
        if (messagesList.Count >= maxMessages) {
            Destroy(messagesList[0].textObject.gameObject);
        messagesList.Remove(messagesList[0]);
    }
        //text에 있는 내용을 messagesList추가함
        Message newMessage = new Message();
        newMessage.text = text;
        GameObject newText = Instantiate(isTextBox, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        messagesList.Add(newMessage);
    }
    public void chatactive() {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            color.a = 0.5f;
            image2.color = color;
            image1.color = color;
            chatBox.enabled = true;
            chatBox.ActivateInputField();
            if (!chatBox.text.Equals(""))
            {
                SendMessageToChat(username + ": " + chatBox.text);
                chatBox.text = "";
                color.a = 0f;
                image1.color = color;
                image2.color = color;
                chatBox.enabled = false;
                emptychat = false;
                return;
            }
            if (emptychat&& chatBox.text.Equals(""))
            {
                color.a = 0f;
                image1.color = color;
                image2.color = color;
                chatBox.enabled = false;
                emptychat = false;
                return;
            }
            emptychat = true;
        }
    }
    public void chatLoad() { 
    //채팅을 서버로부터 받을 부분 구현예정
    }
}
[System.Serializable]
public class Message {
    public string text;
    public Text textObject;
}
