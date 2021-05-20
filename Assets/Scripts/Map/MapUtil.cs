
using UnityEngine;
using UnityEngine.AI;
using Communication;
using Util;

public class MapUtil : MapController
{
    protected void InitializeMap()
    {
        MInfo = NetworkInfo.mapInfo;
        MapObjects = new MapObjects();
        MapObjects.wall = Resources.Load("Labyrinth/Map/Wall") as GameObject; // 왼쪽 오른쪽 벽
        MapObjects.UpDownWall = Resources.Load("Labyrinth/Map/UpDownWall") as GameObject;// 위 아래 벽
       
        MapObjects.PatrolPoint = Resources.Load("Labyrinth/Map/PatrolPoint") as GameObject;
        MapObjects.MapClearItem = Resources.Load("Labyrinth/Map/MapClearItem") as GameObject;
        MapObjects.SpikeTrap = Resources.Load("Labyrinth/Trap/SpikeTrap") as GameObject;// 가시함정
        MapObjects.HoleTrap = Resources.Load("Labyrinth/Trap/HoleTrap") as GameObject;// 바닥함정
        MapObjects.SlowTrap = Resources.Load("Labyrinth/Trap/SlowTrap") as GameObject;// 슬로우함정
        MapObjects.DangerZone = Resources.Load("Labyrinth/Trap/DangerZone") as GameObject;// 위험 지역
        MapObjects.PatrolUnit = Resources.Load("Labyrinth/Patrol/Patrol") as GameObject;// AI

        MapObjects.AIs = new GameObject("AIs");// AI 집합
        MapObjects.AIPoint = new GameObject("AIPoint");// 오브젝트 리스폰 집합
        MapObjects.MazePoint = new GameObject("MazePoint");// 유닛 오브젝트 리스폰 집합
        MapObjects.TrapPoint = new GameObject("TrapPoint");// 함정오브젝트 리스폰 집합
        MapObjects.MazeRespwan = new GameObject("MazeRespwan");// AI 이동 지점
        MapObjects.MazeBake = new GameObject("MazeBake");// Bake 생성
        MapObjects.MazeBake.AddComponent<NavMeshSurface>();
        MapObjects.AIs.transform.SetParent(this.transform);
        MapObjects.AIPoint.transform.SetParent(this.transform);
        MapObjects.MazePoint.transform.SetParent(this.transform);
        MapObjects.TrapPoint.transform.SetParent(this.transform);
        MapObjects.MazeRespwan.transform.SetParent(this.transform);
        MapObjects.MazeBake.transform.SetParent(this.transform);
    }

