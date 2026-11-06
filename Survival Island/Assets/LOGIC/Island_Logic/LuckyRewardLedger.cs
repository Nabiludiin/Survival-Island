using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Catatan hadiah bersama untuk keempat Lucky Machine. Blueprint: Island_Logic/LuckyMachine.md bagian 1
    /// "LuckyRewardLedger (bersama)" (#72). Keputusan developer 11: batas isUnique dan maxGrants dihitung bersama,
    /// sehingga Laser Pistol tetap hanya keluar sekali per run walau ada empat mesin.
    ///
    /// Penyimpangan kecil: blueprint menyebut "kelas biasa". Di sini MonoBehaviour di objek manager scene IslandPhase,
    /// supaya punya OnEnable/OnDisable untuk Register/Unregister ke WorldStateSave. Data dan API tetap sama.
    /// Ledger tidak ikut region, karena mesin region yang tidak dimuat tetap harus tahu hadiah yang sudah keluar.
    /// </summary>
    public class LuckyRewardLedger : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_lucky";

        public static LuckyRewardLedger Instance { get; private set; }

        private readonly Dictionary<string, int> grantCounts = new Dictionary<string, int>();
        // Skin yang menunggu item targetnya didapat. Dua daftar sejajar: skinId dan itemId target.
        private readonly List<string> pendingSkins = new List<string>();
        private readonly List<string> pendingSkinTargets = new List<string>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[LuckyRewardLedger] Instance ganda di scene IslandPhase.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnEnable()
        {
            EventChannel<InventoryChangedEvent>.Raised += HandleInventoryChanged;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Register(this);
        }

        private void OnDisable()
        {
            EventChannel<InventoryChangedEvent>.Raised -= HandleInventoryChanged;
            if (WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
        }

        // ---------------------------------------------------------------- API

        public int GetGrantCount(string rewardId) =>
            !string.IsNullOrEmpty(rewardId) && grantCounts.TryGetValue(rewardId, out int n) ? n : 0;

        /// <summary>false untuk hadiah isUnique yang sudah didapat, atau maxGrants yang sudah tercapai.</summary>
        public bool IsAvailable(LuckyRewardData r)
        {
            if (r == null) return false;
            int n = GetGrantCount(r.rewardId);
            if (r.isUnique && n > 0) return false;
            if (r.maxGrants > 0 && n >= r.maxGrants) return false;
            return true;
        }

        public void RecordGrant(string rewardId)
        {
            if (string.IsNullOrEmpty(rewardId)) return;
            grantCounts[rewardId] = GetGrantCount(rewardId) + 1;
        }

        public void AddPendingSkin(string skinId, string targetItemId)
        {
            if (string.IsNullOrEmpty(skinId) || string.IsNullOrEmpty(targetItemId)) return;
            for (int i = 0; i < pendingSkins.Count; i++)
                if (pendingSkins[i] == skinId && pendingSkinTargets[i] == targetItemId) return;
            pendingSkins.Add(skinId);
            pendingSkinTargets.Add(targetItemId);
        }

        /// <summary>Pasang skin tertunda ke inst kalau itemId cocok. true kalau ada yang terpasang.</summary>
        public bool TryApplyPendingSkins(ItemInstance inst)
        {
            if (inst == null || pendingSkins.Count == 0) return false;
            for (int i = 0; i < pendingSkins.Count; i++)
            {
                if (pendingSkinTargets[i] != inst.itemId) continue;
                inst.skinId = pendingSkins[i];
                pendingSkins.RemoveAt(i);
                pendingSkinTargets.RemoveAt(i);
                if (EquipmentManager.Instance != null) EquipmentManager.Instance.RefreshEquipped();
                return true;
            }
            return false;
        }

        private void HandleInventoryChanged(InventoryChangedEvent e)
        {
            if (e.delta <= 0 || pendingSkins.Count == 0 || InventoryManager.Instance == null) return;
            if (!pendingSkinTargets.Contains(e.itemId)) return;
            TryApplyPendingSkins(InventoryManager.Instance.FindFirstInstance(e.itemId));
        }

        // ---------------------------------------------------------------- ISaveable

        [Serializable]
        private class SaveData
        {
            // JsonUtility tidak bisa menyimpan Dictionary, jadi grantCounts diubah ke dua array sejajar.
            public string[] rewardIds;
            public int[] counts;
            public string[] pendingSkins;
            public string[] pendingSkinTargets;
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 130;

        public string CaptureState()
        {
            var d = new SaveData
            {
                rewardIds = new string[grantCounts.Count],
                counts = new int[grantCounts.Count],
                pendingSkins = pendingSkins.ToArray(),
                pendingSkinTargets = pendingSkinTargets.ToArray()
            };
            int i = 0;
            foreach (var kv in grantCounts) { d.rewardIds[i] = kv.Key; d.counts[i] = kv.Value; i++; }
            return JsonUtility.ToJson(d);
        }

        public void RestoreState(string json)
        {
            grantCounts.Clear();
            pendingSkins.Clear();
            pendingSkinTargets.Clear();
            var d = string.IsNullOrEmpty(json) ? null : JsonUtility.FromJson<SaveData>(json);
            if (d == null) return;
            if (d.rewardIds != null && d.counts != null)
                for (int i = 0; i < d.rewardIds.Length && i < d.counts.Length; i++)
                    if (!string.IsNullOrEmpty(d.rewardIds[i])) grantCounts[d.rewardIds[i]] = d.counts[i];
            if (d.pendingSkins != null && d.pendingSkinTargets != null)
                for (int i = 0; i < d.pendingSkins.Length && i < d.pendingSkinTargets.Length; i++)
                {
                    pendingSkins.Add(d.pendingSkins[i]);
                    pendingSkinTargets.Add(d.pendingSkinTargets[i]);
                }
        }

        public void OnRestoreFinished() { }
    }
}
