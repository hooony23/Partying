using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    // 방을 생성할 때 입력하는 정보
    // 생성된 방들의 정보를 모아 LobbyUI에 스크롤 형태로 표현

    private string roomUuid;
    private string roomName;
    private string roomAdmin; // 방장의 ID 혹은 닉네임
    private string memberCount; // 인원수, 최대 4명
    private string pwd;

    public string RoomUuid { get => roomUuid; set => roomUuid = value; }
    public string RoomName { get => roomName; set => roomName = value; }
    public string RoomAdmin { get => roomAdmin; set => roomAdmin = value; }
    public string MemberCount { get => memberCount; set => memberCount = value; }
    public string Pwd { get => pwd; set => pwd = value; }
}