    public int[,,] GridConverToIntegerArray(string[] grid)
    {
        int[,,] parseArray = new int[grid.Length,grid[0].Length,4];
        for(int row=0;row<grid.GetLength(0);row++)
            for(int col=0;col<grid[0].Length;col++)
            {
                parseArray[row,col,0]=(System.Convert.ToInt32($"{grid[row][col]}",16)&8)>>3;
                parseArray[row,col,1]=(System.Convert.ToInt32($"{grid[row][col]}",16)&4)>>2;
                parseArray[row,col,2]=(System.Convert.ToInt32($"{grid[row][col]}",16)&2)>>1;
                parseArray[row,col,3]=(System.Convert.ToInt32($"{grid[row][col]}",16)&1);
            }
        return parseArray;
    }
    public void CreateGrid()
    {
        //TODO wall -> left-right wall로 바꿀 것
        // 결과를 확인하기 위한 구문 
        // Dictionary<int, string> type = new Dictionary<int, string>() { { 0, "왼쪽" }, { 1, "오른쪽" }, { 2, "위" }, { 3, "아래" } };
        // JArray Jpatrolpoint = data.Value<JArray>("patrolpoint");
        int[,,] labylinthArray = GridConverToIntegerArray(MInfo.labylinthArray);
        // int[,] patrolpoint = Jpatrolpoint.ToObject<int[,]>();
        var labylinthOnSpaceSize = this.MapObjects.wall.transform.localScale.x;
        //wall localScale = (10,5,1)
        Grid = new MazeCell[labylinthArray.GetLength(0), labylinthArray.GetLength(1)]; //행과 열을 설정하여 미로를 위한 격자를 초기화함
        GameObject grandParent = this.gameObject;

        for (int i = 0; i < labylinthArray.GetLength(0); i++)
        {
            for (int j = 0; j < labylinthArray.GetLength(1); j++)
            {
                GameObject parent = new GameObject($"{i}_{j}");
                parent.transform.position = new Vector3(j * labylinthOnSpaceSize - this.MapObjects.wall.transform.localScale.z / 2, 1.01f, -i * labylinthOnSpaceSize + this.MapObjects.wall.transform.localScale.z / 2);
                parent.transform.SetParent(grandParent.transform);
                Grid[i, j] = new MazeCell();// grid 격자를 초기화
                if (labylinthArray[i, j, (int)Direction.LEFT] == 1)
                {
                    Grid[i, j].LeftWall = Instantiate(this.MapObjects.wall, new Vector3(parent.transform.position.x - labylinthOnSpaceSize / 2, 6.0f, parent.transform.position.z - this.MapObjects.wall.transform.localScale.z / 2 +0.5f), Quaternion.Euler(0, 90, 0));
                    Grid[i, j].LeftWall.name = "leftWall";
                    Grid[i, j].LeftWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, (int)Direction.RIGHT] == 1)
                {
                    Grid[i, j].RightWall = Instantiate(this.MapObjects.wall, new Vector3(parent.transform.position.x + labylinthOnSpaceSize / 2, 6.0f, parent.transform.position.z - this.MapObjects.wall.transform.localScale.z / 2 + 0.5f), Quaternion.Euler(0, 90, 0));
                    Grid[i, j].RightWall.name = "rightWall";
                    Grid[i, j].RightWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, (int)Direction.UP] == 1)
                {

                    Grid[i, j].UpWall = Instantiate(this.MapObjects.UpDownWall, new Vector3(parent.transform.position.x, 6.0f, parent.transform.position.z + labylinthOnSpaceSize / 2), Quaternion.identity);
                    Grid[i, j].UpWall.name = "UpWall";
                    Grid[i, j].UpWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, (int)Direction.DOWN] == 1)
                {
                    Grid[i, j].DownWall = Instantiate(this.MapObjects.UpDownWall, new Vector3(parent.transform.position.x, 6.0f, parent.transform.position.z - labylinthOnSpaceSize / 2), Quaternion.identity);
                    Grid[i, j].DownWall.name = "downWall";
                    Grid[i, j].DownWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                Grid[i, j].Respwan = Instantiate(this.MapObjects.MazePoint, parent.transform.position, Quaternion.identity); //플레이어,함정 리스폰을 위해 생성
                Grid[i, j].Respwan.name = "MazeRespawn";
                Grid[i, j].Respwan.transform.SetParent(parent.GetComponent<Transform>());

            }
        }
        IsMapdone = true;
    }
    public void TrapRespawn()
    { //함정생성
        CellInfo[] trapInfo = MInfo.trap;

        GameObject grandParent = this.gameObject;
        foreach (CellInfo item in trapInfo)
        {
            GameObject Trap;
            switch ((string)item.data)
            {
                case "spike":
                    Trap = Instantiate(MapObjects.SpikeTrap, Grid[item.col, item.row].Respwan.transform.position, Quaternion.identity);
                    Trap.name = "SpikeTrap";
                    break;
                case "hole":
                    Trap = Instantiate(MapObjects.HoleTrap, Grid[item.col, item.row].Respwan.transform.position, Quaternion.identity);
                    Trap.name = "HoleTrap";
                    break;
                case "slow":
                    Trap = Instantiate(MapObjects.SlowTrap, Grid[item.col, item.row].Respwan.transform.position, Quaternion.identity);
                    Trap.name = "SlowTrap";
                    break;
                case "danger":
                    Trap = Instantiate(MapObjects.DangerZone, Grid[item.col, item.row].Respwan.transform.position, Quaternion.identity);
                    Trap.name = "DangerZone";
                    break;
                default:
                    Trap = null;
                    break;
            }
            if (Trap == null)
                continue;
            Trap.transform.SetParent(grandParent.transform.Find($"{item.col}_{item.row}"));
        }
    }
    public void ClearItemRespawn()
    {
        Division2 clearLoc = MInfo.clearItem;
        GameObject mapClearItem = Instantiate(MapObjects.MapClearItem, Grid[(int)clearLoc.X,(int)clearLoc.Y].Respwan.transform.position, Quaternion.identity);
        mapClearItem.name = Resources.Load("Labyrinth/Map/MapClearItem").name;

    }

    public void PlayerRespawn()
    {
        CellInfo[] playerInfo = MInfo.playerLocs;
        foreach (CellInfo item in playerInfo)
        {
            GameObject player = null;
            if(Config.userUuid.Equals(item.data.ToString()))
                player = Instantiate(Resources.Load("Player/Player") as GameObject, Grid[item.col, item.row].Respwan.transform.position, Quaternion.identity);
            else
                player = Instantiate(Resources.Load("Player/OtherPlayer") as GameObject, Grid[item.col, item.row].Respwan.transform.position, Quaternion.identity);
            player.name = item.data.ToString();
            player.GetComponent<Player>().UserUuid=player.name;
        }
    } 
    public void PatrolUnitRespawn()
    { //함정생성
        CellInfo[] patrolPintsInfo = MInfo.patrolUnits;
        GameObject grandParent =  this.gameObject;
        foreach(CellInfo item in patrolPintsInfo)
        {
            GameObject patrolUnit =  Instantiate(MapObjects.PatrolUnit, Grid[item.col, item.row].Respwan.transform.position, Quaternion.identity);
            patrolUnit.name = item.data.ToString();
            patrolUnit.transform.SetParent(grandParent.transform.Find("AIs"));
        }
    }

    public void PatrolPointRespawn()
    { //함정생성
        CellInfo[] patrolPintsInfo = MInfo.patrolPoints;

        GameObject grandParent =  this.gameObject;
        foreach(CellInfo item in patrolPintsInfo)
        {
                GameObject patrolPoint =  Instantiate(MapObjects.PatrolPoint, Grid[item.col, item.row].Respwan.transform.position, Quaternion.identity);
                patrolPoint.name = item.data.ToString();
                patrolPoint.transform.SetParent(grandParent.transform.Find($"{item.col}_{item.row}"));
        }
    }
}