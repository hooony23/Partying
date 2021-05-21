using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Util;

public class MapClearItem : MonoBehaviour
{
    // enum 선언으로 열거형 타입으로 아이템 분류
    public enum MapClear { ClearItem };
    public MapClear ItemType;

    //게임클리어 Ui
    private Animator animator;
    private AudioSource openSound;
    public bool isOpened=false;
    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        openSound = this.GetComponent<AudioSource>();
    }
    private void Update()
    {
        //TODO: 만약 아이템이 추가되면 이 부분 수정 필요.
        
        if (Communication.NetworkInfo.GetItemUserQueue.Count != 0)
        {
            this.isOpened=true;
        }  
        animator.SetBool("IsBoxOpen", isOpened);
    }
    public void BoxOpenSound() {
        openSound.Play();
    }
}

