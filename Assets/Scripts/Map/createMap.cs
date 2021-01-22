using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using JetBrains.Annotations;


public class createMap : MonoBehaviour
{

    public MazeCell[,] grid; //미로를 만들기 위한 격자 생성
    public int Rows; // 행에 대한 미로찾기를 위한 처음의 시작값
    public int Columns; // 열에 대한 미로찾기를 위한 처음의 시작값
    private string testJson;

    public GameObject wall; // 벽의 프리팹을 참조하도록 하는 줄
    public GameObject UpDownWall; // 벽의 프리팹을 참조하도록 하는 줄
    public GameObject MazePoint; // 오브젝트 리스폰 확인 오브젝트
    public GameObject MazeRespwan;// 유닛 오브젝트 리스폰 지점확인 오브젝트
    public GameObject TrapPoint;// 함정오브젝트 리스폰 확인 오브젝트
    public GameObject PatrolPoint;
    public GameObject SpikeTrap;// 가시함정 오브젝트
    public GameObject HoleTrap;// 바닥함정 오브젝트


    JObject testJsonObject;
    JObject data;
    public NavMeshSurface[] surfaces;
    Program startmaptest = new Program();

    private bool isMapdone = false;

    private void Start()
    {
        //AsynchronousClient.Connected();
        basicSetting();
        createGrid();
    }
    void Update() // Bake를 최초 갱신하기 위함
    {
        if (isMapdone.Equals(true))
        {
            for (int i = 0; i < surfaces.Length; i++)
            {
                surfaces[i].BuildNavMesh();
            }
            TrapRespwan();
            isMapdone = false;
        }
    }

    /*private void OnApplicationQuit()
    {

        // AsynchronousClient.ConnectedExit(); //서버 연결 해제 
        }*/

