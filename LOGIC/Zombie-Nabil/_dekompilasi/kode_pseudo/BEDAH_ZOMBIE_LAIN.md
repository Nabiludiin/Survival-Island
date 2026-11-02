# Bedah Kode Zombie Lain (server.dll)

Hunter, Smoker, Boomer, Witch, dan zombie biasa. Lokasi seperti `bagian_004.c:53369` mengacu ke folder `server\`.

**Yang sudah pasti:** nama pengaturan, nilai bawaan di DLL, teks log, dan pengali yang terbaca di kode.
**Yang masih perkiraan:** nama peran fungsi dan alur bertanda "(perkiraan)", karena nama aslinya hilang.
Belum ada yang diuji di dalam game. Bagian Tank ada di `BEDAH_TANK.md`.

## Perbandingan cepat

| Zombie | Darah | Kecepatan | Serangan utama |
|---|---|---|---|
| Biasa | `z_health` 50 | `z_speed` 250 | Pukulan jarak dekat |
| Hunter | `z_hunter_health` 250 | `z_hunter_speed` 300 | Terkam, cakar 5 per 0,5 dtk |
| Smoker | `z_gas_health` (nilainya belum dibaca) | `z_gas_speed` (nilainya belum dibaca) | Lidah jarak 750, cekik 10 per dtk |
| Boomer | `z_exploding_health` 50 | `z_exploding_speed` 175 | Muntah jarak 300, ledakan radius 200 |
| Witch | `z_witch_health` 1000 | `z_witch_speed` 300 | Cakar, dengan damage tergantung kesulitan |

## 1. Hunter

Kelas: `Hunter`, `HunterAttack`, `HunterBehavior`, `HunterLeapToCover`, `HunterLungeAtVictim`, `HunterIntention`, `HunterLocomotion`, `CLunge`, `CHunterClaw`.

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `10297050` | bagian_004.c:42633 | Memulai terkaman. Log: `starting a pounce`. `z_pounce_damage_interrupt` (50) kemungkinan batas damage untuk membatalkan terkaman |
| `102a9020` | bagian_004.c:53369 | Saat mendarat di korban: guncangan layar, event, dan bonus jarak |
| `102b4f70` | bagian_004.c:59063 | Mencakar korban berkala dan mengatur suara Hunter |
| `10318700` | bagian_005.c:5828 | Melompat menjauh ke tempat berlindung (perkiraan `HunterLeapToCover`) |

### Damage terkaman

```
setiap z_pounce_damage_interval (0,5 dtk):
    damage = z_pounce_damage (5)
    jika Hard:   damage * 2
    jika Expert: damage * 3
```

### Bonus jarak terkaman

Hanya berjalan kalau `z_lunge_distance_damage` menyala, dan nilai bawaannya **0** (mati) di DLL.

```
jarak <= z_pounce_damage_range_min (500): tidak ada bonus
jarak >= z_pounce_damage_range_max (1000): bonus penuh z_hunter_max_pounce_bonus_damage (24)
di antaranya: naik linear
di versus: tambahan dasar 1
```

### Keputusan melompat menjauh

- Kalau korban lebih jauh dari `hunter_leap_away_give_up_range` (1000): batal, log `Far away`.
- Kalau lebih dekat dari `hunter_committed_attack_range` (75): menyerang, log `Very close - attack!`.
- Sudut lompat = jarak x `hunter_pounce_loft_rate` (0,035), maksimum `hunter_pounce_max_loft_angle` (45).

## 2. Smoker

Kelas: `Smoker`, `SmokerAttack`, `SmokerBehavior`, `SmokerMoveToAttackPosition`, `SmokerRetreatToCover`, `SmokerTongueVictim`, `SmokerIntention`, `SmokerLocomotion`, `SmokerPathCost`, `CTongue`, `CSmokerClaw`. Pengaturannya memakai nama lama `z_gas_*`.

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `102cc940` | bagian_004.c:72751 | Mencari target lidah dalam jarak `tongue_range` (750). Kerucut bidik 0,1, atau 0,015 di versus |
| `102b2c70` | bagian_004.c:57705 | Damage pada korban yang ditarik atau dicekik |
| `102cc170` | bagian_004.c:72362 | Cek tarikan yang tidak maju: gerak kurang dari `tongue_no_progress_tolerance` (25), lalu cekik lebih awal |
| `102ca0c0` | bagian_004.c:71193 | Melepas lidah kalau Smoker terkena damage terlalu banyak |

### Damage lidah

```
setiap tongue_choke_damage_interval (1 dtk):
    saat ditarik:  tongue_drag_damage_amount (3)
    saat dicekik:  tongue_choke_damage_amount (10), Hard x 2, Expert x 3
