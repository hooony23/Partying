using UnityEngine;
using UnityEngine.UI;

public class BaseMainMenu : MonoBehaviour
{

    protected GameObject textMessage;
    protected string serverMsg = "";
    void Awake()
    {
        GameObject parent = this.gameObject;
        textMessage = Instantiate(Resources.Load("MainMenu/Prefab/TextMessage"),parent:parent.transform) as GameObject;
        textMessage.name = Resources.Load("MainMenu/Prefab/TextMessage").name;
        textMessage.SetActive(false);
        // textMessage.transform.position = new Vector3(380,280+130,0);
    }
    protected void SetWarnigText(string message, float time = 3f)
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
}