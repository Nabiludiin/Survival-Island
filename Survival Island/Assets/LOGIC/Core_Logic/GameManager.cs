using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Fase aktif run (GLOSSARY.md bagian 4).</summary>
    public enum GamePhase { SinkingShip, IslandSurvival }

    /// <summary>Hasil akhir run (GLOSSARY.md bagian 4).</summary>
    public enum RunResult { Won, Died }

    /// <summary>
    /// Data run yang disimpan WorldStateSave lewat GameManager.CaptureRunData dan RestoreRunData.
    /// Isi mengikuti WorldStateSave.md tabel "Isi data" baris "Hari dan fase" (#80).
    /// </summary>
    [Serializable]
    public class GameRunSaveData
    {
        public int currentDay = 1;
        public bool hasEscaped;
        public bool hasArrivedOnIsland;
        public ShipOutcomeData shipOutcome = new ShipOutcomeData();
        public bool wasTainted;
        public GamePhase phase = GamePhase.IslandSurvival;
    }

    /// <summary>
    /// Manager persisten yang memegang status run: fase, hari, dan data dari Kapal (Fase 1) ke Pulau (Fase 2).
    /// Menentukan hasil akhir run, tetapi tidak memuat scene dan tidak menghapus save.
    /// Blueprint: Core_Logic/GameManager.md (#82, #74, #56, #14). GDD bagian 1 dan 3.
    ///
    /// Urutan bootstrap ke-1 (RunLifecycle.md). Objeknya di bawah PersistentRoot, jadi tidak memanggil DontDestroyOnLoad sendiri.
    /// </summary>
    public class GameManager : MonoBehaviour, IBootstrapService
    {
        public static GameManager Instance { get; private set; }

        /// <summary>GDD bagian 3: target hari ke-60. Bukan batas, karena currentDay tidak punya batas atas.</summary>
        public const int TargetDay = 60;

        [Tooltip("Aset GameBalanceConfig asli. GameManager memakai salinannya per run (#84).")]
        [SerializeField] private GameBalanceConfig balanceAsset;

        // ---------------------------------------------------------------- Status run (GameManager.md bagian 1)
        public GamePhase currentPhase { get; private set; } = GamePhase.SinkingShip;
        public int currentDay { get; private set; } = 1;
        public bool hasEscaped { get; private set; }
        public bool IsRunEnding { get; private set; }
        public bool IsPastTargetDay => currentDay > TargetDay;
        public bool hasArrivedOnIsland { get; private set; }
        public bool wasTainted { get; private set; }

        /// <summary>Barang dari Fase 1. Dikosongkan IslandArrival.Apply (#74) lewat ClearSavedShipLoot.</summary>
        public readonly List<SavedItemData> savedShipLoot = new List<SavedItemData>();

        /// <summary>Hasil Fase 1 (#74).</summary>
        public ShipOutcomeData shipOutcome { get; private set; } = new ShipOutcomeData();

        /// <summary>Salinan GameBalanceConfig untuk run ini (#84). Semua sistem membaca angka dari sini.</summary>
        public GameBalanceConfig Balance { get; private set; }

        /// <summary>
        /// Pembantu baca: Balance run aktif, atau null kalau GameManager belum siap (mis. tes scene di Editor).
        /// Pemanggil wajib menangani null.
        /// </summary>
        public static GameBalanceConfig CurrentBalance => Instance != null ? Instance.Balance : null;

        private bool dayLimitPassedSent;

        private void Awake()
        {
            // Awake hanya mengisi Instance (RunLifecycle.md: urutan bootstrap).
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[GameManager] Instance ganda. Periksa prefab PersistentRoot.");
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
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
        }

        private void OnDisable()
        {
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
        }

        // ---------------------------------------------------------------- Layanan dan reset

        /// <summary>Membuat salinan GameBalanceConfig. Kegagalan di sini menghentikan boot (RunLifecycle.md, Boot langkah 1).</summary>
        public void Initialize()
        {
            if (balanceAsset == null)
                throw new InvalidOperationException("[GameManager] balanceAsset kosong. Isi aset GameBalanceConfig di prefab PersistentRoot.");
            CreateBalanceCopy();
        }

        /// <summary>
        /// Kembalikan semua data run ke nilai awal. Tidak memuat scene (milik RunLifecycle).
        /// GDD bagian 3 (Permadeath): run baru mulai dari nol.
        /// </summary>
        public void ResetForNewRun()
        {
            savedShipLoot.Clear();
            shipOutcome.Clear();
            currentDay = 1;
            hasEscaped = false;
            hasArrivedOnIsland = false;
            IsRunEnding = false;
            wasTainted = false;
            dayLimitPassedSent = false;
            currentPhase = GamePhase.SinkingShip;
            CreateBalanceCopy();
        }

        private void CreateBalanceCopy()
        {
            if (balanceAsset == null) return;
            if (Balance != null) Destroy(Balance);
            // GameBalanceConfig.md bagian 4: DevConsole mengubah salinan sementara, bukan aset asli.
            Balance = Instantiate(balanceAsset);
            Balance.name = balanceAsset.name + " (Run)";
        }

        /// <summary>Ganti fase, lalu terbitkan PhaseChangedEvent.</summary>
        public void SetPhase(GamePhase phase)
        {
            currentPhase = phase;
            EventChannel<PhaseChangedEvent>.Publish(new PhaseChangedEvent(phase));
        }

        // ---------------------------------------------------------------- Save dan load

        /// <summary>Dipanggil RunLifecycle.ContinueRun. Mengisi data tanpa mengirim event.</summary>
        public void RestoreRunData(GameRunSaveData data)
        {
            if (data == null)
            {
                Debug.LogError("[GameManager] RestoreRunData menerima data null.");
                return;
            }

            currentDay = Mathf.Max(1, data.currentDay);
            hasEscaped = data.hasEscaped;
            hasArrivedOnIsland = data.hasArrivedOnIsland;
            wasTainted = data.wasTainted;

            shipOutcome.Clear();
            if (data.shipOutcome != null)
            {
                if (data.shipOutcome.rescuedNpcIds != null) shipOutcome.rescuedNpcIds.AddRange(data.shipOutcome.rescuedNpcIds);
                if (data.shipOutcome.lostNpcIds != null) shipOutcome.lostNpcIds.AddRange(data.shipOutcome.lostNpcIds);
                shipOutcome.hpOnExit = data.shipOutcome.hpOnExit;
                shipOutcome.infectionOnExit = data.shipOutcome.infectionOnExit;
                shipOutcome.secondsLeftOnExit = data.shipOutcome.secondsLeftOnExit;
                shipOutcome.departedEarly = data.shipOutcome.departedEarly;
            }

            // Dihitung ulang dari currentDay, supaya DayLimitPassedEvent tidak terkirim dua kali setelah load.
            dayLimitPassedSent = IsPastTargetDay;
        }

        /// <summary>Pasangan RestoreRunData. Dipanggil WorldStateSave (#80) saat membangun payload.</summary>
        public GameRunSaveData CaptureRunData()
        {
            var data = new GameRunSaveData
            {
                currentDay = currentDay,
                hasEscaped = hasEscaped,
                hasArrivedOnIsland = hasArrivedOnIsland,
                wasTainted = wasTainted,
                phase = currentPhase
            };
            data.shipOutcome.rescuedNpcIds.AddRange(shipOutcome.rescuedNpcIds);
            data.shipOutcome.lostNpcIds.AddRange(shipOutcome.lostNpcIds);
            data.shipOutcome.hpOnExit = shipOutcome.hpOnExit;
            data.shipOutcome.infectionOnExit = shipOutcome.infectionOnExit;
            data.shipOutcome.secondsLeftOnExit = shipOutcome.secondsLeftOnExit;
            data.shipOutcome.departedEarly = shipOutcome.departedEarly;
            return data;
        }

        // ---------------------------------------------------------------- Data dari Fase 1

        /// <summary>Dipanggil LifeboatExtraction (#74). Menggantikan SaveFase1Loot lama.</summary>
        public void SaveShipOutcome(List<SavedItemData> items, ShipOutcomeData outcome)
        {
            savedShipLoot.Clear();
            if (items != null) savedShipLoot.AddRange(items);
            if (outcome != null) shipOutcome = outcome;
        }

        /// <summary>
        /// Dipanggil IslandArrival.Apply (#74) setelah barang diterapkan: kosongkan savedShipLoot,
        /// lalu hasArrivedOnIsland = true. Ditambahkan karena data ini read-only dari luar (lihat laporan Langkah 1).
        /// </summary>
        public void MarkArrivedOnIsland()
        {
            savedShipLoot.Clear();
            hasArrivedOnIsland = true;
        }

        /// <summary>Dipanggil DevConsole (#56) saat perintah curang dipakai.</summary>
        public void MarkTainted()
        {
            wasTainted = true;
        }

        // ---------------------------------------------------------------- Hari

        /// <summary>
        /// Terbitkan DayChangedEvent tanpa mengubah currentDay. Dipanggil RunLifecycle saat tiba di pulau (isReplay false)
        /// dan setelah FinishRestore (isReplay true).
        /// </summary>
        public void PublishDayState(bool isReplay)
        {
            EventChannel<DayChangedEvent>.Publish(new DayChangedEvent(currentDay, isReplay));
        }

        /// <summary>Dipanggil DayNightDirector (Langkah 4) saat jam melewati 24:00.</summary>
        public void AdvanceDay()
        {
            if (IsRunEnding) return;
            currentDay++;
            EventChannel<DayChangedEvent>.Publish(new DayChangedEvent(currentDay, false));
            CheckDay60Milestone();
        }

        /// <summary>
        /// GDD bagian 3: melewati hari ke-60 BUKAN akhir run. Hanya kirim DayLimitPassedEvent sekali per run.
        /// Kekuatan zombie setelah hari ke-60 dihitung AIDirector (#27).
        /// </summary>
        public void CheckDay60Milestone()
        {
            if (!IsPastTargetDay || dayLimitPassedSent) return;
            dayLimitPassedSent = true;
            EventChannel<DayLimitPassedEvent>.Publish(new DayLimitPassedEvent(currentDay));
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>Perintah setday (#56). Tolak day &lt; 1, tanpa batas atas.</summary>
        public bool DebugSetDay(int day)
        {
            if (day < 1) return false;
            currentDay = day;
            EventChannel<DayChangedEvent>.Publish(new DayChangedEvent(currentDay, false));
            CheckDay60Milestone();
            return true;
        }
#endif

        // ---------------------------------------------------------------- Akhir run

        /// <summary>GDD bagian 3: pemain mati, run berakhir. Panggilan pertama menang, berikutnya diabaikan.</summary>
        public void TriggerGameOver(DeathReason reason)
        {
            if (IsRunEnding) return;
            IsRunEnding = true;
            EventChannel<RunEndedEvent>.Publish(new RunEndedEvent(RunResult.Died, reason));
        }

        /// <summary>GDD bagian 3 (Syarat Menang): helikopter lepas landas. Dipanggil RescueEventManager (#14).</summary>
        public void TriggerWinSequence()
        {
            if (IsRunEnding) return;
            hasEscaped = true;
            IsRunEnding = true;
            EventChannel<RunEndedEvent>.Publish(new RunEndedEvent(RunResult.Won, DeathReason.None));
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            TriggerGameOver(e.reason);
        }
    }
}
