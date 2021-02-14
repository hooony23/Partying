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
        MapObjects.SpikeTrap = Resources.Load("Trap/SpikeTrap") as GameObject;// 가시함정
        MapObjects.HoleTrap = Resources.Load("Trap/HoleTrap") as GameObject;// 바닥함정
        MapObjects.SlowTrap = Resources.Load("Trap/SlowTrap") as GameObject;// 슬로우함정
        MapObjects.DangerZone = Resources.Load("Trap/DangerZone") as GameObject;// 위험 지역
        MapObjects.AIPoint = new GameObject("AIPoint");// 오브젝트 리스폰 확인 오브젝트
        MapObjects.MazePoint = new GameObject("MazePoint");// 유닛 오브젝트 리스폰 지점확인 오브젝트
        MapObjects.TrapPoint = new GameObject("TrapPoint");// 함정오브젝트 리스폰 확인 오브젝트
        MapObjects.MazeRespwan = new GameObject("MazeRespwan");// AI 이동 지점
        MapObjects.MazeBake = new GameObject("MazeBake");// Bake 생성
        MapObjects.MazeBake.AddComponent<NavMeshSurface>();
        MapObjects.AIPoint.transform.SetParent(this.transform);
        MapObjects.MazePoint.transform.SetParent(this.transform);
        MapObjects.TrapPoint.transform.SetParent(this.transform);
        MapObjects.MazeRespwan.transform.SetParent(this.transform);
        MapObjects.MazeBake.transform.SetParent(this.transform);
        CreateGrid(Config.ROW,Config.COL);
        
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