using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSetting : MonoBehaviour
{
    public GameObject warning;
    public GameObject nextScreen; // RoomUI

    [SerializeField] private InputField roomTitleInput = null;  
    [SerializeField] private InputField roomPasswordInput = null;
    private string title = "";
    private string password = "";

    // 서버 : 생성하기(create) 버튼 누를 시 roomInfo 클래스 배열에 roomInfo 추가
    // roomInfo : 방제목, 방장(닉네임또는 아이디), 비밀번호(없으면 공개방), 인원수
    // RoomInfo room = new RoomInfo(title, userID(서버), password, headcount)

    private void Start()
    {

    }

    public void onClickCreate()
    {
        // 방제목(입력필수), 암호 확인 , roomInfo 클래스 객체 이용하여, Lobby의 rooms배열에 추가
        // 방제목 미입력 시 생성하기 못함, 경고 활성화

        title = roomTitleInput.text;
        password = roomPasswordInput.text;

        if (title != "")
        {
            Debug.Log("방을 생성하였습니다");
            // room 정보를 만들어 서버로 전송
            // RoomInfo room = new RoomInfo(title, "서버에서받은 ID", password, 1); // 1 : 최초생성 은 방장 1명 


            // 다음화면으로
            this.gameObject.SetActive(false);
            nextScreen.SetActive(true);
        }
        else
        {
            Debug.Log("방 제목을 입력해주세요");
        }

    }
}
