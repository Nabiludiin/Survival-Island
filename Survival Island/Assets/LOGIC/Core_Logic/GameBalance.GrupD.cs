namespace SurvivalIsland
{
    // =====================================================================================
    // Angka penyetelan Grup D. Nilai awal sesuai tabel "Angka penyetelan" tiap blueprint dan tabel
    // "Bagian config per sistem" di Core_Logic/GameBalanceConfig.md. Semua bagian sudah ada di GameBalanceConfig.
    // =====================================================================================

    /// <summary>NPC teman (NPCRoster.md, NPCCompanion.md, #76). Semua angka usulan disetujui developer.</summary>
    public partial class NpcBalance
    {
        public float thinkSeconds = 0.25f;              // Jeda pembaruan FSM
        public float followDistance = 5f;
        public float followIndoorDistance = 2.5f;       // Saat pemain di dalam base
        public float teleportDistance = 40f;            // Warp saat Follow tertinggal jauh
        public float detectRadius = 15f;                // Jarak mencari musuh untuk Attack
        public float attackLeashDistance = 12f;         // Jarak Attack dari pemain
        public float baseRegenPerSec = 0.5f;            // HP pulih di base
        public float workSearchRadius = 30f;            // Jarak mencari tempat kerja
        public float cookSpeedMultiplier = 1.25f;       // Skill Koki
        public float builderRepairMultiplier = 1.25f;   // Skill Tukang pada perbaikan
        public int avoidancePriority = 70;              // Agen NPC memberi jalan (#87)
        public float corpseSeconds = 60f;               // Lama mayat sebelum dihapus
        public float followThreatWeight = 0.6f;         // ThreatWeight normal (#108)
        public float coverThreatWeight = 0.3f;          // ThreatWeight saat TakeCover (#108)
        // Nama usulan sistem pendukung (dicatat di Catatan Implementasi NPCCompanion.md). Blueprint menyebut angkanya di teks.
        public float doorKeepAwayRadius = 1.5f;         // NPC Follow tidak berdiri dekat pintu
        public float commandBlockEnemyRadius = 5f;      // Perintah E ditolak saat bertarung jarak dekat
        public float fireAvoidMargin = 2f;              // Margin FireSystem.IsNearFire (#97)
    }

    /// <summary>Tambahan interaksi pulau (IslandInteractable.md, #17).</summary>
    public partial class InteractBalance
    {
        // Usulan sistem pendukung: blueprint hanya menyebut "radius kecil, mengikuti #49". Nilai awal = stealth.noiseWalk.
        public float containerNoiseRadius = 8f;
    }

    /// <summary>Tujuan dan Diary (ObjectiveManager.md, #81).</summary>
    public partial class ObjectiveBalance
    {
        public int baseWallCount = 4;                   // Usulan disetujui: 4 dinding + 1 pintu
        public float hudFlashSeconds = 3f;
        public int chapter2TriggerDay = 2;              // Keputusan developer
        public RegionType chapter3TriggerRegion = RegionType.Amazon;   // Keputusan developer. Jangan region di balik S2
    }

    /// <summary>Temuan lore (LoreCollectibles.md, #100).</summary>
    public partial class LoreBalance
    {
        public int maxPartHintsPerRun = 2;
    }

    /// <summary>Ucapan NPC (LoreCollectibles.md bagian NPCBarks, #100).</summary>
    public partial class BarksBalance
    {
        public float checkSeconds = 2f;
        public float globalCooldownSeconds = 6f;
        public float npcCooldownSeconds = 20f;
        public float categoryCooldownSeconds = 90f;
        public float hearRadius = 25f;
        public float greetRadius = 6f;
        public float greetAwaySeconds = 120f;
        public float hurtHpRatio = 0.4f;
        public float zombieAlertRadius = 15f;
    }

    /// <summary>Simulasi base yang ditinggal (RemoteBaseSimulation.md, #90). Semua angka usulan disetujui developer.</summary>
    public partial class RemoteBaseBalance
    {
        // Jarak
        public float remoteDistance = 80f;
        public float returnDistance = 60f;
        public float distanceCheckSeconds = 2f;
        public float clusterRadius = 30f;
        // Serangan
        public int minStructuresForRaid = 4;
        public int maxRaidedClusters = 2;
        public int firstRaidDay = 3;
        public float raidChanceBase = 0.5f;
        public float raidChancePerDay = 0.01f;
        public float raidChanceMax = 0.9f;
        public float basePower = 150f;
        public float powerPerDay = 20f;
        public float maxPower = 2550f;
        public float bloodMoonMultiplier = 2f;
        public float referenceStructures = 12f;
        public float sizeFactorMin = 0.75f;
        public float sizeFactorMax = 1.5f;
        public float varianceFraction = 0.15f;
        // Pertahanan
        public float scoreWallWood = 10f;
        public float scoreWallStone = 25f;
        public float scoreTrap = 15f;
        public float scoreNpcGuard = 20f;
        public float mitigationK = 200f;
        public float maxMitigation = 0.8f;
        // Penerapan
        public float hitChunk = 60f;
        public float doorWeight = 2f;
        public float trapWearFraction = 0.3f;
        public float raidStartHourMin = 20f;
        public float raidStartHourMax = 23f;
        public float raidEndHour = 5f;
        // NPC
        public float npcInjuryChanceMax = 0.6f;
        public float npcInjuryDamage = 40f;
        public float npcDeathChanceOnBreach = 0.35f;
        public float npcGuardDeathMultiplier = 0.5f;
        public float npcGuardMinHpRatio = 0.25f;
        // Peralihan
        public float takeoverZombiesPerPower = 0.02f;
        public int takeoverMaxZombies = 12;
    }

    /// <summary>Pengaman jalur menang (ProgressionSafety.md, #116). Semua angka usulan disetujui developer.</summary>
    public partial class SafetyBalance
    {
        public int unstuckRingSize = 12;
        public float unstuckSampleSeconds = 5f;
        public float unstuckNavSampleRadius = 2f;
        public float unstuckMinRewindDistance = 3f;
        public float unstuckChannelSeconds = 5f;
        public float unstuckCooldownSeconds = 180f;
        public float stuckHintSeconds = 20f;
        public int minStonePickupsRegion1 = 4;
        public float woodPerSecondEstimate = 0.4f;
        public float regionTravelSeconds = 300f;
        public float partRoundTripSeconds = 240f;
        public float estimateMargin = 1.5f;
        // Nama usulan sistem pendukung: blueprint menulis "kurang dari 0,5 meter" di teks.
        public float stuckHintMoveThreshold = 0.5f;
    }
}
