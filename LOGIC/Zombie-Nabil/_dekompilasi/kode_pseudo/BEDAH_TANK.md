# Bedah Kode Tank (server.dll)

Hasil analisis pseudo-C Ghidra dari `server.dll` Left 4 Dead. Lokasi seperti `bagian_003.c:68421` mengacu ke folder `server\`.

**Yang sudah pasti:** nama pengaturan, nilai default, dan teks log berasal langsung dari DLL.
**Yang masih perkiraan:** nama fungsi bertanda "(perkiraan)" dan alur yang saya simpulkan dari kode, karena nama aslinya hilang.
Belum ada yang diuji di dalam game. Pseudo-code di bawah adalah ringkasan saya, bukan kode asli Valve.

## Ringkasan alur

1. **Muncul:** Director memutuskan Tank muncul. Di versus, undian tiket memilih pemain yang mengendalikannya.
2. **Mengejar:** AI memilih korban, mengejar, memukul, dan melempar batu kalau jarak mencukupi.
3. **Macet atau hilang jejak:** AI mengganti target, lalu bunuh diri kalau tetap tidak ada kemajuan.
4. **Frustrasi (pemain):** kalau pemain Tank tidak melihat survivor, bar frustrasi naik sampai kendali dipindahkan.
5. **Mati:** saat darah habis, Tank masuk animasi sekarat dan darahnya berkurang sampai 0.

## 1. Darah

Fungsi: `1021f3d0` di `bagian_003.c:68421`.

```
pengali = 1.0
jika kesulitan == Easy:            pengali = 0.75
jika kesulitan == Hard atau Expert: pengali = 2.0
jika mode versus:                  pengali = versus_tank_bonus_health (1,5)
darah_maks = z_tank_health * pengali
```

| Kesulitan | Pengali | Darah (default 4000) |
|---|---|---|
| Easy | 0,75 | 3000 |
| Normal | 1,0 | 4000 |
| Hard | 2,0 | 8000 |
| Expert | 2,0 | 8000 |
| Versus | 1,5 | 6000 |

Urutan kode kesulitan (0 Easy, 1 Normal, 2 Hard, 3 Expert) dikonfirmasi oleh fungsi terbakar di bagian 7.

## 2. AI menyerang: `TankAttack`

Fungsi utama: `1030b250` di `bagian_004.c:106337`, yaitu slot 39 tabel virtual `TankAttack`, kemungkinan `TankAttack::Update` (perkiraan).

Setiap update:

1. Kalau sedang animasi khusus atau stasis, jalur dan timer macet direset.
2. Pengecekan bunuh diri dijalankan (lihat bawah).
3. Korban dipilih ulang kalau korban saat ini sudah tidak valid.
4. Fungsi lempar batu dicoba, Tank menatap korban, lalu bergerak mengejar.

### Bunuh diri saat macet

Hanya aktif kalau `tank_stuck_failsafe` menyala. Log-nya: `Tank committing suicide`.

| Kondisi | Akibat | Pengaturan (default) |
|---|---|---|
| Macet lebih dari 2 dtk dan survivor tak terlihat minimal 5 dtk | Ganti target | `tank_stuck_time_choose_new_target` (2), `tank_stuck_visibility_tolerance_choose_new_target` (5) |
| Macet lebih dari 10 dtk dan tak terlihat lebih dari 15 dtk | Bunuh diri | `tank_stuck_time_suicide` (10), `tank_stuck_visibility_tolerance_suicide` (15) |
| Tak terlihat lebih dari 60 dtk | Bunuh diri | `tank_visibility_tolerance_suicide` (60) |
| Stasis lebih dari 30 dtk | Bunuh diri | `tank_stasis_time_suicide` (30) |

Makna "macet" dan "tak terlihat" adalah perkiraan saya dari nama pengaturan dan alur kodenya.

## 3. Lempar batu

### Keputusan melempar

Fungsi: `1030adf0` di `bagian_004.c:106189` (log: `Throwing rock`).

Tank melempar kalau, antara lain:
- Korban lebih jauh dari `tank_throw_allow_range` (250).
- Jeda sudah habis. Setelah melempar, timer diisi `tank_throw_min_interval` (8 dtk).
- Di Easy dan Normal, kecuali saat satu penanda Director menyala (kemungkinan finale), ada syarat tambahan minimal 10 detik sejak suatu timer. Timer apa tepatnya belum dipastikan.

### Membidik

Fungsi yang sama menghitung arah lemparan:

```
jarak    = jarak ke korban
sudut    = min(jarak * tank_throw_loft_rate, tank_throw_max_loft_angle)
jika Easy: tambah meleset acak sebesar tank_throw_aim_error (100)
selain itu: bidik ke depan korban, memakai
            jarak / z_tank_throw_force + tank_throw_lead_time_factor
