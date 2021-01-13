using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MazePoint
{
    public GameObject Respwan;
    public GameObject AiPoint;
    public bool ResponCheck = false;
}
public class MazeCell
{
    public bool Visited = false; //미로칸을 방무했는지 구분하는 코드 false로 설정함
    public GameObject UpWall;
    public GameObject DownWall;
    public GameObject LeftWall;
    public GameObject RightWall;
}

