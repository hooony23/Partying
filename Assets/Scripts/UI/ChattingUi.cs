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
using Chatting;
using Util;
namespace Partying.UI
{
    public class ChattingUi : MonoBehaviour
    {
        public const string ALLCOLOR = "#FFFFFF";
        public const string LOBBYCOLOR = "#80FFFF";
        public const string ROOMCOLOR = "#FF80FF";
        //채팅 메세지창 배경 조정
        private bool emptychat = false;

        //채팅창의 입력자와 채팅최대길이
        [SerializeField] private string chatTag = "모두";
        /*[SerializeField]*/
        private string username = "asdfgh";
        [SerializeField] private int maxMessages = 25;

        //메세지의 동적생성
        [SerializeField] GameObject chatPanel, isTextBox, isInputChatBox;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] int chatMode, viewMode = 1;
        [SerializeField] private InputField chatBox;
        private string insertMessage;
        [SerializeField] private Text chatBoxButtonText;
        private ChatModule chatModule;
        private RectTransform rectTransform;
        private void Awake()
        {
            chatModule = ChatModule.GetChatModule();
            isTextBox = Resources.Load("GameUi/Chat/Chat Text Form") as GameObject;
        }
        private void Start()
        {
            //채팅 오브젝트사용을 위한 오브젝트 가져오기
            Debug.Log(this.transform);
            GameObject ChatingText = this.transform.GetChild(0).gameObject;
            UnityEngine.Debug.Log(ChatingText.name);
            canvasGroup = ChatingText.GetComponent<CanvasGroup>();
            rectTransform = this.transform.Find("Canvas").GetComponent<RectTransform>();
            isInputChatBox = ChatingText.transform.Find("Chat View").gameObject;
            GameObject ChatInfoScrollView = isInputChatBox.transform.Find("Viewport").gameObject;
            chatPanel = ChatInfoScrollView.transform.Find("Content").gameObject;
            chatBox = ChatingText.transform.Find("User Input").transform.Find("InputField Chat").GetComponent<InputField>();
            chatBoxButtonText = ChatingText.transform.Find("User Input").transform.Find("Button Group").transform.Find("Text").GetComponent<Text>();
            var tmpColor  = new Color();
            ColorUtility.TryParseHtmlString(LOBBYCOLOR, out tmpColor);
            chatBoxButtonText.text = "로비";
            chatBoxButtonText.color = tmpColor;
            var chatOption = ChatingText.transform.Find("Chat Option");
            chatOption.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnChangeListener("Lobby"); });
            chatOption.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnChangeListener("Room"); });
            chatOption.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(delegate {OnChangeListener("All");});

            CanvasRecent(Config.defaultStage);
        }
        void Update()
        {
            chatactive();
            IsTabKeyDown();
            if (chatModule.ReceiveData != null) {
                var data = chatModule.ReceiveData;
                chatModule.ReceiveData = null;
                ReceiveMessageToChat(data);
            }
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
                canvasGroup.blocksRaycasts = true;
                //input창에 문자가 있을시
                if (emptychat)
                {
                    if (!chatBox.text.Equals(""))
                    {
                        //GameObject newText = Instantiate(isTextBox, chatPanel.transform);
                        //AllMessage(chatTag, username, chatBox.text);
                        insertMessage = chatBox.text;
                        
                        if (chatMode == 1) {
                            chatModule.RemoveFromGroup();
                            chatModule.SendMsg(insertMessage);
                        }
                        else if (chatMode == 2)
                        {
                            // var a = NetworkInfo.roomInfo.RoomName;
                            chatModule.AddToGroup();
                            chatModule.SendMessageToGroup(insertMessage);
                        }
                        chatBox.text = "";
                        chatBox.ActivateInputField();
                        //return;
                    }
                    //input창에 아무것도 입력안할시
                    else if (emptychat && chatBox.text.Equals(""))
                    {
                        emptychat = false;
                        chatBox.DeactivateInputField();
                        canvasGroup.alpha = 0.1f;
                        canvasGroup.blocksRaycasts = false;
                        //return;
                    }
                }
                else if (!emptychat) {
                    chatBox.ActivateInputField();
                    emptychat = true;
                }
            }
        }

        public void IsTabKeyDown()
        {
            
            if (Input.GetKeyDown(KeyCode.Tab)) {
                ChatTabMode();
            }
        }
        public void CanvasRecent(int scenesNumber)
        { //채팅창 Canvas의 피벗과 앵커를 설정하여 채팅창의 위치를 바꿈
            if (scenesNumber == 0)
            {//오른쪽 채팅창
                rectTransform.anchorMin = new Vector2(1, 0);
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.pivot = new Vector2(1f, 0f);
            }
            else if (scenesNumber >= 1)
            {//왼쪽 채팅창
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(0, 0);
                rectTransform.pivot = new Vector2(0f, 0f);
            }

        }

        public void ChatTabMode() {
                string text = "로비";
                if(chatMode==1&&NetworkInfo.roomInfo.RoomUuid==null)
                    return;
                if (chatMode==1)
                    chatMode=2;
                else
                    chatMode=1;
                Color color;
                if (emptychat)
                {
                    switch (chatMode)
                    {
                        case 1:
                            chatTag = "Lobby"; 
                            ColorUtility.TryParseHtmlString(LOBBYCOLOR, out color);
                            break;
                        case 2:
                            chatTag = "Room"; 
                            ColorUtility.TryParseHtmlString(ROOMCOLOR, out color);
                            chatBoxButtonText.color =  color;
                            text = "방";
                            break;
                        default:
                            ColorUtility.TryParseHtmlString(ALLCOLOR, out color);
                            break;
                    }
                    chatBoxButtonText.color = color;
                    chatBoxButtonText.text = text;
                }
        }
        public void chatLoad(string sendMessage)
        {
            //chatModule.SendMessage(sendMessage);
            //채팅을 서버로부터 받을 부분 구현예정
        }
        public void ReceiveMessageToChat(string data) 
            //get set으로 주고받는 방식 구현필요(get set구현시 UI의 Text를 List에 추가하는 부분이 오류 생김 수정및 갱신필요)
        {
            //최대길이의 채팅내역이 생기면 가장 오래된 채팅 내역제거
            string Tag = "";
            string type = Lib.Common.GetType(data);
            ChatInfo chatInfo = Lib.Common.GetData(data).ToObject<ChatInfo>();
            switch (type)
            {
                case "SendMessage":
                    Tag = "Lobby";
                    ChattingMassegeInfo.Everychat.Push(chatInfo);
                    AllMessage(chatInfo,Tag);
                    break;
                case "SendMessageToGroup":
                    Tag = "room";
                    ChattingMassegeInfo.Groupchat.Push(chatInfo);
                    AllMessage(chatInfo,Tag);
                    break;

            }
        }
        public void AllMessage(ChatInfo chatInfo,string chatTag,bool isPooling = false) {      
            
            var chatMode = 0;
            var color = new Color();
            if(chatTag.Equals("Lobby")){
                chatMode = 1;
                ColorUtility.TryParseHtmlString(LOBBYCOLOR, out color);
                }
            else{
                chatMode = 2;
                ColorUtility.TryParseHtmlString(ROOMCOLOR, out color);
                }
                
            if(viewMode == chatMode||isPooling)
            {
                Debug.Log(chatMode);
                Debug.Log(this.chatMode);
                Debug.Log(chatTag + " " + chatInfo.Message + " " + chatInfo.Nickname);
                GameObject newText = Instantiate(isTextBox, chatPanel.transform);
                Text tagText = newText.transform.Find("Tag").GetComponent<Text>();
                Text nicknameText = newText.transform.Find("Nickname").GetComponent<Text>();
                Text messageText = newText.transform.Find("Text").GetComponent<Text>();

                tagText.text = chatTag;
                nicknameText.text = chatInfo.Nickname;
                messageText.text = chatInfo.Message;

                tagText.color = color;
                nicknameText.color = color;
                messageText.color = color;
            }
        }
        public void ObjectPulling(string chatTag)
        {
            ChatQueue<ChatInfo> chatQueue = null;
            for(int i = chatPanel.transform.childCount-1;i>0;i--)
            {
                Debug.Log(chatPanel.transform.childCount);
                Destroy(chatPanel.transform.GetChild(i).gameObject);
            }
            if (chatTag.Equals("All"))   //TODO: AllChat, GroupChat timestamp로 정렬 후 출력.
            {
                var temp = new ChatQueue<ChatInfo>(400);
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
            else if (chatTag.Equals("Lobby"))
            {
                viewMode = 1;
                chatQueue = ChattingMassegeInfo.Everychat;
            }
            else {
                chatQueue = ChattingMassegeInfo.Groupchat;
                viewMode = 2;
            }
            foreach(var item in chatQueue)
            {
                Debug.Log(item.ToString());
                AllMessage(item,chatTag,true);
            }

        }
        public void OnChangeListener(string chatTag)
        {
            ObjectPulling(chatTag);
        }
    }

}