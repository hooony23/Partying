using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Communication.MainServer;
public class BaseMainMenu : MonoBehaviour
{
    protected GameObject mainMenuCanvas;
    protected GameObject textMessage;
    protected string serverMsg = "";
    protected int backUINum = -1;
    protected int UINum = 0;
    protected int nextUINum = -1;
    protected virtual void Awake()
    {
        mainMenuCanvas = GameObject.Find("Main Menu Canvas");
        GameObject parent = this.gameObject;
        textMessage = Instantiate(Resources.Load("MainMenu/Prefab/TextMessage"), parent: parent.transform) as GameObject;
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
    protected virtual void NextUI()
    {
        SetActive(false);
        mainMenuCanvas.transform.GetChild(++UINum).gameObject.SetActive(true);
    }
    protected virtual void BackUI()
    {
        SetActive(false);
        mainMenuCanvas.transform.GetChild(--UINum).gameObject.SetActive(true);
    }
    protected virtual void SelectUI(int selectUINum)
    {
        SetActive(false);
        mainMenuCanvas.transform.GetChild(selectUINum).gameObject.SetActive(true);
    }
    private void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
    protected virtual void OnApplicationQuit()
    {
        MServer.Communicate("GET", "api/v1/session/signOut", $"userUuid={Util.Config.userUuid}");
    }
}