```

Pengali kesulitan terbaca di jalur cekik; untuk tarikan belum saya pastikan.

### Melepas lidah

Kalau darah Smoker turun lebih dari `tongue_break_from_damage_amount` (50) sejak menggenggam, lidah dilepas dengan log `Tongue letting go for taking too much damage.`. Kalau `tongue_kill_smoker_on_detach` menyala (bawaan 1), Smoker kemungkinan ikut mati (perkiraan).

Pengaturan lain: `tongue_fly_speed` 1000, `tongue_victim_max_speed` 175, `tongue_hit_delay` 20, `tongue_miss_delay` 15, `smoker_tongue_delay` 1,5, `tongue_health` 100.

## 3. Boomer

Kelas: `Boomer`, `BoomerAttack`, `BoomerBehavior`, `BoomerHide`, `BoomerRetreatToCover`, `BoomerVomitOnVictim`, `BoomerFilter`, `BoomerIntention`, `BoomerLocomotion`, `CVomit`, `CBoomerClaw`.

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `1030f4e0` | bagian_005.c:794 | AI serang (perkiraan `BoomerAttack`) |
| `10310620` | bagian_005.c:1332 | Awal muntah (perkiraan `BoomerVomitOnVictim`): jeda tergantung kesulitan, suara `BoomerZombie.Warn` |
| `103102d0` | bagian_005.c:1188 | Saat muntah: log `Lost my vomit target` dan `Vomit completed` |
| `1028f1f0` | bagian_004.c:38251 | Efek terkena muntah pada pemain: layar tersentak acak dan pudar |
| `1029ab10` | bagian_004.c:45052 | Fungsi kematian umum yang besar, memakai pengaturan ledakan Boomer (perkiraan) |

### Jeda sebelum muntah

| Kesulitan | Jeda (`boomer_vomit_delay` = 1) |
|---|---|
| Easy | 2 dtk |
| Normal | 1 dtk |
| Hard | 0,2 dtk |
| Expert | 0 dtk |

### Keputusan AI

Dari teks log `1030f4e0`, Boomer:
- Muntah dengan log `Launching vomit attack!` kalau korban dalam `z_vomit_range` (300).
- Bisa kabur dengan log `I'm exposed and my vomit is recharging`.
- Bisa kabur kalau terlalu jauh, yaitu lebih dari jangkauan + 250, dengan log `I'm exposed and too far away`.
- Menunggu kalau Director menahan, dengan log `The Director says its relax time`.

### Ledakan

`z_exploding_inner_radius` 130, `z_exploding_outer_radius` 200, `z_exploding_force` 5000, `z_exploding_splat` 50, `z_exploding_splat_radius` 200.

## 4. Witch

Kelas: `Witch`, `WitchIdle`, `WitchAngry`, `WitchAttack`, `WitchKillIncapVictim`, `WitchRetreat`, `WitchBurn`, `WitchDying`, `WitchBehavior`, `WitchBody`, `WitchVision`, `WitchIntention`, `WitchLocomotion`, `WitchExecAction`, `WitchSlashEnumerator`.

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `10334b60` | bagian_005.c:21544 | Apakah pemain mengganggu Witch |
| `103359d0` | bagian_005.c:22011 | Menaikkan amarah |
| `10220970` | bagian_003.c:69493 | Damage per target (perkiraan: damage cakar Witch) |
| `103370d0` | bagian_005.c:22640 | Reaksi saat diserang: bisa ganti korban kalau `z_witch_allow_change_victim` menyala |
| `10337e40` | bagian_005.c:23146 | Terbakar: setelah `z_witch_burn_time` (15 dtk), damage 9999,9 |
| `10339440` | bagian_005.c:23658 | Mundur: minimum 10 dtk, selesai kalau jauh 1000 atau tak terlihat 10 dtk. Suara `WitchZombie.RetreatHorror` |
| `10334a40` | bagian_005.c:21490 | Update Witch, termasuk melepas lidah Smoker: `Tongue letting go for a witch cutting it.` |

