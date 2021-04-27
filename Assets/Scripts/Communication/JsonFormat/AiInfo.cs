using UnityEngine;
using Util;

namespace Communication.JsonFormat
{
    public class AiInfo
    {
        public string Uuid {get;set;} = "Patrol";
        public Division3 Loc {get;set;} = new Division3();
        public Division3 Vec {get;set;} = new Division3();
        
        public AiInfo(float Lx,float Ly, float Lz,float Vx, float Vy, float Vz,string uuid)
        {
            Loc.X = Lx;
            Loc.Y = Ly;
            Loc.Z = Lz;

            Vec.X = Vx;
            Vec.Y = Vy;
            Vec.Z = Vz;
            Uuid = uuid;
        }
        public AiInfo(Division3 loc,Division3 vec,string uuid):this(loc.X,loc.Y,vec.X,vec.Y,vec.Y,vec.Z,uuid){}
        public AiInfo(Vector3 loc,Vector3 vec,string uuid):this(loc.x,loc.y,vec.z,vec.x,vec.y,vec.z,uuid){}
        public AiInfo():this(0,0,0,0,0,0,"Patrol"){}
        
        public Vector3 GetVecToVector3()
        {
            return new Vector3(Vec.X,Vec.Y,Vec.Z);
        }
        public Vector3 GetLocToVector3()
        {
            return new Vector3(Loc.X,Loc.Y,Loc.Z);
        }
    }
}