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
        public List<Transform> TargetList { get; set; } = new List<Transform>();
        public ParticleSystem ChargingL { get; set; }
        public ParticleSystem OctaL { get; set; }
        public Animator Animator { get; set; }
        public LayerMask PlayerMask { get; set; } = 0;
        public NavMeshAgent NavMeshAgent { get; set; }
        public SphereCollider BossCollider { get; set; }
    }
}

