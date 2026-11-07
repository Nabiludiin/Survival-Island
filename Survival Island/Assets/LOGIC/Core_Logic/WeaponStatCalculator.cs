using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Satu-satunya tempat modifier statistik senjata dan alat digabung. Blueprint: Core_Logic/ToolRepairSystem.md bagian
    /// "Menghitung statistik" (#113). Urutan (usulan disetujui developer): dasar, upgrade, Lucky Machine (perkalian, #72),
    /// lalu batas damage base x upgrade.maxDamageMultiplier dan magazine minimum 1.
    ///
    /// Hasil dihitung saat item dipasang atau berubah, lalu disimpan pengendali senjata (tanpa hitung ulang per tembakan).
    /// Hasil menjadi DamageInfo.amount (DamagePipeline.md langkah 1); armor dan headshot 2x dihitung sesudahnya.
    ///
    /// Catatan implementasi: WeaponStats memakai struct milik GunController (WeaponData.cs). "speed" diterapkan sebagai
    /// pembagi secondsBetweenShots (jeda tembak atau jeda ayun untuk melee). magazineSize 0 berarti senjata tanpa magazine
    /// (melee dan alat), sehingga bonus magazine tidak berlaku. Pencarian ItemUpgradeData dan WeaponModifierData memakai
    /// folder Resources/ItemUpgrades dan Resources/WeaponModifiers (belum ada database resmi), dan bisa diganti lewat Register*.
    /// </summary>
    public static class WeaponStatCalculator
    {
        public const string UpgradesFolder = "ItemUpgrades";
        public const string ModifiersFolder = "WeaponModifiers";

        private static List<ItemUpgradeData> upgrades;
        private static Dictionary<string, WeaponModifierData> modifiers;
        private static HashSet<string> warnedModifiers;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        /// <summary>Statistik akhir dari nilai dasar dan ItemInstance (upgradeMask, luckyModifierId).</summary>
        public static WeaponStats Get(in WeaponStats baseStats, ItemInstance inst)
        {
            var r = baseStats;
            if (inst == null) return r;
            var up = Balance != null ? Balance.upgrade : null;
            bool hasMagazine = baseStats.magazineSize > 0;

            // 2. Upgrade (#113).
            if (up != null)
            {
                if (HasUpgrade(inst, UpgradeKind.Damage)) r.damage *= 1f + up.damageBonus;
                if (HasUpgrade(inst, UpgradeKind.Speed)) r.secondsBetweenShots /= 1f + up.speedBonus;
                if (hasMagazine && HasUpgrade(inst, UpgradeKind.MagazineSize)) r.magazineSize += up.magazineBonus;
            }

            // 3. Lucky Machine (#72). Keputusan developer 13: magazineBonus tidak berlaku untuk alat.
            var mod = FindModifier(inst.luckyModifierId);
            if (mod != null)
            {
                r.damage *= mod.damageMultiplier;
                if (mod.speedMultiplier > 0f) r.secondsBetweenShots /= mod.speedMultiplier;
                if (hasMagazine) r.magazineSize += mod.magazineBonus;
            }

            // 4. Batas.
            float maxMul = up != null ? up.maxDamageMultiplier : 4f;
            r.damage = Mathf.Min(r.damage, baseStats.damage * maxMul);
            if (hasMagazine) r.magazineSize = Mathf.Max(1, r.magazineSize);
            return r;
        }

        /// <summary>Damage melee atau alat setelah upgrade dan modifier (ItemData.weaponDamage sebagai nilai dasar).</summary>
        public static float GetMeleeDamage(float baseDamage, ItemInstance inst)
        {
            var s = Get(new WeaponStats { damage = baseDamage, secondsBetweenShots = 1f }, inst);
            return s.damage;
        }

        // ---------------------------------------------------------------- Upgrade

        public static bool HasUpgrade(ItemInstance inst, UpgradeKind kind)
        {
            if (inst == null || inst.upgradeMask == 0) return false;
            EnsureUpgrades();
            for (int i = 0; i < upgrades.Count; i++)
                if (upgrades[i].kind == kind && (inst.upgradeMask & upgrades[i].Mask) != 0) return true;
            return false;
        }

        /// <summary>Semua ItemUpgradeData yang terdaftar.</summary>
        public static IReadOnlyList<ItemUpgradeData> AllUpgrades
        {
            get { EnsureUpgrades(); return upgrades; }
        }

        /// <summary>Jumlah bit upgrade terpasang.</summary>
        public static int CountUpgrades(ItemInstance inst)
        {
            if (inst == null) return 0;
            int n = 0;
            for (uint m = (uint)inst.upgradeMask; m != 0; m &= m - 1) n++;
            return n;
        }

        public static void RegisterUpgrades(IEnumerable<ItemUpgradeData> list)
        {
            upgrades = new List<ItemUpgradeData>();
            if (list == null) return;
            foreach (var u in list) if (u != null) upgrades.Add(u);
        }

        private static void EnsureUpgrades()
        {
            if (upgrades == null) RegisterUpgrades(Resources.LoadAll<ItemUpgradeData>(UpgradesFolder));
        }

        // ---------------------------------------------------------------- Modifier Lucky Machine

        /// <summary>null kalau kosong atau tidak dikenal (diabaikan dengan peringatan sekali, ToolRepairSystem.md).</summary>
        public static WeaponModifierData FindModifier(string modifierId)
        {
            if (string.IsNullOrEmpty(modifierId)) return null;
            if (modifiers == null) RegisterModifiers(Resources.LoadAll<WeaponModifierData>(ModifiersFolder));
            if (modifiers.TryGetValue(modifierId, out var m)) return m;
            if (warnedModifiers.Add(modifierId))
                Debug.LogWarning($"[WeaponStatCalculator] luckyModifierId {modifierId} tidak dikenal. Diabaikan.");
            return null;
        }

        public static void RegisterModifiers(IEnumerable<WeaponModifierData> list)
        {
            modifiers = new Dictionary<string, WeaponModifierData>(StringComparer.Ordinal);
            warnedModifiers = new HashSet<string>(StringComparer.Ordinal);
            if (list == null) return;
            foreach (var m in list)
                if (m != null && !string.IsNullOrEmpty(m.modifierId) && !modifiers.ContainsKey(m.modifierId))
                    modifiers.Add(m.modifierId, m);
        }
    }
}
