using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Viewmodel palu (Tool_Hammer). Meneruskan klik kiri sesuai mode palu, dan klik kanan membuka BuildMenu.
    /// Blueprint: raft/BuildMenu.md "Klik kiri palu" dan tabel input menu radial (#79).
    ///
    /// Belum ada di Langkah 4 (TODO #47, BuildingIntegrity): mode Repair (TryRepairSwing) dan Upgrade.
    /// Hitung tahan Upgrade sudah disiapkan sama dengan Demolish, tetapi belum memanggil apa pun.
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
                    // TODO #47: BuildingIntegrity.TryRepairSwing(), diulang sesuai jeda ayunan saat ditahan.
                    break;
                case HammerMode.Upgrade:
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
                    // TODO #113: durabilitas palu -= build.hammerDecayPerDemolish.
                }
            }
            // TODO #47: mode Upgrade memanggil BuildingIntegrity setelah tahan selesai.
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
