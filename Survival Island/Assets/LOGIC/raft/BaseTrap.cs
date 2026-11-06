using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Satu target di dalam zona jebakan.</summary>
    public struct TrapVictim
    {
        public IDamageable target;
        public EnemyAI ai;
        public Collider collider;
        public float nextHitTime;
    }

    /// <summary>
    /// Pertahanan base pasif: Spike Trap, kawat berduri, dan api unggun. Blueprint: raft/BaseDefense.md (#26).
    /// GDD bagian 3: horde menyerang base saat malam. Bagian 4: fungsi base. Jenis dan angka jebakan adalah usulan disetujui
    /// developer (#85). Jebakan adalah struktur biasa (BuildMenu, category Defense); komponen ini hanya mengatur efek saat
    /// diinjak. Trigger ada di anak TriggerZone (BaseTrapZone meneruskan pesannya). Tanpa target, Update dimatikan.
    ///
    /// Catatan implementasi (sistem pendukung): Flammable.TryIgnite (#97) belum ada (TODO).
    /// </summary>
    [RequireComponent(typeof(BaseHealth))]
    public class BaseTrap : MonoBehaviour
    {
        [SerializeField] private TrapData data;
        [Tooltip("Hanya untuk TrapKind.Campfire (#44).")]
        [SerializeField] private CampfireManager campfire;
        [SerializeField] private NoiseEmitter noiseEmitter;

        private TrapVictim[] victims;
        private int victimCount;
        private float tickTimer;
        private BaseHealth selfHealth;
        private TrapStats stats;

        private static TrapBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.trap : null;

        private void Awake()
        {
            selfHealth = GetComponent<BaseHealth>();
            if (campfire == null) campfire = GetComponent<CampfireManager>();
            if (noiseEmitter == null) noiseEmitter = GetComponentInChildren<NoiseEmitter>();
            var b = Bal;
            victims = new TrapVictim[b != null ? Mathf.Max(1, b.maxVictimsPerTrap) : 8];
            enabled = false;
        }

        private void OnEnable()
        {
            if (data != null) stats = data.GetStats(Bal);
        }

        private void OnDisable()
        {
            // Region dilepas atau jebakan hancur: daftar kosong. OnTriggerEnter berikutnya mengisinya lagi.
            // (Saat daftar kosong, komponen dimatikan sendiri, jadi tidak ada target yang hilang di sini.)
            ClearVictims();
        }

        private void ClearVictims()
        {
            for (int i = 0; i < victimCount; i++) victims[i] = default;
            victimCount = 0;
        }

        // ---------------------------------------------------------------- Masuk dan keluar (diteruskan BaseTrapZone)

        public void HandleEnter(Collider other)
        {
            if (data == null || victims == null) return;
            // 1. Usulan 2 dan 3: pemain dan NPC tidak terkena; zombie dan hewan terkena.
            if (!other.CompareTag("Zombie") && !other.CompareTag("Animal")) return;
            // 2.
            var target = other.GetComponentInParent<IDamageable>();
            var ai = other.GetComponentInParent<EnemyAI>();
            if (target == null || !target.IsAlive) return;
            // 3. Dormant, Submerged, dan BurnToAshes diabaikan.
            if (ai != null && !ai.IsActiveThreat) return;
            if (victimCount >= victims.Length) return;   // Penuh: diabaikan sampai ada yang keluar
            for (int i = 0; i < victimCount; i++) if (victims[i].collider == other) return;
            // 4.
            victims[victimCount++] = new TrapVictim { target = target, ai = ai, collider = other, nextHitTime = Time.time };
            if (!enabled) { stats = data.GetStats(Bal); tickTimer = 0f; }
            enabled = true;
        }

        public void HandleExit(Collider other)
        {
            for (int i = 0; i < victimCount; i++)
            {
                if (victims[i].collider != other) continue;
                RemoveAt(i);
                break;
            }
            if (victimCount == 0) enabled = false;
        }

        /// <summary>Tukar dengan posisi terakhir, tanpa alokasi.</summary>
        private void RemoveAt(int i)
        {
            victimCount--;
            victims[i] = victims[victimCount];
            victims[victimCount] = default;
        }

        // ---------------------------------------------------------------- Tick

        private void Update()
        {
            var b = Bal;
            if (data == null || b == null) return;
            // Game dijeda: Time.deltaTime 0, timer tidak maju.
            tickTimer += Time.deltaTime;
            if (tickTimer < b.tickIntervalSeconds) return;
            tickTimer = 0f;

            // 1. Api unggun padam: tick dilewati, target tetap tercatat.
            if (data.kind == TrapKind.Campfire && (campfire == null || !campfire.IsLit)) return;

            float now = Time.time;
            for (int i = victimCount - 1; i >= 0; i--)
            {
                var v = victims[i];
                // 2. Musuh kembali ke pool atau mati: OnTriggerExit tidak terpanggil, jadi dibuang di sini.
                bool gone = v.target == null || !v.target.IsAlive || v.collider == null || !v.collider.gameObject.activeInHierarchy
                            || (v.ai != null && !v.ai.IsActiveThreat);
                if (gone) { RemoveAt(i); continue; }
                // 3.
                if (now >= v.nextHitTime)
                {
                    HitVictim(ref victims[i]);
                    if (selfHealth == null || !selfHealth.IsAlive) return;   // Jebakan aus dan hancur
                }
            }
            if (victimCount == 0) enabled = false;
        }

        private void HitVictim(ref TrapVictim v)
        {
            Vector3 pos = v.collider.transform.position;
            // 1. Lewat IDamageable kapsul musuh (tanpa HitboxModifier, tanpa headshot). byPlayer: kematian masuk statistik pemain.
            var info = new DamageInfo
            {
                amount = stats.damagePerHit, type = data.damageType, source = gameObject, sourceId = data.trapId,
                hitPoint = pos, hitDirection = (pos - transform.position).normalized, bodyPart = BodyPart.Leg, byPlayer = true
            };
            v.target.TakeDamage(in info);
            // 2. Kawat: lambat. SpeedSource.Trap hanya satu nilai, jadi dua kawat tidak melipatgandakan.
            if (stats.slowMultiplier < 1f && v.ai != null) v.ai.ApplySpeedModifier(SpeedSource.Trap, stats.slowMultiplier, stats.slowSeconds);
            // 3.
            v.nextHitTime = Time.time + stats.hitIntervalSeconds;
            // 4.
            if (!string.IsNullOrEmpty(data.hitVfxPoolTag) && VFXManager.Instance != null)
                VFXManager.Instance.Play(data.hitVfxPoolTag, pos, Quaternion.identity);
            // 5.
            if (stats.noiseRadius > 0f)
            {
                if (noiseEmitter != null) noiseEmitter.Emit(stats.noiseRadius, NoiseSourceKind.Structure);
                else NoiseEmitter.Emit(transform.position, stats.noiseRadius, NoiseSourceKind.Structure);
            }
            // 7. TODO(FireSystem): Flammable.TryIgnite(fire.campfireTrapIgniteChancePerTick, byPlayer) pada musuh di zona (#97).
            // 6. Keausan lewat HP struktur (usulan 6). Dipanggil terakhir karena bisa menghancurkan jebakan.
            if (stats.selfDamagePerHit > 0f && selfHealth != null)
            {
                var self = new DamageInfo
                {
                    amount = stats.selfDamagePerHit, type = DamageType.Structural, source = gameObject, sourceId = data.trapId,
                    hitPoint = transform.position, hitDirection = Vector3.down, bodyPart = BodyPart.Torso, byPlayer = false
                };
                selfHealth.TakeDamage(in self);
            }
        }
    }
}
