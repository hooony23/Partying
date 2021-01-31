﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClearItem : MonoBehaviour
{
    private Animator anim;
    // enum 선언으로 열거형 타입으로 아이템 분류
    public enum MapClear { ClearItem };
    public MapClear ItemType;
    public void IsBoxOpen() {
        anim.SetBool("isBoxOpen", true);
    }
}

