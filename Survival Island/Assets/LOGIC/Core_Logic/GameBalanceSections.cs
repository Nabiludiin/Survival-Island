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
    public partial class InfectionBalance
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
    [Serializable] public partial class CraftingBalance { }

    /// <summary>Bagian repair (#113). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class RepairBalance { }

    /// <summary>Bagian upgrade (#113). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class UpgradeBalance { }

    /// <summary>Bagian fishing (#55). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class FishingBalance { }

    /// <summary>Bagian debris (#106). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class DebrisBalance { }

    /// <summary>Bagian surface (#86). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class SurfaceBalance { }

    /// <summary>Bagian build (#79). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class BuildBalance { }

    /// <summary>Bagian baseNav (#78). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class BaseNavBalance { }

    /// <summary>Bagian integrity (#47). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class IntegrityBalance { }

    /// <summary>Bagian storage (#46). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public class StorageBalance { }

    /// <summary>Bagian sleep (#67). TODO: diisi Langkah pemiliknya sesuai GameBalanceConfig.md.</summary>
    [Serializable] public partial class SleepBalance { }

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
    public class DamageBalance
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
    public class GunBalance
    {
        public float autoReloadDelay = 0.3f;
        public float dryClickCooldown = 0.25f;
        public float npcFireMaxAngle = 10f;
        public float movingSpeedThreshold = 0.5f;
    }

    /// <summary>Waktu dan batas serangan musuh (EnemyAttack.md, #109).</summary>
    [Serializable]
    public class EnemyAttackBalance
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
    public class HitReactionBalance
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
    public class ThreatBalance
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
    public class StealthBalance
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
    public class WildlifeBalance
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
    public class SpawnBalance
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
    public class SunlightBalance
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
    public class SpecialZombieBalance
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
    public class AIDirectorBalance
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
    public class CrowdBalance
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
    public class PinnedBalance
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
}
