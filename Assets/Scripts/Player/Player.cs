using UnityEngine.SceneManagement;
using UnityEngine;
public class Player : PlayerUtil
{
    [SerializeField] private GameObject cameraArm;

    /*@@@ 서버 @@@*/
    string userID = null;
    PlayerInfo pInfo = new PlayerInfo();
    
    void Awake()
    {

        playerController.CameraArm = cameraArm.transform;
        playerController.Anim = GetComponent<Animator>();
        playerController.Rigid = GetComponent<Rigidbody>();

        /* 서버 연결 */
        // AsynchronousClient.Connected();   

        // 서버로부터 uuid 받아옴
        /* string response = AsynchronousClient.Connected();
        JObject responseJson = JObject.Parse(response);
        userID = responseJson["data"].Value<string>("uuid"); */

    }

    // 에티터 플레이버튼, 앱의 종료 -> 생명주기 종료
    private void OnApplicationQuit()
    {
        /* 서버 연결 해제 */
        // AsynchronousClient.ConnectedExit();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        Move();
        Turn();
        GetItem();
        CameraTurn();
        Dodge();
        PlayerStateUpdate();
    }
    
    private void FixedUpdate() // default : 50fps
    {
        /* 서버 전송 */
        // CharacterInfo 에 현재 플레이어의 상태 입력
        // CharacterInfo 를 서버로 전송
        pInfo.UpdateInfo(transform.position, playerController.MoveDir, playerController.PlayerState, userID); 
        string jsonData = pInfo.ObjectToJson(pInfo);
        // AsynchronousClient.Send(jsonData);

        
        FreezeRotation();
        StopToWall();
    }

    

    private void OnTriggerStay(Collider other) //플레이어 범위에 아이템이 인식할 수 있는지 확인
    {
        IsClear(other);

    }
    private void OnTriggerExit(Collider other) //플레이어 범위에 아이템이 벗어났는지 확인
    {
        IsGetItem(other);
    }
}