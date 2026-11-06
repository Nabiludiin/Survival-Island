using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Efek satu jenis cuaca. Baris di GameBalanceConfig.weather.effects (RegionalWeather.md "Efek per cuaca").</summary>
    [Serializable]
    public struct WeatherEffects
    {
        public WeatherType weather;          // Hanya label di Inspector. Baris dibaca menurut urutan WeatherType
        public float speedMultiplier;
        public float aiSightMultiplier;
        public float aiHearingMultiplier;
        public float warmthLossPerSec;

        public WeatherEffects(WeatherType weather, float speed, float sight, float hearing, float warmthLoss)
        {
            this.weather = weather;
            speedMultiplier = speed;
            aiSightMultiplier = sight;
            aiHearingMultiplier = hearing;
            warmthLossPerSec = warmthLoss;
        }

        public static WeatherEffects Neutral => new WeatherEffects(WeatherType.Clear, 1f, 1f, 1f, 0f);
    }

    /// <summary>Pemaksa cuaca per region dari bencana (GLOSSARY.md). snowWeather berlaku untuk Region 3.</summary>
    public struct WeatherOverride
    {
        public byte regionMask;          // Bit (1 << (int)RegionType)
        public WeatherType weather;
        public WeatherType snowWeather;

        public WeatherOverride(byte regionMask, WeatherType weather, WeatherType snowWeather)
        {
            this.regionMask = regionMask;
            this.weather = weather;
            this.snowWeather = snowWeather;
        }

        public bool Covers(RegionType r) => (regionMask & (1 << (int)r)) != 0;
        public WeatherType For(RegionType r) => r == RegionType.Snow ? snowWeather : weather;
    }

    /// <summary>Penanda override yang dipasang, untuk PopOverride.</summary>
    public readonly struct WeatherOverrideHandle
    {
        public readonly int id;
        public WeatherOverrideHandle(int id) { this.id = id; }
        public bool IsValid => id > 0;
    }

    /// <summary>
    /// Menentukan cuaca tiap region secara terpisah dari seed. WeatherManager hanya menampilkan cuaca di tempat pemain.
    /// Blueprint: Island_Logic/RegionalWeather.md (#115). GDD bagian 5 (Region 3 bersalju) dan bagian 6 (Badai Tropis).
    /// Cuaca adalah fungsi murni dari seed, region, dan slot jam game, jadi tidak disimpan.
    ///
    /// isPlayerInside dari InteriorZoneChangedEvent (#112): di dalam ruangan efek cuaca pada pemain netral.
    /// </summary>
    public class RegionalWeather : MonoBehaviour
    {
        private const int RegionCount = 4;
        private const int WeatherCount = 6;

        public static RegionalWeather Instance { get; private set; }

        [Tooltip("Satu tabel per region. Region tanpa tabel memakai nilai awal RegionWeatherTable.DefaultWeights.")]
        [SerializeField] private RegionWeatherTable[] tables = Array.Empty<RegionWeatherTable>();

        private readonly WeatherType[] current = new WeatherType[RegionCount];
        private readonly float[][] weightsByRegion = new float[RegionCount][];
        private readonly List<KeyValuePair<int, WeatherOverride>> overrides = new List<KeyValuePair<int, WeatherOverride>>();
        private readonly WeatherType[] before = new WeatherType[RegionCount];
        private int nextOverrideId = 1;
        private int currentSlot = int.MinValue;
        private RegionType activeRegion = RegionType.Volcano;
        private bool isPlayerInside;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        public RegionType ActiveRegion => activeRegion;
        public bool IsPlayerInside => isPlayerInside;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            for (int r = 0; r < RegionCount; r++)
            {
                weightsByRegion[r] = RegionWeatherTable.DefaultWeights((RegionType)r);
                current[r] = WeatherType.Clear;
            }
            if (tables != null)
            {
                for (int i = 0; i < tables.Length; i++)
                {
                    var t = tables[i];
                    if (t == null) continue;
                    var buf = new float[WeatherCount];
                    t.FillWeights(buf);
                    weightsByRegion[(int)t.region] = buf;
                }
            }
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<RegionChangedEvent>.Raised += HandleRegionChanged;
            EventChannel<InteriorZoneChangedEvent>.Raised += HandleInterior;   // #112, sticky
            if (EventChannel<InteriorZoneChangedEvent>.HasLast) isPlayerInside = EventChannel<InteriorZoneChangedEvent>.Last.isInside;
            if (EventChannel<RegionChangedEvent>.HasLast) activeRegion = EventChannel<RegionChangedEvent>.Last.to;
            Recompute(force: true);
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<RegionChangedEvent>.Raised -= HandleRegionChanged;
            EventChannel<InteriorZoneChangedEvent>.Raised -= HandleInterior;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Fungsi publik

        /// <summary>Override terbaru yang mencakup r, atau cuaca terjadwal. Sah untuk semua region, dimuat atau tidak.</summary>
        public WeatherType GetWeather(RegionType r)
        {
            for (int i = overrides.Count - 1; i >= 0; i--)
            {
                var o = overrides[i].Value;
                if (o.Covers(r)) return o.For(r);
            }
            return current[(int)r];
        }

        /// <summary>Efek cuaca region. Di dalam ruangan, efek pada pemain netral; efek AI tetap.</summary>
        public WeatherEffects GetEffects(RegionType r)
        {
            var fx = EffectsOf(GetWeather(r));
            if (r == activeRegion && isPlayerInside)
            {
                fx.speedMultiplier = 1f;
                fx.warmthLossPerSec = 0f;
            }
            return fx;
        }

        /// <summary>Dipanggil DisasterManager (#70). Badai Tropis = Storm di Region 1, 2, 4 dan Blizzard di Region 3.</summary>
        public WeatherOverrideHandle PushOverride(in WeatherOverride o)
        {
            SnapshotBefore();
            int id = nextOverrideId++;
            overrides.Add(new KeyValuePair<int, WeatherOverride>(id, o));
            PublishChanges();
            ApplyActive();
            return new WeatherOverrideHandle(id);
        }

        public void PopOverride(WeatherOverrideHandle h)
        {
            if (!h.IsValid) return;
            int idx = overrides.FindIndex(p => p.Key == h.id);
            if (idx < 0) return;
            SnapshotBefore();
            overrides.RemoveAt(idx);
            // Cuaca terjadwal tetap dihitung di belakang, jadi kembali ke jadwal.
            PublishChanges();
            ApplyActive();
        }

        // ---------------------------------------------------------------- Menghitung cuaca

        private static bool IsSevere(WeatherType w) => w == WeatherType.Storm || w == WeatherType.Blizzard;

        private static WeatherType Downgrade(WeatherType w) => w == WeatherType.Storm ? WeatherType.Rain : WeatherType.Snow;

        private WeatherType DrawRaw(RegionType r, int slot)
        {
            var seed = RunSeedManager.Instance;
            if (seed == null) return WeatherType.Clear;
            var s = seed.Fork(RandomStreamId.Weather, (int)r * 10000 + slot);
            int idx = s.PickWeighted(weightsByRegion[(int)r]);
            return idx >= 0 && idx < WeatherCount ? (WeatherType)idx : WeatherType.Clear;
        }

        private WeatherType Scheduled(RegionType r, int slot)
        {
            var bal = Balance;
            // 1.
            var w = DrawRaw(r, slot);
            if (!IsSevere(w)) return w;

            // 2. Usulan disetujui: tidak ada cuaca berat sebelum minSevereDay (game ini permadeath).
            float interval = bal != null ? Mathf.Max(0.01f, bal.weather.weatherIntervalGameHours) : 8f;
            int day = Mathf.FloorToInt(slot * interval / 24f) + 1;
            int minSevereDay = bal != null ? bal.weather.minSevereDay : 2;
            if (day < minSevereDay) return Downgrade(w);

            // 3. Cuaca berat tidak berlangsung dua slot berturut-turut. DrawRaw, jadi tanpa rekursi. Slot negatif dilewati.
            if (slot > 0 && IsSevere(DrawRaw(r, slot - 1))) return Downgrade(w);
            return w;
        }

        private int SlotNow()
        {
            var clock = DayNightDirector.Instance;
            var bal = Balance;
            if (clock == null) return 0;
            float interval = bal != null ? Mathf.Max(0.01f, bal.weather.weatherIntervalGameHours) : 8f;
            return Mathf.Max(0, (int)Math.Floor(clock.TotalGameHours / interval));
        }

        // ---------------------------------------------------------------- Pergantian slot

        private void HandleHour(HourChangedEvent e) => Recompute(force: false);

        private void Recompute(bool force)
        {
            // Jam game, jadi berhenti saat jeda dan ikut cepat saat tidur (#84).
            int slot = SlotNow();
            if (!force && slot == currentSlot) return;
            currentSlot = slot;

            SnapshotBefore();
            for (int r = 0; r < RegionCount; r++) current[r] = Scheduled((RegionType)r, slot);
            PublishChanges(force);
            ApplyActive();
        }

        private void SnapshotBefore()
        {
            for (int r = 0; r < RegionCount; r++) before[r] = GetWeather((RegionType)r);
        }

        /// <summary>Terbitkan WeatherChangedEvent untuk region yang hasil akhirnya berubah. Region aktif paling akhir.</summary>
        private void PublishChanges(bool all = false)
        {
            for (int r = 0; r < RegionCount; r++)
            {
                if (r == (int)activeRegion) continue;
                var w = GetWeather((RegionType)r);
                if (all || w != before[r]) EventChannel<WeatherChangedEvent>.Publish(new WeatherChangedEvent((RegionType)r, w));
            }
            var a = GetWeather(activeRegion);
            if (all || a != before[(int)activeRegion]) EventChannel<WeatherChangedEvent>.Publish(new WeatherChangedEvent(activeRegion, a));
        }

        private void HandleInterior(InteriorZoneChangedEvent e)
        {
            if (isPlayerInside == e.isInside) return;
            isPlayerInside = e.isInside;
            ApplyActive();
        }

        private void HandleRegionChanged(RegionChangedEvent e)
        {
            activeRegion = e.to;
            ApplyActive();
        }

        /// <summary>Kirim cuaca region aktif ke tampilan, kecepatan pemain, dan deteksi musuh.</summary>
        private void ApplyActive()
        {
            var bal = Balance;
            // 1.
            var w = GetWeather(activeRegion);
            var fx = GetEffects(activeRegion);
            float blend = bal != null ? bal.weather.presentBlendSeconds : 6f;

            // 2-4.
            if (WeatherManager.Instance != null) WeatherManager.Instance.Present(w, blend, isPlayerInside);
            if (IslandPlayerController.Instance != null)
                IslandPlayerController.Instance.SetSpeedModifier(SpeedModifierId.Weather, fx.speedMultiplier);
            if (AISensoryHub.Instance != null)
                AISensoryHub.Instance.SetWeatherModifiers(fx.aiSightMultiplier, fx.aiHearingMultiplier);
        }

        private static WeatherEffects EffectsOf(WeatherType w)
        {
            var rows = Balance != null ? Balance.weather.effects : null;
            int i = (int)w;
            if (rows == null || i < 0 || i >= rows.Length) return WeatherEffects.Neutral;
            return rows[i];
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>DevConsole (#56): paksa cuaca satu region sampai PopOverride.</summary>
        public WeatherOverrideHandle DebugForce(RegionType r, WeatherType w)
        {
            return PushOverride(new WeatherOverride((byte)(1 << (int)r), w, w));
        }
#endif
    }
}
