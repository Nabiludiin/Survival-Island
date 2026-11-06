using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Viewmodel palu (Tool_Hammer). Meneruskan klik kiri sesuai mode palu, dan klik kanan membuka BuildMenu.
    /// Blueprint: raft/BuildMenu.md "Klik kiri palu" dan tabel input menu radial (#79).
    ///
    /// Mode Repair dan Upgrade diteruskan ke BuildingIntegrity (#47): TryRepairSwing dan TickUpgrade.
    /// Hitung tahan Demolish tetap di sini; tahan Upgrade dihitung BuildingIntegrity.
    /// </summary>
    public class HammerEquippable : EquippableBehaviour
    {
        private float holdTimer;
        private StructureNode holdTarget;
        private bool holding;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        public override void OnUnequip()
        {
            // Hologram hilang dan hitung tahan batal. Pilihan terakhir tetap diingat BuildManager.
            if (BuildManager.Instance != null) BuildManager.Instance.HideGhost();
            // Tahan upgrade yang sedang berjalan ikut batal (#47).
            if (BuildingIntegrity.Instance != null) BuildingIntegrity.Instance.TickUpgrade(ActionPhase.Released);
            CancelHold();
            base.OnUnequip();
        }

        public override void PrimaryAction(ActionPhase phase)
        {
            var bm = BuildManager.Instance;
            if (bm == null) return;
            switch (bm.Mode)
            {
                case HammerMode.Build:
                    if (phase == ActionPhase.Pressed) bm.TryExecuteBuild();
                    break;
                case HammerMode.Repair:
                    // Ayunan perbaikan (#47), diulang sesuai jeda ayunan saat ditahan.
                    if (BuildingIntegrity.Instance != null && phase != ActionPhase.Released) BuildingIntegrity.Instance.TryRepairSwing();
                    break;
                case HammerMode.Upgrade:
                    // Tahan untuk upgrade (#47), dihitung BuildingIntegrity.
                    if (BuildingIntegrity.Instance != null) BuildingIntegrity.Instance.TickUpgrade(phase);
                    break;
                case HammerMode.Demolish:
                    TickHold(phase, bm.Mode);
                    break;
            }
        }

        public override void SecondaryAction(ActionPhase phase)
        {
            // Klik kanan: buka menu dengan sekali klik (Action Map berganti saat panel terbuka).
            if (phase == ActionPhase.Pressed) BuildMenu.Open();
        }

        private void TickHold(ActionPhase phase, HammerMode mode)
        {
            var registry = StructureRegistry.Instance;
            if (registry == null) return;
            if (phase == ActionPhase.Released) { CancelHold(); return; }

            registry.TryGetAimedStructure(out var aimed);
            if (phase == ActionPhase.Pressed || aimed != holdTarget)
            {
                // Target berganti: hitung ulang dari awal.
                holdTarget = aimed;
                holdTimer = 0f;
                holding = aimed != null;
                // TODO HUD: tampilkan registry.PreviewCollapse(aimed) sebelum bongkar.
                return;
            }
            if (!holding || holdTarget == null) return;

            // Waktu tahan memakai Time.deltaTime, jadi berhenti saat game dijeda.
            holdTimer += Time.deltaTime;
            float need = Balance != null ? Balance.build.demolishHoldSeconds : 1.2f;
            if (holdTimer < need) return;

            if (mode == HammerMode.Demolish)
            {
                if (registry.TryDemolish(holdTarget))
                {
                    // Durabilitas palu -= build.hammerDecayPerDemolish (#113).
                    var eq = EquipmentManager.Instance;
                    if (eq != null) DurabilitySystem.ApplyWear(eq.ActiveInstance, Balance != null ? Balance.build.hammerDecayPerDemolish : 1f);
                }
            }
            CancelHold();
        }

        private void CancelHold()
        {
            holding = false;
            holdTimer = 0f;
            holdTarget = null;
        }
    }
}
