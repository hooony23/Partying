using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    private MapInfo mapInfo;
    private MapObjectInfo mapObjectInfo;
    //column == 행, row == 열 
    public CreateMap(MapInfo mapInfo, MapObjectInfo mapObjectInfo,int size) : this(mapInfo,mapObjectInfo,size,size) {}
    public CreateMap(MapInfo mapInfo, MapObjectInfo mapObjectInfo,int column, int row) {
        this.mapInfo = mapInfo;
        this.mapObjectInfo = mapObjectInfo;
        this.mapInfo.Columns = column;
        this.mapInfo.Rows = row;
        this.mapInfo.testJson = File.ReadAllText("./Assets/Scripts/Map/temp.json");
        this.mapInfo.testJson = @"{'type':'','data':" + this.mapInfo.testJson + "}";
        this.mapInfo.testJsonObject = JObject.Parse(this.mapInfo.testJson);
        this.mapInfo.data = (JObject)this.mapInfo.testJsonObject["data"];
    }
    private const int LEFT = 0;
    private const int RIGHT = 1;
    private const int UP = 2;
    private const int DOWN = 3;

    public void CreateGrid()
    {
        //TODO wall -> left-right wall로 바꿀 것
        // 결과를 확인하기 위한 구문 Dictionary<int, string> type = new Dictionary<int, string>() { { 0, "왼쪽" }, { 1, "오른쪽" }, { 2, "위" }, { 3, "아래" } };
        JArray labylinthJArray = this.mapInfo.data.Value<JArray>("labylinthArray");
        // JArray Jpatrolpoint = data.Value<JArray>("patrolpoint");
        int[,,] labylinthArray = labylinthJArray.ToObject<int[,,]>();
        // int[,] patrolpoint = Jpatrolpoint.ToObject<int[,]>();
        float size = this.mapObjectInfo.wall.transform.localScale.x;
        this.mapInfo.grid = new MazeCell[this.mapInfo.Rows, this.mapInfo.Columns]; //행과 열을 설정하여 미로를 위한 격자를 초기화함

        for (int i = 0; i < labylinthArray.GetLength(0); i++)
        {
            for (int j = 0; j < labylinthArray.GetLength(1); j++)
            {
                this.mapInfo.grid[i, j] = new MazeCell();// gird 격자를 초기화
                if (labylinthArray[i, j, LEFT] == 1)
                {
                    this.mapInfo.grid[i, j].LeftWall = Instantiate(this.mapObjectInfo.wall, new Vector3(j * size - 5.5f, 3f, -i * size), Quaternion.Euler(0, 90, 0));
                    this.mapInfo.grid[i, j].LeftWall.name = "leftWall_" + i + "_" + j;
                    this.mapInfo.grid[i, j].LeftWall.transform.parent = transform;
                }
                if (labylinthArray[i, j, RIGHT] == 1)
                {
                    this.mapInfo.grid[i, j].RightWall = Instantiate(this.mapObjectInfo.wall, new Vector3(j * size + 4.5f, 3f, -i * size), Quaternion.Euler(0, 90, 0));
                    this.mapInfo.grid[i, j].RightWall.name = "rightWall_" + i + "_" + j;
                    this.mapInfo.grid[i, j].RightWall.transform.parent = transform;
                }
                if (labylinthArray[i, j, UP] == 1)
                {
                    
                    this.mapInfo.grid[i, j].UpWall = Instantiate(this.mapObjectInfo.UpDownWall, new Vector3(j * size - 0.5f, 3f, -i * size + 5.5f), Quaternion.identity);
                    this.mapInfo.grid[i, j].UpWall.name = "UpWall_" + i + "_" + j;
                    this.mapInfo.grid[i, j].UpWall.transform.parent = transform;
                }
                if (labylinthArray[i, j, DOWN] == 1)
                {
                    this.mapInfo.grid[i, j].DownWall = Instantiate(this.mapObjectInfo.UpDownWall, new Vector3(j * size - 0.5f, 3f, -i * size - 4.5f), Quaternion.identity);
                    this.mapInfo.grid[i, j].DownWall.name = "downWall_" + i + "_" + j;
                    this.mapInfo.grid[i, j].DownWall.transform.parent = transform;
                }
                this.mapInfo.grid[i, j].Respwan = Instantiate(this.mapObjectInfo.MazePoint, new Vector3(j * size - 0.5f, 1.01f, -i * size), Quaternion.identity); //플레이어,함정 리스폰을 위해 생성
                this.mapInfo.grid[i, j].Respwan.name = "MazeRespawn_" + i + "_" + j;
                this.mapInfo.grid[i, j].Respwan.transform.parent = this.mapObjectInfo.MazeRespwan.transform;
                // if (patrolpoint[i, j] == 0) //Ai가 패트롤할 구간을 지정하기 위한 생성
                // {
                //     continue;
                // }
                // if (patrolpoint[i, j] == 1)
                // {
                //     grid[i, j].AiPoint = Instantiate(MazePoint, new Vector3(j * size - 0.5f, 10f, -i * size), Quaternion.identity);
                //     grid[i, j].AiPoint.name = "PatrolPoint_" + i + "_" + j;
                //     grid[i, j].AiPoint.transform.parent = PatrolPoint.transform;
                // }
            }
        }
        this.mapInfo.isMapdone = true;
    }
    public void TrapRespwan() { //함정생성
        JArray JtrapInfo = this.mapInfo.data.Value<JArray>("trap");
        string[,] trapInfo = JtrapInfo.ToObject<string[,]>();
        GameObject Trap;
        for (int i = 0; i < this.mapInfo.Rows; i++) {
            for (int j = 0; j < this.mapInfo.Rows; j++) {
                
                switch (trapInfo[i, j])
                {
                    case "slow":
                        break;
                    case "ranth":
                        Trap = Instantiate(this.mapObjectInfo.SpikeTrap, this.mapInfo.grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "SpikeTrap_" + i + "_" + j;
                        Trap.transform.parent = this.mapObjectInfo.TrapPoint.transform;
                        break;
                    case "hole":
                        Trap = Instantiate(this.mapObjectInfo.HoleTrap, this.mapInfo.grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "HoleTrap_" + i + "_" + j;
                        Trap.transform.parent = this.mapObjectInfo.TrapPoint.transform;
                        break;
                    case "dange":
                        break;
                }
                
            }
        }
    }
}
