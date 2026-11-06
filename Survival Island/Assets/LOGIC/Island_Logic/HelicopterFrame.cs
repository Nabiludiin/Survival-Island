using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Model kerangka helikopter di Region3_Snow. Menampilkan bagian sesuai progres dari RescueEventManager.
    /// Blueprint: Island_Logic/RescueEventManager.md bagian 1-2 (#14). Tidak memakai IDamageable, jadi zombie,
    /// hewan, dan bencana tidak bisa merusaknya.
    /// </summary>
    public class HelicopterFrame : MonoBehaviour
    {
        [Serializable]
        public class PartVisual
        {
            public string partId;
            public GameObject visual;
        }

        [SerializeField] private PartVisual[] partVisuals = Array.Empty<PartVisual>();
        [SerializeField] private PartInstallZone installZone;
        [SerializeField] private Transform cockpit;

        private readonly List<string> scratch = new List<string>(8);

        public Vector3 CockpitPosition => cockpit != null ? cockpit.position : transform.position;

        private void OnEnable()
        {
            EventChannel<HelicopterPartInstalledEvent>.Raised += HandlePartInstalled;
            var mgr = RescueEventManager.Instance;
            if (mgr != null) mgr.RegisterFrame(this);
            // Region bisa dilepas (#88): progres selalu dibaca dari IslandPhase saat region dimuat.
            RefreshVisuals();
            ReconcileLyingParts();
        }

        private void OnDisable()
        {
            EventChannel<HelicopterPartInstalledEvent>.Raised -= HandlePartInstalled;
            var mgr = RescueEventManager.Instance;
            if (mgr != null) mgr.UnregisterFrame(this);
        }

        private void HandlePartInstalled(HelicopterPartInstalledEvent e) => RefreshVisuals();

        private void RefreshVisuals()
        {
            var mgr = RescueEventManager.Instance;
            for (int i = 0; i < partVisuals.Length; i++)
            {
                var p = partVisuals[i];
                if (p == null || p.visual == null) continue;
                bool installed = mgr != null && Contains(mgr.InstalledPartIds, p.partId);
                p.visual.SetActive(installed);
            }
        }

        private static bool Contains(IReadOnlyList<string> list, string id)
        {
            for (int i = 0; i < list.Count; i++) if (list[i] == id) return true;
            return false;
        }

        /// <summary>
        /// Trigger tidak terpicu kalau region baru dimuat dan barang sudah di dalamnya. Baca records HeavyItemSystem,
        /// lalu pasang barang Lying yang jaraknya dari pusat zona kurang dari radius zona. Tanpa query fisika.
        /// </summary>
        private void ReconcileLyingParts()
        {
            var mgr = RescueEventManager.Instance;
            if (mgr == null || installZone == null) return;
            HeavyItemBridge.CollectLyingNear(installZone.Center, installZone.Radius, scratch);
            for (int i = 0; i < scratch.Count; i++) mgr.InstallPart(scratch[i]);
            scratch.Clear();
        }
    }
}
