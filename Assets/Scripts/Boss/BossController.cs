using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Util;
using GameManager;
using Communication.JsonFormat;
namespace Boss
{
    public class BossController : MonoBehaviour
    {
        public BossInfo.Patterns Pattern{ get; set; } = BossInfo.Patterns.IDLE;
        public float RadarRange { get; set; } = Config.radarRange;
        public float BossHP { get; set; }
        public RaidGameManager GM { get; set; }
        public string Target{get;set;}
        public ParticleSystem ChargingL { get; set; }
        public ParticleSystem OctaL { get; set; }
        public Animator AnimController { get; set; }
        public LayerMask PlayerMask { get; set; } = 0;
        public SphereCollider BossCollider { get; set; }
        public bool PatternActivated { get; set; } = false;

        public Rigidbody BossRigid { get; set; }
    }
}

