using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    public enum HitReactionKind { Stagger, Knockback, Dying }

    /// <summary>Penerima reaksi kena: EnemyAI (zombie) dan WildlifeAI (hewan). EnemyHitReaction.md (#95).</summary>
    public interface IHitReactive
    {
        bool RequestReaction(HitReactionKind kind);   // false kalau ditolak prioritas
        void EndReaction();
        void SetSpeedMultiplier(SpeedSource source, float multiplier);
    }

    /// <summary>
    /// Respons musuh saat kena dan setelah mati: stagger, knockback lewat NavMeshAgent.Move, lambat kena kaki, mayat, ragdoll.
    /// Blueprint: left-4-dead/EnemyHitReaction.md (#95). Satu-satunya pemilik pengembalian musuh yang mati ke pool
    /// (SpawnManager.Despawn(Killed)), kecuali bangkai hewan (AnimalCarcass) dan zombie terbakar (SunlightExposure).
    /// GDD bagian 5: Zombie Militer berarmor tidak terhuyung kalau badannya ditembak (armorNoFlinchBelow).
    /// Percikan logam pada armor lewat VFXManager.Play("VFX_MetalSpark") (#15).
    /// </summary>
    public class EnemyHitReaction : MonoBehaviour, IPoolable
    {
        private const string MetalSparkTag = "VFX_MetalSpark";   // Katalog VFXManager.md

        [SerializeField] private Animator animator;
        [SerializeField] private Collider bodyCapsule;          // Kapsul gerak di layer Enemy
        [SerializeField] private Renderer[] renderers;

        private EnemyHealth health;
        private EnemyAttack attack;
        private NavMeshAgent agent;
        private IHitReactive reactive;
        private EnemyInstance instance;
        private SunlightExposure sunlight;
        private Rigidbody[] bones;
        private int[] boneLayers;
        private MaterialPropertyBlock block;

        private float accumulatedPoise;
        private float staggerImmuneUntil;
        private float legSlowUntil;
        private Coroutine reactionRoutine;
        private Coroutine corpseRoutine;
        private bool holdsRagdoll;
        private bool isDying;

        public bool IsCorpse => isDying;

        private static HitReactionBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.hitReaction : null;
        private EnemyData Data => health != null ? health.Data : null;

        private void Awake()
        {
            health = GetComponent<EnemyHealth>();
            attack = GetComponent<EnemyAttack>();
            agent = GetComponent<NavMeshAgent>();
            reactive = GetComponent<IHitReactive>();
            instance = GetComponent<EnemyInstance>();
            sunlight = GetComponent<SunlightExposure>();
            if (animator == null) animator = GetComponentInChildren<Animator>();
            if (renderers == null || renderers.Length == 0) renderers = GetComponentsInChildren<Renderer>(true);
            bones = GetComponentsInChildren<Rigidbody>(true);
            boneLayers = new int[bones.Length];
            for (int i = 0; i < bones.Length; i++) boneLayers[i] = bones[i].gameObject.layer;
            block = new MaterialPropertyBlock();
        }

        private void OnEnable()
        {
            health.Damaged += HandleDamaged;
            health.Died += HandleDied;
        }

        private void OnDisable()
        {
            health.Damaged -= HandleDamaged;
            health.Died -= HandleDied;
        }

        private bool IsBurning => sunlight != null && sunlight.IsBurning;

        private void HandleDamaged(DamageInfo info)
        {
            var b = Bal;
            if (b == null || isDying || IsBurning || !health.IsAlive) return;
            float staggerSeconds = b.staggerSeconds;

            // Efek bagian tubuh.
            if (info.bodyPart == BodyPart.Head) staggerSeconds *= b.headStaggerMultiplier;
            else if (info.bodyPart == BodyPart.Leg)
            {
                legSlowUntil = Time.time + b.legSlowSeconds;   // Diperpanjang, tidak bertumpuk
                reactive?.SetSpeedMultiplier(SpeedSource.LegHit, b.legSlowMultiplier);
            }

            switch (info.type)
            {
                case DamageType.Bullet:
                case DamageType.Melee:
                    float armor = DamageResistanceTable.GetMultiplier(Data != null ? Data.resistances : null, info.type, info.bodyPart);
                    if (armor < b.armorNoFlinchBelow)
                    {
                        // Badan Zombie Militer: tidak terhuyung, keluar percikan logam (#15). Menghadap balik arah serangan.
                        var vfx = VFXManager.Instance;
                        if (vfx != null)
                        {
                            Vector3 back = info.hitDirection.sqrMagnitude > 0.0001f ? -info.hitDirection : -transform.forward;
                            vfx.Play(MetalSparkTag, info.hitPoint, Quaternion.LookRotation(back));
                        }
                        break;
                    }
                    accumulatedPoise += info.amount;
                    if (Data != null && accumulatedPoise >= Data.poise)
                    {
                        if (info.type == DamageType.Melee) ApplyKnockback(info.hitDirection, b.meleeKnockback, staggerSeconds);
                        else ApplyStagger(staggerSeconds);
                    }
                    break;
                case DamageType.Explosion:
                    if (attack != null) attack.Interrupt(true);
                    ApplyKnockback(info.hitDirection, b.explosionKnockback, staggerSeconds);
                    break;
            }
        }

        private void Update()
        {
            // Efek kaki habis.
            if (legSlowUntil > 0f && Time.time >= legSlowUntil)
            {
                legSlowUntil = 0f;
                reactive?.SetSpeedMultiplier(SpeedSource.LegHit, 1f);
            }
        }

        public void ApplyStagger(float seconds) => ApplyStaggerInternal(seconds, false);

        private void ApplyStaggerInternal(float seconds, bool ignoreImmunity)
        {
            var b = Bal;
            if (b == null || Data == null || !Data.canBeStaggered || isDying) return;
            if (!ignoreImmunity && Time.time < staggerImmuneUntil) return;
            if (attack != null) attack.Interrupt(false);
            if (reactive != null && !reactive.RequestReaction(HitReactionKind.Stagger)) return;
            if (agent != null && agent.enabled && agent.isOnNavMesh) agent.isStopped = true;
            if (animator != null && animator.isActiveAndEnabled) animator.SetTrigger("Stagger");
            float end = Time.time + seconds;
            staggerImmuneUntil = end + b.staggerImmunitySeconds;
            accumulatedPoise = 0f;
            if (reactionRoutine != null) StopCoroutine(reactionRoutine);
            reactionRoutine = StartCoroutine(StaggerRoutine(end));
        }

        private IEnumerator StaggerRoutine(float end)
        {
            while (Time.time < end) yield return null;
            reactionRoutine = null;
            if (isDying) yield break;
            if (agent != null && agent.enabled && agent.isOnNavMesh) agent.isStopped = false;
            reactive?.EndReaction();
        }

        /// <summary>Dorong lewat NavMeshAgent.Move (bukan Rigidbody), lalu stagger. Nama sama dengan ShipHazardManager.md.</summary>
        public void ApplyKnockback(Vector3 dir, float distance, float staggerSeconds)
        {
            var b = Bal;
            if (b == null || Data == null || isDying) return;
            // 1-2. Musuh besar atau agen di link/mati: hanya stagger.
            if (!Data.canBeKnockedBack || agent == null || !agent.enabled || !agent.isOnNavMesh || agent.isOnOffMeshLink)
            {
                ApplyStagger(staggerSeconds);
                return;
            }
            // 3.
            if (attack != null) attack.Interrupt(false);
            if (reactive != null && !reactive.RequestReaction(HitReactionKind.Knockback)) return;
            agent.isStopped = true;
            agent.ResetPath();
            agent.updateRotation = false;
            if (animator != null && animator.isActiveAndEnabled) animator.SetTrigger("Knockback");
            // 4. Arah hampir nol: pakai arah dari pemain ke musuh.
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f && PlayerTargetable.Instance != null)
            {
                dir = transform.position - PlayerTargetable.Instance.Position;
                dir.y = 0f;
            }
            dir = dir.sqrMagnitude > 0.0001f ? dir.normalized : -transform.forward;
            Vector3 velocity = dir * distance * Data.knockbackMultiplier / Mathf.Max(0.01f, b.knockbackSeconds);
            if (reactionRoutine != null) StopCoroutine(reactionRoutine);
            reactionRoutine = StartCoroutine(KnockbackRoutine(velocity, b.knockbackSeconds, staggerSeconds));
        }

        private IEnumerator KnockbackRoutine(Vector3 velocity, float seconds, float staggerSeconds)
        {
            float t = 0f;
            while (t < seconds)
            {
                float dt = Time.deltaTime;
                t += dt;
                // 5. Kecepatan berkurang linier.
                if (agent.enabled && agent.isOnNavMesh) agent.Move(velocity * (1f - t / seconds) * dt);
                yield return null;
            }
            reactionRoutine = null;
            if (isDying) yield break;
            // 6.
            agent.updateRotation = true;
            if (!agent.isOnNavMesh)
            {
                var bal = GameManager.CurrentBalance;
                if (NavMesh.SamplePosition(transform.position, out var hit, bal != null ? bal.spawn.sampleRadius : 6f, agent.areaMask))
                    agent.Warp(hit.position);
                else { Despawn(DespawnReason.OffNavMesh); yield break; }
            }
            // 7. Pemeriksaan kebal stagger dilewati. Stagger meminta reaksi lagi (prioritas Stagger < Knockback),
            // jadi kembalikan dulu ke state biasa.
            reactive?.EndReaction();
            ApplyStaggerInternal(staggerSeconds, true);
        }

        // ---------------------------------------------------------------- Mati

        private void HandleDied(DamageInfo info)
        {
            if (IsBurning) return;   // SunlightExposure memegang urutan terbakar
            isDying = true;
            if (attack != null) attack.CancelAttack();
            reactive?.RequestReaction(HitReactionKind.Dying);
            if (reactionRoutine != null) { StopCoroutine(reactionRoutine); reactionRoutine = null; }
            if (agent != null && agent.enabled) agent.enabled = false;
            if (bodyCapsule != null) bodyCapsule.enabled = false;
            if (instance != null && instance.IsAnimal) instance.IsCarcass = true;

            // 2. Drop dari LootTable (#103).
            SpawnDrop(info);

            // 3. Ragdoll untuk ledakan atau melee kuat, kalau izin tersedia. Selain itu animasi mati.
            bool wantsRagdoll = info.type == DamageType.Explosion || info.type == DamageType.Melee;
            if (wantsRagdoll && bones.Length > 0 && CorpseBudget.TryAcquireRagdoll())
            {
                holdsRagdoll = true;
                EnableRagdoll(info);
            }
            else if (animator != null && animator.isActiveAndEnabled) animator.SetTrigger("Die");

            // 4-5.
            CorpseBudget.Register(this);
            corpseRoutine = StartCoroutine(CorpseRoutine());
        }

        /// <summary>Jumlah drop malam ini. Direset saat DaybreakEvent lewat CorpseBudget.FadeAllZombies dan saat Clear.</summary>
        internal static int dropsThisNight;

        /// <summary>EnemyHitReaction.md "Drop" (usulan): drop zombie dari LootTable, dibatasi maxDropsPerNight.</summary>
        private void SpawnDrop(DamageInfo info)
        {
            var data = Data;
            var b = Bal;
            // 1.
            if (data == null || data.dropTable == null || (b != null && dropsThisNight >= b.maxDropsPerNight)) return;
            // 2. Hanya kematian oleh pemain atau NPC (bukan matahari, Lucky Machine, atau bencana).
            if (!info.byPlayer && (info.source == null || !info.source.CompareTag("NPC"))) return;
            var seeds = RunSeedManager.Instance;
            var inv = InventoryManager.Instance;
            if (seeds == null || inv == null || WorldItemManager.Instance == null) return;
            // 3. Peluang dropChance dengan aliran Gameplay (#89).
            ref var stream = ref seeds.GetStream(RandomStreamId.Gameplay);
            if (stream.NextFloat01() >= data.dropChance) return;
            // 4.
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            if (!RegionLootTable.TryRoll(data.dropTable, LootPointKind.Any, day, ref stream, out var item, out int amount)) return;
            WorldItemManager.Instance.Spawn(inv.CreateInstance(item), amount, transform.position + Vector3.up * 0.3f, true);
            dropsThisNight++;
        }

        private void EnableRagdoll(DamageInfo info)
        {
            var b = Bal;
            if (animator != null) animator.enabled = false;
            int ragdollLayer = LayerMask.NameToLayer(PhysicsMasks.RagdollLayer);
            Rigidbody nearest = null;
            float best = float.MaxValue;
            for (int i = 0; i < bones.Length; i++)
            {
                if (ragdollLayer >= 0) bones[i].gameObject.layer = ragdollLayer;
                bones[i].isKinematic = false;
                float d = (bones[i].position - info.hitPoint).sqrMagnitude;
                if (d < best) { best = d; nearest = bones[i]; }
            }
            if (nearest != null && b != null) nearest.AddForceAtPosition(info.hitDirection * b.ragdollForce, info.hitPoint, ForceMode.Impulse);
        }

        private IEnumerator CorpseRoutine()
        {
            var b = Bal;
            float hold = b != null ? b.corpseHoldSeconds : 6f;
            float settle = b != null ? b.ragdollSettleSeconds : 2f;
            float t = 0f;
            bool skip = false;
            while (t < hold && !skip)
            {
                t += Time.deltaTime;
                if (holdsRagdoll && t >= settle) FreezeRagdoll();
                skip = fadeNow;
                yield return null;
            }
            if (holdsRagdoll) FreezeRagdoll();

            // 1. Hewan dengan bangkai: AnimalCarcass memegang sisa urutan (#61).
            if (Data != null && Data.leavesCarcass)
            {
                var carcass = GetComponent<AnimalCarcass>();
                if (carcass != null) { CorpseBudget.Remove(this); carcass.Activate(); corpseRoutine = null; yield break; }
            }

            // 2. Pudar lewat MaterialPropertyBlock.
            float fade = b != null ? b.corpseFadeSeconds : 1.5f;
            for (float f = 0f; f < fade; f += Time.deltaTime)
            {
                SetFade(1f - f / fade);
                yield return null;
            }
            corpseRoutine = null;
            // 3.
            Despawn(DespawnReason.Killed);
        }

        private bool fadeNow;

        /// <summary>CorpseBudget meminta mayat tertua langsung memudar.</summary>
        public void FadeNow() => fadeNow = true;

        private void FreezeRagdoll()
        {
            if (!holdsRagdoll) return;
            for (int i = 0; i < bones.Length; i++) bones[i].isKinematic = true;
            holdsRagdoll = false;
            CorpseBudget.ReleaseRagdoll();
        }

        private void SetFade(float alpha)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] == null) continue;
                renderers[i].GetPropertyBlock(block);
                block.SetFloat("_Fade", alpha);
                block.SetColor("_BaseColor", new Color(1f, 1f, 1f, alpha));
                renderers[i].SetPropertyBlock(block);
            }
        }

        private void Despawn(DespawnReason reason)
        {
            CorpseBudget.Remove(this);
            bool ship = GameManager.Instance != null && GameManager.Instance.currentPhase == GamePhase.SinkingShip;
            if (!ship && SpawnManager.Instance != null) SpawnManager.Instance.Despawn(gameObject, reason);
            else if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);   // Fase 1
        }

        // ---------------------------------------------------------------- Pool

        public void OnSpawned()
        {
            if (corpseRoutine != null) { StopCoroutine(corpseRoutine); corpseRoutine = null; }
            if (reactionRoutine != null) { StopCoroutine(reactionRoutine); reactionRoutine = null; }
            isDying = false;
            fadeNow = false;
            accumulatedPoise = 0f;
            staggerImmuneUntil = 0f;
            legSlowUntil = 0f;
            for (int i = 0; i < bones.Length; i++) { bones[i].isKinematic = true; bones[i].gameObject.layer = boneLayers[i]; }
            if (animator != null) { animator.enabled = true; animator.Rebind(); }
            if (bodyCapsule != null) bodyCapsule.enabled = true;
            for (int i = 0; i < renderers.Length; i++) if (renderers[i] != null) renderers[i].SetPropertyBlock(null);
            if (agent != null) agent.updateRotation = true;
            reactive?.SetSpeedMultiplier(SpeedSource.LegHit, 1f);
        }

        public void OnReturned()
        {
            CorpseBudget.Remove(this);
            if (holdsRagdoll) { holdsRagdoll = false; CorpseBudget.ReleaseRagdoll(); }
            if (corpseRoutine != null) { StopCoroutine(corpseRoutine); corpseRoutine = null; }
            if (reactionRoutine != null) { StopCoroutine(reactionRoutine); reactionRoutine = null; }
        }
    }
}
