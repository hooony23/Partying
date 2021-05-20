using UnityEngine;
using Util;
using Communication.GameServer.API;
using Communication.JsonFormat;
using Boss;
using Weapon;
using GameUi;

public class PlayerController : MonoBehaviour
{
    public enum Movement 
    {
        Idle,
        Shot,
        Run,
        Dodge,
        Dead,
    }
    public GameManager.GameManager GM {get;set;}
    public bool IsStun { get; set; } = false;
    public bool IsDead { get; set; } = false;
    public bool HaveItem { get; set; } = false; // 클리어 아이템 획득 표시
    public string UserUuid { get; set; }
    public PlayerInfo PInfo { get; set; } = new PlayerInfo();

    public UserScore UserScore { get; set; } = new UserScore();

    // 기본 움직임(w,a,s,d, spacebar)
    public bool JDown { get; set; } // sacebar키 입력 여부
    public float HAxis { get; set; }
    public float VAxis { get; set; }
    public Vector3 MoveVec { get; set; }
    public Vector3 MoveDir { get; set; }
    public Vector2 MoveInput { get; set; }
    public bool MouseClickInput { get; set; }

    // 움직임 상태, 플레이어 상태
    public bool IsMove { get; set; }
    public bool IsDodge { get; set; } // 회피동작 상태 여부
    public bool IsAttack { get; set; }
    public PlayerController.Movement PlayerState { get; set; } // 플레이어 이벤트, 상태(run, dodge, ...)
    public float PlayerSpeed { get; set; } = Config.playerSpeed;
    public float PlayerMaxHealth { get; set; } = Config.playerHealth;
    public float PlayerHealth { get; set; } = Config.playerHealth;
    public float AttackDamage { get; set; } = Config.playerAttackDamage;
    public float ShotSpeed { get; set; } = Config.shotSpeed;

    // 상호작용
    public bool EDown { get; set; } // E키 입력 여부
    public GameObject NearObject { get; set; } // 아이템 습득로직을 위한 오브젝트 정의

    // 애니메이션
    public Animator Anim { get; set; }

    // 물리효과
    public bool IsBorder { get; set; }
    public Rigidbody Rigid { get; set; }

    // 캐릭터 시점
    public Camera CameraMain { get; set; }
    public CMController CameraArm { get; set; }
    public bool isAimming { get; set; } = false;
    public Transform CmFollowTarget { get; set; }

    // 플레이어 피격 효과
    public Material Mat { get; set; }

    public bool IsBeatable { get; set; } = true; // 플레이어 무적 상태 컨트롤

    // 총
    public MusslePoint Pistol { get; set; }

    public Transform ShotPoint { get; set; }

}
