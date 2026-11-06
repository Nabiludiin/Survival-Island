using Unity.Cinemachine;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu pintu getaran kamera: tembakan, terkena serangan, ledakan, gempa, dan kapal.
    /// Blueprint: Core_Logic/CameraShake.md (#24). Mendukung GDD bagian 3 (kapal dan combat), bagian 5 (erupsi),
    /// dan bagian 6 (gempa). Kekuatan getaran adalah usulan teknis.
    ///
    /// Berada di prefab rig kamera (#102), jadi ada per scene fase. Tidak memakai Virtual Camera atau CinemachineBrain:
    /// sinyal dibuat CinemachineImpulseSource dan dibaca lewat CinemachineImpulseManager.GetImpulseAt, lalu
    /// diserahkan ke FirstPersonCamera sebagai ICameraOffsetSource. Memakai Cinemachine 3.x (Unity.Cinemachine).
    /// </summary>
    [DefaultExecutionOrder(150)]   // Sebelum FirstPersonCamera (200)
    public class CameraShake : MonoBehaviour, ICameraOffsetSource
    {
        public static CameraShake Instance { get; private set; }

        [SerializeField] private CameraShakeProfile profile;
        [Tooltip("Satu per CameraShakeKind, urut sesuai enum. Yang kosong dibuat otomatis di objek ini saat Awake.")]
        [SerializeField] private CinemachineImpulseSource[] sources = new CinemachineImpulseSource[0];
        [Tooltip("Rig kamera. Kosong = dicari di induk, lalu FirstPersonCamera.Instance.")]
        [SerializeField] private FirstPersonCamera fpCamera;

        private CameraShakePreset[] lookup;
        private bool[] hasPreset;
        private float[] nextAllowedTime;
        private bool isDead;
        private bool registered;

        public Vector3 EulerOffset { get; private set; }
        public Vector3 PositionOffset { get; private set; }

        private static int KindCount => System.Enum.GetValues(typeof(CameraShakeKind)).Length;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (fpCamera == null) fpCamera = GetComponentInParent<FirstPersonCamera>();

            int n = KindCount;
            lookup = new CameraShakePreset[n];
            hasPreset = new bool[n];
            nextAllowedTime = new float[n];
            if (profile != null && profile.presets != null)
            {
                for (int i = 0; i < profile.presets.Length; i++)
                {
                    int k = (int)profile.presets[i].kind;
                    if (k < 0 || k >= n) continue;
                    lookup[k] = profile.presets[i];
                    hasPreset[k] = true;
                }
            }
#if UNITY_EDITOR
            else Debug.LogWarning("[CameraShake] profile kosong. Semua getaran dilewati.", this);
#endif

            // Sumber impuls jumlahnya tetap, satu per jenis. ImpulseDefinition diisi dari preset.
            if (sources == null || sources.Length != n)
            {
                var old = sources;
                sources = new CinemachineImpulseSource[n];
                if (old != null) for (int i = 0; i < old.Length && i < n; i++) sources[i] = old[i];
            }
            for (int k = 0; k < n; k++)
            {
                if (!hasPreset[k]) continue;
                if (sources[k] == null) sources[k] = gameObject.AddComponent<CinemachineImpulseSource>();
                ConfigureSource(sources[k], lookup[k]);
            }

            // Kasus Khusus: waktu impuls mengikuti Time.time, jadi getaran berhenti saat jeda dan lanjut setelahnya.
            CinemachineImpulseManager.Instance.IgnoreTimeScale = false;
        }

        private static void ConfigureSource(CinemachineImpulseSource source, in CameraShakePreset p)
        {
            var def = source.ImpulseDefinition;
            def.ImpulseChannel = (int)p.channel;
            def.ImpulseShape = p.shape;
            def.ImpulseDuration = p.duration;
            // Jarak dihitung sendiri (keputusan teknis #24), jadi impuls seragam tanpa dissipation bawaan.
            def.ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform;
        }

        private void OnEnable()
        {
            EventChannel<WeaponFiredEvent>.Raised += HandleWeaponFired;
            EventChannel<PlayerDamagedEvent>.Raised += HandleDamaged;
            EventChannel<PlayerDiedEvent>.Raised += HandleDied;
            TryRegister();
        }

        private void Start()
        {
            // FirstPersonCamera bisa belum Awake saat OnEnable pertama.
            TryRegister();
        }

        private void OnDisable()
        {
            EventChannel<WeaponFiredEvent>.Raised -= HandleWeaponFired;
            EventChannel<PlayerDamagedEvent>.Raised -= HandleDamaged;
            EventChannel<PlayerDiedEvent>.Raised -= HandleDied;

            var cam = Rig;
            if (cam != null) cam.UnregisterOffsetSource(this);
            registered = false;

            // Manager Impulse statis; impuls lama tidak boleh terbawa ke scene berikutnya.
            CinemachineImpulseManager.Instance.Clear();
            EulerOffset = Vector3.zero;
            PositionOffset = Vector3.zero;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private FirstPersonCamera Rig => fpCamera != null ? fpCamera : FirstPersonCamera.Instance;

        private void TryRegister()
        {
            if (registered) return;
            var cam = Rig;
            if (cam == null) return;
            cam.RegisterOffsetSource(this);
            registered = true;
        }

        // ---------------------------------------------------------------- Fungsi publik (statis)

        /// <summary>Getaran tanpa jarak. Di menu (tanpa rig) tidak melakukan apa-apa.</summary>
        public static void Play(CameraShakeKind kind, float scale = 1f)
        {
            var s = Instance;
            if (s == null || !s.TryGetPreset(kind, out var p)) return;
            s.Emit(kind, scale, p.direction);
        }

        /// <summary>Getaran dengan jarak: scale *= 1 - d / falloffRadius; di luar radius tidak ada impuls.</summary>
        public static void PlayAt(CameraShakeKind kind, Vector3 worldPos, float scale = 1f)
        {
            var s = Instance;
            if (s == null || !s.TryGetPreset(kind, out var p)) return;
            if (p.falloffRadius > 0f)
            {
                float d = Vector3.Distance(s.CameraPosition, worldPos);
                if (d >= p.falloffRadius) return;
                scale *= 1f - d / p.falloffRadius;
            }
            s.Emit(kind, scale, p.direction);
        }

        /// <summary>Getaran dari arah serangan (arah dunia, dari sumber ke pemain).</summary>
        public static void PlayDirectional(CameraShakeKind kind, Vector3 worldDirection, float scale = 1f)
        {
            var s = Instance;
            if (s == null || !s.TryGetPreset(kind, out var p)) return;
            Vector3 local = worldDirection.sqrMagnitude > 0.0001f
                ? s.CameraTransform.InverseTransformDirection(worldDirection.normalized)
                : p.direction;
            s.Emit(kind, scale, local);
        }

        // ---------------------------------------------------------------- Inti

        private bool TryGetPreset(CameraShakeKind kind, out CameraShakePreset preset)
        {
            int k = (int)kind;
            if (lookup == null || k < 0 || k >= lookup.Length || !hasPreset[k] || sources[k] == null)
            {
                preset = default;
#if UNITY_EDITOR
                Debug.LogWarning($"[CameraShake] Preset atau sumber untuk {kind} tidak ada.", this);
#endif
                return false;
            }
            preset = lookup[k];
            return true;
        }

        private Transform CameraTransform
        {
            get
            {
                var cam = Rig;
                if (cam != null && cam.WorldCamera != null) return cam.WorldCamera.transform;
                return transform;
            }
        }

        private Vector3 CameraPosition => CameraTransform.position;

        private void Emit(CameraShakeKind kind, float scale, Vector3 localDirection)
        {
            if (isDead) return;

            // 1. Pilihan aksesibilitas #104: 0 mematikan getaran. Recoil #58 tetap berjalan.
            float settingScale = LocalizationManager.Instance != null ? LocalizationManager.Instance.Accessibility.cameraShakeScale : 1f;
            if (settingScale <= 0f) return;

            // 2. Cooldown per jenis.
            int k = (int)kind;
            var p = lookup[k];
            if (Time.time < nextAllowedTime[k]) return;
            nextAllowedTime[k] = Time.time + p.cooldownSeconds;

            // 3-4. Sinyal dianggap ruang lokal kamera (x = yaw, y = pitch), dibuat di posisi kamera.
            Vector3 velocity = localDirection * (p.force * scale * settingScale);
            sources[k].GenerateImpulseAt(CameraPosition, velocity);
        }

        // ---------------------------------------------------------------- Membaca sinyal

        private void LateUpdate()
        {
            if (isDead) return;
            var b = GameManager.CurrentBalance;
            if (b == null) return;
            var sh = b.shake;

            var mgr = CinemachineImpulseManager.Instance;
            Vector3 camPos = CameraPosition;
            mgr.GetImpulseAt(camPos, false, (int)ShakeChannel.Aim, out Vector3 aimPos, out Quaternion aimRot);
            mgr.GetImpulseAt(camPos, false, (int)ShakeChannel.Visual, out Vector3 visPos, out Quaternion _);

            // Channel Aim menggeser pitch dan yaw; Visual hanya posisi dan roll, jadi bidikan GunFire tidak bergeser
            // (usulan disetujui developer #24 poin 2).
            Vector3 euler = new Vector3(-aimPos.y * sh.degreesPerUnit, aimPos.x * sh.degreesPerUnit,
                                        (aimPos.x + visPos.x) * sh.rollPerUnit);
            euler += NormalizeEuler(aimRot.eulerAngles);   // Nol kalau sinyal hanya berisi posisi

            EulerOffset = ClampPerAxis(euler, sh.maxDegrees);
            PositionOffset = Vector3.ClampMagnitude((aimPos + visPos) * sh.metersPerUnit, sh.maxMeters);
        }

        private static Vector3 NormalizeEuler(Vector3 e)
        {
            return new Vector3(Mathf.DeltaAngle(0f, e.x), Mathf.DeltaAngle(0f, e.y), Mathf.DeltaAngle(0f, e.z));
        }

        private static Vector3 ClampPerAxis(Vector3 v, float max)
        {
            return new Vector3(Mathf.Clamp(v.x, -max, max), Mathf.Clamp(v.y, -max, max), Mathf.Clamp(v.z, -max, max));
        }

        // ---------------------------------------------------------------- Event

        private void HandleWeaponFired(WeaponFiredEvent e)
        {
            if (!e.byPlayer || !TryGetPreset(CameraShakeKind.GunFire, out var p)) return;
            // Usulan disetujui developer (#24 poin 3): kekuatan per weapon dari WeaponData.cameraShakeScale.
            var item = ItemDatabase.Get(e.itemId);
            float scale = item != null && item.weaponData != null ? item.weaponData.cameraShakeScale : 1f;
            Vector3 dir = p.direction;
            dir.x = (Random.value < 0.5f ? -1f : 1f) * Mathf.Abs(dir.x);   // Sisi kiri atau kanan acak
            Emit(CameraShakeKind.GunFire, scale, dir);
        }

        private void HandleDamaged(PlayerDamagedEvent e)
        {
            CameraShakeKind kind;
            switch (e.info.type)
            {
                case DamageType.Melee:
                    kind = e.info.infection > 0f ? CameraShakeKind.ZombieBite : CameraShakeKind.PlayerHit;
                    break;
                case DamageType.Bullet:
                    kind = CameraShakeKind.PlayerHit;
                    break;
                // Explosion: pengirim ledakan sudah memanggil PlayAt. Fall: dip kamera milik #101.
                // Damage berkala tidak bergetar (usulan disetujui developer #24 poin 4).
                default:
                    return;
            }

            var b = GameManager.CurrentBalance;
            if (b == null) return;
            float scale = Mathf.Clamp(e.info.amount / Mathf.Max(0.0001f, b.shake.hitReferenceDamage), b.shake.hitScaleMin, b.shake.hitScaleMax);
            PlayDirectional(kind, e.info.hitDirection, scale);
        }

        private void HandleDied(PlayerDiedEvent e)
        {
            // Sesuai DetachForDeath (#102): getaran berhenti, kamera kematian stabil.
            isDead = true;
            CinemachineImpulseManager.Instance.Clear();
            EulerOffset = Vector3.zero;
            PositionOffset = Vector3.zero;
        }

        // Perintah "shake <kind> [scale]" ada di DevConsole.CmdShake (#56).
    }
}
