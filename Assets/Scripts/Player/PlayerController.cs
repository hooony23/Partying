using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool isStun = false;
    private bool isDead = false;
    private bool getItem = false; // 클리어 아이템 획득 표시

    // 기본 움직임(w,a,s,d, spacebar)
    private float hAxis;
    private float vAxis;
    private Vector3 moveVec;
    private Vector3 moveDir;
    private Vector2 moveInput;

    private string walkSound = "Walk";

    private bool jDown; // sacebar키 입력 여부

    // 움직임 상태, 플레이어 상태
    private string playerState; // 플레이어 이벤트, 상태(run, dodge, ...)
    public float playerSpeed = Config.playerSpeed;
    public float playerHealth = Config.playerHealth;
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
    public Transform cameraArm;
    private Vector2 mouseDelta;
    public bool IsStun{
        
        get
        {
            return this.isStun;
        }
        set
        {
            this.isStun = value;
        }
    }
    public bool IsDead{
        
        get
        {
            return this.isDead;
        }
        set
        {
            this.isDead = value;
        }
    }
    public bool GetItem{
        
        get
        {
            return this.getItem;
        }
        set
        {
            this.getItem = value;
        }
    }
    public float HAxis
    {
        get
        {
            return this.hAxis;
        }
        set
        {
            this.hAxis = value;
        }
    }

    public float VAxis
    {
        get
        {
            return this.vAxis;
        }
        set
        {
            this.vAxis = value;
        }
    }

    public Vector3 MoveVec
    {
        get
        {
            return this.moveVec;
        }
        set
        {
            this.moveVec = value;
        }
    }

    public Vector3 MoveDir
    {
        get
        {
            return this.moveDir;
        }
        set
        {
            this.moveDir = value;
        }
    }

    public bool JDown
    {
        get
        {
            return this.jDown;
        }
        set
        {
            this.jDown = value;
        }
    }

    public string PlayerState
    {
        get
        {
            return this.playerState;
        }
        set
        {
            this.playerState = value;
        }
    }

    public float PlayerSpeed
    {
        get
        {
            return this.playerSpeed;
        }
        set
        {
            this.playerSpeed = value;
        }
    }

    public float PlayerHealth
    {
        get
        {
            return this.playerHealth;
        }
        set
        {
            this.playerHealth = value;
        }
    }

    public bool IsMove
    {
        get
        {
            return this.isMove;
        }
        set
        {
            this.isMove = value;
        }
    }

    public bool IsDodge
    {
        get
        {
            return this.isDodge;
        }
        set
        {
            this.isDodge = value;
        }
    }

    public bool EDown
    {
        get
        {
            return this.eDown;
        }
        set
        {
            this.eDown = value;
        }
    }

    public Animator Anim
    {
        get
        {
            return this.anim;
        }
        set
        {
            this.anim = value;
        }
    }

    public Rigidbody Rigid
    {
        get
        {
            return this.rigid;
        }
        set
        {
            this.rigid = value;
        }
    }

    public bool IsBorder
    {
        get
        {
            return this.isBorder;
        }
        set
        {
            this.isBorder = value;
        }
    }

    public Transform CameraArm
    {
        get
        {
            return this.cameraArm;
        }
        set
        {
            this.cameraArm = value;
        }
    }

    public Vector2 MouseDelta
    {
        get
        {
            return this.mouseDelta;
        }
        set
        {
            this.mouseDelta = value;
        }
    }
    public Vector2 MoveInput
    {
        
        get
        {
            return this.moveInput;
        }
        set
        {
            this.moveInput = value;
        }
    }
    public GameObject NearObject
    {
        
        get
        {
            return this.nearObject;
        }
        set
        {
            this.nearObject = value;
        }
    }
    public string WalkSound
    {
        get
        {
            return this.walkSound;
        }
        set
        {
            this.walkSound = value;
        }
    }
}