### Yang membuat Witch marah

Fungsi `10334b60` menganggap pemain mengganggu kalau:
- Senter menyala, dalam `z_witch_flashlight_range` (400), dan Witch melihatnya.
- Atau pemain dalam `z_witch_personal_space` (100).
- Atau ada kondisi lain dari `FUN_10282950` yang belum saya lacak.

Amarah dari ancaman langsung naik sebesar `z_witch_anger_rate` (0,2) x `z_witch_hostile_at_me_anger` (2) x waktu frame, jadi sekitar 0,4 per detik. Penurunannya kemungkinan memakai `z_witch_relax_rate` (0,05), tetapi pemakainya tidak tertangkap.

### Damage (perkiraan)

| Target | Easy | Normal | Hard | Expert |
|---|---|---|---|---|
| Survivor berdiri | 24 | 24 | 33 | 100 |
| Survivor sudah jatuh | 75 | 75 | 75 | 150 |
| Infected pemain | 250 | 250 | 250 | 250 |
| Entitas bukan pemain, seperti zombie biasa | 1000 | 1000 | 1000 | 1000 |

Survivor di versus memakai pengaturan lain yang namanya belum dilacak. `z_witch_damage` (100) ada di DLL, tetapi pemakainya tidak tertangkap.

## 5. Zombie biasa dan gelombang mob

Kelas: `Infected`, `InfectedAttack`, `InfectedAlert`, `InfectedWander`, `InfectedBehavior`, `InfectedBurn`, `InfectedShoved`, `InfectedStaggerAround`, `InfectedDying`, `InfectedChasePath`, `InfectedSitDown`, `InfectedLieDown`, `InfectedLeanOnWall`, `InfectedBlinded`, dan lainnya.

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `10324c60` | bagian_005.c:12760 | `Infected::Update` |
| `10333970` | bagian_005.c:20938 | `InfectedWander::Update` |
| `1032bf40` | bagian_005.c:17028 | `InfectedAlert::Update` |
| `1032efc0` | bagian_005.c:18634 | `InfectedAttack::ChaseVictim::Update`: mengejar, dan bisa terpancing `pipe_bomb*` |
| `1032e710` | bagian_005.c:18297 | `InfectedAttack::PunchVictim::Update`: memukul dalam `z_attack_max_range` (20), juga mengenai pintu |
| `10224810` | bagian_003.c:72426 | Menentukan waktu mob berikutnya |
| `102c0d30` | bagian_004.c:66525 | Meminta mob, log `SpawnMob requested of size` |
| `10225400` | bagian_003.c:73052 | `Director::StartPanicEvent` |

### Waktu dan ukuran mob

Waktu mob dipilih acak di antara nilai minimum dan maksimum:

| Kesulitan | Minimum | Maksimum |
|---|---|---|
| Easy | 120 dtk | 240 dtk |
| Normal | 90 dtk | 180 dtk |
| Hard | 90 dtk | 180 dtk |
| Expert | 90 dtk | 180 dtk |

Ukuran awal `z_mob_spawn_min_size` (10), maksimum `z_mob_spawn_max_size` (30). Laju isi ulang `z_mob_recharge_rate` (0,0025). Mega mob `z_mega_mob_size` (50), dengan jeda 420 sampai 900 dtk. Batas zombie sekaligus `z_common_limit` (30).

Pengaturan pemain: `z_hit_from_behind_factor` (0,5), `z_attack_incapacitated_damage` (10), dan `z_hit_incap_factor_easy` (0,1).

## Celah analisis

- Banyak pengaturan punya pemakai `0` di tabel karena kode pemakainya tidak dikenali Ghidra sebagai fungsi. Pengaturan itu tetap ada di DLL.
- Fungsi besar seperti `1029ab10` hanya saya baca teks dan pengaturannya, belum seluruh alurnya.

