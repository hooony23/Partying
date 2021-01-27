using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    // 방을 생성할 때 입력하는 정보
    // 생성된 방들의 정보를 모아 LobbyUI에 스크롤 형태로 표현
    private int roomIndex; // 0번부터 시작
    private string roomTitle;
    private string roomMaster; // 방장의 ID 혹은 닉네임
    private string roomPassword;
    private int headcount; // 인원수, 최대 4명

    public RoomInfo(int index, string title, string master, string password, int headcount)
    {
        this.roomIndex = index;
        this.roomTitle = title;
        this.roomMaster = master;
        this.roomPassword = password;
        this.headcount = headcount;
    }

    public int RoomIndex { get => roomIndex; set => roomIndex = value; }
    public string RoomTitle { get => roomTitle; set => roomTitle = value; }
    public string RoomMaster { get => roomMaster; set => roomMaster = value; }
    public string RoomPassword { get => roomPassword; set => roomPassword = value; }
    public int Headcount { get => headcount; set => headcount = value; }
}
