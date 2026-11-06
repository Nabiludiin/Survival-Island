using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Senter di tangan kiri (HandUse.OffHand) pada viewmodel Tool_Flashlight: lampu, kedip, kuras baterai, dan sorotan.
    /// Blueprint: Island_Logic/FlashlightController.md (#98, melengkapi #18 dan #60). Usulan, belum ada di GDD; mendukung
    /// Region 2 yang gelap (GDD bagian 5). Senter tidak memberi panas (GDD bagian 5 hanya menyebut obor untuk Region 3).
    ///
    /// Isi baterai ada di ItemInstance.durability, jadi tersimpan bersama inventori. Tombol F dibaca EquipmentManager
    /// (ToggleOffHand), bukan komponen ini. Kedip tanpa angka acak, jadi sama di tiap uji.
    /// </summary>
    public class FlashlightController : EquippableBehaviour
    {
        [SerializeField] private Light flashlight;
        [SerializeField] private LightSource lightSource;
        [SerializeField] private Transform beamOrigin;

        private float baseIntensity = 1f;
        private ItemInstance instance;
        private bool isOn;

        /// <summary>Senter yang sedang dipegang. Dibaca Recharge dan IsPointInBeam.</summary>
        public static FlashlightController Active { get; private set; }

        public static bool IsActiveLit => Active != null && Active.isOn;
        public static ItemInstance ActiveInstance => Active != null ? Active.instance : null;
        public bool IsOn => isOn;

        private static FlashlightBalance FB => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.flashlight : null;

        private void Awake()
        {
            if (flashlight == null) flashlight = GetComponentInChildren<Light>(true);
            if (lightSource == null) lightSource = GetComponentInChildren<LightSource>(true);
            if (beamOrigin == null && flashlight != null) beamOrigin = flashlight.transform;
            if (flashlight != null) baseIntensity = flashlight.intensity;
            if (lightSource != null)
            {
                lightSource.isHeld = true;
                lightSource.isFlashlight = true;
            }
        }

        // ---------------------------------------------------------------- Dipegang dan disimpan

        public override void OnEquip(ItemInstance inst)
        {
            base.OnEquip(inst);
            // 1.
            instance = inst;
            Active = this;
            // 2. range dan spotAngle dari config, supaya lampu yang terlihat dan IsPointInBeam memakai angka yang sama.
            var fb = FB;
            if (flashlight != null && fb != null)
            {
                flashlight.type = LightType.Spot;
                flashlight.range = fb.range;
                flashlight.spotAngle = fb.spotAngle;
            }
            if (lightSource != null && fb != null) lightSource.range = fb.range;
            // 3.
            if (inst != null && inst.durability > 0f) SetLit(true);
            else
            {
                SetLit(false);
                Notify();
            }
        }

        public override void OnUnequip()
        {
            SetLit(false);
            instance = null;
            if (Active == this) Active = null;
            base.OnUnequip();
        }

        private void OnDisable()
        {
            if (Active == this) Active = null;
        }

        private void SetLit(bool on)
        {
            isOn = on;
            if (flashlight != null)
            {
                flashlight.enabled = on;
                flashlight.intensity = baseIntensity;
            }
            if (lightSource != null)
            {
                lightSource.IsLit = on;
                lightSource.intensity01 = on ? 1f : 0f;
            }
        }

        private static void Notify()
        {
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show("notif.battery_empty", NotificationKind.Warning);   // "Baterai habis"
        }

        // ---------------------------------------------------------------- Kuras baterai

        private void Update()
        {
            if (!isOn || instance == null) return;
            var fb = FB;
            if (fb == null) return;
            var lb = GameManager.CurrentBalance.light;
            // 1. Time.deltaTime: jeda menghentikan kuras, tidur tidak mempercepatnya.
            instance.durability -= fb.drainPerSec * Time.deltaTime;
            // 2-3.
            float remainingSeconds = fb.drainPerSec > 0f ? instance.durability / fb.drainPerSec : float.MaxValue;
            if (flashlight != null)
            {
                flashlight.intensity = remainingSeconds < lb.flickerSeconds
                    ? baseIntensity * (Mathf.PingPong(Time.time * fb.flickerHz, 1f) > 0.5f ? 1f : 0.2f)
                    : baseIntensity;
            }
            // 4. Senter tetap di tangan.
            if (instance.durability <= 0f)
            {
                instance.durability = 0f;
                SetLit(false);
                Notify();
            }
        }

        // ---------------------------------------------------------------- Isi ulang

        /// <summary>
        /// Isi baterai senter target (boleh senter di inventori). false kalau sudah penuh, supaya baterai tidak terbuang.
        /// Pemanggil (ItemUseSystem lewat LightSourceSystem.TryUseBattery) mengurangi Item_Battery hanya kalau true.
        /// </summary>
        public static bool Recharge(ItemInstance target, float amount)
        {
            // 1.
            if (target == null) return false;
            var fb = FB;
            float max = fb != null ? fb.maxBattery : 100f;
            // 2.
            if (target.durability >= max) return false;
            // 3.
            target.durability = Mathf.Min(Mathf.Max(0f, target.durability) + amount, max);
            // 4. Usulan 3: senter yang dipegang langsung menyala lagi.
            if (Active != null && Active.instance == target && !Active.isOn) Active.SetLit(true);
            // 5.
            return true;
        }

        // ---------------------------------------------------------------- Sorotan

        /// <summary>Titik di dalam kerucut sorot. Tanpa query fisika; garis pandang diperiksa pemanggil (Zombie Lumut #25).</summary>
        public static bool IsPointInBeam(Vector3 pos)
        {
            // 1.
            if (Active == null || !Active.isOn) return false;
            var origin = Active.beamOrigin != null ? Active.beamOrigin : Active.transform;
            var fb = FB;
            float range = fb != null ? fb.range : 25f;
            float angle = fb != null ? fb.spotAngle : 50f;
            // 2.
            Vector3 d = pos - origin.position;
            if (d.sqrMagnitude > range * range) return false;
            // 3.
            return Vector3.Angle(origin.forward, d) <= angle * 0.5f;
        }
    }
}
