using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pemilik nilai Warmth 0 sampai 100. Menghitung panas dan dingin secara berkala, lalu memberi tahu SurvivalStats
    /// kapan pemain membeku lewat WarmthChangedEvent. Blueprint: Island_Logic/WarmthSystem.md (#65).
    /// GDD bagian 5, Region 3: pakaian hangat atau obor mencegah frostbite; frostbite menguras Stamina dulu, lalu HP.
    /// Akibat frostbite dihitung SurvivalStats (frostbiteStaminaDrainPerSec, frostbiteDamagePerSec). Sistem ini tidak mengurangi HP.
    ///
    /// Claude.md Tahap 4 menyebut coroutine FrostbiteTick(). Di sini FrostbiteTick dipanggil dari akumulator Time.deltaTime
    /// (blueprint: "Tick setiap tickSeconds"), supaya ikut berhenti saat jeda tanpa coroutine.
    ///
    /// Belum ada (TODO): SwimStateChangedEvent (#13, sementara isInWater dibaca dari PlayerTraversal.Mode == Swimming),
    /// InteriorZoneChangedEvent (#112), CampfireManager.ActiveLit (#44), dan notifikasi HUD (#22).
    /// </summary>
    public class WarmthSystem : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "player_warmth";
        private const int RegionCount = 4;

        public static WarmthSystem Instance { get; private set; }

        [SerializeField] private PlayerTraversal traversal;

        private readonly WeatherType[] weatherByRegion = new WeatherType[RegionCount];
        private RegionType currentRegion = RegionType.Volcano;
        private ItemInstance wornClothing;      // Pakaian di slot pakaian, di luar hotbar dan 15 slot inventori
        private bool isTorchHeld;
        private bool isInWater;
        private bool isIndoors;
        private bool isInInterior;
        private bool runEnded;
        private bool registeredForSave;
        private float tickTimer;
        private float shelterTimer;
        private int lastPublishedRounded = -1;
        private bool lastPublishedFreezing;
        private bool hasPublished;

        /// <summary>Nilai suhu tubuh.</summary>
        public float Warmth { get; private set; } = 100f;
        /// <summary>Dibaca SleepSystem (#67), dan dikirim ke SurvivalStats.</summary>
        public bool IsFreezing { get; private set; }
        public bool IsInWater => isInWater;
        public RegionType CurrentRegion => currentRegion;
        public int WornClothingInstanceId => wornClothing != null ? wornClothing.instanceId : -1;
        public ItemInstance WornClothing => wornClothing;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (traversal == null) traversal = GetComponent<PlayerTraversal>();
        }

        private void OnEnable()
        {
            EventChannel<RegionChangedEvent>.Raised += HandleRegion;
            EventChannel<WeatherChangedEvent>.Raised += HandleWeather;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            // TODO #13: SwimStateChangedEvent. TODO #112: InteriorZoneChangedEvent.

            // Sticky: pemain yang dimuat di Region 3 langsung terkena dingin.
            if (EventChannel<RegionChangedEvent>.HasLast) currentRegion = EventChannel<RegionChangedEvent>.Last.to;
            for (int r = 0; r < RegionCount; r++)
                weatherByRegion[r] = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather((RegionType)r) : WeatherType.Clear;

            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<RegionChangedEvent>.Raised -= HandleRegion;
            EventChannel<WeatherChangedEvent>.Raised -= HandleWeather;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void HandleRegion(RegionChangedEvent e) => currentRegion = e.to;
        private void HandleWeather(WeatherChangedEvent e) => weatherByRegion[(int)e.region] = e.weather;
        private void HandleRunEnded(RunEndedEvent e) => runEnded = true;

        // ---------------------------------------------------------------- Pemeriksaan berkala

        private void Update()
        {
            var bal = Balance;
            if (bal == null || runEnded) return;
            var w = bal.warmth;
            float dt = Time.deltaTime; // Game dijeda: Warmth dan durasi obor ikut berhenti

            // TODO #13: ganti dengan SwimStateChangedEvent.isSwimming.
            isInWater = traversal != null && traversal.Mode == TraversalMode.Swimming;

            shelterTimer -= dt;
            if (shelterTimer <= 0f)
            {
                shelterTimer = w.shelterCheckSeconds;
                RefreshShelter(w);
            }

            tickTimer += dt;
            if (tickTimer < w.tickSeconds) return;
            float step = tickTimer;
            tickTimer = 0f;
            FrostbiteTick(step, w);
        }

        /// <summary>
        /// Tick kehangatan (blueprint WarmthSystem.md "Tick", nama dari Claude.md Tahap 4).
        /// GDD bagian 5, Region 3: dingin menurunkan Warmth sampai membeku. Batas bawah hujan dan air di region lain
        /// menjaga frostbite tetap khusus Region 3 (usulan disetujui #85 poin 2).
        /// </summary>
        private void FrostbiteTick(float dt, WarmthBalance w)
        {
            bool snow = currentRegion == RegionType.Snow;
            var weather = weatherByRegion[(int)currentRegion];

            // 1. Panas.
            float heat = snow ? 0f : w.ambientHeat;
            if (wornClothing != null) heat += w.clothesHeat;
            if (isTorchHeld) heat += w.torchHeat;
            if (IsNearLitCampfire()) heat += w.campfireHeat;
            if (isIndoors || isInInterior) heat += w.indoorHeat;

            // 2-5. Dingin dan batas bawah.
            float cold = 0f;
            float floor = 100f;
            bool hasBoundedCold = false;
            if (snow)
            {
                cold += w.regionSnowCold;
                if (weather == WeatherType.Blizzard) cold += w.blizzardCold;
                floor = 0f;
                hasBoundedCold = true;
            }
            else if (!isIndoors && !isInInterior && IsExposedToSky())
            {
                if (weather == WeatherType.Rain)
                {
                    cold += w.rainCold;
                    floor = Mathf.Min(floor, w.rainFloor);
                    hasBoundedCold = true;
                }
                else if (weather == WeatherType.Storm)
                {
                    cold += w.stormCold;
                    floor = Mathf.Min(floor, w.stormFloor);
                    hasBoundedCold = true;
                }
            }
            if (isInWater)
            {
                if (snow)
                {
                    cold += w.snowWaterCold;
                    floor = 0f;
                }
                else
                {
                    cold += w.waterCold;
                    floor = Mathf.Min(floor, w.waterFloor);
                }
                hasBoundedCold = true;
            }
            if (!hasBoundedCold) floor = 0f;

            // 6-7.
            float delta = (heat - cold) * dt;
            if (delta < 0f) Warmth = Mathf.Max(Warmth + delta, Mathf.Min(floor, Warmth));
            else Warmth = Mathf.Min(100f, Warmth + delta);

            // 8. Usulan 8 (disetujui developer): pakaian atau obor menahan Warmth di atas ambang beku.
            if (IsProtected()) Warmth = Mathf.Max(Warmth, w.protectedFloor);

            // 9.
            UpdateFreezing(w);
        }

        /// <summary>
        /// Keputusan developer (GDD bagian 5): pakaian hangat tetap melindungi di air, termasuk air es Region 3.
        /// Obor tidak melindungi di air, karena padam saat masuk air.
        /// </summary>
        private bool IsProtected() => wornClothing != null || (isTorchHeld && !isInWater);

        private void UpdateFreezing(WarmthBalance w)
        {
            bool before = IsFreezing;
            if (Warmth <= w.freezeEnterThreshold) IsFreezing = true;
            else if (Warmth >= w.freezeExitThreshold) IsFreezing = false;

            int rounded = Mathf.RoundToInt(Warmth);
            if (!hasPublished || IsFreezing != before || IsFreezing != lastPublishedFreezing || rounded != lastPublishedRounded)
                Publish(rounded);
        }

        private void Publish(int rounded)
        {
            hasPublished = true;
            lastPublishedRounded = rounded;
            lastPublishedFreezing = IsFreezing;
            EventChannel<WarmthChangedEvent>.Publish(new WarmthChangedEvent(Warmth, IsFreezing));
        }

        /// <summary>Ledakan Zombie Beku (#25, frozenBurstWarmthLoss 25). Pemain terlindung berhenti di protectedFloor.</summary>
        public void ApplyColdBurst(float amount)
        {
            var bal = Balance;
            if (bal == null) return;
            float min = IsProtected() ? bal.warmth.protectedFloor : 0f;
            Warmth = Mathf.Max(min, Warmth - amount);
            UpdateFreezing(bal.warmth);
        }

        /// <summary>Dipanggil TorchController saat obor menyala di tangan kiri atau disimpan.</summary>
        public void SetTorchHeld(bool held) => isTorchHeld = held;

        // ---------------------------------------------------------------- Bangunan dan langit

        private bool IsNearLitCampfire()
        {
            // TODO #44: CampfireManager.ActiveLit dalam campfire.warmthRadius.
            return false;
        }

        private void RefreshShelter(WarmthBalance w)
        {
            Vector3 origin = transform.position + Vector3.up * 1.2f;
            // 1. Atap: harus mengenai layer Structure.
            bool roof = PhysicsQuery.Raycast(new Ray(origin, Vector3.up), out var hit, w.shelterProbeDistance, PhysicsMasks.Ground)
                        && hit.collider.gameObject.layer == LayerMask.NameToLayer(PhysicsMasks.StructureLayer);
            if (!roof)
            {
                isIndoors = false;
                return;
            }
            // 2. Minimal tiga dari empat arah mendatar mengenai BaseWall atau BaseDoor (aturan tiga dinding BaseNavigation.md).
            int walls = 0;
            if (HitsWall(origin, transform.forward, w.shelterProbeDistance)) walls++;
            if (HitsWall(origin, -transform.forward, w.shelterProbeDistance)) walls++;
            if (HitsWall(origin, transform.right, w.shelterProbeDistance)) walls++;
            if (HitsWall(origin, -transform.right, w.shelterProbeDistance)) walls++;
            // 3.
            isIndoors = walls >= 3;
        }

        private static bool HitsWall(Vector3 origin, Vector3 dir, float dist)
        {
            if (!PhysicsQuery.Raycast(new Ray(origin, dir), out var hit, dist, PhysicsMasks.Ground)) return false;
            var go = hit.collider.gameObject;
            return go.CompareTag("BaseWall") || go.CompareTag("BaseDoor");
        }

        /// <summary>Raycast ke atas dengan PhysicsMasks.Sunlight tidak mengenai apa pun (sama dengan RainwaterCollector).</summary>
        public bool IsExposedToSky()
        {
            return !PhysicsQuery.Raycast(new Ray(transform.position + Vector3.up * 1.6f, Vector3.up), out _, 200f, PhysicsMasks.Sunlight);
        }

        public bool IsWeather(WeatherType w) => weatherByRegion[(int)currentRegion] == w;

        // ---------------------------------------------------------------- Pakaian hangat

        /// <summary>Aksi "Pakai" di ItemUseSystem (#66). Keluarkan item dari inventori; pakaian lama dilepas.</summary>
        public bool WearClothing(int instanceId)
        {
            var inv = InventoryManager.Instance;
            var inst = inv != null ? inv.FindInstance(instanceId) : null;
            var data = inst != null ? ItemDatabase.Get(inst.itemId) : null;
            if (data == null || !data.isWarmClothing) return false;

            var copy = inst.Clone();
            inv.RemoveInstance(instanceId);
            if (wornClothing != null) RemoveClothing();
            wornClothing = copy;
            return true;
        }

        /// <summary>Kembalikan pakaian ke inventori. Kalau penuh, menjadi WorldItem (#66).</summary>
        public void RemoveClothing()
        {
            if (wornClothing == null) return;
            var item = wornClothing;
            wornClothing = null;
            var inv = InventoryManager.Instance;
            int added = inv != null ? inv.AddItem(item, 1) : 0;
            if (added < 1 && WorldItemManager.Instance != null)
                WorldItemManager.Instance.Spawn(item, 1, transform.position + transform.forward, false);
        }

        // ---------------------------------------------------------------- Save (#80)

        [Serializable]
        private struct WarmthSaveData
        {
            public float warmth;
            public bool hasClothing;
            public ItemInstance clothing;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 70;

        public string CaptureState()
        {
            // IsFreezing tidak disimpan.
            return JsonUtility.ToJson(new WarmthSaveData
            {
                warmth = Warmth,
                hasClothing = wornClothing != null,
                clothing = wornClothing,
            });
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<WarmthSaveData>(json);
            Warmth = Mathf.Clamp(d.warmth, 0f, 100f);
            wornClothing = d.hasClothing ? d.clothing : null;
            hasPublished = false;
        }

        public void OnRestoreFinished()
        {
            // Tick langsung berjalan dan mengirim WarmthChangedEvent.
            var bal = Balance;
            if (bal != null) FrostbiteTick(0f, bal.warmth);
        }
    }
}
