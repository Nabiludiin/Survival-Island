using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Tahap alur Fase 1 (GLOSSARY.md, pemilik LifeboatExtraction #74).</summary>
    public enum ShipFlowState { PreGame, Scavenge, Resolving, Summary, Finished }

    /// <summary>
    /// Sekoci: titik aman dan titik setor, jeda pra-game, hasil saat waktu habis, dan ringkasan Fase 1.
    /// Blueprint: 60-seconds/LifeboatExtraction.md (#74).
    /// GDD bagian 3: Fase 1, Base Item, prioritas barang dan NPC, dan kembali ke Fase 1 saat kalah.
    /// Sekoci, aturan setor, dan akibat gagal adalah usulan yang disetujui developer (#85).
    ///
    /// Catatan implementasi:
    /// - SpawnShipNpcs: keputusan developer (sistem pendukung): NPCData di ShipPhase diambil dari daftar npcDatabase di
    ///   Inspector (aset yang sama dengan NPCRoster), dan titik dari shipNpcPoints sesuai RunLayout.shipNpcPointByNpc.
    /// Penanda sekoci di HUD sejak sirene (HUDManager.SetLifeboatHighlighted) dan isi sekoci setelah setor
    /// (HUDManager.ShowLifeboatContents) sudah tersambung.
    /// Sirene pra-game dan SFX setor lewat AudioManager, teks prompt lewat LocalizationManager (#104).
    /// </summary>
    public class LifeboatExtraction : MonoBehaviour, IInteractable
    {
        private static readonly ShipBalance FallbackShip = new ShipBalance();

        public static LifeboatExtraction Instance { get; private set; }

        [SerializeField] private Transform safePoint;
        [Tooltip("Keputusan developer: aset NPCData yang sama dengan NPCRoster.database, dicari lewat npcId.")]
        [SerializeField] private NPCData[] npcDatabase = new NPCData[0];
        [Tooltip("Titik NPC di kapal. Indeks dari RunLayout.shipNpcPointByNpc (#89).")]
        [SerializeField] private Transform[] shipNpcPoints = new Transform[0];
        [SerializeField] private RunSummaryPanel runSummaryPanel;
        [SerializeField] private InteractHighlight highlight;

        [Header("Suara (AudioManager)")]
        [Tooltip("Sirene pada jeda pra-game (ship.sirenAtSeconds).")]
        [SerializeField] private AudioClip sirenClip;
        [Tooltip("SFX saat barang disetor ke sekoci.")]
        [SerializeField] private AudioClip depositClip;

        [Header("Pemain dan sistem scene")]
        [SerializeField] private ShipPlayerController playerController;
        [SerializeField] private SurvivalStats playerStats;

        public ShipFlowState State { get; private set; } = ShipFlowState.PreGame;
        public Transform SafePoint => safePoint;

        /// <summary>Isi sekoci. Dibaca RunSummaryPanel.</summary>
        public IReadOnlyList<SavedItemData> DepositedItems => depositedItems;
        public IReadOnlyList<string> RescuedNpcIds => rescuedNpcIds;

        /// <summary>Hasil terakhir yang disusun ResolveOutcome. Dibaca RunSummaryPanel.</summary>
        public ShipOutcomeData LastOutcome { get; private set; }

        private readonly List<SavedItemData> depositedItems = new List<SavedItemData>();
        private readonly List<string> rescuedNpcIds = new List<string>();

        private static ShipBalance Ship
        {
            get { var b = GameManager.CurrentBalance; return b != null ? b.ship : FallbackShip; }
        }

        private static bool RunEnding => GameManager.Instance != null && GameManager.Instance.IsRunEnding;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("[LifeboatExtraction] Sudah ada satu di scene. Komponen ini dihapus.", this);
                Destroy(this);
                return;
            }
            Instance = this;
            if (playerController == null) playerController = FindAnyObjectByType<ShipPlayerController>();
            if (playerStats == null && playerController != null) playerStats = playerController.GetComponent<SurvivalStats>();
            if (highlight == null) highlight = GetComponent<InteractHighlight>();
            if (safePoint == null) safePoint = transform;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable() { EventChannel<RunEndedEvent>.Raised += HandleRunEnded; }
        private void OnDisable() { EventChannel<RunEndedEvent>.Raised -= HandleRunEnded; }

        private void Start()
        {
            SpawnShipNpcs();
            StartCoroutine(PreGameRoutine());
        }

        // ---------------------------------------------------------------- Jeda pra-game

        /// <summary>Dimulai saat RunLifecycle.State == InShip dan panel Cinematic (#99) sudah tertutup.</summary>
        private IEnumerator PreGameRoutine()
        {
            // Tunggu RunLifecycle selesai memuat kapal. Tanpa RunLifecycle (uji scene langsung) langsung lanjut.
            while (RunLifecycle.Instance != null && RunLifecycle.Instance.State != RunState.InShip) yield return null;
            while (IsCinematicOpen()) yield return null;
            if (RunEnding) yield break;

            // 1-3.
            State = ShipFlowState.PreGame;
            if (playerController != null) playerController.SetControlLevel(ShipControlLevel.LookOnly);
            // Lapar dan haus tidak berjalan di kapal (#111).
            if (playerStats != null) playerStats.SetDrainEnabled(false);
            // 4. Timer menampilkan 60 tanpa berjalan (ShipTimerManager.ShowIdle di Awake).

            // 5-6. Semua penantian memakai Time.deltaTime, jadi Pause ikut menahannya.
            var ship = Ship;
            float elapsed = 0f;
            bool sirenPlayed = false;
            while (elapsed < ship.preGameSeconds)
            {
                elapsed += Time.deltaTime;
                if (!sirenPlayed && elapsed >= ship.sirenAtSeconds)
                {
                    sirenPlayed = true;
                    // Sirene lewat grup Warning, lalu sekoci ditandai di HUD (langkah 5).
                    AudioManager.Instance?.PlayWarning(sirenClip);
                    HUDManager.Instance?.SetLifeboatHighlighted(true);
                }
                if (RunEnding) yield break;
                yield return null;
            }

            // 7.
            if (playerController != null) playerController.SetControlLevel(ShipControlLevel.Full);
            if (ShipTimerManager.Instance != null) ShipTimerManager.Instance.StartCountdown();
            else Debug.LogError("[LifeboatExtraction] ShipTimerManager tidak ada di ShipPhase.", this);
            if (ShipHazardManager.Instance != null) ShipHazardManager.Instance.Begin();
            State = ShipFlowState.Scavenge;
        }

        private static bool IsCinematicOpen()
        {
            var ui = UIManager.Instance;
            var focus = UIFocusStack.Instance;
            return ui != null && focus != null && ui.cinematicPanel != null && focus.Contains(ui.cinematicPanel);
        }

        // ---------------------------------------------------------------- NPC

        private void SpawnShipNpcs()
        {
            // LifeboatExtraction.md "Penempatan": NPC dari RunSeedManager.CurrentLayout.shipNpcIds di titik shipNpcPointByNpc.
            // Data langsung diisi lewat Setup(npcData), bukan di OnSpawned.
            var layout = RunSeedManager.Instance != null ? RunSeedManager.Instance.CurrentLayout : null;
            var pooler = ObjectPooler.Instance;
            if (layout == null || layout.shipNpcIds == null || pooler == null) return;
            for (int i = 0; i < layout.shipNpcIds.Length; i++)
            {
                var data = FindNpcData(layout.shipNpcIds[i]);
                if (data == null)
                {
                    Debug.LogWarning($"[LifeboatExtraction] NPCData '{layout.shipNpcIds[i]}' tidak ada di npcDatabase.", this);
                    continue;
                }
                int point = layout.shipNpcPointByNpc != null && i < layout.shipNpcPointByNpc.Length ? layout.shipNpcPointByNpc[i] : i;
                if (point < 0 || point >= shipNpcPoints.Length || shipNpcPoints[point] == null)
                {
                    Debug.LogWarning($"[LifeboatExtraction] Titik NPC {point} untuk '{data.npcId}' tidak ada di shipNpcPoints.", this);
                    continue;
                }
                var t = shipNpcPoints[point];
                var npc = pooler.SpawnFromPool<ShipInteractable>("ShipNpc_Generic", t.position, t.rotation);
                if (npc != null) npc.Setup(data);
            }
        }

        private NPCData FindNpcData(string npcId)
        {
            if (string.IsNullOrEmpty(npcId) || npcDatabase == null) return null;
            for (int i = 0; i < npcDatabase.Length; i++)
                if (npcDatabase[i] != null && npcDatabase[i].npcId == npcId) return npcDatabase[i];
            return null;
        }

        // ---------------------------------------------------------------- IInteractable (sekoci)

        public bool IsAvailable => State == ShipFlowState.Scavenge && isActiveAndEnabled;

        /// <summary>0 kalau tangan berisi (setor langsung), departHoldSeconds kalau kosong (berangkat).</summary>
        public float HoldSeconds => HandWeight() > 0 ? 0f : Ship.departHoldSeconds;

        public string GetPrompt()
        {
            // Kunci teks #104: "Setor Barang" / "Tahan untuk Berangkat".
            return UIManager.Localize(HandWeight() > 0 ? "prompt.lifeboat_deposit" : "prompt.lifeboat_depart");
        }

        public InteractBlockReason CanInteract(in InteractContext ctx)
        {
            return State == ShipFlowState.Scavenge ? InteractBlockReason.None : InteractBlockReason.Locked;
        }

        public void OnHoverEnter()
        {
            if (highlight != null) highlight.SetState(State == ShipFlowState.Scavenge ? HighlightState.Available : HighlightState.Blocked);
        }

        public void OnHoverExit()
        {
            if (highlight != null) highlight.SetState(HighlightState.Off);
        }

        public bool Interact(in InteractContext ctx)
        {
            if (CanInteract(ctx) != InteractBlockReason.None) return false;
            // Tangan berisi: setor. Tangan kosong (setelah ditahan departHoldSeconds): berangkat.
            if (HandWeight() > 0) DepositHand();
            else RequestDepart();
            return true;
        }

        private static int HandWeight()
        {
            var inv = InventoryManager.Instance;
            return inv != null ? inv.ShipHandWeight : 0;
        }

        /// <summary>Aturan setor pola 60 Seconds: seluruh tangan kosong sekaligus. Barang di sekoci aman dari bahaya.</summary>
        private void DepositHand()
        {
            var inv = InventoryManager.Instance;
            if (inv == null) return;

            var hand = inv.ShipHandItems;
            for (int i = 0; i < hand.Count; i++)
            {
                var entry = hand[i];
                if (entry.isNpc)
                {
                    if (!rescuedNpcIds.Contains(entry.itemId))
                    {
                        rescuedNpcIds.Add(entry.itemId);
                        EventChannel<NPCRescuedEvent>.Publish(new NPCRescuedEvent(entry.itemId));
                    }
                }
                else
                {
                    AddDeposited(entry.itemId, entry.amount);
                }
            }

            inv.ClearShipHand();
            AudioManager.Instance?.PlaySFX(depositClip);
            // Langkah setor 3: isi sekoci di HUD.
            int itemTotal = 0;
            for (int i = 0; i < depositedItems.Count; i++) itemTotal += depositedItems[i].amount;
            HUDManager.Instance?.ShowLifeboatContents(itemTotal, rescuedNpcIds.Count);
        }

        private void AddDeposited(string itemId, int amount)
        {
            for (int i = 0; i < depositedItems.Count; i++)
            {
                if (depositedItems[i].itemId != itemId) continue;
                var merged = depositedItems[i];
                merged.amount += amount;
                depositedItems[i] = merged;
                return;
            }
            depositedItems.Add(new SavedItemData { itemId = itemId, amount = amount });
        }

        // ---------------------------------------------------------------- Waktu habis dan berangkat

        /// <summary>Dipanggil ShipTimerManager.ExecuteTimeUp setelah FlushScheduleAtTimeUp.</summary>
        public void HandleTimeUp()
        {
            // Mati pada frame yang sama: TriggerGameOver sudah memasang IsRunEnding.
            if (State != ShipFlowState.Scavenge || RunEnding) return;

            State = ShipFlowState.Resolving;
            if (playerController != null) playerController.SetControlLevel(ShipControlLevel.None);

            if (IsPlayerInSafeZone())
            {
                // Usulan disetujui (#85): barang di tangan ikut kalau pemain di area aman.
                DepositHand();
                ResolveOutcome(departedEarly: false);
            }
            else
            {
                // Usulan disetujui (#85): gagal mencapai sekoci = permadeath langsung (GDD bagian 3: kalah kembali ke Fase 1).
                State = ShipFlowState.Finished;
                if (ShipHazardManager.Instance != null) ShipHazardManager.Instance.StopAll();
                if (GameManager.Instance != null) GameManager.Instance.TriggerGameOver(DeathReason.LeftOnShip);
            }
        }

        /// <summary>Jarak langsung, bukan trigger, supaya hasilnya pasti pada frame waktu habis.</summary>
        private bool IsPlayerInSafeZone()
        {
            if (playerController == null || safePoint == null) return false;
            var ship = Ship;
            Vector3 d = playerController.transform.position - safePoint.position;
            if (Mathf.Abs(d.y) > ship.lifeboatSafeHeight) return false;
            return new Vector2(d.x, d.z).sqrMagnitude <= ship.lifeboatSafeRadius * ship.lifeboatSafeRadius;
        }

        /// <summary>
        /// Keputusan developer: 60 detik adalah batas paling lama, pemain boleh berangkat lebih awal.
        /// Tangan harus kosong dan E ditahan departHoldSeconds. Barang dan NPC yang belum disetor tertinggal.
        /// </summary>
        private void RequestDepart()
        {
            State = ShipFlowState.Resolving;
            if (playerController != null) playerController.SetControlLevel(ShipControlLevel.None);
            if (ShipTimerManager.Instance != null) ShipTimerManager.Instance.StopCountdown();
            ResolveOutcome(departedEarly: true);
        }

        private void ResolveOutcome(bool departedEarly)
        {
            // 1. Serangan pada frame terakhir tidak mengubah hasil.
            if (playerStats != null) playerStats.SetInvulnerable(true);
            // 2.
            if (ShipHazardManager.Instance != null) ShipHazardManager.Instance.StopAll();

            // 3-4.
            var outcome = new ShipOutcomeData();
            outcome.rescuedNpcIds.AddRange(rescuedNpcIds);
            FillLostNpcs(outcome.lostNpcIds);
            outcome.hpOnExit = playerStats != null ? playerStats.HP : 0f;
            outcome.infectionOnExit = playerStats != null ? playerStats.Infection : 0f;
            outcome.secondsLeftOnExit = ShipTimerManager.Instance != null ? ShipTimerManager.Instance.SecondsLeft : 0f;
            outcome.departedEarly = departedEarly;
            LastOutcome = outcome;

            // 5. GameManager menyimpan hasil lintas scene.
            if (GameManager.Instance != null) GameManager.Instance.SaveShipOutcome(new List<SavedItemData>(depositedItems), outcome);

            // 6.
            State = ShipFlowState.Summary;
            if (runSummaryPanel != null && UIFocusStack.Instance != null) UIFocusStack.Instance.Push(runSummaryPanel);
            else
            {
                Debug.LogError("[LifeboatExtraction] Panel RunSummary tidak ada. Langsung ke pulau.", this);
                ContinueToIsland();
            }
        }

        /// <summary>NPC di shipNpcIds yang tidak disetor (termasuk yang masih dibawa) menjadi korban.</summary>
        private void FillLostNpcs(List<string> lost)
        {
            var seeds = RunSeedManager.Instance;
            var layout = seeds != null ? seeds.CurrentLayout : null;
            if (layout == null || layout.shipNpcIds == null) return;
            for (int i = 0; i < layout.shipNpcIds.Length; i++)
            {
                string id = layout.shipNpcIds[i];
                if (!string.IsNullOrEmpty(id) && !rescuedNpcIds.Contains(id)) lost.Add(id);
            }
        }

        /// <summary>Tombol Lanjut di RunSummaryPanel.</summary>
        public void ContinueToIsland()
        {
            if (State == ShipFlowState.Finished) return;
            State = ShipFlowState.Finished;
            if (runSummaryPanel != null && UIFocusStack.Instance != null) UIFocusStack.Instance.Close(runSummaryPanel, UICloseReason.User);
            if (RunLifecycle.Instance != null) RunLifecycle.Instance.EnterIslandPhase();
            else Debug.LogWarning("[LifeboatExtraction] RunLifecycle tidak ada (uji scene langsung). IslandPhase tidak dimuat.", this);
        }

        // ---------------------------------------------------------------- Run berakhir

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Mati di kapal: alur berhenti, timer dan bahaya dihentikan oleh pemiliknya masing-masing.
            StopAllCoroutines();
            if (State != ShipFlowState.Summary) State = ShipFlowState.Finished;
        }
    }
}
