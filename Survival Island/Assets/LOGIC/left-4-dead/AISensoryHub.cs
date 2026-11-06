using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pembagi tick AISensory, nilai UI Detection Meter, dan titik perburuan malam. Blueprint: left-4-dead/AISensory.md (#49).
    /// Singleton per scene IslandPhase.
    /// IgnoreStealth saat Blood Moon diisi HordeManager (#50). Tarikan cahaya SetLightAttraction (#98) diisi LightSourceSystem.
    /// Belum diimplementasikan (TODO): DetectionMeterUI (HUD).
    /// </summary>
    public class AISensoryHub : MonoBehaviour
    {
        public static AISensoryHub Instance { get; private set; }

        private readonly List<AISensory> sensors = new List<AISensory>(64);
        private int cursor;
        private float externalVisibility = 1f;
        private Vector3 lightAttractionPoint;
        private float lightAttractionChance;
        private float huntTimer;
        private float uiTimer;
        private float lastSentLevel = -1f;

        /// <summary>Blood Moon: semua sensor Aware (#50). Diisi HordeManager setelah ditulis.</summary>
        public bool IgnoreStealth { get; set; }
        public float WeatherSight { get; private set; } = 1f;
        public float WeatherHearing { get; private set; } = 1f;
        public Vector3 HuntHint { get; private set; }

        public Vector3 PlayerEyePosition
        {
            get
            {
                var cam = FirstPersonCamera.Instance != null ? FirstPersonCamera.Instance.WorldCamera : null;
                if (cam != null) return cam.transform.position;
                var p = PlayerTargetable.Instance;
                return p != null ? p.Position + Vector3.up * 1.6f : Vector3.zero;
            }
        }

        /// <summary>Pengali terlihat pemain: jongkok, lari, dan modifier luar (#98, senter).</summary>
        public float PlayerVisibility
        {
            get
            {
                var bal = GameManager.CurrentBalance;
                if (bal == null) return externalVisibility;
                var pc = IslandPlayerController.Instance;
                float v = 1f;
                if (pc != null)
                {
                    var trav = pc.GetComponent<PlayerTraversal>();
                    if (trav != null && trav.IsCrouching) v = bal.stealth.crouchVisibility;
                    else if (pc.IsSprinting) v = bal.stealth.sprintVisibility;
                }
                return v * externalVisibility;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this) { Debug.LogError("[AISensoryHub] Lebih dari satu instance."); enabled = false; return; }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        public void Register(AISensory s)
        {
            if (s == null || s.hubIndex >= 0) return;
            s.hubIndex = sensors.Count;
            s.lastTickTime = Time.time;
            sensors.Add(s);
        }

        public void Unregister(AISensory s)
        {
            if (s == null || s.hubIndex < 0 || s.hubIndex >= sensors.Count || sensors[s.hubIndex] != s) { if (s != null) s.hubIndex = -1; return; }
            int i = s.hubIndex;
            int last = sensors.Count - 1;
            sensors[i] = sensors[last];
            sensors[i].hubIndex = i;
            sensors.RemoveAt(last);
            s.hubIndex = -1;
        }

        public void SetVisibilityModifier(float value) => externalVisibility = Mathf.Max(0f, value);
        public void SetWeatherModifiers(float sight, float hearing) { WeatherSight = Mathf.Max(0f, sight); WeatherHearing = Mathf.Max(0f, hearing); }

        /// <summary>Titik tarikan cahaya base dan peluangnya untuk HuntHint (#98). Peluang 0 mematikan tarikan.</summary>
        public void SetLightAttraction(Vector3 point, float chance)
        {
            lightAttractionPoint = point;
            lightAttractionChance = Mathf.Clamp01(chance);
        }

        private void Update()
        {
            if (Time.deltaTime <= 0f) return;
            var bal = GameManager.CurrentBalance;
            if (bal == null) return;
            var s = bal.stealth;
            float now = Time.time;

            // Titik perburuan: posisi pemain dengan geseran acak (huntHintRadius), diperbarui tiap huntHintSeconds.
            huntTimer -= Time.deltaTime;
            if (huntTimer <= 0f && PlayerTargetable.Instance != null)
            {
                huntTimer = s.huntHintSeconds;
                // Geseran perburuan tidak memengaruhi isi dunia yang disimpan, jadi memakai UnityEngine.Random.
                Vector2 off = Random.insideUnitCircle * s.huntHintRadius;
                HuntHint = PlayerTargetable.Instance.Position + new Vector3(off.x, 0f, off.y);
                // AISensory.md (#98): tarikan cahaya aktif, lempar peluang dengan aliran Gameplay; kena = titik tarikan.
                if (lightAttractionChance > 0f)
                {
                    var seeds = RunSeedManager.Instance;
                    float roll = seeds != null ? seeds.GetStream(RandomStreamId.Gameplay).NextFloat01() : Random.value;
                    if (roll < lightAttractionChance) HuntHint = lightAttractionPoint;
                }
            }

            int count = sensors.Count;
            int processed = 0;
            for (int n = 0; n < count && processed < s.sensorsPerFrame; n++)
            {
                if (cursor >= sensors.Count) cursor = 0;
                var sensor = sensors[cursor++];
                if (sensor == null) continue;
                float dt = now - sensor.lastTickTime;
                if (dt < s.sensorTickSeconds) continue;
                sensor.lastTickTime = now;
                sensor.Tick(Mathf.Min(dt, 1f));
                processed++;
            }

            // Nilai UI: Detection01 tertinggi tiap 0,25 detik, dikirim kalau berubah lebih dari 0,02.
            uiTimer -= Time.deltaTime;
            if (uiTimer <= 0f)
            {
                uiTimer = 0.25f;
                float max = 0f;
                for (int i = 0; i < sensors.Count; i++) if (sensors[i] != null && sensors[i].Detection01 > max) max = sensors[i].Detection01;
                if (Mathf.Abs(max - lastSentLevel) > 0.02f)
                {
                    lastSentLevel = max;
                    EventChannel<PlayerDetectionChangedEvent>.Publish(new PlayerDetectionChangedEvent(max, max >= 1f));
                }
            }
        }
    }
}
