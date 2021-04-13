using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using Communication.JsonFormat;
public class MapController : MonoBehaviour
{
    protected enum Direction { LEFT, RIGHT, UP, DOWN }// LEFT = 0, RIGHT = 1, UP = 2, DOWN = 3
    //TODO GETTER SETTER  만들고 private 접근자로 바꿀 것
    public MapInfo MInfo { get; set; }
    public MapObjects MapObjects { get; set; }
    public MazeCell[,] Grid { get; set; }
    public NavMeshSurface[] Surfaces { get; set; }
    public bool IsMapdone { get; set; }
}