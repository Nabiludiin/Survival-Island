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
    public partial class TimeBalance
    {
        public float timeMultiplier = 96f;      // 1 hari = 900 detik
        public float dayLengthSeconds = 900f;   // Hanya informasi
    }

    /// <summary>Status tubuh pemain (angka inti #84). Tambahan #111 diisi Langkah 3/4.</summary>
    [Serializable]
    public partial class VitalsBalance
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
    public partial class ResourceBalance
    {
        public int treeDropMin = 8;
        public int treeDropMax = 12;
        public float axeDecayPerUse = 1f;
        public float harvestNoiseRadius = 10f;   // Disetujui developer (#61)
        // TODO Langkah 4: tambahan #73 (rockDrop, respawn, dll.).
    }

    /// <summary>Region (angka inti #84). Tambahan #68 diisi Langkah 6.</summary>
    [Serializable]
    public partial class RegionBalance
    {
        public int bridgeWoodCost = 500;                 // GDD bagian 5: nilai awal 500, bisa disetel
        public float heavyItemSpeedMultiplier = 0.5f;    // Claude.md Tahap 4, Region 4
        public int helicopterPartCount = 5;              // Disetujui developer (#69)
        // TODO Langkah 6: tambahan #68 (bridgeDepositHoldSeconds, ravineMinWidth, gateVerifySeconds, ravineFallDamage).
    }

    /// <summary>Cuaca (angka inti #84). Tambahan #115 diisi Langkah 6.</summary>
    [Serializable]
    public partial class WeatherBalance
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
    public partial class RunBalance
    {
        public float endSequenceTimeoutSeconds = 15f;
        public float endingTimeoutSeconds = 120f;
        public float gameOverInputDelaySeconds = 1f;
        public float saveWaitTimeoutSeconds = 5f;
        public float sceneLoadMinShowSeconds = 0.5f;
    }

    /// <summary>UI (#83). Dipakai UIFocusStack dan UIManager.</summary>
    [Serializable]
    public partial class UIBalance
    {
        public float panelFadeSeconds = 0.15f;
        public float pausedMusicVolumeDb = -12f;
        public float pauseAudioFadeSeconds = 0.2f;
        public float loadingBarSpeed = 1.5f;             // Usulan UIManager.md (disetujui sebagai nilai awal)
        public float confirmInputDelaySeconds = 0.25f;   // Usulan UIManager.md (disetujui sebagai nilai awal)
    }

    /// <summary>Seed run (#89). Dipakai RunSeedManager.</summary>
    [Serializable]
    public partial class RunSeedBalance
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
    public partial class SaveBalance
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
    public partial class InputBalance
    {
        public float rebindTimeoutSeconds = 5f;
    }

    /// <summary>Bagian streaming (#88). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class StreamingBalance { }

    /// <summary>Bagian equipment (#60). Diisi Langkah 3 (EquipmentManager.md).</summary>
    [Serializable]
    public partial class EquipmentBalance
    {
        public int hotbarSlotCount = 8;                     // Tombol 1 sampai 8 (#18)
        public float lowerSeconds = 0.15f;
        public float raiseSeconds = 0.2f;                   // Total jeda ganti 0,35 detik (disetujui developer)
        public bool offHandAllowedWhileCarrying = false;    // Final di #65
        public float fistDamage = 5f;                       // Keputusan developer (sistem pendukung): pukulan tangan kosong
    }

    /// <summary>Bagian items (#66). Diisi Langkah 3 (ItemUseSystem.md).</summary>
    [Serializable]
    public partial class ItemsBalance
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
    public partial class InteractBalance
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
    public partial class CameraBalance
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
    public partial class TraversalBalance
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
    public partial class CarryBalance
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
    [Serializable] public partial class ConsoleBalance { }

    /// <summary>
    /// Fase 1 (#74, #75). Diisi Langkah 2 dari LifeboatExtraction.md, ShipTimerManager.md,
    /// ShipPlayerController.md, dan ShipInteractable.md. Selain durationSeconds (GDD bagian 3), semua usulan yang disetujui (#85).
    /// </summary>
    [Serializable]
    public partial class ShipBalance
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
    public partial class ShipHazardBalance
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
    public partial class InfectionBalance
    {
        // Ditambahkan Langkah 2 karena IslandArrival (#74) memberi vaksin awal. Field lain #64 diisi Langkah 4.
        public int startingVaccineDoses = 2;
    }

    /// <summary>Bagian hydration (#43). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable]
    public partial class HydrationBalance
    {
        public float exertionThirstMultiplier = 1.25f;      // Ditambah Langkah 3: dipakai SurvivalStats.TickDrains (VitalsRules.md, #43)
    }

    /// <summary>Bagian campfire (#44). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class CampfireBalance { }

    /// <summary>Bagian disease (#45). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class DiseaseBalance { }

    /// <summary>Bagian statusEffects (#20). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class StatusEffectsBalance { }

    /// <summary>Bagian crafting (#77). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class CraftingBalance { }

    /// <summary>Bagian repair (#113). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class RepairBalance { }

    /// <summary>Bagian upgrade (#113). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class UpgradeBalance { }

    /// <summary>Bagian fishing (#55). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class FishingBalance { }

    /// <summary>Bagian debris (#106). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class DebrisBalance { }

    /// <summary>Bagian surface (#86). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class SurfaceBalance { }

    /// <summary>Bagian build (#79). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class BuildBalance { }

    /// <summary>Bagian baseNav (#78). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class BaseNavBalance { }

    /// <summary>Bagian integrity (#47). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class IntegrityBalance { }

    /// <summary>Bagian storage (#46). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class StorageBalance { }

    /// <summary>Bagian sleep (#67). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class SleepBalance { }

    /// <summary>Bagian trap (#26). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class TrapBalance { }

    /// <summary>Bagian throwable (#96). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class ThrowableBalance { }

    /// <summary>Bagian lair (#112). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class LairBalance { }

    /// <summary>Bagian warmth (#65). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class WarmthBalance { }

    /// <summary>Bagian heavyItem (#69). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class HeavyItemBalance { }

    /// <summary>Bagian swimming (#13). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class SwimmingBalance { }

    /// <summary>Bagian tide (#91). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class TideBalance { }

    /// <summary>Bagian environment (#92). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class EnvironmentBalance { }

    /// <summary>Bagian disaster (#70). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class DisasterBalance { }

    /// <summary>Bagian beachSupply (#71). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class BeachSupplyBalance { }

    /// <summary>Bagian bloodMoon (#50). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class BloodMoonBalance { }

    /// <summary>Bagian luckyMachine (#72). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class LuckyMachineBalance { }

    /// <summary>Bagian npc (#76). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class NpcBalance { }

    /// <summary>Bagian objective (#81). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class ObjectiveBalance { }

    /// <summary>Bagian loot (#103). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class LootBalance { }

    /// <summary>Bagian remoteBase (#90). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class RemoteBaseBalance { }

    /// <summary>Bagian fire (#97). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class FireBalance { }

    /// <summary>Bagian light (#98). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class LightBalance { }

    /// <summary>Bagian flashlight (#98). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class FlashlightBalance { }

    /// <summary>Bagian rescue (#14). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class RescueBalance { }

    /// <summary>Bagian deathCinematic (#53). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class DeathCinematicBalance { }

    /// <summary>Bagian stats (#57). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class StatsBalance { }

    /// <summary>Bagian narrative (#99). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class NarrativeBalance { }

    /// <summary>Bagian safety (#116). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class SafetyBalance { }

    /// <summary>Bagian animation (#12). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class AnimationBalance { }

    /// <summary>Bagian vfx (#15). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class VfxBalance { }

    /// <summary>Bagian tutorial (#16). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class TutorialBalance { }

    /// <summary>Bagian settings (#19). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class SettingsBalance { }

    /// <summary>Bagian footstep (#21). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class FootstepBalance { }

    /// <summary>Bagian notification (#22). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class NotificationBalance { }

    /// <summary>Bagian postProcess (#23). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class PostProcessBalance { }

    /// <summary>Bagian shake (#24). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class ShakeBalance { }

    /// <summary>Bagian combatFeedback (#51). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class CombatFeedbackBalance { }

    /// <summary>Bagian ambientAudio (#52). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class AmbientAudioBalance { }

    /// <summary>Bagian combatMusic (#114). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class CombatMusicBalance { }

    /// <summary>Bagian lore (#100). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class LoreBalance { }

    /// <summary>Bagian barks (#100). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class BarksBalance { }

    /// <summary>Bagian localization (#104). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class LocalizationBalance { }

    /// <summary>Bagian optimization (#62, #93). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class OptimizationBalance { }

    /// <summary>Bagian compass (#30). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class CompassBalance { }

    /// <summary>Bagian hazard (#59). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class HazardBalance { }

    // =====================================================================================
    // Ditambahkan Langkah 4. Bagian yang dipakai Langkah 4 ditandai `partial` di atas (satu kata per baris),
    // lalu field-nya diisi di sini, supaya semua tambahan Langkah 4 berada di satu bagian di akhir file.
    // Nilai dari tabel "Angka per sistem" di GameBalanceConfig.md (#64, #73, #77, #79, #86, #67).
    // =====================================================================================

    /// <summary>Tambahan resource (#73, ResourceRespawn.md). treeDropMin/Max, axeDecayPerUse, harvestNoiseRadius ada di atas.</summary>
    public partial class ResourceBalance
    {
        public int rockDropMin = 4;
        public int rockDropMax = 6;
        public float pickaxeDecayPerUse = 1f;
        public float knifeDecayPerHarvest = 0.5f;
        public float knifeDecayPerCombatHit = 1f;       // Keputusan developer (sistem pendukung): tusukan Pisau yang kena musuh
        public float coconutRespawnDays = 3f;
        public float stoneRespawnDays = 2f;
        public float regrowJitterHours = 6f;
        public float regrowMinPlayerDistance = 25f;     // Usulan disetujui: node tidak tumbuh di depan pemain
        public float regrowClearRadius = 3f;            // Usulan disetujui: radius bebas bangunan
        public int regrowChecksPerFrame = 20;
        public int dropChunkSize = 4;                   // 10 kayu menjadi 3 WorldItem
        public float dropSettleSeconds = 3f;
        public float dropMaxFallMeters = 4f;
        public float dropImpulse = 1.5f;                // Dorongan acak item jatuh. Nilai usulan Langkah 4 ("Item jatuh" langkah 3)
        public RegionNodeMinimum[] minNodesPerRegion =
        {
            new RegionNodeMinimum { region = RegionType.Volcano, trees = 120, rocks = 40 },
            new RegionNodeMinimum { region = RegionType.Amazon, trees = 60, rocks = 20, mushrooms = 10, sapTrees = 8 },
            new RegionNodeMinimum { region = RegionType.Snow, trees = 20, rocks = 40 },
            new RegionNodeMinimum { region = RegionType.Logistic, trees = 30, rocks = 20 },
        };
    }

    /// <summary>Jumlah node minimum per region (tabel kepadatan ResourceRespawn.md). Dibaca ResourceAuditWindow.</summary>
    [Serializable]
    public struct RegionNodeMinimum
    {
        public RegionType region;
        public int trees, rocks, mushrooms, sapTrees;
    }

    /// <summary>Isi infection (#64, InfectionSystem.md). startingVaccineDoses ditulis Langkah 2 di atas.</summary>
    public partial class InfectionBalance
    {
        public float gainStandard = 5f;
        public float gainTribal = 6f;
        public float gainMoss = 8f;
        public float gainMilitary = 10f;
        public float gainFrozen = 5f;
        public float gainVillage = 6f;
        public float stage2At = 25f;
        public float stage3At = 50f;                    // Sama dengan vitals.regenMaxInfection
        public float stage4At = 75f;                    // Sama dengan vitals.infectionDamageThreshold
        public float[] stageMaxStamina = { 1f, 0.9f, 0.75f, 0.5f };
        public float[] stageStaminaRegen = { 1f, 1f, 0.75f, 0.5f };
        public float[] stageVision = { 0f, 0.15f, 0.4f, 0.8f };
        public float selfProgressPerGameHour = 0f;      // Usulan disetujui: infeksi hanya naik saat diserang
        public float selfProgressMinInfection = 50f;
        public float vaccineReduction = 50f;
        public float vaccineInjectSeconds = 4f;
        public float medicVaccineMultiplier = 1.25f;
        public float criticalWarningAt = 90f;
        public float zombifyCinematicSeconds = 4f;
    }

    /// <summary>Isi crafting (#77, CraftingStation.md).</summary>
    public partial class CraftingBalance
    {
        public int maxQueue = 5;
        public float stationRangeMeters = 4f;
        public float leaveGraceMeters = 2f;
        public float stationCheckSeconds = 0.5f;
        public float hitPauseSeconds = 1.5f;
        public float npcCrafterSpeedMultiplier = 1.5f;
    }

    /// <summary>Isi surface (#86, SurfaceDetection.md).</summary>
    public partial class SurfaceBalance
    {
        public float probeUp = 1f;
        public float probeDown = 3f;
        public float beachBuildSandWeight = 0.35f;      // Usulan disetujui (#85): 35 persen pasir pada salah satu titik tapak
        public int nearSandSampleCount = 8;
        public int runtimeBakeRowsPerFrame = 64;
        public int debugGridCells = 64;
        public float debugGridSpacing = 1f;
    }

    /// <summary>Isi build (#79, BuildMenu.md dan BuildManager.md).</summary>
    public partial class BuildBalance
    {
        public float gridSize = 2.5f;
        public float maxBuildDistance = 6f;
        public float levelHeight = 2.5f;
        public float heightSnap = 0.25f;
        public float maxFoundationHeightDelta = 1.25f;
        public float groundProbeHeight = 5f;
        public int maxLevels = 3;
        public int maxHorizontalSpan = 2;
        public int maxStructures = 400;
        public float overlapShrink = 0.1f;
        public int maxCollapsePerFrame = 4;
        public int maxCollapseVfxPerSecond = 8;
        public float demolishHoldSeconds = 1.2f;
        public float demolishRefundRatio = 0.5f;
        public float demolishBlockedAfterDamageSeconds = 5f;
        public float hammerDecayPerBuild = 1f;
        public float hammerDecayPerDemolish = 1f;
    }

    /// <summary>Bagian sleep (#67) yang dipakai DayNightDirector. Field lain diisi Langkah pemilik SleepSystem.</summary>
    public partial class SleepBalance
    {
        public float maxGameHoursPerStep = 0.25f;       // Batas maju jam dalam satu langkah DayNightDirector (SleepSystem.md)
    }

    // =====================================================================================
    // Ditambahkan Langkah 5 (damage, senjata, musuh, spawn, deteksi, pinned).
    // Kelas di bawah dulunya baris kosong "[Serializable] public class XBalance { }" di atas. Baris kosong itu dihapus
    // dan kelas lengkapnya dipindah ke sini, supaya semua perubahan Langkah 5 berada di satu bagian (C# tidak bisa
    // menambah field ke kelas yang sudah dideklarasikan tanpa partial). Nama field mengikuti tabel angka di blueprint.
    // =====================================================================================

    /// <summary>Jalur damage (DamagePipeline.md, HitboxModifier.md, #59). Headshot 2x: keputusan #85.</summary>
    [Serializable]
    public partial class DamageBalance
    {
        public float headMultiplier = 2f;
        public float torsoMultiplier = 1f;
        public float legMultiplier = 0.5f;
        public float explosionMinFalloff = 0.25f;
        public float playerExplosionSelfMultiplier = 0.5f;
        public float minDamageAfterArmor = 1f;
        public int maxExplosionTargets = 64;
    }

    /// <summary>Senjata api (GunController.md, #58). Angka per senjata ada di aset WeaponData.</summary>
    [Serializable]
    public partial class GunBalance
    {
        public float autoReloadDelay = 0.3f;
        public float dryClickCooldown = 0.25f;
        public float npcFireMaxAngle = 10f;
        public float movingSpeedThreshold = 0.5f;
    }

    /// <summary>Waktu dan batas serangan musuh (EnemyAttack.md, #109).</summary>
    [Serializable]
    public partial class EnemyAttackBalance
    {
        public int maxAttackersOnPlayer = 3;
        public int maxAttackersOnNpc = 2;
        public float waitRingRadius = 3.5f;
        public float tokenRetrySeconds = 0.3f;
        public float animEventGraceSeconds = 0.1f;
        public float interruptCooldownSeconds = 0.6f;
        public float telegraphMinInterval = 0.5f;
    }

    /// <summary>Reaksi kena, mayat, dan drop (EnemyHitReaction.md, #95).</summary>
    [Serializable]
    public partial class HitReactionBalance
    {
        public float staggerSeconds = 0.35f;
        public float staggerImmunitySeconds = 1f;
        public float headStaggerMultiplier = 1.5f;
        public float legSlowMultiplier = 0.6f;
        public float legSlowSeconds = 2f;
        public float armorNoFlinchBelow = 0.5f;
        public float meleeKnockback = 0.8f;
        public float explosionKnockback = 2.5f;
        public float knockbackSeconds = 0.25f;
        public float shoveRadius = 1.6f;
        public float shoveConeDot = 0.5f;
        public float shoveKnockback = 2.5f;
        public float shoveStaggerSeconds = 1f;
        public float shoveCooldownSeconds = 0.8f;
        public float shoveStaminaCost = 10f;
        public float corpseHoldSeconds = 6f;
        public float corpseFadeSeconds = 1.5f;
        public float ragdollSettleSeconds = 2f;
        public int maxCorpses = 12;
        public int maxRagdolls = 4;
        public float ragdollForce = 6f;
        public int maxDropsPerNight = 10;
    }

    /// <summary>Pemilihan target (ThreatSystem.md, #108).</summary>
    [Serializable]
    public partial class ThreatBalance
    {
        public float reselectSeconds = 1f;
        public float farReselectSeconds = 2.5f;
        public float farDistance = 30f;
        public float minTargetHoldSeconds = 3f;
        public float switchMargin = 15f;
        public float aggroOverrideSeconds = 4f;
        public float crowdPenalty = 8f;
        public int maxChasersPerNpc = 3;
        public float bloodMoonPlayerRange = 8f;
        public float loseSightSeconds = 8f;
        public float searchSeconds = 3f;
        public float huntOffsetRadius = 8f;
        public float huntUpdateSeconds = 5f;
        public float unreachableSeconds = 6f;
        public float unreachableBanSeconds = 10f;
        public int maxSelectionsPerFrame = 8;
        public int maxCandidates = 16;
    }

    /// <summary>Pandangan dan suara zombie (AISensory.md, #49). GDD bagian 3: Mengendap.</summary>
    [Serializable]
    public partial class StealthBalance
    {
        public bool enabled = true;                 // Saklar tes. false = semua zombie langsung Aware
        public float sightRange = 22f;
        public float sightAngleDeg = 120f;
        public float closeSenseRange = 2.5f;
        public float sightGainMin = 0.4f;
        public float sightGainMax = 2f;
        public float decayPerSec = 0.15f;
        public float suspiciousThreshold = 0.35f;
        public float loseAwareSeconds = 6f;
        public float crouchVisibility = 0.6f;
        public float sprintVisibility = 1.2f;
        public float hearGain = 0.5f;
        public float wallMuffle = 0.6f;
        public float sensorTickSeconds = 0.25f;
        public int sensorsPerFrame = 6;
        public float huntHintSeconds = 8f;
        public float huntHintRadius = 12f;
        public float noiseIdle = 0.5f;
        public float noiseCrouchMove = 2f;
        public float noiseWalk = 8f;
        public float noiseSprint = 20f;
        public float noiseSwim = 10f;
        public float noiseGunshot = 40f;
        public float noiseMelee = 8f;
        public float noiseLandingMax = 15f;
        public float noiseTickSeconds = 0.5f;
    }

    /// <summary>Hewan siang (WildlifeAI.md, #61). Angka per spesies ada di aset AnimalData.</summary>
    [Serializable]
    public partial class WildlifeBalance
    {
        public float tickSeconds = 0.25f;
        public float alertSeconds = 1.5f;
        public float crouchAggroMultiplier = 0.6f;
        public float noiseAggroMultiplier = 0.5f;
        public float targetSampleRadius = 2f;
        public float returnHealPerSec = 0.05f;
        public float reaggroCooldownSeconds = 5f;
        public float pathCheckSeconds = 1f;
        public float unreachableConfirmSeconds = 2f;
        public float unreachableGiveUpSeconds = 15f;
        public float fleeSpeedMultiplier = 1.2f;
        public float fleeTimeoutSeconds = 60f;
        public float skinHoldSeconds = 3f;
        public float skinNoiseRadius = 6f;
        public float carcassLifetimeSeconds = 180f;
        public float territorySpawnChance = 0.7f;   // Disetujui developer (WildlifeAI.md)
        public float wanderWaitMinSeconds = 2f;
        public float wanderWaitMaxSeconds = 6f;
        public float returnSpeedMultiplier = 1.5f;
        // Terkaman Jaguar
        public float pounceTriggerRadius = 9f;
        public float groundPounceDistance = 6f;
        public float pounceTelegraphSeconds = 0.8f;
        public float pounceSeconds = 0.6f;
        public float pounceHitRadius = 1.5f;
        public float pounceBreakDamage = 40f;
        public float pinDamage = 5f;
        public float pinDamageInterval = 0.5f;
        public float pounceMissStunSeconds = 1.5f;
        public float pounceCooldownSeconds = 12f;
        // Serudukan Babi
        public float chargeTriggerDistance = 12f;
        public float chargeTelegraphSeconds = 1f;
        public float chargeDistance = 12f;
        public float chargeSpeed = 10f;
        public float chargeStunSeconds = 2f;
        // Gagak
        public float crowCruiseHeight = 4f;
        public float crowDiveHeight = 1.2f;
        public float crowDiveSeconds = 0.5f;
        public int maxDiversAtOnce = 2;
    }

    /// <summary>Tempat dan jenis spawn (SpawnManager.md, #62).</summary>
    [Serializable]
    public partial class SpawnBalance
    {
        public float tickSeconds = 2f;
        public int maxSpawnsPerTick = 4;
        public int maxSpawnsPerFrame = 2;
        public int maxZombiesAlive = 30;
        public int maxZombiesOverride = 50;
        public int maxAnimalsAlive = 8;
        public float minDistance = 20f;
        public float maxDistance = 45f;
        public float sampleRadius = 6f;
        public int attemptsPerSpawn = 8;
        public float behindChance = 0.75f;
        public float behindHalfAngleDeg = 70f;
        public float despawnDistance = 70f;
        public float stuckCheckSeconds = 5f;
        public float stuckMoveEpsilon = 0.5f;
        public float stuckSeconds = 20f;
        public float stuckForceSeconds = 45f;
        public float cleanupSeconds = 30f;
        public int dayAnimalsAlive = 4;
        // Catatan Langkah 5: jam siang dan malam dari DayNightDirector.md (18:00 dan 06:00). Dipakai DayPhaseTracker
        // selama NightfallEvent (Langkah 4) belum digabung. Hapus setelah DayNightDirector tersedia.
        public int nightStartHour = 18;
        public int dayStartHour = 6;
    }

    /// <summary>Terbakar matahari (SunlightExposure.md, #63). GDD bagian 3.</summary>
    [Serializable]
    public partial class SunlightBalance
    {
        public float checkIntervalSeconds = 2f;
        public float rayLength = 200f;
        public float minSunElevationDeg = 35f;
        public float burnStaggerMaxSeconds = 4f;
        public float burnDurationSeconds = 3f;
        public float maxShelteredSeconds = 60f;
        public int seekHidingHour = 5;
        public float spotSearchRadius = 120f;
        public float submergeSeconds = 2f;
        public float emergeSeconds = 2f;
        public float emergeStaggerSeconds = 4f;
        public float emergeBoostMultiplier = 1.2f;
        public float emergeBoostSeconds = 8f;
        public int daySubmergedCount = 12;
    }

    /// <summary>Perilaku varian zombie (SpecialZombieAI.md, #25). Statistik varian ada di aset ZombieData.</summary>
    [Serializable]
    public partial class SpecialZombieBalance
    {
        public float tribalRangeMin = 5f;
        public float tribalRangeMax = 14f;
        public float tribalWindupSeconds = 1.2f;
        public float tribalProjectileSpeed = 18f;
        public float tribalCooldownSeconds = 10f;
        public float tribalPullSpeed = 2f;
        public float tribalDamagePerSec = 5f;
        public float tribalReleaseDamage = 40f;     // Keputusan developer (USULAN_TERBUKA.md)
        public float tribalBreakDistance = 18f;
        public float mossWakeDistance = 4f;
        public float mossAlertDistance = 8f;
        public float mossLightDistance = 12f;
        public float mossRagePerSec = 0.4f;
        public float mossRageDecayPerSec = 0.2f;
        public float mossTellMinSeconds = 6f;
        public float mossTellMaxSeconds = 10f;
        public float militarySlowMultiplier = 0.5f;
        public float militarySlowFullDistance = 4f;
        public float militarySlowEndDistance = 8f;
        public float militarySlowSeconds = 1f;
        public float frozenBurstRadius = 4f;
        public float frozenBurstDamage = 20f;
        public float frozenBurstDelaySeconds = 0.3f;
        public float frozenBurstWarmthLoss = 25f;
        public float villagePounceMin = 3f;
        public float villagePounceMax = 10f;
        public float villagePounceWindupSeconds = 0.5f;
        public float villagePounceSpeed = 16f;
        public float villagePounceRecoverySeconds = 1.5f;
        public float villagePounceCooldownSeconds = 6f;
        public float largeStuckRetargetSeconds = 2f;
        public float largeStuckUnseenRetargetSeconds = 5f;
        public float largeStuckReturnSeconds = 10f;
        public float largeStuckUnseenReturnSeconds = 15f;
        public float largeUnseenReturnSeconds = 60f;
    }

    /// <summary>Tempo malam dan kurva kesulitan (AIDirector.md, #27). GDD bagian 3: zombie makin kuat setelah hari 60.</summary>
    [Serializable]
    public partial class AIDirectorBalance
    {
        public ThreatKey[] threatCurve =
        {
            new ThreatKey { day = 1,  mobSize = 6,  mobIntervalMin = 100f, mobIntervalMax = 140f, maxAlive = 12, nightBudget = 24,  specialShare = 0.10f, maxLarge = 1, hpMultiplier = 1.00f, speedMultiplier = 1.00f, damageMultiplier = 1.00f, recoveryMultiplier = 1.00f },
            new ThreatKey { day = 10, mobSize = 9,  mobIntervalMin = 90f,  mobIntervalMax = 125f, maxAlive = 15, nightBudget = 40,  specialShare = 0.15f, maxLarge = 1, hpMultiplier = 1.05f, speedMultiplier = 1.00f, damageMultiplier = 1.00f, recoveryMultiplier = 0.97f },
            new ThreatKey { day = 20, mobSize = 13, mobIntervalMin = 80f,  mobIntervalMax = 110f, maxAlive = 18, nightBudget = 60,  specialShare = 0.20f, maxLarge = 2, hpMultiplier = 1.10f, speedMultiplier = 1.03f, damageMultiplier = 1.05f, recoveryMultiplier = 0.94f },
            new ThreatKey { day = 30, mobSize = 17, mobIntervalMin = 70f,  mobIntervalMax = 100f, maxAlive = 22, nightBudget = 85,  specialShare = 0.30f, maxLarge = 2, hpMultiplier = 1.20f, speedMultiplier = 1.06f, damageMultiplier = 1.10f, recoveryMultiplier = 0.90f },
            new ThreatKey { day = 40, mobSize = 21, mobIntervalMin = 60f,  mobIntervalMax = 90f,  maxAlive = 25, nightBudget = 110, specialShare = 0.35f, maxLarge = 3, hpMultiplier = 1.30f, speedMultiplier = 1.09f, damageMultiplier = 1.15f, recoveryMultiplier = 0.85f },
            new ThreatKey { day = 50, mobSize = 26, mobIntervalMin = 55f,  mobIntervalMax = 80f,  maxAlive = 28, nightBudget = 140, specialShare = 0.40f, maxLarge = 3, hpMultiplier = 1.40f, speedMultiplier = 1.12f, damageMultiplier = 1.20f, recoveryMultiplier = 0.80f },
            new ThreatKey { day = 60, mobSize = 30, mobIntervalMin = 45f,  mobIntervalMax = 70f,  maxAlive = 30, nightBudget = 180, specialShare = 0.50f, maxLarge = 4, hpMultiplier = 1.50f, speedMultiplier = 1.15f, damageMultiplier = 1.25f, recoveryMultiplier = 0.75f },
        };
        public float regionMultiplierVolcano = 0.8f;
        public float regionMultiplierAmazon = 1f;
        public float regionMultiplierSnow = 1f;
        public float regionMultiplierLogistic = 1.2f;  // GDD bagian 5: "secara masif"
        public float firstWaveDelaySeconds = 20f;
        public float minBuildUpSeconds = 15f;
        public float peakThreshold = 0.9f;
        public float sustainMinSeconds = 3f;
        public float sustainMaxSeconds = 5f;
        public float fadeMaxSeconds = 30f;
        public float relaxMinSeconds = 30f;
        public float relaxMaxSeconds = 45f;
        public float relaxBreakDistance = 60f;
        public float intensityDecayPerSec = 0.033f;
        public float intensityPerPlayerDamage = 0.01f;
        public float intensityPerPin = 0.3f;
        public float intensityPerNearKill = 0.2f;
        public float nearKillMin = 3f;
        public float nearKillMax = 10f;
        public int minWaveSize = 3;
        public float waveRetrySeconds = 5f;
        public float heldGrowthSeconds = 60f;
        public float heldGrowthMax = 0.5f;
        public int holdMobLargeCount = 3;
        public float maxZombieSpeed = 7.8f;
        // Keputusan developer: pengali setelah hari ke-60
        public float postTargetCountPerDay = 0.02f;
        public float postTargetCountMax = 1.5f;
        public float postTargetHpPerDay = 0.02f;
        public float postTargetHpMax = 2f;
        public float postTargetDamagePerDay = 0.01f;
        public float postTargetDamageMax = 1.5f;
    }

    /// <summary>Pembagian tick AI (HordeCrowdManager.md, #93).</summary>
    [Serializable]
    public partial class CrowdBalance
    {
        public int capacity = 64;
        public int maxTicksPerFrame = 12;
        public float maxTickMilliseconds = 2f;
        public float intervalCloseVisible = 0.1f;
        public float intervalCloseHidden = 0.2f;
        public float intervalMidVisible = 0.25f;
        public float intervalMidHidden = 0.5f;
        public float intervalFar = 1f;
        public float closeDistance = 15f;
        public float midDistance = 30f;
        public float tierUpdateSeconds = 0.25f;
        public float repathMinSeconds = 0.5f;
        public float repathMinDistance = 1f;
        public float repathMaxDistance = 6f;
        public float repathDistanceFactor = 0.1f;
        public int maxDestinationsPerFrame = 4;
        public int animStaggerMid = 2;
        public int animStaggerFar = 4;
        public int goodAvoidanceCount = 8;
        public int overlapBufferSize = 16;
        public int pathfindingIterationsPerFrame = 150;
        // Catatan Langkah 5: angka pemeriksaan terhalang sementara, menyalin nilai baseNav di BaseNavigation.md (#78).
        // EnemyAI memakai jalur sementara ini sampai BaseNavigation (Langkah 4) digabung. Setelah itu baca baseNav.
        public float fallbackSpawnGraceSeconds = 2f;
        public float fallbackPathCheckIntervalSeconds = 1f;
        public float fallbackBlockedConfirmSeconds = 1.5f;
        public float fallbackBreachSearchRadius = 4f;
    }

    /// <summary>Pemain diterkam (PlayerPinnedState.md, #110). GDD bagian 3: lepas sendiri tanpa bantuan NPC.</summary>
    [Serializable]
    public partial class PinnedBalance
    {
        public bool enabled = true;
        public float mashProgressPerPress = 0.1f;
        public float mashDecayPerSec = 0.05f;
        public float mashMinPressInterval = 0.08f;
        public float holdProgressPerSec = 0.35f;
        public float struggleStaminaPerPress = 3f;
        public float struggleStaminaHoldPerSec = 10f;
        public float staminaEmptyMultiplier = 0.5f;
        public float hpEmptyMultiplier = 0.75f;
        public float infectionFullMultiplier = 0.75f;
        public float minEscapeMultiplier = 0.35f;
        public float knifeProgress = 0.6f;
        public float knifeDamage = 25f;
        public float knifeCooldownSeconds = 1.5f;
        public float knifeDurabilityCost = 5f;
        public float maxPinSeconds = 8f;
        public int maxOtherAttackers = 1;
        public float otherAttackerDamageMultiplier = 0.5f;
        public float postEscapeGraceSeconds = 1.5f;
        public float repinImmuneSeconds = 4f;
        public float releasePushDistance = 1f;
        public bool holdToStruggle = false;         // Pilihan aksesibilitas #19. Dibaca dari sini sampai SettingsManager ada
    }

    // =====================================================================================
    // Ditambahkan Langkah 6 (region, cuaca, suasana, kehangatan, barang berat).
    // Lima deklarasi kelas di atas (RegionBalance, WeatherBalance, WarmthBalance, HeavyItemBalance, EnvironmentBalance)
    // hanya ditambah kata partial, supaya field-nya bisa diisi di bagian ini. Nilai awal dari blueprint pemilik.
    // =====================================================================================

    /// <summary>Tambahan #68 (RegionGateSystem.md).</summary>
    public partial class RegionBalance
    {
        public float bridgeDepositHoldSeconds = 1f;     // Lama tahan E di BridgeSite (IslandInteractable.md)
        public float ravineMinWidth = 30f;              // Diperiksa alat Editor. Lompatan lari terjauh sekitar 7,5 m
        public float gateVerifySeconds = 2f;            // Jeda VerifyPlayerRegion
        public float ravineFallDamage = 20f;            // Damage Fall saat dikembalikan dari jurang
    }

    /// <summary>Tambahan #115 (RegionalWeather.md) dan WeatherManager.md.</summary>
    public partial class WeatherBalance
    {
        // Satu baris per WeatherType, urutan Clear, Rain, Fog, Storm, Snow, Blizzard (usulan disetujui developer).
        public WeatherEffects[] effects =
        {
            new WeatherEffects(WeatherType.Clear,    1f,    1f,    1f,    0f),
            new WeatherEffects(WeatherType.Rain,     1f,    0.9f,  0.85f, 0.3f),
            new WeatherEffects(WeatherType.Fog,      1f,    0.6f,  1f,    0f),
            new WeatherEffects(WeatherType.Storm,    0.9f,  0.7f,  0.6f,  0.5f),
            new WeatherEffects(WeatherType.Snow,     0.95f, 0.85f, 0.9f,  0.5f),
            new WeatherEffects(WeatherType.Blizzard, 0.75f, 0.45f, 0.6f,  2f),
        };
        public int minSevereDay = 2;                    // Hari pertama tanpa Storm dan Blizzard
        public float presentBlendSeconds = 6f;          // Transisi partikel dan audio
        public float audioVolume = 0.8f;                // WeatherManager.md (usulan disetujui developer)
        public float insideVolumeMultiplier = 0.3f;     // Hujan tetap terdengar dari dalam base
        public float insideBlendSeconds = 1.5f;         // Sama dengan environment.interiorBlendSeconds
    }

    /// <summary>Kehangatan (#65, WarmthSystem.md). Per detik kecuali disebut lain.</summary>
    public partial class WarmthBalance
    {
        public float tickSeconds = 0.5f;
        public float freezeEnterThreshold = 20f;
        public float freezeExitThreshold = 30f;
        public float ambientHeat = 5f;
        public float regionSnowCold = 20f;              // GDD bagian 5, Region 3: sekitar 4 detik sampai membeku
        public float blizzardCold = 2f;
        public float rainCold = 8f;
        public float rainFloor = 40f;
        public float stormCold = 12f;
        public float stormFloor = 30f;
        public float waterCold = 15f;
        public float waterFloor = 30f;
        public float snowWaterCold = 30f;
        public float clothesHeat = 20f;
        public float torchHeat = 20f;
        public float protectedFloor = 30f;              // Usulan 8, disetujui developer
        public float campfireHeat = 40f;
        public float indoorHeat = 25f;
        public float shelterCheckSeconds = 2f;
        public float shelterProbeDistance = 6f;
        public float torchBurnSeconds = 300f;           // Sekitar 8 jam game
        public float torchRainExtinguishSeconds = 20f;
    }

    /// <summary>Barang berat (#69, HeavyItemSystem.md).</summary>
    public partial class HeavyItemBalance
    {
        public float pickupHoldSeconds = 0.5f;
        public float dropDamageThreshold = 25f;         // Zombie biasa 15 tidak menjatuhkan, serangan berat 30 menjatuhkan
        public float dropForwardDistance = 1.2f;
        public float validSampleSeconds = 1f;
        public float navSampleRadius = 2f;
        public float recoveryCheckSeconds = 5f;
        public float discoverRadius = 40f;
    }

    /// <summary>Suasana region (#92, EnvironmentProfile.md).</summary>
    public partial class EnvironmentBalance
    {
        // Satu baris per WeatherType, urutan Clear, Rain, Fog, Storm, Snow, Blizzard (usulan disetujui developer).
        public WeatherEnvironmentLayer[] weatherLayers =
        {
            new WeatherEnvironmentLayer(WeatherType.Clear,    0f,     1f,    1f,    new UnityEngine.Color32(0xFF, 0xFF, 0xFF, 0xFF), 0f),
            new WeatherEnvironmentLayer(WeatherType.Rain,     0.006f, 0.7f,  0.8f,  new UnityEngine.Color32(0x6B, 0x77, 0x80, 0xFF), 0.4f),
            new WeatherEnvironmentLayer(WeatherType.Fog,      0.03f,  0.75f, 0.85f, new UnityEngine.Color32(0x9A, 0xA3, 0xA8, 0xFF), 0.6f),
            new WeatherEnvironmentLayer(WeatherType.Storm,    0.012f, 0.35f, 0.6f,  new UnityEngine.Color32(0x3C, 0x43, 0x48, 0xFF), 0.6f),
            new WeatherEnvironmentLayer(WeatherType.Snow,     0.01f,  0.8f,  0.95f, new UnityEngine.Color32(0xD9, 0xE4, 0xEC, 0xFF), 0.5f),
            new WeatherEnvironmentLayer(WeatherType.Blizzard, 0.07f,  0.3f,  0.65f, new UnityEngine.Color32(0xE4, 0xEC, 0xF2, 0xFF), 0.9f),
        };

        // Lapisan event (tabel "Lapisan event"). Blood Moon: langit merah GDD bagian 3, warna dan transisi usulan #50.
        public EnvironmentEventLayer[] eventLayers =
        {
            new EnvironmentEventLayer(EnvEventId.BloodMoon, 30, new UnityEngine.Color32(0x6B, 0x0F, 0x12, 0xFF), 0.01f, 0.8f,
                new UnityEngine.Color32(0xFF, 0x50, 0x40, 0xFF), 20f, 10f),
            new EnvironmentEventLayer(EnvEventId.Eruption, 20, new UnityEngine.Color32(0xA5, 0x39, 0x1F, 0xFF), 0.008f, 0.6f,
                new UnityEngine.Color32(0xFF, 0x9A, 0x50, 0xFF), -1f, -1f),   // -1 = pakai eventBlendSeconds
        };

        public float regionBlendSeconds = 8f;
        public float weatherBlendSeconds = 12f;
        public float eventBlendSeconds = 4f;
        public float interiorBlendSeconds = 1.5f;
        public float applyIntervalSeconds = 0.1f;
        public float maxFogDensity = 0.1f;              // Sekitar 20 m
        public float defaultMinAmbientLuminance = 0.035f;
        public float eruptionRemoteWeight = 0.35f;      // Bobot langit merah di luar Region 1
    }

    // =====================================================================================
    // Ditambahkan Langkah 7 (bencana, Lucky Machine, helikopter, sinematik mati).
    // Deklarasi kelas kosong di atas hanya ditambah kata partial (cara yang sama dengan Langkah 4).
    // Angka mengikuti tabel di DisasterManager.md, LuckyMachine.md, RescueEventManager.md, dan DeathCinematic.md.
    // =====================================================================================

    /// <summary>Bencana (#70). Nilai awal dari DisasterManager.md bagian 1 "Nilai config bencana".</summary>
    public partial class DisasterBalance
    {
        public int scheduleBlockDays = 60;
        // Salinan sementara interval Blood Moon (#50). Pemilik resmi: bloodMoon (HordeManager, belum ditulis).
        // Saat BloodMoonBalance diisi, DisasterScheduler membaca dari sana dan field ini dihapus.
        public int bloodMoonIntervalDays = 10;
        public float fireballIntervalSeconds = 4f;
        public float fireballIntervalSecondsScale2 = 2.5f;
        public float fireballRingMin = 8f;
        public float fireballRingMax = 35f;
        public float fireballMarkerSeconds = 2.5f;
        public float fireballRadius = 6f;
        public float fireballDamageCenter = 35f;
        public float fireballDamageEdge = 10f;
        public float fireballStructureDamage = 60f;
        public float fireballTreeChance = 0.25f;
        public float fireballDropHeight = 60f;
        public int eruptionOffscreenHits = 3;
        public float quakeDamageRatio = 0.25f;
        public float quakeDamageRatioScale2 = 0.5f;
        public int quakePulses = 3;
        public float quakeRemoteDamagePerHit = 75f;
        public int quakeRemoteHits = 4;
        public int maxStructuresPerFrame = 16;
        public float stormPulseSeconds = 30f;
        public float stormStructureDamage = 30f;
        public float treeFallSeconds = 45f;
        public float treeFallRingMin = 12f;
        public float treeFallRingMax = 30f;
        public float treeFallRadius = 3f;
        public float treeFallDamage = 25f;
        public float treeFallStructureDamage = 60f;
        public float treeFallMarkerSeconds = 2f;
    }

    /// <summary>Lucky Machine (#72). Angka sama untuk keempat mesin (keputusan developer, LuckyMachine.md).</summary>
    public partial class LuckyMachineBalance
    {
        public float freeWindowSeconds = 30f;     // GDD bagian 7
        public int maxFreeSpinsPerWindow = 3;
        public int maxStoredSpins = 1;
        public float spinSeconds = 4f;
        public float vacuumSeconds = 1.2f;
        public float tierWeightS = 5f;
        public float tierWeightA = 25f;
        public float tierWeightB = 70f;
        public float gateReopenSeconds = 2f;
        public float cageRewardOffset = 2f;
        public float vacuumEndScale = 0.2f;
    }

    /// <summary>Helikopter (#14). Nilai awal dari RescueEventManager.md bagian 1.</summary>
    public partial class RescueBalance
    {
        public float engineWarmupSeconds = 120f;
        public float engineNoiseRadius = 60f;
        public float engineNoiseIntervalSeconds = 5f;
        public int warmupPanicWaveSize = 15;
        public int targetNightWaveSize = 25;
        public float abortDistance = 40f;
        public float partInstallRadius = 4f;      // Radius ReconcileLyingParts, sama dengan ukuran PartInstallZone
    }

    /// <summary>Sinematik mati (#53). Nilai awal dari DeathCinematic.md bagian 1.</summary>
    public partial class DeathCinematicBalance
    {
        public float ragdollForce = 300f;
        public float explosionForceMultiplier = 3f;
        public float sinkDrag = 4f;
        public float slowMoScale = 0.35f;
        public float slowMoSeconds = 1.2f;
        public float cameraRiseSeconds = 1.5f;
        public float cameraHeight = 2.5f;
        public float cameraDistance = 3f;
        public float orbitDegreesPerSecond = 12f;
        public float holdSeconds = 3.5f;
        public float collapseHoldSeconds = 5f;
        public float fadeSeconds = 1.5f;
        public float messageSeconds = 2f;
        public float skipAfterSeconds = 2f;
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (FondasiA). Field bagian config dari tabel blueprint terkait.
    // =====================================================================================

    // =====================================================================================
    // Field config grup FondasiA (sistem pendukung): settings (#19), localization (#104), shake (#24),
    // vfx (#15), notification (#22), postProcess (#23). Kelas bagian sudah dideklarasikan partial di
    // GameBalanceSections.cs dan sudah punya field di GameBalanceConfig.cs.
    // Nilai awal mengikuti tabel "Angka penyetelan" di blueprint masing-masing dan tabel
    // "Bagian config per sistem" di GameBalanceConfig.md. Nilai yang berlaku dibaca dari aset.
    // =====================================================================================

    /// <summary>Batas menu pengaturan. Blueprint: Core_Logic/SettingsManager.md (#19).</summary>
    public partial class SettingsBalance
    {
        public float sensitivityMin = 0.02f;
        public float sensitivityMax = 0.5f;
        public float fovMin = 60f;
        public float fovMax = 100f;
        // Usulan disetujui developer (#19): batas sempit, supaya gelapnya Region 2 (GDD bagian 5) tetap terasa.
        public float brightnessMin = 0.8f;
        public float brightnessMax = 1.2f;
    }

    /// <summary>Teks, subtitle, dan aksesibilitas. Blueprint: Core_Logic/LocalizationManager.md (#104).</summary>
    public partial class LocalizationBalance
    {
        public float textScaleMin = 0.85f;
        public float textScaleMax = 1.5f;
        public float subtitleMinSeconds = 1.5f;
        public float subtitleSecondsPerChar = 0.06f;
        public int subtitleMaxRows = 3;
        public int maxFlashesPerSecond = 3;
    }

    /// <summary>Konversi sinyal Impulse ke offset kamera. Blueprint: Core_Logic/CameraShake.md (#24).</summary>
    public partial class ShakeBalance
    {
        public float degreesPerUnit = 2f;
        public float metersPerUnit = 0.02f;
        public float rollPerUnit = 1f;
        public float maxDegrees = 8f;          // Lebih kecil dari camera.maxOffsetDegrees (15)
        public float maxMeters = 0.12f;
        public float hitReferenceDamage = 25f; // Damage yang menghasilkan skala 1
        public float hitScaleMin = 0.5f;
        public float hitScaleMax = 1.5f;
    }

    /// <summary>Katalog efek sekali muncul. Blueprint: Core_Logic/VFXManager.md (#15).</summary>
    public partial class VfxBalance
    {
        // Tabel "Katalog efek" di VFXManager.md: poolTag, umur, maxActive, cullDistance, minInterval.
        // Isi pool di ObjectPooler.pools tidak boleh lebih kecil dari maxActive.
        public VFXEntry[] catalog =
        {
            new VFXEntry("VFX_Muzzle", 0.5f, 8, 0f, 0f),
            new VFXEntry("VFX_LaserMuzzle", 0.5f, 8, 0f, 0f),
            new VFXEntry("VFX_Impact", 0.5f, 24, 60f, 0f),
            new VFXEntry("VFX_ImpactDust", 0.5f, 24, 60f, 0f),
            new VFXEntry("VFX_ImpactWood", 0.5f, 16, 60f, 0f),
            new VFXEntry("VFX_LaserImpact", 0.5f, 8, 60f, 0f),
            new VFXEntry("VFX_Blood", 0.8f, 32, 40f, 0.02f),
            new VFXEntry("VFX_BloodHeavy", 1.2f, 12, 40f, 0f),
            new VFXEntry("VFX_MetalSpark", 0.4f, 12, 40f, 0f),
            new VFXEntry("VFX_WoodDust", 0.8f, 8, 40f, 0.05f),
            new VFXEntry("VFX_StoneDust", 0.8f, 8, 40f, 0.05f),
            new VFXEntry("VFX_LeafBurst", 0.8f, 8, 40f, 0.05f),
            new VFXEntry("VFX_GachaSpark", 2f, 4, 0f, 0f),
            new VFXEntry("VFX_Splash", 1f, 8, 40f, 0.1f),
            new VFXEntry("VFX_Explosion", 2f, 4, 0f, 0f),     // Usulan EnvironmentalHazard.md
            new VFXEntry("VFX_ShipPickup", 0.5f, 4, 0f, 0f),  // Usulan ShipInteractable.md
        };

        public float lowQualityDensity = 0.5f;
        public float mediumQualityDensity = 0.75f;
    }

    /// <summary>Notifikasi pojok layar. Blueprint: Core_Logic/NotificationManager.md (#22).</summary>
    public partial class NotificationBalance
    {
        public int maxVisible = 4;
        public int queueCapacity = 12;
        public float infoSeconds = 3f;
        public float warningSeconds = 5f;
        public float fadeSeconds = 0.3f;
        public float mergeWindowSeconds = 2f;
        public int logCapacity = 100;
    }

    /// <summary>Efek layar URP. Blueprint: Core_Logic/PostProcessController.md (#23).</summary>
    public partial class PostProcessBalance
    {
        public float lowHpStartRatio = 0.4f;
        public float lowHpMaxVignette = 0.5f;
        public float lowHpPulseHz = 1.2f;
        public float lowHpPulseAmount = 0.08f;
        public float woundVignette = 0.25f;
        public float hitPulseAmount = 0.2f;
        public float hitPulseDecayPerSec = 1f;
        public float freezeVignette = 0.4f;
        public float breathVignette = 0.55f;
        public float breathStartRatio = 0.4f;
        public float sleepVignette = 0.6f;
        public float infectionSaturationMin = -35f;
        public float underwaterBlurStart = 0f;
        public float underwaterBlurEnd = 8f;
        public float changePerSec = 1.5f;
        public float underwaterChangePerSec = 6f;
        public float deathDesaturateSeconds = 2f;
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (FondasiB). Field bagian config dari tabel blueprint terkait.
    // =====================================================================================

    // =====================================================================================
    // Angka penyetelan grup FondasiB. Nilai awal sesuai tabel "Angka penyetelan" tiap blueprint
    // dan tabel "Bagian config per sistem" di Core_Logic/GameBalanceConfig.md. Semua angka adalah usulan
    // yang disetujui developer sebagai nilai awal (#85, #113, #20, #103).
    // =====================================================================================

    /// <summary>Loot per lokasi (RegionLootTable.md, #103). Peluang titik House kosong ada di runSeed.lootPointEmptyChance.</summary>
    public partial class LootBalance
    {
        public float warehouseEmptyChance = 0.25f;   // Titik Warehouse kosong
        public int chestRollsMin = 1;                // Tumpukan per peti
        public int chestRollsMax = 3;
        public int bodyRollsMin = 1;                 // Tumpukan per mayat tentara
        public int bodyRollsMax = 2;
        public float amountMulVolcano = 1f;          // Pengali jumlah Region 1
        public float amountMulAmazon = 1.1f;         // Region 2
        public float amountMulLogistic = 1.25f;      // Region 4
        public float amountMulSnow = 1.4f;           // Region 3, area terjauh
        public float rareWeightPerDay = 0.02f;       // Tambahan bobot item langka per hari
        public float rareWeightMaxMul = 2f;          // Batas pengali bobot item langka
        public float amountBonusPer10Days = 0.1f;    // Tambahan jumlah tiap 10 hari
    }

    /// <summary>Efek status (StatusEffectManager.md, #20).</summary>
    public partial class StatusEffectsBalance
    {
        public int maxActive = 8;                        // Batas efek aktif
        public float supplementSeconds = 60f;            // Durasi Buff_Supplement
        public float supplementSpeedMultiplier = 1.2f;   // Kecepatan Buff_Supplement
        public float expiringWarningSeconds = 5f;        // Ikon berkedip menjelang habis
    }

    /// <summary>Durabilitas dan perbaikan (ToolRepairSystem.md, DurabilitySystem.md, #113).</summary>
    public partial class RepairBalance
    {
        public float lowRatio = 0.2f;            // Batas status Low
        public float maxLossPerRepair = 0.1f;    // Penurunan maksimum per perbaikan
        public int maxRepairs = 6;               // Batas perbaikan item non-inti
        public float repairSeconds = 3f;         // Lama memperbaiki
        public float upgradeSeconds = 6f;        // Lama memasang upgrade
    }

    /// <summary>Upgrade kecil per item (ToolRepairSystem.md, #113).</summary>
    public partial class UpgradeBalance
    {
        public float damageBonus = 0.1f;
        public int magazineBonus = 2;
        public float speedBonus = 0.1f;
        public float durabilityBonus = 0.25f;
        public int maxPerItem = 2;
        public float maxDamageMultiplier = 4f;   // Batas akhir pengali damage
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupC1). Field bagian config dari tabel blueprint terkait.
    // =====================================================================================

    // =====================================================================================
    // Field config grup C1 (sistem pendukung audio, animasi, dan optimasi): ambientAudio (#52), combatMusic (#114),
    // footstep (#21), animation (#12), optimization (#62, #93). Kelas bagian sudah dideklarasikan partial di
    // GameBalanceSections.cs dan sudah punya field di GameBalanceConfig.cs.
    // Nilai awal mengikuti tabel "Angka penyetelan" di blueprint masing-masing. Field yang ditandai
    // "tambahan implementasi" adalah angka yang ditulis langsung di teks blueprint (bukan di tabel), dipindah ke config
    // supaya tidak tertulis di logika (#84). Lihat "Catatan Implementasi (Sistem pendukung)" di blueprint terkait.
    // =====================================================================================

    /// <summary>Suara latar per area. Blueprint: Core_Logic/AmbientAudio.md (#52).</summary>
    public partial class AmbientAudioBalance
    {
        public float crossfadeSeconds = 3f;
        public float zoneExitGraceSeconds = 1.5f;
        public float retargetDuckSeconds = 0.2f;
        public float dayNightBlendSeconds = 20f;
        public float underwaterLowpassHz = 800f;
        public float sleepDuckDb = -6f;
        public float runEndFadeSeconds = 3f;

        // Tambahan implementasi (angka dari teks AmbientAudio.md bagian 2 dan 4).
        public float startCrossfadeSeconds = 0.5f;    // "BeginCrossfade(target, 0,5)" saat mulai run dan load
        public float tideMinVolume = 0.6f;            // "Lerp(0,6, 1, level)" untuk layer followsTide
        public float oneShotHeightMin = 5f;           // One-shot naik 5 sampai 15 meter dari pemain
        public float oneShotHeightMax = 15f;
    }

    /// <summary>Musik pertempuran dan suara penanda musuh. Blueprint: Core_Logic/CombatMusicDirector.md (#114).</summary>
    public partial class CombatMusicBalance
    {
        public float alertThreshold = 0.2f;
        public float combatThreshold = 0.45f;
        public float peakThreshold = 0.85f;
        public float hysteresis = 0.1f;
        public float riseConfirmSeconds = 0.5f;
        public float minTierHoldSeconds = 12f;
        public float riseFadeSeconds = 1.5f;
        public float fallFadeSeconds = 6f;
        public float nearRadius = 25f;
        public float nearSaturation = 6f;
        public float damageScorePerPoint = 0.02f;
        public float damageScoreDecayPerSec = 0.05f;
        public float pinnedScore = 0.7f;
        public int maxEnemyVoices = 8;
        public int maxSameVariantVoices = 3;
        public float occlusionCheckSeconds = 0.25f;
        public float occlusionLowpassHz = 1500f;
        public int crowdBedMinEngaged = 12;
        public float disasterMusicDuckDb = -6f;
        public float stingerMinInterval = 3f;

        // Tambahan implementasi (angka dari teks CombatMusicDirector.md bagian 2 dan 4).
        public float evaluateIntervalSeconds = 0.25f;  // EvaluateScore "4 kali per detik"
        public float phaseFloorRelax = 0.1f;           // Tabel PhaseFloor
        public float phaseFloorBuildUp = 0.25f;
        public float phaseFloorPeakFade = 0.5f;
        public float phaseFloorSustainPeak = 0.85f;
        public float largeThreatWeight = 3f;           // Bobot musuh cues.isLarge
        public float alertCalmVolume = 0.5f;           // "Calm lebih pelan" pada tingkat Alert (usulan nilai)
        public float calmDayNightFadeSeconds = 10f;    // calmDay dan calmNight saling crossfade 10 detik
        public float scheduleLeadSeconds = 0.1f;       // PlayScheduled(dspTime + 0,1)
        public int emitterTicksPerFrame = 16;          // Maksimal 16 emitter per frame
        public int tellPriorityBonus = 2;              // PlayTell memakai priority + 2
        public float voiceStealFadeSeconds = 0.05f;
        public float sameClipMinSeconds = 0.15f;
        public int crowdBedFullEngaged = 24;           // Jumlah musuh untuk volume kerumunan penuh (usulan nilai)
        public float cinematicFadeOutSeconds = 1f;
        public float cinematicFadeInSeconds = 2f;
        public float runEndFadeSeconds = 2f;
        public float loadStingerSuppressSeconds = 2f;  // bloodMoonStartClip dilewati 2 detik pertama setelah load
        public float cueMinDistance = 2f;              // Pengaturan sumber 3D
        public float cueSpread = 60f;
    }

    /// <summary>Suara langkah pemain. Blueprint: Core_Logic/FootstepAudio.md (#21).</summary>
    public partial class FootstepBalance
    {
        public float stepDistance = 1.75f;
        public float minSpeed = 0.5f;
        public float baseVolume = 0.6f;
        public float crouchVolumeMultiplier = 0.4f;
        public float sprintSpeedThreshold = 6f;
        public float sprintVolumeMultiplier = 1.3f;
        public float heavyVolumeMultiplier = 1.2f;
        public float pitchMin = 0.93f;
        public float pitchMax = 1.07f;
        public float volumeJitter = 0.1f;
        public float minLandingAirSeconds = 0.25f;
        public float landingFullFallSpeed = 12f;

        // Tambahan implementasi: "Mathf.Lerp(0.5f, 1.3f, impact01)" di PlayLanding.
        public float landingVolumeMin = 0.5f;
        public float landingVolumeMax = 1.3f;
    }

    /// <summary>Pembungkus Animator. Blueprint: Core_Logic/AnimationController.md (#12).</summary>
    public partial class AnimationBalance
    {
        public float locomotionDampSeconds = 0.1f;
        public float enemySpeedSampleSeconds = 0.1f;
        public float minReloadAnimSpeed = 0.5f;
        public float maxReloadAnimSpeed = 2f;
    }

    /// <summary>Culling jarak jauh. Blueprint: Core_Logic/OptimizationManager.md (#62, #93). Usulan.</summary>
    public partial class OptimizationBalance
    {
        public float cullingDistance = 50f;
        public float cullCheckSeconds = 1f;

        // Tambahan implementasi: radius NavMesh.SamplePosition saat agen dinyalakan kembali (Uncull langkah 1).
        public float uncullSampleRadius = 2f;
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupC2). Field bagian config dari tabel blueprint terkait.
    // =====================================================================================

    // =====================================================================================
    // Field config grup C2 (sistem pendukung): combatFeedback (#51), compass (#30), tutorial (#16), stats (#57),
    // console (#56), narrative (#99). Kelas bagian sudah dideklarasikan partial di GameBalanceSections.cs dan sudah
    // punya field di GameBalanceConfig.cs. Nilai awal mengikuti tabel "Angka penyetelan" di blueprint masing-masing.
    // run.endingTimeoutSeconds (NarrativeSequencer.md) sudah ada di RunBalance, jadi tidak ditulis ulang.
    // =====================================================================================

    /// <summary>Indikator damage, Hit Marker, dan Kill Marker. Blueprint: Core_Logic/CombatFeedbackUI.md (#51).</summary>
    public partial class CombatFeedbackBalance
    {
        public int maxIndicators = 6;               // Sekaligus ukuran pool UI_DamageIndicator
        public float indicatorSeconds = 1.2f;
        public float indicatorMergeAngle = 30f;
        public float minAlpha = 0.35f;
        public float fullAlphaDamage = 30f;
        public float sourceOffsetMeters = 10f;
        public float telegraphSeconds = 0.6f;
        public float hitMarkerSeconds = 0.12f;
        public float headshotMarkerScale = 1.3f;
        public int killBlinkCount = 2;
        public float killBlinkSeconds = 0.3f;
        public float tinkMinInterval = 0.08f;
    }

    /// <summary>Bilah kompas. Blueprint: Core_Logic/CompassSystem.md (#30). Usulan disetujui developer sebagai nilai awal.</summary>
    public partial class CompassBalance
    {
        public float visibleArcDegrees = 180f;
        public float edgeFadeDegrees = 15f;
        public bool showDistance = true;
        public bool clampObjectiveToEdge = true;
    }

    /// <summary>Pop-up panduan. Blueprint: Core_Logic/TutorialManager.md (#16).</summary>
    public partial class TutorialBalance
    {
        public float displaySeconds = 6f;
        public float minGapSeconds = 3f;
        public float dangerHoldSeconds = 4f;
        public float minSeenSeconds = 2f;
        public int queueLimit = 4;
        public float lowVitalThreshold = 50f;
    }

    /// <summary>Statistik lokal stats.json. Blueprint: Core_Logic/LocalStatsManager.md (#57).</summary>
    public partial class StatsBalance
    {
        public float flushIntervalSeconds = 60f;
    }

    /// <summary>Konsol pengembang. Blueprint: Core_Logic/DevConsole.md (#56).</summary>
    public partial class ConsoleBalance
    {
        public int maxOutputLines = 200;
        public int maxHistory = 50;
    }

    /// <summary>Pemutar adegan. Blueprint: Core_Logic/NarrativeSequencer.md (#99).</summary>
    public partial class NarrativeBalance
    {
        public float beatFadeSeconds = 0.5f;
        public float musicFadeSeconds = 1f;
        public float skipHintSeconds = 3f;
        public float watchdogExtraSeconds = 10f;
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupD). Field bagian config dari tabel blueprint terkait.
    // =====================================================================================

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

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupE). Field bagian config dari tabel blueprint terkait.
    // =====================================================================================

    // =====================================================================================
    // Angka penyetelan Grup E. Nilai awal sesuai tabel "Angka penyetelan" tiap blueprint dan tabel
    // "Bagian config per sistem" di Core_Logic/GameBalanceConfig.md. Semua bagian sudah ada di GameBalanceConfig.
    // Semua angka di sini usulan yang disetujui developer sebagai nilai awal (#85), kecuali disebut lain.
    // =====================================================================================

    /// <summary>Tong Merah (EnvironmentalHazard.md, #59). Usulan.</summary>
    public partial class HazardBalance
    {
        public float barrelRadius = 5f;
        public float barrelDamage = 100f;
        public float chainDelaySeconds = 0.15f;
        public float fireFuseSeconds = 2f;
        public float noiseRadius = 40f;
    }

    /// <summary>Api menjalar (FireSystem.md, #97).</summary>
    public partial class FireBalance
    {
        public int maxBurning = 24;
        public int maxAreas = 8;
        public int maxCarvedAreas = 4;
        public float burnTickSeconds = 0.5f;
        public float spreadTickSeconds = 1f;
        public int spreadChecksPerTick = 6;
        public float areaTickSeconds = 0.5f;
        public float areaIgniteChancePerTick = 0.25f;
        public float reigniteCooldownSeconds = 20f;
        public int maxVegetationGeneration = 3;
        public float rainSpreadMultiplier = 0.25f;
        public float rainBurnMultiplier = 0.5f;          // Dipakai sebagai: burnRemaining berkurang 1/0,5 = 2 kali lebih cepat, dan durasi area dikali 0,5
        public float snowSpreadMultiplier = 0.5f;
        public float stormAreaMultiplier = 0.25f;
        public float roofRecheckSeconds = 5f;
        public float roofCheckDistance = 20f;            // Tambahan implementasi: sama dengan campfire.skyCheckDistance
        public float extinguishHoldSeconds = 1f;
        public float fireballAreaRadius = 4f;
        public float fireballAreaSeconds = 12f;
        public float fireballDamagePerSec = 8f;
        public float fireAxeIgniteChance = 0.25f;
        public float campfireTrapIgniteChancePerTick = 0.1f;
    }

    /// <summary>Sumber cahaya, keterlihatan, dan batas lampu (LightSourceSystem.md, #98).</summary>
    public partial class LightBalance
    {
        public float visibilityTickSeconds = 0.25f;
        public float budgetTickSeconds = 0.5f;
        public float fixtureTickSeconds = 1f;
        public float ambientDay = 1f;
        public float ambientNight = 0.1f;
        public float ambientAmazonDay = 0.45f;
        public float ambientInterior = 0.1f;
        public int interiorMaxRealtimeLights = 3;
        public float stormAmbientMultiplier = 0.7f;
        public float visibilityInDark = 0.6f;
        public float visibilityInLight = 1f;
        public float flashlightVisibilityBonus = 0.3f;
        public int maxRegisteredSources = 64;
        public int maxRealtimeLights = 6;
        public int maxShadowLights = 1;
        public float budgetSwapMargin = 1.2f;
        public int maxLightStructures = 24;
        public float rainFuelMultiplier = 1.5f;
        public bool stormExtinguishes = true;
        public float flickerSeconds = 2f;
        public float attractionChancePerLight = 0.05f;
        public float attractionChanceMax = 0.35f;
        public float mossStaticLightLevel = 0.5f;
        public float lightFadeSeconds = 0.25f;           // Peralihan lampu real-time (blueprint: 0,25 detik)
        public float addFuelHoldSeconds = 0.6f;          // Tahan E untuk menambah bahan bakar (blueprint: 0,6 detik)
        public float visibilityPublishDelta = 0.02f;
    }

    /// <summary>Senter (FlashlightController.md, #98).</summary>
    public partial class FlashlightBalance
    {
        public float maxBattery = 100f;
        public float drainPerSec = 0.2f;
        public float batteryRecharge = 100f;
        public float range = 25f;
        public float spotAngle = 50f;
        public float flickerHz = 6f;
    }

    /// <summary>Air pasang (TideManager.md, #91).</summary>
    public partial class TideBalance
    {
        public float lowWaterY = 0f;
        public float highWaterY = 1.6f;
        public float dayAmplitude = 0.6f;
        public float nightAmplitude = 1f;
        public float highThreshold = 0.7f;
        public float lowThreshold = 0.2f;
        public float warnLevel = 0.55f;
        public float stormSurgeLevel = 0.25f;
        public float surgeRatePerGameHour = 0.125f;
        public float colliderStepMeters = 0.15f;
        public float wadeDepth = 0.35f;
        public float wadeStaminaPerSec = 2f;
        public float highAreaCost = 20f;
        public float tickSeconds = 0.25f;                // Blueprint: Update setiap 0,25 detik
        public float visualLerpSpeed = 0.5f;             // Meter per detik untuk gerak halus mesh laut
        public float publishLevelDelta = 0.05f;
    }

    /// <summary>Berenang (WaterSwimmingController.md, #13). swimStaminaPerSec dan drownDamagePerSec ada di vitals.</summary>
    public partial class SwimmingBalance
    {
        public float idleStaminaPerSec = 2f;
        public float swimSpeedMultiplier = 0.5f;
        public float wadeSpeedMultiplier = 0.7f;
        public float wadeEnterDepth = 0.5f;
        public float swimEnterDepth = 1.3f;
        public float swimExitDepth = 1.0f;
        public float maxBreathSeconds = 30f;
        public float breathRecoverPerSec = 10f;
        public float sinkingBreathMultiplier = 2f;
        public float sinkSpeed = 0.5f;
        public float verticalSwimSpeed = 1.5f;
        public float surfaceEyeOffset = 0.3f;
        public float waterCheckSeconds = 0.1f;
        public float landingProbeDistance = 30f;
        public float breathPublishHz = 4f;               // Blueprint: 4 kali per detik selama breathRatio < 1
    }

    /// <summary>Kargo pantai mingguan (BeachSupplyManager.md, #71). GDD bagian 6: Weekly Beach Supply setiap 7 hari.</summary>
    public partial class BeachSupplyBalance
    {
        public int intervalDays = 7;                     // GDD bagian 6
        public int crateCountBase = 2;
        public int crateCountPerTwoWeeks = 1;
        public int crateCountMax = 4;
        public int rollsPerCrate = 3;
        public float crateLifetimeGameHours = 18f;
        public float crateMinLevel = 0.85f;
        public float openHoldSeconds = 1.5f;
        public float openNoiseRadius = 10f;
        public float ambushTriggerDistance = 50f;
        public int ambushAnimalsBase = 2;
        public int ambushAnimalsMax = 4;
        public int ambushZombiesBase = 4;
        public int ambushZombiesMax = 10;
        public float ambushSpawnRadius = 25f;            // Blueprint: RequestBurst(..., tengah, 25)
        public float pointNavSampleDistance = 2f;
        public float pointClearRadius = 1.5f;
        public float fallbackSpacing = 1.5f;
    }

    /// <summary>Tidur (SleepSystem.md, #67). maxGameHoursPerStep sudah ada di GameBalanceSections.</summary>
    public partial class SleepBalance
    {
        public float timeMultiplierAsleep = 2160f;
        public float drainRatioAsleep = 0.5f;
        public float enemyBlockRadius = 25f;
        public float enemyWakeRadius = 8f;
        public float enemyCheckSeconds = 0.5f;
        public float structureAlarmRadius = 15f;
        public float minVitalToSleep = 10f;
        public float retrySeconds = 5f;
        public float hpPerGameHourInBase = 2f;
        public float hpPerGameHourOutside = 0f;
        public float minHoursForStamina = 4f;
        public float interruptChanceInBase = 0.25f;
        public float interruptChanceOutside = 0.6f;
        public float interruptChanceBeach = 1f;
        public float interruptMinProgress = 0.25f;
        public float interruptMaxProgress = 0.9f;
        public int ambushBaseCount = 4;
        public int ambushCountPerTenDays = 1;
        public float ambushRadiusInBase = 20f;
        public float ambushRadiusExposed = 10f;
        public float beachRadius = 8f;
        public float beachTideStaminaLoss = 30f;
    }

    /// <summary>Streaming region (RegionStreaming.md, #88). Usulan teknis.</summary>
    public partial class StreamingBalance
    {
        public float loadDistance = 150f;
        public float unloadDistance = 250f;
        public float urgentDistance = 40f;
        public float checkIntervalSeconds = 0.5f;
        public int maxLoadedRegions = 2;
        public float stallNoticeSeconds = 2f;
        public float stallPanelSeconds = 10f;
        public float safePositionIntervalSeconds = 1f;
        public float unusedAssetsMinIntervalSeconds = 120f;
        public int baseMemoryLimitMB = 1000;
        public int regionMemoryLimitMB = 1200;
        public int regionBatchLimit = 600;
        public int baseBatchLimit = 300;
        public float regionLoadTimeLimitSeconds = 8f;
        public float loadRetrySeconds = 5f;              // Blueprint "Scene gagal dimuat": coba lagi setelah 5 detik
        public float safeGroundProbeDistance = 3f;
        public float stallSpeedThreshold = 0.3f;         // Pemain dianggap tertahan kalau hampir diam di dekat SeamBarrier
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupF). Field bagian config dari tabel blueprint terkait.
    // =====================================================================================

    // =====================================================================================
    // Angka penyetelan Grup F. Nilai awal sesuai tabel "Angka penyetelan" tiap blueprint dan tabel
    // "Bagian config per sistem" di Core_Logic/GameBalanceConfig.md. Semua bagian sudah ada di GameBalanceConfig.
    // =====================================================================================

    /// <summary>Blood Moon (HordeManager.md, #50). GDD bagian 3: setiap 10 hari. Isi gelombang usulan disetujui developer.</summary>
    public partial class BloodMoonBalance
    {
        public bool enabled = true;                 // Aturan GDD bagian 3. Saklar hanya untuk tes
        public int intervalDays = 10;               // GDD bagian 3: hari ke-10, 20, 30, ...
        public int warningHour = 12;
        public int waveCount = 4;
        public float firstWaveDelaySeconds = 20f;
        public float waveIntervalSeconds = 90f;
        public int baseWaveSize = 12;
        public int waveSizePerEvent = 4;
        public int maxAlive = 50;                   // SpawnOverride.capOverride
        public int sustainAlive = 20;
        public float sustainCheckSeconds = 30f;
        public float burstRadius = 45f;
        public float baseEngageDistance = 120f;
    }

    /// <summary>Jalur musuh, pintu, dan deteksi dalam base (BaseNavigation.md, #78). Usulan disetujui developer (#85).</summary>
    public partial class BaseNavBalance
    {
        public float pathCheckIntervalSeconds = 1f;
        public int maxPathChecksPerFrame = 4;
        public float spawnGraceSeconds = 2f;
        public float blockedConfirmSeconds = 1.5f;
        public float retargetCooldownSeconds = 4f;
        public float targetSearchRadius = 6f;
        public int maxAttackersPerWall = 4;
        public int maxAttackersPerDoor = 3;
        public float slotSpacing = 0.8f;
        public float slotStandoff = 0.9f;
        public float waitRingRadius = 4f;
        public float insideCheckSeconds = 0.5f;
        public float insideRayDistance = 6f;
        public int insideMinWallHits = 3;
        public float doorSwingSeconds = 0.4f;
        public float doorNpcAutoCloseSeconds = 1.5f;
        public float lockedDamageMultiplier = 0.75f;
    }

    /// <summary>Perbaikan dan upgrade bangunan (BuildingIntegrity.md, #47). Usulan disetujui developer (#85).</summary>
    public partial class IntegrityBalance
    {
        public float repairRange = 4f;
        public float repairSwingSeconds = 0.6f;
        public float repairHpRatioPerSwing = 0.15f;
        public float repairBlockedAfterDamageSeconds = 2f;
        public float upgradeHoldSeconds = 1f;
        public float upgradeBlockedAfterDamageSeconds = 5f;
        public float hammerDecayPerRepairSwing = 0.5f;
        public float hammerDecayPerUpgrade = 2f;
        public float noiseRadiusPerSwing = 15f;
    }

    /// <summary>Jebakan base (BaseDefense.md, #26). Usulan disetujui developer (#85).</summary>
    public partial class TrapBalance
    {
        public float spikeDamagePerHit = 25f;
        public float spikeHitIntervalSeconds = 1.5f;
        public float spikeSelfDamagePerHit = 5f;     // Paku rusak setelah 12 kali kena (HP 60)
        public float spikeNoiseRadius = 6f;
        public float wireDamagePerHit = 4f;
        public float wireHitIntervalSeconds = 1f;
        public float wireSlowMultiplier = 0.5f;
        public float wireSlowSeconds = 1.5f;
        public float wireSelfDamagePerHit = 2f;      // Kawat rusak setelah 40 kali kena (HP 80)
        public float campfireDamagePerHit = 8f;
        public float campfireHitIntervalSeconds = 1f;
        public float tickIntervalSeconds = 0.25f;
        public int maxVictimsPerTrap = 8;
        public int maxTrapsPerRegion = 30;
    }

    /// <summary>Barang lempar (ThrowableSystem.md, #96). GDD bagian 3: Molotov, Bom Rakitan, Batu, Suar.</summary>
    public partial class ThrowableBalance
    {
        public bool enabled = true;                  // Sistem dipakai. false hanya sakelar uji
        public float molotovForce = 15f;
        public float molotovRadius = 3.5f;
        public float molotovDamagePerSec = 15f;
        public float molotovDuration = 8f;
        public float molotovNoise = 15f;
        public float bombForce = 13f;
        public float bombFuse = 3f;
        public float bombRadius = 6f;
        public float bombDamage = 120f;
        public float bombNoise = 40f;
        public float rockForce = 18f;
        public float rockNoise = 12f;
        public float flareForce = 15f;
        public float flareFuse = 0.5f;
        public float flareDuration = 15f;
        public float flareNoise = 25f;
        public float flareNoiseInterval = 1f;
        public float holdToAimSeconds = 0.2f;
        public float throwCooldownSeconds = 0.8f;
        public int arcPointCount = 30;
        public float arcTimeStep = 0.06f;
        public float maxLifetimeSeconds = 25f;
        public int maxActiveLures = 2;
        public float fireTickSeconds = 1f;           // Cadangan Molotov sampai FireSystem (#97) ada (Kasus Khusus)
    }

    /// <summary>Sarang zombie (ZombieLair.md, #112). GDD bagian 3: zombie diam di ruang gelap saat siang.</summary>
    public partial class LairBalance
    {
        public bool enabled = true;                  // Sistem dipakai. false hanya sakelar uji
        public float activationRadius = 40f;
        public float deactivationRadius = 60f;
        public float senseIntervalSeconds = 0.5f;
        public float proximityWakeRadius = 3f;
        public float crouchProximityWakeRadius = 1.5f;
        public float lightWakeRange = 8f;
        public float lightWakeSeconds = 2f;
        public float noiseWakeMultiplier = 1f;
        public float groupWakeRadius = 6f;
        public float doorWaitSeconds = 6f;
        public float returnToDormantSeconds = 20f;
        public float clearedBudgetReduction = 0.1f;
        public float maxBudgetReduction = 0.3f;
        public float nightReleaseRatio = 0.5f;
        public float nestHP = 300f;
        public float registryTickSeconds = 1f;       // LairRegistry.Tick "tiap 1 detik"
    }

    /// <summary>
    /// Bagian AI (#94, AITraversal.md) dari TraversalBalance. Bagian pemain (#101) ada di GameBalanceSections.cs.
    /// Usulan disetujui developer (#85).
    /// </summary>
    public partial class TraversalBalance
    {
        public float probeIntervalSeconds = 1.5f;
        public int maxProbesPerFrame = 2;
        public float campShotWindowSeconds = 5f;
        public float linkAvoidSeconds = 5f;
        public int retreatMaxTries = 3;
        public float reachTolerance = 2.5f;
        public float sampleRadius = 1.5f;
        public float surroundAfterSeconds = 3f;
        public float rockAfterSeconds = 10f;
        public float retreatAfterSeconds = 20f;
        public float surroundRadius = 6f;
        public int maxRockThrowers = 3;
        public float rockRangeMin = 5f;
        public float rockRangeMax = 25f;
        public float rockCooldownSeconds = 8f;
        public float campMoveDistance = 5f;
        public float campShotMultiplier = 2f;
        public float campTier1Seconds = 45f;
        public float campTier2Seconds = 90f;
        public float campRepeatSeconds = 60f;
        public int campCounterCount = 2;
        public float offNavGraceSeconds = 1f;
        public float offNavSampleRadius = 3f;
        public int linkUsersDefault = 1;
        public float linkWaitSeconds = 3f;
        public float jumpSeconds = 0.6f;
        public float climbSpeed = 1.5f;
        public float rockDamage = 10f;               // AttackData Zombie_Rock_Throw (#109): damage 10, infeksi 0
        public float rockWindupSeconds = 1f;         // Ancang-ancang 1 detik
        public float rockSpeed = 16f;                // Usulan teknis: kecepatan lempar batu zombie
        public float retreatDistance = 15f;          // Usulan teknis: jarak titik mundur dari pemain
        public float rockLifetimeSeconds = 6f;       // Usulan teknis: batu yang tidak mengenai apa pun kembali ke pool
        public float linkAvoidCost = 1000f;          // Usulan teknis: biaya area link yang penuh selama linkAvoidSeconds
    }

    // =====================================================================================
    // Ditambahkan: sistem pendukung (GrupG). Field bagian config dari tabel blueprint terkait.
    // =====================================================================================

    // =====================================================================================
    // Angka penyetelan sistem pendukung Grup G. Nilai awal dari tabel "Angka penyetelan" tiap blueprint:
    // raft/HydrationSystem.md (#43), DiseaseManager.md (#45), CampfireManager.md (#44), FishingSystem.md (#55),
    // DebrisRetrieval.md (#106), StorageChest.md (#46). Semua nilai adalah usulan yang disetujui developer sebagai nilai awal.
    // =====================================================================================

    public partial class HydrationBalance
    {
        // exertionThirstMultiplier sudah ada di GameBalanceSections.cs (Langkah 3).
        public float dehydratedStaminaRegenMultiplier = 0f;   // Regenerasi stamina saat Thirst nol
        public float dirtyWaterHoldSeconds = 1f;              // holdSeconds WaterSourcePoint
        public float collectorCapacityPortions = 4f;          // Isi penuh RainwaterCollector
        public float rainFillPerGameHour = 0.5f;
        public float stormFillPerGameHour = 0.75f;
        public float skyCheckDistance = 20f;                  // Raycast atap ke atas
        public float structureRecheckRadius = 6f;             // Bangunan baru dalam jarak ini memicu cek ulang atap
    }

    public partial class DiseaseBalance
    {
        public float foodPoisonGameHours = 3f;
        public float venomGameHours = 2f;
        public float bleedingGameHours = 6f;
        public float foodPoisonDamagePerSec = 0.25f;
        public float venomDamagePerSec = 0.5f;
        public float bleedingDamagePerSec = 0.3f;
        public float bleedChance = 0.25f;
        public float bleedMinDamage = 15f;
        public float overlayMaxAlpha = 0.18f;
        public float overlayPulseSeconds = 2.5f;
    }

    public partial class CampfireBalance
    {
        public float cookSeconds = 30f;          // Checklist issue #44
        public float boilSeconds = 20f;
        public float burnAfterSeconds = 30f;
        public int cookRowCount = 3;
        public float maxFuelSeconds = 300f;
        public float tickIntervalSeconds = 0.25f;
        public float rainFuelMultiplier = 1.5f;
        public bool stormExtinguishes = true;
        public float warmthRadius = 5f;          // Dibaca WarmthSystem (#65)
        public int recipeWood = 5;               // Bahan Kit_Campfire (diisi di aset resep rec_campfire)
        public int recipeStone = 3;
        public float skyCheckDistance = 20f;     // Sama dengan hydration.skyCheckDistance (aturan cek atap #43)
        public float structureRecheckRadius = 6f;
        public float cookedNotifyDistance = 15f; // Notifikasi "Makanan matang" hanya dalam jarak ini
    }

    public partial class FishingBalance
    {
        public float castMinDistance = 6f;
        public float castMaxDistance = 25f;
        public float castChargeSeconds = 1.2f;
        public float castProbeDepth = 40f;
        public float lureFlightSeconds = 0.8f;
        public float leashDistance = 32f;
        public float biteDelayMin = 12f;
        public float biteDelayMax = 30f;
        public float rainBiteMultiplier = 0.85f;
        public float biteWindowSeconds = 2f;
        public float tensionStart = 0.35f;
        public float tensionRisePerSec = 0.30f;
        public float tensionFallPerSec = 0.35f;
        public float surgeIntervalMin = 1.5f;
        public float surgeIntervalMax = 3.5f;
        public float surgeSeconds = 0.8f;
        public float surgeRisePerSec = 0.35f;
        public float progressPerSec = 0.10f;
        public float slackEscapeSeconds = 2f;
        public float recoverSeconds = 1f;
        public float lineBreakRecoverSeconds = 5f;
        public float waterRecheckSeconds = 1f;
    }

    public partial class DebrisBalance
    {
        public bool enabled = true;                  // Sakelar uji
        public int maxActive = 4;
        public float spawnIntervalGameHours = 3f;
        public float spawnCheckSeconds = 1f;
        public float stormIntervalMultiplier = 0.5f;
        public float rainIntervalMultiplier = 0.75f;
        public float highTideIntervalMultiplier = 0.75f;
        public float spawnPlayerMaxDistance = 150f;
        public float driftSpeed = 1.2f;
        public float hookRadius = 2.5f;
        public float pullSpeed = 4f;
        public float collectDistance = 3f;
        public float pullMaxDistance = 32f;
        public float bobAmplitude = 0.08f;           // Efek mengapung (Mathf.Sin kecil), usulan teknis
        public float bobFrequency = 1.2f;
    }

    public partial class StorageBalance
    {
        public int chestSlotCount = 20;
        public float dropScatterRadius = 1.2f;
    }
}
