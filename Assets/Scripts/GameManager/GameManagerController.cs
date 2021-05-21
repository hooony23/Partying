using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GameManager
{
    public class GameManagerController : MonoBehaviour
    {
        public int currentStage=Util.Config.defaultStage;
        public GameObject PlayerCamera {get;set;}
        public GameObject GameClearUi {get;set;} = null;
        public GameObject GameOverUi {get;set;} = null;
        public Button ContinueButton = null;
        public bool GameClear {get;set;} = false;
        public bool GameStart {get;set;} = false;
        public bool GameOver {get;set;} = false;
        public List<GameObject> PlayerList { get; set; } = new List<GameObject>();
        public List<GameObject> DeathPlayerList { get; set; } = new List<GameObject>();
    }
}