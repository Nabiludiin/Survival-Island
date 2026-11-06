using System;

namespace SurvivalIsland
{
    // =====================================================================================
    // Kelas tiap bagian GameBalanceConfig. Blueprint: Core_Logic/GameBalanceConfig.md (#84).
    //
    // Langkah 1 mengisi: angka inti (time, vitals, resource, region, weather, difficulties)
    // dan bagian yang dipakai sistem Langkah 1 (run, ui, runSeed, save).
    // Bagian lain masih kosong. Langkah pemiliknya mengisi field sesuai tabel
    // "Bagian config per sistem" di GameBalanceConfig.md, dengan nilai awal dari blueprint pemilik.
    // Nilai di sini adalah nilai awal saat aset dibuat. Nilai yang berlaku dibaca dari aset.
    // =====================================================================================

    // ---------------------------------------------------------------- Angka inti

    /// <summary>Waktu dunia. Target: 1 hari game = 15 menit (usulan disetujui developer, #84).</summary>
    [Serializable]
    public class TimeBalance
    {
        public float timeMultiplier = 96f;      // 1 hari = 900 detik
        public float dayLengthSeconds = 900f;   // Hanya informasi
    }

    /// <summary>Status tubuh pemain (angka inti #84). Tambahan #111 diisi Langkah 3/4.</summary>
    [Serializable]
    public class VitalsBalance
    {
        public float hungerDrainPerSec = 0.11f;            // Lapar habis dalam 1 hari game
        public float thirstDrainPerSec = 0.15f;            // Haus habis dalam sekitar 0,75 hari game
        public float sprintThirstMultiplier = 1.5f;
        public float starvationDamagePerSec = 0.5f;
        public float dehydrationDamagePerSec = 0.5f;
        public float frostbiteStaminaDrainPerSec = 5f;     // GDD bagian 5, Region 3: stamina dikuras dulu
        public float frostbiteDamagePerSec = 1f;           // Lalu HP. Diturunkan dari 5 ke 1 (disetujui developer)
        // TODO Langkah 3/4: tambahan #111 (damage tenggelam dan infeksi, pemulihan HP, stamina, interval event).
    }

    /// <summary>Panen (angka inti #84). Tambahan #73 diisi Langkah 4.</summary>
    [Serializable]
    public class ResourceBalance
    {
        public int treeDropMin = 8;
        public int treeDropMax = 12;
        public float axeDecayPerUse = 1f;
        public float harvestNoiseRadius = 10f;   // Disetujui developer (#61)
        // TODO Langkah 4: tambahan #73 (rockDrop, respawn, dll.).
    }

    /// <summary>Region (angka inti #84). Tambahan #68 diisi Langkah 6.</summary>
    [Serializable]
    public class RegionBalance
    {
        public int bridgeWoodCost = 500;                 // GDD bagian 5: nilai awal 500, bisa disetel
        public float heavyItemSpeedMultiplier = 0.5f;    // Claude.md Tahap 4, Region 4
        public int helicopterPartCount = 5;              // Disetujui developer (#69)
        // TODO Langkah 6: tambahan #68 (bridgeDepositHoldSeconds, ravineMinWidth, gateVerifySeconds, ravineFallDamage).
    }

    /// <summary>Cuaca (angka inti #84). Tambahan #115 diisi Langkah 6.</summary>
    [Serializable]
    public class WeatherBalance
    {
        public float weatherIntervalGameHours = 8f;   // Waktu game, bukan waktu nyata
        // TODO Langkah 6: tambahan #115 (effects, minSevereDay, presentBlendSeconds, audio).
    }

    /// <summary>Profil kesulitan. Versi awal hanya satu profil Normal dengan semua pengali 1 (#84).</summary>
    [Serializable]
    public class DifficultyProfile
    {
        public string profileName = "Normal";
        public float hungerMultiplier = 1f;
        public float thirstMultiplier = 1f;
        public float enemyDamageMultiplier = 1f;
        // TODO: pengali jumlah musuh dan hasil panen disebut di blueprint tanpa nama field. Diberi nama oleh Langkah pemakainya.
    }

    // ---------------------------------------------------------------- Bagian yang dipakai Langkah 1

