using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pemilik jam dunia di IslandPhase. Memajukan waktu dan mengirim DaybreakEvent, NightfallEvent, serta HourChangedEvent.
    /// Blueprint: left-4-dead/DayNightDirector.md (#56, #63, #66, #67, #80, #92).
    ///
    /// GDD bagian 3: siklus siang dan malam, target bertahan 60 hari. Siang = hewan buas, malam = horde zombie
    /// (pendengar NightfallEvent dan DaybreakEvent yang mengatur spawn, bukan kelas ini).
    /// DayNightDirector TIDAK menulis Light. Rotasi cahaya dibaca EnvironmentDirector (#92) dari SunRotation.
    /// </summary>
    [DisallowMultipleComponent]
    public class DayNightDirector : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_clock";
        private const float StartHour = 6f;     // Run baru mulai jam 6 pagi (DayNightDirector.md)
        private const float NightStartHour = 18f;
        private const float DayStartHour = 6f;

        public static DayNightDirector Instance { get; private set; }

        [SerializeField] private float currentTimeOfDay = StartHour;
        private double totalGameHours = StartHour;
        private bool isNight;
        private int lastHour = (int)StartHour;
        private float overrideMultiplier = -1f;   // Di bawah 0 = tanpa override

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Properti publik

        public float CurrentTimeOfDay => currentTimeOfDay;
        public bool IsNight => isNight;
        public bool IsDay => !isNight;

        /// <summary>Jam sejak run dimulai. Disimpan sendiri, jadi tidak mundur saat AdvanceDay keluar lebih awal.</summary>
        public double TotalGameHours => totalGameHours;

        public float BaseTimeMultiplier => Balance != null ? Balance.time.timeMultiplier : 96f;

        /// <summary>Rotasi matahari dari jam. Dibaca EnvironmentDirector (#92).</summary>
        public Quaternion SunRotation => Quaternion.Euler((currentTimeOfDay / 24f) * 360f - 90f, -30f, 0f);

        /// <summary>Arah datangnya cahaya matahari (dari matahari ke tanah dibalik). Dibaca SunlightExposure (#63).</summary>
        public Vector3 SunDirection => -(SunRotation * Vector3.forward);

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[DayNightDirector] Ada dua DayNightDirector di scene.");
                enabled = false;
                return;
            }
            Instance = this;
            ResetClock();
        }

        private void OnEnable()
        {
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
        }

        private void OnDisable()
        {
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Start()
        {
            // Run baru: kirim status awal sekali supaya pendengar sticky (InventoryManager, WorldItemManager) punya jam.
            // Save yang dimuat menimpa lewat RestoreState lalu OnRestoreFinished.
            PublishStickyState();
        }

        private void ResetClock()
        {
            // Hari dari GameManager, supaya TotalGameHours = (hari - 1) * 24 + jam (DayNightDirector.md tabel 1).
            int day = GameManager.Instance != null ? GameManager.Instance.currentDay : 1;
            currentTimeOfDay = StartHour;
            totalGameHours = (day - 1) * 24.0 + StartHour;
            isNight = false;
            lastHour = (int)StartHour;
            overrideMultiplier = -1f;
        }

        // ---------------------------------------------------------------- Memajukan jam

        private void Update()
        {
            // Game dijeda: Time.deltaTime = 0, jadi jam berhenti.
            float multiplier = overrideMultiplier > 0f ? overrideMultiplier : BaseTimeMultiplier;
            float remaining = (Time.deltaTime * multiplier) / 3600f;
            if (remaining <= 0f) return;

            float step = Balance != null ? Balance.sleep.maxGameHoursPerStep : 0.25f;
            if (step <= 0f) step = 0.25f;

            // Frame panjang atau tidur dipecah per langkah, supaya batas 18:00, 06:00, dan tiap jam tidak terlewat.
            while (remaining > 0f)
            {
                AdvanceClock(Mathf.Min(remaining, step));
                remaining -= step;
            }
        }

        /// <summary>Maju satu langkah, lalu periksa batas (tabel SleepSystem.md).</summary>
        private void AdvanceClock(float hours)
        {
            currentTimeOfDay += hours;
            totalGameHours += hours;

            // 24:00: hari baru. GameManager pemilik currentDay. Lewat hari ke-60 jam tetap berjalan (GDD bagian 3).
            if (currentTimeOfDay >= 24f)
            {
                currentTimeOfDay -= 24f;
                if (GameManager.Instance != null) GameManager.Instance.AdvanceDay();
            }

            if (currentTimeOfDay >= NightStartHour && !isNight)
            {
                isNight = true;
                EventChannel<NightfallEvent>.Publish(new NightfallEvent(CurrentDay()));
            }
            else if (currentTimeOfDay >= DayStartHour && currentTimeOfDay < NightStartHour && isNight)
            {
                isNight = false;
                EventChannel<DaybreakEvent>.Publish(new DaybreakEvent(CurrentDay()));
            }

            int hour = Mathf.FloorToInt(currentTimeOfDay);
            if (hour != lastHour)
            {
                lastHour = hour;
                EventChannel<HourChangedEvent>.Publish(new HourChangedEvent(hour));
            }
        }

        private static int CurrentDay() => GameManager.Instance != null ? GameManager.Instance.currentDay : 1;

        private void PublishStickyState()
        {
            int day = CurrentDay();
            if (isNight) EventChannel<NightfallEvent>.Publish(new NightfallEvent(day));
            else EventChannel<DaybreakEvent>.Publish(new DaybreakEvent(day));
            EventChannel<HourChangedEvent>.Publish(new HourChangedEvent(lastHour));
        }

        // ---------------------------------------------------------------- Override kecepatan (SleepSystem #67)

        public void SetTimeMultiplierOverride(float value) => overrideMultiplier = value;

        public void ClearTimeMultiplierOverride() => overrideMultiplier = -1f;

        // ---------------------------------------------------------------- Pengembangan (DevConsole #56)

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>Perintah settime. Hanya mengatur jam dalam hari; batas siang/malam diperiksa pada langkah berikutnya.</summary>
        public void DebugSetTime(float jam)
        {
            jam = Mathf.Repeat(jam, 24f);
            double dayStart = totalGameHours - currentTimeOfDay;
            currentTimeOfDay = jam;
            totalGameHours = dayStart + jam;
            lastHour = Mathf.FloorToInt(jam);
            // Samakan status siang/malam dengan jam baru. Batas berikutnya (18:00 atau 06:00) tetap mengirim event
            // saat dilewati (skenario tes 2 dan 3).
            bool shouldBeNight = jam >= NightStartHour || jam < DayStartHour;
            if (shouldBeNight != isNight)
            {
                isNight = shouldBeNight;
                PublishStickyState();
            }
        }
#endif

        // ---------------------------------------------------------------- Simpan dan muat (#80)

        [Serializable]
        private class ClockSaveData
        {
            public float currentTimeOfDay;
            public double totalGameHours;
            public bool isNight;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 10;

        public string CaptureState()
        {
            return JsonUtility.ToJson(new ClockSaveData
            {
                currentTimeOfDay = currentTimeOfDay,
                totalGameHours = totalGameHours,
                isNight = isNight,
            });
        }

        public void RestoreState(string json)
        {
            // Hanya mengisi data, tanpa event.
            ClockSaveData data = null;
            try { data = JsonUtility.FromJson<ClockSaveData>(json); }
            catch (Exception e) { Debug.LogError($"[DayNightDirector] Gagal membaca save: {e.Message}"); }
            if (data == null) { ResetClock(); return; }
            currentTimeOfDay = Mathf.Repeat(data.currentTimeOfDay, 24f);
            totalGameHours = data.totalGameHours;
            isNight = data.isNight;
            lastHour = Mathf.FloorToInt(currentTimeOfDay);
        }

        public void OnRestoreFinished()
        {
            // Event sticky dikirim ulang setelah semua data pulih (DayNightDirector.md "Kasus Khusus").
            PublishStickyState();
        }
    }
}
