# Blueprint: GameBalanceConfig.cs (ScriptableObject)
**Lokasi:** `docs/blueprint/Core_Logic/GameBalanceConfig.md`
**Deskripsi Utama:** Satu sumber angka penyetelan, dengan target pengalaman bermain yang jelas.
**Issue:** #84
**Rujukan GDD:** Bagian 1, 3, 5, dan 6. Semua angka di sini adalah usulan awal, dan harus disetel lewat playtest.

## 1. Arsitektur & Variabel Utama

```csharp
[CreateAssetMenu(menuName = "Survival/Game Balance Config")]
public class GameBalanceConfig : ScriptableObject
{
    public TimeBalance time;
    public VitalsBalance vitals;
    public ResourceBalance resource;
    public RegionBalance region;
    public WeatherBalance weather;
    public DifficultyProfile[] difficulties;

    // Bagian milik sistem lain. Daftarnya ada di "Bagian config per sistem".
    public RunBalance run;
    public UIBalance ui;
    public InputBalance input;
    public StreamingBalance streaming;
    public RunSeedBalance runSeed;
    public DamageBalance damage;
    public EquipmentBalance equipment;
    public ItemsBalance items;
    public InteractBalance interact;
    public CameraBalance camera;
    public TraversalBalance traversal;
    public CarryBalance carry;
    public ConsoleBalance console;
    public ShipBalance ship;
    public ShipHazardBalance shipHazard;
    public InfectionBalance infection;
    public HydrationBalance hydration;
    public CampfireBalance campfire;
    public DiseaseBalance disease;
    public StatusEffectsBalance statusEffects;
    public CraftingBalance crafting;
    public RepairBalance repair;
    public UpgradeBalance upgrade;
    public FishingBalance fishing;
    public DebrisBalance debris;
    public SurfaceBalance surface;
    public BuildBalance build;
    public BaseNavBalance baseNav;
    public IntegrityBalance integrity;
    public StorageBalance storage;
    public SleepBalance sleep;
    public SaveBalance save;
    public GunBalance gun;
    public EnemyAttackBalance enemyAttack;
    public HitReactionBalance hitReaction;
    public ThreatBalance threat;
    public StealthBalance stealth;
    public WildlifeBalance wildlife;
    public SpawnBalance spawn;
    public SunlightBalance sunlight;
    public SpecialZombieBalance specialZombie;
    public AIDirectorBalance aiDirector;
    public CrowdBalance crowd;
    public PinnedBalance pinned;
    public TrapBalance trap;
    public ThrowableBalance throwable;
    public LairBalance lair;
    public WarmthBalance warmth;
    public HeavyItemBalance heavyItem;
    public SwimmingBalance swimming;
    public TideBalance tide;
    public EnvironmentBalance environment;
    public DisasterBalance disaster;
    public BeachSupplyBalance beachSupply;
    public SupplyDropBalance supplyDrop;
    public BloodMoonBalance bloodMoon;
    public LuckyMachineBalance luckyMachine;
    public NpcBalance npc;
    public ObjectiveBalance objective;
    public LootBalance loot;
    public RemoteBaseBalance remoteBase;
    public FireBalance fire;
    public LightBalance light;
    public FlashlightBalance flashlight;
    public RescueBalance rescue;
    public DeathCinematicBalance deathCinematic;
    public StatsBalance stats;
    public NarrativeBalance narrative;
    public SafetyBalance safety;
    public AnimationBalance animation;
    public VfxBalance vfx;
    public TutorialBalance tutorial;
    public SettingsBalance settings;
    public FootstepBalance footstep;
    public NotificationBalance notification;
    public PostProcessBalance postProcess;
    public ShakeBalance shake;
    public CombatFeedbackBalance combatFeedback;
    public AmbientAudioBalance ambientAudio;
    public CombatMusicBalance combatMusic;
    public LoreBalance lore;
    public BarksBalance barks;
    public LocalizationBalance localization;
}
```

- `GameManager.Instance.Balance` menyimpan aset yang aktif. Sistem lain membacanya, bukan menulis angka sendiri.
- Tiap bagian adalah kelas `[Serializable]`, supaya mudah dibaca di Inspector.
- Sistem baru menambah bagiannya sendiri dan menulis namanya di blueprint, misalnya `GameBalanceConfig.infection`.

**Aturan angka:** Angka di blueprint lain ditulis sebagai `GameBalanceConfig.<bagian>.<nama>`, beserta nilai awalnya. Tabel di bawah menampung angka inti yang saling memengaruhi.

## 2. Detail Logika & Fungsi

### Target durasi (usulan)

| Target | Nilai | Alasan |
|---|---|---|
| Panjang satu hari game | 15 menit | Siang dan malam masing-masing sekitar 7,5 menit |
| Total 60 hari | 15 jam | Target. Run boleh lebih panjang, tetapi zombie semakin kuat setelah hari ke-60 |
| Run menang umum | 10 sampai 15 jam | Pemain cepat selesai sebelum hari ke-60, dan yang lebih lambat tetap bisa menang |

