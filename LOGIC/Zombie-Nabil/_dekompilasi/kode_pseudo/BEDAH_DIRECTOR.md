# Bedah Director AI (server.dll)

Director adalah sistem yang mengatur tempo permainan: kapan mob, special infected, Tank, Witch, dan finale terjadi. Lokasi seperti `bagian_003.c:84503` mengacu ke folder `server\`.

**Yang sudah pasti:** nama tahap, nama pengaturan, nilai bawaan di DLL, dan teks log.
**Yang masih perkiraan:** bagian bertanda "(perkiraan)", karena nama asli banyak fungsi dan variabel hilang.
Belum ada yang diuji di dalam game.

## Inti: siklus empat tahap

Fungsi: `Director::Update`, `10235ff0` di `bagian_003.c:84503`. Siklus ini tidak dipakai di versus; kodenya membuat tahap tetap `BUILD_UP` dan menangani boss lewat `102286e0`.

```
        intensitas >= 0,9                  timer 3-5 dtk
BUILD_UP ---------------> SUSTAIN_PEAK ----------------> PEAK_FADE
   ^                                                         |
   |  timer 30-45 dtk habis, atau                            | tidak ada Tank aktif
   |  survivor maju lebih dari 3000 unit                     | dan intensitas < 0,9
   +------------------------ RELAX <-------------------------+
