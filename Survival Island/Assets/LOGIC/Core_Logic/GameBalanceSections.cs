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

        // Tambahan VitalsRules.md (#111), diisi Langkah 3. Semua usulan yang disetujui developer sebagai nilai awal.
        public float drownDamagePerSec = 10f;               // HP penuh habis 10 detik setelah oksigen habis (#13)
        public float infectionDamagePerSec = 0.3f;          // Keputusan developer: infeksi 100 tidak membunuh seketika
        public float infectionDamageThreshold = 75f;        // Selaras tahap 4 di #64
        public float regenPerSec = 0.25f;                   // HP penuh pulih dalam 400 detik
        public float regenMinHunger = 50f;
        public float regenMinThirst = 50f;
        public float regenMaxInfection = 50f;
        public float regenDelayAfterDamageSeconds = 12f;
        public float sprintStaminaPerSec = 10f;
        public float jumpStaminaCost = 8f;
        public float meleeStaminaCost = 15f;
        public float swimStaminaPerSec = 6f;
        public float climbStaminaPerSec = 5f;
        public float staminaRegenPerSec = 15f;
        public float staminaRegenDelaySeconds = 1.5f;
        public float exhaustedRecoverThreshold = 25f;
        public float exhaustedMeleeCooldownMultiplier = 1.5f;
        public float exhaustedMeleeDamageMultiplier = 0.7f;
        public float lowVitalThreshold = 25f;
        public float lowHungerMaxStaminaMultiplier = 0.75f;
        public float lowThirstRegenMultiplier = 0.5f;
        public float heavyStaminaRegenMultiplier = 0.75f;
        public float lowHPWarningRatio = 0.25f;
        public float vitalsPublishIntervalSeconds = 0.25f;  // VitalsChangedEvent 4 kali per detik
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

    /// <summary>Bagian input (#18). Diisi Langkah 3 (InputManager.md).</summary>
    [Serializable]
    public class InputBalance
    {
        public float rebindTimeoutSeconds = 5f;
    }

    /// <summary>Bagian streaming (#88). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class StreamingBalance { }

    /// <summary>Bagian damage (#59). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class DamageBalance { }

    /// <summary>Bagian equipment (#60). Diisi Langkah 3 (EquipmentManager.md).</summary>
    [Serializable]
    public class EquipmentBalance
    {
        public int hotbarSlotCount = 8;                     // Tombol 1 sampai 8 (#18)
        public float lowerSeconds = 0.15f;
        public float raiseSeconds = 0.2f;                   // Total jeda ganti 0,35 detik (disetujui developer)
        public bool offHandAllowedWhileCarrying = false;    // Final di #65
    }

    /// <summary>Bagian items (#66). Diisi Langkah 3 (ItemUseSystem.md).</summary>
    [Serializable]
    public class ItemsBalance
    {
        public float useMoveSpeedMultiplier = 0.5f;
        public float defaultRestoreSeconds = 20f;           // Pemulihan bertahap ala Raft (CATATAN_MEKANIK bagian 2)
        public float dropForwardDistance = 1f;
        public float worldItemDespawnGameHours = 48f;
        public int maxLooseWorldItems = 64;
        public float staleRatio = 0.5f;
        public float staleHungerMultiplier = 0.75f;
        public float rottenHungerMultiplier = 0.25f;
        public float rottenPoisonChance = 0.8f;
        public float dropPushForce = 1.5f;                  // Dorongan kecil saat membuang (ItemUseSystem.md langkah 4). Nilai usulan Langkah 3
    }

    /// <summary>Interaksi tombol E (#17). Diisi Langkah 2 dari Island_Logic/IslandInteractable.md.</summary>
    [Serializable]
    public class InteractBalance
    {
        public float range = 3f;                    // Jarak interaksi
        public float sphereRadius = 0.15f;          // Lebar bidikan
        public float scanIntervalSeconds = 0.05f;   // Pemindaian 20 kali per detik
        public float targetGraceSeconds = 0.15f;    // Target bertahan sebentar setelah bidikan lepas
        public float holdLoseRange = 3.5f;          // Interaksi tahan batal di luar jarak ini
        public float moveCancelSpeed = 6f;          // Interaksi tahan batal kalau pemain berlari
        // Warna InteractHighlight. Nama field usulan Langkah 2 (blueprint hanya menyebut "dibaca dari interact").
        public UnityEngine.Color availableColor = new UnityEngine.Color(1f, 0.95f, 0.6f);   // Kuning muda
        public UnityEngine.Color blockedColor = new UnityEngine.Color(0.9f, 0.15f, 0.15f);  // Merah
    }

    /// <summary>Bagian camera (#102). Diisi Langkah 3 (FirstPersonCamera.md).</summary>
    [Serializable]
    public class CameraBalance
    {
        public float maxLookAngle = 80f;
        public float baseFov = 70f;
        public float viewmodelFov = 55f;
        public float eyeHeightStand = 1.6f;
        public float eyeHeightSpeed = 6f;
        public float maxOffsetDegrees = 15f;
        public float recoilRecoverPerSec = 12f;
        public float swayAmount = 0.02f;
        public float swayMax = 0.06f;
        public float swaySmooth = 10f;
        public float bobFrequency = 9f;
        public float bobAmplitude = 0.025f;
        public float bobSprintMultiplier = 1.5f;
        public float bobHeavyMultiplier = 1.8f;
        public float fovSprintBonus = 8f;
        public float fovChangeSpeed = 40f;
        public float landingDipMax = 0.25f;
        public float ladderYawRange = 70f;
        public float lowerSpeed = 8f;
        public float heavyLookScale = 0.8f;                 // Sensitivitas 80 persen saat membawa Heavy Item (FirstPersonCamera.md)
        public float landingDipRecoverSpeed = 1.5f;         // Pemulihan pegas dip mendarat, m/s. Nilai usulan Langkah 3
    }

    /// <summary>Satu baris tabel permukaan (PlayerTraversal.md).</summary>
    [Serializable]
    public struct SurfaceProfile
    {
        public SurfaceType surface;
        public float speedMultiplier;
        public float fallDamageMultiplier;

        public SurfaceProfile(SurfaceType surface, float speed, float fall)
        {
            this.surface = surface;
            speedMultiplier = speed;
            fallDamageMultiplier = fall;
        }
    }

    /// <summary>Bagian traversal (#101, #94). Diisi Langkah 3 (PlayerTraversal.md, IslandPlayerController.md). Bagian #94 diisi Langkah 5.</summary>
    [Serializable]
    public class TraversalBalance
    {
        // IslandPlayerController.md bagian 5: walkSpeed dan sprintSpeed dipindah ke config (#84)
        public float walkSpeed = 5f;
        public float sprintSpeed = 8f;
        public float gravity = -9.81f;
        public float groundCheckDistance = 0.2f;            // Raycast tanah pendek. Nilai usulan Langkah 3
        public float standHeight = 1.8f;
        public float crouchHeight = 1.1f;
        public float heightChangeSpeed = 6f;
        public float eyeHeightCrouch = 0.95f;
        public float crouchSpeedMultiplier = 0.5f;
        public float ladderClimbSpeed = 2.5f;
        public float ropeClimbSpeed = 1.8f;
        public float climbExhaustedMultiplier = 0.4f;
        public float climbEnterSeconds = 0.25f;
        public float climbExitSeconds = 0.35f;
        public float climbJumpOffForce = 3f;
        public float fallSafeSpeed = 9f;                    // Sekitar jatuh 4 meter
        public float fallLethalSpeed = 20f;                 // Sekitar jatuh 20 meter
        public float slideMinAngle = 45f;                   // Sama dengan CharacterController.slopeLimit
        public float slideSpeed = 6f;
        public float slideSteerSpeed = 1.5f;
        public float surfaceCheckSeconds = 0.2f;
        public float jumpHeight = 1.1f;                     // Menggantikan nilai 2 (celah dinding base)
        public SurfaceProfile[] surfaceProfiles =
        {
            new SurfaceProfile(SurfaceType.Pasir, 0.9f, 0.9f),
            new SurfaceProfile(SurfaceType.Rumput, 1f, 1f),
            new SurfaceProfile(SurfaceType.Tanah, 1f, 1f),
            new SurfaceProfile(SurfaceType.Batu, 1f, 1f),
            new SurfaceProfile(SurfaceType.Salju, 0.8f, 0.75f),
            new SurfaceProfile(SurfaceType.Lumpur, 0.7f, 0.75f),
            new SurfaceProfile(SurfaceType.Kayu, 1f, 1f),
            new SurfaceProfile(SurfaceType.Logam, 1f, 1f),
        };
    }

    /// <summary>Bagian carry (#118). Diisi Langkah 3 (CarryCapacity.md).</summary>
    [Serializable]
    public class CarryBalance
    {
        public int baseSlots = 15;
        public int woodStack = 100;
        public int stoneStack = 100;
        public int materialStack = 20;
        public int foodStack = 5;
        public int ammoStack = 60;
        public float overflowSpacing = 0.6f;
    }

    /// <summary>Bagian console (#56). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class ConsoleBalance { }

    /// <summary>
    /// Fase 1 (#74, #75). Diisi Langkah 2 dari LifeboatExtraction.md, ShipTimerManager.md,
    /// ShipPlayerController.md, dan ShipInteractable.md. Selain durationSeconds (GDD bagian 3), semua usulan yang disetujui (#85).
    /// </summary>
    [Serializable]
    public class ShipBalance
    {
        // LifeboatExtraction.md
        public float durationSeconds = 60f;        // GDD bagian 3. Keputusan developer: batas paling lama
        public float preGameSeconds = 3f;          // Jeda sebelum kontrol dibuka, di luar 60 detik
        public float sirenAtSeconds = 1.5f;        // Sirene pada jeda pra-game
        public int handCapacity = 4;               // Batas berat di tangan (dibaca InventoryManager.ShipHandCapacity)
        public float lifeboatSafeRadius = 6f;      // Radius area aman
        public float lifeboatSafeHeight = 2.5f;    // Selisih tinggi maksimum
        public float departHoldSeconds = 1.5f;     // Tahan E untuk berangkat
        public float arrivalNpcSpacing = 2f;       // Jarak antar-NPC di pantai
        // ShipTimerManager.md
        public float panicStartSeconds = 15f;
        public float tickPitchMin = 1f;
        public float tickPitchMax = 2f;
        public float tickVolume = 0.6f;
        public float vignetteMaxAlpha = 0.55f;
        public float timerShakePixels = 6f;
        public float timerShakeFrequency = 25f;
        public float musicFadeInSeconds = 1f;
        public float musicFadeOutSeconds = 0.5f;
        // ShipPlayerController.md
        public float moveSpeed = 5f;
        public float gravity = -9.81f;
        public float groundedStickVelocity = -2f;
        // ShipInteractable.md
        public float pickupColliderPadding = 0.05f;
    }

    /// <summary>Bahaya kapal (#75). Diisi Langkah 2 dari ShipHazardManager.md. Semua usulan yang disetujui (#85).</summary>
    [Serializable]
    public class ShipHazardBalance
    {
        public float staticTiltDegrees = 4f;
        public float tiltSpeedInfluence = 0.12f;
        public float driftSpeedMax = 0.4f;
        public float swaySpeed = 1f;
        public float[] swayDegreesByStage = { 2f, 3.5f, 5f, 7f };
        public float waterRiseMaxPerSec = 0.5f;
        public float waterAnkleDepth = 0.2f;
        public float waterKneeDepth = 0.6f;
        public float waterWaistDepth = 1.2f;
        public float waterHeadDepth = 1.6f;
        public float waterAnkleMultiplier = 0.85f;
        public float waterKneeMultiplier = 0.6f;
        public float waterWaistMultiplier = 0.35f;
        public float collapseWarningSeconds = 3f;
        public int zombieMaxAlive = 8;
        public float zombieMoveSpeed = 3.5f;
        public float zombieAggroRadius = 12f;
        public float zombieMinSpawnDistance = 10f;
        public float zombieTickSeconds = 0.25f;
        public float shoveRadius = 1.2f;
        public float shoveForwardOffset = 1f;
        public float shoveKnockback = 3f;
        public float shoveStaggerSeconds = 1.5f;
        public float shoveCooldownSeconds = 1.5f;
    }

    /// <summary>Bagian infection (#64). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable]
    public class InfectionBalance
    {
        // Ditambahkan Langkah 2 karena IslandArrival (#74) memberi vaksin awal. Field lain #64 diisi Langkah 4.
        public int startingVaccineDoses = 2;
    }

    /// <summary>Bagian hydration (#43). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable]
    public class HydrationBalance
    {
        public float exertionThirstMultiplier = 1.25f;      // Ditambah Langkah 3: dipakai SurvivalStats.TickDrains (VitalsRules.md, #43)
    }

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
