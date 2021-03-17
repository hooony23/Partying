using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChattingUi : MonoBehaviour
{
    //채팅 메세지창 배경 조정
    private bool emptychat = false;
    Color color;
    Image image1, image2;

    //채팅창의 입력자와 채팅최대길이
    [SerializeField] private string username;
    [SerializeField] private int maxMessages = 25;

    //메세지의 동적생성
    [SerializeField] GameObject chatPanel, isTextBox, isInputChatBox;
    [SerializeField] private InputField chatBox;
    [SerializeField] List<Message> messagesList = new List<Message>();
    private string ScoreResult;
    private void Awake()
    {
        //채팅 오브젝트사용을 위한 오브젝트 가져오기
        GameObject ChatingObject = Instantiate(Resources.Load("GameUi/ChatUi")) as GameObject;
        GameObject ChatingText = ChatingObject.transform.Find("ChatingText").gameObject;
        isTextBox = Resources.Load("GameUi/Text") as GameObject;
        isInputChatBox = ChatingText.transform.Find("ChatScroll View").gameObject;
        GameObject ChatInfoScrollView = isInputChatBox.transform.Find("Viewport").gameObject;
        chatPanel = ChatInfoScrollView.transform.Find("Content").gameObject;
        chatBox = ChatingText.transform.Find("InputField").GetComponent<InputField>();
    }
    void Start()
    {
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
    public void SendMessageToChat(string chattext) //get set으로 주고받는 방식 구현필요(get set구현시 UI의 Text를 List에 추가하는 부분이 오류 생김 수정및 갱신필요)
    {
        //최대길이의 채팅내역이 생기면 가장 오래된 채팅 내역제거
        if (messagesList.Count >= maxMessages)
        {
            Destroy(messagesList[0].textObject.gameObject);
            messagesList.Remove(messagesList[0]);
        }
        //text에 있는 내용을 messagesList추가함
        Message newMessage = new Message();
        newMessage.text = chattext;
        GameObject newText = Instantiate(isTextBox, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        messagesList.Add(newMessage);
    }
    public void chatactive()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            color.a = 0.5f;
            image2.color = color;
            image1.color = color;
            chatBox.enabled = true;
            chatBox.ActivateInputField();
            //input창에 문자가 있을시
            if (!chatBox.text.Equals(""))
            {
                string ScoreResult = string.Format("{0} : {1}", username, chatBox.text);
                SendMessageToChat(ScoreResult);
                chatBox.text = "";
                color.a = 0f;
                image1.color = color;
                image2.color = color;
                chatBox.enabled = false;
                emptychat = false;
                return;
            }
            //input창에 아무것도 입력안할시
            if (emptychat && chatBox.text.Equals(""))
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
    public void chatLoad()
    {
        //채팅을 서버로부터 받을 부분 구현예정
    }
}
[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
