using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Asal air: daftar penampung hujan, tick per jam game, dan peringatan haus. Komponen di pemain Fase 2 (IslandPhase).
    /// Blueprint: raft/HydrationSystem.md (#43).
    ///
    /// GDD bagian 3: haus diisi Air Minum Bersih, kelapa, atau air hujan yang ditampung; air kotor berisiko sakit.
    /// SurvivalStats tetap pemilik Thirst, pengurangan berdasar aktivitas, dan penalti dehidrasi (VitalsRules.md).
    /// Kelas ini TIDAK menghitung ulang aturan vital.
    /// </summary>
    [DisallowMultipleComponent]
    public class HydrationSystem : MonoBehaviour
    {
        public static HydrationSystem Instance { get; private set; }

        // Termasuk penampung yang region-nya sedang tidak dimuat (objeknya nonaktif).
        private readonly List<RainwaterCollector> collectors = new List<RainwaterCollector>(16);
        private bool lastThirstLowNotified;
        private VitalWarningFlags lastFlags;
        private bool hasLastFlags;

        public IReadOnlyList<RainwaterCollector> Collectors => collectors;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<HourChangedEvent>.Raised += HandleHour;
            EventChannel<VitalWarningChangedEvent>.Raised += HandleWarnings;
            EventChannel<StructureBuiltEvent>.Raised += HandleStructureBuilt;
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
        }

        private void OnDisable()
        {
            EventChannel<HourChangedEvent>.Raised -= HandleHour;
            EventChannel<VitalWarningChangedEvent>.Raised -= HandleWarnings;
            EventChannel<StructureBuiltEvent>.Raised -= HandleStructureBuilt;
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Pendaftaran penampung

        /// <summary>Dipanggil saat penampung dibuat atau dipulihkan. Tidak memakai OnEnable (region bisa dilepas, #88).</summary>
        public void Register(RainwaterCollector c)
        {
            if (c != null && !collectors.Contains(c)) collectors.Add(c);
        }

        public void Unregister(RainwaterCollector c)
        {
            collectors.Remove(c);
        }

        // ---------------------------------------------------------------- Tick per jam game

        private void HandleHour(HourChangedEvent e)
        {
            // Game dijeda: jam game berhenti, event ini tidak terkirim, jadi penampung tidak bertambah.
            for (int i = collectors.Count - 1; i >= 0; i--)
            {
                var c = collectors[i];
                if (c == null) { collectors.RemoveAt(i); continue; }
                c.TickHour();
            }
        }

        // ---------------------------------------------------------------- Atap berubah

        private void HandleStructureBuilt(StructureBuiltEvent e) => RecheckNear(e.position);
        private void HandleStructureDestroyed(StructureDestroyedEvent e) => RecheckNear(e.position);

        private void RecheckNear(Vector3 pos)
        {
            var b = GameManager.CurrentBalance;
            float r = b != null ? b.hydration.structureRecheckRadius : 6f;
            float r2 = r * r;
            for (int i = collectors.Count - 1; i >= 0; i--)
            {
                var c = collectors[i];
                if (c == null) { collectors.RemoveAt(i); continue; }
                if ((c.transform.position - pos).sqrMagnitude <= r2) c.RefreshSkyExposure();
            }
        }

        // ---------------------------------------------------------------- Peringatan

        private void HandleWarnings(VitalWarningChangedEvent e)
        {
            var prev = hasLastFlags ? lastFlags : VitalWarningFlags.None;
            bool thirstLowNow = (e.flags & VitalWarningFlags.ThirstLow) != 0;
            bool thirstLowBefore = (prev & VitalWarningFlags.ThirstLow) != 0;
            bool dehydratedNow = (e.flags & VitalWarningFlags.Dehydrated) != 0;
            bool dehydratedBefore = (prev & VitalWarningFlags.Dehydrated) != 0;
            lastFlags = e.flags;
            hasLastFlags = true;

            if (thirstLowNow && !thirstLowBefore && !lastThirstLowNotified)
            {
                lastThirstLowNotified = true;
                Notify("notif.thirst_low", NotificationKind.Warning);   // "Kamu haus"
            }
            if (dehydratedNow && !dehydratedBefore)
                Notify("notif.dehydrated", NotificationKind.Danger);   // "Dehidrasi: stamina tidak pulih"
            if (!thirstLowNow) lastThirstLowNotified = false;
        }

        private static void Notify(string key, NotificationKind kind)
        {
            if (NotificationManager.Instance != null) NotificationManager.Instance.Show(key, kind);
        }
    }
}
