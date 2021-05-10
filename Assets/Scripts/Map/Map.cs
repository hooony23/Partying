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
        PatrolUnitRespawn();
        PatrolPointRespawn();
        ClearItemRespawn();
        PlayerRespawn();
        NavMeshSurface surface = GameObject.Find("Floor").GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();
    }
}