using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace SurvivalIsland
{
    public enum DoorState { Closed, Open, Locked }

    [Serializable]
    public class BaseDoorSaveData
    {
        public int state;
    }

    /// <summary>
    /// Pintu base. Blueprint: raft/BaseNavigation.md bagian "BaseDoor" (#78). GDD bagian 4: base sebagai tempat berlindung NPC.
    /// Usulan disetujui developer (#85): gerendel mengunci pintu, pintu terkunci menahan NPC dan menerima 75 persen damage,
    /// NPC boleh lewat pintu tertutup yang tidak terkunci, lalu pintu menutup sendiri.
    /// Status disimpan lewat StructureRecord.extraJson (StructureRegistry memanggil ISaveable di prefab); TIDAK mendaftar
    /// ke WorldStateSave.
    /// </summary>
    [RequireComponent(typeof(BaseHealth))]
    public class BaseDoor : MonoBehaviour, IInteractable, ISaveable
    {
        [Tooltip("Daun pintu yang berputar.")]
        [SerializeField] private Transform leafPivot;
        [Tooltip("Menutup lubang pintu saat tertutup atau terkunci.")]
        [SerializeField] private NavMeshObstacle leafObstacle;
        [Tooltip("Lewat pintu tertutup, hanya untuk NPC (Area DoorLink).")]
        [SerializeField] private NavMeshLink npcLink;
        [Tooltip("Sudut daun saat terbuka (derajat). Tanda dipilih supaya pintu berputar menjauhi pemain.")]
        [SerializeField] private float openAngle = 90f;
        [Header("Volume pemeriksaan (ruang lokal pintu)")]
        [SerializeField] private Vector3 doorwayCenter = new Vector3(0f, 1f, 0f);
        [SerializeField] private Vector3 doorwayHalfExtents = new Vector3(0.7f, 1f, 0.3f);
        [SerializeField] private Vector3 swingCenter = new Vector3(0f, 1f, 0.7f);
        [SerializeField] private Vector3 swingHalfExtents = new Vector3(0.7f, 1f, 0.7f);
        [Header("Teks dan suara")]
        [SerializeField] private string openPrompt = "Buka";
        [SerializeField] private string closePrompt = "Tutup";
        [SerializeField] private string lockedPrompt = "Terkunci";
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip latchClip;

        public DoorState State { get; private set; } = DoorState.Closed;
        public NavMeshObstacle LeafObstacle => leafObstacle;
        public bool IsNpcPassing => npcPassing;

        private BaseHealth health;
        private Quaternion closedRotation;
        private float currentSign = 1f;
        private Coroutine swing;
        private bool npcPassing;
        private bool leafOpenForNpc;
        private float npcCloseAt = -1f;
        private static readonly Collider[] overlap = new Collider[4];

        private static BaseNavBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.baseNav : null;
        public string StructureId => health != null ? health.StructureId : string.Empty;

        private void Awake()
        {
            health = GetComponent<BaseHealth>();
            if (leafPivot != null) closedRotation = leafPivot.localRotation;
            ApplyNavigation();
        }

        // ---------------------------------------------------------------- IInteractable

        public bool IsAvailable => isActiveAndEnabled && health != null && health.IsAlive;
        public float HoldSeconds => 0f;

        public string GetPrompt()
        {
            switch (State)
            {
                case DoorState.Open: return closePrompt;
                case DoorState.Locked: return lockedPrompt;
                default: return openPrompt;
            }
        }

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            // Pintu tetap boleh dipakai saat membawa Heavy Item (IslandInteractable.md).
            return State == DoorState.Locked ? InteractBlockReason.Locked : InteractBlockReason.None;
        }

        public void OnHoverEnter() { }
        public void OnHoverExit() { }

        public bool Interact(in InteractContext ctx)
        {
            if (State == DoorState.Locked) { PlayLatch(); return false; }
            return Toggle(ctx.Player != null ? ctx.Player.position : transform.position + transform.forward);
        }

        /// <summary>Closed ke Open (busur kosong), Open ke Closed (lubang kosong). Locked ditolak.</summary>
        public bool Toggle(Vector3 userPosition)
        {
            // Kasus Khusus: ditolak selama NPC sedang lewat.
            if (npcPassing) return false;
            switch (State)
            {
                case DoorState.Closed:
                    currentSign = Vector3.Dot(userPosition - transform.position, transform.forward) >= 0f ? -1f : 1f;
                    if (IsAreaBlocked(SwingCenterWorld(), swingHalfExtents)) { NotifyBlocked(); return false; }
                    SetState(DoorState.Open, true);
                    return true;
                case DoorState.Open:
                    if (IsAreaBlocked(transform.TransformPoint(doorwayCenter), doorwayHalfExtents)) { NotifyBlocked(); return false; }
                    SetState(DoorState.Closed, true);
                    return true;
                default:
                    PlayLatch();
                    return false;
            }
        }

        /// <summary>Gerendel (BaseDoorLatch): Closed ke Locked, Locked ke Closed. Tidak tersedia saat Open.</summary>
        public bool ToggleLock()
        {
            if (npcPassing) return false;
            if (State == DoorState.Closed) { SetState(DoorState.Locked, true); PlayLatch(); return true; }
            if (State == DoorState.Locked) { SetState(DoorState.Closed, true); PlayLatch(); return true; }
            return false;
        }

        // ---------------------------------------------------------------- Status

        private void SetState(DoorState next, bool animate)
        {
            State = next;
            // Pintu terkunci menerima lockedDamageMultiplier dari damage Structural (usulan 2).
            var b = Bal;
            if (health != null) health.SetDamageMultiplier(DamageType.Structural, next == DoorState.Locked && b != null ? b.lockedDamageMultiplier : 1f);
            RotateLeaf(next == DoorState.Open, animate);
            ApplyNavigation();
            EventChannel<DoorStateChangedEvent>.Publish(new DoorStateChangedEvent(StructureId, next));
        }

        /// <summary>Tabel "NavMesh pintu dan akses NPC".</summary>
        private void ApplyNavigation()
        {
            if (leafObstacle != null)
            {
                leafObstacle.enabled = State != DoorState.Open;
                leafObstacle.carving = true;
                leafObstacle.carveOnlyStationary = false;
            }
            if (npcLink != null) npcLink.enabled = State == DoorState.Closed;
        }

        private void RotateLeaf(bool open, bool animate)
        {
            if (leafPivot == null) return;
            Quaternion target = open ? closedRotation * Quaternion.Euler(0f, openAngle * currentSign, 0f) : closedRotation;
            if (swing != null) StopCoroutine(swing);
            swing = null;
            if (!animate || !isActiveAndEnabled) { leafPivot.localRotation = target; return; }
            swing = StartCoroutine(SwingRoutine(target));
        }

        private IEnumerator SwingRoutine(Quaternion target)
        {
            var b = Bal;
            float duration = b != null ? Mathf.Max(0.01f, b.doorSwingSeconds) : 0.4f;
            Quaternion from = leafPivot.localRotation;
            float t = 0f;
            // Waktu berskala: berhenti saat game dijeda.
            while (t < duration)
            {
                t += Time.deltaTime;
                leafPivot.localRotation = Quaternion.Slerp(from, target, Mathf.Clamp01(t / duration));
                yield return null;
            }
            leafPivot.localRotation = target;
            swing = null;
        }

        private Vector3 SwingCenterWorld()
        {
            var c = swingCenter;
            c.z *= -currentSign;   // Busur di sisi tempat daun berputar
            return transform.TransformPoint(c);
        }

        private bool IsAreaBlocked(Vector3 center, Vector3 halfExtents)
        {
            // Daun pintu tidak pernah menembus pemain, NPC, atau zombie.
            return PhysicsQuery.OverlapBox(center, halfExtents, transform.rotation, PhysicsMasks.Occupant, overlap) > 0;
        }

        private void NotifyBlocked()
        {
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.door.blocked", NotificationKind.Info);
        }

        private void PlayLatch()
        {
            if (audioSource != null && latchClip != null) audioSource.PlayOneShot(latchClip);
        }

        // ---------------------------------------------------------------- NPC lewat pintu tertutup

        /// <summary>
        /// NPC di npcLink meminta lewat (#76). Daun terbuka, tetapi State tetap Closed untuk NavMesh sampai NPC selesai,
        /// jadi jalur zombie tidak berubah. true kalau daun sudah terbuka dan NPC boleh berjalan.
        /// </summary>
        public bool RequestNpcPass(Component npc)
        {
            if (State != DoorState.Closed) return State == DoorState.Open;
            if (!npcPassing)
            {
                npcPassing = true;
                leafOpenForNpc = false;
                currentSign = npc != null && Vector3.Dot(npc.transform.position - transform.position, transform.forward) >= 0f ? -1f : 1f;
                RotateLeaf(true, true);
                if (leafObstacle != null) leafObstacle.enabled = false;
            }
            if (!leafOpenForNpc && swing == null) leafOpenForNpc = true;
            return leafOpenForNpc;
        }

        /// <summary>NPC selesai melintas (setelah CompleteOffMeshLink). Pintu menutup setelah doorNpcAutoCloseSeconds.</summary>
        public void NotifyNpcPassed()
        {
            if (!npcPassing) return;
            var b = Bal;
            npcCloseAt = Time.time + (b != null ? b.doorNpcAutoCloseSeconds : 1.5f);
        }

        private void Update()
        {
            if (!npcPassing || npcCloseAt < 0f || Time.time < npcCloseAt) return;
            // Lubang pintu belum kosong: tunggu.
            if (IsAreaBlocked(transform.TransformPoint(doorwayCenter), doorwayHalfExtents)) return;
            npcPassing = false;
            leafOpenForNpc = false;
            npcCloseAt = -1f;
            RotateLeaf(false, true);
            ApplyNavigation();
        }

        // ---------------------------------------------------------------- Save lewat StructureRecord.extraJson

        public string SaveId => "door_" + StructureId;
        public int LoadOrder => 0;

        public string CaptureState() => JsonUtility.ToJson(new BaseDoorSaveData { state = (int)State });

        public void RestoreState(string json)
        {
            var d = string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<BaseDoorSaveData>(json);
            if (d == null) return;
            // Setelah load: obstacle dan link mengikuti status, tanpa animasi dan tanpa event.
            State = (DoorState)Mathf.Clamp(d.state, 0, 2);
            RotateLeaf(State == DoorState.Open, false);
            ApplyNavigation();
        }

        public void OnRestoreFinished()
        {
            var b = Bal;
            if (health != null) health.SetDamageMultiplier(DamageType.Structural, State == DoorState.Locked && b != null ? b.lockedDamageMultiplier : 1f);
        }

        /// <summary>Upgrade pintu (#47): status dipindahkan ke objek baru tanpa animasi.</summary>
        public void CopyStateFrom(BaseDoor other)
        {
            if (other == null) return;
            RestoreState(other.CaptureState());
            OnRestoreFinished();
        }
    }
}
