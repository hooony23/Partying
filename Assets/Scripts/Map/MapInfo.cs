using Newtonsoft.Json.Linq;
using UnityEngine.AI;

public class MapInfo
{
    //TODO GETTER SETTER  만들고 private 접근자로 바꿀 것

    public MazeCell[,] grid; //미로를 만들기 위한 격자 생성
    public int Rows; // 행에 대한 미로찾기를 위한 처음의 시작값
    public int Columns; // 열에 대한 미로찾기를 위한 처음의 시작값
    public string testJson;

    public JObject testJsonObject;
    public JObject data;
    public NavMeshSurface[] surfaces;

    public bool isMapdone = false;
}