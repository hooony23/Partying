using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Maze : MonoBehaviour
{
    public int Rows = 2; //배열의 행에 해당함
    public int Columns = 2; // 배열의 열에 해당함
    public GameObject wall; // 벽의 프리팹을 참조하도록 하는 줄
    public GameObject UpDownWall; // 벽의 프리팹을 참조하도록 하는 줄
    public NavMeshSurface[] surfaces;

    private MazeCell[,] grid; //미로를 만들기 위한 격자 생성
    private int currentRow = 0; // 행에 대한 미로찾기를 위한 처음의 시작값
    private int currentColumns = 0; // 열에 대한 미로찾기를 위한 처음의 시작값
    private bool scanComplete = false;
    private bool Completemap = false;

    void Start()
    {
        CreateGrid();
        HuntAndKill();

    }
    void Update() // Bake를 최초 갱신하기 위함
    {
        if (Completemap == true)
        {

            for (int i = 0; i < surfaces.Length; i++)
            {
                surfaces[i].BuildNavMesh();
            }
            Completemap = false;
        }
    }
    void CreateGrid() // 그리드를 쉽게 호출하기 위해 함수로 정의
    {
        float size = wall.transform.localScale.x;
        grid = new MazeCell[Rows, Columns]; //행과 열을 설정하여 미로를 위한 격자를 초기화함
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                grid[i, j] = new MazeCell();// gird 격자를 초기화
                if (i == 0)
                {
                    grid[i, j].UpWall = Instantiate(UpDownWall, new Vector3(j * size - 0.5f, 3f, -i * size + 5.5f), Quaternion.identity);
                    grid[i, j].UpWall.name = "UpWall_" + i + "_" + j;
                    grid[i, j].UpWall.transform.parent = transform;
                }
                grid[i, j].DownWall = Instantiate(UpDownWall, new Vector3(j * size - 0.5f, 3f, -i * size - 4.5f), Quaternion.identity);
                grid[i, j].DownWall.name = "downWall_" + i + "_" + j;
                if (j == 0)
                {
                    grid[i, j].LeftWall = Instantiate(wall, new Vector3(j * size - 5.5f, 3f, -i * size), Quaternion.Euler(0, 90, 0));
                    grid[i, j].LeftWall.name = "leftWall_" + i + "_" + j;
                    grid[i, j].LeftWall.transform.parent = transform;
                }
                grid[i, j].RightWall = Instantiate(wall, new Vector3(j * size + 4.5f, 3f, -i * size), Quaternion.Euler(0, 90, 0));
                grid[i, j].RightWall.name = "rightWall_" + i + "_" + j;
                grid[i, j].DownWall.transform.parent = transform;
                grid[i, j].RightWall.transform.parent = transform;
            }
        }
    }
    bool AreThereUnvisitedNeighbors()
    {
        if (IsCellUnvisitredAndWithBoundaries(currentRow - 1, currentColumns)) //위쪽방향에 대한 이웃을 방문했는지 확인
        {
            return true;
        }

        if (IsCellUnvisitredAndWithBoundaries(currentRow + 1, currentColumns)) //아래방향에 대한 이웃을 방문했는지 확인
        {
            return true;
        }

        if (IsCellUnvisitredAndWithBoundaries(currentRow, currentColumns + 1)) //오른방향에 대한 이웃을 방문했는지 확인
        {
            return true;
        }

        if (IsCellUnvisitredAndWithBoundaries(currentRow, currentColumns - 1)) //왼방향에 대한 이웃을 방문했는지 확인
        {
            return true;
        }
        return false;
    }

    public bool AreThereVisitedNeighbors(int row, int column)
    {
        if (row > 0 && grid[row - 1, column].Visited) //위쪽방문확인
        {
            return true;
        }
        if (row < Rows - 1 && grid[row + 1, column].Visited) //아래쪽방문확인
        {
            return true;
        }
        if (column > 0 && grid[row, column - 1].Visited) //왼쪽방문확인
        {
            return true;
        }
        if (column > Columns && grid[row, column + 1].Visited) //오른쪽확인
        {
            return true;
        }
        return false;
    }

    void HuntAndKill()
    {
        grid[currentRow, currentColumns].Visited = true; // 현재 행과 열에 대해 방문한 grid를 설정한다. / 랜덤한 방향으로 나아가기 위한 첫번째 cell을 방문한것으로 처리한다.
        while (!scanComplete) // Walk를 통한 길찾기와 Hunt를 통한 새로운 길 생성
        {
            Walk();
            Hunt();
        }
        Completemap = true;
    }
    void Walk()
    {
        while (AreThereUnvisitedNeighbors()) //이웃방문여부를 확인하며 최초의 길을 찾음
        {
            int direction = Random.Range(0, 4); //0~4의 범위인 1,2,3을 제공하여 랜덤으로 돌린다. / 임의의 방향으로 진행하는 것을 말한다.

            // 위쪽방향 확인
            if (direction == 0)
            {
                if (IsCellUnvisitredAndWithBoundaries(currentRow - 1, currentColumns)) //이전행의 방문여부 확인
                {
                    if (grid[currentRow, currentColumns].UpWall) // 위쪽벽을 확인후 위쪽벽을 부순다.
                    {
                        Destroy(grid[currentRow, currentColumns].UpWall);
                    }

                    currentRow--; //이전행의 중복 벽을 제거하기 위함
                    grid[currentRow, currentColumns].Visited = true;

                    if (grid[currentRow, currentColumns].DownWall) //아래벽 확인후 아래쪽 벽을 부순다
                    {
                        Destroy(grid[currentRow, currentColumns].DownWall);
                    }

                }
            }
            //아래방향 확인
            else if (direction == 1)
            {
                if (IsCellUnvisitredAndWithBoundaries(currentRow + 1, currentColumns)) 
                {
                    if (grid[currentRow, currentColumns].DownWall) // 아래벽 확인후 아래쪽 벽을 부순다
                    {
                        Destroy(grid[currentRow, currentColumns].DownWall);

                    }

                    currentRow++; //다음행의 중복 벽을 제거하기 위함
                    grid[currentRow, currentColumns].Visited = true;

                    if (grid[currentRow, currentColumns].UpWall) // 위쪽벽을 확인후 위쪽벽을 부순다.
                    {
                        Destroy(grid[currentRow, currentColumns].UpWall);

                    }
                }
            }
            //왼쪽방향 확인
            else if (direction == 2)
            {
                if (IsCellUnvisitredAndWithBoundaries(currentRow, currentColumns - 1)) 
                {
                    if (grid[currentRow, currentColumns].LeftWall) // 왼쪽벽을 확인하기 위해 왼쪽벽을 부순다.
                    {
                        Destroy(grid[currentRow, currentColumns].LeftWall);
                    }

                    currentColumns--; //이전열의 중복벽 제거를 위함
                    grid[currentRow, currentColumns].Visited = true;

                    if (grid[currentRow, currentColumns].RightWall) // 만약 왼쪽벽이 없다면 오른쪽벽을 부순다
                    {
                        Destroy(grid[currentRow, currentColumns].RightWall);
                    }
                }

            }
            //오른쪽방향 확인
            else if (direction == 3)
            {
                if (IsCellUnvisitredAndWithBoundaries(currentRow, currentColumns + 1)) 
                {
                    if (grid[currentRow, currentColumns].RightWall) // 오른쪽벽을 확인하기 위해 오른쪽벽을 부순다.
                    {
                        Destroy(grid[currentRow, currentColumns].RightWall);
                    }


                    currentColumns++;//다음열의 중복벽 제거를 위함
                    grid[currentRow, currentColumns].Visited = true;

                    if (grid[currentRow, currentColumns].LeftWall) // 만약 오른쪽벽이 없다면 왼쪽벽을 부순다
                    {
                        Destroy(grid[currentRow, currentColumns].LeftWall);
                    }

                }
            }
        }
    }
    void Hunt() //방문하지 않은 길을 찾기 위함
    {
        scanComplete = true;
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (!grid[i, j].Visited && AreThereVisitedNeighbors(i, j)) //해당 배열의 방문여부와 이웃방문여부를 확인후 진행
                {
                    scanComplete = false;
                    currentRow = i;
                    currentColumns = j;
                    grid[currentRow, currentColumns].Visited = true;
                    DestroyAdjacentWall();
                    return;
                }
            }
        }
    }
    void DestroyAdjacentWall() //벽제거(사냥)을 위한 함수
    {
        bool destroyed = false;

        while (!destroyed)
        {
            int direction = Random.Range(0, 4);

            if (direction == 0) //위쪽확인
            {
                if (currentRow > 0 && grid[currentRow - 1, currentColumns].Visited) // 현재 행이 0보다 크고, 배열의 현재 행보다 이전행을 방문했는지 확인
                {
                    if (grid[currentRow, currentColumns].UpWall) // 현재 위치의 위쪽벽 존재시
                    {
                        Destroy(grid[currentRow, currentColumns].UpWall); // 그 위쪽벽을 부순다.

                    }
                    if (grid[currentRow - 1, currentColumns].DownWall) //중복벽 제거를 위한 이전행 아래벽확인
                    {
                        Destroy(grid[currentRow - 1, currentColumns].DownWall); // 아래쪽 벽을 부순다.

                    }

                    destroyed = true;
                }
            }
            else if (direction == 1) //아래쪽 확인
            {
                if (currentRow < Rows - 1 && grid[currentRow + 1, currentColumns].Visited)
                {
                    if (grid[currentRow, currentColumns].DownWall)
                    {

                        Destroy(grid[currentRow, currentColumns].DownWall);

                    }
                    if (grid[currentRow + 1, currentColumns].UpWall)
                    {

                        Destroy(grid[currentRow + 1, currentColumns].UpWall);
                    }


                    destroyed = true;
                }
            }
            else if (direction == 2) //왼쪽확인
            {
                if (currentColumns > 0 && grid[currentRow, currentColumns - 1].Visited)
                {
                    if (grid[currentRow, currentColumns].LeftWall)
                    {

                        Destroy(grid[currentRow, currentColumns].LeftWall);
                    }
                    if (grid[currentRow, currentColumns - 1].RightWall)
                    {
                        Destroy(grid[currentRow, currentColumns - 1].RightWall);
                    }


                    destroyed = true;
                }
            }
            else if (direction == 3) //오른쪽 확인
            {
                if (currentColumns < Columns - 1 && grid[currentRow, currentColumns + 1].Visited)
                {
                    if (grid[currentRow, currentColumns].RightWall)
                    {
                        Destroy(grid[currentRow, currentColumns].RightWall);
                    }
                    if (grid[currentRow, currentColumns + 1].LeftWall)
                    {
                        Destroy(grid[currentRow, currentColumns + 1].LeftWall);
                    }

                    destroyed = true;
                }
            }
        }

    }
    bool IsCellUnvisitredAndWithBoundaries(int row, int column)
    {// 경계선에 방문안했는지 확인
        if (row > 0 && row < Rows && column >= 0 && column < Columns && !grid[row, column].Visited)
        {
            return true;
        }
        return false;
    }

}
