using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using GameManager;

public class CMController : MonoBehaviour
{
    public static CMController CMC { get; set; }
    public Cinemachine.CinemachineVirtualCamera vcam;

    private Animator animator;
    private GameManager.GameManager GM;
    
    private void Start()
    {
        vcam = this.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        animator = this.GetComponent<Animator>();
        GM = GameObject.Find("Game Manager").GetComponent<GameManager.GameManager>();

    }

    private void Update()
    {
        
    }

    public void InitTarget(Transform target)
    {
        vcam.Follow = target;
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
