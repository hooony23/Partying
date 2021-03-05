using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserScoreInfo
{
    private string text;
    private int score;

    public UserScoreInfo(string name, int score) {
        this.text = name;
        this.score = score;
    }
    public string Text { get => text; set => text = value;}
    public int Score { get => score; set => score = value; }
}