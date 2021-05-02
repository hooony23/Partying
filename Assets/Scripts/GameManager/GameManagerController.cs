using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameManager
{
    public class GameManagerController : MonoBehaviour
    {
        public int currentStage=Util.Config.defaultStage;
        public GameObject GameClearUi {get;set;} = null;
        public Button ContinueButton = null;
        public bool gameClear {get;set;} = false;
        public List<GameObject> PlayerList { get; set; } = new List<GameObject>();
        public List<GameObject> DeathPlayerList { get; set; } = new List<GameObject>();
    }
}