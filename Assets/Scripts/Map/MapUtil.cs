
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Util;

public class MapUtil : MapController
{

    public void CreateGrid(int size)
    {
        CreateGrid(size,size);
    }
    public void CreateGrid(int Rows, int Columns)
    {
        //TODO wall -> left-right wall로 바꿀 것
        // 결과를 확인하기 위한 구문 
        // Dictionary<int, string> type = new Dictionary<int, string>() { { 0, "왼쪽" }, { 1, "오른쪽" }, { 2, "위" }, { 3, "아래" } };
        // JArray Jpatrolpoint = data.Value<JArray>("patrolpoint");
        int[,,] labylinthArray = MapInfo.LabylinthArray;
        // int[,] patrolpoint = Jpatrolpoint.ToObject<int[,]>();
        Config.labylinthOnSpaceSize = this.MapObjects.wall.transform.localScale.x;
        //wall localScale = (10,5,1)
        Grid = new MazeCell[Rows, Columns]; //행과 열을 설정하여 미로를 위한 격자를 초기화함
        GameObject grandParent = GameObject.Find("Map");

        for (int i = 0; i < labylinthArray.GetLength(0); i++)
        {
            for (int j = 0; j < labylinthArray.GetLength(1); j++)
            {
                GameObject parent = new GameObject($"{i}_{j}");
                parent.transform.position = new Vector3(j * Config.labylinthOnSpaceSize - this.MapObjects.wall.transform.localScale.z / 2, 1.01f, -i * Config.labylinthOnSpaceSize + this.MapObjects.wall.transform.localScale.z / 2);
                parent.transform.SetParent(grandParent.transform);
                Grid[i, j] = new MazeCell();// gird 격자를 초기화
                if (labylinthArray[i, j, (int)Direction.LEFT] == 1)
                {
                    Grid[i, j].LeftWall = Instantiate(this.MapObjects.wall, new Vector3(parent.transform.position.x - Config.labylinthOnSpaceSize / 2, 3.0f, parent.transform.position.z - this.MapObjects.wall.transform.localScale.z / 2), Quaternion.Euler(0, 90, 0));
                    Grid[i, j].LeftWall.name = "leftWall";
                    Grid[i, j].LeftWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, (int)Direction.RIGHT] == 1)
                {
                    Grid[i, j].RightWall = Instantiate(this.MapObjects.wall, new Vector3(parent.transform.position.x + Config.labylinthOnSpaceSize / 2, 3.0f, parent.transform.position.z - this.MapObjects.wall.transform.localScale.z / 2), Quaternion.Euler(0, 90, 0));
                    Grid[i, j].RightWall.name = "rightWall";
                    Grid[i, j].RightWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, (int)Direction.UP] == 1)
                {

                    Grid[i, j].UpWall = Instantiate(this.MapObjects.UpDownWall, new Vector3(parent.transform.position.x, 3.0f, parent.transform.position.z + Config.labylinthOnSpaceSize / 2), Quaternion.identity);
                    Grid[i, j].UpWall.name = "UpWall";
                    Grid[i, j].UpWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                if (labylinthArray[i, j, (int)Direction.DOWN] == 1)
                {
                    Grid[i, j].DownWall = Instantiate(this.MapObjects.UpDownWall, new Vector3(parent.transform.position.x, 3.0f, parent.transform.position.z - Config.labylinthOnSpaceSize / 2), Quaternion.identity);
                    Grid[i, j].DownWall.name = "downWall";
                    Grid[i, j].DownWall.transform.SetParent(parent.GetComponent<Transform>());
                }
                Grid[i, j].Respwan = Instantiate(this.MapObjects.MazePoint, parent.transform.position, Quaternion.identity); //플레이어,함정 리스폰을 위해 생성
                Grid[i, j].Respwan.name = "MazeRespawn";
                Grid[i, j].Respwan.transform.SetParent(parent.GetComponent<Transform>());

            }
        }
        IsMapdone = true;
        TrapRespawn(Rows,Columns);
    }
    public void TrapRespawn(int Rows,int Columns)
    { //함정생성
        string[,] trapInfo = MapInfo.Trap;
        
        GameObject grandParent = GameObject.Find("Map");
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Rows; j++)
            {
                GameObject Trap;
                switch (trapInfo[i, j])
                {
                    case "ranth":
                        Trap = Instantiate(MapObjects.SpikeTrap, Grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "SpikeTrap";
                        Trap.transform.parent = MapObjects.TrapPoint.transform;
                        break;
                    case "hole":
                        Trap = Instantiate(MapObjects.HoleTrap, Grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "HoleTrap";
                        Trap.transform.parent = MapObjects.TrapPoint.transform;
                        break;
                    case "slow":
                        Trap = Instantiate(MapObjects.SlowTrap, Grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "SlowTrap";
                        Trap.transform.parent = MapObjects.TrapPoint.transform;
                        break;
                    case "dange":
                        Trap = Instantiate(MapObjects.DangerZone, Grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "DangerZone";
                        Trap.transform.parent = MapObjects.TrapPoint.transform;
                        break;
                    default:
                        Trap = null;
                        break;
                }
                if (Trap == null)
                    continue;
                Trap.transform.SetParent(grandParent.transform.Find($"{i}_{j}"));
            }
        }
    }
}