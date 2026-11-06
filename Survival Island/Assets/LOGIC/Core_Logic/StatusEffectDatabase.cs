using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pencarian StatusEffectData lewat effectId (GLOSSARY.md, #20). Dipakai StatusEffectManager.ApplyById dan saat load.
    /// Catatan implementasi (usulan teknis): berupa aset ScriptableObject berisi daftar efek, diisi di Inspector.
    /// Dictionary dibangun sekali saat pertama dipakai.
    /// </summary>
    [CreateAssetMenu(menuName = "Survival/Status Effect Database")]
    public class StatusEffectDatabase : ScriptableObject
    {
        [SerializeField] private StatusEffectData[] effects = Array.Empty<StatusEffectData>();

        [NonSerialized] private Dictionary<string, StatusEffectData> byId;

        public IReadOnlyList<StatusEffectData> All => effects;

        /// <summary>null kalau effectId tidak ditemukan.</summary>
        public StatusEffectData Get(string effectId)
        {
            if (string.IsNullOrEmpty(effectId)) return null;
            if (byId == null) Build();
            return byId.TryGetValue(effectId, out var d) ? d : null;
        }

        private void Build()
        {
            byId = new Dictionary<string, StatusEffectData>(StringComparer.Ordinal);
            if (effects == null) return;
            for (int i = 0; i < effects.Length; i++)
            {
                var e = effects[i];
                if (e == null || string.IsNullOrEmpty(e.effectId)) continue;
                if (byId.ContainsKey(e.effectId))
                {
                    Debug.LogWarning($"[StatusEffectDatabase] effectId ganda: {e.effectId}. Aset {e.name} diabaikan.", this);
                    continue;
                }
                byId.Add(e.effectId, e);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            byId = null;
        }
#endif
    }
}
