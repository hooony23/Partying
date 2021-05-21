using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using GameManager;

public class CMController : MonoBehaviour
{
    public static CMController CMC { get; set; }
    public Cinemachine.CinemachineVirtualCamera vcam;

    private Player player;
    private Animator animator;
    private Transform target = null;
    private List<GameObject> playerList;
    private GameManager.GameManager gameManager;
    private int targetNum = 0;

    
    private void Start()
    {
        vcam = this.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        animator = this.GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager.GameManager>();
        playerList = gameManager.PlayerList;

    }

    private void Update()
    {
        UpdateFirstTarget();
        CameraChange();
    }
    public void UpdateFirstTarget()
    {
        if (vcam.Follow != null)
            return;

        GameObject playerObj = GameObject.Find(Config.userUuid);
        this.player = playerObj.GetComponent<Player>();
        this.target = playerObj.transform.Find("CM Follow Target");
        vcam.Follow = this.target;

    }

    public void ChangeTarget()
    {
        Debug.Log("카메라 변경");
        if (targetNum == playerList.Count)
            targetNum = 0;
        // 안될 시 변경
        //GameObject.Find(playerList[targetNum].ToString()).transform.Find("CM Follow Target");
        vcam.Follow = playerList[targetNum].transform;
        targetNum += 1;
    }

    public void CameraChange()
    {
        if (Input.GetMouseButton(2) && player.IsDead)
        {
            Debug.Log("카메라 체인쥥쥥쥥쥥");
            ChangeTarget();
        }
    }

    public void Aim()
    {
        animator.SetBool("isAimming", true);
    }

    public void AimOut()
    {
        animator.SetBool("isAimming", false);
    }

}
