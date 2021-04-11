using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class GameManagerController : MonoBehaviour
    {
        public List<GameObject> PlayerList { get; set; } = new List<GameObject>();
        public List<GameObject> DeathPlayerList { get; set; } = new List<GameObject>();
    }
}