```

| Tahap | Artinya | Pengaturan |
|---|---|---|
| `BUILD_UP` | Tekanan dibangun, mob boleh muncul. Minimal 15 dtk sebelum bisa naik | `director_build_up_min_interval` (15) |
| `SUSTAIN_PEAK` | Puncak ketegangan dipertahankan sebentar | `director_sustain_peak_min_time` (3), `_max_time` (5) |
| `PEAK_FADE` | Menunggu pemain tenang dan Tank mati | `director_intensity_relax_threshold` (0,9) |
| `RELAX` | Istirahat, mob berhenti | `director_relax_min_interval` (30), `_max_interval` (45), `director_relax_max_flow_travel` (3000) |

Pemicu naik ke puncak adalah `director_intensity_threshold` (0,9).

Tahap dipaksa kembali ke `BUILD_UP` setiap frame saat salah satu dari tiga penanda menyala. Salah satunya penanda panik, yang dipasang oleh `Director::StartPanicEvent`. Dua lainnya belum dipastikan (perkiraan: terkait finale).

**Hal penting:** batas `director_relax_max_flow_travel` mencegah pemain "memanfaatkan" masa istirahat. Kalau survivor berlari jauh saat `RELAX`, istirahat langsung berakhir.

## Intensitas

Intensitas adalah ukuran seberapa tegang situasi pemain.

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `10241390` | bagian_003.c:91169 | Menurunkan intensitas dari waktu ke waktu dan menghitung rata-ratanya |
| `10261e60` | bagian_004.c:10756 | Kematian musuh menambah intensitas, tergantung jarak |
| `102210d0` | - | Dibaca untuk keputusan tahap; kemungkinan mengambil nilai tertinggi antar pemain (perkiraan) |

Pengaturan:
- `intensity_decay_time` (30): intensitas turun penuh dari 1 ke 0 dalam 30 dtk, kalau tidak ada tambahan.
- `intensity_averaged_following_decay` (20): kecepatan nilai rata-rata mengikuti.
- `intensity_enemy_death_near_range` (150) dan `intensity_enemy_death_far_range` (500): jarak kematian musuh yang dihitung.
- `intensity_lock` (-1): kalau bernilai 0 atau lebih, intensitas dikunci ke nilai itu. Berguna untuk eksperimen.

## Mob

Fungsi pengambil keputusan: `1022d300` di `bagian_003.c:78816`. Mob biasa hanya muncul kalau:

1. Tahap sekarang `BUILD_UP`.
2. Tidak sedang holdout atau finale, dan event panik sudah selesai.
3. `director_no_mobs` mati.
4. Timer mob habis.
5. Tidak ada Tank aktif, atau survivor sudah maju lebih dari `director_tank_bypass_max_flow_travel` (1500) sejak Tank muncul.

Syarat 5 berarti Tank menahan mob, tetapi pemain tidak bisa aman dengan terus berlari jauh saat Tank hidup.

### Timer

`10224810` di `bagian_003.c:72426` memilih waktu acak berdasarkan kesulitan, lalu mereset ukuran mob:

| Kesulitan | Minimum | Maksimum |
|---|---|---|
| Easy | 120 dtk | 240 dtk |
| Normal, Hard, Expert | 90 dtk | 180 dtk |

Mob pertama setelah keluar safe room punya rentang lebih lebar: `minimum x 0,25` sampai maksimum, jadi 22,5 sampai 180 dtk di Normal (`10230dc0` di `bagian_003.c:81369`).

### Ukuran mob makin besar kalau lama menunggu

Di `Director::Update`:

```
laju   += z_mob_recharge_rate (0,0025) * waktu_frame
ukuran += laju * waktu_frame
ukuran  = maksimum z_mob_spawn_max_size (30)
```

Ukuran mulai dari `z_mob_spawn_min_size` (10). Dengan rumus ini, ukuran mencapai 30 setelah sekitar 126 dtk. Itu hasil hitungan saya dari rumusnya, bukan hasil uji.

### Asal mob

`102c0d30` di `bagian_004.c:66525` menerima permintaan mob. `z_spawn_mobs_behind_chance` (75) kemungkinan peluang mob datang dari belakang (perkiraan). `102c28d0` di `bagian_004.c:67791` memilih area spawn, dan `102c3800` (`ZombieManager::StartFrame`) di `bagian_004.c:68396` memunculkan zombie satu per satu.

## Special infected

Fungsi: `1022c9a0` di `bagian_003.c:78501`, dipanggil di luar versus.

- Jeda awal acak `director_special_initial_spawn_delay_min` (30) sampai `_max` (60).
- Peta `l4d_airport02_offices` memakai batas atas khusus `director_special_initial_spawn_delay_max_extra` (180).
- Setelah itu berulang tiap `director_special_respawn_interval` (45), atau `director_special_battlefield_respawn_interval` (10) dalam kondisi tertentu.
- `director_no_specials` mematikan semuanya.

## Tank dan Witch

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `10224e00` | bagian_003.c:72718 | Memunculkan boss, termasuk `director_force_tank`, `director_force_witch`, dan `director_no_bosses` |
| `10224ca0` | bagian_003.c:72660 | Setelah Tank mati, timer berikutnya diisi acak `director_tank_min_interval` (350) sampai `_max_interval` (500) |
| `1021f880` | - | Menempatkan "ancaman" di peta memakai `director_threat_radius` (1000) dan `director_threat_min_separation`/`_max_separation` (5000) |
| `10220ed0` | bagian_003.c:69724 | Peluang dan posisi boss di versus |

### Peluang boss di versus

| Jenis peta | Tank | Witch | Posisi di jalur peta |
|---|---|---|---|
| Peta pertama | 0,3 | 0,3 | 50% sampai 75% |
| Peta tengah | 0,75 | 0,75 | 10% sampai 90% |
| Peta terakhir | 0,1 | 0,1 | 25% sampai 60% |

Untuk peta pertama, kodenya memeriksa nomor peta 0. Untuk peta terakhir, ia memeriksa satu penanda yang saya perkirakan berarti peta terakhir.

## Event panik

Mulai: `Director::StartPanicEvent`, `10225400` di `bagian_003.c:73052`. Tahapan: `1022ecc0` di `bagian_003.c:79882`.

```
STAGE_INITIAL_DELAY -> STAGE_MEGA_MOB -> STAGE_WAIT_FOR_COMBAT_TO_END
                             ^                       |
                             |                       +-> STAGE_DONE (gelombang habis)
                        STAGE_PAUSE <----------------+  (masih ada gelombang, jeda 5-7 dtk)
