using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Status helikopter (RescueEventManager.md bagian 1).</summary>
    public enum RescueState { Assembling, Ready, WarmingUp, ReadyToLift, Departed }

    /// <summary>
    /// Pemilik syarat kemenangan: perakitan helikopter dari suku cadang, urutan lepas landas, dan tekanan hari target.
    /// Blueprint: Island_Logic/RescueEventManager.md (#14). GDD bagian 1 dan 3 (Syarat Menang: helikopter lepas landas,
    /// tanpa batas waktu), bagian 5 Region 3 dan 4. Pemanasan mesin dan gelombang hari ke-60 adalah usulan disetujui (#85).
    ///
    /// Singleton di scene IslandPhase, selalu aktif. ISaveable world_helicopter, LoadOrder 100.
    /// Model kerangka (HelicopterFrame) hidup di Region3_Snow dan membaca progres dari sini.
    /// </summary>
    public class RescueEventManager : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_helicopter";

        public static RescueEventManager Instance { get; private set; }

        public RescueState State { get; private set; } = RescueState.Assembling;
        public int InstalledCount => installedPartIds.Count;
        public int TotalParts => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.region.helicopterPartCount : 5;
        public bool IsDeparted => State == RescueState.Departed;
        public float WarmupLeft => warmupLeft;
        public IReadOnlyList<string> InstalledPartIds => installedPartIds;

        private readonly List<string> installedPartIds = new List<string>(8);
        private readonly List<string> scratch = new List<string>(8);
        private float warmupLeft;
        private float noiseTimer;
        private int lastPublishedSecond = -1;
        private bool targetDayAnnounced;
        private bool targetWaveSent;
        private bool runEnded;

        private HelicopterFrame frame;

        private RescueBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.rescue : null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[RescueEventManager] Instance ganda di scene IslandPhase.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<DayChangedEvent>.Raised += HandleDayChanged;
            EventChannel<DayLimitPassedEvent>.Raised += HandleDayLimitPassed;
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
        }

        private void OnDisable()
        {
            EventChannel<DayChangedEvent>.Raised -= HandleDayChanged;
            EventChannel<DayLimitPassedEvent>.Raised -= HandleDayLimitPassed;
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
        }

        /// <summary>Dipanggil HelicopterFrame saat Region3_Snow dimuat atau dilepas.</summary>
        public void RegisterFrame(HelicopterFrame f) => frame = f;
        public void UnregisterFrame(HelicopterFrame f) { if (frame == f) frame = null; }

        // ---------------------------------------------------------------- Perakitan

        /// <summary>Dipanggil PartInstallZone (trigger tag HeavyItem, tanpa tombol E) dan ReconcileLyingParts.</summary>
        public void InstallPart(string partId)
        {
            // 1.
            if (string.IsNullOrEmpty(partId) || installedPartIds.Contains(partId) || State != RescueState.Assembling) return;
            // 2. Barang yang dibawa dilepas lebih dulu oleh HeavyItemSystem.
            HeavyItemBridge.MarkInstalled(partId);
            // 3-4.
            installedPartIds.Add(partId);
            int total = TotalParts;
            EventChannel<HelicopterPartInstalledEvent>.Publish(new HelicopterPartInstalledEvent(partId, installedPartIds.Count, total));
            // 5. Keputusan 6: save setelah tiap pemasangan, supaya perjalanan panjang tidak hilang.
            EventChannel<SaveRequestedEvent>.Publish(new SaveRequestedEvent(SaveReason.PartInstalled));
            // 6.
            if (installedPartIds.Count >= total)
            {
                State = RescueState.Ready;
                EventChannel<HelicopterReadyEvent>.Publish(new HelicopterReadyEvent());
            }
        }

        // ---------------------------------------------------------------- Lepas landas

        /// <summary>Dipanggil CockpitInteractable saat State = Ready.</summary>
        public bool StartEngine()
        {
            var bal = Bal;
            if (State != RescueState.Ready || bal == null || runEnded) return false;
            // 1-2.
            State = RescueState.WarmingUp;
            warmupLeft = bal.engineWarmupSeconds;
            noiseTimer = 0f;
            lastPublishedSecond = Mathf.CeilToInt(warmupLeft);
            EventChannel<HelicopterEngineChangedEvent>.Publish(new HelicopterEngineChangedEvent(State, warmupLeft));
            // 3. Gelombang panik hanya saat malam: siang tidak ada zombie di luar ruangan (GDD, keputusan 2).
            if (DayNightDirector.Instance != null && DayNightDirector.Instance.IsNight && AIDirector.Instance != null)
                AIDirector.Instance.RequestPanicWave(bal.warmupPanicWaveSize, "helicopter_engine");
            return true;
        }

        private void Update()
        {
            if (State != RescueState.WarmingUp || runEnded) return;
            var bal = Bal;
            if (bal == null) return;

            // Time.deltaTime: pemanasan berhenti saat game dijeda.
            float dt = Time.deltaTime;
            warmupLeft -= dt;

            // 2. Suara mesin menarik musuh (#49).
            noiseTimer -= dt;
            if (noiseTimer <= 0f)
            {
                noiseTimer = bal.engineNoiseIntervalSeconds;
                Vector3 src = frame != null ? frame.CockpitPosition : transform.position;
                NoiseEmitter.Emit(src, bal.engineNoiseRadius, NoiseSourceKind.Structure);
            }

            // 4. Pemain menjauh lebih dari abortDistance: mesin mati, kembali Ready. Region 3 dilepas juga masuk ke sini.
            var player = IslandPlayerController.Instance;
            if (frame == null || (player != null && (player.transform.position - frame.CockpitPosition).sqrMagnitude > bal.abortDistance * bal.abortDistance))
            {
                State = RescueState.Ready;
                warmupLeft = 0f;
                // TODO #22: notifikasi "Mesin mati".
                EventChannel<HelicopterEngineChangedEvent>.Publish(new HelicopterEngineChangedEvent(State, 0f));
                return;
            }

            // 5.
            if (warmupLeft <= 0f)
            {
                State = RescueState.ReadyToLift;
                warmupLeft = 0f;
                EventChannel<HelicopterEngineChangedEvent>.Publish(new HelicopterEngineChangedEvent(State, 0f));
                return;
            }

            // 3. Event hanya saat detik bulat berubah.
            int sec = Mathf.CeilToInt(warmupLeft);
            if (sec != lastPublishedSecond)
            {
                lastPublishedSecond = sec;
                EventChannel<HelicopterEngineChangedEvent>.Publish(new HelicopterEngineChangedEvent(State, warmupLeft));
            }
        }

        /// <summary>
        /// GDD bagian 3 (Syarat Menang): helikopter lepas landas. Langkah berurutan pada satu frame.
        /// force = true untuk DevConsole "heli liftoff" (tanpa syarat state).
        /// </summary>
        public bool LiftOff(bool force = false)
        {
            var gm = GameManager.Instance;
            // 1.
            if (gm == null || gm.IsRunEnding) return false;
            if (!force && State != RescueState.ReadyToLift) return false;
            // 2.
            State = RescueState.Departed;
            EventChannel<HelicopterEngineChangedEvent>.Publish(new HelicopterEngineChangedEvent(State, 0f));
            // 3. TODO #99: NarrativeSequencer.Instance.PrepareEnding(BuildEndingContext()). Konteks: aliveNpcCount dari
            //    NPCRoster.GetAliveNpcIds (#76), lostOnShipCount dari gm.shipOutcome.lostNpcIds, rescuedCount, dan day.
            // 4. hasEscaped = true, RunEndedEvent { Won, None }. RunLifecycle melanjutkan alur akhir (#82).
            gm.TriggerWinSequence();
            return true;
        }

        // ---------------------------------------------------------------- Hari target (usulan, tidak mengubah syarat menang)

        private void HandleDayChanged(DayChangedEvent e)
        {
            if (e.isReplay) return;
            if (e.newDay == GameManager.TargetDay && !targetDayAnnounced)
            {
                // TODO #22: notifikasi kritis "Hari ke-60. Setelah malam ini, zombie akan semakin kuat".
                targetDayAnnounced = true;
            }
        }

        private void HandleNightfall(NightfallEvent e)
        {
            var bal = Bal;
            if (bal == null || e.day != GameManager.TargetDay || State == RescueState.Departed || targetWaveSent) return;
            if (AIDirector.Instance != null) AIDirector.Instance.RequestPanicWave(bal.targetNightWaveSize, "target_night");
            targetWaveSent = true;
        }

        private void HandleDayLimitPassed(DayLimitPassedEvent e)
        {
            // GDD bagian 3: tidak ada batas waktu kabur. Perakitan dan lepas landas tetap berjalan setelah hari ke-60.
            // TODO #22: notifikasi kritis "Hari ke-60 terlewat. Zombie semakin kuat, tetapi helikopter masih bisa kabur".
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Mati saat pemanasan: mesin berhenti.
            runEnded = true;
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>DevConsole "heli warmup &lt;detik&gt;" (#56).</summary>
        public void DebugSetWarmup(float seconds) { if (State == RescueState.WarmingUp) warmupLeft = Mathf.Max(0f, seconds); }
#endif

        // ---------------------------------------------------------------- ISaveable (bagian 4)

        [Serializable]
        private class SaveData
        {
            public string[] installedPartIds;
            public bool targetDayAnnounced;
            public bool targetWaveSent;
            public RescueState state;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 100;

        public string CaptureState()
        {
            // WarmingUp dan ReadyToLift disimpan sebagai Ready: pemanasan mengulang setelah load,
            // jadi tidak bisa dipakai untuk menghindari gelombang.
            var s = State == RescueState.WarmingUp || State == RescueState.ReadyToLift ? RescueState.Ready : State;
            return JsonUtility.ToJson(new SaveData
            {
                installedPartIds = installedPartIds.ToArray(),
                targetDayAnnounced = targetDayAnnounced,
                targetWaveSent = targetWaveSent,
                state = s
            });
        }

        public void RestoreState(string json)
        {
            // Tidak mengirim event.
            installedPartIds.Clear();
            var d = string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<SaveData>(json);
            if (d == null) return;
            if (d.installedPartIds != null) installedPartIds.AddRange(d.installedPartIds);
            targetDayAnnounced = d.targetDayAnnounced;
            targetWaveSent = d.targetWaveSent;
            State = d.state == RescueState.Departed ? RescueState.Departed : RescueState.Assembling;
            warmupLeft = 0f;
        }

        public void OnRestoreFinished()
        {
            // records HeavyItemSystem yang menang. Lalu kirim ulang event sticky untuk HUD dan kerangka.
            HeavyItemBridge.CollectInstalled(scratch);
            if (HeavyItemBridge.IsAvailable)
            {
                installedPartIds.Clear();
                installedPartIds.AddRange(scratch);
            }
            scratch.Clear();

            int total = TotalParts;
            if (installedPartIds.Count > 0)
            {
                string last = installedPartIds[installedPartIds.Count - 1];
                EventChannel<HelicopterPartInstalledEvent>.Publish(new HelicopterPartInstalledEvent(last, installedPartIds.Count, total));
            }
            if (State != RescueState.Departed && installedPartIds.Count >= total)
            {
                State = RescueState.Ready;
                EventChannel<HelicopterReadyEvent>.Publish(new HelicopterReadyEvent());
            }
        }
    }
}
