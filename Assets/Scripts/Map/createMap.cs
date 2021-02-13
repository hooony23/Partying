using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
using Util;


public class CreateMap : MonoBehaviour
{
    private MapInfo mapInfo;
    private MapObjects mapObjects;
    //column == 행, row == 열 
    public CreateMap(MapInfo mapInfo, int size) : this(mapInfo, size, size) { }
    public CreateMap(MapInfo mapInfo, int column, int row)
    {
        this.mapInfo = mapInfo;
        this.mapObjects = mapInfo.MapObjects;
        this.mapInfo.Columns = column;
        this.mapInfo.Rows = row;
        this.mapInfo.TestJson = File.ReadAllText("./Assets/Scripts/Map/temp.json");
        this.mapInfo.TestJson = @"{'type':'','data':" + this.mapInfo.TestJson + "}";
        this.mapInfo.TestJsonObject = JObject.Parse(this.mapInfo.TestJson);
        this.mapInfo.Data = (JObject)this.mapInfo.TestJsonObject["data"];
    }
    private const int LEFT = 0;
    private const int RIGHT = 1;
    private const int UP = 2;
    private const int DOWN = 3;

    public void CreateGrid()
    {
        //TODO wall -> left-right wall로 바꿀 것
        // 결과를 확인하기 위한 구문 
        // Dictionary<int, string> type = new Dictionary<int, string>() { { 0, "왼쪽" }, { 1, "오른쪽" }, { 2, "위" }, { 3, "아래" } };
        JArray labylinthJArray = this.mapInfo.Data.Value<JArray>("labylinthArray");
        // JArray Jpatrolpoint = data.Value<JArray>("patrolpoint");
        int[,,] labylinthArray = labylinthJArray.ToObject<int[,,]>();
        // int[,] patrolpoint = Jpatrolpoint.ToObject<int[,]>();
        Config.labylinthOnSpaceSize = this.mapObjects.wall.transform.localScale.x;
        //wall localScale = (10,5,1)
        this.mapInfo.Grid = new MazeCell[this.mapInfo.Rows, this.mapInfo.Columns]; //행과 열을 설정하여 미로를 위한 격자를 초기화함
        GameObject grandParent = GameObject.Find("Map");

        for (int i = 0; i < labylinthArray.GetLength(0); i++)
        {
            for (int j = 0; j < labylinthArray.GetLength(1); j++)
            {
                GameObject parent = new GameObject($"{i}_{j}");
                Debug.Log(this.mapObjects.wall.transform.localScale);
                parent.transform.position = new Vector3(j * Config.labylinthOnSpaceSize - this.mapObjects.wall.transform.localScale.z / 2, 1.01f, -i * Config.labylinthOnSpaceSize + this.mapObjects.wall.transform.localScale.z / 2);
                parent.transform.SetParent(grandParent.transform);
                this.mapInfo.Grid[i, j] = new MazeCell();// gird 격자를 초기화
                if (labylinthArray[i, j, LEFT] == 1)
                {
                    this.mapInfo.Grid[i, j].LeftWall = Instantiate(this.mapObjects.wall, new Vector3(parent.transform.position.x - Config.labylinthOnSpaceSize/2,3.0f, parent.transform.position.z - this.mapObjects.wall.transform.localScale.z / 2), Quaternion.Euler(0, 90, 0));
                    this.mapInfo.Grid[i, j].LeftWall.name = "leftWall";
                    this.mapInfo.Grid[i, j].LeftWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, RIGHT] == 1)
                {
                    this.mapInfo.Grid[i, j].RightWall = Instantiate(this.mapObjects.wall, new Vector3(parent.transform.position.x + Config.labylinthOnSpaceSize/2,3.0f, parent.transform.position.z - this.mapObjects.wall.transform.localScale.z / 2), Quaternion.Euler(0, 90, 0));
                    this.mapInfo.Grid[i, j].RightWall.name = "rightWall";
                    this.mapInfo.Grid[i, j].RightWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, UP] == 1)
                {

                    this.mapInfo.Grid[i, j].UpWall = Instantiate(this.mapObjects.UpDownWall, new Vector3(parent.transform.position.x,3.0f, parent.transform.position.z + Config.labylinthOnSpaceSize/2), Quaternion.identity);
                    this.mapInfo.Grid[i, j].UpWall.name = "UpWall";
                    this.mapInfo.Grid[i, j].UpWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, DOWN] == 1)
                {
                    this.mapInfo.Grid[i, j].DownWall = Instantiate(this.mapObjects.UpDownWall, new Vector3(parent.transform.position.x,3.0f, parent.transform.position.z - Config.labylinthOnSpaceSize/2), Quaternion.identity);
                    this.mapInfo.Grid[i, j].DownWall.name = "downWall";
                    this.mapInfo.Grid[i, j].DownWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                this.mapInfo.Grid[i, j].Respwan = Instantiate(this.mapObjects.MazePoint, parent.transform.position, Quaternion.identity); //플레이어,함정 리스폰을 위해 생성
                this.mapInfo.Grid[i, j].Respwan.name = "MazeRespawn";
                this.mapInfo.Grid[i, j].Respwan.transform.SetParent(parent.GetComponent<Transform>());

            }
        }
        this.mapInfo.IsMapdone = true;
    }
    public void TrapRespwan()
    { //함정생성
        JArray JtrapInfo = this.mapInfo.Data.Value<JArray>("trap");
        string[,] trapInfo = JtrapInfo.ToObject<string[,]>();
        GameObject Trap;
        for (int i = 0; i < this.mapInfo.Rows; i++)
        {
            for (int j = 0; j < this.mapInfo.Rows; j++)
            {

                switch (trapInfo[i, j])
                {
                    case "ranth":
                        Trap = Instantiate(this.mapObjects.SpikeTrap, this.mapInfo.Grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "SpikeTrap_" + i + "_" + j;
                        Trap.transform.parent = this.mapObjects.TrapPoint.transform;
                        break;
                    case "hole":
                        Trap = Instantiate(this.mapObjects.HoleTrap, this.mapInfo.Grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "HoleTrap_" + i + "_" + j;
                        Trap.transform.parent = this.mapObjects.TrapPoint.transform;
                        break;
                    case "slow":
                        break;
                    case "dange":
                        break;
                }

            }
        }
    }
}
