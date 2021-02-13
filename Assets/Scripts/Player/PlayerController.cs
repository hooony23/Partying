using UnityEngine;
using Util;
using Communication.API;
using Communication.JsonFormat;
public class PlayerController : MonoBehaviour
{
    

    private string userUuid = null;
    private PlayerInfo pInfo = new PlayerInfo();

    private bool isStun = false;
    private bool isDead = false;
    private bool getItem = false; // 클리어 아이템 획득 표시

    // 기본 움직임(w,a,s,d, spacebar)
    private float hAxis;
    private float vAxis;
    private Vector3 moveVec;
    private Vector3 moveDir;
    private Vector2 moveInput;


    private bool jDown; // sacebar키 입력 여부

    // 움직임 상태, 플레이어 상태
    private string playerState; // 플레이어 이벤트, 상태(run, dodge, ...)
    private float playerSpeed = Config.playerSpeed;
    private float playerHealth = Config.playerHealth;
    private bool isMove;
    private bool isDodge; // 회피동작 상태 여부

    // 상호작용
    private bool eDown; // E키 입력 여부    
    private GameObject nearObject; // 아이템 습득로직을 위한 오브젝트 정의

    // 애니메이션
    private Animator anim;

    // 물리효과
    private Rigidbody rigid;
    private bool isBorder;

    // 캐릭터 시점
    private Transform cameraArm;
    private Vector2 mouseDelta;

    public string UserUuid { get => UserUuid; set => UserUuid = value; }
    public PlayerInfo PInfo { get => pInfo; set => pInfo = value; }
    public bool IsStun { get => isStun; set => isStun = value; }
    public bool IsDead { get => isDead; set => isDead = value; }
    public bool GetItem { get => getItem; set => getItem = value; }
    public float HAxis { get => hAxis; set => hAxis = value; }
    public float VAxis { get => vAxis; set => vAxis = value; }
    public Vector3 MoveVec { get => moveVec; set => moveVec = value; }
    public Vector3 MoveDir { get => moveDir; set => moveDir = value; }
    public Vector2 MoveInput { get => moveInput; set => moveInput = value; }
    public bool JDown { get => jDown; set => jDown = value; }
    public string PlayerState { get => playerState; set => playerState = value; }
    public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }
    public float PlayerHealth { get => playerHealth; set => playerHealth = value; }
    public bool IsMove { get => isMove; set => isMove = value; }
    public bool IsDodge { get => isDodge; set => isDodge = value; }
    public bool EDown { get => eDown; set => eDown = value; }
    public GameObject NearObject { get => nearObject; set => nearObject = value; }
    public Animator Anim { get => anim; set => anim = value; }
    public Rigidbody Rigid { get => rigid; set => rigid = value; }
    public bool IsBorder { get => isBorder; set => isBorder = value; }
    public Transform CameraArm { get => cameraArm; set => cameraArm = value; }
    public Vector2 MouseDelta { get => mouseDelta; set => mouseDelta = value; }
}