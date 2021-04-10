using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class GameManagerController : MonoBehaviour
    {
        public List<string> PlayerList{get; set;} = new List<string>();
        public List<string> DeathPlayerList{get; set;} = new List<string>();
    }
}