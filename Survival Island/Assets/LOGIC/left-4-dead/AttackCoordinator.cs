using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pembagi izin menyerang per target. Blueprint: left-4-dead/EnemyAttack.md bagian "Membatasi penyerang" (#109).
    /// Usulan disetujui developer: 3 penyerang untuk pemain, 2 untuk tiap NPC. Array kecil, tanpa Dictionary baru.
    /// Kalau tidak ada instance di scene, semua permintaan diizinkan (Fase 1 tanpa coordinator).
    /// </summary>
    public class AttackCoordinator : MonoBehaviour
    {
        public static AttackCoordinator Instance { get; private set; }

        private const int MaxHolders = 64;
        private const int MaxTargets = 16;

        // Pemegang izin: enemyId -> targetId
        private readonly int[] holderEnemy = new int[MaxHolders];
        private readonly int[] holderTarget = new int[MaxHolders];
        private int holderCount;

        // Batas sementara per target (#110)
        private readonly int[] limitTarget = new int[MaxTargets];
        private readonly int[] limitValue = new int[MaxTargets];
        private int limitCount;

        private void Awake()
        {
            if (Instance != null && Instance != this) { enabled = false; return; }
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
        }

        private void OnDisable()
        {
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public static bool TryAcquireStatic(ITargetable t, int enemyId) => Instance == null || Instance.TryAcquire(t, enemyId);
        public static void ReleaseStatic(int enemyId) { if (Instance != null) Instance.Release(enemyId); }

        public bool TryAcquire(ITargetable t, int enemyId)
        {
            if (t == null) return false;
            int targetId = t.TargetId;
            int held = 0;
            for (int i = 0; i < holderCount; i++)
            {
                if (holderEnemy[i] == enemyId) return holderTarget[i] == targetId;   // Sudah memegang
                if (holderTarget[i] == targetId) held++;
            }
            if (held >= LimitFor(t) || holderCount >= MaxHolders) return false;
            holderEnemy[holderCount] = enemyId;
            holderTarget[holderCount] = targetId;
            holderCount++;
            return true;
        }

        /// <summary>Aman dipanggil berkali-kali.</summary>
        public void Release(int enemyId)
        {
            for (int i = 0; i < holderCount; i++)
            {
                if (holderEnemy[i] != enemyId) continue;
                holderCount--;
                holderEnemy[i] = holderEnemy[holderCount];
                holderTarget[i] = holderTarget[holderCount];
                return;
            }
        }

        /// <summary>Batas sementara (#110). -1 mengembalikan batas awal.</summary>
        public void SetAttackerLimit(ITargetable t, int limit)
        {
            if (t == null) return;
            int id = t.TargetId;
            for (int i = 0; i < limitCount; i++)
            {
                if (limitTarget[i] != id) continue;
                if (limit < 0) { limitCount--; limitTarget[i] = limitTarget[limitCount]; limitValue[i] = limitValue[limitCount]; }
                else limitValue[i] = limit;
                return;
            }
            if (limit >= 0 && limitCount < MaxTargets) { limitTarget[limitCount] = id; limitValue[limitCount] = limit; limitCount++; }
        }

        private int LimitFor(ITargetable t)
        {
            int id = t.TargetId;
            for (int i = 0; i < limitCount; i++) if (limitTarget[i] == id) return limitValue[i];
            var bal = GameManager.CurrentBalance;
            if (bal == null) return 3;
            return t.Kind == TargetKind.NPC ? bal.enemyAttack.maxAttackersOnNpc : bal.enemyAttack.maxAttackersOnPlayer;
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            holderCount = 0;
        }
    }
}
