using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Komponen panen pada pohon, batu, dan tanaman. Mengatur sisa pukulan, validasi alat, item jatuh, dan status habis.
    /// Blueprint: raft/ResourceNode.md dan raft/ResourceRespawn.md (#15, #16, #64, #73, #88, #89).
    ///
    /// GDD bagian 5: kayu dan batu di Region 1, material medis (jamur, getah) di Region 2.
    /// Node tidak pernah di-Destroy: saat habis hanya berganti status dan visual, sehingga tunggul tetap ber-collider.
    ///
    /// Catatan implementasi (Langkah 4):
    /// - Region node dari RegionSceneRoot induk, status lewat RegionStateStore (#88, RegionSaveRouting). Tanpa RegionStreaming
    ///   (scene uji): region dari Inspector, dan node mendaftar ke WorldStateSave (LoadOrder 0) sebagai jalur cadangan.
    /// - VFXManager.Play (#15), NotificationManager "Butuh alat lain" (#22), dan NoiseEmitter (#49) sudah disambung.
    ///   Belum ada (TODO): suara 3D lewat AudioManager (#16, sementara AudioSource opsional di node),
    ///   Flammable (#97).
    /// </summary>
    [DisallowMultipleComponent]
    public class ResourceNode : MonoBehaviour, ISaveable, IRegrowable
    {
        [SerializeField] private ResourceNodeData data;
        [Tooltip("Region node ini. Dibaca dari RegionSceneRoot induk kalau ada (#88); isian ini untuk scene uji.")]
        [SerializeField] private RegionType region = RegionType.Volcano;
        [SerializeField] private GameObject standingVisual;
        [SerializeField] private GameObject depletedVisual;
        [SerializeField] private Collider[] colliders;
        [SerializeField] private AudioClip hitSound;
        [SerializeField] private AudioClip breakSound;
        [SerializeField] private AudioClip rejectSound;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private float shakeDegrees = 3f;
        [SerializeField] private float shakeSeconds = 0.15f;

        public NodeStatus Status { get; private set; } = NodeStatus.Standing;
        public float RegrowAtGameHour { get; private set; } = -1f;
        public ResourceNodeData Data => data;
        public RegionType Region => region;
        public bool IsDepleted => Status == NodeStatus.Depleted;
        public Vector3 Position => transform.position;

        private int currentHits;
        private SaveIdTag saveTag;
        private bool registeredForSave;
        private float shakeTimer;
        private Quaternion visualBaseRotation;

        private static RandomStream fallbackStream = new RandomStream(0x5EEDUL);
        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            saveTag = GetComponent<SaveIdTag>();
            // Region dari RegionSceneRoot induk (#88). Isian Inspector hanya untuk scene uji tanpa RegionSceneRoot.
            if (RegionSceneRoot.TryGetRegionOf(this, out var r)) region = r;
            if (colliders == null || colliders.Length == 0) colliders = GetComponents<Collider>();
            currentHits = data != null ? data.maxHits : 1;
            if (standingVisual != null) visualBaseRotation = standingVisual.transform.localRotation;
            ApplyVisual();
        }

        private void OnEnable()
        {
            // Objek region: RegionStateStore (#88). Tanpa RegionStreaming: jalur cadangan WorldStateSave (RegionSaveRouting).
            if (saveTag != null) registeredForSave = RegionSaveRouting.Register(this, this);
            // Node habis yang dimuat ulang ikut jadwal tumbuh lagi.
            if (IsDepleted && ResourceRespawn.Instance != null) ResourceRespawn.Instance.MarkDepleted(this);
        }

        private void OnDisable()
        {
            RegionSaveRouting.Unregister(this, ref registeredForSave);
            if (ResourceRespawn.Instance != null) ResourceRespawn.Instance.Forget(this);
        }

        private void Update()
        {
            // Goyangan singkat saat dipukul. Tanpa coroutine supaya tanpa garbage.
            if (shakeTimer <= 0f || standingVisual == null) return;
            shakeTimer -= Time.deltaTime;
            float t = Mathf.Clamp01(shakeTimer / Mathf.Max(0.01f, shakeSeconds));
            float angle = Mathf.Sin(t * Mathf.PI * 4f) * shakeDegrees * t;
            standingVisual.transform.localRotation = visualBaseRotation * Quaternion.Euler(angle, 0f, angle * 0.5f);
            if (shakeTimer <= 0f) standingVisual.transform.localRotation = visualBaseRotation;
        }

        // ---------------------------------------------------------------- Memanen

        /// <summary>
        /// Dipanggil MeleeCombatSystem setelah PhysicsMasks.Melee mengenai node (byPlayer = true), atau NPC pekerja (#76, false).
        /// MeleeCombatSystem mengurangi durabilitas hanya saat hasilnya Hit.
        /// </summary>
        public HitResult ReceiveHit(ToolClass tool, Vector3 hitPoint, bool byPlayer = true)
        {
            if (data == null) return HitResult.Rejected;

            // 1. Node habis: tolak, suara tumpul.
            if (Status == NodeStatus.Depleted)
            {
                PlaySound(rejectSound);
                return HitResult.Rejected;
            }

            // 2. Alat tidak cocok: tolak (usulan disetujui: batu butuh Kapak/Beliung, pohon getah butuh Pisau).
            int damage = data.GetDamage(tool);
            if (damage <= 0)
            {
                PlaySound(rejectSound);
                // Notifikasi "Butuh alat lain" (#22).
                if (byPlayer && NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.need_tool", NotificationKind.Warning);
                return HitResult.Rejected;
            }

            // 3. Kurangi pukulan, lalu efek, suara, dan goyangan.
            currentHits -= damage;
            // Efek pukulan lewat VFXManager (#15).
            if (VFXManager.Instance != null && !string.IsNullOrEmpty(data.HitVfxTag))
            {
                Vector3 away = hitPoint - transform.position;
                VFXManager.Instance.Play(data.HitVfxTag, hitPoint, away.sqrMagnitude > 0.0001f ? Quaternion.LookRotation(away) : Quaternion.identity);
            }
            PlaySound(hitSound);
            shakeTimer = shakeSeconds;

            // 4. Event dan suara panen. Keputusan developer: hewan buas menjaga resource lewat suara Tool (WildlifeAI.md).
            bool willDeplete = currentHits <= 0;
            EventChannel<ResourceNodeHitEvent>.Publish(new ResourceNodeHitEvent(data.nodeType, willDeplete, byPlayer));
            if (byPlayer)
            {
                // Suara panen memancing hewan penjaga (#49, #61).
                NoiseEmitter.Emit(hitPoint, Balance != null ? Balance.resource.harvestNoiseRadius : 10f, NoiseSourceKind.Tool);
            }

            // 5. Habis.
            if (willDeplete) Deplete(true);
            return HitResult.Hit;
        }

        private void Deplete(bool withDrops)
        {
            // Tanpa RunSeedManager (scene uji) memakai aliran cadangan, supaya tidak error.
            ref var stream = ref (RunSeedManager.Instance != null
                ? ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay)
                : ref fallbackStream);

            // 1-2. Jumlah drop dari aliran Gameplay (#89), supaya seed sama menghasilkan drop sama.
            if (withDrops && data.dropItem != null)
            {
                GetDropRange(out int min, out int max);
                int total = stream.Range(min, max + 1);
                SpawnDrops(total, ref stream);
            }

            // 3. Ganti status dan visual. Collider tetap kalau keepColliderWhenDepleted (tunggul, puing).
            Status = NodeStatus.Depleted;
            ApplyVisual();
            if (standingVisual != null) standingVisual.transform.localRotation = visualBaseRotation;
            shakeTimer = 0f;

            // 4. Jadwal tumbuh: jam absolut + hari region + jitter. Region tanpa aturan: tidak tumbuh (-1).
            if (data.TryGetRegrowDays(region, out float days) && days > 0f)
            {
                float jitter = Balance != null ? Balance.resource.regrowJitterHours : 6f;
                RegrowAtGameHour = ItemUseSystem.CurrentTotalGameHours() + days * 24f + stream.Range(0f, jitter);
            }
            else
            {
                RegrowAtGameHour = -1f;
            }

            // 5. Daftar ke ResourceRespawn, lalu suara.
            if (ResourceRespawn.Instance != null) ResourceRespawn.Instance.MarkDepleted(this);
            PlaySound(breakSound);
        }

        private void GetDropRange(out int min, out int max)
        {
            min = data.dropMin;
            max = data.dropMax;
            if (Balance == null) return;
            if (data.nodeType == NodeType.Tree) { min = Balance.resource.treeDropMin; max = Balance.resource.treeDropMax; }
            else if (data.nodeType == NodeType.Rock) { min = Balance.resource.rockDropMin; max = Balance.resource.rockDropMax; }
            if (max < min) max = min;
        }

        /// <summary>Item jatuh dibagi per dropChunkSize, diambil dari pool WorldItem_Generic (bukan Instantiate).</summary>
        private void SpawnDrops(int total, ref RandomStream stream)
        {
            if (total <= 0 || WorldItemManager.Instance == null || InventoryManager.Instance == null) return;
            int chunk = Balance != null ? Mathf.Max(1, Balance.resource.dropChunkSize) : 4;
            float impulse = Balance != null ? Balance.resource.dropImpulse : 1.5f;
            Vector3 safePoint = transform.position + Vector3.up * 0.5f;

            while (total > 0)
            {
                int amount = Mathf.Min(chunk, total);
                total -= amount;
                var inst = InventoryManager.Instance.CreateInstance(data.dropItem);
                Vector2 offset = new Vector2(stream.Range(-0.6f, 0.6f), stream.Range(-0.6f, 0.6f));
                Vector3 pos = safePoint + new Vector3(offset.x, 0.5f, offset.y);
                var item = WorldItemManager.Instance.Spawn(inst, amount, pos, true);
                if (item == null) continue;
                item.Push(new Vector3(offset.x, 1f, offset.y).normalized * impulse);
                var guard = item.GetComponent<ResourceDropGuard>();
                if (guard != null) guard.Arm(safePoint);
            }
        }

        /// <summary>Bencana (#70) dan api (#97) menghabiskan node. withDrops = false untuk api.</summary>
        public void ForceDeplete(bool withDrops)
        {
            if (data == null || Status == NodeStatus.Depleted) return;
            currentHits = 0;
            Deplete(withDrops);
        }

        /// <summary>Tumbuh kembali: utuh, pukulan penuh, visual dan collider kembali.</summary>
        public void Regrow()
        {
            Status = NodeStatus.Standing;
            currentHits = data != null ? data.maxHits : 1;
            RegrowAtGameHour = -1f;
            ApplyVisual();
        }

        private void ApplyVisual()
        {
            bool standing = Status == NodeStatus.Standing;
            if (standingVisual != null) standingVisual.SetActive(standing);
            if (depletedVisual != null) depletedVisual.SetActive(!standing);
            bool colliderOn = standing || (data != null && data.keepColliderWhenDepleted);
            if (colliders != null)
                for (int i = 0; i < colliders.Length; i++)
                    if (colliders[i] != null) colliders[i].enabled = colliderOn;
        }

        private void PlaySound(AudioClip clip)
        {
            // TODO #16: AudioManager.Instance.PlaySFXAt(clip, transform.position). Sementara AudioSource opsional.
            if (clip != null && audioSource != null) audioSource.PlayOneShot(clip);
        }

        // ---------------------------------------------------------------- Simpan dan muat (#80)

        [Serializable]
        private class NodeSaveData
        {
            public NodeStatus status;
            public int currentHits;
            public float regrowAtGameHour;
        }

        public string SaveId => saveTag != null ? saveTag.SaveId : name;
        public int LoadOrder => 0;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new NodeSaveData { status = Status, currentHits = currentHits, regrowAtGameHour = RegrowAtGameHour });
        }

        public void RestoreState(string json)
        {
            NodeSaveData d = null;
            try { d = JsonUtility.FromJson<NodeSaveData>(json); }
            catch (Exception e) { Debug.LogError($"[ResourceNode] '{SaveId}' gagal dibaca: {e.Message}"); }
            if (d == null) return;
            Status = d.status;
            currentHits = d.currentHits;
            RegrowAtGameHour = d.regrowAtGameHour;
            ApplyVisual();
        }

        public void OnRestoreFinished()
        {
            // Node habis mendaftar lagi. Waktu yang sudah lewat langsung diperiksa ResourceRespawn.
            if (IsDepleted && ResourceRespawn.Instance != null) ResourceRespawn.Instance.MarkDepleted(this);
        }
    }
}
