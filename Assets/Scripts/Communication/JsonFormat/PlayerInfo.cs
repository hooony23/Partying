using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Util;

namespace Communication.JsonFormat
{

    public class PlayerInfo
    {
        public PlayerController.Movement movement = 0;
        public string uuid = "";
        public Division3 vec = new Division3();
        public Division3 loc = new Division3();
        public Division3 angle = new Division3();
        public PlayerInfo() : this(new Vector3(0, 0, 0), new Vector3(0, 0, 0), PlayerController.Movement.Idle, "None") { }
        public PlayerInfo(Vector3 location, Vector3 moveVec, PlayerController.Movement playerEvent, string userID)
        {
            ///<summary>
            /// Description
            /// 플레이어의 정보를 설정합니다.
            /// 
            /// params 
            /// location : 플레이어의 위치
            /// moveVec : 플레이어의 움직임에 대한 벡터값
            /// playerEvent : 현재 플레이어의 상태
            /// userID : 플레이어의 uuid
            /// </summary>

            loc.X = location.x;
            loc.Y = location.y;
            loc.Z = location.z;
            vec.X = moveVec.x;
            vec.Y = moveVec.y;
            vec.Z = moveVec.z;

            // event
            movement = playerEvent;

            // uuid
            uuid = userID;


        }
        public void SetInfo(Vector3 location, Vector3 moveVec, PlayerController.Movement playerEvent, string userID)
        {
            ///<summary>
            /// Description
            /// 플레이어의 정보를 새로운 정보로 변경합니다.
            ///
            /// /// params 
            /// location : 플레이어의 위치
            /// moveVec : 플레이어의 움직임에 대한 벡터값
            /// playerEvent : 현재 플레이어의 상태
            /// userID : 플레이어의 uuid
            /// </summary>

            loc.X = location.x;
            loc.Y = location.y;
            loc.Z = location.z;
            vec.X = moveVec.x;
            vec.Y = moveVec.y;
            vec.Z = moveVec.z;

            // event
            movement = playerEvent;

            // uuid
            uuid = userID;


        }

        public string ObjectToJson()
        {
            /// <summary>
            /// 현재 오브젝트를 json 형식의 string으로 반환합니다.
            /// </summary>
            /// <returns>
            /// 
            /// {
            ///     "uuid":"",
            ///     "movement":"",
            ///     "vec":{
            ///         "x":float,
            ///         "y":float,
            ///         "z":float
            ///     },
            ///     "loc":{
            ///         "x":float,
            ///         "y":float,
            ///         "z":float
            ///     }
            /// }
            /// </returns>
            return JsonConvert.SerializeObject(this);
        }

        public void SetAngle(Vector3 angle)
        {
            this.angle.X = angle.x;
            this.angle.Y = angle.y;
            this.angle.Z = angle.z;
        }


    }
}