using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Obor di tangan kiri (HandUse.OffHand, prefab Tool_Torch). Durasi bakar, cahaya, dan padam.
    /// Blueprint: Island_Logic/WarmthSystem.md bagian "Obor" (#65). GDD bagian 5, Region 3: membawa obor mencegah frostbite.
    /// Keputusan developer (USULAN_TERBUKA, #85 poin 3): Storm dan Blizzard tidak memadamkan obor; Rain memadamkan setelah 20 detik.
    ///
    /// Sisa bakar disimpan sebagai rasio di ItemInstance.durability. EquipmentManager mematikan objek ini saat obor disimpan,
    /// jadi durasi yang berjalan saat obor disimpan otomatis (kapak, Heavy Item) dihitung dari Time.time saat dipegang lagi.
    ///
    /// Belum ada (TODO): LightSource isHeld (#98), partikel api lewat VFXManager (#15), notifikasi #22.
    /// </summary>
    public class TorchController : EquippableBehaviour
    {
        [SerializeField] private Light torchLight;
        [SerializeField] private ParticleSystem flame;

        private float burnRemaining;
        private float rainTimer;
        private int stashedInstanceId = -1;
        private float stashedAt;

        public bool IsLit { get; private set; }

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        private void Awake()
        {
            SetVisual(false);
        }

        public override void OnEquip(ItemInstance instance)
        {
            base.OnEquip(instance);
            var bal = Balance;
            float total = bal != null ? Mathf.Max(1f, bal.warmth.torchBurnSeconds) : 300f;
            if (instance.durability < 0f) instance.durability = 1f;
            burnRemaining = instance.durability * total;

            // Durasi tetap berjalan selama disimpan otomatis.
            if (stashedInstanceId == instance.instanceId)
            {
                burnRemaining -= Mathf.Max(0f, Time.time - stashedAt);
                stashedInstanceId = -1;
                if (burnRemaining <= 0f)
                {
                    BurnOut();
                    return;
                }
            }

            // 1. Terlalu basah. TODO #22: notifikasi "Terlalu basah".
            if (!CanLight())
            {
                Extinguish();
                return;
            }
            // 2-3. Menyalakan ulang gratis selama durasi masih ada. TODO #98: LightSource isHeld = true.
            IsLit = true;
            rainTimer = 0f;
            SetVisual(true);
            if (WarmthSystem.Instance != null) WarmthSystem.Instance.SetTorchHeld(true);
        }

        public override void OnUnequip()
        {
            var inst = CurrentInstance;
            SaveBurn();
            if (WarmthSystem.Instance != null) WarmthSystem.Instance.SetTorchHeld(false);

            // Disimpan otomatis (kapak, Heavy Item): tangan kiri masih aktif, jadi IsLit tetap dan durasi terus berjalan.
            var eq = EquipmentManager.Instance;
            bool autoStash = inst != null && IsLit && eq != null && eq.OffHandActive && eq.OffHandInstanceId == inst.instanceId;
            if (autoStash)
            {
                stashedInstanceId = inst.instanceId;
                stashedAt = Time.time;
            }
            else
            {
                // Dimatikan lewat F: durasi berhenti.
                IsLit = false;
            }
            SetVisual(false);
            base.OnUnequip();
        }

        private void Update()
        {
            if (!IsLit || CurrentInstance == null) return;
            var bal = Balance;
            if (bal == null) return;

            // 1.
            burnRemaining -= Time.deltaTime;
            if (burnRemaining <= 0f)
            {
                BurnOut();
                return;
            }

            // 2. Storm dan Blizzard tidak memadamkan obor (keputusan developer).
            // 3. Rain di bawah langit terbuka memadamkan setelah torchRainExtinguishSeconds. Di bawah atap timer kembali 0.
            var warmth = WarmthSystem.Instance;
            if (warmth != null && warmth.IsInWater)
            {
                Extinguish();
                return;
            }
            if (warmth != null && warmth.IsWeather(WeatherType.Rain) && warmth.IsExposedToSky())
            {
                rainTimer += Time.deltaTime;
                if (rainTimer >= bal.warmth.torchRainExtinguishSeconds) Extinguish();
            }
            else rainTimer = 0f;
        }

        /// <summary>Ditolak di air, atau saat Rain di bawah langit terbuka. Storm dan Blizzard tidak menghalangi.</summary>
        public bool CanLight()
        {
            var warmth = WarmthSystem.Instance;
            if (warmth == null) return true;
            if (warmth.IsInWater) return false;
            return !(warmth.IsWeather(WeatherType.Rain) && warmth.IsExposedToSky());
        }

        public void Extinguish()
        {
            SaveBurn();
            IsLit = false;
            SetVisual(false);
            if (WarmthSystem.Instance != null) WarmthSystem.Instance.SetTorchHeld(false);
        }

        private void BurnOut()
        {
            // Obor habis: item hilang. TODO #22: notifikasi.
            var inst = CurrentInstance;
            IsLit = false;
            SetVisual(false);
            if (WarmthSystem.Instance != null) WarmthSystem.Instance.SetTorchHeld(false);
            if (inst != null && InventoryManager.Instance != null) InventoryManager.Instance.RemoveInstance(inst.instanceId);
        }

        private void SaveBurn()
        {
            var inst = CurrentInstance;
            var bal = Balance;
            if (inst == null || bal == null) return;
            inst.durability = Mathf.Clamp01(burnRemaining / Mathf.Max(1f, bal.warmth.torchBurnSeconds));
        }

        private void SetVisual(bool on)
        {
            if (torchLight != null) torchLight.enabled = on;
            if (flame != null)
            {
                if (on) flame.Play(true);
                else flame.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }
}
