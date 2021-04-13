using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using Communication.JsonFormat;
public class MapController : MonoBehaviour
{
    protected enum Direction { LEFT, RIGHT, UP, DOWN }// LEFT = 0, RIGHT = 1, UP = 2, DOWN = 3
    //TODO GETTER SETTER  만들고 private 접근자로 바꿀 것
    private MapInfo mapInfo;
    private MazeCell[,] grid;
    private NavMeshSurface[] surfaces;
    private bool isMapdone = false; // ??
    private MapObjects mapObjects;
    public MapInfo MapInfo { get => mapInfo; set => mapInfo = value; }
    public MapObjects MapObjects { get => mapObjects; set => mapObjects = value; }
    public MazeCell[,] Grid { get => grid; set => grid = value; }
    public NavMeshSurface[] Surfaces { get => surfaces; set => surfaces = value; }
    public bool IsMapdone { get => isMapdone; set => isMapdone = value; }
}