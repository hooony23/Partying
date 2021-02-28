using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserScoreInfo : MonoBehaviour
{
    [SerializeField]
    private GameObject InfoPanel, textObject, scroolPanel;
    private Animator animator;
    private bool userInfoOpen = false;
    [SerializeField]
    List<UserScore> userInfo = new List<UserScore>();
    public void Awake()
    {
      animator = InfoPanel.GetComponent<Animator>();
    }
    public void Start()
    {
        for (int i = 0; i < 4; i++) {
            SendMessageToChat("Player "+i +"Score : "+"0000000");
        }

    }
    public void Update()
    {
        if (Config.StartGame) {
            OpenPanel();
        }
    }
    public void OpenPanel() {
            if (Input.GetKeyDown(KeyCode.Tab))
        {
                userInfoOpen = animator.GetBool("UserInfoOpen");
            animator.SetBool("UserInfoOpen", userInfoOpen);
                
            }
            if (Input.GetKeyUp(KeyCode.Tab))
        {
                userInfoOpen = animator.GetBool("UserInfoOpen");
                animator.SetBool("UserInfoOpen", !userInfoOpen);

        }
        }
    public void SendMessageToChat(string text)
    {
        //text에 있는 내용을 messagesList추가함
        UserScore NewUserText = new UserScore();
        NewUserText.text = text;
        GameObject newText = Instantiate(textObject, scroolPanel.transform);
        NewUserText.textObject = newText.GetComponent<Text>();
        NewUserText.textObject.text = NewUserText.text;
        userInfo.Add(NewUserText);
    }
}
[System.Serializable]
public class UserScore {
    public string text;
    public Text textObject;
}
