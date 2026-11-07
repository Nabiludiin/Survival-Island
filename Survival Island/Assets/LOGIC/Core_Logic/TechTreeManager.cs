using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Status node tech tree. Singleton per scene di IslandPhase, bukan ISaveable: statusnya disimpan CraftingManager
    /// di entri world_recipes. Blueprint: Core_Logic/TechTreeManager.md (#34, #77, #80). Usulan, belum ada di GDD.
    ///
    /// Validasi Editor ada di TechTreeAuditWindow (menu Survival > Audit > Tech Tree).
    /// </summary>
    [DisallowMultipleComponent]
    public class TechTreeManager : MonoBehaviour
    {
        public static TechTreeManager Instance { get; private set; }

        [SerializeField] private TechNodeData[] nodes;
        [SerializeField] private AudioClip unlockClip;

        private readonly Dictionary<string, TechNodeData> nodesById = new Dictionary<string, TechNodeData>(StringComparer.Ordinal);
        private readonly HashSet<string> unlocked = new HashSet<string>(StringComparer.Ordinal);
        private readonly HashSet<string> warnedUnknown = new HashSet<string>(StringComparer.Ordinal);
        private bool listensCrafted, listensBuilt;

        private void Awake()
        {
            Instance = this;
            nodesById.Clear();
            if (nodes == null) return;
            for (int i = 0; i < nodes.Length; i++)
            {
                var n = nodes[i];
                if (n == null) continue;
                if (string.IsNullOrEmpty(n.nodeId)) { Debug.LogError($"[TechTreeManager] Node '{n.name}' tanpa nodeId."); continue; }
                if (nodesById.ContainsKey(n.nodeId)) { Debug.LogError($"[TechTreeManager] nodeId ganda: {n.nodeId}. Node kedua diabaikan."); continue; }
                nodesById.Add(n.nodeId, n);
                if (n.trigger == TechUnlockTrigger.ItemCrafted) listensCrafted = true;
                else if (n.trigger == TechUnlockTrigger.StructureBuilt) listensBuilt = true;
            }
        }

        private void OnEnable()
        {
            // Langganan hanya kalau ada node dengan pemicu itu.
            if (listensCrafted) EventChannel<ItemCraftedEvent>.Raised += HandleItemCrafted;
            if (listensBuilt) EventChannel<StructureBuiltEvent>.Raised += HandleStructureBuilt;
        }

        private void OnDisable()
        {
            EventChannel<ItemCraftedEvent>.Raised -= HandleItemCrafted;
            EventChannel<StructureBuiltEvent>.Raised -= HandleStructureBuilt;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Status

        /// <summary>ID kosong berarti tanpa syarat (true). ID tidak dikenal: false, dengan satu peringatan per ID.</summary>
        public bool IsUnlocked(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId)) return true;
            if (!nodesById.ContainsKey(nodeId))
            {
                if (warnedUnknown.Add(nodeId)) Debug.LogWarning($"[TechTreeManager] nodeId tidak dikenal: {nodeId}");
                return false;
            }
            return unlocked.Contains(nodeId);
        }

        /// <summary>Node dikenal, belum terbuka, dan prasyaratnya terbuka.</summary>
        public bool CanUnlock(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId) || !nodesById.TryGetValue(nodeId, out var n)) return false;
            return !unlocked.Contains(nodeId) && IsUnlocked(n.requiredNodeId);
        }

        public bool UnlockNode(string nodeId)
        {
            if (string.IsNullOrEmpty(nodeId) || !nodesById.TryGetValue(nodeId, out var n)) return false;
            if (unlocked.Contains(nodeId)) return false;
            // Pemicu tidak disimpan: pemain mengulang pemicunya setelah prasyarat terbuka.
            if (!IsUnlocked(n.requiredNodeId)) return false;
            unlocked.Add(nodeId);
            AudioManager.Instance?.PlaySFX(unlockClip);   // SFX node terbuka (#16)
            EventChannel<TechNodeUnlockedEvent>.Publish(new TechNodeUnlockedEvent(nodeId));
            return true;
        }

        private void HandleItemCrafted(ItemCraftedEvent e) => UnlockByTrigger(TechUnlockTrigger.ItemCrafted, e.recipeId);

        private void HandleStructureBuilt(StructureBuiltEvent e) => UnlockByTrigger(TechUnlockTrigger.StructureBuilt, e.buildableId);

        private void UnlockByTrigger(TechUnlockTrigger trigger, string key)
        {
            if (nodes == null || string.IsNullOrEmpty(key)) return;
            // Daftar kecil: pencarian linear, tanpa garbage. Urutan array menentukan node berantai.
            for (int i = 0; i < nodes.Length; i++)
            {
                var n = nodes[i];
                if (n != null && n.trigger == trigger && n.triggerKey == key) UnlockNode(n.nodeId);
            }
        }

        // ---------------------------------------------------------------- Save lewat CraftingManager (world_recipes)

        /// <summary>Salinan terurut, supaya isi save stabil.</summary>
        public string[] GetUnlockedIds()
        {
            var ids = new string[unlocked.Count];
            unlocked.CopyTo(ids);
            Array.Sort(ids, StringComparer.Ordinal);
            return ids;
        }

        /// <summary>Dipanggil CraftingManager.RestoreState. Tanpa event dan tanpa suara.</summary>
        public void RestoreUnlocked(string[] ids)
        {
            unlocked.Clear();
            if (ids == null) return;
            for (int i = 0; i < ids.Length; i++)
            {
                if (string.IsNullOrEmpty(ids[i])) continue;
                if (nodesById.ContainsKey(ids[i])) unlocked.Add(ids[i]);
                else Debug.LogWarning($"[TechTreeManager] Node di save tidak dikenal dan dibuang: {ids[i]}");
            }
        }
    }
}