```

- Ukuran `z_mega_mob_size` (50).
- Jumlah gelombang bawaan: 1 di peta ke-1 dan ke-2, serta 2 mulai peta ke-3.
- `director_panic_forever` membuat panik tidak pernah selesai.

## Finale

Fungsi: `10227260` di `bagian_003.c:74514`, dimulai dari `1022b330` di `bagian_003.c:77506`.

```
FINALE_HORDE_ATTACK -> FINALE_HALFTIME_BOSS -> FINALE_HORDE_ATTACK_2 -> FINALE_FINAL_BOSS -> FINALE_HORDE_ESCAPE
```

Urutan ini saya susun dari teks log. `director_finale_stage_delay` (5) adalah jeda antar tahap. Kalau Tank terakhir gagal muncul, kode menyerah dengan log `Final Tank has FAILED to spawn! Giving up and starting escape!`. `director_finale_infinite` dan `director_finale_max_loops` (2) mengatur pengulangan.

Kode ini juga punya jalur `[HOLDOUT]` di `1022cea0` (`bagian_003.c:78658`) dan DLC3 di `10237330` (`bagian_003.c:85400`).

## Peringatan untuk mod: nama peta tertulis langsung di DLL

Peta di mod ini sudah diganti dari `l4d_*` menjadi `zombie_*`, tetapi DLL masih membandingkan nama lama:

| DLL | Nama peta `l4d_*` lengkap yang tertulis | Contoh |
|---|---|---|
| `server.dll` | 10 | `l4d_airport02_offices`, `l4d_river01_docks`, `l4d_farm01_hilltop` |
| `client.dll` | 16 | `l4d_river03_port`, `l4d_hospital05_rooftop` |

Selain itu ada awalan seperti `l4d_airport01_`, `l4d_vs_farm`, dan `left4dead` di kedua DLL.

Pengecekan seperti ini tidak akan cocok lagi dengan peta `zombie_*`. Contoh pastinya: batas jeda khusus untuk `l4d_airport02_offices` tidak akan berlaku. Dampak keseluruhannya belum saya uji.

## Celah analisis

- `102210d0`, `1021f880`, dan beberapa fungsi lain belum saya baca isinya.
- Beberapa pengaturan tidak tertangkap pemakainya, seperti `director_ai_tanks` dan `director_finale_mob_min_interval`.
- `1022cea0` dan `10227260` baru saya baca dari teks log dan pengaturannya, belum seluruh alurnya.

## Perintah konsol dan pengaturan

Perintah yang ditemukan di DLL: `director_stop`, `director_start`, `director_report`, `director_force_panic_event`, `director_force_versus_start`, `director_print_player_counts`, `director_log_boss_threats`, `director_log_scavenge_items`.

Ini nilai bawaan di DLL; nilai saat bermain bisa berbeda kalau diubah file konfigurasi. Sebagian mungkin butuh `sv_cheats`, karena flag-nya belum saya periksa.

| Pengaturan | Default | Keterangan dari DLL | Fungsi pemakai |
|---|---|---|---|
| `director_afk_timeout` | 45 | Humans idle longer than this many seconds will be forced to spectator mode | `10281be0` `1029c280` `102a8040` |
| `director_ai_tanks` | 0 |  |  |
| `director_build_up_min_interval` | 15 |  | `10235ff0` |
| `director_convert_pills` | 1 |  | `1022d640` |
| `director_convert_pills_critical_health` | 50 |  | `1021fe80` |
| `director_debug` | 0 |  | `102207c0` `10235ff0` |
| `director_debug_scavenge_items` | 0 |  | `10233560` `10234f20` `10235ff0` |
| `director_debug_threat_placement` | 0 |  | `10220820` `102c2e30` |
| `director_finale_infinite` | 0 | Never bring in a rescue vehicle. | `10227260` |
| `director_finale_item_cluster_count` | 3 | How many clusters of items will be populated in the finale | `10247010` `102470f0` `10247660` |
| `director_finale_max_loops` | 2 | Versus mode: Number of times to loop through the finale waves before rescue. | `10227260` |
| `director_finale_mob_max_interval` | 45 |  |  |
| `director_finale_mob_min_interval` | 5 |  |  |
| `director_finale_mob_relax_interval` | 20 | How long a 'relax' wave lasts during the finale |  |
| `director_finale_stage_delay` | 5 |  | `10227260` `10237720` |
| `director_force_background` | 0 | Forces background map population logic for testing | `1021fc80` `1022a040` `102356c0` |
| `director_force_tank` | 0 |  | `10224e00` |
| `director_force_witch` | 0 |  | `10224e00` |
| `director_garage_pistol_density` | 1.7 | Garage has too many pistols | `10247580` |
| `director_garage_weapon_density` | 1.28 | Garage has too many weapons | `102475c0` |
| `director_gas_can_density` | 6.48 | Items per 100 yards square | `10247010` `102470f0` `10247600` |
| `director_ingress_range` | 3000 |  |  |
| `director_intensity_relax_allow_wanderers_threshold` | 0.3 | All survivors must be below this intensity during RELAX for wandering zombies to be spawned | `1022b5a0` |
| `director_intensity_relax_allow_wanderers_threshold_expert` | 0.8 | All survivors must be below this intensity during RELAX for wandering zombies to be spawned | `1022b5a0` |
| `director_intensity_relax_allow_wanderers_threshold_hard` | 0.5 | All survivors must be below this intensity during RELAX for wandering zombies to be spawned | `1022b5a0` |
| `director_intensity_relax_threshold` | 0.9 | All survivors must be below this intensity before a Peak is allowed to switch to Relax (in addition to the normal peak timer) | `10235ff0` |
| `director_intensity_threshold` | 0.9 |  | `10235ff0` |
| `director_item_cluster_range` | 50 | Scavenge items of the same kind that are this close to each other are considered a single 'cluster' for population purposes | `1022df10` `10247010` `102470f0` |
| `director_min_start_players` | 1 | Freezes players until this # of players have joined, then starts mission | `1022a040` `1022aa90` `1022c0f0` +1 |
| `director_molotov_density` | 6.48 | Items per 100 yards square | `10247010` `102470f0` `10247540` |
| `director_must_create_all_scavenge_items` | 0 |  | `10233560` |
| `director_no_bosses` | 0 | Disable boss spawns | `10224e00` `10227e10` `1022c3d0` |
| `director_no_death_check` | 0 | Disable survivor team death ending scenario | `10235b80` |
| `director_no_mobs` | 0 | Disable mob rushes | `1022d300` |
| `director_no_specials` | 0 | Disable PZ spawns | `1022c9a0` `10275980` `10290f50` +1 |
| `director_no_survivor_bots` | 0 | Disable filling out the survivor team with bots | `1022e200` `1022e2d0` `1022eb80` +5 |
| `director_oxygen_tank_density` | 6.48 | Items per 100 yards square | `10247010` `102470f0` `10247640` |
| `director_pain_pill_density` | 6.48 | Items per 100 yards square | `10247010` `102470f0` `10247520` |
| `director_panic_forever` | 0 | Panic events never end | `1022ecc0` |
| `director_pipe_bomb_density` | 6.48 | Items per 100 yards square | `10247010` `102470f0` `10247560` |
| `director_pistol_density` | 4 | Items per 100 yards square | `10247010` `102470f0` `10247580` |
| `director_propane_tank_density` | 6.48 | Items per 100 yards square | `10247010` `102470f0` `10247620` |
| `director_race_mode` | 0 | Are we both competing to try and win the mission first? | `10220280` |
| `director_ready_duration` | 60 | If nonzero, survivor team has this amount of time to get ready before starting out | `10226fd0` `1022a040` |
| `director_ready_radius` | 300 | The distance survivors can move from the starting weapons during the ready period |  |
| `director_relax_max_flow_travel` | 3000 |  | `10235ff0` |
| `director_relax_max_interval` | 45 |  | `10235ff0` |
| `director_relax_min_interval` | 30 |  | `10235ff0` |
| `director_report_screen_delay` | 10 |  | `1022ac80` `1022f360` |
| `director_scavenge_item_override` | 0 | Override map-specified item densities with cvar values for tuning | `10247010` `10247520` `10247540` +7 |
| `director_short_finale` | 0 | Short finale for testing escape vehicles |  |
| `director_solo_mode` | 0 | If nonzero, create a single-player dramatic experience | `10220b00` `10223190` `102238c0` +9 |
| `director_special_battlefield_respawn_interval` | 10 |  | `1022c9a0` |
| `director_special_finale_offer_length` | 10 | How long is given to a player to accept the offer of a special zombie during the finale. |  |
| `director_special_initial_spawn_delay_max` | 60 |  | `1022c9a0` |
| `director_special_initial_spawn_delay_max_extra` | 180 |  | `1022c9a0` |
| `director_special_initial_spawn_delay_min` | 30 |  | `1022c9a0` |
| `director_special_original_offer_length` | 30 | How long is given to the first player to accept the offer of a special zombie. | `10230c90` |
| `director_special_respawn_interval` | 45 |  | `1022c9a0` |
| `director_spectate_specials` | 0 | Allow spectating zombies | `10287af0` |
| `director_sustain_peak_max_time` | 5 |  | `10235ff0` |
| `director_sustain_peak_min_time` | 3 |  | `10235ff0` |
| `director_swap_teams` | 0 | Swap teams each mission | `10235000` |
| `director_tank_bypass_max_flow_travel` | 1500 |  | `1022b500` |
| `director_tank_checkpoint_interval` | 15 | Min time after leaving a checkpoint that a tank can spawn | `10230dc0` |
| `director_tank_force_offer` | 0 | Forces the director to offer a tank |  |
| `director_tank_lottery_entry_time` | 0.1 | Time it takes for entry into the tank lottery | `10223240` |
| `director_tank_lottery_selection_time` | 4 | Time it takes for selection in the tank lottery | `1022c3d0` |
| `director_tank_max_interval` | 500 |  | `10224ca0` `1022c0f0` `10230c90` |
| `director_tank_min_interval` | 350 |  | `10224ca0` `1022c0f0` `10230c90` |
| `director_tank_offer_debug` | 0 |  | `1022c3d0` |
| `director_threat_clear_radius` | 500 | Prevent wandering zombies within this radius of threats | `10224e00` `102286e0` |
| `director_threat_max_separation` | 5000 |  | `1021f880` |
| `director_threat_min_separation` | 5000 |  | `1021f880` |
| `director_threat_radius` | 1000 |  | `1021f880` |
| `director_transition_timeout` | 60 | Duration (in seconds) to wait for survivors to transition across changelevels | `102356c0` |
| `director_unfreeze_time` | 20 | Duration (in seconds) to wait to unfreeze a team after the first player has connected | `1022aa90` |
| `director_vs_convert_pills` | 0.5 | What fraction of pain pills should be converted to health kits in vs mode | `1021eee0` |
| `director_weapon_density` | 4 | Items per 100 yards square | `10247010` `102470f0` `102475c0` |
| `intensity_averaged_following_decay` | 20 | Seconds for the 'time averaged intensity' to meet the baseline intensity | `10241390` |
| `intensity_decay_time` | 30 | Seconds to decay full intensity to zero | `10241390` |
| `intensity_enemy_death_far_range` | 500 |  | `10261e60` |
| `intensity_enemy_death_near_range` | 150 |  | `10261e60` |
| `intensity_lock` | -1 | Lock players' intensities at this value | `10241390` `10241490` |
| `versus_boss_buffer` | 2200 |  | `102286e0` |
| `versus_boss_debug` | 0 | Enable debug output for versus tank/witch spawning logic | `102259f0` `102286e0` |
| `versus_boss_flow_max` | 0.9 | Max fraction of map flow for tank/witch spawn location | `10220ed0` `102259f0` |
| `versus_boss_flow_max_finale` | 0.6 | Max fraction of map flow for tank/witch spawn location within the finale map of a campaign | `10220ed0` |
| `versus_boss_flow_max_intro` | 0.75 | Max fraction of map flow for tank/witch spawn location within the first map of a campaign | `10220ed0` |
| `versus_boss_flow_min` | 0.1 | Min fraction of map flow for tank/witch spawn location | `10220ed0` `102259f0` |
| `versus_boss_flow_min_finale` | 0.25 | Min fraction of map flow for tank/witch spawn location within the finale map of a campaign | `10220ed0` |
| `versus_boss_flow_min_intro` | 0.5 | Min fraction of map flow for tank/witch spawn location within the first map of a campaign | `10220ed0` |
| `versus_boss_padding_max` | 0.3 | How far apart the tank/witch have to be along the flow | `102259f0` |
| `versus_boss_padding_min` | 0.1 | How far apart the tank/witch have to be along the flow | `102259f0` |
| `versus_boss_spawning` | 1 | Enable alternative spawning logic for tanks and witches in versus mode | `10220820` `102259f0` |
| `versus_force_start_time` | 90 | Starts versus game after this amount of time, even if survivors are still inside the safe room | `10235b80` |
| `versus_round_restarttimer` | 15 |  | `1022ac80` `1022b9c0` |
| `versus_round_restarttimer_finale` | 20 |  | `1022ac80` `1022b9c0` |
| `versus_shove_hunter_fov` | 90 | Angle that survivor shove will hit hunters | `1029d4f0` `10354130` |
| `versus_shove_hunter_fov_pouncing` | 45 | Angle that survivor shove will hit hunters while pouncing | `1029d4f0` `10354130` |
| `versus_tank_bonus_health` | 1.5 |  | `1021f3d0` |
| `versus_tank_chance` | 0.75 | Chance of a tank spawning within a regular versus map | `10220ed0` `102259f0` |
| `versus_tank_chance_finale` | 0.1 | Chance of a tank spawning within the finale map of a campaign | `10220ed0` |
| `versus_tank_chance_intro` | 0.3 | Chance of a tank spawning within the first map of a campaign | `10220ed0` |
| `versus_tank_flow_team_variation` | 0.1 | Window of flow distance that the tank can spawn in for the second team in versus | `102259f0` |
| `versus_winning_team_goes_last` | 1 | In versus mode, the team that is losing plays as survivors first | `10235000` |
| `versus_witch_chance` | 0.75 | Chance of a witch spawning within a regular versus map | `10220ed0` `102259f0` |
| `versus_witch_chance_finale` | 0.1 | Chance of a witch spawning within the finale map of a campaign | `10220ed0` |
| `versus_witch_chance_intro` | 0.3 | Chance of a witch spawning within the first map of a campaign | `10220ed0` |
| `versus_witch_flow_team_variation` | 0.0 | Window of flow distance that the witch can spawn in for the second team in versus | `102259f0` |
| `z_background_limit` | 20 | How many common infected are on the background map at once. | `1026fbd0` `10277320` `10277910` |
| `z_finale_chance` | 100 |  |  |
| `z_finale_spawn_safety_range` | 600 |  | `102a9d60` `102c1850` |
| `z_ghost_ahead_flow` | 500 | Ghosts must be this far ahead of the survivors to materialize |  |
| `z_ghost_allowed_in_battlefield` | 0 | Should ghosts be allowed when players are in a battle field? | `10290f50` |
| `z_ghost_checkpoint_spawn_interval` | 30 | Interval for spawning special zombies while survivors are in the checkpoint |  |
| `z_ghost_cooldown` | 0 |  | `102a9d60` |
| `z_ghost_delay_max` | 30 |  | `10290f50` |
| `z_ghost_delay_min` | 20 |  | `10290f50` |
| `z_ghost_delay_minspawn` | 3 |  | `10290f50` |
| `z_ghost_duration` | 1 | dead PZs will become ghosts when they are this close to being able to spawn |  |
| `z_ghost_finale_spawn_interval` | 20 | Interval for spawning special zombies during the finale | `102252f0` |
| `z_ghost_friction` | 1.5 |  |  |
| `z_ghost_group_spawn` | 1 | Should ghosts all spawn at once instead of a set time after death? | `102252f0` |
| `z_ghost_line_of_scrimmage_far` | 2000 |  | `1027ce40` |
| `z_ghost_line_of_scrimmage_near` | 1000 |  | `1027ce40` |
| `z_ghost_line_of_scrimmage_safety` | 10 | If the survivors are this close to the checkpoint, allow ghosts based on time | `10288960` |
| `z_ghost_line_of_scrimmage_threat_bonus` | 1000 |  | `1027ce40` |
| `z_ghost_los_expected_progress` | 1500 |  | `1028b630` |
| `z_ghost_offer_acceptance_time` | 0 | Ghosts who accept offers extend their timers to this much |  |
| `z_ghost_offer_spawn_safety_time` | 20 | Ghost offers don't go to ghosts who spawn sooner than this |  |
| `z_ghost_spawn_distance` | 300 | ghosts will be placed this far ahead of survivors when they cross the line of scrimmage | `10289860` |
| `z_ghost_spawn_in_start` | 0 | Allow ghosts to materialize while players are in the start area | `102a9d60` |
| `z_ghost_spawn_interval` | 60 | Interval for spawning special zombies | `10224d70` `102252f0` |
| `z_ghost_speed` | 450 |  | `1025b000` |
| `z_ghost_travel_distance` | 1000 | Ghosts this far away from survivors in travel distance can materialize regardless of linear distance from survivors | `10289660` |
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
| `z_spawn_flow_limit` | 1500 |  |  |
| `z_spawn_height` | 0 | Height above ground z_spawn places new infected |  |
| `z_spawn_mobs_behind_chance` | 75 | Percentage chance that a mob will spawn behind the Survivor team | `102c0d30` |
| `z_spawn_mobs_from_selected_set` | 0 | Spawn subsequent mobs from the currently selected set via the nav mesh editor | `102c2730` `102c28d0` |
| `z_spawn_range` | 1500 |  | `102a2d70` `102c2e30` |
| `z_spawn_safety_range` | 550 |  | `102a9d60` `102bfe00` |
| `z_spawn_speed` | 450 |  |  |
| `z_special_spawn_interval` | 45 | Interval for spawning special zombies |  |