    void basicSetting() {
        Rows = config.ROW;
        Columns = config.COL;
        testJson = startmaptest.CreateLabylinth(Rows); //데이터 테스트용 값삽입
        testJson = @"{'type':'','data':" + testJson + "}";
        testJsonObject = JObject.Parse(testJson);
        data = (JObject)testJsonObject["data"];
    }
    void createGrid()
    {
        // 결과를 확인하기 위한 구문 Dictionary<int, string> type = new Dictionary<int, string>() { { 0, "왼쪽" }, { 1, "오른쪽" }, { 2, "위" }, { 3, "아래" } };
        JArray labylinthJArray = data.Value<JArray>("labylinthArray");
        JArray Jpatrolpoint = data.Value<JArray>("patrolpoint");
        int[,,] labylinthArray = labylinthJArray.ToObject<int[,,]>();
        int[,] patrolpoint = Jpatrolpoint.ToObject<int[,]>();
        float size = wall.transform.localScale.x;
        grid = new MazeCell[Rows, Columns]; //행과 열을 설정하여 미로를 위한 격자를 초기화함

        for (int i = 0; i < labylinthArray.GetLength(0); i++)
        {
            for (int j = 0; j < labylinthArray.GetLength(1); j++)
            {
                grid[i, j] = new MazeCell();// gird 격자를 초기화
                for (int k = 0; k < labylinthArray.GetLength(2); k++)
                {
                    if (labylinthArray[i, j, k] == 0)
                    {
                        continue;
                    }
                    else if (labylinthArray[i, j, k] == 1)
                    {
                        switch (k) {
                            case 0:
                                grid[i, j].LeftWall = Instantiate(wall, new Vector3(j * size - 5.5f, 3f, -i * size), Quaternion.Euler(0, 90, 0));
                                grid[i, j].LeftWall.name = "leftWall_" + i + "_" + j;
                                grid[i, j].LeftWall.transform.parent = transform;
                                break;
                            case 1:
                                grid[i, j].RightWall = Instantiate(wall, new Vector3(j * size + 4.5f, 3f, -i * size), Quaternion.Euler(0, 90, 0));
                                grid[i, j].RightWall.name = "rightWall_" + i + "_" + j;
                                grid[i, j].RightWall.transform.parent = transform;
                                break;
                            case 2:
                                grid[i, j].UpWall = Instantiate(UpDownWall, new Vector3(j * size - 0.5f, 3f, -i * size + 5.5f), Quaternion.identity);
                                grid[i, j].UpWall.name = "UpWall_" + i + "_" + j;
                                grid[i, j].UpWall.transform.parent = transform;
                                break;
                            case 3:
                                grid[i, j].DownWall = Instantiate(UpDownWall, new Vector3(j * size - 0.5f, 3f, -i * size - 4.5f), Quaternion.identity);
                                grid[i, j].DownWall.name = "downWall_" + i + "_" + j;
                                grid[i, j].DownWall.transform.parent = transform;
                                break;
                        }
                    }
                }
                grid[i, j].Respwan = Instantiate(MazePoint, new Vector3(j * size - 0.5f, 1.01f, -i * size), Quaternion.identity); //플레이어,함정 리스폰을 위해 생성
                grid[i, j].Respwan.name = "MazeRespawn_" + i + "_" + j;
                grid[i, j].Respwan.transform.parent = MazeRespwan.transform;
                if (patrolpoint[i, j] == 0) //Ai가 패트롤할 구간을 지정하기 위한 생성
                {
                    continue;
                }
                if (patrolpoint[i, j] == 1)
                {
                    grid[i, j].AiPoint = Instantiate(MazePoint, new Vector3(j * size - 0.5f, 10f, -i * size), Quaternion.identity);
                    grid[i, j].AiPoint.name = "PatrolPoint_" + i + "_" + j;
                    grid[i, j].AiPoint.transform.parent = PatrolPoint.transform;
                }
            }
        }
        isMapdone = true;
    }
    void TrapRespwan() { //함정생성
        JArray JtrapInfo = data.Value<JArray>("trap");
        string[,] trapInfo = JtrapInfo.ToObject<string[,]>();
        GameObject Trap;
        for (int i = 0; i < Rows; i++) {
            for (int j = 0; j < Rows; j++) {
                
                switch (trapInfo[i, j])
                {
                    case "slow":
                        break;
                    case "ranth":
                        Trap = Instantiate(SpikeTrap, grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "SpikeTrap_" + i + "_" + j;
                        Trap.transform.parent = TrapPoint.transform;
                        break;
                    case "hole":
                        Trap = Instantiate(HoleTrap, grid[i, j].Respwan.transform.position, Quaternion.identity);
                        Trap.name = "HoleTrap_" + i + "_" + j;
                        Trap.transform.parent = TrapPoint.transform;
                        break;
                    case "dange":
                        break;
                }
                
            }
        }
    }
}
public class Program
{
    public class MapInfo
    {
        public int[,,] wallInfo;
        public int[,] patrolpointInfo;
        public string[,] trapInfo;
        public int clearItemLocation;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(new { labylinthArray = wallInfo, patrolpoint = patrolpointInfo, trap = trapInfo }); //, clearItem = clearItemLocation 
        }
    }
    public static string randomTrap(int num)
    {
        string result = "None";
        switch (num)
        {
            case 1:
                result = "ranth";
                break;
            case 2:
                result = "hole";
                break;
            case 3:
                result = "slow";
                break;
            case 4:
                result = "dange";
                break;
        }
        return result;
    }
    public string CreateLabylinth(int size)
    {
        MapInfo mapInfo = new MapInfo();
        System.Random r = new System.Random();
        //mapInfo.clearItemLocation = new Vector3(r.Next(0, size), 3, r.Next(0, size));
        mapInfo.wallInfo = new int[size, size, 4];
        mapInfo.patrolpointInfo = new int[size, size];
        mapInfo.trapInfo = new string[size, size];
        for (int i = 0; i < mapInfo.wallInfo.GetLength(0); i++)
        {
            for (int j = 0; j < mapInfo.wallInfo.GetLength(1); j++)
            {

                for (int k = 0; k < mapInfo.wallInfo.GetLength(2); k++)
                {
                    mapInfo.wallInfo[i, j, k] = r.Next(0, 2);

                }
                mapInfo.trapInfo[i, j] = randomTrap(r.Next(1, 5));
                //Debug.Log(mapInfo.trapInfo[i, j]);
                mapInfo.patrolpointInfo[i, j] = r.Next(0, 2);
            }

        }
        return mapInfo.ToString();
    }
   
}

