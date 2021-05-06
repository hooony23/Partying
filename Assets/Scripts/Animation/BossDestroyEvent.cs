using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Communication;
using GameManager;
namespace Animation
{
    
public class BossDestroyEvent : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var boss = GameObject.Find("Boss");
        var bossInfo = boss.GetComponent<Boss.Boss>();
        if (bossInfo.Pattern==Communication.JsonFormat.BossInfo.Patterns.DIE)
        {
            var gameManager = GameObject.Find("GameManager").GetComponent<GameManager.RaidGameManager>();
            gameManager.GameClear = true;
        }
    }
}

}