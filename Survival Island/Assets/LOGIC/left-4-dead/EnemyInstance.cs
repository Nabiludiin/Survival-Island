using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    /// <summary>
    /// Data spawn per musuh Fase 2, terpisah dari AI. Blueprint: left-4-dead/SpawnManager.md bagian "Pencatatan per musuh" (#62).
    /// </summary>
    public class EnemyInstance : MonoBehaviour, IPoolable
    {
        public string PoolTag { get; internal set; }
        public int GroupId { get; internal set; }
        public bool IsEngaged { get; set; }
        public bool IsDormantCounted { get; set; }
        public bool IsSacrificing;          // Diisi LuckyMachine (#72)
        public bool IsCarcass;              // Diisi EnemyHitReaction untuk hewan mati (#95)
        public bool UsesNightBudget;        // Hanya zombie dari RequestWave (#27)
        public bool IsFromOverride { get; internal set; }

        // Pengaman macet
        internal Vector3 lastPosition;
        internal float stuckTimer;
        internal float offMeshTimer;

        // Status culling milik OptimizationManager (usulan)
        public bool IsCulled;
        public bool culledAgentWasEnabled;
        public bool culledAnimatorWasEnabled;

        public EnemyHealth Health { get; private set; }
        public EnemyData Data => Health != null ? Health.Data : null;
        public bool IsZombie => Data is ZombieData;
        public bool IsAnimal => Data is AnimalData;
        public NavMeshAgent Agent { get; private set; }
        public EnemyAI AI { get; private set; }
        public WildlifeAI Wildlife { get; private set; }

        private Animator animator;

        private void Awake()
        {
            Health = GetComponent<EnemyHealth>();
            Agent = GetComponent<NavMeshAgent>();
            AI = GetComponent<EnemyAI>();
            Wildlife = GetComponent<WildlifeAI>();
            animator = GetComponentInChildren<Animator>();
        }

        public void OnSpawned()
        {
            var pooled = GetComponent<PooledObject>();
            PoolTag = pooled != null ? pooled.PoolTag : (Data != null ? Data.enemyId : null);
            stuckTimer = 0f;
            offMeshTimer = 0f;
            lastPosition = transform.position;
            IsEngaged = false;
            IsDormantCounted = false;
            IsSacrificing = false;
            IsCarcass = false;
            UsesNightBudget = false;
            IsFromOverride = false;
            GroupId = 0;
            IsCulled = false;
            culledAgentWasEnabled = false;
            culledAnimatorWasEnabled = false;
            if (animator != null) animator.enabled = true;
        }

        public void OnReturned() { }
    }
}
