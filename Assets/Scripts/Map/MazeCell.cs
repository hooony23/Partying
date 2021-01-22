using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MazeCell
{
    public bool Visited = false; //미로칸을 방무했는지 구분하는 코드 false로 설정함
    public bool ResponCheck = false; //스폰지점 생성 구분하는 코드
    public GameObject Respwan;
    public GameObject AiPoint;
    public GameObject UpWall;
    public GameObject DownWall;
    public GameObject LeftWall;
    public GameObject RightWall;
}

