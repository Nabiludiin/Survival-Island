using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu busur di pinggir layar yang menunjuk sumber damage atau serangan yang akan datang.
    /// Objek pool dengan tag UI_DamageIndicator. Blueprint: Core_Logic/CombatFeedbackUI.md (#51), bagian "Directional Damage".
    /// Hanya menyimpan salinan posisi sumber, tidak pernah rujukan ke musuh.
    /// </summary>
    public class DamageIndicator : MonoBehaviour, IPoolable
    {
        public const string PoolTag = "UI_DamageIndicator";

        [Tooltip("Gambar busur. Prefab berputar di tengah layar, busur berada di radius 45 persen sisi pendek.")]
        [SerializeField] private Image image;
        [Tooltip("Warna busur damage (merah darah).")]
        [SerializeField] private Color damageColor = new Color(0.6f, 0f, 0f, 1f);
        [Tooltip("Warna penanda serangan yang akan datang (kuning pucat).")]
        [SerializeField] private Color telegraphColor = new Color(1f, 0.95f, 0.6f, 1f);

        public Vector3 sourcePosition;
        public float timer;
        public float duration;
        public float strength;
        public float lastAngle;
        public bool isTelegraph;

        /// <summary>Slot di array active milik CombatFeedbackUI. -1 kalau tidak terpasang.</summary>
        public int slotIndex = -1;

        /// <summary>Pemilik saat ini. OnReturned mengosongkan slotnya, supaya ReturnAllActive tidak meninggalkan rujukan.</summary>
        public CombatFeedbackUI owner;

        public RectTransform Rect => (RectTransform)transform;

        /// <summary>Atur data setelah keluar dari pool.</summary>
        public void Setup(Vector3 source, float seconds, float strength01, bool telegraph)
        {
            sourcePosition = source;
            duration = Mathf.Max(0.01f, seconds);
            timer = duration;
            strength = strength01;
            isTelegraph = telegraph;
            if (image != null) image.color = WithAlpha(telegraph ? telegraphColor : damageColor, strength01);
        }

        public void SetAlpha(float alpha)
        {
            if (image == null) return;
            var c = image.color;
            c.a = alpha;
            image.color = c;
        }

        public void OnSpawned()
        {
            // CombatFeedbackUI.md bagian 4: timer 0, alpha 0, warna merah, rotasi nol.
            timer = 0f;
            duration = 0f;
            strength = 0f;
            lastAngle = 0f;
            isTelegraph = false;
            slotIndex = -1;
            owner = null;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            if (image != null)
            {
                image.raycastTarget = false;
                image.color = WithAlpha(damageColor, 0f);
            }
        }

        public void OnReturned()
        {
            if (owner != null) owner.ReleaseSlot(this);
            owner = null;
            slotIndex = -1;
            timer = 0f;
        }

        private static Color WithAlpha(Color c, float a) { c.a = a; return c; }
    }
}
