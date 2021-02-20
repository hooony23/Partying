using Util;
using UnityEngine;
using UnityEngine.AI;

public class Map : MapUtil
{
    
    private void Start()
    {
        MapInfo = Config.mapInfo;
        MapObjects = new MapObjects();
        InitializeMap();
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