## Semua pengaturan yang ditemukan

Ini nilai bawaan di DLL; nilai saat bermain bisa berbeda kalau diubah file konfigurasi. Sebagian mungkin butuh `sv_cheats`, karena flag-nya belum saya periksa.

| Pengaturan | Default | Keterangan dari DLL | Fungsi pemakai |
|---|---|---|---|
| `boomer_exposed_time_tolerance` | 1.0 | How long an out-of-range Boomer will tolerate being visible before fleeing | `1030f4e0` |
| `boomer_pz_claw_dmg` | 4 | Amount of damage done by a PZ boomer's regular melee attack |  |
| `boomer_vomit_delay` | 1 | How long the Boomer waits before he vomits on his target on Normal difficulty | `10310620` |
| `hunter_committed_attack_range` | 75 |  | `10318700` |
| `hunter_leap_away_give_up_range` | 1000 |  | `10318700` |
| `hunter_pounce_air_speed` | 700 |  |  |
| `hunter_pounce_loft_rate` | .035 | Hunter adds this angle/distance when pouncing | `10318700` `10318be0` |
| `hunter_pounce_max_loft_angle` | 45 | Maximum loft angle for Hunter Pounce angle adjustment | `10318700` `10318be0` |
| `hunter_pounce_ready_range` | 500 | If victim is this close, crouch and get ready |  |
| `hunter_pz_claw_dmg` | 6 | Amount of damage done by a PZ hunter's regular melee attack |  |
| `smoker_pz_claw_dmg` | 4 | Amount of damage done by a PZ smoker's regular melee attack |  |
| `smoker_tongue_delay` | 1.5 |  | `10314f90` |
| `tongue_allow_voluntary_release` | 0 | Can a Smoker let go with his tongue by clicking or turning away? | `102cddd0` |
| `tongue_bend_point_deflection` | 5 | How far off the first obstacle the tongue bends. | `102ca8d0` |
| `tongue_bend_point_needs_LOS` | 0 | Does a bent tongue still need LOS from the bend point? |  |
| `tongue_break_from_damage_amount` | 50 | How much damage to the smoker makes him let go of his victim. | `102ca0c0` |
| `tongue_bullet_radius` | 6 |  | `1033dad0` `10346b90` |
| `tongue_choke_damage_amount` | 10 | How much damage the choke does. | `102b2c70` |
| `tongue_choke_damage_interval` | 1 | How often the choke does damage. | `102b2c70` `102cb220` `102cb820` |
| `tongue_cone_start_tolerance` | .1 | How wide the cone is for a tongue hit. | `102cc940` |
| `tongue_debug` | 0 | Print debug info for tongue | `102ca2a0` `102ca3a0` `102ca8d0` +4 |
| `tongue_drag_damage_amount` | 3 | How much damage the tongue drag does. | `102b2c70` |
| `tongue_dropping_to_ground_time` | 2 | A miss or a wall hit will wait this long before pulling back. |  |
| `tongue_fly_speed` | 1000 | How fast a tongue flies through the air. | `102cced0` |
| `tongue_force_break` | 0 | Force an existing attached tongue to break, for debugging | `102cddd0` |
| `tongue_health` | 100 | Tongue health | `102cd880` |
| `tongue_hit_delay` | 20 | env_rock_launcher | `102c8ad0` `102cd880` |
| `tongue_kill_smoker_on_detach` | 1 | Do we kill the smoker when they take enough damage to detach the tongue? | `102ca0c0` |
| `tongue_los_forgiveness_time` | 1.0 | A traveling tongue can lose LOS for this amount of time and still hit. |  |
| `tongue_miss_delay` | 15 | How long a smoker must wait to shoot his tongue after a miss. | `102c8ad0` `102cced0` |
| `tongue_no_progress_break_interval` | 10 | How long of the victim making no progress until we break the tongue. | `102cc170` |
| `tongue_no_progress_choke_early_ambush_delay` | 0.5 | Use a smaller delay if the smoker started choking us from behind. So this plus TongueNoProgressChokeTime equals the earliest start of a ground choke in the event of an ambush. | `102cddd0` |
| `tongue_no_progress_choke_early_delay` | 1.5 | We won't think about ground choking for this long after the hit. So this plus TongueNoProgressChokeTime equals the earliest start of a ground choke. | `102cddd0` |
| `tongue_no_progress_choke_time` | .5 | If our victim doesn't make tongue_no_progress_tolerance progress towards in this time, start to hurt him. | `102cc170` |
| `tongue_no_progress_damage_interval` | .5 | How long of the victim making no progress until we start choking him. | `102cc170` |
| `tongue_no_progress_tolerance` | 25 | If our victim doesn't make this much progress in tongue_no_progress_release_time, start to hurt him. | `102cc170` |
| `tongue_player_dropping_to_ground_time` | 1 | How long after the tongue disconnects will a player need to wait. |  |
| `tongue_range` | 750 | How far a smoker can shoot his tongue. | `102cc940` `102efb00` `10314190` +1 |
| `tongue_release_fatigue_penalty` | 2500 | How much fatigue the victim gets when released, to slow him down. | `102cc4d0` |
| `tongue_start_pull_delay` | 0.1 | How long from tongue contact to tongue pulling. | `102c8ed0` |
| `tongue_unbend` | 1 | Can the smoker tongue unbend? | `102cabf0` |
| `tongue_vertical_choke_dot` | 0 | DotProduct between tongue and vertical required to start choking. | `102cbef0` |
| `tongue_vertical_choke_height` | 40 | Need to have victim this high off ground to choke him. | `102cbef0` |
| `tongue_vertical_choke_time_off_ground` | .5 | Need to have victim off ground for this long to choke him. | `102cbef0` |
| `tongue_victim_acceleration` | 30 | Acceleration while tongued. | `102c8ed0` |
| `tongue_victim_accuracy_penalty` | 0.133 | How much someone's accuracy suffers while being dragged by a tongue. | `103501f0` |
| `tongue_victim_max_speed` | 175 | The fastest the tongue can get you going. | `1025b000` `102c8ed0` |
| `tongue_vs_cone_start_tolerance` | 0.015 | How wide the cone is for a tongue hit in versus. | `102cc940` |
| `z_attack_change_target_range` | 100 |  | `1032efc0` |
| `z_attack_flow_range` | 1500 |  | `1030f4e0` |
| `z_attack_incapacitated_damage` | 10 |  | `1021f360` |
| `z_attack_interval` | 1 |  | `1032efc0` |
| `z_attack_max_range` | 20 |  | `1032d7b0` `1032e710` |
| `z_attack_min_range` | 10 |  | `1032efc0` |
| `z_attack_movement_penalty` | 0.5 |  |  |
| `z_attack_on_the_run_range` | 35 |  | `1032efc0` |
| `z_boomer_near_dist` | 180 |  | `102a0700` `102a4840` |
| `z_burn_max` | 0.85 | Max amount for the burn effect | `102b5c80` |
| `z_burn_rate` | 0.01 | How fast the burn effect grows on burning players | `102b5c80` |
| `z_burning_lifetime` | 30 | Number of seconds a burning zombie takes to crisp |  |
| `z_common_limit` | 30 | How many common infecteds we can have at once. | `10233660` `10275590` `102bf660` +3 |
| `z_exploding_force` | 5000 |  | `1029ab10` |
| `z_exploding_health` | 50 | Exploding Zombie max health | `10297f50` |
| `z_exploding_inner_radius` | 130 |  | `1029ab10` |
| `z_exploding_limit` | 1 |  | `10227e10` `10298920` |
| `z_exploding_outer_radius` | 200 |  | `1029ab10` `102e75f0` `102ef980` +2 |
| `z_exploding_shove_interval` | 10 |  | `102b38d0` |
| `z_exploding_shove_max` | 5 |  | `102a4840` `102b38d0` |
| `z_exploding_shove_min` | 4 |  | `102a4840` `102b38d0` |
| `z_exploding_speed` | 175 |  | `1025b000` |
| `z_exploding_splat` | 50 | Boomers splat instead of making a thud when taking this much falling damage | `1028eea0` |
| `z_exploding_splat_radius` | 200 | Boomers splattering on the ground make survivors inside this radius it | `1029ab10` |
| `z_gun_damage` | 10 |  | `10354130` |
| `z_gun_debug_player_index` | 0 |  | `1033a9d0` `10350070` |
| `z_gun_force` | 300 |  | `10354130` |
| `z_gun_horiz_punch` | 0 | Toggles horizontal punchangles for guns | `10351b60` |
| `z_gun_kick` | 0 | Firing a gun can knock the player's view this amount of the current spread |  |
| `z_gun_physics_force` | 25 | How hard we push things that aren't players or doors. | `10354130` |
| `z_gun_range` | 75 |  | `10352740` |
| `z_gun_start_empty` | 0 |  | `10350730` `10350ec0` |
| `z_gun_stun_duration` | 1.25 |  | `10354130` |
| `z_gun_survivor_force` | 200 |  | `10354130` |
| `z_gun_survivor_friend_push` | 1 | Allow survivors to push survivors with a right click. | `10354130` |
| `z_gun_survivor_shove_dot` | 0.95 | Max dot of a shove swing that can push a fellow survivor | `10357390` |
| `z_gun_swing_duration` | 0.2 |  | `10352740` |
| `z_gun_swing_interval` | 0.7 |  | `10352740` |
| `z_gun_vertical_punch` | 1 | Toggles vertical punchangles for guns | `10351b60` |
| `z_health` | 50 | Zombie max health | `10323cb0` |
| `z_hit_from_behind_cosine` | 0.0 |  | `1029a200` |
| `z_hit_from_behind_factor` | 0.5 | Damage multiplier if zombie hits Survivor from behind | `1029a200` |
| `z_hit_incap_factor_easy` | 0.1 | Damage multiplier if zombie hits incapacitated Survivor | `1029a200` |
| `z_hit_incap_factor_expert` | 1.0 | Damage multiplier if zombie hits incapacitated Survivor | `1029a200` |
| `z_hit_incap_factor_hard` | 1.0 | Damage multiplier if zombie hits incapacitated Survivor | `1029a200` |
| `z_hit_incap_factor_normal` | 1.0 | Damage multiplier if zombie hits incapacitated Survivor | `1029a200` |
| `z_hunter_claw_buildup_growl_percent` | .5 | When at least this charged up, Hunter starts growling constantly. |  |
| `z_hunter_claw_force` | 240 | Force with which the claw shoves other zombies away |  |
| `z_hunter_claw_max_buildup_time` | 3 | Attack button held longer than this gets no extra benefit. |  |
| `z_hunter_claw_plays_hit_anims` | 0 | Play the new hit anims for debugging? |  |
| `z_hunter_claw_range` | 52 | Range of the Claw weapon |  |
| `z_hunter_claw_range_down` | 70 | Range of the Claw weapon when looking straight down, so it can reach your feet from your eyes. |  |
| `z_hunter_claw_start_buildup_time` | .5 | Attack button released in less than this time is just a click. |  |
| `z_hunter_claw_swing_duration` | 0.2 |  |  |
| `z_hunter_claw_swing_interval` | 1 |  |  |
| `z_hunter_claw_swing_miss_interval` | 0.5 |  |  |
| `z_hunter_ground_normal` | 0.2 |  | `102630a0` |
| `z_hunter_health` | 250 | Zombie max health | `10297f50` |
| `z_hunter_limit` | 1 |  | `10227e10` `10298920` |
| `z_hunter_lunge_distance` | ? | Distance at which bot hunters will try to lunge |  |
| `z_hunter_lunge_pitch` | 25 |  |  |
| `z_hunter_lunge_stagger_time` | 1 |  |  |
| `z_hunter_max_pounce_bonus_damage` | 24 |  | `102a9020` |
| `z_hunter_speed` | 300 |  |  |
| `z_lunge_cooldown` | 0.1 | Cooldown after lunge where zombies can't attack |  |
| `z_lunge_distance_damage` | 0 | Does the hunter do damage based on the distance of the pounce? | `102a9020` |
| `z_lunge_interval` | 0.1 |  |  |
| `z_lunge_power` | 600 |  |  |
| `z_lunge_reflect` | 0 | Reflects wall-kick lunges |  |
| `z_lunge_release` | 0 | Whether the hunter can release from a pounced survivor | `102c5040` |
| `z_lunge_specials` | 0 | Zombies can shove special zombies with a lunge. | `1025aed0` |
| `z_lunge_up` | 200 |  | `10265160` |
| `z_max_hunter_pounce_stagger_duration` | 0.5 | Max time a PZ staggers when bashed by a survivor that was pounced by said hunter. | `1029cc20` |
| `z_max_neighbor_range` | 200 | Max range for neighbor collection for avoidance | `10322aa0` |
| `z_max_path_length` | 5000 |  |  |
| `z_max_stagger_duration` | 6 | Max time a PZ staggers when bashed by a survivor. | `1029cc20` `1029da00` `1029df50` +1 |
| `z_max_survivor_damage` | 100 |  | `10290960` |
| `z_mega_mob_size` | 50 |  | `1022cd20` `1022ecc0` `10237000` +1 |
| `z_mega_mob_spawn_max_interval` | 900 |  |  |
| `z_mega_mob_spawn_min_interval` | 420 |  |  |
| `z_mob_min_notify_count` | 10 | Mobs at least this large trigger 'mob' events | `102c0d30` |
| `z_mob_music_size` | 2 | Spotting a mob this large plays music | `1024b400` `102b69d0` |
| `z_mob_population_density` | 0.0064 | Density of mob spawns, per square inch (0.0064 = 4 per 1x1 nav area) | `102c28d0` `102c3800` |
| `z_mob_recharge_rate` | 0.0025 |  | `10235ff0` |
| `z_mob_sacrifice_timeout` | 5 |  |  |
| `z_mob_spawn_finale_size` | 20 |  |  |
| `z_mob_spawn_max_interval_easy` | 240 |  | `10224810` `10230dc0` |
| `z_mob_spawn_max_interval_expert` | 180 |  | `10224810` `10230dc0` |
| `z_mob_spawn_max_interval_hard` | 180 |  | `10224810` `10230dc0` |
| `z_mob_spawn_max_interval_normal` | 180 |  | `10224810` `10230dc0` |
| `z_mob_spawn_max_size` | 30 |  | `10223190` `1022a040` `10233660` +3 |
| `z_mob_spawn_min_interval_easy` | 120 |  | `10224810` `10230dc0` |
| `z_mob_spawn_min_interval_expert` | 90 |  | `10224810` `10230dc0` |
| `z_mob_spawn_min_interval_hard` | 90 |  | `10224810` `10230dc0` |
| `z_mob_spawn_min_interval_normal` | 90 |  | `10224810` `10230dc0` |
| `z_mob_spawn_min_size` | 10 |  | `10224810` |
| `z_pounce_allow_partial_hidden` | 1 |  |  |
| `z_pounce_crouch_delay` | 1 |  |  |
| `z_pounce_damage` | 5 |  | `102b4f70` |
| `z_pounce_damage_delay` | 1 |  | `10293650` `102a9020` |
| `z_pounce_damage_interrupt` | 50 | Taking this much damage interrupts a pounce attempt | `10297050` |
| `z_pounce_damage_interval` | 0.5 |  | `102b4f70` |
| `z_pounce_damage_range_max` | 1000 |  | `102a9020` |
| `z_pounce_damage_range_min` | 500 |  | `102a9020` |
| `z_pounce_delay` | 0 |  | `1029e500` |
| `z_pounce_door_damage` | 500 |  | `102c55a0` |
| `z_pounce_shake_amplitude` | 4 |  | `102a9020` |
| `z_pounce_shake_duration` | 1.5 |  | `102a9020` |
| `z_pounce_shake_radius` | 250 |  | `102a9020` |
| `z_pounce_silence_range` | 500 |  | `102b4f70` |
| `z_pounce_stumble_force` | 5 | Force of the stumble effect when a hunter pounces on someone | `1029da00` |
| `z_pounce_stumble_radius` | 0 | Radius of the stumble effect when a hunter pounces on someone | `1029e590` `102a9020` |
| `z_run_exertion_interval` | 1 |  | `102b3110` |
| `z_shove_friend_speed` | 130 |  | `1032b960` |
| `z_speed` | 250 |  | `1025b000` `1031e8a0` |
| `z_vomit` | 1 |  |  |
| `z_vomit_boxsize` | 1.0 | Size of vomit damage entities. | `102cfd50` |
| `z_vomit_debug` | 0 | Visualize the vomit damage. |  |
| `z_vomit_drag` | 0.89 | Air drag of vomit damage entities. |  |
| `z_vomit_duration` | 1.5 | How long a puker continuously pukes for. |  |
| `z_vomit_fade_duration` | 5 | How long the fade takes |  |
| `z_vomit_fade_start` | 5 | When the vomit starts to fade away | `1028f1f0` |
| `z_vomit_fatigue` | 3000 | Stamina impact of puking. High number will pin in place for a long time, lower will just slow. |  |
| `z_vomit_float` | -130.0 | Upward float velocity of vomit damage entities. |  |
| `z_vomit_hit_pitch_max` | 15 |  | `1028f1f0` |
| `z_vomit_hit_pitch_min` | -15 |  | `1028f1f0` |
| `z_vomit_hit_yaw_max` | 10 |  | `1028f1f0` |
| `z_vomit_hit_yaw_min` | -10 |  | `1028f1f0` |
| `z_vomit_lifetime` | 0.5 | Time to live of vomit damage entities. | `102cfd50` |
| `z_vomit_maxdamagedist` | 350.0 | Maximum damage distance for vomit. |  |
| `z_vomit_range` | 300 |  | `102ef980` `1030f4e0` `103102d0` |
| `z_vomit_slide_mult` | 0.5 | Multiplier for second texture slide rate |  |
| `z_vomit_slide_rate` | 0.1 | Percentage of screen height per second |  |
| `z_vomit_vecrand` | 0.05 | Random vector added to initial velocity of vomit damage entities. | `102cea00` |
| `z_vomit_velocity` | 1700.0 | Initial velocity of vomit damage entities. | `102cea00` |
| `z_vomit_velocityfadeend` | .5 | Time at which attacker's velocity contribution finishes fading. |  |
| `z_vomit_velocityfadestart` | .3 | Time at which attacker's velocity contribution starts to fade. |  |
| `z_walk_speed` | 85 |  | `1025c970` |
| `z_wandering_density` | 0.03 |  | `102c2e30` |
| `z_witch_allow_change_victim` | 1 |  | `103370d0` |
| `z_witch_anger_rate` | 0.2 |  | `103359d0` |
| `z_witch_attack_range` | 60 |  | `10334490` `10336c10` |
| `z_witch_berserk_range` | 200 |  |  |
| `z_witch_burn_time` | 15 |  | `10337e40` |
| `z_witch_damage` | 100 |  |  |
| `z_witch_damage_per_kill_hit` | 30 |  |  |
| `z_witch_discard_range` | 2000 |  |  |
| `z_witch_flashlight_range` | 400 |  | `10334b60` |
| `z_witch_health` | 1000 | Witch max health | `10220970` `10334d80` |
| `z_witch_hostile_at_me_anger` | 2 |  | `103359d0` |
| `z_witch_max_retreat_range` | 2000 |  | `103391a0` |
| `z_witch_max_threat_time` | 7 |  |  |
| `z_witch_min_threat_time` | 5 |  |  |
| `z_witch_personal_space` | 100 |  | `10334b60` |
| `z_witch_relax_rate` | 0.05 |  |  |
| `z_witch_retreat_exit_hidden_duration` | 10 |  | `10339440` |
| `z_witch_retreat_exit_range` | 1000 |  | `10339440` |
| `z_witch_retreat_min_duration` | 10 |  | `10339440` |
| `z_witch_speed` | 300 |  |  |
| `z_witch_speed_inured` | 200 |  | `10337300` `103374a0` |
| `z_witch_threat_hostile_range` | 600 |  | `10303410` |
| `z_witch_threat_normal_range` | 300 |  | `10334a40` |
| `z_witch_tongue_range` | 100 | If a tongue passes this close to a witch's head, she will snap it | `10334050` |
