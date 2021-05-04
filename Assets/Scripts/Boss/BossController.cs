using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Util;
using GameManager;
namespace Boss
{
    public class BossController : MonoBehaviour
    {
        public float RadarRange { get; set; } = Config.radarRange;
        public float BossHP { get; set; } = Config.bossHP;
        public RaidGameManager GM { get; set; }
        public List<Transform> TargetList { get; set; } = new List<Transform>();
        public ParticleSystem ChargingL { get; set; }
        public ParticleSystem OctaL { get; set; }
        public Animator AnimController { get; set; }
        public LayerMask PlayerMask { get; set; } = 0;
        public NavMeshAgent NavMeshAgent { get; set; }
        public SphereCollider BossCollider { get; set; }
    }
}

