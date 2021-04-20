using signalR_Test_Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Partying.UI;
using Newtonsoft.Json.Linq;

namespace Partying.UI
{
    public class ChattingUi : MonoBehaviour
    {
        //채팅 메세지창 배경 조정
        private bool emptychat = false;

        //채팅창의 입력자와 채팅최대길이
        [SerializeField] private string tag = "모두";
        /*[SerializeField]*/
        private string username = "asdfgh";
        [SerializeField] private int maxMessages = 25;

        //메세지의 동적생성
        [SerializeField] GameObject chatPanel, isTextBox, isInputChatBox;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] int Chatmodecount = 1;
        [SerializeField] private InputField chatBox;
        [SerializeField] private Text bb;
        [SerializeField] List<Message> messagesList = new List<Message>();

        //ChatModule chatModule = new ChatModule();
        private void Awake()
        {
            //chatModule.Start();
            //채팅 오브젝트사용을 위한 오브젝트 가져오기
            GameObject ChatingObject = Instantiate(Resources.Load("Chat/ChatUI")) as GameObject;
            GameObject ChatingText = ChatingObject.transform.Find("Canvas").gameObject;
            canvasGroup = ChatingText.GetComponent<CanvasGroup>();
            isTextBox = Resources.Load("Chat/Chat Text Form") as GameObject;
            isInputChatBox = ChatingText.transform.Find("Chat View").gameObject;
            GameObject ChatInfoScrollView = isInputChatBox.transform.Find("Viewport").gameObject;
            chatPanel = ChatInfoScrollView.transform.Find("Content").gameObject;
            chatBox = ChatingText.transform.Find("User Input").transform.Find("InputField Chat").GetComponent<InputField>();
            bb = ChatingText.transform.Find("User Input").transform.Find("Button Group").transform.Find("Text").GetComponent<Text>();
        }
        void Update()
        {
            chatactive();
            chatTap();
        }
        /// <summary>
        /// 채팅을 서버로 보내는 부분
        /// </summary>
        /// <param name="text">채팅의 내용을 서버로 전송하는 부분 구현예정. chatBox에 채팅내용은 서버수신부분 구현 후 수정예정</param>
        public void chatactive()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {

                canvasGroup.alpha = 0.8f;
                //input창에 문자가 있을시
                if (emptychat)
                {
                    if (!chatBox.text.Equals(""))
                    {
                        //GameObject newText = Instantiate(isTextBox, chatPanel.transform);
                        Allmassage(tag, username, chatBox.text);
                        chatLoad(chatBox.text);
                        chatBox.text = "";

                        chatBox.ActivateInputField();
                        return;
                    }
                    //input창에 아무것도 입력안할시
                    else if (emptychat && chatBox.text.Equals(""))
                    {
                        emptychat = false;
                        chatBox.DeactivateInputField();
                        canvasGroup.alpha = 0.1f;
                        canvasGroup.blocksRaycasts = false;
                        return;
                    }
                }
                else if (!emptychat) {
                    chatBox.ActivateInputField();
                    emptychat = true;
                }
            }
        }
        public void chatTap() {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                if (emptychat)
                {
                    switch (Chatmodecount)
                    {        
                        case 0:
                            Chatmodecount = 1;
                            tag = "모두";
                            bb.text = "<color=#FFFFFF>" + tag + "</color>";
                            break;
                        case 1:
                            tag = "전체"; 
                            bb.text = "<color=#80FFFF>" + tag + "</color>";
                            Chatmodecount = 2;
                            break;
                        case 2:
                            tag = "방"; 
                            bb.text = "<color=#FF80FF>" + tag + "</color>";
                            Chatmodecount = 0;
                            break;
                    }
                }
                    
            }
        }
        public void chatLoad(string sendMessage)
        {
            //chatModule.SendMessage(sendMessage);
            //채팅을 서버로부터 받을 부분 구현예정
        }
        public void SendMessageToChat(string data) 
            //get set으로 주고받는 방식 구현필요(get set구현시 UI의 Text를 List에 추가하는 부분이 오류 생김 수정및 갱신필요)
        {
            //최대길이의 채팅내역이 생기면 가장 오래된 채팅 내역제거

            JObject JsonData = JObject.Parse(data);

            string typestring = JsonData["type"].Value<string>();
            string nickname = "";
            string Tag = "";
            string message = "";
            switch (typestring)
            {
                case "SendMessage":
                    Tag = "All";
                    message = JsonData["data"]["message"].Value<string>();
                    nickname = "";
                    Allmassage(Tag, nickname, message);
                    break;
                case "SendMessageToGroup":
                    message = JsonData["data"]["message"].Value<string>();
                    break;
                case "AddToGroup":
                    message = JsonData["data"]["message"].Value<string>();
                    break;
                case "RemoveFromGroup":
                    message = JsonData["data"]["message"].Value<string>();
                    break;
            }

            Tag = typestring;
            if (messagesList.Count >= maxMessages)
            {
                Destroy(messagesList[0].textObject.gameObject);
                Destroy(messagesList[0].Nickname.gameObject);
                Destroy(messagesList[0].colum.gameObject);
                Destroy(messagesList[0].Tag.gameObject);
                messagesList.Remove(messagesList[0]);
            }
            //text에 있는 내용을 messagesList추가함
        }
        public void Allmassage(string Tag,string nickname, string chattext) {
            Message newMessage = new Message();
            newMessage.text = chattext;
            newMessage.nickname = nickname;
            newMessage.tag = Tag;
            GameObject newText = Instantiate(isTextBox, chatPanel.transform);
            newMessage.Tag = newText.transform.Find("Tag").GetComponent<Text>();
            newMessage.Nickname = newText.transform.Find("Nickname").GetComponent<Text>();
            newMessage.textObject = newText.transform.Find("Text").GetComponent<Text>();
            newMessage.colum = newText.transform.Find(":").GetComponent<Text>();
            newMessage.textObject.text = newMessage.text;
            newMessage.Nickname.text = newMessage.nickname;
            newMessage.Tag.text = newMessage.tag;
            messagesList.Add(newMessage);
        }
    }
    [System.Serializable]
    public class Message
    {
        public string text, tag, nickname;
        public Text textObject, Tag, Nickname, colum;
    }
}