Sebelumnya `timeMultiplier = 50` menghasilkan 28,8 menit per hari dan 28,8 jam total. Angka itu terlalu panjang untuk game permadeath yang mengulang dari awal.

### Angka inti

| Bagian | Nama | Nilai lama | Nilai usulan | Hasilnya |
|---|---|---|---|---|
| `time` | `timeMultiplier` | 50 | 96 | 1 hari = 900 detik |
| `time` | `dayLengthSeconds` | Tidak ada | 900 | Hanya informasi |
| `vitals` | `hungerDrainPerSec` | 0,3 | 0,11 | Lapar habis dalam 1 hari game |
| `vitals` | `thirstDrainPerSec` | Tidak ada | 0,15 | Haus habis dalam sekitar 0,75 hari game |
| `vitals` | `sprintThirstMultiplier` | Tidak ada | 1,5 | Berlari menguras air lebih banyak |
| `vitals` | `starvationDamagePerSec` | 1,5 | 0,5 | HP penuh habis dalam 200 detik |
| `vitals` | `dehydrationDamagePerSec` | Tidak ada | 0,5 | HP penuh habis dalam 200 detik |
| `vitals` | `frostbiteStaminaDrainPerSec` | Tidak ada | 5 | Stamina penuh habis dalam 20 detik |
| `vitals` | `frostbiteDamagePerSec` | 5 | 1 | Setelah stamina habis, HP penuh habis dalam 100 detik |
| `resource` | `treeDropMin` / `treeDropMax` | 2 / 4 | 8 / 12 | Rata-rata 10 kayu per pohon |
| `resource` | `axeDecayPerUse` | 5 | 1 | 100 ayunan per kapak, sekitar 50 pohon |
| `region` | `bridgeWoodCost` | 500 | 500 | Mengikuti keputusan #85 |
| `region` | `heavyItemSpeedMultiplier` | 0,5 | 0,5 | Sesuai `Claude.md` |
| `region` | `helicopterPartCount` | Tidak ada | 5 | Disetel bersama #69 |
| `weather` | `weatherIntervalGameHours` | 300 detik nyata | 8 jam game | 3 kali per hari game |

**Waktu cuaca:** cuaca memakai waktu game, bukan waktu nyata. Dengan begitu cuaca ikut melambat saat game dijeda dan ikut cepat saat tidur.

### Target makan dan minum

