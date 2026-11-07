using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Tension Bar di HUD. Tampil saat FishingStateChangedEvent.state == Reeling, lalu membaca Tension dan CatchProgress dari
    /// Kail yang sedang dipegang. Hanya membaca (aturan UI di GameEventBus.md). Blueprint: raft/FishingSystem.md "Tension Bar" (#55).
    /// Zona warna: hijau di bawah 0,7, kuning sampai 0,9, merah di atasnya.
    /// </summary>
    public class FishingTensionHUD : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Image tensionFill;
        [SerializeField] private Image progressFill;
        [SerializeField] private float yellowFrom = 0.7f;
        [SerializeField] private float redFrom = 0.9f;
        [SerializeField] private Color green = new Color(0.3f, 0.85f, 0.35f);
        [SerializeField] private Color yellow = new Color(0.95f, 0.8f, 0.2f);
        [SerializeField] private Color red = new Color(0.9f, 0.25f, 0.2f);

        private FishingSystem source;
        private bool visible;

        private void Awake()
        {
            SetVisible(false);
        }

        private void OnEnable()
        {
            EventChannel<FishingStateChangedEvent>.Raised += HandleState;
        }

        private void OnDisable()
        {
            EventChannel<FishingStateChangedEvent>.Raised -= HandleState;
            SetVisible(false);
        }

        private void HandleState(FishingStateChangedEvent e)
        {
            bool reeling = e.state == FishingState.Reeling;
            if (reeling)
            {
                // Kail yang aktif diambil dari EquipmentManager.
                var em = EquipmentManager.Instance;
                source = em != null ? em.Active as FishingSystem : null;
            }
            SetVisible(reeling && source != null);
        }

        private void Update()
        {
            if (!visible || source == null) return;
            float t = source.Tension;
            if (tensionFill != null)
            {
                tensionFill.fillAmount = t;
                tensionFill.color = t >= redFrom ? red : t >= yellowFrom ? yellow : green;
            }
            if (progressFill != null) progressFill.fillAmount = source.CatchProgress;
        }

        private void SetVisible(bool on)
        {
            visible = on;
            if (root != null) root.SetActive(on);
            if (!on) source = null;
        }
    }
}
