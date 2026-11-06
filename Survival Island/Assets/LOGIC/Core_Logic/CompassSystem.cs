using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalIsland
{
    /// <summary>Jenis penanda. Urutan juga dipakai untuk indeks iconSprites. Blueprint: Core_Logic/CompassSystem.md (#30).</summary>
    public enum MarkerIcon { Supply, Part, Helipad, Objective }

    /// <summary>Satu penanda lokasi. Data posisi saja, jadi tetap berlaku walau regionnya dilepas (#88).</summary>
    [Serializable]
    public struct MapMarker
    {
        public string id;           // part_2, supply, objective, helipad
        public Vector3 position;
        public MarkerIcon icon;
        public string labelKey;     // Kunci #104, boleh kosong. Belum ditampilkan di bilah
    }

    /// <summary>
    /// Data penanda milik CompassSystem. Kelas biasa, hidup bersama scene IslandPhase.
    /// Setiap perubahan menerbitkan MapMarkerChangedEvent (tidak sticky). Keputusan developer: #105 tidak dipakai,
    /// jadi registry ini berpindah ke CompassSystem dan penanda buatan pemain dihapus.
    /// </summary>
    public class MapMarkerRegistry
    {
        private readonly List<MapMarker> markers = new List<MapMarker>(16);
        private int nextAutoId = 1;

        public IReadOnlyList<MapMarker> All => markers;

        /// <summary>Buat penanda dengan ID otomatis marker_&lt;n&gt;, lalu kembalikan ID-nya.</summary>
        public string Add(Vector3 position, MarkerIcon icon)
        {
            string id = "marker_" + nextAutoId;
            nextAutoId++;
            markers.Add(new MapMarker { id = id, position = position, icon = icon, labelKey = string.Empty });
            EventChannel<MapMarkerChangedEvent>.Publish(new MapMarkerChangedEvent(id, false));
            return id;
        }

        /// <summary>Buat atau perbarui. ID kosong ditolak, ID ganda memperbarui penanda lama.</summary>
        public void Set(in MapMarker marker)
        {
            if (string.IsNullOrEmpty(marker.id))
            {
                Debug.LogWarning("[MapMarkerRegistry] Set dengan ID kosong ditolak.");
                return;
            }

            int index = IndexOf(marker.id);
            if (index >= 0) markers[index] = marker;
            else markers.Add(marker);
            EventChannel<MapMarkerChangedEvent>.Publish(new MapMarkerChangedEvent(marker.id, false));
        }

        /// <summary>Hapus penanda. ID yang tidak ada mengembalikan false tanpa event.</summary>
        public bool Remove(string id)
        {
            int index = IndexOf(id);
            if (index < 0) return false;
            markers.RemoveAt(index);
            EventChannel<MapMarkerChangedEvent>.Publish(new MapMarkerChangedEvent(id, true));
            return true;
        }

        public bool TryGet(string id, out MapMarker marker)
        {
            int index = IndexOf(id);
            if (index < 0) { marker = default; return false; }
            marker = markers[index];
            return true;
        }

        // Loop biasa: isinya paling banyak belasan, tanpa alokasi.
        private int IndexOf(string id)
        {
            if (string.IsNullOrEmpty(id)) return -1;
            for (int i = 0; i < markers.Count; i++)
                if (string.Equals(markers[i].id, id, StringComparison.Ordinal)) return i;
            return -1;
        }
    }

    /// <summary>
    /// Bilah kompas di atas layar: arah mata angin dan penanda lokasi.
    /// Blueprint: Core_Logic/CompassSystem.md (#30, #69, #71, #81). Usulan, belum ada di GDD; membantu tujuan GDD bagian 1
    /// dan 5 (memindahkan suku cadang dari Region 4 ke Region 3). Usulan disetujui developer sebagai nilai awal:
    /// hanya di IslandPhase, sumbu +Z dunia = utara, +X = timur.
    ///
    /// Singleton per scene, komponen di canvas HUD (anak HUDManager). Awake mengisi Instance dan Markers, supaya pemasang
    /// penanda bisa memanggil sejak OnEnable atau OnRestoreFinished. Tidak menyimpan data: penanda dipasang ulang pemiliknya.
    /// </summary>
    public class CompassSystem : MonoBehaviour
    {
        public static CompassSystem Instance { get; private set; }

        /// <summary>Data penanda. Dibuat di Awake.</summary>
        public MapMarkerRegistry Markers { get; private set; }

        [Tooltip("Objek Player di IslandPhase. Yaw pemain diputar pada objek ini (FirstPersonCamera.md).")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private RectTransform compassBarUI;
        [Tooltip("Tekstur garis derajat, Wrap Mode = Repeat, satu putaran 360 derajat.")]
        [SerializeField] private RawImage barTicks;
        [Tooltip("Huruf arah pada 0, 90, 180, 270 derajat (N, E, S, W).")]
        [SerializeField] private TextMeshProUGUI[] cardinalLabels = new TextMeshProUGUI[4];
        [Tooltip("12 slot di prefab. Ukuran prefab, bukan angka permainan.")]
        [SerializeField] private CompassIcon[] iconSlots = new CompassIcon[0];
        [Tooltip("Urut menurut MarkerIcon: Supply, Part, Helipad, Objective.")]
        [SerializeField] private Sprite[] iconSprites = new Sprite[4];

        // Kunci #104 huruf arah. Bahasa Indonesia: U, T, S, B.
        private static readonly string[] CardinalKeys = { "ui.compass.n", "ui.compass.e", "ui.compass.s", "ui.compass.w" };

        private const string ObjectiveId = "objective";
        private const string HelipadId = "helipad";
        private const string PartPrefix = "part_";

        private readonly List<int> visible = new List<int>(12);
        private bool layoutDirty = true;
        private bool isHidden;
        private bool playerDead;
        private bool warnedNoPlayer;
        private bool warnedUnknownId;
        private float yaw;
        private RectTransform[] cardinalRects = new RectTransform[0];
        private CanvasGroup[] cardinalGroups = new CanvasGroup[0];

        private static CompassBalance B
        {
            get
            {
                var cfg = GameManager.CurrentBalance;
                return cfg != null ? cfg.compass : null;
            }
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[CompassSystem] Instance ganda di scene. Yang baru dimatikan.");
                enabled = false;
                return;
            }
            Instance = this;
            Markers = new MapMarkerRegistry();

            cardinalRects = new RectTransform[cardinalLabels.Length];
            cardinalGroups = new CanvasGroup[cardinalLabels.Length];
            for (int i = 0; i < cardinalLabels.Length; i++)
            {
                if (cardinalLabels[i] == null) continue;
                cardinalRects[i] = cardinalLabels[i].rectTransform;
                cardinalGroups[i] = cardinalLabels[i].GetComponent<CanvasGroup>();
            }
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            if (Instance != this) return;
            EventChannel<MapMarkerChangedEvent>.Raised += HandleMarkerChanged;
            EventChannel<ObjectiveMarkerChangedEvent>.Raised += HandleObjectiveMarker;
            EventChannel<CinematicStateChangedEvent>.Raised += HandleCinematic;
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
            EventChannel<LocaleChangedEvent>.Raised += HandleLocaleChanged;

            // Event sticky dibaca di OnEnable.
            if (EventChannel<ObjectiveMarkerChangedEvent>.HasLast) HandleObjectiveMarker(EventChannel<ObjectiveMarkerChangedEvent>.Last);
            if (EventChannel<CinematicStateChangedEvent>.HasLast) HandleCinematic(EventChannel<CinematicStateChangedEvent>.Last);
            RefreshCardinalText();
            layoutDirty = true;
        }

        private void OnDisable()
        {
            if (Instance != this) return;
            EventChannel<MapMarkerChangedEvent>.Raised -= HandleMarkerChanged;
            EventChannel<ObjectiveMarkerChangedEvent>.Raised -= HandleObjectiveMarker;
            EventChannel<CinematicStateChangedEvent>.Raised -= HandleCinematic;
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
            EventChannel<LocaleChangedEvent>.Raised -= HandleLocaleChanged;
        }

        // ---------------------------------------------------------------- Fungsi untuk sistem lain

        /// <summary>BeachSupplyManager (#71): penanda kargo dengan ID otomatis.</summary>
        public string AddMarker(Vector3 position, MarkerIcon icon) => Markers.Add(position, icon);

        /// <summary>HeavyItemSystem (#69): ID part_&lt;partId&gt; dan helipad. Ikon diambil dari ID.</summary>
        public void SetMarker(string markerId, Vector3 position)
        {
            MarkerIcon icon;
            string label = string.Empty;
            if (!string.IsNullOrEmpty(markerId) && markerId.StartsWith(PartPrefix, StringComparison.Ordinal)) icon = MarkerIcon.Part;
            else if (markerId == HelipadId) icon = MarkerIcon.Helipad;
            else if (markerId == ObjectiveId) icon = MarkerIcon.Objective;
            else if (Markers.TryGet(markerId, out var old)) { icon = old.icon; label = old.labelKey; }
            else
            {
                icon = MarkerIcon.Objective;
                if (!warnedUnknownId)
                {
                    warnedUnknownId = true;
                    Debug.LogWarning($"[CompassSystem] SetMarker('{markerId}') tanpa ikon. Memakai Objective.");
                }
            }
            Markers.Set(new MapMarker { id = markerId, position = position, icon = icon, labelKey = label });
        }

        /// <summary>Pemanggil baru dengan ikon yang jelas.</summary>
        public void SetMarker(string markerId, Vector3 position, MarkerIcon icon)
        {
            Markers.Set(new MapMarker { id = markerId, position = position, icon = icon, labelKey = string.Empty });
        }

        /// <summary>ID yang tidak ada diabaikan.</summary>
        public void RemoveMarker(string markerId) => Markers.Remove(markerId);

        // ---------------------------------------------------------------- Event

        private void HandleMarkerChanged(MapMarkerChangedEvent e) => layoutDirty = true;

        private void HandleObjectiveMarker(ObjectiveMarkerChangedEvent e)
        {
            // #81: satu penanda tujuan dengan ID tetap.
            if (e.hasMarker)
                Markers.Set(new MapMarker { id = ObjectiveId, position = e.position, icon = MarkerIcon.Objective, labelKey = e.label });
            else
                Markers.Remove(ObjectiveId);
        }

        private void HandleCinematic(CinematicStateChangedEvent e)
        {
            // Usulan 5: kompas disembunyikan saat adegan (#53, #99) dan setelah mati.
            isHidden = e.isPlaying || playerDead;
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            playerDead = true;
            isHidden = true;
        }

        private void HandleLocaleChanged(LocaleChangedEvent e) => RefreshCardinalText();

        private void RefreshCardinalText()
        {
            var loc = LocalizationManager.Instance;
            for (int i = 0; i < cardinalLabels.Length && i < CardinalKeys.Length; i++)
            {
                if (cardinalLabels[i] == null) continue;
                cardinalLabels[i].text = loc != null ? loc.Get(CardinalKeys[i]) : CardinalKeys[i];
            }
        }

        // ---------------------------------------------------------------- Pembagian slot

        private static int Priority(MarkerIcon icon)
        {
            switch (icon)
            {
                case MarkerIcon.Objective: return 0;
                case MarkerIcon.Helipad: return 1;
                case MarkerIcon.Part: return 2;
                default: return 3;
            }
        }

        /// <returns>true kalau penanda a harus lebih dulu dari b.</returns>
        private static bool Before(in MapMarker a, in MapMarker b)
        {
            int pa = Priority(a.icon), pb = Priority(b.icon);
            if (pa != pb) return pa < pb;
            return string.CompareOrdinal(a.id, b.id) < 0;
        }

        private void RebuildVisible()
        {
            // 1.
            visible.Clear();
            var all = Markers.All;
            int cap = iconSlots.Length;

            // 2-3. Sisip berurutan (prioritas, lalu ID), ambil paling banyak iconSlots.Length. Tanpa alokasi.
            for (int i = 0; i < all.Count; i++)
            {
                var m = all[i];
                int pos = visible.Count;
                while (pos > 0 && Before(m, all[visible[pos - 1]])) pos--;
                if (pos >= cap) continue;
                if (visible.Count == cap) visible.RemoveAt(cap - 1);   // Buang yang terendah dulu, kapasitas tidak tumbuh
                visible.Insert(pos, i);
            }

            for (int s = 0; s < iconSlots.Length; s++)
            {
                var slot = iconSlots[s];
                if (slot == null) continue;
                bool used = s < visible.Count;
                slot.gameObject.SetActive(used);
                if (!used) continue;
                int iconIndex = (int)all[visible[s]].icon;
                if (slot.icon != null && iconIndex < iconSprites.Length) slot.icon.sprite = iconSprites[iconIndex];
                slot.lastMeters = -1;
            }

            // 4.
            layoutDirty = false;
        }

        // ---------------------------------------------------------------- Tampilan per frame

        private void LateUpdate()
        {
            // 1.
            if (isHidden || playerTransform == null)
            {
                if (playerTransform == null && !warnedNoPlayer)
                {
                    warnedNoPlayer = true;
                    Debug.LogWarning("[CompassSystem] playerTransform kosong. Bilah kompas disembunyikan.", this);
                }
                if (compassBarUI != null && compassBarUI.gameObject.activeSelf) compassBarUI.gameObject.SetActive(false);
                return;
            }
            if (compassBarUI == null) return;
            if (!compassBarUI.gameObject.activeSelf) compassBarUI.gameObject.SetActive(true);

            var b = B;
            if (b == null) return;
            if (layoutDirty) RebuildVisible();

            // 2.
            yaw = playerTransform.eulerAngles.y;
            float arc = Mathf.Max(1f, b.visibleArcDegrees);

            // 3. Bilah bergeser lewat uvRect, jadi tidak ada sambungan di 0/360 derajat.
            if (barTicks != null) barTicks.uvRect = new Rect(yaw / 360f - arc / 720f, 0f, arc / 360f, 1f);

            // 4. Huruf arah: 0 = utara (+Z), 90 = timur (+X).
            for (int i = 0; i < cardinalRects.Length; i++)
            {
                if (cardinalRects[i] == null) continue;
                PlaceOnBar(cardinalRects[i], cardinalGroups[i], i * 90f, false, b, out _, out _);
            }

            // 5-6. Penanda.
            var all = Markers.All;
            Vector3 p = playerTransform.position;
            for (int s = 0; s < visible.Count && s < iconSlots.Length; s++)
            {
                var slot = iconSlots[s];
                if (slot == null) continue;
                var m = all[visible[s]];
                float dx = m.position.x - p.x;
                float dz = m.position.z - p.z;   // Tinggi diabaikan
                float bearing = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg;
                bool clamp = m.icon == MarkerIcon.Objective && b.clampObjectiveToEdge;
                PlaceOnBar(slot.Rect, slot.group, bearing, clamp, b, out bool outside, out float delta);
                slot.SetArrows(outside && clamp, delta);

                if (slot.distanceText != null)
                {
                    if (b.showDistance)
                    {
                        if (!slot.distanceText.enabled) slot.distanceText.enabled = true;
                        int meters = Mathf.RoundToInt(Mathf.Sqrt(dx * dx + dz * dz));
                        if (meters != slot.lastMeters)
                        {
                            slot.lastMeters = meters;
                            slot.distanceText.SetText("{0} m", meters);   // Tanpa string baru
                        }
                    }
                    else if (slot.distanceText.enabled) slot.distanceText.enabled = false;
                }
            }
        }

        private void PlaceOnBar(RectTransform item, CanvasGroup group, float bearing, bool clampToEdge, CompassBalance b,
            out bool outside, out float delta)
        {
            float half = Mathf.Max(0.5f, b.visibleArcDegrees * 0.5f);
            delta = Mathf.DeltaAngle(yaw, bearing);   // -180 sampai 180
            outside = Mathf.Abs(delta) > half;
            if (outside && !clampToEdge)
            {
                if (group != null) group.alpha = 0f;
                return;
            }
            float clamped = Mathf.Clamp(delta, -half, half);
            item.anchoredPosition = new Vector2(clamped / half * compassBarUI.rect.width * 0.5f, item.anchoredPosition.y);
            float fade = Mathf.InverseLerp(half, half - b.edgeFadeDegrees, Mathf.Abs(delta));
            if (group != null) group.alpha = outside ? 1f : fade;   // Penanda yang dijepit tetap jelas
        }
    }
}