- **Makanan:** 3 kali per hari game, dengan pemulihan sekitar 33 per porsi.
- **Minuman:** 4 kali per hari game, dengan pemulihan sekitar 25 per porsi.
- **Sumber makanan yang perlu dicek:** satu sesi memancing sebaiknya menghasilkan 1 porsi dalam sekitar 60 detik, sehingga 3 porsi butuh 180 detik atau 40 persen waktu siang. Berburu mengisi sisanya dengan risiko lebih tinggi. Bertani tidak dipakai (#48), jadi run setelah hari ke-60 tetap makan dari memancing (`FishingSystem.md`), berburu, panen kelapa yang tumbuh kembali (`ResourceRespawn.md`), dan kargo mingguan yang terus datang (`BeachSupplyManager.md`).

### Biaya bangun terhadap hasil panen

| Target | Perhitungan | Hasil |
|---|---|---|
| Satu pohon | 2 ayunan kapak (`maxHits` 5, damage 3) dan 20 detik berjalan serta memungut | Sekitar 25 detik per 10 kayu |
| Jembatan | 500 kayu = 50 pohon, 50 x 25 detik | 1.250 detik, sekitar 3 siang penuh |
| Base awal | 4 dinding, 1 pintu, 1 lantai, dan 1 atap. Sekitar 80 kayu | Sekitar 200 detik |
| Vaksin | 2 getah dan 2 jamur Region 2 per dosis. Sekitar 60 detik panen | Harus disetel bersama #64 |
| Kapak | 50 pohon menghabiskan 1 kapak penuh | Satu kapak cukup untuk jembatan |

Jembatan bisa selesai sekitar hari ke-5 sampai hari ke-8, kalau pemain juga makan, minum, dan membangun base. Angka ini memberi cukup waktu untuk region berikutnya.

### Perjalanan Region 4 ke Region 3

Asumsi: jarak 400 meter, dan 150 meter di antaranya berada di area dingin. Jarak final mengikuti #88.

| Hal | Perhitungan | Hasil |
|---|---|---|
| Kecepatan membawa barang | `walkSpeed` 5 x 0,5 | 2,5 m/s |
| Perjalanan berangkat | 400 / 2,5 | 160 detik |
| Perjalanan kembali | 400 / 5 | 80 detik |
| Satu kali bolak-balik | 160 + 80 | 240 detik |
| Semua suku cadang | 5 x 240 detik | 1.200 detik, sekitar 1,3 hari game tanpa pertempuran |

**Catatan permukaan salju (#101):** pengali permukaan 0,8 membuat kecepatan membawa barang di salju menjadi 2 m/s. Jadi 150 meter area dingin butuh 75 detik, bukan 60 detik. Tabel di atas belum memasukkan pengali ini.

### Ketahanan frostbite

- **Area dingin saat membawa barang:** kecepatan di salju 2 m/s karena pengali permukaan 0,8 (#101). 150 / 2 = 75 detik.
- **Tanpa pakaian hangat:** stamina habis dalam 20 detik, lalu HP turun sekitar 55 selama 55 detik sisanya. Pemain selamat, tetapi tidak bisa bolak-balik berkali-kali tanpa perlindungan.
- **Nilai lama:** `frostbiteDamagePerSec = 5` membunuh pemain dari HP penuh dalam 20 detik, jadi satu kali perjalanan pasti mematikan.

### Profil kesulitan

`DifficultyProfile` berisi `hungerMultiplier`, `thirstMultiplier`, dan `enemyDamageMultiplier`, yang dibaca `SurvivalStats` (#111), serta pengali jumlah musuh dan hasil panen. Versi awal hanya punya satu profil, `Normal`, dengan semua pengali bernilai 1.

### Lembar catatan playtest

Setiap sesi uji mengisi tabel ini di `docs/PLAYTEST_LOG.md`, yang dibuat saat Unity siap:

| Tanggal | Versi | Hari tercapai | Penyebab mati | Porsi makan per hari | Hari jembatan selesai | Waktu angkut suku cadang | Catatan |
|---|---|---|---|---|---|---|---|

### Bagian config per sistem

Angka di tabel ini adalah usulan awal dari blueprint pemiliknya. Rincian dan alasannya ada di blueprint tersebut.

| Bagian | Issue | Isi |
|---|---|---|
| `run` | #82, #99 | endSequenceTimeoutSeconds 15, gameOverInputDelaySeconds 1, saveWaitTimeoutSeconds 5, sceneLoadMinShowSeconds 0,5, endingTimeoutSeconds 120 |
| `ui` | #83 | panelFadeSeconds 0,15, pausedMusicVolumeDb -12, pauseAudioFadeSeconds 0,2 |
| `input` | #18 | rebindTimeoutSeconds 5 |
| `streaming` | #88 | loadDistance 150, unloadDistance 250, urgentDistance 40, checkIntervalSeconds 0,5, maxLoadedRegions 2, stallNoticeSeconds 2, stallPanelSeconds 10, safePositionIntervalSeconds 1, unusedAssetsMinIntervalSeconds 120, baseMemoryLimitMB 1000, regionMemoryLimitMB 1200, regionBatchLimit 600, baseBatchLimit 300, regionLoadTimeLimitSeconds 8 |
| `runSeed` | #89 | maxSeedRerolls 8, lootPointEmptyChance 0,5, minShipFoodItems 3, minShipAmmoItems 2, minShipDrinkItems 1, minPartPointSpacing 30, firstMajorDisasterDay 3, minDaysBetweenMajorDisasters 4 |
| `damage` | #59 | Pengali kepala 2, badan 1, kaki 0,5, falloff ledakan, pengali ledakan buatan pemain, damage minimum setelah armor, dan buffer target ledakan |
| `vitals` (tambahan) | #111 | Damage tenggelam dan infeksi, syarat dan laju pemulihan HP, biaya stamina, regenerasi dan kelelahan, penalti status rendah, serta interval event |
| `equipment` | #60 | Jumlah slot hotbar 8, waktu turun 0,15 dan naik 0,2 detik, dan izin tangan kiri saat membawa Heavy Item |
| `items` | #66 | Kecepatan saat memakai item, pemulihan bertahap 20 detik, jarak jatuh, penghapusan `WorldItem`, batas 64 item lepas, serta rasio dan akibat basi dan busuk |
| `interact` | #17 | Jarak 3 meter, lebar bidikan, jeda pemindaian, waktu ketahanan target, batas jarak dan kecepatan saat menahan tombol, dan warna highlight |
| `camera` | #102 | Batas sudut, FOV dunia dan tangan, tinggi mata, batas offset, recoil, weapon sway, head bob, FOV lari, dip mendarat, batas yaw tangga, dan kecepatan viewmodel |
| `traversal` | #101, #94 | Pemain (#101): tinggi dan kecepatan jongkok, kecepatan tangga dan tali, damage jatuh, lereng, permukaan, jeda pembacaan permukaan, dan jumpHeight 1,1. AI (#94): pemeriksaan keterjangkauan, waktu respons, lempar batu, hukuman berkemah, toleransi keluar NavMesh, dan link |
| `carry` | #118 | Slot dasar 15, tumpukan kayu dan batu 100, bahan 20, makanan 5, peluru 60, dan jarak barang sisa di pantai |
| `console` | #56 | Jumlah baris keluaran 200 dan riwayat perintah 50 |
| `ship` | #74 | Durasi 60 detik, pra-game 3 detik, waktu sirene, kapasitas tangan 4, radius dan tinggi area aman, waktu tahan berangkat, dan jarak NPC di pantai |
| `shipHazard` | #75 | Kemiringan tetap, pengaruh ke kecepatan, sway per tahap, kedalaman dan pengali air, tanda runtuh, batas dan perilaku zombie kapal, serta angka dorongan |
| `infection` | #64 | gainStandard 5, gainTribal 6, gainMoss 8, gainMilitary 10, gainFrozen 5, gainVillage 6, stage2At 25, stage3At 50, stage4At 75, stageMaxStamina [1; 0,9; 0,75; 0,5], stageStaminaRegen [1; 1; 0,75; 0,5], stageVision [0; 0,15; 0,4; 0,8], selfProgressPerGameHour 0, selfProgressMinInfection 50, startingVaccineDoses 2, vaccineReduction 50, vaccineInjectSeconds 4, medicVaccineMultiplier 1,25, criticalWarningAt 90, zombifyCinematicSeconds 4 |
| `hydration` | #43 | exertionThirstMultiplier 1,25, dehydratedStaminaRegenMultiplier 0, dirtyWaterHoldSeconds 1, collectorCapacityPortions 4, rainFillPerGameHour 0,5, stormFillPerGameHour 0,75, skyCheckDistance 20, structureRecheckRadius 6 |
| `campfire` | #44 | cookSeconds 30, boilSeconds 20, burnAfterSeconds 30, cookRowCount 3, maxFuelSeconds 300, tickIntervalSeconds 0,25, rainFuelMultiplier 1,5, stormExtinguishes true, warmthRadius 5, recipeWood 5, recipeStone 3 |
| `disease` | #45 | foodPoisonGameHours 3, venomGameHours 2, bleedingGameHours 6, foodPoisonDamagePerSec 0,25, venomDamagePerSec 0,5, bleedingDamagePerSec 0,3, bleedChance 0,25, bleedMinDamage 15, overlayMaxAlpha 0,18, overlayPulseSeconds 2,5 |
| `statusEffects` | #20 | maxActive 8, supplementSeconds 60, supplementSpeedMultiplier 1,2, expiringWarningSeconds 5 |
| `crafting` | #77 | maxQueue 5, stationRangeMeters 4, leaveGraceMeters 2, stationCheckSeconds 0,5, hitPauseSeconds 1,5, npcCrafterSpeedMultiplier 1,5 |
| `resource` (tambahan) | #73 | rockDropMin 4, rockDropMax 6, pickaxeDecayPerUse 1, knifeDecayPerHarvest 0,5, coconutRespawnDays 3, regrowJitterHours 6, regrowMinPlayerDistance 25, regrowClearRadius 3, regrowChecksPerFrame 20, dropChunkSize 4, dropSettleSeconds 3, dropMaxFallMeters 4, minNodesPerRegion sesuai tabel kepadatan, stoneRespawnDays 2 |
| `repair` | #113 | lowRatio 0,2, maxLossPerRepair 0,1, maxRepairs 6, repairSeconds 3, upgradeSeconds 6 |
| `upgrade` | #113 | damageBonus 0,1, magazineBonus 2, speedBonus 0,1, durabilityBonus 0,25, maxPerItem 2, maxDamageMultiplier 4 |
| `fishing` | #55 | Jarak dan tenaga lempar (castMinDistance 6, castMaxDistance 25, leashDistance 32), waktu gigit 12 sampai 30 detik, biteWindowSeconds 2, serta angka Tension Bar (tensionRisePerSec 0,30, tensionFallPerSec 0,35, progressPerSec 0,10). Target 1 porsi sekitar 60 detik |
| `debris` | #106 | enabled true, maxActive 4, spawnIntervalGameHours 3, spawnCheckSeconds 1, pengali Storm 0,5, Rain 0,75, dan pasang 0,75, driftSpeed 1,2, hookRadius 2,5, pullSpeed 4, collectDistance 3, pullMaxDistance 32. Sekitar 13 kayu per hari, jauh di bawah menebang |
| `surface` | #86 | probeUp 1, probeDown 3, beachBuildSandWeight 0,35, nearSandSampleCount 8, runtimeBakeRowsPerFrame 64, debugGridCells 64, debugGridSpacing 1 |
| `build` | #79 | gridSize 2,5, maxBuildDistance 6, levelHeight 2,5, heightSnap 0,25, maxFoundationHeightDelta 1,25, groundProbeHeight 5, maxLevels 3, maxHorizontalSpan 2, maxStructures 400, overlapShrink 0,1, maxCollapsePerFrame 4, maxCollapseVfxPerSecond 8, demolishHoldSeconds 1,2, demolishRefundRatio 0,5, demolishBlockedAfterDamageSeconds 5, hammerDecayPerBuild 1, hammerDecayPerDemolish 1 |
| `baseNav` | #78 | pathCheckIntervalSeconds 1, maxPathChecksPerFrame 4, spawnGraceSeconds 2, blockedConfirmSeconds 1,5, retargetCooldownSeconds 4, targetSearchRadius 6, maxAttackersPerWall 4, maxAttackersPerDoor 3, slotSpacing 0,8, slotStandoff 0,9, waitRingRadius 4, insideCheckSeconds 0,5, insideRayDistance 6, insideMinWallHits 3, doorSwingSeconds 0,4, doorNpcAutoCloseSeconds 1,5, lockedDamageMultiplier 0,75 (pintu terkunci menerima 25 persen lebih sedikit damage `Structural`, tetapi tetap bisa didobrak) |
| `integrity` | #47 | repairRange 4, repairSwingSeconds 0,6, repairHpRatioPerSwing 0,15, repairBlockedAfterDamageSeconds 2, upgradeHoldSeconds 1, upgradeBlockedAfterDamageSeconds 5, hammerDecayPerRepairSwing 0,5, hammerDecayPerUpgrade 2, noiseRadiusPerSwing 15 |
| `storage` | #46 | chestSlotCount 20, dropScatterRadius 1,2 |
| `sleep` | #67 | timeMultiplierAsleep 2160, drainRatioAsleep 0,5, maxGameHoursPerStep 0,25, enemyBlockRadius 25, enemyWakeRadius 8, enemyCheckSeconds 0,5, structureAlarmRadius 15, minVitalToSleep 10, retrySeconds 5, hpPerGameHourInBase 2, hpPerGameHourOutside 0, minHoursForStamina 4, interruptChanceInBase 0,25, interruptChanceOutside 0,6, interruptChanceBeach 1, interruptMinProgress 0,25, interruptMaxProgress 0,9, ambushBaseCount 4, ambushCountPerTenDays 1, ambushRadiusInBase 20, ambushRadiusExposed 10, beachRadius 8, beachTideStaminaLoss 30 |
| `save` | #80 | combatEnemyRadius 30, combatQuietSeconds 10, autosaveRetrySeconds 10, autosaveMaxDeferSeconds 120, autosaveMinHpRatio 0,1, combatRestoreMaxEnemies 10, combatRestoreRadius 20 |
| `gun` | #58 | Jeda reload otomatis, jeda klik kosong, sudut tembak NPC, ambang bergerak, dan umur efek. Angka per senjata berada di aset `WeaponData` |
| `enemyAttack` | #109 | Batas penyerang pemain dan NPC, radius tunggu, jeda meminta izin, toleransi Animation Event, jeda setelah dibatalkan, dan jeda tanda arah |
| `hitReaction` | #95 | Stagger, efek kaki, ambang armor, knockback, Shove, mayat, ragdoll, dan batas drop per malam |
| `threat` | #108 | Jeda penilaian, waktu minimum target, selisih skor, aggro, pengurang kerumunan, Blood Moon, kehilangan jejak, target tak terjangkau, dan batas per frame |
| `stealth` | #49 | Saklar sistem, jarak dan sudut pandang, laju deteksi, pengali jongkok dan lari, radius suara, jadwal tick, dan titik perburuan |
| `wildlife` | #61 | Jeda FSM, aggro dan suara, teritori, target tak terjangkau, kabur ke sarang, serangan khusus, dan menguliti. Angka per spesies ada di `AnimalData` |
| `spawn` | #62 | Jeda dan batas spawn, batas zombie dan hewan, jarak, arah belakang, pengaman macet, sapu bersih, target hewan, dan batas pengurangan anggaran dari sarang |
| `sunlight` | #63 | Jeda cek sinar, sudut matahari, durasi terbakar, batas berlindung, jam sembunyi, waktu turun dan naik, boost, dan jumlah Zombie Desa siang |
| `specialZombie` | #25 | Angka perilaku varian: jerat Tribal, amarah Lumut, lambat Militer, ledakan Beku, terkaman Desa, dan pengaman macet varian besar. Statistik HP dan kecepatan ada di aset `ZombieData` |
| `aiDirector` | #27 | `threatCurve` per hari, pengali region, waktu tahap Director, intensitas, ukuran mob, batas kecepatan zombie, dan pengali setelah hari ke-60: postTargetCountPerDay 0,02, postTargetCountMax 1,5, postTargetHpPerDay 0,02, postTargetHpMax 2, postTargetDamagePerDay 0,01, postTargetDamageMax 1,5 (keputusan developer, juga dipakai Blood Moon dan penyergapan) |
| `crowd` | #93 | Kapasitas agen, batas tick per frame, interval per tingkatan, ambang update tujuan, kelompok, avoidance, buffer overlap, dan stagger animasi |
| `pinned` | #110 | enabled, kemajuan lepas, biaya stamina, pengali status, Pisau, bantuan NPC, batas tertahan, batas penyerang lain, dan waktu kebal |
| `trap` | #26 | Damage, jeda, keausan, dan suara Spike Trap, kawat, dan api unggun, serta batas jebakan per region |
| `throwable` | #96 | enabled true, gaya, sumbu, radius, damage, durasi, dan noise tiap barang, serta bidikan, jeda lempar, dan batas Suar |
| `lair` | #112 | enabled true, radius aktif, pemicu Dormant, pintu keluar, HP sarang, pengurangan horde, dan porsi zombie yang keluar saat malam |
| `warmth` | #65 | tickSeconds 0,5; freezeEnterThreshold 20; freezeExitThreshold 30; ambientHeat 5; regionSnowCold 20; blizzardCold 2; rainCold/rainFloor 8/40; stormCold/stormFloor 12/30; waterCold/waterFloor 15/30; snowWaterCold 30; clothesHeat 20; torchHeat 20; protectedFloor 30 (usulan); campfireHeat 40; indoorHeat 25; shelterCheckSeconds 2; shelterProbeDistance 6; torchBurnSeconds 300; torchRainExtinguishSeconds 20 |
| `heavyItem` | #69 | pickupHoldSeconds 0,5; dropDamageThreshold 25; dropForwardDistance 1,2; validSampleSeconds 1; navSampleRadius 2; recoveryCheckSeconds 5; discoverRadius 40 |
| `swimming` | #13 | idleStaminaPerSec 2; swimSpeedMultiplier 0,5; wadeSpeedMultiplier 0,7; wadeEnterDepth 0,5; swimEnterDepth 1,3; swimExitDepth 1,0; maxBreathSeconds 30; breathRecoverPerSec 10; sinkingBreathMultiplier 2; sinkSpeed 0,5; verticalSwimSpeed 1,5; surfaceEyeOffset 0,3; waterCheckSeconds 0,1; landingProbeDistance 30 |
| `region` (tambahan) | #68 | bridgeDepositHoldSeconds 1; ravineMinWidth 30; gateVerifySeconds 2; ravineFallDamage 20 |
| `tide` | #91 | lowWaterY, dayAmplitude, highThreshold, warnLevel, stormSurgeLevel 0,25, surgeRatePerGameHour, colliderStepMeters, wadeDepth, wadeStaminaPerSec, highAreaCost 20 |
| `environment` | #92 | weatherLayers, regionBlendSeconds, weatherBlendSeconds, eventBlendSeconds, interiorBlendSeconds, applyIntervalSeconds, maxFogDensity, defaultMinAmbientLuminance 0,035, eruptionRemoteWeight |
| `weather` (tambahan) | #115 | effects (tabel `WeatherEffects` per cuaca), minSevereDay, dan presentBlendSeconds 6. `weatherIntervalGameHours` tetap memakai nilai #84 |
| `disaster` | #70 | scheduleBlockDays 60, fireballIntervalSeconds, fireballRingMin/Max, fireballMarkerSeconds 2,5, fireballRadius, fireballDamageCenter/Edge, fireballStructureDamage, fireballTreeChance 0,25, eruptionOffscreenHits, quakeDamageRatio, quakePulses, maxStructuresPerFrame, stormPulseSeconds, stormStructureDamage, treeFallSeconds 45, treeFallRingMin/Max 12/30 m, treeFallRadius, treeFallDamage, treeFallStructureDamage, quakeRemoteDamagePerHit 75, quakeRemoteHits 4 |
| `beachSupply` | #71 | intervalDays 7, crateCountBase, crateCountPerTwoWeeks, crateCountMax, rollsPerCrate, crateLifetimeGameHours, crateMinLevel, openHoldSeconds, openNoiseRadius, ambushTriggerDistance, ambushAnimalsBase, ambushZombiesBase |
| `supplyDrop` | #54 | Saklar enabled false, dailyChance 0,10, minDay 3, cooldownDays 3, jam jatuh 8 sampai 14, jarak titik jatuh 120 sampai 260, serta angka pesawat, parasut, suar, dan kedaluwarsa peti |
| `bloodMoon` | #50 | Saklar enabled true (aturan GDD), intervalDays 10, warningHour 12, waveCount 4, baseWaveSize 12, waveSizePerEvent 4, maxAlive 50, sustainAlive 20, burstRadius 45, baseEngageDistance 120, dan jeda gelombang |
| `luckyMachine` | #72 | freeWindowSeconds 30, maxFreeSpinsPerWindow 3, maxStoredSpins 1, spinSeconds 4, vacuumSeconds 1,2, bobot tier 5/25/70, gateReopenSeconds 2, dan cageRewardOffset 2 |
| `npc` | #76 | thinkSeconds 0,25, jarak ikut 5 dan 2,5, teleportDistance 40, detectRadius 15, attackLeashDistance 12, baseRegenPerSec 0,5, workSearchRadius 30, cookSpeedMultiplier 1,25, builderRepairMultiplier 1,25, dan avoidancePriority 70 |
| `objective` | #81 | baseWallCount 4, hudFlashSeconds 3, chapter2TriggerDay 2, chapter3TriggerRegion `Amazon` (keputusan developer) |
| `loot` | #103 | warehouseEmptyChance 0,25, chestRollsMin/Max 1/3, bodyRollsMin/Max 1/2, amountMulVolcano 1, amountMulAmazon 1,1, amountMulLogistic 1,25, amountMulSnow 1,4, rareWeightPerDay 0,02, rareWeightMaxMul 2, amountBonusPer10Days 0,1. Memakai `runSeed.lootPointEmptyChance` 0,5 yang sudah ada |
| `remoteBase` | #90 | Jarak: remoteDistance 80, returnDistance 60, distanceCheckSeconds 2, clusterRadius 30. Serangan: minStructuresForRaid 4, maxRaidedClusters 2, firstRaidDay 3, raidChanceBase 0,5, raidChancePerDay 0,01, raidChanceMax 0,9, basePower 150, powerPerDay 20, maxPower 2550, bloodMoonMultiplier 2, referenceStructures 12, sizeFactorMin/Max 0,75/1,5, varianceFraction 0,15. Pertahanan: scoreWallWood/Stone 10/25, scoreTrap 15, scoreNpcGuard 20, mitigationK 200, maxMitigation 0,8. Penerapan: hitChunk 60, doorWeight 2, trapWearFraction 0,3, raidStartHourMin/Max 20/23, raidEndHour 5. NPC: npcInjuryChanceMax 0,6, npcInjuryDamage 40, npcDeathChanceOnBreach 0,35, npcGuardDeathMultiplier 0,5, npcGuardMinHpRatio 0,25. Peralihan: takeoverZombiesPerPower 0,02, takeoverMaxZombies 12 |
| `fire` | #97 | Batas api aktif (maxBurning 24, maxAreas 8, maxCarvedAreas 4), jeda tick, peluang menyala dari area, jeda nyala ulang, batas lompatan vegetasi, pengali cuaca, lama memadamkan, angka area Fireball, serta peluang Fire Axe dan jebakan api unggun |
| `light` | #98 | Jeda tick, cahaya alam per kondisi, pengali keterlihatan, batas sumber, lampu real-time 6, bayangan 1, dan struktur cahaya 24, aturan cuaca, kedip, tarikan zombie, dan ambang cahaya Zombie Lumut, interiorMaxRealtimeLights 3 |
| `flashlight` | #98 | maxBattery 100, drainPerSec 0,2, batteryRecharge 100 |
| `rescue` | #14 | engineWarmupSeconds 120, engineNoiseRadius 60, engineNoiseIntervalSeconds 5, warmupPanicWaveSize 15, targetNightWaveSize 25, abortDistance 40 |
| `deathCinematic` | #53 | ragdollForce 300, explosionForceMultiplier 3, sinkDrag 4, slowMoScale 0,35, slowMoSeconds 1,2, cameraRiseSeconds 1,5, cameraHeight 2,5, cameraDistance 3, orbitDegreesPerSecond 12, holdSeconds 3,5, collapseHoldSeconds 5, fadeSeconds 1,5, messageSeconds 2, skipAfterSeconds 2 |
| `stats` | #57 | flushIntervalSeconds 60 |
| `narrative` | #99 | beatFadeSeconds 0,5, musicFadeSeconds 1, skipHintSeconds 3, watchdogExtraSeconds 10 |
| `safety` | #116 | unstuckRingSize 12, unstuckSampleSeconds 5, unstuckNavSampleRadius 2, unstuckMinRewindDistance 3, unstuckChannelSeconds 5, unstuckCooldownSeconds 180, stuckHintSeconds 20, minStonePickupsRegion1 4, woodPerSecondEstimate 0,4, regionTravelSeconds 300, partRoundTripSeconds 240, estimateMargin 1,5 |
| `animation` | #12 | locomotionDampSeconds 0,1; enemySpeedSampleSeconds 0,1; minReloadAnimSpeed 0,5; maxReloadAnimSpeed 2 |
| `vfx` | #15 | catalog (`VFXEntry[]`: poolTag, lifetimeSeconds, maxActive, cullDistance, minIntervalSeconds); lowQualityDensity 0,5; mediumQualityDensity 0,75 |
| `tutorial` | #16 | displaySeconds 6; minGapSeconds 3; dangerHoldSeconds 4; minSeenSeconds 2; queueLimit 4; lowVitalThreshold 50 |
| `settings` | #19 | sensitivityMin 0,02; sensitivityMax 0,5; fovMin 60; fovMax 100; brightnessMin 0,8; brightnessMax 1,2 |
| `footstep` | #21 | stepDistance 1,75; minSpeed 0,5; baseVolume 0,6; crouchVolumeMultiplier 0,4; sprintSpeedThreshold 6; sprintVolumeMultiplier 1,3; heavyVolumeMultiplier 1,2; pitchMin/pitchMax 0,93/1,07; volumeJitter 0,1; minLandingAirSeconds 0,25; landingFullFallSpeed 12 |
| `notification` | #22 | maxVisible 4; queueCapacity 12; infoSeconds 3; warningSeconds 5; fadeSeconds 0,3; mergeWindowSeconds 2; logCapacity 100 |
| `postProcess` | #23 | lowHpStartRatio 0,4; lowHpMaxVignette 0,5; lowHpPulseHz 1,2; lowHpPulseAmount 0,08; woundVignette 0,25; hitPulseAmount 0,2; hitPulseDecayPerSec 1; freezeVignette 0,4; breathVignette 0,55; breathStartRatio 0,4; sleepVignette 0,6; infectionSaturationMin -35; underwaterBlurStart 0; underwaterBlurEnd 8; changePerSec 1,5; underwaterChangePerSec 6; deathDesaturateSeconds 2 |
| `shake` | #24 | degreesPerUnit 2; metersPerUnit 0,02; rollPerUnit 1; maxDegrees 8; maxMeters 0,12; hitReferenceDamage 25; hitScaleMin/hitScaleMax 0,5/1,5. Preset per jenis berada di aset `CameraShakeProfile` |
| `combatFeedback` | #51 | Batas dan lama indikator damage, kecerahan, penanda serangan, lama Hit Marker, skala headshot, kedipan Kill Marker, dan jeda suara Tink |
| `ambientAudio` | #52 | Lama crossfade, jeda keluar area, transisi siang dan malam, low-pass bawah air, peredaman saat tidur, dan fade saat run berakhir |
| `combatMusic` | #114 | Ambang tingkat musik, hysteresis, lama tahan dan fade, radius musuh dekat, skor damage, batas suara musuh, oklusi, suara kerumunan, peredaman saat bencana, dan jeda peringatan |
| `lore` | #100 | maxPartHintsPerRun 2 |
| `barks` | #100 | checkSeconds 2, globalCooldownSeconds 6, npcCooldownSeconds 20, categoryCooldownSeconds 90, hearRadius 25, greetRadius 6, greetAwaySeconds 120, hurtHpRatio 0,4, zombieAlertRadius 15 |
| `localization` | #104 | textScaleMin 0,85, textScaleMax 1,5, subtitleMinSeconds 1,5, subtitleSecondsPerChar 0,06, subtitleMaxRows 3, maxFlashesPerSecond 3 |

## 3. Hubungan dengan Sistem Lain

- **Tidak mengirim atau mendengar event.** Config hanya dibaca.
- **`DayNightDirector`, `SurvivalStats`, `ResourceNode`, `DurabilitySystem`, `IslandPlayerController`, `WeatherManager`, dan `RegionGateSystem` (#68):** membaca angka dari config, bukan variabel `public` sendiri.
- **#27 `AIDirector`:** mengatur kurva zombie per hari di bagian config-nya sendiri.
- **#56 `DevConsole`:** perintah `balance <bagian>.<nama> <nilai>` mengubah angka saat playtest.

## 4. Kasus Khusus

- **Perubahan aset saat bermain:** di Editor, ScriptableObject yang diubah saat Play Mode tetap tersimpan. `DevConsole` mengubah salinan sementara yang dibuat `Instantiate` saat run mulai.
- **Pool:** objek pool membaca angka saat `OnEnable`, jadi perubahan lewat konsol berlaku pada kemunculan berikutnya.
- **Game dijeda:** config tidak berubah.
- **Permadeath dan save:** config tidak disimpan. Save hanya mencatat nama profil kesulitan.
- **Tidur:** tidur mempercepat `timeMultiplier` lewat `SleepSystem` (#67), tanpa mengubah nilai dasar di config.

## 5. Keputusan Desain

**Keputusan developer:** `bridgeWoodCost` bernilai awal 500. Pengali `aiDirector.postTarget*` setelah hari ke-60 (`AIDirector.md`). Laju `vitals.infectionDamagePerSec` 0,3 saat `Infection` 100 (`VitalsRules.md`). Pemicu `objective.chapter2TriggerDay` dan `objective.chapter3TriggerRegion` (`ObjectiveManager.md`). Satu Lucky Machine di setiap region, dengan angka `luckyMachine` yang sama untuk keempat mesin (`LuckyMachine.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

- Panjang satu hari game 15 menit, dan total 15 jam.
- Hasil panen 8 sampai 12 kayu per pohon.
- `frostbiteDamagePerSec` diturunkan dari 5 ke 1.
- Jumlah suku cadang 5.
- Semua nilai lain di tabel angka inti.

## 6. Skenario Tes

1. Jalankan satu hari penuh. **Hasil:** berlangsung sekitar 900 detik, dan `DayChangedEvent` terkirim sekali.
2. Biarkan lapar turun dari 100 tanpa makan. **Hasil:** nol setelah sekitar 900 detik.
3. Tebang pohon sampai 500 kayu dan catat waktunya. **Hasil:** sekitar 3 siang penuh. Catat selisihnya di lembar playtest.
4. Angkut satu suku cadang dari Region 4 ke Region 3 tanpa pakaian hangat. **Hasil:** pemain sampai dengan HP berkurang sekitar 55.
5. Ubah `timeMultiplier` lewat `DevConsole`. **Hasil:** jam berubah, dan aset asli tidak ikut berubah.
