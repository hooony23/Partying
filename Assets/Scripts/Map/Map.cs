using Util;
using UnityEngine;
using UnityEngine.AI;

public class Map : MapUtil
{

    private void Start()
    {
        InitializeMap();
        CreateGrid(Config.ROW, Config.COL);
        TrapRespawn();
        PlayerRespawn();

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