```

Jadi Tank di Easy sengaja meleset, sedangkan di kesulitan lain membidik posisi korban berikutnya.

### Objek batu: `CTankRock`

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `102c8630` | bagian_004.c:70246 | Membuat batu dan menghitung lintasan ke target, memakai `z_tank_throw_force` dan gravitas (perkiraan) |
| `102c8820` | bagian_004.c:70326 | Membuat batu dan melemparnya lurus ke depan |
| `102c7100` | bagian_004.c:69836 | Memilih model dan efek batu atau tunggul (`tank_rock_throw_rock` / `tank_rock_throw_stump`) |
| `102c7d10` | bagian_004.c:70055 | `CTankRock::ProximityThink`: kotak tabrakan membesar dari 4 ke 16 dalam 0,5 dtk, batu bisa ditembak kalau `z_tank_throw_health` di atas 0, dan pintu dalam radius 40 ikut diperiksa |
| `102c79e0` | bagian_004.c:69910 | Cek jarak ke pemain, dengan radius `z_tank_rock_radius` (100) dan dikali 0,75 di Easy. Kalau jalur bersih, batu meledak mengenai pemain |
| `102c6570` | bagian_004.c:69569 | Efek benturan dan suara `HulkZombie.Throw.FlyLoop` |

## 4. Pukulan: `CTankClaw`

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `1034d580` | bagian_005.c:33424 | Menghitung jeda pukulan berikutnya dari `tank_swing_interval` (1,5), `tank_swing_miss_interval` (1,0), dan `tank_swing_fast_interval` (0,6, saat membersihkan zombie di jalannya) |
| `1034e990` | bagian_005.c:33880 | Memulai pukulan dan mencari survivor yang sudah jatuh untuk dihantam ke tanah (ground pound), memakai `tank_swing_range` (56) dan `tank_ground_pound_duration` (1,5) |

## 5. Frustrasi

Fungsi: `102b6020` di `bagian_004.c:59693`.

```
setiap 1 detik, kalau z_frustration aktif dan pemain Tank tak melihat survivor
selama lebih dari z_frustration_los_delay (2 dtk):
    frustrasi += 100 / z_frustration_lifetime     (default 100 / 20 = 5 per detik)
    jika frustrasi >= 100:
        kirim event tank_frustrated
        jika ada pemain infected lain: pindahkan lewat undian
        jika tidak: kendali dilepas (perkiraan: diambil alih AI)
