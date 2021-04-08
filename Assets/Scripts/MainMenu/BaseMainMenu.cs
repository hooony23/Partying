using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseMainMenu : MonoBehaviour
{
    protected GameObject mainMenuCanvas;
    protected GameObject textMessage;
    protected string serverMsg = "";
    protected int backUINum = -1;
    protected int UINum = 0;
    protected int nextUINum = -1;
    void Awake()
    {
        mainMenuCanvas = GameObject.Find("Main Menu Canvas");
        GameObject parent = this.gameObject;
        textMessage = Instantiate(Resources.Load("MainMenu/Prefab/TextMessage"),parent:parent.transform) as GameObject;
        textMessage.name = Resources.Load("MainMenu/Prefab/TextMessage").name;
        textMessage.SetActive(false);
        // textMessage.transform.position = new Vector3(380,280+130,0);
    }
    protected void SetwarningText(string message, float time = 3f)
    {
        textMessage.SetActive(true);
        textMessage.GetComponent<Text>().text = message;
        Invoke("ResetMessage", time);
    }
    protected void ResetMessage()
    {
        textMessage.GetComponent<Text>().text = "";
        textMessage.SetActive(false);
    }
    protected void NextUI()
    {
        this.gameObject.SetActive(false);
        if(nextUINum != -1)
            mainMenuCanvas.transform.GetChild(nextUINum).gameObject.SetActive(true);
        else
            mainMenuCanvas.transform.GetChild(++UINum).gameObject.SetActive(true);
    }
    protected void BackUI()
    {
        this.gameObject.SetActive(false);
        if(backUINum != -1)
            mainMenuCanvas.transform.GetChild(backUINum).gameObject.SetActive(true);
        else
            mainMenuCanvas.transform.GetChild(--UINum).gameObject.SetActive(true);
    }
}