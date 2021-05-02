using UnityEngine;
using UnityEngine.AI;
public class MapObjects
{
    // 프리팹을 참조
    public GameObject wall; // 왼쪽 오른쪽 벽
    public GameObject UpDownWall; // 위 아래 벽
    public GameObject SpikeTrap;// 가시함정
    public GameObject HoleTrap;// 바닥함정
    public GameObject SlowTrap;
    public GameObject MapClearItem;
    public GameObject DangerZone;

    public GameObject PatrolUnit;

    // 생성자에서 생성
    public GameObject AIs;
    public GameObject MazePoint; // 오브젝트 리스폰 확인 오브젝트
    public GameObject MazeRespwan;// 유닛 오브젝트 리스폰 지점확인 오브젝트
    public GameObject TrapPoint;// 함정오브젝트 리스폰 확인 오브젝트
    public GameObject PatrolPoint; // AI 이동 지점
    public GameObject AIPoint;  // AI 리스혼 확인 오브젝트
    public GameObject MazeBake; // Bake 생성
}