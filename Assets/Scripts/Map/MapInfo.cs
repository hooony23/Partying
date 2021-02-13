using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;

public class MapInfo : MonoBehaviour
{
    //TODO GETTER SETTER  만들고 private 접근자로 바꿀 것

    private MazeCell[,] grid; //미로를 만들기 위한 격자 생성
    private int rows; // 행에 대한 미로찾기를 위한 처음의 시작값
    private int columns; // 열에 대한 미로찾기를 위한 처음의 시작값
    private string testJson;

    private JObject testJsonObject;
    private JObject data;
    private NavMeshSurface[] surfaces;
    private bool isMapdone = false;
    private MapObjects mapObjects;
    private CreateMap createMap;
    public MapObjects MapObjects{get=>mapObjects;set=>mapObjects=value;}
    public CreateMap CreateMap{get=>createMap;set=>createMap=value;}
    public MazeCell[,] Grid{get=> grid;set=>grid=value;}
    public int Rows {get=> rows; set => rows = value; }
    public int Columns {get=> columns; set => columns = value; }
    public string TestJson { get => testJson; set => testJson = value; }
    public JObject TestJsonObject {get=> testJsonObject; set => testJsonObject = value; }
    public JObject Data {get=> data; set => data = value;}
    public NavMeshSurface[] Surfaces {get=>surfaces;set=>surfaces = value;}
    public bool IsMapdone {get=> isMapdone; set => isMapdone = value; }
}