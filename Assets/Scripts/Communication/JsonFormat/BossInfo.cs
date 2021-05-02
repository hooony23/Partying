using UnityEngine;
using Newtonsoft.Json;
using Util;

namespace Communication.JsonFormat
{
    public class BossInfo
    {
        public enum Patterns { CHANGINGELAGER, OCTALASER, BODYSLAM, DIE, IDLE }
        public Patterns pattern = Patterns.IDLE;
        public Division3 Vec {get; set;}= new Division3();
        public Division3 Loc {get; set;}= new Division3();
        private float bossHP = Config.bossHP;
        public float BossHP
        {
            get
            {
                return bossHP;
            }
            set
            {
                lock(this)
                {
                    bossHP = value;
                }
            }
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
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