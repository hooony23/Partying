using Util;
using UnityEngine;
using UnityEngine.AI;

public class Map : MapUtil
{
    private void Start()
    {
        MapObjects = new MapObjects();
        MapObjects.wall = Resources.Load("Map/Wall") as GameObject; // 왼쪽 오른쪽 벽
        MapObjects.UpDownWall = Resources.Load("Map/UpDownWall") as GameObject;// 위 아래 벽
        MapObjects.SpikeTrap = Resources.Load("Map/SpikeTrap") as GameObject;// 가시함정
        MapObjects.HoleTrap = Resources.Load("Map/HoleTrap") as GameObject;// 바닥함정
        MapObjects.AIPoint = new GameObject("AIPoint");// 오브젝트 리스폰 확인 오브젝트
        MapObjects.MazePoint = new GameObject("MazePoint");// 유닛 오브젝트 리스폰 지점확인 오브젝트
        MapObjects.TrapPoint = new GameObject("TrapPoint");// 함정오브젝트 리스폰 확인 오브젝트
        MapObjects.MazeRespwan = new GameObject("MazeRespwan");// AI 이동 지점
        MapObjects.PatrolPoint = new GameObject("PatrolPoint");// AI 리스혼 확인 오브젝트
        MapObjects.MazeBake = new GameObject("MazeBake");// Bake 생성
        MapObjects.MazeBake.AddComponent<NavMeshSurface>();
        CreateMap = new CreateMap((MapInfo)this, Config.ROW, Config.COL);
        CreateMap.CreateGrid();
        
    }
    void Update() // Bake를 최초 갱신하기 위함
    {
        // if (IsMapdone.Equals(true))
        // {
        //     for (int i = 0; i < Surfaces.Length; i++)
        //     {
        //         Surfaces[i].BuildNavMesh();
        //     }
        //     CreateMap.TrapRespwan();
        //     IsMapdone = false;
        // }
    }
}