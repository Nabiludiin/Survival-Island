using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace SurvivalIsland
{
    public enum ShadowQuality { Off, Low, Medium, High }
    public enum TextureQuality { Quarter, Half, Full }
    public enum QualityPreset { Low, Medium, High, Custom }
    public enum AudioChannel { Master, BGM, SFX }

    /// <summary>
    /// Data setting pemain, disimpan sebagai satu JSON di PlayerPrefs (SettingsManager.md bagian 1).
    /// Nama dan nilai awal setting kamera mengikuti FirstPersonCamera.md "Pengaturan pemain dari #19".
    /// </summary>
    [Serializable]
    public class SettingsData
    {
        public int version = 1;
        public float mouseSensitivity = 0.12f;
        public bool invertY = false;
        public float fieldOfView = 70f;
        public bool headBobEnabled = true;
        public bool chromaticAberrationEnabled = true;   // Dibaca PostProcessController tiap frame (#23)
        public bool holdToStruggle = false;              // Pilihan aksesibilitas #110
        public bool weaponSwayEnabled = true;
        public bool fovKickEnabled = true;
        public float masterVolume = 1f;
        public float bgmVolume = 1f;
        public float sfxVolume = 1f;
        public QualityPreset qualityPreset = QualityPreset.High;
        public ShadowQuality shadowQuality = ShadowQuality.High;
        public TextureQuality textureQuality = TextureQuality.Full;
        public float brightness = 1f;
        public bool tutorialEnabled = true;
        public bool showHitMarkers = true;               // Usulan 4 CombatFeedbackUI (#51): nilai awal aktif

        /// <summary>Salinan dangkal. Semua kolom bertipe nilai, jadi salinan ini lepas dari aslinya.</summary>
        public SettingsData Clone() => (SettingsData)MemberwiseClone();
    }

    /// <summary>Satu baris tabel "Pemetaan kualitas" bayangan, ditambah usulan batas lampu #98.</summary>
    [Serializable]
    public struct ShadowQualityLevel
    {
        public ShadowQuality quality;
        public UnityEngine.ShadowQuality unityShadows;
        public UnityEngine.ShadowResolution resolution;
        public bool keepResolution;    // Off: resolusi tetap
        public float shadowDistance;
        public int lights;             // Usulan batas lampu (#98)
        public int lightShadows;

        public ShadowQualityLevel(ShadowQuality q, UnityEngine.ShadowQuality s, UnityEngine.ShadowResolution r, bool keep,
            float distance, int lights, int lightShadows)
        {
            quality = q; unityShadows = s; resolution = r; keepResolution = keep;
            shadowDistance = distance; this.lights = lights; this.lightShadows = lightShadows;
        }
    }

    /// <summary>
    /// Pemilik preferensi pemain: kontrol, volume, grafis, tampilan, dan tutorial.
    /// Blueprint: Core_Logic/SettingsManager.md (#19). Tidak disebut langsung di GDD.
    ///
    /// Urutan bootstrap ke-3 (RunLifecycle.md). Disimpan di PlayerPrefs terpisah dari save run, jadi permadeath
    /// (GDD bagian 1 dan 3) tidak menghapusnya. Menu memanggil fungsi kelas ini, dan tidak menulis QualitySettings
    /// atau PlayerPrefs sendiri.
    /// </summary>
    public class SettingsManager : MonoBehaviour, IBootstrapService
    {
        public const string PrefsKey = "settings.data";

        public static SettingsManager Instance { get; private set; }

        [Tooltip("Suara contoh saat slider SFX dilepas. Diputar lewat AudioManager.PlaySFX (ignoreListenerPause).")]
        [SerializeField] private AudioClip sfxPreviewClip;

        [Tooltip("Tabel 'Pemetaan kualitas' SettingsManager.md, satu baris per ShadowQuality. Data prefab.")]
        [SerializeField] private ShadowQualityLevel[] shadowLevels =
        {
            new ShadowQualityLevel(ShadowQuality.Off, UnityEngine.ShadowQuality.Disable, UnityEngine.ShadowResolution.Low, true, 0f, 3, 0),
            new ShadowQualityLevel(ShadowQuality.Low, UnityEngine.ShadowQuality.HardOnly, UnityEngine.ShadowResolution.Low, false, 40f, 4, 0),
            new ShadowQualityLevel(ShadowQuality.Medium, UnityEngine.ShadowQuality.All, UnityEngine.ShadowResolution.Medium, false, 70f, 5, 1),
            new ShadowQualityLevel(ShadowQuality.High, UnityEngine.ShadowQuality.All, UnityEngine.ShadowResolution.High, false, 120f, 6, 1),
        };

        private SettingsData data = new SettingsData();
        private SettingsData applied = new SettingsData();
        private bool isDirty;

        // Nilai grafis yang terakhir benar-benar ditulis ke QualitySettings.
        private ShadowQuality appliedShadow = (ShadowQuality)(-1);
        private TextureQuality appliedTexture = (TextureQuality)(-1);

        private FirstPersonCamera lastCamera;
        private bool initialized;

        // ---------------------------------------------------------------- Properti baca

        public float mouseSensitivity => data.mouseSensitivity;
        public bool invertY => data.invertY;
        public float fieldOfView => data.fieldOfView;
        public bool headBobEnabled => data.headBobEnabled;
        public bool chromaticAberrationEnabled => data.chromaticAberrationEnabled;
        public bool holdToStruggle => data.holdToStruggle;
        public bool weaponSwayEnabled => data.weaponSwayEnabled;
        public bool fovKickEnabled => data.fovKickEnabled;
        public float masterVolume => data.masterVolume;
        public float bgmVolume => data.bgmVolume;
        public float sfxVolume => data.sfxVolume;
        public QualityPreset qualityPreset => data.qualityPreset;
        public ShadowQuality shadowQuality => data.shadowQuality;
        public TextureQuality textureQuality => data.textureQuality;
        public float brightness => data.brightness;
        public bool tutorialEnabled => data.tutorialEnabled;
        public bool showHitMarkers => data.showHitMarkers;

        /// <summary>Ada perubahan yang belum di-Commit.</summary>
        public bool IsDirty => isDirty;

        /// <summary>Grafis di data berbeda dengan yang sedang berlaku (tombol Terapkan perlu menyala).</summary>
        public bool HasPendingGraphics => data.shadowQuality != appliedShadow || data.textureQuality != appliedTexture;

        /// <summary>Setting kamera dalam bentuk yang dipakai FirstPersonCamera (#102).</summary>
        public PlayerViewSettings ViewSettings => new PlayerViewSettings
        {
            mouseSensitivity = data.mouseSensitivity,
            invertY = data.invertY,
            fieldOfView = data.fieldOfView,
            headBobEnabled = data.headBobEnabled,
            weaponSwayEnabled = data.weaponSwayEnabled,
            fovKickEnabled = data.fovKickEnabled
        };

        private static SettingsBalance Limits => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.settings : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[SettingsManager] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void Update()
        {
            // Rig kamera dibuat ulang tiap scene fase. Setting dikirim sekali saat rig baru muncul.
            var cam = FirstPersonCamera.Instance;
            if (initialized && cam != null && cam != lastCamera)
            {
                lastCamera = cam;
                cam.SetViewSettings(ViewSettings);
            }
        }

        // ---------------------------------------------------------------- Layanan

        public void Initialize()
        {
            Load();
            ClampAll();
            ApplyGraphics();
            // Audio tidak diterapkan di sini: AudioManager (urutan ke-4) membaca volume dari kelas ini saat Initialize.
            initialized = true;
            PushViewSettings();
            EventChannel<SettingsChangedEvent>.Publish(new SettingsChangedEvent(SettingsCategory.All));
        }

        /// <summary>Permadeath: setting tetap. Hanya menerbitkan ulang event sticky yang dihapus GameEventRegistry.ClearAll.</summary>
        public void ResetForNewRun()
        {
            EventChannel<SettingsChangedEvent>.Publish(new SettingsChangedEvent(SettingsCategory.All));
        }

        private void Load()
        {
            data = new SettingsData();
            if (PlayerPrefs.HasKey(PrefsKey))
            {
                try
                {
                    // FromJsonOverwrite: kolom yang belum ada di versi lama tetap memakai nilai bawaan.
                    JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(PrefsKey), data);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"[SettingsManager] '{PrefsKey}' rusak, memakai nilai bawaan. {ex.Message}");
                    data = new SettingsData();
                }
            }
            // version 1 belum memerlukan migrasi.
            applied = data.Clone();
            isDirty = false;
        }

        private void ClampAll()
        {
            var l = Limits;
            if (l != null)
            {
                data.mouseSensitivity = Mathf.Clamp(data.mouseSensitivity, l.sensitivityMin, l.sensitivityMax);
                data.fieldOfView = Mathf.Clamp(data.fieldOfView, l.fovMin, l.fovMax);
                data.brightness = Mathf.Clamp(data.brightness, l.brightnessMin, l.brightnessMax);
            }
            data.masterVolume = Mathf.Clamp01(data.masterVolume);
            data.bgmVolume = Mathf.Clamp01(data.bgmVolume);
            data.sfxVolume = Mathf.Clamp01(data.sfxVolume);
            if (!Enum.IsDefined(typeof(ShadowQuality), data.shadowQuality)) data.shadowQuality = ShadowQuality.High;
            if (!Enum.IsDefined(typeof(TextureQuality), data.textureQuality)) data.textureQuality = TextureQuality.Full;
            if (!Enum.IsDefined(typeof(QualityPreset), data.qualityPreset)) data.qualityPreset = QualityPreset.Custom;
            applied = data.Clone();
        }

        // ---------------------------------------------------------------- Setter langsung berlaku

        // Usulan disetujui developer (#19): sensitivitas, tampilan, dan volume langsung berlaku; grafis lewat Terapkan.

        public void SetMouseSensitivity(float value)
        {
            var l = Limits;
            data.mouseSensitivity = l != null ? Mathf.Clamp(value, l.sensitivityMin, l.sensitivityMax) : value;
            MarkViewChanged();
        }

        public void SetInvertY(bool value) { data.invertY = value; MarkViewChanged(); }

        public void SetFieldOfView(float value)
        {
            var l = Limits;
            data.fieldOfView = l != null ? Mathf.Clamp(value, l.fovMin, l.fovMax) : value;
            MarkViewChanged();
        }

        public void SetHeadBob(bool value) { data.headBobEnabled = value; MarkViewChanged(); }
        public void SetWeaponSway(bool value) { data.weaponSwayEnabled = value; MarkViewChanged(); }
        public void SetFovKick(bool value) { data.fovKickEnabled = value; MarkViewChanged(); }
        public void SetChromaticAberration(bool value) { data.chromaticAberrationEnabled = value; isDirty = true; }
        public void SetHoldToStruggle(bool value) { data.holdToStruggle = value; isDirty = true; }
        public void SetTutorialEnabled(bool value) { data.tutorialEnabled = value; isDirty = true; }
        public void SetShowHitMarkers(bool value) { data.showHitMarkers = value; isDirty = true; }   // Usulan 4 #51

        /// <summary>Dibaca EnvironmentDirector (#92), yang mengalikan ambient dengan nilai ini.</summary>
        public void SetBrightness(float value)
        {
            var l = Limits;
            data.brightness = l != null ? Mathf.Clamp(value, l.brightnessMin, l.brightnessMax) : value;
            isDirty = true;
        }

        public void SetVolume(AudioChannel channel, float value)
        {
            value = Mathf.Clamp01(value);
            switch (channel)
            {
                case AudioChannel.Master: data.masterVolume = value; break;
                case AudioChannel.BGM: data.bgmVolume = value; break;
                case AudioChannel.SFX: data.sfxVolume = value; break;
            }
            isDirty = true;
            ApplyAudio();
        }

        public float GetVolume(AudioChannel channel)
        {
            switch (channel)
            {
                case AudioChannel.BGM: return data.bgmVolume;
                case AudioChannel.SFX: return data.sfxVolume;
                default: return data.masterVolume;
            }
        }

        /// <summary>Dipanggil menu saat slider SFX dilepas. Tetap terdengar walau AudioListener.pause aktif (#83).</summary>
        public void PlaySfxPreview()
        {
            if (sfxPreviewClip != null && AudioManager.Instance != null) AudioManager.Instance.PlaySFX(sfxPreviewClip);
        }

        // ---------------------------------------------------------------- Grafis (menunggu Terapkan)

        public void SetQualityPreset(QualityPreset preset)
        {
            data.qualityPreset = preset;
            switch (preset)
            {
                case QualityPreset.Low: data.shadowQuality = ShadowQuality.Low; data.textureQuality = TextureQuality.Quarter; break;
                case QualityPreset.Medium: data.shadowQuality = ShadowQuality.Medium; data.textureQuality = TextureQuality.Half; break;
                case QualityPreset.High: data.shadowQuality = ShadowQuality.High; data.textureQuality = TextureQuality.Full; break;
            }
            isDirty = true;
        }

        public void SetShadowQuality(ShadowQuality q)
        {
            data.shadowQuality = q;
            data.qualityPreset = QualityPreset.Custom;
            isDirty = true;
        }

        public void SetTextureQuality(TextureQuality q)
        {
            data.textureQuality = q;
            data.qualityPreset = QualityPreset.Custom;
            isDirty = true;
        }

        /// <summary>
        /// Tulis grafis ke QualitySettings dan aset URP. Perubahan tekstur bisa membuat game tersendat sesaat,
        /// jadi hanya dipanggil dari tombol Terapkan, Commit, dan Initialize.
        /// </summary>
        public void ApplyGraphics()
        {
            var level = FindShadowLevel(data.shadowQuality);

            QualitySettings.shadows = level.unityShadows;
            if (!level.keepResolution) QualitySettings.shadowResolution = level.resolution;
            QualitySettings.shadowDistance = level.shadowDistance;

            // Usulan #19 poin 3: proyek memakai URP, jadi jarak bayangan dan jumlah lampu juga ditulis ke aset URP.
            // Catatan: ini mengubah aset URP aktif. Di Editor perubahan ikut tersimpan ke aset (lihat Catatan Implementasi).
            if (GraphicsSettings.currentRenderPipeline is UniversalRenderPipelineAsset urp)
            {
                urp.shadowDistance = level.shadowDistance;
                urp.maxAdditionalLightsCount = level.lights;
            }

            switch (data.textureQuality)
            {
                case TextureQuality.Full: QualitySettings.globalTextureMipmapLimit = 0; break;
                case TextureQuality.Half: QualitySettings.globalTextureMipmapLimit = 1; break;
                case TextureQuality.Quarter: QualitySettings.globalTextureMipmapLimit = 2; break;
            }

            LightSourceSystem.Instance?.ApplyQuality(level.lights, level.lightShadows);   // Batas lampu (#98)

            appliedShadow = data.shadowQuality;
            appliedTexture = data.textureQuality;
            EventChannel<SettingsChangedEvent>.Publish(new SettingsChangedEvent(SettingsCategory.Graphics));
        }

        private ShadowQualityLevel FindShadowLevel(ShadowQuality q)
        {
            if (shadowLevels != null)
            {
                for (int i = 0; i < shadowLevels.Length; i++)
                    if (shadowLevels[i].quality == q) return shadowLevels[i];
            }
            Debug.LogWarning($"[SettingsManager] Baris shadowLevels untuk {q} tidak ada di prefab.");
            return new ShadowQualityLevel(q, QualitySettings.shadows, QualitySettings.shadowResolution, true,
                QualitySettings.shadowDistance, 0, 0);
        }

        // ---------------------------------------------------------------- Simpan dan batal

        /// <summary>Simpan ke PlayerPrefs. Penyimpanan tidak terjadi saat slider digeser.</summary>
        public void Commit()
        {
            if (HasPendingGraphics) ApplyGraphics();
            PlayerPrefs.SetString(PrefsKey, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
            applied = data.Clone();
            isDirty = false;
            EventChannel<SettingsChangedEvent>.Publish(new SettingsChangedEvent(SettingsCategory.All));
        }

        /// <summary>Kembali ke nilai terakhir yang tersimpan.</summary>
        public void Revert()
        {
            data = applied.Clone();
            ApplyAudio();
            PushViewSettings();
            // Kalau Terapkan sudah ditekan tanpa disimpan, grafis ikut dikembalikan ke nilai tersimpan.
            if (HasPendingGraphics) ApplyGraphics();
            isDirty = false;
        }

        /// <summary>Nilai bawaan. Grafis menunggu Terapkan. Tombol input tidak ikut (InputManager.ResetBindings terpisah).</summary>
        public void ResetToDefaults()
        {
            data = new SettingsData();
            ApplyAudio();
            PushViewSettings();
            isDirty = true;
        }

        // ---------------------------------------------------------------- Bantuan

        private void MarkViewChanged()
        {
            isDirty = true;
            PushViewSettings();
        }

        private void PushViewSettings()
        {
            var cam = FirstPersonCamera.Instance;
            if (cam == null) return;
            lastCamera = cam;
            cam.SetViewSettings(ViewSettings);
        }

        private void ApplyAudio()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.UpdateVolume(data.masterVolume, data.bgmVolume, data.sfxVolume);
        }
    }
}
