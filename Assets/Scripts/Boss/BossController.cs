using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Util;

public class BossController : MonoBehaviour
{
    private ParticleSystem chargingL, octaL;
    private Animator animator;

    private float radarRange = Config.radarRange;
    private float bossHP = Config.bossHP;

    private LayerMask playerMask = 0;
    private List<Transform> playersList = new List<Transform>(); // 추후 UUID를 담을 리스트로 활용예정
    private List<Transform> targetList = new List<Transform>();

    private NavMeshAgent navMeshAgent;
    private Transform bossTransform;


    public ParticleSystem ChargingL { get => chargingL; set => chargingL = value; }
    public ParticleSystem OctaL { get => octaL; set => octaL = value; }
    public Animator Animator { get => animator; set => animator = value; }
    public float RadarRange { get => radarRange; set => radarRange = value; }
    public LayerMask PlayerMask { get => playerMask; set => playerMask = value; }
    public float BossHP { get => bossHP; set => bossHP = value; }
    public List<Transform> PlayersList { get => playersList; set => playersList = value; }
    public NavMeshAgent NavMeshAgent { get => navMeshAgent; set => navMeshAgent = value; }
    public Transform BossTransform { get => bossTransform; set => bossTransform = value; }
    public List<Transform> TargetList { get => targetList; set => targetList = value; }


    // 주변의 Layer : Player 인 오브젝트 4개 검출
    public void UpdatePlayersList()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, RadarRange, PlayerMask);

        PlayersList.Clear();
        TargetList.Clear();

        if (cols.Length == 0)
        {
            /// 게임 종료 ///
        }

        else if (cols.Length > 5)
        {
            /// 초대받지 않은 손님이 있다!! ///
        }

        else
        {
            if (cols.Length > 0 && cols.Length < 5)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    PlayersList.Add(cols[i].transform); 
                    // 추후 UUID 값 리스트로 받기 : Add(cols[i].transform.name);
                    TargetList.Add(PlayersList[i].transform.Find("Boss Aim"));
                }
            }
        }
    }
}

