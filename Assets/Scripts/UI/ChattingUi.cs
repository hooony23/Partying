using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Partying.UI;
using Newtonsoft.Json.Linq;
using Communication.JsonFormat;
using Communication;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using signalR_Test_Client;
namespace Partying.UI
{
    public class ChattingUi : MonoBehaviour
    {
        //채팅 메세지창 배경 조정
        public const string ALLCOLOR = "#FFFFFF";
        public const string LOBBYCOLOR = "#80FFFF";
        public const string ROOMCOLOR = "#FF80FF";
        public const string ALLCHATVIEW = "All";
        public const string LOBBYCHATVIEW = "LOBBY";
        public const string ROOMCHATVIEW = "ROOM";

        /*[SerializeField]*/
        private string username = "asdfgh";
        [SerializeField] private int maxMessages = 25;

        //메세지의 동적생성
        [SerializeField] GameObject chatPanel, isTextBox, isInputChatBox;
        [SerializeField] CanvasGroup canvasGroup;

        //상단 채팅 버튼 클릭 시 변경.
        [SerializeField] int chatViewMode = 0;

        //채팅창 입력 활성화 상태에서 tab 키 누를 시 변경.
        [SerializeField] int chatMode = 1;
        [SerializeField] private InputField chatBox;
        public string userMessage;
        [SerializeField] private Text chatModeButton;
        ChatModule chatModule = new ChatModule();
        private void Awake()
        {
            chatModule.Start();
            GameObject ChatingText = this.transform.Find("Canvas").gameObject;
            canvasGroup = ChatingText.GetComponent<CanvasGroup>();
            isTextBox = Resources.Load("Chat/Chat Text Form") as GameObject;
            isInputChatBox = ChatingText.transform.Find("Chat View").gameObject;
            GameObject ChatInfoScrollView = isInputChatBox.transform.Find("Viewport").gameObject;
            chatPanel = ChatInfoScrollView.transform.Find("Content").gameObject;
            chatBox = ChatingText.transform.Find("User Input").transform.Find("InputField Chat").GetComponent<InputField>();
            chatModeButton = ChatingText.transform.Find("User Input").transform.Find("Button Group").transform.Find("Text").GetComponent<Text>();
            chatModeButton.text = "로비";
            var chatViewGroup = GameObject.Find("ChatUI").transform.GetChild(0).Find("Chat Option");
            chatViewGroup.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnChangeListener(ALLCHATVIEW); });
            chatViewGroup.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnChangeListener(LOBBYCHATVIEW); });
            chatViewGroup.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnChangeListener(ROOMCHATVIEW); });
        }
        void Update()
        {
            EnterKeyEvent();
            ChatTabMode();
            IsReceiveData();
        }
        public void EnterKeyEvent()
        {
            if (!IsEnterKeyDown())
                return;
            if (!canvasGroup.blocksRaycasts)
            {
                ChatActive(true);
                return;    
            }
            if (chatBox.text.Replace(" ","").Equals(""))
            {
                ChatActive(false);
                return;
            }
            SendMessage();
            
        }
        
        public void ChatTabMode()
        {
            if (!(canvasGroup.blocksRaycasts&&IsTabKeyDown()))
            {
                return;
            }

            string text = "모두";
            if (chatMode == 1)
                chatMode = 2;
            else
                chatMode = 1;
            Color color;
            if (chatBox.text.Equals(""))
            {
                switch (chatMode)
                {
                    case 1:
                        ColorUtility.TryParseHtmlString(LOBBYCOLOR, out color);
                        break;
                    case 2:
                        ColorUtility.TryParseHtmlString(ROOMCOLOR, out color);
                        chatModeButton.color = color;
                        text = "방";
                        break;
                    default:
                        ColorUtility.TryParseHtmlString(ALLCOLOR, out color);
                        break;
                }
                chatModeButton.color = color;
                chatModeButton.text = text;
            }
        }
        public void IsReceiveData()
        {
            if (chatModule.ReceiveData != null)
            {
                var data = chatModule.ReceiveData;
                chatModule.ReceiveData = null;
                ReceiveMessageToChat(data);
            }
        }
        
        public void ChatActive(bool isActive)
        {
            if (isActive)
            {
                canvasGroup.alpha = 0.8f;
                canvasGroup.blocksRaycasts = true;
                chatBox.ActivateInputField();
            }
            else
            {
                chatBox.DeactivateInputField();
                canvasGroup.alpha = 0.1f;
                canvasGroup.blocksRaycasts = false;
            }
        }
        public bool IsEnterKeyDown()
        {
            if (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.KeypadEnter))
                return true;
            return false;
        }
        public void SendMessage()
        {
            //input창에 문자가 있을시
            userMessage = chatBox.text;

            if (chatMode == 1)
            {
                chatModule.RemoveFromGroup();
                chatModule.SendMsg(userMessage);
            }
            else if (chatMode == 2)
            {
                // var a = NetworkInfo.roomInfo.RoomName;
                chatModule.AddToGroup();
                chatModule.SendMessageToGroup(userMessage);
            }
            chatBox.text = "";
            chatBox.ActivateInputField();
        }
        public bool IsTabKeyDown()
        {

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                return true;
            }
            return false;
        }
        public void ReceiveMessageToChat(string data)
        //get set으로 주고받는 방식 구현필요(get set구현시 UI의 Text를 List에 추가하는 부분이 오류 생김 수정및 갱신필요)
        {
            //채팅 버퍼가 가득 차면 가장 뒤에 것 부터 제거
            string type = Lib.Common.GetType(data);
            ChatInfo chatInfo = Lib.Common.GetData(data).ToObject<ChatInfo>();
            switch (type)
            {
                case "SendMessage":
                    chatInfo.C = LOBBYCOLOR;
                    chatInfo.ChatType = LOBBYCHATVIEW;
                    ChattingMassegeInfo.Allchat.Push(chatInfo);
                 ViewPrintText(chatInfo, chatInfo.ChatType);
                    break;
                case "SendMessageToGroup":
                    chatInfo.C = ROOMCOLOR;
                    chatInfo.ChatType = ROOMCHATVIEW;
                    ChattingMassegeInfo.Groupchat.Push(chatInfo);
                 ViewPrintText(chatInfo, chatInfo.ChatType);
                    break;

            }
        }
        public void ViewPrintText(ChatInfo chatInfo, string chatType)
        {

            var chatMode = 0;
            var color = new Color();
            ColorUtility.TryParseHtmlString(chatInfo.C, out color);
            if (chatType.Equals(LOBBYCHATVIEW))
            {
                chatMode = 1;
            }
            else if(chatType.Equals(ROOMCHATVIEW))
            {
                chatMode = 2;
            }
            Debug.Log($"input chat mode : {chatMode}\n current chat mode : {this.chatMode}");
            if (chatMode == this.chatViewMode || this.chatViewMode == 0)
            {
                Debug.Log(chatType + " " + chatInfo.Message + " " + chatInfo.Nickname);
                GameObject newText = Instantiate(isTextBox, chatPanel.transform);
                Text chatTypeText = newText.transform.Find("Tag").GetComponent<Text>();
                Text nicknameText = newText.transform.Find("Nickname").GetComponent<Text>();
                Text messageText = newText.transform.Find("Text").GetComponent<Text>();

                chatTypeText.text = chatType;
                nicknameText.text = chatInfo.Nickname;
                messageText.text = chatInfo.Message;

                chatTypeText.color = color;
                nicknameText.color = color;
                messageText.color = color;
            }
        }
        
        public void OnChangeListener(string chatViewMode)
        {
            ObjectPulling(chatViewMode);
        }
        public void ObjectPulling(string chatViewMode)
        {
            for (int i = chatPanel.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(chatPanel.transform.GetChild(i).gameObject);
            }
            
            ChatQueue<ChatInfo> chatQueue = null;
            if (chatViewMode.Equals(LOBBYCHATVIEW)){
                this.chatViewMode = 1;
                chatQueue = ChattingMassegeInfo.Allchat;
                }
            else if (chatViewMode.Equals(ROOMCHATVIEW)){
                this.chatViewMode = 2;
                chatQueue = ChattingMassegeInfo.Groupchat;
                }
            else   //TODO: AllChat, GroupChat timestamp로 정렬 후 출력.
            {
                this.chatViewMode = 0;
                var chatInfo = new ChatInfo();
                var temp = new ChatQueue<ChatInfo>();
                foreach (var item in ChattingMassegeInfo.Allchat)
                {
                    temp.Enqueue(item);
                }
                foreach (var item in ChattingMassegeInfo.Groupchat)
                {
                    temp.Enqueue(item);
                }
                chatQueue = temp;
            }
            Debug.Log($"object pulling size : {chatQueue.Count}");
            foreach (var item in chatQueue)
            {
                Debug.Log($"object pulling item : {item.ToString()}");
                ViewPrintText(item, item.ChatType);
            }

        }
    }

}