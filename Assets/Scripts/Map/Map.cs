using Util;
using UnityEngine;
using UnityEngine.AI;

public class Map : MapUtil
{

    private void Awake()
    {
        InitializeMap();
        CreateGrid();
        PlayerRespawn();
        TrapRespawn();
        PatrolUnitRespawn();
        PatrolPointRespawn();
        ClearItemRespawn();
    }
    private void Start()
    {
        GameObject.Find("Floor").GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}