    /// <summary>Alur run (#82, #99). Dipakai RunLifecycle, AsyncSceneLoader, UIManager.</summary>
    [Serializable]
    public class RunBalance
    {
        public float endSequenceTimeoutSeconds = 15f;
        public float endingTimeoutSeconds = 120f;
        public float gameOverInputDelaySeconds = 1f;
        public float saveWaitTimeoutSeconds = 5f;
        public float sceneLoadMinShowSeconds = 0.5f;
    }

    /// <summary>UI (#83). Dipakai UIFocusStack dan UIManager.</summary>
    [Serializable]
    public class UIBalance
    {
        public float panelFadeSeconds = 0.15f;
        public float pausedMusicVolumeDb = -12f;
        public float pauseAudioFadeSeconds = 0.2f;
        public float loadingBarSpeed = 1.5f;             // Usulan UIManager.md (disetujui sebagai nilai awal)
        public float confirmInputDelaySeconds = 0.25f;   // Usulan UIManager.md (disetujui sebagai nilai awal)
    }

    /// <summary>Seed run (#89). Dipakai RunSeedManager.</summary>
    [Serializable]
    public class RunSeedBalance
    {
        public int maxSeedRerolls = 8;
        public float lootPointEmptyChance = 0.5f;
        public int minShipFoodItems = 3;
        public int minShipAmmoItems = 2;
        public int minShipDrinkItems = 1;
        public float minPartPointSpacing = 30f;
        public int firstMajorDisasterDay = 3;
        public int minDaysBetweenMajorDisasters = 4;
    }

    /// <summary>Save (#80). Dipakai WorldStateSave.</summary>
    [Serializable]
    public class SaveBalance
    {
        public float combatEnemyRadius = 30f;
        public float combatQuietSeconds = 10f;
        public float autosaveRetrySeconds = 10f;
        public float autosaveMaxDeferSeconds = 120f;
        public float autosaveMinHpRatio = 0.1f;
        public int combatRestoreMaxEnemies = 10;
        public float combatRestoreRadius = 20f;
    }

    // ---------------------------------------------------------------- Bagian yang belum diisi (kerangka)

    /// <summary>Bagian input (#18). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class InputBalance { }

    /// <summary>Bagian streaming (#88). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class StreamingBalance { }

    /// <summary>Bagian damage (#59). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class DamageBalance { }

    /// <summary>Bagian equipment (#60). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class EquipmentBalance { }

    /// <summary>Bagian items (#66). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ItemsBalance { }

    /// <summary>Bagian interact (#17). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class InteractBalance { }

    /// <summary>Bagian camera (#102). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class CameraBalance { }

    /// <summary>Bagian traversal (#101, #94). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class TraversalBalance { }

    /// <summary>Bagian carry (#118). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class CarryBalance { }

    /// <summary>Bagian console (#56). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ConsoleBalance { }

    /// <summary>Bagian ship (#74, #75). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ShipBalance { }

    /// <summary>Bagian shipHazard (#75). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ShipHazardBalance { }

    /// <summary>Bagian infection (#64). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class InfectionBalance { }

    /// <summary>Bagian hydration (#43). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class HydrationBalance { }

    /// <summary>Bagian campfire (#44). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class CampfireBalance { }

    /// <summary>Bagian disease (#45). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class DiseaseBalance { }

    /// <summary>Bagian statusEffects (#20). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class StatusEffectsBalance { }

    /// <summary>Bagian crafting (#77). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class CraftingBalance { }

    /// <summary>Bagian repair (#113). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class RepairBalance { }

    /// <summary>Bagian upgrade (#113). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class UpgradeBalance { }

    /// <summary>Bagian fishing (#55). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class FishingBalance { }

    /// <summary>Bagian debris (#106). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class DebrisBalance { }

    /// <summary>Bagian surface (#86). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class SurfaceBalance { }

    /// <summary>Bagian build (#79). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class BuildBalance { }

    /// <summary>Bagian baseNav (#78). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class BaseNavBalance { }

    /// <summary>Bagian integrity (#47). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class IntegrityBalance { }

    /// <summary>Bagian storage (#46). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class StorageBalance { }

