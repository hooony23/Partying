using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


// 방 안에 들어옴

public class Room : MonoBehaviour
{
    [SerializeField] private Text title = null;
    [SerializeField] private Button startButton = null;
    
    // 현재 방의 정보
    private RoomInfo currentRoom = null;

    // 서버에서 받아온 현재 방에 들어온 유저 정보
    private List<string> users = new List<string>();

    // 현재 방의 유저 UI
    [SerializeField] GameObject[] players = null;

    
    private void Start()
    {
        // 서버에서 받아온 roomInfo
        //urrentRoom = new RoomInfo(0, "안녕안녕", "hooony23", "1234", 1);

        // 서버에서 받아온 유저 정보
        users.Add("hooony23");
        users.Add("skine134");
        users.Add("prawn5252");
        //users.Add("sunkyu29");

        // 입장한 방의 정보(방 제목...등)를 바탕으로 세팅
        players = GetComponents<GameObject>(); // PLAYER(1~4) UI
        SetupRoom();

    }

    private void FixedUpdate()
    {
        // 유저수 4명인지 확인하고 4명이면 시작하기 활성화, 아니면 비활성화
        ActiveStartButton();

        UpdatePlayerList();
    }
    public void SetupRoom()
    {
        // 방제목 ...등등 초기화
        //title.text = currentRoom.RoomTitle;
    }

    // 플레이어가 들어오면 PLAYER1, PLAYER2, PLAYER3, PLAYER4 UI 업데이트
    private void UpdatePlayerList()
    {
        Text pText;
        Image pImage;

        for (int i = 0; i < users.Count; i++)
        {
            // USER(1~4) UI 초기화
            pText = players[i].GetComponentInChildren<Text>();
            pImage = players[i].GetComponentInChildren<Image>();

            pText.text = users[i];
            pImage.color = Color.white;
        }
    }

    private void ActiveStartButton()
    {
        if (users.Count == 4)
            startButton.interactable = true;
        else
            startButton.interactable = false;
    }

     public void OnClickGameStart() //private으로 하려 했으나, 이벤트 요소는 public으로만 끌어올 수 있는듯.
    {
        SceneManager.LoadScene("LodingScene");
        // Game Scene 으로 넘어감
    }

}
