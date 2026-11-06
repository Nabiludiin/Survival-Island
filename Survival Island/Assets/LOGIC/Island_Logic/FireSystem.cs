using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Aturan bersama api: daftar api aktif, tick damage dan penjalaran, batas jumlah, pengaruh cuaca, dan API sistem lain.
    /// Manager di scene IslandPhase, ISaveable (world_fire, LoadOrder 100). Blueprint: Island_Logic/FireSystem.md (#97).
    /// GDD bagian 5 (Erupsi, batu pijar menghancurkan area sekitar) dan bagian 6 (bencana memaksa perbaikan).
    /// Api menjalar dan semua angkanya adalah usulan yang disetujui developer (#85).
    ///
    /// Bukan milik sistem ini: terbakar sinar matahari (#63) dan jalur damage dasar (#59).
    /// </summary>
    public class FireSystem : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_fire";
        public const string AreaPoolTag = "FX_FireArea";
        public const string BurnPoolTag = "FX_FireBurn";

        public static FireSystem Instance { get; private set; }

        private static Collider[] spreadBuffer = new Collider[32];

        private readonly List<Flammable> burning = new List<Flammable>(32);
        private readonly List<FireArea> areas = new List<FireArea>(8);
        private float burnTimer;
        private float spreadTimer;
        private int spreadCursor;
        private bool registeredForSave;
        private FireRecord[] pendingRestore;

        public int BurningCount => burning.Count;
        public int AreaCount => areas.Count;

        private static FireBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.fire : null;

        /// <summary>Batas Flammable menyala bersamaan belum tercapai.</summary>
        public bool CanIgniteMore => burning.Count < (B != null ? B.maxBurning : 24);

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake() { Instance = this; }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            // Permadeath: padamkan semua dan kembalikan ke pool.
            for (int i = burning.Count - 1; i >= 0; i--) if (burning[i] != null) burning[i].Extinguish(true);
            burning.Clear();
            for (int i = areas.Count - 1; i >= 0; i--) if (areas[i] != null) areas[i].Release();
            areas.Clear();
        }

        // ---------------------------------------------------------------- API

        /// <summary>
        /// Area api sementara (Molotov #96, Fireball #70). Api baru selalu menang: kalau maxAreas tercapai, area dengan sisa
        /// waktu terkecil dikembalikan ke pool.
        /// </summary>
        public FireArea SpawnFireArea(Vector3 pos, float radius, float durationSeconds, float damagePerSec, bool byPlayer, bool carveNavMesh = false)
        {
            var b = B;
            var pooler = ObjectPooler.Instance;
            if (pooler == null) return null;
            // 1.
            int maxAreas = b != null ? b.maxAreas : 8;
            while (areas.Count >= maxAreas && areas.Count > 0)
            {
                int oldest = 0;
                for (int i = 1; i < areas.Count; i++) if (areas[i].Remaining < areas[oldest].Remaining) oldest = i;
                var old = areas[oldest];
                areas.RemoveAt(oldest);
                old.Release();
            }
            // 2.
            var area = pooler.SpawnFromPool<FireArea>(AreaPoolTag, pos, Quaternion.identity);
            if (area == null) return null;

            // 3. Atap dan cuaca.
            bool roofed = PhysicsQuery.Raycast(new Ray(pos + Vector3.up * 0.2f, Vector3.up), out _, b != null ? b.roofCheckDistance : 20f, PhysicsMasks.Sunlight);
            float duration = durationSeconds;
            if (!roofed)
            {
                RegionType r = RegionStreaming.RegionOf(pos);
                WeatherType w = RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(r) : WeatherType.Clear;
                if (w == WeatherType.Rain) duration *= b != null ? b.rainBurnMultiplier : 0.5f;
                else if (w == WeatherType.Storm || w == WeatherType.Blizzard) duration *= b != null ? b.stormAreaMultiplier : 0.25f;
            }

            // 4. Usulan 4: hanya Fireball yang memotong NavMesh, dibatasi maxCarvedAreas.
            bool carve = false;
            if (carveNavMesh)
            {
                int carved = 0;
                for (int i = 0; i < areas.Count; i++) if (areas[i].IsCarving) carved++;
                carve = carved < (b != null ? b.maxCarvedAreas : 4);
            }

            area.Setup(radius, duration, damagePerSec, byPlayer, roofed, carve);
            areas.Add(area);
            return area;
        }

        /// <summary>Ada FireArea aktif dalam radius + margin. Tanpa query fisika. Dipakai hewan dan NPC untuk menghindar.</summary>
        public bool IsNearFire(Vector3 pos, float margin)
        {
            for (int i = 0; i < areas.Count; i++)
            {
                var a = areas[i];
                if (a == null || !a.IsActive) continue;
                float r = a.Radius + margin;
                if ((a.transform.position - pos).sqrMagnitude <= r * r) return true;
            }
            return false;
        }

        /// <summary>Angka acak 0..1 dari aliran Gameplay (#89). Cadangan UnityEngine.Random di scene uji.</summary>
        public static float Roll01()
        {
            var seeds = RunSeedManager.Instance;
            if (seeds == null) return UnityEngine.Random.value;
            ref var rng = ref seeds.GetStream(RandomStreamId.Gameplay);
            return rng.NextFloat01();
        }

        // ---------------------------------------------------------------- Daftar (dipanggil Flammable dan FireArea)

        internal void RegisterBurning(Flammable f)
        {
            if (f != null && !burning.Contains(f)) burning.Add(f);
        }

        internal void UnregisterBurning(Flammable f) => burning.Remove(f);

        internal void UnregisterArea(FireArea a) => areas.Remove(a);

        internal BurnEffect AcquireBurnEffect(Flammable owner)
        {
            var pooler = ObjectPooler.Instance;
            if (pooler == null) return null;
            var fx = pooler.SpawnFromPool<BurnEffect>(BurnPoolTag, owner.FirePosition, Quaternion.identity);
            if (fx != null) fx.Bind(owner);
            return fx;
        }

        // ---------------------------------------------------------------- Tick

        private void Update()
        {
            float dt = Time.deltaTime;
            if (dt <= 0f) return;   // Jeda
            var b = B;

            // 1. Efek mengikuti Flammable musuh setiap frame.
            for (int i = 0; i < burning.Count; i++)
            {
                var f = burning[i];
                if (f != null && f.IsEnemy && f.ActiveEffect != null) f.ActiveEffect.transform.position = f.FirePosition;
            }

            // 2. Damage bakar.
            float burnTick = b != null ? b.burnTickSeconds : 0.5f;
            burnTimer += dt;
            if (burnTimer >= burnTick)
            {
                burnTimer -= burnTick;
                for (int i = burning.Count - 1; i >= 0; i--)
                {
                    if (i >= burning.Count) continue;
                    var f = burning[i];
                    if (f == null) { burning.RemoveAt(i); continue; }
                    f.TickBurn(burnTick, WeatherOf(f.HomeRegion));
                }
            }

            // 3. Penjalaran bergilir.
            float spreadTick = b != null ? b.spreadTickSeconds : 1f;
            spreadTimer += dt;
            if (spreadTimer >= spreadTick)
            {
                spreadTimer -= spreadTick;
                TickSpread(b != null ? b.spreadChecksPerTick : 6);
            }
        }

        private void TickSpread(int checks)
        {
            int n = Mathf.Min(checks, burning.Count);
            for (int k = 0; k < n; k++)
            {
                if (burning.Count == 0) return;
                if (spreadCursor >= burning.Count) spreadCursor = 0;
                var f = burning[spreadCursor++];
                if (f == null || f.Profile == null || !f.IsBurning) continue;
                float chance = f.Profile.spreadChancePerTick * f.SpreadWeatherMultiplier(WeatherOf(f.HomeRegion));
                if (chance <= 0f || f.Profile.spreadRadius <= 0f) continue;
                int count = PhysicsQuery.OverlapSphere(f.FirePosition, f.Profile.spreadRadius, PhysicsMasks.FireReach, spreadBuffer);
                for (int i = 0; i < count; i++)
                {
                    var target = spreadBuffer[i] != null ? spreadBuffer[i].GetComponentInParent<Flammable>() : null;
                    spreadBuffer[i] = null;
                    if (target == null || target == f || target.IsBurning) continue;
                    target.TryIgnite(chance, f.ByPlayer, f.Generation + 1);
                }
            }
        }

        private static WeatherType WeatherOf(RegionType r)
        {
            return RegionalWeather.Instance != null ? RegionalWeather.Instance.GetWeather(r) : WeatherType.Clear;
        }

        // ---------------------------------------------------------------- Save (world_fire)

        [Serializable]
        private class FireRecord
        {
            public string structureId;
            public float burnRemaining;
            public int generation;
            public bool byPlayer;
        }

        [Serializable]
        private class SaveData
        {
            public FireRecord[] fires;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 100;   // Setelah world_structures (WorldStateSave.md)

        /// <summary>Hanya api struktur. FireArea dan api pada musuh tidak disimpan.</summary>
        public string CaptureState()
        {
            int count = 0;
            for (int i = 0; i < burning.Count; i++) if (burning[i] != null && burning[i].IsStructure) count++;
            var d = new SaveData { fires = new FireRecord[count] };
            int n = 0;
            for (int i = 0; i < burning.Count; i++)
            {
                var f = burning[i];
                if (f == null || !f.IsStructure) continue;
                d.fires[n++] = new FireRecord { structureId = f.StructureId, burnRemaining = f.BurnRemaining, generation = f.Generation, byPlayer = f.ByPlayer };
            }
            return JsonUtility.ToJson(d);
        }

        public void RestoreState(string json)
        {
            pendingRestore = null;
            if (string.IsNullOrEmpty(json)) return;
            SaveData d = null;
            try { d = JsonUtility.FromJson<SaveData>(json); }
            catch (Exception e) { Debug.LogWarning($"[FireSystem] Save tidak terbaca: {e.Message}"); }
            pendingRestore = d != null ? d.fires : null;
        }

        /// <summary>Api struktur dinyalakan lagi tanpa lempar peluang.</summary>
        public void OnRestoreFinished()
        {
            if (pendingRestore == null) return;
            var list = Flammable.Structures;
            for (int i = 0; i < pendingRestore.Length; i++)
            {
                var r = pendingRestore[i];
                if (r == null || string.IsNullOrEmpty(r.structureId) || r.burnRemaining <= 0f) continue;
                for (int k = 0; k < list.Count; k++)
                {
                    var f = list[k];
                    if (f == null || !string.Equals(f.StructureId, r.structureId, StringComparison.Ordinal)) continue;
                    f.ForceIgnite(r.burnRemaining, r.byPlayer, r.generation);
                    break;
                }
            }
            pendingRestore = null;
        }
    }
}
