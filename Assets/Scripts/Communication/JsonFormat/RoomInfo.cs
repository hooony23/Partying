using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo
{
    // 방을 생성할 때 입력하는 정보
    // 생성된 방들의 정보를 모아 LobbyUI에 스크롤 형태로 표현
    public string RoomUuid {get;set;}
    public string RoomName {get;set;}
    public string RoomAdmin {get;set;}  // 방장의 ID 혹은 닉네임
    public string MemberCount {get;set;} // 인원수, 최대 4명
    public string Pwd {get;set;}
}
