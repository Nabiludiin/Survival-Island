using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Batu pijar erupsi (pool Hazard_Fireball, layer Projectile). Blueprint: Island_Logic/DisasterManager.md bagian 2
    /// "Erupsi" dan "Fireball.Impact()" (#70). Claude.md Tahap 4, Region 1: fireball diambil dari ObjectPooler dan
    /// dijatuhkan dari langit di sekitar pemain.
    ///
    /// Jalur kinematik tanpa fisika: posisi dihitung dari waktu, jadi tidak bisa menembus sesuatu tanpa terdeteksi.
    /// GDD bagian 5: target yang terlindung atap atau dinding tidak menerima damage, sehingga MC terpaksa berlindung.
    /// </summary>
    public class Fireball : MonoBehaviour, IPoolable
    {
        public const string PoolTag = "Hazard_Fireball";

        [Tooltip("Jejak partikel. Dikosongkan setiap keluar dari pool.")]
        [SerializeField] private TrailRenderer trail;
        [SerializeField] private ParticleSystem trailParticles;

        private static Collider[] buffer = new Collider[64];
        private static readonly List<IDamageable> targets = new List<IDamageable>(32);
        private static readonly List<Collider> targetColliders = new List<Collider>(32);
        private static readonly Collider[] treeBuffer = new Collider[32];

        private Vector3 startPos;
        private Vector3 targetPos;
        private float travelSeconds;
        private float elapsed;
        private bool hasImpacted;
        private bool isFlying;
        private HazardImpactMarker marker;

        /// <summary>Isi setelah SpawnFromPool. marker boleh null.</summary>
        public void Launch(Vector3 from, Vector3 to, float seconds, HazardImpactMarker impactMarker)
        {
            startPos = from;
            targetPos = to;
            travelSeconds = Mathf.Max(0.05f, seconds);
            elapsed = 0f;
            hasImpacted = false;
            isFlying = true;
            marker = impactMarker;
            transform.position = from;
            Vector3 dir = to - from;
            if (dir.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(dir);
        }

        private void Update()
        {
            if (!isFlying || hasImpacted) return;
            // Waktu gameplay, berhenti saat jeda (DisasterManager.md bagian 4).
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / travelSeconds);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            if (t >= 1f) Impact();
        }

        /// <summary>Langkah 1-6 "Fireball.Impact()" di DisasterManager.md.</summary>
        public void Impact()
        {
            if (hasImpacted) return;
            hasImpacted = true;
            isFlying = false;

            var bal = GameManager.CurrentBalance;
            if (bal != null)
            {
                var d = bal.disaster;
                ApplyBlastDamage(targetPos, d.fireballRadius, d.fireballDamageCenter, d.fireballDamageEdge, d.fireballStructureDamage,
                                 "Fireball", true);

                // Langkah 4: dengan peluang fireballTreeChance, pohon terdekat habis tanpa drop (aliran tampilan, bukan seed).
                if (Random.value < d.fireballTreeChance)
                {
                    var node = FindNearestTree(targetPos, d.fireballRadius, 0f);
                    if (node != null) node.ForceDeplete(false);
                }
            }

            // TODO #15: VFXManager.Play("Fx_FireballImpact", titik). TODO #24: CameraShake.PlayAt(FireballImpact, titik).
            // TODO #97: FireSystem.Instance.SpawnFireArea(titik, fire.fireballAreaRadius, ...), setelah FireSystem ditulis.

            ReturnSelf();
        }

        /// <summary>
        /// Damage ledakan dengan pemeriksaan perlindungan. Bangunan selalu menerima structureDamage. Makhluk diperiksa
        /// dengan Raycast PhysicsMasks.AIVision ke target; kalau terhalang objek lain, target terlindung.
        /// Dipakai juga pohon tumbang (checkCover = false).
        /// </summary>
        public static void ApplyBlastDamage(Vector3 center, float radius, float damageCenter, float damageEdge,
                                            float structureDamage, string sourceId, bool checkCover)
        {
            if (radius <= 0f) return;
            int count = PhysicsQuery.OverlapSphere(center, radius, PhysicsMasks.Explosion, buffer);
            PhysicsQuery.CollectUniqueDamageables(buffer, count, targets, targetColliders);

            for (int i = 0; i < targets.Count; i++)
            {
                var target = targets[i];
                var col = targetColliders[i];
                if (target == null || col == null || !target.IsAlive) continue;

                Vector3 closest = col.ClosestPoint(center);
                Vector3 delta = closest - center;
                float dist = delta.magnitude;
                var info = new DamageInfo
                {
                    type = DamageType.Explosion,
                    sourceId = sourceId,
                    hitPoint = closest,
                    hitDirection = dist > 0.0001f ? delta / dist : Vector3.up,
                    bodyPart = BodyPart.Torso,
                    byPlayer = false
                };

                if (target is IStructureHealth)
                {
                    info.amount = structureDamage;
                    target.TakeDamage(info);
                    continue;
                }

                if (checkCover && IsCovered(center, col)) continue;
                info.amount = Mathf.Lerp(damageCenter, damageEdge, Mathf.Clamp01(dist / radius));
                target.TakeDamage(info);
            }
            targets.Clear();
            targetColliders.Clear();
        }

        private static bool IsCovered(Vector3 center, Collider targetCol)
        {
            // Titik awal sedikit di atas benturan, supaya tanah tempat jatuh tidak dihitung sebagai penghalang.
            Vector3 origin = center + Vector3.up * 0.5f;
            Vector3 aim = targetCol.bounds.center;
            Vector3 dir = aim - origin;
            float dist = dir.magnitude;
            if (dist < 0.01f) return false;
            if (!PhysicsQuery.Raycast(new Ray(origin, dir / dist), out var hit, dist, PhysicsMasks.AIVision)) return false;
            // Mengenai target itu sendiri (atau anaknya) berarti tidak terhalang.
            return hit.collider != targetCol && hit.collider.transform.root != targetCol.transform.root;
        }

        /// <summary>Pohon ResourceNode terdekat yang masih berdiri, di antara minDistance dan radius.</summary>
        public static ResourceNode FindNearestTree(Vector3 center, float radius, float minDistance)
        {
            int count = PhysicsQuery.OverlapSphere(center, radius, PhysicsMasks.Ground, treeBuffer);
            ResourceNode best = null;
            float bestSq = float.MaxValue;
            float minSq = minDistance * minDistance;
            for (int i = 0; i < count; i++)
            {
                var c = treeBuffer[i];
                if (c == null) continue;
                var node = c.GetComponentInParent<ResourceNode>();
                if (node == null || node.IsDepleted || node.Data == null || node.Data.nodeType != NodeType.Tree) continue;
                float sq = (node.transform.position - center).sqrMagnitude;
                if (sq < minSq || sq >= bestSq) continue;
                bestSq = sq;
                best = node;
            }
            return best;
        }

        private void ReturnSelf()
        {
            if (marker != null) { marker.ReturnNow(); marker = null; }
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
            else gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------- IPoolable (DisasterManager.md bagian 4 "Pool")

        public void OnSpawned()
        {
            elapsed = 0f;
            hasImpacted = false;
            isFlying = false;
            marker = null;
            if (trail != null) trail.Clear();
            if (trailParticles != null) { trailParticles.Clear(true); trailParticles.Play(true); }
        }

        public void OnReturned()
        {
            isFlying = false;
            // Penanda ikut kembali ke pool (DisasterManager.md: OnReturned mengembalikan penandanya).
            if (marker != null) { marker.ReturnNow(); marker = null; }
            if (trailParticles != null) trailParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