jika syarat di atas tidak terpenuhi: frustrasi = 0
```

Dengan nilai default, bar penuh dalam sekitar 20 detik.

## 6. Undian pemain Tank

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `10223240` | bagian_003.c:71568 | Memulai penawaran. Tidak ada pemain lain: jadi bot. Satu pemain: langsung ke pemain itu. Dua atau lebih: undian. `tank_raffle_debug` memaksa undian |
| `1022c3d0` | bagian_003.c:78309 | Undian berbobot: makin banyak tiket, makin besar peluang menang. Kalau tiket semua nol, dipilih acak |

## 7. Terbakar, sekarat, dan melambat

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `1028d1c0` | bagian_004.c:37052 | Terbakar: suara `HulkZombie.PainFire`, durasi `tank_burn_duration_normal` (30), `_hard` (35), `_expert` (40). Di luar mode versus (perkiraan) |
| `102b19f0` | bagian_004.c:56915 | Sekarat: damage berkala sebesar `z_tank_incapacitated_decay_rate` (1). Pada animasi tertentu, damage memakai `z_tank_incapacitated_health` (5000) |
| `1027c770` | bagian_004.c:26214 | Melambat karena tembakan: faktor 1 sampai jarak 200, turun linear ke 0 di jarak 400 (`z_tank_damage_slow_min_range`, `z_tank_damage_slow_max_range`) |
| `1029cc20` | bagian_004.c:46130 | Stagger, antara lain memakai `z_tank_max_stagger_distance` (400) dan `z_tank_max_stagger_duration` (6) |

## 8. Gerak dan langkah

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `1025b000` | bagian_004.c:6109 | Kecepatan maksimum: `z_tank_speed` (210), atau `z_tank_speed_vs` (210) di versus. Fungsi ini juga mengatur kecepatan kelas lain |
| `10292990` | bagian_004.c:40548 | Guncangan langkah: `z_tank_footstep_shake_amplitude` (5), `_duration` (2), `_radius` (750), `_interval` (0,4) |

## 9. Kelas Tank dari RTTI

`Tank`, `TankAttack`, `TankBehavior`, `TankIdle`, `TankIntention`, `TankLocomotion`, `CTankClaw`, `CTankRock`, `CThrow`.

**Celah:** dari 12 slot `TankAttack` yang berbeda dari `TankIdle`, hanya 3 yang punya pseudo-C. Sisanya ada di DLL tetapi tidak dikenali Ghidra sebagai fungsi, sehingga tidak ikut diekspor. Begitu juga kode pendaftaran pengaturan di area `0x10408000`-an.

| Kelas | Slot | Alamat | Status |
|---|---|---|---|
| `TankAttack` | 38 | `1030acd0` | Ada: bagian_004.c:106143 |
| `TankAttack` | 39 | `1030b250` | Ada: bagian_004.c:106337 |
| `TankAttack` | 70 | `103176c0` | Ada: bagian_005.c:5589 |
| `TankIdle` | 46 | `1030c3f0` | Ada: bagian_004.c:106619 |
| `TankIdle` | 56 | `1030c240` | Ada: bagian_004.c:106564 |

## 10. Semua pengaturan Tank

Diambil dari kode pendaftaran di DLL. Ini nilai bawaan yang tertanam di DLL; nilai yang dipakai saat bermain bisa berbeda kalau diubah file konfigurasi.
Sebagian pengaturan mungkin butuh `sv_cheats`, karena flag-nya belum saya periksa.

| Pengaturan | Default | Keterangan dari DLL | Fungsi pemakai |
|---|---|---|---|
| `tank_attack_range` | 50 | Distance from Survivor that tank triggers own swing. |  |
| `tank_auto_swing` | 0 |  |  |
| `tank_burn_duration_expert` | 40 | Number of seconds a burning Tank takes to die | `1028d1c0` |
| `tank_burn_duration_hard` | 35 | Number of seconds a burning Tank takes to die | `1028d1c0` |
| `tank_burn_duration_normal` | 30 | Number of seconds a burning Tank takes to die | `1028d1c0` |
| `tank_burn_duration_vs` | 30 | Number of seconds a burning Tank takes to die |  |
| `tank_fist_radius` | 15 |  | `1034f2e0` |
| `tank_ground_pound_duration` | 1.5 |  | `1034e990` |
| `tank_ground_pound_reveal_distance` | 500 |  | `1034c780` |
| `tank_pz_forward` | -0.5 |  | `1034e990` |
| `tank_raffle_debug` | 0 | Fill the tank raffle with dummy values for debugging | `10223240` |
| `tank_rock_overhead_percent` | 100 | Percent of time the tank throws overhand (0-100) |  |
| `tank_stasis_time_suicide` | 30 |  | `1030b250` |
| `tank_stuck_failsafe` | 1 |  | `1030b250` |
| `tank_stuck_time_choose_new_target` | 2 |  | `1030b250` |
| `tank_stuck_time_suicide` | 10 |  | `1030b250` |
| `tank_stuck_visibility_tolerance_choose_new_target` | 5 |  | `1030b250` |
| `tank_stuck_visibility_tolerance_suicide` | 15 |  | `1030b250` |
| `tank_swing_arc` | 180 |  |  |
| `tank_swing_duration` | 0.2 | Duration of the actual swing |  |
| `tank_swing_fast_interval` | 0.6 | Interval between tank swings when he is clearing zombies out of his path | `1034d580` |
| `tank_swing_interval` | 1.5 | Interval between tank swings | `1034d580` |
| `tank_swing_miss_interval` | 1.0 | Interval between tank swings after a miss | `102b4880` `1034d580` |
| `tank_swing_physics_prop_force` | 4.0 | Multiplier for tank hitting a phys prop. | `1034c850` |
| `tank_swing_range` | 56 | Range of the actual swing | `1034e800` `1034e990` |
| `tank_swing_yaw` | 80 |  |  |
| `tank_throw_aim_error` | 100 | Margin of error for Easy mode Tank rock throws | `1030adf0` |
| `tank_throw_allow_range` | 250 | How far away our victim must be before we'll try to throw debris at them | `1030adf0` |
| `tank_throw_lead_time_factor` | 0.5 | How much to lead a moving target | `1030adf0` |
| `tank_throw_loft_rate` | .01 | Beyond no-loft range, Tank adds this angle/distance when throwing | `1030adf0` |
| `tank_throw_max_loft_angle` | 30 | Maximum loft angle for Tank throw angle adjustment | `1030adf0` |
| `tank_throw_min_interval` | 8 | Minimum interval between Tank rock throws | `1030adf0` |
| `tank_visibility_tolerance_suicide` | 60 |  | `1030b250` |
| `tank_windup_time` | .5 | Time from intent to swing that swing actually happens. | `1034d3d0` |
| `z_frustration` | 1 |  | `102b6020` |
| `z_frustration_lifetime` | 20 | Frustration will accumulate for this many seconds before forcing an AI tank takeover | `102b6020` |
| `z_frustration_los_delay` | 2 |  | `102b6020` |
| `z_frustration_spawn_delay` | 10 |  | `102a4840` |
| `z_tank_attack_interval` | 1.5 |  |  |
| `z_tank_autoshotgun_dmg_scale` | 0.75 | percent damage taken by the tank from the autoshotgun in versus |  |
| `z_tank_burning_lifetime` | 75 | Number of seconds a burning tank takes to die in versus |  |
| `z_tank_damage_slow_max_range` | 400 | Range at which Tank is slowed by gunfire | `1027c770` |
| `z_tank_damage_slow_min_range` | 200 | Range at which Tank is slowed by gunfire | `1027c770` |
| `z_tank_footstep_shake_amplitude` | 5 |  | `10292990` `1034e120` |
| `z_tank_footstep_shake_duration` | 2 |  | `10292990` `1034e120` |
| `z_tank_footstep_shake_interval` | 0.4 |  | `10292990` |
| `z_tank_footstep_shake_radius` | 750 |  | `10292990` `1034e120` |
| `z_tank_grenade_damage` | 750 | How much damage a Tank takes from a grenade. |  |
| `z_tank_grenade_roll` | -10 |  | `102642b0` |
| `z_tank_grenade_slowdown` | 0 | The speed setting put on a Tank when hit by a grenade. |  |
| `z_tank_has_special_blood` | 0 | Does the Tank get his own blood effect instead of the normal infected one? | `10281980` |
| `z_tank_health` | 4000 | Tank Zombie max health | `1021f3d0` |
| `z_tank_incapacitated_decay_rate` | 1 | How much health a dying Tank loses each update. | `102b19f0` |
| `z_tank_incapacitated_health` | 5000 | Health Tank starts with in death throes. | `1029eb20` `102ad8a0` `102b19f0` |
| `z_tank_max_stagger_distance` | 400 | Max distance a Tank staggers when hurt by a grenade. | `1029cc20` `1029da00` `1029df50` |
| `z_tank_max_stagger_duration` | 6 | Max time a Tank staggers when hit by a grenade. | `1029cc20` |
| `z_tank_max_stagger_fade_duration` | 6 | Fade in time when a Tank is staggered by a grenade. |  |
| `z_tank_rock_debug` | 0 | Show debug for the rock thinking about detonating on people. | `102c79e0` |
| `z_tank_rock_radius` | 100 |  | `102c79e0` |
| `z_tank_speed` | 210 |  | `1025b000` |
| `z_tank_speed_vs` | 210 |  | `1025b000` |
| `z_tank_stagger_fade_alpha` | 192 | How opaque the screen fade is when a Tank is hit by a grenade. | `1029cc20` |
| `z_tank_stagger_fade_duration` | 3 | How long the screen fade lasts when a Tank is hit by a grenade. | `1029cc20` |
| `z_tank_throw_fail_interval` | 0 | How soon a tank can try again after failing to throw. |  |
| `z_tank_throw_force` | 800 |  | `102c8630` `102c8820` `1030adf0` |
| `z_tank_throw_health` | 50 | Health of the tank projectile (0 disables) | `102c7d10` |
| `z_tank_throw_interval` | 5 | How often a tank can throw. | `102c6570` |
| `z_tank_walk_speed` | 100 |  | `1025c970` |
| `z_tanks_block_molotovs` | 1 |  | `1026af10` |
