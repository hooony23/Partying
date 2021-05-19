using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed = 6f;
    public float turnSpeed = 15f;
    public Transform mainCamera;

    public float turnSmoothTime = 0.05f;
    float turnSmoothVelocity;

    public Cinemachine.AxisState xAxis;
    public Cinemachine.AxisState yAxis;

    private Transform cmFollowTarget;

    private GameObject CameraArm;
    private Cinemachine.CinemachineVirtualCamera vcam;

    private Animator aimAnimator;


    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera").transform;
        cmFollowTarget = this.transform.Find("CM Follow Target").transform;
        CameraArm = GameObject.Find("CameraArm2");
        vcam = CameraArm.GetComponent<Cinemachine.CinemachineVirtualCamera>();
        aimAnimator = vcam.GetComponent<Animator>();
    }
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y; // 카메라가 보는 화면기준으로 플레이어의 방향을 전환
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // 플레이어의 방향을 부드럽게 전환
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            this.transform.position += moveDir.normalized * speed * Time.deltaTime;
        }

        // 카메라 컨트롤
        xAxis.Update(Time.fixedDeltaTime);
        yAxis.Update(Time.fixedDeltaTime);

        cmFollowTarget.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        aim();
    }

    private void aim()
    {
        if (Input.GetMouseButton(1))
        {
            aimAnimator.SetBool("isAimming", true);
        }
        else
        {
            aimAnimator.SetBool("isAimming", false);
        }
    }
}


//if (direction.magnitude >= 0.1f)
//{
//    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y; // 카메라가 보는 화면기준으로 플레이어의 방향을 전환
//    float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // 플레이어의 방향을 부드럽게 전환
//    transform.rotation = Quaternion.Euler(0f, angle, 0f);

//    Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

//    this.transform.position += (moveDir.normalized * speed * Time.deltaTime);
//}
