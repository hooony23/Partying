using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Communication;
using GameManager;
namespace Animation
{
    
public class ItemDestroyEvent : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            Debug.Log("item destroyed");
            var gameManager = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
            gameManager.GameClear = true;
            string userUuid = NetworkInfo.GetItemUserQueue.Dequeue();
            Destroy(GameObject.Find("GameClearItem"));
    }
}

}