    /// <summary>Bagian sleep (#67). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class SleepBalance { }

    /// <summary>Bagian gun (#58). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class GunBalance { }

    /// <summary>Bagian enemyAttack (#109). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class EnemyAttackBalance { }

    /// <summary>Bagian hitReaction (#95). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class HitReactionBalance { }

    /// <summary>Bagian threat (#108). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ThreatBalance { }

    /// <summary>Bagian stealth (#49). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class StealthBalance { }

    /// <summary>Bagian wildlife (#61). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class WildlifeBalance { }

    /// <summary>Bagian spawn (#62). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class SpawnBalance { }

    /// <summary>Bagian sunlight (#63). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class SunlightBalance { }

    /// <summary>Bagian specialZombie (#25). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class SpecialZombieBalance { }

    /// <summary>Bagian aiDirector (#27). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class AIDirectorBalance { }

    /// <summary>Bagian crowd (#93). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class CrowdBalance { }

    /// <summary>Bagian pinned (#110). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class PinnedBalance { }

    /// <summary>Bagian trap (#26). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class TrapBalance { }

    /// <summary>Bagian throwable (#96). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ThrowableBalance { }

    /// <summary>Bagian lair (#112). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class LairBalance { }

    /// <summary>Bagian warmth (#65). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class WarmthBalance { }

    /// <summary>Bagian heavyItem (#69). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class HeavyItemBalance { }

    /// <summary>Bagian swimming (#13). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class SwimmingBalance { }

    /// <summary>Bagian tide (#91). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class TideBalance { }

    /// <summary>Bagian environment (#92). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class EnvironmentBalance { }

    /// <summary>Bagian disaster (#70). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class DisasterBalance { }

    /// <summary>Bagian beachSupply (#71). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class BeachSupplyBalance { }

    /// <summary>Bagian bloodMoon (#50). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class BloodMoonBalance { }

    /// <summary>Bagian luckyMachine (#72). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class LuckyMachineBalance { }

    /// <summary>Bagian npc (#76). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class NpcBalance { }

    /// <summary>Bagian objective (#81). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ObjectiveBalance { }

    /// <summary>Bagian loot (#103). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class LootBalance { }

    /// <summary>Bagian remoteBase (#90). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class RemoteBaseBalance { }

    /// <summary>Bagian fire (#97). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class FireBalance { }

    /// <summary>Bagian light (#98). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class LightBalance { }

    /// <summary>Bagian flashlight (#98). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class FlashlightBalance { }

    /// <summary>Bagian rescue (#14). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class RescueBalance { }

    /// <summary>Bagian deathCinematic (#53). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class DeathCinematicBalance { }

    /// <summary>Bagian stats (#57). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class StatsBalance { }

    /// <summary>Bagian narrative (#99). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class NarrativeBalance { }

    /// <summary>Bagian safety (#116). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class SafetyBalance { }

    /// <summary>Bagian animation (#12). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class AnimationBalance { }

    /// <summary>Bagian vfx (#15). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class VfxBalance { }

    /// <summary>Bagian tutorial (#16). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class TutorialBalance { }

    /// <summary>Bagian settings (#19). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class SettingsBalance { }

    /// <summary>Bagian footstep (#21). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class FootstepBalance { }

    /// <summary>Bagian notification (#22). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class NotificationBalance { }

    /// <summary>Bagian postProcess (#23). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class PostProcessBalance { }

    /// <summary>Bagian shake (#24). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ShakeBalance { }

    /// <summary>Bagian combatFeedback (#51). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class CombatFeedbackBalance { }

    /// <summary>Bagian ambientAudio (#52). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class AmbientAudioBalance { }

    /// <summary>Bagian combatMusic (#114). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class CombatMusicBalance { }

    /// <summary>Bagian lore (#100). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class LoreBalance { }

    /// <summary>Bagian barks (#100). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class BarksBalance { }

    /// <summary>Bagian localization (#104). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class LocalizationBalance { }

    /// <summary>Bagian optimization (#62, #93). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class OptimizationBalance { }

    /// <summary>Bagian compass (#30). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class CompassBalance { }

    /// <summary>Bagian hazard (#59). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class HazardBalance { }
}
