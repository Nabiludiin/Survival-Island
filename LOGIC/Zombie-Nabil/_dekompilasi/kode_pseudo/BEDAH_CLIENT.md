# Bedah client.dll

`client.dll` adalah sisi pemain: tampilan, HUD, glow, suara, kamera, dan menu. Logika gameplay seperti damage dan spawn ada di `server.dll`. Lokasi seperti `bagian_003.c:40216` mengacu ke folder `client\`.

**Yang sudah pasti:** nama kelas dari RTTI, nama pengaturan, nilai bawaan di DLL, dan teks yang tertanam.
**Yang masih perkiraan:** peran fungsi bertanda "(perkiraan)".
Belum ada yang diuji di dalam game.

## Peta sistem

Total 12.019 fungsi yang diekspor, 1.138 kelas RTTI, dan sekitar 1.360 pengaturan.

| Sistem | Kelas penting | Mulai baca dari |
|---|---|---|
| Glow / siluet | `C_PropGlowingObject`, `CTraceFilterGlow` | `101ffca0` di bagian_003.c:40216 |
| HUD | `CHudFrustrationMeter`, `CHudFinaleMeter`, `CHudGhostPanel`, `CHudZombiePanel`, `CHudZombieHealth`, `CHudAbilityTimer`, `CHudCrosshair`, `CHudIntensityGraph` | `10237ef0` di bagian_003.c:72275 |
| Pemain dan zombie | `C_TerrorPlayer`, `C_Infected`, `C_Tank`, `C_Witch`, `C_SurvivorBot` | `10208720` di bagian_003.c:45012 |
| Senjata dan kemampuan | `C_TerrorGun`, `C_Claw`, `C_TankClaw`, `C_Tongue`, `C_Vomit` | `102245f0` di bagian_003.c:61114 |
| Musik | `Music`, `MusicMasterTrack`, `MusicChannels` | `101f6010` di bagian_003.c:33292 |
| Instruktur (petunjuk di layar) | `C_GameInstructor`, `CBaseLesson`, `CScriptedIconLesson` | `1007f910` di bagian_000.c:81406 |
| Kamera dan tampilan | `CTerrorViewRender`, `C_TerrorViewModel`, `CCSInput` | `100b5050` di bagian_001.c:24023 |
| Partikel dan darah | `CParticleMgr`, `CNewParticleEffect`, `CHudBlood`, `CBloodSprayEmitter` | `101d2560` di bagian_003.c:14741 |

## Glow

Ini siluet warna yang terlihat menembus dinding.

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `101ffca0` | bagian_003.c:40216 | Memilih warna glow pemain, lalu dikali `cl_glow_brightness` |
| `10201660` | bagian_003.c:41083 | Menentukan glow tampil atau tidak, termasuk memudar saat terlihat langsung |
| `10212c00` | bagian_003.c:51535 | Warna glow barang |
| `10217030` | bagian_003.c:54044 | Menggambar glow, memakai material `dev/glow_*` (perkiraan) |

### Warna bawaan (R, G, B)

| Target | Pengaturan | Warna | Nilai |
|---|---|---|---|
| Survivor darah tinggi | `cl_glow_survivor_health_high_*` | Hijau | 0,039 / 0,69 / 0,196 |
| Survivor darah sedang | `cl_glow_survivor_health_med_*` | Kuning | 0,59 / 0,4 / 0,032 |
| Survivor darah rendah | `cl_glow_survivor_health_low_*` | Merah | 0,63 / 0,098 / 0,098 |
| Survivor terluka | `cl_glow_survivor_hurt_*` | Oranye | 1,0 / 0,4 / 0,0 |
| Survivor kena muntah | `cl_glow_survivor_vomit_*` | Oranye | 1,0 / 0,4 / 0,0 |
| Infected | `cl_glow_infected_*` | Biru | 0,3 / 0,4 / 1,0 |
| Infected kena muntah | `cl_glow_infected_vomit_*` | Ungu | 0,79 / 0,07 / 0,72 |
| Ghost infected | `cl_glow_ghost_infected_*` | Biru | 0,3 / 0,4 / 1,0 |
| Kemampuan | `cl_glow_ability_*` | Merah | 1,0 / 0,0 / 0,0 |
| Barang (dekat) | `cl_glow_item_*` | Putih kebiruan | 0,7 / 0,7 / 1,0 |
| Barang (jauh) | `cl_glow_item_far_*` | Biru | 0,3 / 0,4 / 1,0 |

Warna darah dan kemampuan punya versi `_colorblind_`. Versi itu dipilih kalau satu pengaturan bernilai 2 (perkiraan: mode buta warna).

Pengaturan perilaku: `cl_glow_los_fade_in_time` (0,5), `cl_glow_los_fade_out_time` (0,5), `cl_glow_range_exp` (1,0), `cl_glow_infected` (1), `cl_glow_infected_ghosts` (1).

## HUD

| Alamat | Lokasi | Yang ditampilkan |
|---|---|---|
| `10237ef0` | bagian_003.c:72275 | Pesan zombie seperti `#L4D_Zombie_UI_Action` dan `#L4D_Zombie_UI_BadPlace` |
| `102483d0` | bagian_003.c:81435 | Ikon tim infected: `hud/ZombieTeamImage_*` dan `hud/GhostTeamImage_*` |
| `1024c0c0` | bagian_003.c:83953 | Status di papan skor: mati, menunggu spawn, atau Tank |
| `1025ba90` | bagian_004.c:3265 | Pesan skor versus |
| `10230d30` | bagian_003.c:68001 | Ikon arah instruktur: `vgui/hud/icon_arrow_*` |
| `1025d300` | bagian_004.c:4136 | Teks pilihan spawn: `#L4D_pz_spawn_*` |
| `10263aa0` | - | Crosshair, memakai `cl_crosshair_*` |

Crosshair bawaan: warna 138 / 182 / 220, `cl_crosshair_alpha` 255, `cl_crosshair_dynamic` 1.

Teks berawalan `#` adalah kunci terjemahan di file `resource`, jadi banyak teks HUD bisa diganti lewat file itu tanpa mengubah DLL. **Perhatian:** di mod ini kuncinya sudah berubah dari `L4D_*` menjadi `ZOMBIE_*`, sedangkan DLL masih meminta `L4D_*`. Lihat `BEDAH_HUD.md`.

## Musik

Fungsi: `Music::Update`, `101f6010` di `bagian_003.c:33292`. Pemutar: `101f5d00` di `bagian_003.c:33215`.

Saat ada pemicu, musik yang dipilih tergantung satu nilai status:

| Status | Musik |
|---|---|
| 1 | `Checkpoint_0` |
| 2 atau 3 | `Checkpoint_1` |
| 4 | `Event.MissionStart_Holdout` dan `Event.LeavingSafety_Holdout` |

Musik Tank memakai `Event.Tank`, `Event.TankMidpoint`, dan `Event.TankBrothers`. Pengaturan debug: `music_master_debug`, `verbose_tank_music`, `cl_music_debug`.

Karena musik dipanggil lewat nama event, kemungkinan besar suaranya bisa diganti lewat file skrip suara tanpa mengubah DLL (perkiraan).

## Kamera dan tampilan

| Alamat | Lokasi | Fungsi |
|---|---|---|
| `100b5050` | bagian_001.c:24023 | Pengaturan kamera bahu `c_thirdpersonshoulder*` |
| `101727d0` | bagian_002.c:55209 | FOV senjata per kelas: `cl_viewmodelfovsurvivor`, `cl_viewmodelfovhunter`, `cl_viewmodelfovsmoker`, `cl_viewmodelfovboomer`, `cl_viewmodelfovtank` |
| `101fc390` | bagian_003.c:38493 | Cahaya kecil pada survivor: `cl_survivor_light_*`, termasuk `cl_survivor_light_brightness` (4) dan `cl_survivor_light_radius` (12) |

## Peringatan untuk mod: nama lama di client.dll

| Lokasi | Yang diperiksa | Akibat setelah diganti nama |
|---|---|---|
| bagian_002.c:72465 | `left4dead`, di dalam peringatan `minimum requirement of Shader Model 2.0` | Pengecekan kemungkinan tidak cocok lagi, sehingga peringatan untuk grafis lama bisa tidak muncul (perkiraan) |
| bagian_003.c:25691 | Awalan peta pertama: `l4d_airport01_`, `l4d_farm01_`, `l4d_hospital01_`, `l4d_smalltown01_`, `l4d_garage01_` | Perilaku khusus peta pembuka tidak akan terpanggil di peta `zombie_*` |
| bagian_003.c:25752 sampai 25794 | Awalan kampanye seperti `l4d_airport`, `l4d_vs_farm`, `l4d_sv_hospital` | Pengelompokan peta per kampanye tidak cocok lagi |
| bagian_003.c:28501, 28580 | `l4d_river03_port`, bersama event `generator_started` dan `player_begin_sacrifice_run` | Logika khusus finale DLC3, kemungkinan achievement, tidak berjalan |

Teks-teks ini tertanam di DLL, jadi mengganti nama peta saja tidak cukup untuk menjaga perilakunya.

## Pembagian client dan server

| Yang ingin diubah | Letaknya |
|---|---|
| Warna glow, crosshair, FOV, kamera, musik, HUD | `client.dll` atau konfigurasi pemain |
| Damage, darah, kecepatan, spawn, AI, Director | `server.dll` |
| Teks, gambar, model, suara | File di luar DLL, seperti `resource`, `materials`, `models`, `sound` |

Beberapa pengaturan `z_*` ada di kedua DLL. Nilai yang menentukan gameplay kemungkinan milik server (perkiraan).

## Celah analisis

- Banyak kelas HUD, seperti `CHudFrustrationMeter`, baru saya ketahui namanya. Fungsi gambarnya belum dilacak satu per satu.
- Fungsi besar seperti `10217030` dan `10201660` baru saya baca dari teks dan pengaturannya, belum seluruh alurnya.
- Tabel di bawah bisa memuat beberapa nama yang bukan pengaturan, karena dideteksi otomatis dari pola kode. Default yang tidak terbaca ditulis `?`.

## Semua pengaturan yang ditemukan

Ini nilai bawaan di DLL; nilai saat bermain bisa berbeda kalau diubah file konfigurasi. Sebagian mungkin butuh `sv_cheats`, karena flag-nya belum saya periksa.

| Pengaturan | Default | Keterangan dari DLL | Fungsi pemakai |
|---|---|---|---|
| `ability_lunge` | 0.1 | CLunge | `10223bb0` |
| `ability_vomit` | startvomit | CVomit | `102264e0` |
| `achievement_debug` | 0 | Turn on achievement debug msgs. | `10011f90` `100120a0` |
| `achievement_disable` | 0 | Turn off achievements. | `10002340` |
| `achievement_notification_test` | Test the hud notification UI |  | `10001d00` |
| `ai_debug_shoot_positions` | 0 |  |  |
| `ai_shot_bias_max` | 1.0 |  | `10019720` |
| `ai_shot_bias_min` | -1.0 |  | `10019720` |
| `ammo_assaultrifle_max` | 360 |  |  |
| `ammo_buckshot_max` | 128 |  |  |
| `ammo_firstaid_max` | -1 |  | `10264ed0` |
| `ammo_huntingrifle_max` | 150 |  |  |
| `ammo_minigun_max` | 800 |  |  |
| `ammo_molotov_max` | 1 |  |  |
| `ammo_painpills_max` | 1 |  |  |
| `ammo_pipebomb_max` | 1 |  |  |
| `ammo_pistol_max` | -2 |  |  |
| `ammo_smg_max` | 480 |  |  |
| `ammo_turret_infected_damage` | 15 | Turret damage per shot against the horde (turret fires at 10Hz) | `1021c680` |
| `ammo_turret_max` | 300 |  |  |
| `ammo_turret_pz_damage` | 8 | Turret damage per shot against player zombies (turret fires at 10Hz) | `1021c680` |
| `ammo_turret_survivor_damage` | 1 | Turret damage per shot against survivors (turret fires at 10Hz) | `1021c680` |
| `ammo_turret_tank_damage` | 40 | Turret damage per shot against tanks (turret fires at 10Hz) | `1021c680` |
| `ammo_turret_witch_damage` | 16 | Turret damage per shot against witches (turret fires at 10Hz) |  |
| `anim_3wayblend` | 1 | Toggle the 3-way animation blending code. | `10030d80` |
| `ash_outer` | ? |  | `10132fe0` `101952d0` `10195350` +4 |
| `blink_duration` | 0.2 | How many seconds an eye blink will last. | `10053fa0` `100543f0` |
| `boomer_pz_claw_dmg` | 4 | Amount of damage done by a PZ boomer's regular melee attack |  |
| `bot_mimic` | 0 |  | `10060fd0` `100b5310` `101941e0` |
| `bot_mimic_spec_buttons` | 1 | +attack, +jump etc are used for spectator control instead of being passed on to spectated bot | `100b5310` |
| `bullet_ff_through_walls` | 0 |  | `102602b0` |
| `c_maxdistance` | 200 |  | `1010e210` |
| `c_maxpitch` | 90 |  | `1010e210` |
| `c_maxyaw` | 135 |  | `1010e210` |
| `c_mindistance` | 30 |  | `1010e210` |
| `c_minpitch` | 0 |  | `1010e210` |
| `c_minyaw` | -135 |  | `1010e210` |
| `c_orthoheight` | 100 |  |  |
| `c_orthowidth` | 100 |  |  |
| `c_thirdpersonshoulder` | ? |  | `100b5050` `100b52c0` |
| `c_thirdpersonshoulderaimdist` | ? |  | `100b5050` |
| `c_thirdpersonshoulderdist` | 40.0 |  | `1010dd90` |
| `c_thirdpersonshoulderheight` | ? |  | `100b5050` |
| `c_thirdpersonshoulderoffset` | ? |  | `100b5050` `100b52c0` |
| `cam_collision` | 1 | When in thirdperson and cam_collision is set to 1, an attempt is made to keep the camera from passing though walls. | `1010dd40` |
| `cam_command` | Tells camera to change modes |  |  |
| `cam_idealdelta` | 4.0 | Controls the speed when matching offset to ideal angles in thirdperson view | `1010e210` |
| `cam_idealdist` | 150 |  | `1010e210` |
| `cam_ideallag` | 4.0 | Amount of lag used when matching offset to ideal angles in thirdperson view | `1010e210` |
| `cam_idealpitch` | 0 |  | `1010e210` `1010fcd0` `10114350` |
| `cam_idealyaw` | 0 |  | `1010e210` `1010fcd0` `101126a0` +3 |
| `cam_showangles` | 0 | When in thirdperson, print viewangles/idealangles/cameraoffsets to the console. | `1010e210` |
| `cc_captiontrace` | 1 | Show missing closecaptions (0 = no, 1 = devconsole, 2 = show in hud) | `100fe220` |
| `cc_emit` | Emits a closed caption |  |  |
| `cc_findsound` | Searches for soundname which emits specified text. |  | `10102420` `10102480` |
| `cc_flush` | Flushes async'd captions. |  |  |
| `cc_lang` | Current close caption language (emtpy = use game UI language |  |  |
| `cc_linger_time` | 1.0 | Close caption linger time. | `100fe220` |
| `cc_minvisibleitems` | 1 | Minimum number of caption items to show. | `100ff120` |
| `cc_predisplay_time` | 0.25 | Close caption delay before showing caption. | `100fdbf0` |
| `cc_random` | Emits a random caption |  |  |
| `cc_sentencecaptionnorepeat` | 4 | How often a sentence can repeat. |  |
| `cc_showblocks` | Toggles showing which blocks are pending/loaded async. |  |  |
| `cc_showmissing` | 0 | Show missing closecaption entries. |  |
| `cc_subtitles` | 0 | If set, don't show sound effect captions, just voice overs (i.e., won't help hearing impaired players). | `100fe220` |
| `cl_addon_debug` | 0 |  |  |
| `cl_addon_fade_max` | 1000 |  | `101d3f20` |
| `cl_addon_fade_min` | 800 |  | `101d3f20` |
| `cl_aggregate_particles` | 1 |  | `10130c10` |
| `cl_anglespeedkey` | 0.67 |  | `101131e0` |
| `cl_animationinfo` | Hud element to examine. |  | `100f6c10` `100f6c20` `100f7550` +1 |
| `cl_autohelp` | 1 | Auto-help |  |
| `cl_bob` | 0.002 |  |  |
| `cl_bobcycle` | 0.8 |  | `101e4620` |
| `cl_bobup` | 0.5 |  | `101e4620` |
| `cl_buy_favorite` | Purchase a favorite weapon/equipment loadout |  |  |
| `cl_buy_favorite_nowarn` | 0 | Skips the error prompt when saving an invalid buy favorite |  |
| `cl_buy_favorite_quiet` | 0 | Skips the prompt when saving a buy favorite in the buy menu |  |
| `cl_buy_favorite_reset` | Reset favorite loadouts to the default |  | `101cfb80` |
| `cl_buy_favorite_set` | Saves the current loadout as a favorite |  |  |
| `cl_c4dynamiclight` | 0 | Draw dynamic light when planted c4 flashes |  |
| `cl_chat_active` | 0 |  | `100f85e0` `100f8890` `100fa4e0` +1 |
| `cl_chat_wipehistory` | 0 | Wipes chat history after all chat text faded out | `100f9210` |
| `cl_chatfilters` | 31 | Stores the chat filter settings  | `100f6d20` `100f7330` `100fa4e0` |
| `cl_cinematiclight_b` | 0.05 |  | `10214bb0` `103af3a0` |
| `cl_cinematiclight_g` | 0.3 |  | `10214bb0` |
| `cl_cinematiclight_r` | 3.0 |  | `10214bb0` |
| `cl_cinematiclight_scale` | 7.0 |  | `10214bb0` |
| `cl_cinematiclight_tonemap` | 0.4 |  | `10214bb0` |
| `cl_clearhinthistory` | Clear memory of client side hints displayed to the player. |  | `100eef30` `100eef90` `100eeff0` +3 |
| `cl_clientmenudistancecap` | 10 | Furthest the client menu will think the mouse has gone. |  |
| `cl_clientmenuscalefactor` | 0.05 | Multiplier to the mouse's units to get the client menu's units. |  |
| `cl_color_survivor` | 138 182 220 |  |  |
| `cl_color_zombie` | 209 199 151 |  |  |
| `cl_colorfastpath` | 0 |  | `1011f820` |
| `cl_crash` | Causes a client crash for testing |  |  |
| `cl_create_server` | ? |  | `10276ca0` `10276e70` |
| `cl_crosshair_alpha` | 255 | Crosshair alpha | `10263aa0` |
| `cl_crosshair_blue` | 220 | Crosshair blue component | `10263aa0` |
| `cl_crosshair_center_dot` | 0 | Crosshair center dot |  |
| `cl_crosshair_circle_alpha` | 0 | Crosshair circle alpha | `10263aa0` |
| `cl_crosshair_circle_mode` | 0 | Crosshair circle mode (0=actual aim error + pellet scatter, 1=green is aim error, red is pellet scatter, 2=inside of cro | `10263aa0` |
| `cl_crosshair_dynamic` | 1 | Crosshair scales based on accuracy | `10263aa0` |
| `cl_crosshair_green` | 182 | Crosshair green component | `10263aa0` |
| `cl_crosshair_red` | 138 | Crosshair red component | `10263aa0` |
| `cl_crosshair_shotgun_center_dot` | 1 | Crosshair center dot for shotguns |  |
| `cl_crosshair_thickness` | 1 | Crosshair thickness in pixels | `10263aa0` |
| `cl_customsounds` | 0 | Enable customized player sound playback |  |
| `cl_cycle_latch_tolerance` | 0.125 | How close an incoming Cycle reminder can be for us to ignore the input and trust the value we have. |  |
| `cl_damageindicator_3d` | 1 | EjectBrass_338Mag | `101e18a0` |
| `cl_debug_player_use` | 0 | Visualizes +use logic for the given range. Green cross=trace success, Red cross=trace too far, Green box=radius success |  |
| `cl_demo_polish_print_time` | Show the demo polish options dialog |  | `100669f0` `100699d0` `10069a30` +14 |
| `cl_demoviewoverride` | 0 | Override view during demo playback | `10177710` `10177e50` |
| `cl_detail_avoid_force` | 0 | force with which to avoid players ( in units, percentage of the width of the detail sprite ) | `100cfbd0` `101d5f60` |
| `cl_detail_avoid_radius` | 0 | radius around detail sprite to avoid players | `100cfbd0` `101d5f60` |
| `cl_detail_avoid_recover_speed` | 0 | how fast to recover position after avoiding players | `100cfbd0` `101d5f60` |
| `cl_detail_max_sway` | 0 | Amplitude of the detail prop sway | `100d05d0` `100d0a60` `100d0fc0` +1 |
| `cl_detail_multiplier` | 1 | extra details to create | `100cf3e0` `100d3ce0` |
| `cl_detaildist` | 1200 | Distance at which detail props are no longer visible | `100ccd40` |
| `cl_detailfade` | 400 | Distance across which detail props fade in | `100ccd40` |
| `cl_disable_ragdolls` | 0 |  | `1003d430` |
| `cl_drawescaperoute` | 0 |  | `10209e00` |
| `cl_drawhud` | 1 | Enable the rendering of the hud | `100b52c0` |
| `cl_drawleaf` | -1 |  |  |
| `cl_drawmaterial` | Draw a particular material over the frame | viewangleanim | `101804b0` |
| `cl_drawmonitors` | 1 |  |  |
| `cl_drawprogressbar` | 1 | Draw progress bar when healing |  |
| `cl_drawshadowtexture` | 0 |  | `100c0020` `101804b0` |
| `cl_dump_particle_stats` | dump particle profiling info to particle_profile.csv |  | `101314c0` |
| `cl_dumpplayer` | Dumps info about a player |  | `1010b380` `1010b3e0` `1010b440` +1 |
| `cl_dumpsplithacks` | Dump split screen workarounds. |  |  |
| `cl_ejectbrass` | 1 |  |  |
| `cl_elevator_physics` | 1 |  |  |
| `cl_ent_absbox` | Displays the client's absbox for the entity under the crossh |  |  |
| `cl_ent_bbox` | Displays the client's bounding box for the entity under the  |  |  |
| `cl_ent_rbox` | Displays the client's render box for the entity under the cr |  |  |
| `cl_extrapolate` | 1 | Enable/disable extrapolation if interpolation history runs out. | `1004fcb0` |
| `cl_extrapolate_amount` | 0.25 | Set how many seconds the client will extrapolate entities for. | `1003ee60` `1003f010` `10044470` +5 |
| `cl_fastdetailsprites` | 1 | whether to use new detail sprite system | `100cf3e0` `100d3ce0` |
| `cl_fastradial` | 1 | If 1, releasing the button on a radial menu executes the highlighted button |  |
| `cl_fasttempentcollision` | 5 |  | `101b49a0` |
| `cl_find_ent` | Find and list all client entities with classnames that conta |  |  |
| `cl_find_ent_index` | Display data for clientside entity matching specified index. |  | `100461e0` `10047380` |
| `cl_footstep_fx` | 0 |  | `100d8850` |
| `cl_force_3rd_strike` | 0 |  | `10213b90` `10215d30` |
| `cl_fov` | ? |  | `101727d0` `10175270` `10175550` +4 |
| `cl_glow_ability_b` | 0.0 | Color of ability glow | `101ffca0` `10212c00` |
| `cl_glow_ability_colorblind_b` | 1.0 | Color of ability glow | `101ffca0` `10212c00` |
| `cl_glow_ability_colorblind_g` | 1.0 | Color of ability glow | `101ffca0` `10212c00` |
| `cl_glow_ability_colorblind_r` | 0.3 | Color of ability glow | `101ffca0` `10212c00` |
| `cl_glow_ability_g` | 0.0 | Color of ability glow | `101ffca0` `10212c00` |
| `cl_glow_ability_r` | 1.0 | Color of ability glow | `101ffca0` `10212c00` |
| `cl_glow_blur_scale` | 3 | Controls the size of the halo shown around players and usable items |  |
| `cl_glow_brightness` | 1 | Brightness of player halos | `101ffca0` `10201660` `10212c00` |
| `cl_glow_debug` | 0 |  | `102140e0` |
| `cl_glow_force` | 255 | Forces glows on | `101fdfc0` |
| `cl_glow_ghost_infected_b` | 1.0 | Color of infected ghost glow | `101ffca0` |
| `cl_glow_ghost_infected_g` | 0.4 | Color of infected ghost glow | `101ffca0` |
| `cl_glow_ghost_infected_r` | 0.3 | Color of infected ghost glow | `101ffca0` |
| `cl_glow_infected` | 1 | Do infected get to see each other? | `10201660` |
| `cl_glow_infected_b` | 1.0 | Color of infected glow | `101ffca0` |
| `cl_glow_infected_g` | 0.4 | Color of infected glow | `101ffca0` |
| `cl_glow_infected_ghosts` | 1 | Do infected get to see ghost glows? | `10201660` |
| `cl_glow_infected_r` | 0.3 | Color of infected glow | `101ffca0` |
| `cl_glow_infected_vomit_b` | 0.72 | Color the PZs see the IT victim glow | `101ffca0` |
| `cl_glow_infected_vomit_g` | 0.07 | Color the PZs see the IT victim glow | `101ffca0` |
| `cl_glow_infected_vomit_r` | 0.79 | Color the PZs see the IT victim glow | `101ffca0` |
| `cl_glow_item_b` | 1.0 |  | `10212c00` |
| `cl_glow_item_far_b` | 1.0 |  | `10212c00` |
| `cl_glow_item_far_g` | 0.4 |  | `10212c00` |
| `cl_glow_item_far_r` | 0.3 |  | `10212c00` |
| `cl_glow_item_g` | 0.7 |  | `10212c00` |
| `cl_glow_item_r` | 0.7 |  | `10212c00` |
| `cl_glow_los_delay` | 0.0 | Time out of sight before a survivor friend shows up through a wall to a survivor. | `10201660` |
| `cl_glow_los_fade_in_time` | 0.5 | Time after cl_glow_los_delay before a survivor friend shows up fully through wall. | `10201660` |
| `cl_glow_los_fade_out_time` | 0.5 | Time after cl_glow_los_delay before a survivor friend goes away | `10201660` |
| `cl_glow_noise` | 1 | Is Survivor glow seen by infected based on noise? | `10201660` |
| `cl_glow_physics_props` | 0 |  |  |
| `cl_glow_range_end` | 1500 |  |  |
| `cl_glow_range_exp` | 1.0 |  | `10201660` |
| `cl_glow_range_start` | 100 |  |  |
| `cl_glow_survivor_b` | 1.0 | Color of survivor team mate glow | `101ffca0` |
| `cl_glow_survivor_g` | 0.4 | Color of survivor team mate glow | `101ffca0` |
| `cl_glow_survivor_health_high_b` | 0.196 | Color the Infected see Survivors when their health is high | `101ffca0` |
| `cl_glow_survivor_health_high_colorblind_b` | 0.392 | Color the Infected see Survivors when their health is high | `101ffca0` |
| `cl_glow_survivor_health_high_colorblind_g` | 0.694 | Color the Infected see Survivors when their health is high | `101ffca0` |
| `cl_glow_survivor_health_high_colorblind_r` | 0.047 | Color the Infected see Survivors when their health is high | `101ffca0` |
| `cl_glow_survivor_health_high_g` | 0.69 | Color the Infected see Survivors when their health is high | `101ffca0` |
| `cl_glow_survivor_health_high_r` | 0.039 | Color the Infected see Survivors when their health is high | `101ffca0` |
| `cl_glow_survivor_health_low_b` | 0.098 | Color the Infected see Survivors when their health is low | `101ffca0` |
| `cl_glow_survivor_health_low_colorblind_b` | 0.807 | Color the Infected see Survivors when their health is low | `101ffca0` |
| `cl_glow_survivor_health_low_colorblind_g` | 0.807 | Color the Infected see Survivors when their health is low | `101ffca0` |
| `cl_glow_survivor_health_low_colorblind_r` | 0.047 | Color the Infected see Survivors when their health is low | `101ffca0` |
| `cl_glow_survivor_health_low_g` | 0.098 | Color the Infected see Survivors when their health is low | `101ffca0` |
| `cl_glow_survivor_health_low_r` | 0.63 | Color the Infected see Survivors when their health is low | `101ffca0` |
| `cl_glow_survivor_health_med_b` | 0.032 | Color the Infected see Survivors when their health is medium | `101ffca0` |
| `cl_glow_survivor_health_med_colorblind_b` | 0.098 | Color the Infected see Survivors when their health is medium | `101ffca0` |
| `cl_glow_survivor_health_med_colorblind_g` | 0.573 | Color the Infected see Survivors when their health is medium | `101ffca0` |
| `cl_glow_survivor_health_med_colorblind_r` | 0.694 | Color the Infected see Survivors when their health is medium | `101ffca0` |
| `cl_glow_survivor_health_med_g` | 0.4 | Color the Infected see Survivors when their health is medium | `101ffca0` |
| `cl_glow_survivor_health_med_r` | 0.59 | Color the Infected see Survivors when their health is medium | `101ffca0` |
| `cl_glow_survivor_hurt_b` | 0.0 | Color of survivor team mate glow when incapacitated | `101ffca0` |
| `cl_glow_survivor_hurt_g` | 0.4 | Color of survivor team mate glow when incapacitated | `101ffca0` |
| `cl_glow_survivor_hurt_r` | 1.0 | Color of survivor team mate glow when incapacitated | `101ffca0` |
| `cl_glow_survivor_r` | 0.3 | Color of survivor team mate glow | `101ffca0` |
| `cl_glow_survivor_vomit_b` | 0.0 | Color the Survivors see the IT victim glow | `101ffca0` |
| `cl_glow_survivor_vomit_g` | 0.4 | Color the Survivors see the IT victim glow | `101ffca0` |
| `cl_glow_survivor_vomit_r` | 1.0 | Color the Survivors see the IT victim glow | `101ffca0` |
| `cl_glow_thirdstrike_item_b` | 0.0 |  | `10212c00` |
| `cl_glow_thirdstrike_item_colorblind_b` | 1.0 |  | `10212c00` |
| `cl_glow_thirdstrike_item_colorblind_g` | 1.0 |  | `10212c00` |
| `cl_glow_thirdstrike_item_colorblind_r` | 0.3 |  | `10212c00` |
| `cl_glow_thirdstrike_item_g` | 0.0 |  | `10212c00` |
| `cl_glow_thirdstrike_item_r` | 1.0 |  | `10212c00` |
| `cl_hidemenu_spawnclass_boomer` | 0 |  |  |
| `cl_hidemenu_spawnclass_hunter` | 0 |  |  |
| `cl_hidemenu_spawnclass_smoker` | 0 |  |  |
| `cl_hidemenu_spawnmode` | 0 | KeybindLabel |  |
| `cl_ideal_spec_mode` | 5 | desired spectator mode (4 = in-eye, 5 = chase, 6 = roaming) |  |
| `cl_idealpitchscale` | 0.8 |  |  |
| `cl_impacteffects` | 1 |  | `10260090` `102601b0` |
| `cl_impacteffects_limit_exit` | 3 | Limits the number of exit impact effects per frame |  |
| `cl_impacteffects_limit_general` | 10 | Limits the number of impact effects per frame |  |
| `cl_impacteffects_limit_water` | 2 | Limits the number of water impact effects per frame |  |
| `cl_interp_all` | 0 | Disable interpolation list optimizations. | `1004c7d0` |
| `cl_interp_npcs` | 0.0 | Interpolate NPC positions starting this many seconds in past (or cl_interp, if greater) | `1004a8b0` `1004d0b0` |
| `cl_interp_ratio` | 2.0 | Sets the interpolation amount (final amount is cl_interp_ratio / cl_updaterate). | `1009fcb0` |
| `cl_interp_threadmodeticks` | 0 | Additional interpolation ticks to use when interpolating with threaded engine mode set. | `1004a970` `1004fcb0` |
| `cl_interpolate` | 1.0f |  | `1004fcb0` |
| `cl_jiggle_bone_debug` | 0 | IsNPC | `10115b70` |
| `cl_jiggle_bone_debug_pitch_constraints` | 0 | Display physics-based 'jiggle bone' debugging information | `10115b70` |
| `cl_jiggle_bone_debug_yaw_constraints` | 0 | Display physics-based 'jiggle bone' debugging information | `10115b70` |
| `cl_jiggle_bone_invert` | 0 |  | `10115b70` |
| `cl_jiggle_bone_sanity` | 1 | Prevent jiggle bones from pointing directly away from their target in case of numerical instability. | `10115b70` |
| `cl_lagcomp_errorcheck` | 0 | Player index of other player to check for position errors. |  |
| `cl_lagcompensation` | 1 | Perform server side lag compensation of weapon firing events. |  |
| `cl_language` | ? |  | `102de380` `102de610` |
| `cl_leafsystemvis` | 0 |  | `100b3470` |
| `cl_left_hand_ik` | 0 | Attach player's left hand to rifle with IK. |  |
| `cl_leveloverview` | 0 |  | `100b12a0` `10178640` `10185ed0` +3 |
| `cl_leveloverviewmarker` | 0 |  |  |
| `cl_maxrenderable_dist` | 3000 | Max distance from the camera at which things will be rendered | `10185f80` |
| `cl_menuduration` | 10 | Client menus will hide after this many seconds (-1 disables)  |  |
| `cl_modelfastpath` | 1 |  | `1018d410` |
| `cl_mouseenable` | 1 |  | `10133280` |
| `cl_movesetscycle` | 0 |  | `101dbbe0` |
| `cl_music_debug` | 0 |  | `1020e2b0` |
| `cl_names_debug` | 0 |  | `10087df0` |
| `cl_new_impact_effects` | 1 | RagdollImpact | `100e07a0` |
| `cl_observercrosshair` | 1 |  | `10101f80` |
| `cl_overdraw_test` | 0 |  | `1017f270` |
| `cl_panelanimation` | Shows panel animation variables: <panelname / blank for all  |  | `10171940` `10171cb0` `10171cf0` +7 |
| `cl_particle_batch_mode` | 1 |  | `10128240` |
| `cl_particle_fallback_base` | 0 | Base for falling back to cheaper effects under load. | `1012e9a0` |
| `cl_particle_fallback_multiplier` | 1 | Multiplier for falling back to cheaper effects under load. | `1012e9a0` |
| `cl_particle_max_count` | 0 |  | `1012e740` |
| `cl_particle_retire_cost` | 0 |  | `10129ce0` |
| `cl_particles_dump_effects` | ? |  | `101306f0` |
| `cl_particles_show_bbox` | 0 |  | `101306f0` |
| `cl_pclass` | Dump entity by prediction classname. |  | `10146a20` |
| `cl_pdump` | -1 | Dump info about this entity to screen. | `10146a20` `101486f0` |
| `cl_perf_wizard_enable` | 1 | Enables a performance-tuning wizard |  |
| `cl_phys_block_dist` | 1.0 |  | `10135890` |
| `cl_phys_block_fraction` | 0.1 |  | `10135890` |
| `cl_phys_maxticks` | 0 | Sets the max number of physics ticks allowed for client-side physics (ragdolls) | `10135890` |
| `cl_pitchdown` | 89 |  | `1010fcd0` `10112860` `10114350` +1 |
| `cl_pitchspeed` | 225 |  | `1010fcd0` `101132a0` |
| `cl_pitchup` | 89 |  | `1010fcd0` `10112860` `10114350` +1 |
| `cl_player_max_decal_count` | 2 |  |  |
| `cl_player_max_separation_force` | 500 |  | `10210440` |
| `cl_player_shadow_dist` | 1000 |  | `101d0f40` |
| `cl_playerspraydisable` | 0 | Disable player sprays. | `101ba140` |
| `cl_pred_doresetlatch` | 1 |  | `10147b80` |
| `cl_pred_error_verbose` | 0 | Show more field info when spewing prediction errors. | `101497c0` |
| `cl_pred_optimize` | 2 | Optimize for not copying data if didn't receive a network update (1), and also for not repredicting if there were no err | `10147b80` `10147ea0` |
| `cl_pred_track` | <entindex> <fieldname>:  Track changes to entity index entin |  | `1014cc20` |
| `cl_predict` | 1.0 | Camo | `10050380` `1005f340` `100a2320` +8 |
| `cl_predictioncopy_describe` | Describe datamap_t for entindex |  |  |
| `cl_predictionlist` | 0 | Show which entities are predicting  | `101486f0` |
| `cl_predictweapons` | 1 | Perform client side prediction of weapon effects. |  |
| `cl_ragdoll_collide` | 0 |  | `1014fd30` |
| `cl_ragdoll_force_multiplier` | 8 | Multiplies the damage force applied to new ragdolls |  |
| `cl_ragdoll_gravity` | 386 | Sets the gravity client-side ragdolls | `10133c40` |
| `cl_ragdoll_limit` | 20 | Maximum number of ragdolls to show (-1 disables limit) |  |
| `cl_ragdoll_maxcount` | 25 | Clients will only show this many ragdolls overall | `1021ed50` `1021f820` |
| `cl_ragdoll_maxcount_boss` | 1 | Clients will only show this many boss ragdolls |  |
| `cl_ragdoll_maxcount_generic` | 15 | Clients will only show this many generic infected ragdolls |  |
| `cl_ragdoll_maxcount_gib` | 10 | Clients will only show this many gib ragdolls |  |
| `cl_ragdoll_maxcount_special` | 3 | Clients will only show this many special ragdolls |  |
| `cl_ragdoll_maxcount_survivor` | 4 | Clients will only show this many survivor ragdolls |  |
| `cl_ragdoll_physics_enable` | 1 | Enable/disable ragdoll physics. |  |
| `cl_ragdoll_self_collision` | 1 | Pupil | `1014fd30` |
| `cl_reloadpostprocessparams` | ? |  | `102126e0` `10212700` `103af3a0` |
| `cl_removedecals` | Remove the decals from the entity under the crosshair. |  |  |
| `cl_report_soundpatch` | reports client-side sound patch count |  | `1015f9c0` `10160540` `10160db0` |
| `cl_ricochet_percent` | 3 | Percent chance a bullet will create a ricochet tracer (0..10) | `10260090` |
| `cl_ricochet_percent_shotgun` | 0 | Percent chance a shotgun pellet will create a ricochet tracer (0..10) | `10260090` |
| `cl_rosetta_debug` | 0 |  | `10235c50` `102360a0` `10236170` +3 |
| `cl_rosetta_line_inner_radius` | 25 |  | `10235c50` |
| `cl_rosetta_line_outer_radius` | 45 |  | `10235c50` |
| `cl_shadowtextureoverlaysize` | 256 |  | `101804b0` |
| `cl_show_bounds_errors` | 0 |  | `100c52a0` |
| `cl_show_demo_polish_options_dlg` | Show the demo polish options dialog |  | `1006c7f0` `1006dba0` |
| `cl_show_path` | 1 |  | `10209a50` |
| `cl_show_splashes` | 1 | PrecacheEffectSplash | `100e2bc0` `100e2f20` `100e2f90` |
| `cl_show_tickets` | 0 |  | `1024cd00` |
| `cl_showanimstate` | -1 | AnimateSpecificTexture | `1000e990` `10274090` `10274a60` |
| `cl_showanimstate_activities` | 1 | Show activities in the (client) animation state display. | `1000fa40` |
| `cl_showanimstate_log` | 0 | 1 to output cl_showanimstate to Msg(). 2 to store in AnimStateClient.log. 3 for both. | `1000eed0` |
| `cl_showerror` | 0 | Show prediction errors, 2 for above plus detailed field deltas. | `1004f3e0` `10148430` |
| `cl_showfps` | 0 | Draw fps meter at top of screen (1 = fps, 2 = smooth fps, 3 = server MS, 4 = Show FPS and Log to file ) | `101706e0` |
| `cl_showhelp` | 1 | Set to 0 to not show on-screen help |  |
| `cl_showing_stats_crawl` | 0 |  | `100ff120` `10239970` `1023a6e0` +2 |
| `cl_showpausedimage` | 1 | Show the 'Paused' image when game is paused. | `10172210` |
| `cl_showpos` | 0 | Draw current position at top of screen | `101706e0` |
| `cl_showposeparams` | 1 |  | `101dd120` |
| `cl_showtextmsg` | 1 | Enable/disable text messages printing on the screen. | `100f7c40` |
| `cl_skipfastpath` | 0 | Set to 1 to stop all models that go through the model fast path from rendering | `1011fea0` |
| `cl_skipslowpath` | 0 | Set to 1 to skip any models that don't go through the model fast path |  |
| `cl_smoke_alpha` | 0.4 |  | `101a0260` |
| `cl_smoke_far` | 150 |  | `101a0260` |
| `cl_smoke_fog_percent` | 0 |  | `101a0cf0` |
| `cl_smoke_near` | 0 |  | `101a0260` |
| `cl_smooth` | 1 | Smooth view/eye origin after prediction errors | `1005f340` |
| `cl_smoothtime` | 0.1 | Smooth client's view after prediction error over this many seconds | `1005f340` |
| `cl_soundemitter_flush` | Flushes the sounds.txt system (client only) |  |  |
| `cl_soundscape_flush` | Flushes the client side soundscapes |  |  |
| `cl_soundscape_printdebuginfo` | print soundscapes |  |  |
| `cl_sporeclipdistance` | 512 |  | `101a4320` |
| `cl_ss_origin` | print origin in script format |  | `100964a0` |
| `cl_sun_decay_rate` | 0.05 |  | `100ec5c0` |
| `cl_survivor_light_brightness` | 4 |  | `101fc390` |
| `cl_survivor_light_cone` | 0 |  | `101fc390` |
| `cl_survivor_light_debug` | 0 |  | `101fc390` |
| `cl_survivor_light_enable` | 1 |  | `101fc390` |
| `cl_survivor_light_offset_x` | 5 |  | `101fc390` |
| `cl_survivor_light_offset_y` | 2 |  | `101fc390` |
| `cl_survivor_light_offset_z` | 5 |  | `101fc390` |
| `cl_survivor_light_radius` | 12 |  | `101fc390` |
| `cl_survivor_light_rot` | 0 |  | `101fc390` |
| `cl_temp_health_fade` | 0 | temp health fades top to bottom | `1022a200` |
| `cl_temp_health_pulse_incap` | 0 | incap health pulses in the same fashion as temp health | `1022a200` |
| `cl_temp_health_pulse_rate` | 2.5 | HealthPanel | `1022a200` |
| `cl_threaded_bone_setup` | 0 | Enable parallel processing of C_BaseAnimating::SetupBones() | `10040db0` |
| `cl_threaded_init` | 0 | VClient016 | `100a6330` |
| `cl_tracer_whiz_distance` | 72 |  | `100c3d10` |
| `cl_tracers` | 1 |  | `10260090` `102615b0` |
| `cl_transition_blur_fade_time` | 2.0 |  |  |
| `cl_updatevisibility` | Updates visibility bits. |  | `10048470` `10049a50` `1004c6a0` +2 |
| `cl_upspeed` | 320 |  | `10112a80` |
| `cl_use_simd_bones` | 1 | 1 use SIMD bones 0 use scalar bones. | `10033d30` |
| `cl_use_update_interval` | .2 | Time between use target updates | `10205470` |
| `cl_viewbob` | 1 | enables view bob | `101fcdc0` |
| `cl_viewbob_cyclerate_duck` | 0.6 | Speed of view bob while ducked | `101fcdc0` |
| `cl_viewbob_cyclerate_run` | 0.4 | Speed of view bob while running | `101fcdc0` |
| `cl_viewbob_cyclerate_walk` | 0.8 | Speed of view bob while walking | `101fcdc0` |
| `cl_viewbob_scale` | 0.01 | Size of view bob | `101fcdc0` |
| `cl_viewbob_scale_duck` | 0.02 | Size of view bob while ducked | `101fcdc0` |
| `cl_viewbob_up` | 0.5 | Percent of view bob that is above the eye pos | `101fcdc0` |
| `cl_viewmodelfovboomer` | ? |  | `101727d0` `10175270` `10175550` |
| `cl_viewmodelfovhunter` | ? |  | `101727d0` `10175270` `10175550` +4 |
| `cl_viewmodelfovsmoker` | ? |  | `101727d0` `10175270` `10175550` |
| `cl_viewmodelfovsurvivor` | ? |  | `101727d0` `10175270` `10175550` +4 |
| `cl_viewmodelfovtank` | ? |  | `101727d0` `10175270` `10175550` +1 |
| `cl_viewtarget_clamp` | 1 |  | `100514c0` |
| `cl_viewtarget_debug` | 0 |  | `10201000` |
| `cl_viewtarget_offset` | 0 |  | `10202340` |
| `cl_viewtarget_player` | 0 | Lock 3rd person spec looking into target's eyes | `10202340` |
| `cl_vote_active` | 0 | CHoldoutModeScoreboard | `1024ec50` `1024f790` |
| `cl_winddir` | 0 | Weather effects wind direction angle | `101959f0` `10195f50` |
| `cl_windspeed` | 0 | Weather effects wind speed scalar | `10195f50` `10198d10` |
| `cl_witch_light_brightness` | 5 |  | `10275f90` |
| `cl_witch_light_color_b` | 0 |  | `10275f90` |
| `cl_witch_light_color_g` | 0 |  | `10270ca0` `10275f90` |
| `cl_witch_light_color_r` | 255 |  | `10275f90` |
| `cl_witch_light_cone` | 45 |  | `10275f90` |
| `cl_witch_light_debug` | 0 |  | `10275f90` |
| `cl_witch_light_enable` | 1 |  | `10275f90` |
| `cl_witch_light_offset_x` | 10 |  | `10275f90` |
| `cl_witch_light_offset_y` | 0 |  | `10275f90` |
| `cl_witch_light_offset_z` | 35 |  | `10275f90` |
| `cl_witch_light_offset_z_max` | 30 |  | `10275f90` |
| `cl_witch_light_radius` | 60 |  | `10275f90` |
| `cl_wpn_sway_interp` | 0.1 |  |  |
| `cl_wpn_sway_scale` | 1.0 |  |  |
| `cl_yawspeed` | 210 |  | `1010fcd0` `101126a0` |
| `claw_force` | 240 | Force with witch the claw shoves other zombies away |  |
| `claw_plays_hit_anims` | 0 | Play the new hit anims for debugging? | `10262fa0` |
| `claw_range` | 52 | Range of the Claw weapon | `10262e30` |
| `claw_range_down` | 70 | Range of the Claw weapon when looking straight down, so it can reach your feet from your eyes. | `10262e30` |
| `claw_swing_duration` | 0.2 |  | `10262e30` |
| `claw_swing_interval` | 1 |  | `10262e30` `10263000` |
| `claw_swing_miss_interval` | 0.5 |  | `10263000` |
| `command_number` | WriteUsercmd: from=%d to=%d  |  | `1016b660` |
| `commentary_showmodelviewer` | Display the commentary model viewer. Usage: commentary_showm |  | `100c8e40` `100c8fd0` |
| `con_enable` | ? |  | `101f0890` `10233e60` |
| `cpu_level` | 2 | CPU Level - Default: High | `100a0d40` `100a0d80` |
| `cs_gamerules_data` | DT_CSGameRulesProxy | baseclass | `101d8860` `1021c130` |
| `debug_materialmodifycontrol_client` | 0 |  | `10082a50` |
| `debug_zombie_panel` | 0 | -1 = Too Far 1 = Tank 2 = Someone else tank | `10247120` `102471f0` `102472c0` +1 |
| `default_fov` | 90 |  | `1016e080` `10177e50` `10178440` +1 |
| `demo_polish_auto_polish` | 1 | When enabled will cause a recorded demo to be polished automatically on end record. | `1006e820` |
| `demo_polish_bone_overrides_enabled` | 1 | Enable bone overrides in the bone path for polished demo playback. |  |
| `demo_polish_bone_test_index` | -1 | Test bone index. | `1006b340` |
| `demo_polish_draw_next_skeleton_frames` | 0 | Draw next frames? | `1006b1d0` |
| `demo_polish_draw_path_enabled` | 0 | Draw player path? | `1006af90` |
| `demo_polish_draw_prev_skeleton_frames` | 0 | Draw previous frames? | `1006b1d0` |
| `demo_polish_enabled` | 0 | Enable demo polishing during demo record & playback. | `10069620` `10069630` `10069660` +1 |
| `demo_polish_event_time_offset` | 0 |  | `100741f0` |
| `demo_polish_global_adjustments_enabled` | 0 | Enable global adjustments to skeleton during polished demo playback. | `1006b770` |
| `demo_polish_ik_enabled` | 1 | Enable IK in polished demos. |  |
| `demo_polish_jitter_enabled` | 0 | Rapidly toggle demo_polish_enabled on/off. | `10069690` |
| `demo_polish_local_adjustments_enabled` | 0 | Enable local adjustments to skeleton during polished demo playback. | `1006b340` |
| `demo_polish_playback_offset` | 0.02 |  |  |
| `demo_polish_root_adjustments_enabled` | 0 | Enable root adjustments to skeleton during polished demo playback. |  |
| `demo_polish_stand_in_place_time_overlap_threshold` | .5 |  | `1006e4d0` |
| `demo_polish_step_duration` | .2 |  |  |
| `demo_polish_step_in_place_enabled` | 1 | Enable stepping in place in polished demos. |  |
| `demo_polish_step_lift` | 1 |  |  |
| `demo_returntolobby` | ? |  | `10214bb0` |
| `director_intensity_threshold` | 0.9 |  | `1022cc10` |
| `director_show_intensity` | 0 |  |  |
| `dlight_debug` | Creates a dlight in front of the player |  |  |
| `enable_skeleton_draw` | 0 | Render skeletons in wireframe | `1003f790` |
| `engine_post` | 0 |  | `10184bf0` |
| `env_fire_large` | ? |  | `10074aa0` `10074bf0` |
| `env_fire_large_smoke` | ? |  | `10074aa0` `10074bf0` `10155fa0` |
| `env_fire_medium` | ? |  | `10074aa0` |
| `env_fire_medium_smoke` | ? |  | `10074aa0` |
| `env_fire_small` | ? |  | `10074aa0` |
| `env_fire_small_smoke` | ? |  | `10074aa0` |
| `env_fire_tiny_smoke` | ? |  | `10074aa0` |
| `first_aid_kit_continuous_fire` | 1 | Players need to hold +attack to heal with the first aid kit | `10265030` `102650a0` `102658d0` |
| `first_aid_kit_heal_other_attack2` | 1 | Players need to hold +attack2 to heal others with the first aid kit | `10265030` `102650a0` `102658d0` |
| `first_aid_kit_max_heal` | 100 |  | `10264f40` |
| `first_aid_kit_range` | 100 |  |  |
| `first_aid_kit_use_duration` | 5 |  |  |
| `first_aid_kit_view_duration` | 0.5 | time it takes to rotate the view when healing others | `101fe080` `101ff1b0` |
| `fish_debug` | 0 | Show debug info for fish | `100753a0` |
| `flex_rules` | 1 | Allow flex animation rules to run. | `100543f0` |
| `flex_smooth` | 1 | Applies smoothing/decay curve to flex animation controller changes. | `1011e4c0` |
| `fly_add_keyframe` | ? |  |  |
| `fly_clear_all_keyframes` | ? |  |  |
| `fly_show_path` | ? |  | `101fb010` `10203a10` `102764d0` +4 |
| `fly_speed` | 200 |  |  |
| `fly_start` | ? |  |  |
| `fog_color` | -1 -1 -1 |  | `10187890` `1018cc20` |
| `fog_colorskybox` | -1 -1 -1 |  | `10187b40` `1018cc20` |
| `fog_enable` | 1 |  | `1018c820` `1018cc20` |
| `fog_enableskybox` | 1 |  | `10189a00` `1018cc20` |
| `fog_end` | -1 |  | `1018bb60` `1018c820` `1018cc20` |
| `fog_endskybox` | -1 |  | `10189a00` `1018cc20` |
| `fog_hdrcolorscale` | -1 |  | `10187890` `1018cc20` |
| `fog_hdrcolorscaleskybox` | -1 |  | `10187b40` `1018cc20` |
| `fog_maxdensity` | -1 |  | `1018bc10` `1018c820` `1018cc20` |
| `fog_maxdensityskybox` | -1 |  | `10185ed0` `1018cc20` |
| `fog_override` | 0 | Overrides the map's fog settings (-1 populates fog_ vars with map's values) | `10185ed0` `10187890` `10187b40` +5 |
| `fog_start` | -1 |  | `1018bab0` `1018c820` `1018cc20` |
| `fog_startskybox` | -1 |  | `10189a00` `1018cc20` |
| `force_centerview` | ? |  |  |
| `fov_desired` | ? |  | `101727d0` |
| `fs_convert` | 1 | Allow Xbox 360 files to be generated at runtime |  |
| `func_break_max_pieces` | 15 |  | `101b76c0` |
| `func_ragdoll_fader` | CFuncRagdollFader |  | `10220280` |
| `fx_drawimpactdebris` | 1 | Draw impact debris effects. | `1019c280` |
| `fx_drawimpactdust` | 1 | Draw impact dust effects. | `1019daa0` |
| `fx_drawmetalspark` | 1 | Draw metal spark effects. | `101c0fc0` |
| `g15_dumpplayer` | Spew player data. |  | `100a1de0` `100a2320` `100f14c0` +2 |
| `g15_reload` | Reloads the Logitech G-15 Keyboard configs. |  |  |
| `g15_update_msec` | 250 | Logitech G-15 Keyboard update interval. | `10109a40` |
| `g_debug_physcannon` | 0 |  |  |
| `g_debug_ragdoll_removal` | 0 |  | `1021eea0` `1021f820` |
| `g_debug_ragdoll_visualize` | 0 |  | `10037830` |
| `g_ragdoll_fadespeed` | 600 |  | `10035f60` |
| `g_ragdoll_lvfadespeed` | 100 |  | `10035f60` |
| `g_ragdoll_max_fps` | 30 | Clients with framerates less than this will reduce the ideal number of ragdolls to display |  |
| `g_ragdoll_min_fps` | 10 | Clients with framerates less than this will only show 1 ragdoll of each type |  |
| `gameinstructor_dump_open_lessons` | Gives a list of all currently open lessons. |  |  |
| `gameinstructor_enable` | 1 | Display in game lessons that teach new players. | `1007dc50` `1007f910` `10080400` |
| `gameinstructor_reload_lessons` | Shuts down all open lessons and reloads them from the script |  |  |
| `gameinstructor_reset_counts` | Resets all display and success counts to zero. |  |  |
| `gameinstructor_start_sound_cooldown` | 4.0 | Number of seconds forced between similar lesson start sounds. | `1007df80` |
| `gameinstructor_verbose` | 0 | Set to 1 for standard debugging or 2 (in combo with gameinstructor_verbose_lesson) to show update actions. | `10055df0` `10056730` `10056d50` +24 |
| `gameinstructor_verbose_lesson` | Display more verbose information for lessons have this name. |  | `10055b70` `1005ce60` |
| `gameui_ingame_menu_active` | Holds the state of the in game menu. Do not set this manuall |  | `10230d30` `1023d4a0` `1023fd60` +2 |
| `getpos_exact` | dump origin and angles to the console |  | `10178640` `10179190` |
| `gl_clear` | 0 |  |  |
| `gl_clear_randomcolor` | 0 | Clear the back buffer to random colors every frame. Helps spot open seams in geometry. |  |
| `gpu_level` | 3 | GPU Level - Default: High | `100a0d60` `100a0d80` |
| `gpu_mem_level` | 2 | Memory Level - Default: High | `100a0d80` |
| `hap_damagescale_game` | 1.0 | hap_HasDevice |  |
| `hap_melee_scale` | 0.8 |  |  |
| `hap_noclip_avatar_scale` | 0.10f |  | `100ee510` |
| `hap_tank_damage_scale` | 0.10 |  |  |
| `hap_ui_vehicles` | 0 |  |  |
| `hap_zombie_damage_scale` | 0.25 |  |  |
| `hide_message_panel` | Hides the message panel |  | `10231740` `10231890` |
| `hl2_episodic` | 0 |  | `101560d0` |
| `host_timescale` | ? |  | `102762a0` |
| `hud_autoreloadscript` | 0 | CModelPanel | `101c5310` `101c5580` |
| `hud_backswap` | 0 |  |  |
| `hud_classautokill` | 1 | Automatically kill player after choosing a new playerclass. |  |
| `hud_deathnotice_bots` | 1 | Show death notices for bots |  |
| `hud_deathnotice_threats` | 0 | Show death notices for Tanks and Threats |  |
| `hud_dmgrecord_persisttime_deaths` | 4.0 | Time to show death related lines in the damage record. | `10233e60` |
| `hud_dmgrecord_persisttime_other` | 2.0 | Time to show non-death related lines in the damage record. | `10233e60` |
| `hud_drawhistory_time` | 5 | WaterLOD | `100ee870` `100ef360` `100ef630` |
| `hud_fastswitch` | 1 | 1.25 | `101925a0` `101940b0` `101941e0` |
| `hud_holdout_target_time_range` | 5 |  | `1022c090` |
| `hud_reloadscheme` | Reloads hud layout and animation scripts. |  | `100b7640` `100bb160` `100bb2c0` +8 |
| `hud_saytext_time` | 12 |  | `100f6f60` `100f9610` |
| `hud_showtargetid` | 1 |  |  |
| `hud_takesshots` | 0 | Auto-save a scoreboard screenshot at the end of a map. |  |
| `hud_targetid_health` | 0 |  | `1023bfe0` |
| `hud_targetid_name_height` | 12 | Player names are this many inches over player eye positions | `1023bfe0` |
| `hud_targetid_name_height_boomer` | 12 | Player names are this many inches over player eye positions | `1023bfe0` |
| `hud_targetid_name_height_hunter` | 10 | Player names are this many inches over player eye positions | `1023bfe0` |
| `hud_targetid_name_height_smoker` | 25 | Player names are this many inches over player eye positions | `1023bfe0` |
| `hud_targetid_name_height_tank` | 12 | Player names are this many inches over player eye positions | `1023bfe0` |
| `hud_targetid_player_view_cone` | 0.92 | View cone for survivors seeing names of other survivors | `1023d4a0` |
| `hud_targetid_pz_ss_view_cone` | 0.98 | View cone for seeing player names as a PZ in splitscreen | `1023d4a0` |
| `hud_targetid_rangefinder` | shows range for different masks |  |  |
| `hud_targetid_skill` | 0 |  | `1023bfe0` |
| `hud_targetid_upgrades` | 0 |  | `1023bfe0` |
| `hud_winpanel_scoreanim_chingdelay` | 0.2 |  | `1025ab70` |
| `hud_winpanel_scoreanim_effectdelay` | 1.0 |  | `1025ab70` |
| `hud_winpanel_scoreanim_effectdelay_mult` | 0.7 |  | `1025ab70` |
| `hud_winpanel_scoreanim_scoredelay` | 1.8 |  | `1025ab70` |
| `hud_zombieteam` | 1 | Show infected team status on the HUD | `10247cd0` |
| `hud_zombieteam_showself` | 0 | Show your own status in the zombie team display | `10247b70` |
| `hunter_pz_claw_dmg` | 6 | Amount of damage done by a PZ hunter's regular melee attack |  |
| `in_forceuser` | 0 | Force user input to this split screen player. | `10112ce0` `101136f0` |
| `in_usekeyboardsampletime` | 1 | Use keyboard sample time smoothing. | `101131e0` |
| `inferno_dlight_spacing` | 200 | Inferno dlights are at least this far apart | `102268f0` |
| `inferno_dlights` | 30 | Min FPS at which molotov dlights will be created | `10226bd0` |
| `inferno_entity_flame_scale` | 0 |  |  |
| `inferno_fire` | 2 |  | `102270d0` `102274b0` |
| `inferno_particles` | ? |  | `102274b0` |
| `jal_quick_entry` | Debug entry point |  |  |
| `joy_accel_filter` | 0.2 |  | `1010f520` |
| `joy_accelmax` | 1.0 |  | `1010f520` |
| `joy_accelscale` | 0.6 |  | `1010f340` `1010f520` |
| `joy_advanced` | 0 |  | `1010f730` `1010fcd0` |
| `joy_advaxisr` | 0 |  | `1010f730` |
| `joy_advaxisu` | 0 |  | `1010f730` |
| `joy_advaxisv` | 0 |  | `1010f730` |
| `joy_advaxisx` | 0 |  | `1010f730` |
| `joy_advaxisy` | 0 |  | `1010f730` |
| `joy_advaxisz` | 0 |  | `1010f730` |
| `joy_autoaimdampen` | 0 | How much to scale user stick input when the gun is pointing at a valid target. | `1010f340` `1010f520` |
| `joy_autoaimdampenrange` | 0 | The stick range where autoaim dampening is applied. 0 = off | `1010f340` `1010f520` |
| `joy_autosprint` | 0 | Automatically sprint when moving with an analog joystick | `10060200` `1010fcd0` |
| `joy_cfg_preset` | 0 |  |  |
| `joy_circle_correct` | 1 |  | `1010fcd0` |
| `joy_diagonalpov` | 0 | POV manipulator operates on diagonal axes, too. |  |
| `joy_display_input` | 0 |  | `1010f340` `1010f520` |
| `joy_forwardsensitivity` | -1 |  | `1010fcd0` |
| `joy_forwardthreshold` | 0.15 |  | `1010fcd0` |
| `joy_inverty` | ? |  | `1010fcd0` |
| `joy_lookspin_default` | 0.35 |  | `1010fcd0` |
| `joy_lowend` | 1 |  | `1010f340` `1010f520` |
| `joy_lowmap` | 1 |  | `1010f340` `1010f520` |
| `joy_movement_stick` | Using joystick '%s' configuration  |  | `1010f730` `1010fcd0` |
| `joy_name` | joystick |  | `1010f730` |
| `joy_pegged` | 0.75 |  | `1010f340` `1010f520` |
| `joy_pitchsensitivity` | ? |  | `1010fcd0` |
| `joy_pitchsensitivity_default` | -1.0 |  |  |
| `joy_pitchthreshold` | 0.15 |  | `1010fcd0` |
| `joy_response_look` | 0 | 'Look' stick response mode: 0=Default, 1=Acceleration Promotion | `1010fcd0` |
| `joy_response_move` | 1 | 'Movement' stick response mode: 0=Linear, 1=quadratic, 2=cubic, 3=quadratic extreme, 4=power function(i.e., pow(x,1/sens | `1010fcd0` |
| `joy_response_move_vehicle` | 6 |  | `1010fcd0` |
| `joy_sensitive_step0` | 0.1 |  |  |
| `joy_sensitive_step1` | 0.4 |  |  |
| `joy_sensitive_step2` | 0.90 |  | `1010fcd0` |
| `joy_sidesensitivity` | 1 |  | `1010fcd0` |
| `joy_sidethreshold` | 0.15 |  | `1010fcd0` |
| `joy_vehicle_turn_lowend` | 0.7 |  | `1010f140` |
| `joy_vehicle_turn_lowmap` | 0.4 |  | `1010f140` |
| `joy_virtual_peg` | 0 |  | `1010f340` |
| `joy_wingmanwarrior_turnhack` | 0 | Wingman warrior hack related to turn axes. | `1010fcd0` |
| `joy_xcontroller_cfg_loaded` | 0 | If 0, the 360controller.cfg file will be executed on startup & option changes. | `1010f730` |
| `joy_yawsensitivity` | ? |  | `1010fcd0` |
| `joy_yawsensitivity_default` | -1.25 |  |  |
| `joy_yawthreshold` | 0.15 |  | `1010fcd0` |
| `l4d_garage01_` | ? |  | `101e5930` `101e5f70` |
| `l4d_stats_nogameplaycheck` | ? |  | `101f0890` `10233e60` |
| `ladder_autocenter` | 1 | Auto-center players on ladders. | `100e9c30` |
| `ladder_horizon` | -0.3 |  | `100e9c30` |
| `locator_fade_time` | 0.3 | Number of seconds it takes for a lesson to fully fade in/out. | `1022e870` `1022e8c0` `1022f210` +2 |
| `locator_lerp_rest` | 2.25f | Number of seconds before moving from the center. | `1022e9e0` `1022ea10` `1022ea40` +4 |
| `locator_lerp_speed` | 5.0f | Speed that static lessons move along the Y axis. | `1022fd90` |
| `locator_lerp_time` | 1.75f | Number of seconds to lerp before reaching final destination | `10230d30` |
| `locator_pulse_time` | 1.0f | Number of seconds to pulse after changing icon or position | `1022f5e0` |
| `lower_body` | 1 |  | `1020cb40` |
| `lower_body_debug` | 0 |  | `1020bbc0` |
| `lower_body_hulk_layers` | 0 |  | `1020bbc0` |
| `m_customaccel` | 0 |  | `101141e0` |
| `m_customaccel_exponent` | 1 | Mouse move is raised to this power before being scaled by scale factor. | `10113fa0` |
| `m_customaccel_max` | 0 | Max mouse move scale factor, 0 for no limit | `101141e0` |
| `m_customaccel_scale` | 0.04 | Custom mouse acceleration value. | `101141e0` |
| `m_forward` | 1 | Mouse forward factor. | `10114350` |
| `m_heat` | DT_PropMinigun |  | `102671d0` `10267710` |
| `m_mass` | DT_PhysBox | baseclass | `10077050` `100838f0` `10083990` +2 |
| `m_mouseaccel1` | 0 | Windows mouse acceleration initial threshold (2x movement). | `100bd9f0` `100cdc50` `100f2490` +2 |
| `m_mouseaccel2` | 0 | Windows mouse acceleration secondary threshold (4x movement). | `10113fa0` |
| `m_order` | DT_SurvivorPosition | baseclass | `10212190` |
| `m_pitch` | unknown |  | `101f0890` `10233e60` |
| `m_rage` | DT_Witch | baseclass | `10275db0` `10275f90` |
| `m_rawinput` | 0 | Use Raw Input for mouse input. | `101147a0` |
| `m_spawnflags` | DT_BaseTrigger | m_bClientSidePredicted | `10068bc0` `1009a200` `1009a2b0` +1 |
| `m_subject` | DT_TETutorLesson |  | `10222980` `10222a70` |
| `m_survivor` | DT_SurvivorRescue | baseclass | `101face0` `101fb910` `101fb970` +2 |
| `m_timestamp` | DT_IntervalTimer |  | `1016cca0` `1016d320` `1016d400` +3 |
| `m_usable` | DT_BaseButton | m_glowEntity | `100642b0` `10064f50` |
| `m_viewtarget` | DT_BaseFlex |  | `10044ce0` `100509f0` `100537c0` +3 |
| `m_yaw` | 0.022 | Mouse yaw factor. | `101141e0` `10114350` |
| `mat_aaquality` | 0 |  |  |
| `mat_accelerate_adjust_exposure_down` | 3.0 |  | `101813d0` `10182720` |
| `mat_antialias` | 0 |  |  |
| `mat_autoexposure_max` | 8 |  | `10180f80` |
| `mat_autoexposure_min` | 0.1 |  | `10180f80` |
| `mat_bloom_scalefactor_scalar` | 1.0 |  | `10181c20` |
| `mat_bloomamount_rate` | 0.05f |  | `10181c20` |
| `mat_bloomscale` | 1 |  | `10181c20` `10182720` |
| `mat_camerarendertargetoverlaysize` | 128 |  |  |
| `mat_clipz` | 1 |  |  |
| `mat_colcorrection_disableentities` | 0 | Disable map color-correction entities | `10068860` |
| `mat_colcorrection_editor` | 0 |  | `100c8620` |
| `mat_colcorrection_forceentitiesclientside` | 0 | Forces color correction entities to be updated on the client | `10068860` `10068a50` |
| `mat_colorcorrection` | 1 |  | `10184bf0` |
| `mat_debug_bloom` | 0 |  | `10184bf0` |
| `mat_debug_postprocessing_effects` | 0 | 0 = off, 1 = show post-processing passes in quadrants of the screen, 2 = only apply post-processing to the centre of the | `10184bf0` |
| `mat_disable_bloom` | 0 |  | `10181c20` |
| `mat_dof_enabled` | 1 |  | `10181cf0` |
| `mat_dof_far_blur_depth` | 1000.0 |  | `10184620` |
| `mat_dof_far_blur_radius` | 5.0 |  | `10184620` |
| `mat_dof_far_focus_depth` | 250.0 |  | `10184620` |
| `mat_dof_near_blur_depth` | 20.0 |  | `10184620` |
| `mat_dof_near_blur_radius` | 10.0 |  | `10184620` |
| `mat_dof_near_focus_depth` | 100.0 |  | `10184620` |
| `mat_dof_override` | 0 |  | `10181cf0` `10184620` |
| `mat_dof_quality` | 0 |  | `10184620` |
| `mat_drawwater` | 1 |  | `10186950` |
| `mat_dump_rts` | 0 |  | `10184bf0` |
| `mat_dynamic_tonemapping` | 1 |  | `10182720` `10183250` |
| `mat_exposure_center_region_x` | 0.9 |  | `10181dd0` |
| `mat_exposure_center_region_y` | 0.85 |  | `10181dd0` |
| `mat_force_bloom` | 0 |  | `10181c20` |
| `mat_force_tonemap_scale` | 0.0 |  | `101813d0` `10182f50` |
| `mat_framebuffercopyoverlaysize` | 128 |  | `1017fdc0` |
| `mat_fullbright` | 0 |  |  |
| `mat_grain_enable` | 1 |  |  |
| `mat_hdr_level` | 2 |  | `10181c20` |
| `mat_hdr_manual_tonemap_rate` | 1.0 |  | `101813d0` `10182720` |
| `mat_hdr_uncapexposure` | 0 |  | `10180f80` |
| `mat_hsv` | 0 |  | `101804b0` |
| `mat_local_contrast_enable` | 1 |  | `10184bf0` |
| `mat_lpreview_mode` | -1 |  | `10187590` |
| `mat_motion_blur_enabled` | 1 | MotionBlur | `10183ba0` `1018f9f0` |
| `mat_motion_blur_falling_intensity` | 1.0 |  | `10183ba0` |
| `mat_motion_blur_falling_max` | 20.0 |  | `10183ba0` |
| `mat_motion_blur_falling_min` | 10.0 |  | `10183ba0` |
| `mat_motion_blur_forward_enabled` | 0 |  | `10183ba0` |
| `mat_motion_blur_rotation_intensity` | 1.0 |  | `10183ba0` |
| `mat_motion_blur_strength` | 1.0 |  | `10183ba0` |
| `mat_non_hdr_bloom_scalefactor` | .3 |  | `10181c20` |
| `mat_postprocess_enable` | 1 |  | `10184bf0` |
| `mat_postprocess_x` | 4 |  | `10184bf0` |
| `mat_postprocess_y` | 1 |  | `10184bf0` |
| `mat_show_ab_hdr_hudelement` | 0 | HDR Demo HUD Element toggle. |  |
| `mat_show_histogram` | 0 |  | `10182720` `10183250` |
| `mat_showcamerarendertarget` | 0 |  | `101804b0` |
| `mat_showframebuffertexture` | 0 |  | `101804b0` |
| `mat_showlightmappage` | -1 |  | `1017f100` `101804b0` |
| `mat_showwatertextures` | 0 |  | `101804b0` |
| `mat_software_aa_blur_one_pixel_lines` | 0.5 | How much software AA should blur one-pixel thick lines: (0.0 - none), (1.0 - lots) | `101819d0` |
| `mat_software_aa_debug` | 0 | Software AA debug mode: (0 - off), (1 - show number of 'unlike' samples: 0->black, 1->red, 2->green, 3->blue), (2 - show | `101819d0` |
| `mat_software_aa_edge_threshold` | 1.0 | Software AA - adjusts the sensitivity of the software AA shader's edge detection (default 1.0 - a lower value will softe | `101819d0` |
| `mat_software_aa_quality` | 0 | Software AA quality mode: (0 - 5-tap filter), (1 - 9-tap filter) | `101819d0` |
| `mat_software_aa_strength` | -1.0 | BloomAdd | `10184bf0` |
| `mat_software_aa_strength_vgui` | -1.0 | Same as mat_software_aa_strength, but forced to this value when called by the post vgui AA pass. | `10184bf0` |
| `mat_software_aa_tap_offset` | 1.0 | Software AA - adjusts the displacement of the taps used by the software AA shader (default 1.0 - a lower value will make | `101819d0` |
| `mat_stub` | 0 |  | `100a93e0` |
| `mat_tonemap_algorithm` | 1 | 0 = Original Algorithm 1 = New Algorithm | `10180e00` `10181000` `101813d0` +4 |
| `mat_tonemap_min_avglum` | 3.0 |  | `101823a0` `10182720` |
| `mat_tonemap_percent_bright_pixels` | 1.0 |  | `101823a0` `10182720` `10183250` |
| `mat_tonemap_percent_target` | 45 |  | `101823a0` `10182720` `10183250` |
| `mat_tonemapping_occlusion_use_stencil` | 0 |  | `10181dd0` |
| `mat_viewportscale` | 1.0 | Scale down the main viewport (to reduce GPU impact on CPU profiling) |  |
| `mat_vignette_enable` | 1 |  |  |
| `mat_wateroverlaysize` | 128 |  | `1017f6f0` `101804b0` |
| `mat_wireframe` | 0 |  | `10022910` `10038ac0` `10096e10` +1 |
| `mat_yuv` | 0 |  | `101804b0` |
| `mem_level` | 2 | Memory Level - Default: High | `100a0d80` |
| `mod_load_showstall` | 0 | 1 - show hitches , 2 - show stalls | `10164ab0` |
| `molotov_groundfire` | ? |  | `102274b0` |
| `motd_intros` | ? |  |  |
| `mounted_gun_cooldown_time` | 20 |  | `102683c0` |
| `mounted_gun_distance` | 36.0 |  | `10268890` |
| `mounted_gun_overheat_time` | 20 |  | `102683c0` |
| `mp_allowspectators` | 1.0 | toggles whether the server allows spectator mode or not |  |
| `mp_blockstyle` | 1 | Sets the style of capture point blocking used. 0 = Blocks break captures completely. 1 = Blocks only pause captures. |  |
| `mp_buytime` | 1.5 | How many minutes after round start players can buy items for. |  |
| `mp_capdeteriorate_time` | 90.0 | Time it takes for a full capture point to deteriorate. | `10099c00` |
| `mp_capstyle` | 1 | Sets the style of capture points used. 0 = Fixed players required to cap. 1 = More players cap faster, but longer cap ti | `10099c00` |
| `mp_facefronttime` | 9999 | After this amount of time of standing in place but aiming to one side, go ahead and move feet to face upper body. | `1000ec60` `101dc550` |
| `mp_fadetoblack` | 0 | fade a player's screen to black when he dies | `101d3090` |
| `mp_feetyawrate` | 180 | How many degrees per second that we can turn our feet or upper body. | `1000edd0` |
| `mp_feetyawrate_max` | 360 | How many degrees per second that we can turn our feet or upper body when we're outside the max body yaw. | `1000edd0` |
| `mp_forcecamera` | 0 | Restricts spectator modes for dead players | `10061220` |
| `mp_friendlyfire` | 1 |  | `102601b0` |
| `mp_gamemode` | coop | Current game mode, acceptable values are coop, versus, survival; changed using map command, eg: map mapname versus | `1021c760` `1021c7e0` `1021ce00` |
| `mp_ik` | 1 | Use IK on in-place turns. |  |
| `mp_playerid` | 1 | Controls what information player see in the status bar: 0 all names; 1 team names; 2 no names | `101d3090` |
| `mp_playerid_delay` | 0 | Number of seconds to delay showing information in the status bar | `101d3090` |
| `mp_respawnwavetime` | 10.0 | Time between respawn waves. |  |
| `mp_timelimit` | 0 | game time per map in minutes |  |
| `mp_usehwmmodels` | 0 | Enable the use of the hw morph models. (-1 = never, 1 = always, 0 = based upon GPU) | `1001d390` |
| `mp_usehwmvcds` | 0 | Enable the use of the hw morph vcd(s). (-1 = never, 1 = always, 0 = based upon GPU) | `100901f0` |
| `mp_weapon_ak47_price` | 2500 |  |  |
| `mp_weapon_aug_price` | 3500 |  |  |
| `mp_weapon_awp_price` | 4750 |  |  |
| `mp_weapon_deagle_price` | 650 |  |  |
| `mp_weapon_elite_price` | 800 |  |  |
| `mp_weapon_famas_price` | 2250 |  |  |
| `mp_weapon_fiveseven_price` | 750 |  |  |
| `mp_weapon_g3sg1_price` | 5000 |  |  |
| `mp_weapon_galil_price` | 2000 |  |  |
| `mp_weapon_glock_price` | 400 |  |  |
| `mp_weapon_m249_price` | 5750 |  |  |
| `mp_weapon_m3_price` | 1700 |  |  |
| `mp_weapon_m4a1_price` | 3100 |  |  |
| `mp_weapon_mac10_price` | 1400 |  |  |
| `mp_weapon_mp5navy_price` | 1500 |  |  |
| `mp_weapon_p228_price` | 600 |  |  |
| `mp_weapon_p90_price` | 2350 |  |  |
| `mp_weapon_scout_price` | 2750 |  |  |
| `mp_weapon_sg550_price` | 4200 |  |  |
| `mp_weapon_sg552_price` | 3500 |  |  |
| `mp_weapon_tmp_price` | 1250 |  |  |
| `mp_weapon_ump45_price` | 1700 |  |  |
| `mp_weapon_usp_price` | 500 |  |  |
| `mp_weapon_xm1014_price` | 3000 |  |  |
| `music_dynamic_play` | ? |  | `101f7a40` `101f7c90` `101f7ce0` +10 |
| `music_dynamic_stop_playing` | ? |  |  |
| `music_intensity_override` | -1 | Overrides the player's music intensity track for testing |  |
| `music_intensity_threshold` | 0.3 |  |  |
| `music_master_debug` | 0 |  | `101f6010` |
| `music_master_latency_report` | 0 |  | `101f7b80` |
| `music_master_spew` | 0 |  | `101fa5e0` |
| `music_master_tag_threshold` | .01 |  | `101f9e20` `101fa410` |
| `music_print` | client |  |  |
| `muzzleflash_light` | 1 |  |  |
| `nb_shadow_dist` | 400 |  | `10248fa0` |
| `nb_stop` | 0 | Stop all NextBots | `102749b0` `10274a60` |
| `net_graph` | 0 | Draw the network usage graph, = 2 draws data on payload, = 3 draws payload legend. | `101740b0` `10174280` |
| `net_graphheight` | 64 | Height of netgraph panel | `10174280` |
| `net_graphmsecs` | 400 | The latency graph represents this many milliseconds. | `10175ae0` |
| `net_graphpos` | 1 |  | `10174120` |
| `net_graphproportionalfont` | 1 | Determines whether netgraph font is proportional or not | `10174280` `10174300` `101743f0` +1 |
| `net_graphshowinterp` | 1 | Draw the interpolation graph. | `10175660` |
| `net_graphshowlatency` | 1 | Draw the ping/packet loss graph. | `10175ae0` |
| `net_graphsolid` | 1 |  | `10175660` |
| `net_graphtext` | 1 | Draw text fields | `10174640` |
| `net_showusercmd` | 0 | Show user command encoding | `1016b320` `1016b3b0` `1016b440` +3 |
| `net_showusermessages` | 0 |  |  |
| `next_weapon_timer` | 1 | 1.25 | `101925a0` `101940b0` `101941e0` |
| `old_radiusdamage` | 0.0 |  |  |
| `overlay_vars` | DT_BaseAnimatingOverlay | baseclass | `10042170` `10044c40` `100509f0` |
| `overview_alpha` | 1.0 | Overview map translucency.  |  |
| `overview_health` | 1 | Show player's health in map overview.  |  |
| `overview_locked` | 1 | Locks map angle, doesn't follow view angle.  |  |
| `overview_mode` | Sets overview map mode off,small,large: <0/1/2> |  | `101cb2c0` `101cc3b0` `101cc930` |
| `overview_names` | 1 | Show player's names in map overview.  |  |
| `overview_tracks` | 1 | Show player's tracks in map overview.  |  |
| `overview_zoom` | Sets overview map zoom: <zoom> [<time>] [rel] |  |  |
| `pain_pills_decay_rate` | 0.27 |  | `101fbbc0` `101fbc00` `1022a200` |
| `pain_pills_health_threshold` | 99 | Pills can't be used unless total health is less than this amount | `10269260` |
| `pain_pills_health_value` | 50 |  |  |
| `panel_test_title_safe` | 0 | Test vgui panel positioning with title safe indentation | `10287d00` |
| `particle_sim_alt_cores` | 2 |  |  |
| `particle_simulateoverflow` | 0 | Used for stress-testing particle systems. Randomly denies creation of particles. | `1012e620` |
| `perf_fire_bullet_firstpredictedonly` | 1 | Only do effects tracing, etc., first time a CUserCmd is predicted. | `10262500` |
| `perf_fire_bullet_single` | 0 |  | `10262500` |
| `perfvisualbenchmark_abort` | ? |  | `10133ac0` |
| `peril_notice_timer` | 5.0 |  |  |
| `phonemes_strong` | phonemes |  | `10053fa0` `100543f0` `1007abb0` +3 |
| `phonemes_weak` | phonemes |  | `10053fa0` `100543f0` `1007abb0` +3 |
| `phys_pushscale` | 1 |  | `10168a60` |
| `pixelvis_debug` | Dump debug info |  | `10086b80` |
| `playvideo_exitcommand` | Plays a video and fires and exit command when it is stopped  |  | `1001e590` `1005ed90` `10060c10` +38 |
| `polish_demo` | Polish the given demo.  Pass demo name as only parameter. |  | `1006ec90` |
| `predicted_viewmodel` | CTerrorViewModel |  | `10220810` `10221d80` |
| `prop_data` | ? |  | `10141fb0` `1014cc20` `1014e070` +3 |
| `proportional_int` | ? |  | `100ff120` `10239970` `1023a6e0` +13 |
| `props_break_max_pieces` | -1 | Maximum prop breakable piece count (-1 = model default) | `1014cc20` |
| `props_break_max_pieces_perframe` | -1 | Maximum prop breakable piece count per frame (-1 = model default) | `1014cc20` |
| `punch_angle_decay_rate` | 20 |  | `101d7e90` `102199c0` |
| `r_3dsky` | 1 | Enable the rendering of 3d sky boxes | `10189bb0` |
| `r_bloomtintexponent` | 2.2 | 0 | `10213b90` |
| `r_bloomtintexponent_ghost` | 0.1 |  | `10213b90` |
| `r_cheapwaterend` | ? |  | `10185240` |
| `r_cheapwaterstart` | ? |  |  |
| `r_debugcheapwater` | 0 |  | `10186950` |
| `r_decals` | 2048 | IEffects001 | `101ae6a0` `101ae730` `101b0fb0` +1 |
| `r_depthoverlay` | 0 | Replaces opaque objects with their grayscaled depth values. r_showz_power scales the output. | `1017e3b0` |
| `r_dopixelvisibility` | 1 |  | `10084e90` `10085b20` `10086630` |
| `r_draw_flashlight_3rd_person` | 1 | Draw flashlight beams for other players | `10206ee0` |
| `r_draw_lasersight_1st_person` | 1 | Draw laser sight for the local player | `10200810` |
| `r_draw_lasersight_3rd_person` | 1 | Draw laser sights for other players | `10200810` |
| `r_drawflecks` | 1 |  | `100e05b0` `100e07a0` |
| `r_drawmodeldecals` | 1 |  | `10049ed0` |
| `r_drawmodelstatsoverlay` | 0 |  | `10038ac0` |
| `r_drawopaquerenderables` | 1 |  | `1018d410` |
| `r_drawopaqueworld` | 1 |  | `10186e60` |
| `r_drawothermodels` | 1 | 0=Off, 1=Normal, 2=Wireframe | `10036460` `10038ac0` `1018d410` |
| `r_drawparticles` | 1 | Enable/disable particle rendering | `101306f0` |
| `r_drawpixelvisibility` | 0 | Show the occlusion proxies |  |
| `r_drawrenderboxes` | 0 |  | `10048b10` |
| `r_drawropes` | 1 |  | `1008db30` `1008f880` |
| `r_drawsprites` | 1 |  | `10096e10` `10097330` `100ec900` |
| `r_drawtracers_firstperson` | 1 |  | `100e2160` |
| `r_drawtranslucentrenderables` | 1 |  | `10189730` `10189820` `1018db80` |
| `r_drawtranslucentworld` | 1 |  | `1018db80` |
| `r_drawviewmodel` | 1 |  | `101619e0` `101867c0` `10220a30` +1 |
| `r_entityclips` | 1 |  | `10189390` `10189540` |
| `r_eyewaterepsilon` | 7.0f |  | `101861e0` |
| `r_fade360style` | 1 |  | `10185c90` |
| `r_farz` | -1 | Override the far clipping plane. -1 means to use the value in env_fog_controller. |  |
| `r_fastzreject` | 0 | Activate/deactivates a fast z-setting algorithm to take advantage of hardware with fast z reject. Use -1 to default to h |  |
| `r_flashlight_3rd_person_range` | 60 | Distance to draw flashlight beams for other players | `10206ee0` |
| `r_flashlight_attach_to_viewmodel` | 1 | Attach the flashlight effect to the viewmodel | `10207700` |
| `r_flashlightambient` | 0.0 |  | `100d82d0` |
| `r_flashlightbacktraceoffset` | 0.4 |  | `100d77e0` |
| `r_flashlightconstant` | 0.0 |  | `100d82d0` |
| `r_flashlightdepthres` | 1024 |  | `100bfc50` `100c13e0` `100c33e0` +1 |
| `r_flashlightdepthtexture` | 1 |  | `100bc880` `100c33e0` `100d82d0` |
| `r_flashlightdrawdepth` | 0 |  | `101804b0` |
| `r_flashlightdrawfrustum` | 0 |  |  |
| `r_flashlightfar` | 750.0 |  | `100d77e0` `100d82d0` |
| `r_flashlightfov` | 53.0 |  | `100d82d0` |
| `r_flashlightinfectedfar` | 750 |  |  |
| `r_flashlightinfectedfov` | 90 |  |  |
| `r_flashlightinfectedlinear` | 100 |  |  |
| `r_flashlightinfectedshadows` | 0 |  |  |
| `r_flashlightinfectedtexture` | effects/flashlight001_infected |  |  |
| `r_flashlightladderdist` | 40.0 |  | `100d77e0` |
| `r_flashlightlinear` | 100.0 |  | `100d82d0` |
| `r_flashlightlockposition` | 0 |  | `100d8490` `100d8660` |
| `r_flashlightmodels` | 1 |  | `100c06c0` |
| `r_flashlightmuzzleflash` | 1 |  |  |
| `r_flashlightmuzzleflashfov` | 120 |  | `100d82d0` |
| `r_flashlightnear` | 4.0 |  | `100d82d0` |
| `r_flashlightnearoffsetscale` | 0.3 |  | `100d82d0` |
| `r_flashlightoffsetforward` | 0.0 |  | `100d7410` `10207700` |
| `r_flashlightoffsetforward_low` | 0.0 |  | `10207700` |
| `r_flashlightoffsetright` | 5.0 |  | `100d7410` `10207700` |
| `r_flashlightoffsetright_low` | 10.0 |  | `10207700` |
| `r_flashlightoffsetup` | -5.0 |  | `100d7410` `10207700` |
| `r_flashlightoffsetup_low` | -16.0 |  | `10207700` |
| `r_flashlightquadratic` | 0.0 |  | `100d82d0` |
| `r_flashlightshadowatten` | 0.35 |  | `100d82d0` |
| `r_flashlighttracedistcutoff` | 32 |  | `100d77e0` |
| `r_flashlightvisualizetrace` | 0 |  | `100d77e0` |
| `r_flashlightvolumetrics` | 1 |  | `1018ce50` |
| `r_impactparticles` | 1 | HelicopterMegaBomb | `100e07a0` |
| `r_jiggle_bones` | 1 |  | `100401a0` |
| `r_lod` | -1 |  | `1011d930` |
| `r_mapextents` | 16384 | Set the max dimension for the map.  This determines the far clipping plane | `10062f10` |
| `r_particle_sim_spike_threshold_ms` | 5 |  | `1012e740` |
| `r_particle_timescale` | 1.0 |  | `1012e740` |
| `r_pixelvisibility_partial` | 1 |  | `100854e0` `10085b20` |
| `r_pixelvisibility_spew` | 0 |  | `10084c10` `100854e0` |
| `r_portalsopenall` | 0 | Open all portals | `100affa0` |
| `r_propsmaxdist` | 1200 | Maximum visible distance | `10142680` |
| `r_queued_post_processing` | 0 |  | `10184bf0` |
| `r_queued_ropes` | 1 |  | `1008f880` |
| `r_rainalpha` | 0.4 |  | `10197000` |
| `r_rainalphapow` | 0.8 |  | `10197000` |
| `r_raindensity` | 0.001 |  |  |
| `r_rainlength` | 0.1f |  | `10197000` |
| `r_rainspeed` | 600.0f |  | `10195c70` `1019a3e0` |
| `r_rainwidth` | 0.5 |  | `10197000` |
| `r_ropetranslucent` | 1 |  |  |
| `r_screenoverlay` | Draw specified material as an overlay |  | `101804b0` |
| `r_sequence_debug` | ? |  | `10038140` `1003f790` `100411d0` +2 |
| `r_shadow_debug_spew` | 0 |  | `100c2eb0` `100c3040` |
| `r_shadow_deferred` | 0 | Toggle deferred shadow rendering | `100b8030` `100b8070` `100bac70` +7 |
| `r_shadow_deferred_downsample` | 0 | Toggle low-res deferred shadow rendering |  |
| `r_shadow_deferred_simd` | 0 |  |  |
| `r_shadow_half_update_rate` | 0 | Updates shadows at half the framerate |  |
| `r_shadow_lightpos_lerptime` | 0.5 |  | `100b7880` |
| `r_shadow_shortenfactor` | 2 | Makes shadows cast from local lights shorter | `100b7880` |
| `r_shadowangles` | Set shadow angles |  |  |
| `r_shadowblobbycutoff` | some shadow stuff |  |  |
| `r_shadowcolor` | Set shadow color |  |  |
| `r_shadowdir` | Set shadow direction |  |  |
| `r_shadowdist` | Set shadow distance |  |  |
| `r_shadowfromanyworldlight` | 0 |  | `100b7880` |
| `r_shadowfromworldlights` | 1 | Enable shadowing from world lights |  |
| `r_shadowfromworldlights_debug` | 0 |  | `100b7880` |
| `r_shadowmaxrendered` | 32 |  | `100c0c80` |
| `r_shadowrendertotexture` | 0 |  | `100c33e0` |
| `r_shadows` | 1 |  | `100c0c80` |
| `r_shadows_gamecontrol` | -1 |  | `100bfea0` `100c0c80` |
| `r_shadows_on_renderables_enable` | 0 | Support casting RTT shadows onto other renderables | `100b3090` `100b3200` |
| `r_skybox` | 1 | Enable the rendering of sky boxes | `10189bb0` `1018ce50` |
| `r_swingflashlight` | 1 |  |  |
| `r_threaded_particles` | 1 |  | `1012e740` |
| `r_threadeddetailprops` | 1 | enable threading of detail prop drawing |  |
| `r_unlimitedrefract` | 0 |  | `10189910` |
| `r_updaterefracttexture` | 1 |  | `1012ae20` |
| `r_visocclusion` | 0 |  | `1018f5c0` |
| `r_visualizeproplightcaching` | 0 |  | `10083dc0` `10083f00` |
| `r_visualizetraces` | 0 |  | `10012f00` `10012f70` `10019f10` +16 |
| `r_waterforceexpensive` | 0 |  | `10186950` |
| `r_waterforcereflectentities` | 0 |  | `10186950` |
| `r_worldlistcache` | 1 |  | `1018e300` |
| `ragdoll_sleepaftertime` | 5 | After this many seconds of being basically stationary, the ragdoll will go to sleep. | `1014f760` |
| `ragdoll_velocity_multiplier` | 0 | Acceleration to last living velocity given to new ragdolls |  |
| `rain_outer` | ? |  | `10132fe0` `101952d0` `10195350` +4 |
| `report_cliententitysim` | 0 | List all clientside simulations and time - will report and turn itself off. | `1004f550` |
| `report_clientthinklist` | 0 | List all clientside entities thinking and time - will report and turn itself off. | `100ac000` |
| `rope_averagelight` | 1 | Makes ropes use average of cubemap lighting instead of max intensity. | `1008c4e0` |
| `rope_collide` | 1 | Collide rope with the world | `1008ccc0` |
| `rope_rendersolid` | 1 |  |  |
| `rope_shake` | 0 |  | `1008d310` |
| `rope_smooth` | 1 | Do an antialiasing effect on ropes |  |
| `rope_smooth_enlarge` | 1.4 | How much to enlarge ropes in screen space for antialiasing effect |  |
| `rope_smooth_maxalpha` | 0.5 | Alpha for rope antialiasing effect |  |
| `rope_smooth_maxalphawidth` | 1.75 |  |  |
| `rope_smooth_minalpha` | 0.2 | Alpha for rope antialiasing effect |  |
| `rope_smooth_minwidth` | 0.3 | When using smoothing, this is the min screenspace width it lets a rope shrink to |  |
| `rope_solid_maxalpha` | 1 |  |  |
| `rope_solid_maxwidth` | 1 |  |  |
| `rope_solid_minalpha` | 0.0 |  |  |
| `rope_solid_minwidth` | 0.3 |  |  |
| `rope_subdiv` | 1 | Rope subdivision amount | `1008ccc0` |
| `rope_wind_dist` | 1000 | Don't use CPU applying small wind gusts to ropes when they're past this distance. | `1008d310` |
| `sb_perf_collect` | 1 | Collect perf for Steam stats | `10207700` |
| `sb_perf_crawl` | 0 | If true, survivor bot will be doing a perf-crawl walk through the map |  |
| `sb_perf_dump` | 0 | If true, low perf places will be dumped | `10207700` |
| `sb_perf_dump_bots` | Which bots to use for dumping: namvet, girl, biker, manager |  | `10207700` |
| `sb_perf_dump_low` | 40 | Low perf threshold | `10207700` |
| `sb_perf_dump_low_dist` | 64 | Dump low perf points at least this distance apart | `10203070` `10207700` |
| `sb_pushscale` | 1.0 |  | `10210440` |
| `scene_clientflex` | 1 | Do client side flex animation. | `100517f0` |
| `scene_print` | 0 | When playing back a scene, print timing and event info to console. | `1015c4c0` |
| `scene_vcdautosave` | 0 | Create a savegame before VCD playback | `10091da0` |
| `scr_centertime` | 2 | CAvatarImagePanel | `1016fc00` `1016fc30` |
| `shake_stop` | Stops all active screen shakes.  |  | `1017dec0` `1017df40` |
| `shotgun_reload_cancel_delay` | 0.5 | On reload from empty, time before a fire command is recognized to cancel the reload. | `1025eef0` |
| `should_never_see_this` | DT_GameRulesProxy |  | `10169220` `1016dbf0` `101d8860` |
| `show_menu` | Opens a menu |  |  |
| `sk_autoaim_mode` | 1 |  |  |
| `sky_urb01` | Current name of the skybox texture |  |  |
| `smoker_pz_claw_dmg` | 4 | Amount of damage done by a PZ smoker's regular melee attack |  |
| `snd_musicvolume` | ? |  | `101f7a40` `101f7c90` `101f7ce0` +12 |
| `snd_playsounds` | Play sounds from the game sounds txt file at a given locatio |  |  |
| `snd_setsoundparam` | Set a sound paramater |  | `1015dce0` |
| `soundpatch_captionlength` | 2.0 | How long looping soundpatch captions should display for. | `1015f840` |
| `soundscape_dumpclient` | Dumps the client's soundscape data.  | player | `1005de50` |
| `soundscape_message` | 0 |  | `10093f00` `100962c0` |
| `spec_allowroaming` | 0 |  |  |
| `spec_autodirector` | 1 | Auto-director chooses best view modes while spectating | `100ef930` `100efeb0` `100f0710` |
| `spec_centerchasecam` | 0 | Looks at the target player's center, instead of his eye position, in chase came mode | `10060700` |
| `spec_chasedistance` | 96 | Chase cam's ideal distance from target | `100607b0` `101941e0` `101fcb00` |
| `spec_cinematic` | 0 |  | `101fcb00` |
| `spec_closeup_range` | 10 | Range at which we framing the head of our subject | `101fcb00` |
| `spec_freeze_distance_max` | 200 | Maximum random distance from the target to stop when framing them in observer freeze cam. | `10062920` |
| `spec_freeze_distance_min` | 96 | Minimum random distance from the target to stop when framing them in observer freeze cam. | `10062920` |
| `spec_freeze_time` | 4.0 | Time spend frozen in observer freeze cam. | `10060c10` |
| `spec_freeze_traveltime` | 0.4 | Time taken to zoom in to frame a target in observer freeze cam. | `10060c10` |
| `spec_help` | Show spectator help screen |  |  |
| `spec_keep_team_cc` | 1 | Use team color correction even when spectating in vs mode | `10213b90` `10213e80` |
| `spec_keep_team_glows` | 1 | Use team glows even when spectating in vs mode | `10038d10` `101ffca0` `10201660` +1 |
| `spec_lockedchasecam` | 1 | Spectator chase camera centers behind the spectated player |  |
| `spec_lockedchasecam_center_speed` | 1.5 | Controls rate that camera is centered on the target |  |
| `spec_lockedchasecam_delay` | 3 | Locked chasecam doesn't auto-center this many seconds after mouse movement |  |
| `spec_lockedchasecam_pitch` | 15 |  |  |
| `spec_lockedchasecam_yaw` | 15 |  |  |
| `spec_longshot_range` | 75 | Range at which we framing the whole body of our subject | `101fcb00` |
| `spec_max_tracking_error` | 1000 |  |  |
| `spec_menu` | Activates spectator menu |  |  |
| `spec_mode` | Set spectator mode |  |  |
| `spec_next` | Spectate next player |  | `101cca60` |
| `spec_player` | Spectate player by name |  | `101cc3b0` `101ccb60` |
| `spec_portraits` | 0 |  | `1023f3d0` |
| `spec_pos` | dump position and angles to the console |  |  |
| `spec_prev` | Spectate previous player |  |  |
| `spec_scoreboard` | 0 |  | `101cca60` |
| `spec_track` | 0 | Tracks an entity in spec mode | `10060ab0` |
| `spec_tracking_power` | 5 |  |  |
| `ss_debug_draw_player` | -1 |  |  |
| `ss_enable` | 0 | Enables Split Screen support. Play Single Player now launches into split screen mode. NO ONLINE SUPPORT |  |
| `ss_mimic` | 0 | Split screen users mimic base player's CUserCmds |  |
| `ss_splitmode` | 0 | Two player split screen mode (0 - recommended settings base on the width, 1 - horizontal, 2 - vertical (only allowed in  |  |
| `ss_teleport` | Teleport other splitscreen player to my location. |  |  |
| `ss_verticalsplit` | ? |  | `1022f5e0` `10230640` `102306a0` +6 |
| `stats_highlight_interval` | 10 | Interval between hightlight screens in the transition stats panel |  |
| `stats_nexthighlight` | ? |  | `1025b420` `1025c5b0` `1025cc30` |
| `steam_stats_write_to_keyvalue` | 0 | If enabled, steam stats will be written to a keyvalue. | `101f3170` `101f3230` `101f32c0` +10 |
| `still_vel_limit` | 5 | if player velocity is lower than this, he is 'still' | `10210cf0` |
| `still_view_accel_limit` | 1500 | if player's view is accelerating less than this, his view is 'still' | `10210cf0` |
| `survivor_accuracy_upgrade_factor` | 0.5 |  | `1026e9d0` |
| `survivor_allow_crawling` | 0 |  | `1020faa0` `1020fab0` |
| `survivor_autolook_voc_delay` | 1 | After staring an object for this many seconds, a survivor will say a smartlook concept for it if available. | `10203230` |
| `survivor_crawl_speed` | 15 |  |  |
| `survivor_crouch_speed` | 75 |  |  |
| `survivor_drag_continuous_fire` | 1 | Survivors need to hold +attack2 to drag incapacitated survivors |  |
| `survivor_drag_speed_multiplier` | .5 | Multiplier to speed when we are dragging somebody. |  |
| `survivor_draw_addons` | 2 | 0=none, 1=guns, 2=all | `101d4160` |
| `survivor_expressions` | 1 | 0.5 | `101fd650` `101ff730` `10201000` |
| `survivor_expressions_anger` | -1 |  | `101ff830` |
| `survivor_expressions_fear` | -1 |  | `101ff830` |
| `survivor_fumes_run_speed` | 85 |  | `1020f7f0` |
| `survivor_fumes_walk_speed` | 85 |  | `1020eb10` |
| `survivor_hanging_eye_height` | 85 |  | `1020ffa0` |
| `survivor_hanging_from_tongue_eye_height` | 40 |  | `1020ffa0` |
| `survivor_incap_health` | ? |  | `1020f5c0` `10215d30` `10216560` +1 |
| `survivor_incapacitated_accuracy_penalty` | 0.4 | Penalty given for shooting while incapacitated. | `1026e9d0` |
| `survivor_incapacitated_cycle_time` | 0.3 | New cycle time (RoF) used when incapacitated, overriding weapon. | `1026e4f0` |
| `survivor_incapacitated_dizzy_severity` | 2.0 | From negative to positive this, how much bump we add to a incap'd player's mouse. | `10270bc0` |
| `survivor_incapacitated_dizzy_timer` | 2.5 | How often our incap'd dizzy-drift changes directions. | `10270bc0` |
| `survivor_incapacitated_eye_height` | 24 |  | `1020f0c0` |
| `survivor_incapacitated_max_yaw` | 180 |  | `101dd9e0` |
| `survivor_incapacitated_reload_multiplier` | 1.25 | Multiplier to reload time when incapacitated. | `10270450` |
| `survivor_incapacitated_roll` | 20 |  | `102199c0` |
| `survivor_it_duration` | 20 |  |  |
| `survivor_it_fade_in_interval` | 0.2 |  | `10200380` |
| `survivor_it_fade_out_interval` | 2 |  | `10200380` |
| `survivor_ledge_grab_health` | 300 |  | `101ff830` |
| `survivor_limp_health` | 40 |  | `101db760` `101ff830` `1020f7f0` |
| `survivor_limp_run_speed` | 150 |  | `1020f7f0` |
| `survivor_limp_walk_speed` | 85 |  | `1020eb10` |
| `survivor_min_local_yaw` | -45 |  | `101dd9e0` |
| `survivor_pounce_victim_eye_height` | 12 |  | `1020ffa0` |
| `survivor_revive_after_drag` | 0 | Standing still after dragging someone will start reviving him |  |
| `survivor_revive_continuous_fire` | 1 | Players need to hold +attack to revive |  |
| `survivor_skills` | 0 | If nonzero, force Survivors to make interesting choices at checkpoints | `1020f7f0` `10210370` `1023bfe0` +3 |
| `survivor_slots` | 0 | If nonzero, force Survivors to make interesting choices at checkpoints |  |
| `survivor_speed_boost_factor` | 1.2 | Speed boost from upgrade | `1020f7f0` |
| `survivor_sprint_multiplier` | 1.5 |  |  |
| `survivor_still_penalty` | 3 | Amount gun accuracy is affected when the view is not still | `1026e9d0` |
| `survivor_teammatefocus_enabled` | 0 | Enable or disable focusing on teammates via the use key | `102005b0` |
| `survivor_teammatefocus_speed` | 5 | Scalar speed for locking on a teammate in focus | `101fe080` |
| `survivor_teammatefocus_stickyness` | 5 | Amount the look controller must move before unlocking a teammate in focus | `101fe080` |
| `survivor_unstoppable_speed` | 150 |  | `1020f7f0` |
| `survivor_upgrade_accuracy` | 0.7 |  |  |
| `survivor_upgrade_reload_duration` | 0.5 | Upgraded survivors reload in this fraction of the base time. | `10270450` |
| `survivor_upgrade_reload_shotgun_duration` | 0.5 | Upgraded survivors reload shotguns in this fraction of the base time. | `1025e990` |
| `survivor_upgrades` | 0 |  | `1024b9a0` |
| `survivor_use_glow` | 1 | Usable items glow when the survivor is close to them and looking near them. | `10203230` |
| `survivor_use_glow_range` | 500 |  | `101fdd70` `10203230` |
| `survivor_walk_speed` | 85 |  | `1020eb10` `1020f7f0` |
| `sv_accelerate` | 5 |  | `100e6350` `100e6920` `1021b910` |
| `sv_airaccelerate` | 10 |  | `100e44d0` `10218400` |
| `sv_alternateticks` | 0 | If set, server only simulates entities on even numbered ticks.  | `1001ad90` |
| `sv_backspeed` | 0.6 | How much to slow down backwards motion |  |
| `sv_bounce` | 0 | Bounce multiplier for when physically simulated objects collide with other objects. | `100e9410` |
| `sv_cheats` | [LEADERBOARDS] WriteLeaderboardStats has nothing to write.  |  | `1020f5c0` `10216560` `102195f0` |
| `sv_debug_player_use` | 0 | Visualizes +use logic. Green cross=trace success, Red cross=trace too far, Green box=radius success | `1001ed10` |
| `sv_footstepinterval` | 0.22 |  | `100d8850` |
| `sv_footsteps` | 1 | Play footstep sound for players | `1001f3a0` `100d8850` |
| `sv_friction` | 4 | World friction. | `100e4290` `100e6350` `100e6ed0` +1 |
| `sv_glowenable` | 1 | Turns on and off the terror glow highlight effects | `10217030` |
| `sv_gravity` | 800 | World gravity. | `100e4c30` `100e5660` `100e5f90` +8 |
| `sv_infinite_ammo` | 0 | Player's active weapon will never run out of ammo | `1026f170` |
| `sv_lan` | ? |  | `101f3170` `101f3230` `101f32c0` +10 |
| `sv_maxspeed` | 320 |  | `100e3da0` `100e46d0` `100f0870` |
| `sv_maxvelocity` | 3500 | Maximum speed any ballistically moving object is allowed to attain per axis. | `100e4a80` `100e6ed0` `101369e0` |
| `sv_noclipaccelerate` | 5 |  | `100e89c0` |
| `sv_noclipduringpause` | 0 | If cheats are enabled, then you can noclip with the game paused (for doing screenshots, etc.). | `101136f0` |
| `sv_noclipfriction` | 4 | Friction during noclip move. | `100e46d0` |
| `sv_noclipspeed` | 5 |  | `100e89c0` |
| `sv_nojumpingonelevators` | 1 | Disallow jumping when on lifts. | `10219c60` |
| `sv_optimizedmovement` | 1 |  | `100e89c0` |
| `sv_pushaway_clientside` | 0 | Clientside physics push away (0=off, 1=only localplayer, 1=all players) | `10121050` |
| `sv_pushaway_clientside_size` | 15 | Minimum size of pushback objects | `1014c140` |
| `sv_pushaway_max_force` | 2000 | Maximum amount of force applied to physics objects by players. | `10121050` |
| `sv_pushaway_max_player_force` | 10000 | Maximum of how hard the player is pushed away from physics objects. | `10120cc0` |
| `sv_pushaway_min_force` | 500 | Minimum amount of force applied to physics objects by players. |  |
| `sv_pushaway_min_player_speed` | 75 | If a player is moving slower than this, don't push away physics objects (enables ducking behind things). | `10121050` |
| `sv_pushaway_player_force` | 200000 | How hard the player is pushed away from physics objects (falls off with inverse square of distance). | `10120cc0` |
| `sv_rollangle` | 0 | Max view roll angle | `1001da40` `101d6c50` |
| `sv_rollspeed` | 200 |  | `1001da40` `101d6c50` |
| `sv_showdamage` | 0 | Shows base damage below what sv_showimpacts would show, and if a player was hit he'll show the damage he took above it ( |  |
| `sv_showdebugtracers` | 0 | 1: Shows a line following bullet path. Blue on client, red on server. (They should always match.) 2:Show autoaim candida | `102615b0` |
| `sv_showfootsteps` | 0 | Shows client (red) and server (green) footsteps (1=client-only, 2=server-only, 3=both) | `100d8850` |
| `sv_showhitboxes` | ? |  | `10274a60` `10274fa0` `10275b50` |
| `sv_showplayerhitboxes` | 0 | Show lag compensated hitboxes for the specified player index whenever a player fires. | `10262500` |
| `sv_skyname` | Current name of the skybox texture |  |  |
| `sv_soundemitter_trace` | -1 | Show all EmitSound calls including their symbolic name and the actual wave file they resolved to. (-1 = for nobody, 0 =  |  |
| `sv_specaccelerate` | 5 |  | `100e6ed0` `100f0870` |
| `sv_specnoclip` | 1 |  | `100e6ed0` |
| `sv_specspeed` | 3 |  | `100e6ed0` `100f0870` |
| `sv_stepsize` | 18 |  | `1001f590` |
| `sv_stickysprint_default` | 0 |  |  |
| `sv_stopspeed` | 75 | Minimum stopping speed when on ground. | `100e4290` |
| `sv_suppress_viewpunch` | 0 |  |  |
| `sv_tracereffects_limit_general` | 15 | Limits the number of visible tracers per frame |  |
| `sv_turbophysics` | 1 | Turns on turbo physics | `10120b70` |
| `sv_wateraccelerate` | 10 |  |  |
| `sv_waterdist` | 12 | Vertical view fixup when eyes are near water plane. |  |
| `sv_waterfriction` | 1 |  |  |
| `tank_attack_range` | 50 | Distance from Survivor that tank triggers own swing. |  |
| `tank_auto_swing` | 0 |  |  |
| `tank_fist_radius` | 15 |  | `1026dbb0` |
| `tank_ground_pound_duration` | 1.5 |  | `1026cb20` |
| `tank_pz_forward` | -0.5 |  | `1026cb20` |
| `tank_raffle_debug` | 0 | Fill the tank raffle with dummy values for debugging |  |
| `tank_swing_arc` | 180 |  |  |
| `tank_swing_duration` | 0.2 | Duration of the actual swing |  |
| `tank_swing_fast_interval` | 0.6 | Interval between tank swings when he is clearing zombies out of his path | `1026d0a0` |
| `tank_swing_interval` | 1.5 | Interval between tank swings | `1026d0a0` |
| `tank_swing_miss_interval` | 1.0 | Interval between tank swings after a miss | `1026d0a0` |
| `tank_swing_physics_prop_force` | 4.0 | Multiplier for tank hitting a phys prop. |  |
| `tank_swing_range` | 56 | Range of the actual swing | `1026cb20` `1026d740` |
| `tank_swing_yaw` | 80 |  |  |
| `tank_windup_time` | .5 | Time from intent to swing that swing actually happens. | `1026c9e0` |
| `teamplayroundbased_gamerules_data` | DT_TeamplayRoundBasedRulesProxy | baseclass | `10099c00` `10169220` |
| `teamwork_event_display_time` | 8.0 |  | `1023fc80` |
| `teamwork_event_fade_in_time` | 0.25 |  |  |
| `teamwork_event_fade_out_time` | 2.0 |  |  |
| `teamwork_event_min_interval` | 3.0 |  | `1023fc80` |
| `terror_gamerules_data` | DT_TerrorGameRulesProxy | baseclass | `1021c130` |
| `terror_reload_team_display` | Reload team display |  |  |
| `terror_test_hud_glow` | Tests hud glow |  | `102401f0` `10240410` |
| `test_freezeframe` | Test the freeze frame code. |  | `1018f5c0` |
| `think_limit` | 10 | Maximum think time in milliseconds, warning is printed if this is exceeded. | `101362e0` |
| `thirdperson_mayamode` | Switch to thirdperson Maya-like camera controls. |  |  |
| `thirdperson_platformer` | 0 | Player will aim in the direction they are moving. | `1010fcd0` `101126a0` `101128d0` +2 |
| `thirdperson_screenspace` | 0 | Movement will be relative to the camera, eg: left means screen-left | `1010fcd0` `101128d0` `10112ae0` |
| `tick_count` | ? |  | `1016b660` |
| `tir_maxpitch` | 15 | TrackIR Max Pitch | `10114c30` |
| `tir_maxroll` | 90 | TrackIR Max Roll | `10114c30` |
| `tir_maxx` | 4 | TrackIR Max X | `10114c30` |
| `tir_maxy` | 6 | TrackIR Max Y | `10114c30` |
| `tir_maxz` | 1 | TrackIR Max Z | `10114c30` |
| `tir_start` | 0 | TrackIR Start | `10114c30` |
| `toggle_duck` | ? |  |  |
| `toggle_multimap_scoreboard` | Toggles the multi map versus scoreboard for testing |  | `1025a9c0` `1025b030` |
| `tongue_hit_delay` | 20 | How long a smoker must wait to shoot his tongue after a hit, from the time he lets go. |  |
| `tongue_miss_delay` | 15 | How long a smoker must wait to shoot his tongue after a miss. |  |
| `tongue_start_pull_delay` | 0.1 | How long from tongue contact to tongue pulling. | `10225130` |
| `tongue_victim_acceleration` | 30 | Acceleration while tongued. | `10225130` |
| `tongue_victim_accuracy_penalty` | 0.133 | How much someone's accuracy suffers while being dragged by a tongue. | `1026e9d0` |
| `tongue_victim_max_speed` | 175 | The fastest the tongue can get you going. | `1020f7f0` `10225130` |
| `tracer_extra` | 1 | TeslaZap |  |
| `ui_posedebug_fade_in_time` | 0.2 | Time during which a new pose activity layer is shown in green in +posedebug UI | `10143c10` |
| `ui_posedebug_fade_out_time` | 0.8 | Time to keep a no longer active pose activity layer in red until removing it from +posedebug UI | `10143c10` |
| `v_centermove` | 0.15 |  | `10177c00` |
| `v_centerspeed` | 500 |  | `10177c00` |
| `vcollide_wireframe` | 0 | Render physics collision models in wireframe | `10038bc0` `1011e620` `10186f20` |
| `verbose_tank_music` | 0 |  | `101f5a90` `101f5d00` `101f7ce0` |
| `versus_shove_hunter_fov` | 90 | Angle that survivor shove will hit hunters |  |
| `versus_shove_hunter_fov_pouncing` | 45 | Angle that survivor shove will hit hunters while pouncing |  |
| `versus_stats_highlight_interval` | 5 | Interval between hightlight screens in the transition stats panel for versus mode |  |
| `vgui_screen_panel` | ? |  | `1009dfd0` `1009e410` |
| `view_offset` | 0 |  | `1020fdf0` |
| `view_offset_down` | 16 |  | `1020f000` |
| `view_offset_forward` | 8 |  | `1020f000` |
| `view_offset_up` | -16 |  | `1020f000` |
| `viewanim_addkeyframe` | ? |  |  |
| `viewanim_create` | ? |  |  |
| `viewanim_load` | load animation from file |  | `10022910` `10096e10` `100cd0d0` +3 |
| `viewanim_reset` | reset view angles! |  |  |
| `viewanim_save` | Save current animation to file |  |  |
| `viewanim_test` | test view animation |  |  |
| `viewmodel_fov` | 50 |  |  |
| `violence_ablood` | 1 | Draw alien blood | `1016d960` `1016d9b0` `1016dbf0` |
| `violence_agibs` | 1 | Show alien gib entities | `1016d960` |
| `violence_hblood` | 1 | Draw human blood | `1016d960` `1016d9b0` `1016dbf0` |
| `violence_hgibs` | 1 | Show human gib entities | `1016d960` |
| `vm_debug` | 0 |  | `10064f50` |
| `vm_draw_addon` | 1 |  |  |
| `vm_draw_always` | 0 |  |  |
| `vm_fade` | 1 |  | `10065520` |
| `voice_all_icons` | 0 | Draw all players' voice icons | `1010bf70` `10191350` |
| `voice_clientdebug` | 0 |  | `10190bc0` `101910e0` `101919b0` |
| `voice_head_icon_height` | 20 | Voice icons are this many inches over player eye positions | `10191350` |
| `voice_head_icon_size` | 6 | Size of voice icon over player heads in inches | `10191350` |
| `voice_local_icon` | 0 | Draw local player's voice icon | `10191350` |
| `voice_modenable` | 1 | Enable/disable voice in this mod. | `10190bc0` |
| `voice_spatialize` | 2.2 | Spatializes voice chat | `10213b90` |
| `voice_vox` | 0 | Voice chat uses a vox-style always on | `102126e0` `10212700` `103af3a0` |
| `vote_ended` | ? |  | `1024ec50` `1024f790` `102502b0` |
| `vote_failed` | ? |  | `1024ec50` `1024f790` `102502b0` |
| `vote_passed` | ? |  | `1024ec50` `1024f790` `102502b0` |
| `vs_score_graph_bias` | 0.777 | CMultiMapVersusModeScoreboardRow | `1025b420` |
| `weapon_autoshotgun` | CAutoShotgun |  | `1025e640` |
| `weapon_basecsgrenade` | 0.5 | CBaseCSGrenade |  |
| `weapon_boomer_claw` | CBoomerClaw |  | `1025fdd0` |
| `weapon_cs_base` | 0.8 | CWeaponCSBase | `101e4620` |
| `weapon_first_aid_kit` | CFirstAidKit |  | `102651a0` |
| `weapon_gascan` | CGasCan |  | `10265700` |
| `weapon_hunter_claw` | CHunterClaw |  | `10266d90` |
| `weapon_hunting_rifle` | CSniperRifle |  | `1026bbe0` |
| `weapon_molotov` | CMolotov |  | `10267e90` |
| `weapon_oxygentank` | COxygenTank |  | `10268bd0` |
| `weapon_pain_pills` | CPainPills |  | `10269450` |
| `weapon_pipe_bomb` | CPipeBomb |  | `10269a40` |
| `weapon_pistol` | CPistol |  | `1026aa40` |
| `weapon_propanetank` | CPropaneTank |  | `1026ac30` |
| `weapon_pumpshotgun` | CPumpShotgun |  | `1026b1e0` |
| `weapon_reparse_client` | Reloads the weapon script files |  | `102739a0` |
| `weapon_rifle` | CAssaultRifle |  | `1025df60` |
| `weapon_smg` | CSubMachinegun |  | `1026c1c0` |
| `weapon_smoker_claw` | CSmokerClaw |  | `1026b390` |
| `weapon_tank_claw` | CTankClaw |  | `1026e020` |
| `z_added_light_debug` | 0 |  |  |
| `z_added_light_max` | 0.25 |  |  |
| `z_added_light_min` | 0.025 |  |  |
| `z_avoidforce` | 2500 |  |  |
| `z_avoidteammates` | 1 |  | `1020f5c0` |
| `z_award_debug` | 0 | Get debugging info on all award calcs to the console. (0/1/2/3 for none/yes-no thoughts/every thought/every spammy calc) |  |
| `z_backspeed` | 450 |  | `10112ae0` `10210440` |
| `z_blend_attack_layer` | 1 |  | `101de0f0` `101de430` |
| `z_boomer_gibs` | 0 |  | `101d2560` |
| `z_burn_decay_rate` | 0.0 | How fast the burn effect dies on burning ragdolls | `10037830` |
| `z_camo_decay_rate` | 0.001 | How fast the camouflage effect dies on ragdolls | `10037830` |
| `z_carry_max_mass` | 250 |  |  |
| `z_cough_duration` | 5 | How long a cougher continuously coughs for. |  |
| `z_cough_fatigue` | 3000 | Stamina impact of coughing. High number will pin in place for a long time, lower will just slow. |  |
| `z_cough_interval` | 20 | How often a cougher can cough. |  |
| `z_crouch_speed` | 75 |  |  |
| `z_debug` | ? |  | `10274a60` `10274fa0` `10275b50` |
| `z_debug_infected_anim` | 0 |  | `10274a60` |
| `z_debug_infected_server_anim` | ? |  | `10274a60` `10274fa0` `10275b50` |
| `z_debug_shared_random` | 0 |  | `1016d320` `1016d400` |
| `z_difficulty` | Normal | Difficulty of the current game (Easy, Normal, Hard, Impossible) | `102206d0` |
| `z_do_tracers` | 1 | Do we want tracers at all? |  |
| `z_escape_route_alpha` | 25 |  | `10209a50` |
| `z_escape_route_far` | 500 | Escape route draws segments that are this close to the local player | `10209a50` |
| `z_escape_route_force_visible` | 0 |  | `10209a50` |
| `z_escape_route_near` | 200 | Escape route draws segments that are this far from the local player | `10209a50` |
| `z_escape_route_segment_fade_in_rate` | 150 |  | `10209a50` |
| `z_escape_route_segment_fade_out_rate` | 150 |  | `10209a50` |
| `z_exploding_speed` | 175 |  | `1020f7f0` |
| `z_forwardspeed` | 450 |  | `1010fcd0` `10112ae0` `10177c00` +1 |
| `z_frustration` | 1 |  |  |
| `z_frustration_blink_percent` | 0.75 |  |  |
| `z_frustration_blink_rate` | 10 |  |  |
| `z_gas_speed` | 210 |  | `1020f7f0` |
| `z_ghost_ahead_flow` | 500 | Ghosts must be this far ahead of the survivors to materialize |  |
| `z_ghost_delay_max` | 30 |  |  |
| `z_ghost_delay_min` | 20 |  |  |
| `z_ghost_friction` | 1.5 |  |  |
| `z_ghost_speed` | 450 |  | `1020f7f0` |
| `z_gun_damage` | 10 |  |  |
| `z_gun_debug_player_index` | 0 |  | `1025e9c0` `1026e550` `1026e800` |
| `z_gun_horiz_punch` | 0 | Toggles horizontal punchangles for guns | `1026f170` |
| `z_gun_kick` | 0 | Firing a gun can knock the player's view this amount of the current spread | `1026f170` |
| `z_gun_range` | 75 |  | `10270230` |
| `z_gun_start_empty` | 0 |  | `1026ede0` |
| `z_gun_stun_duration` | 1.25 |  |  |
| `z_gun_survivor_friend_push` | 1 | Allow survivors to push survivors with a right click. |  |
| `z_gun_swing_crosshair_mode` | 0 |  | `10241090` |
| `z_gun_swing_duration` | 0.2 |  | `10270230` |
| `z_gun_swing_interval` | 0.7 |  | `10270230` |
| `z_gun_vertical_punch` | 1 | Toggles vertical punchangles for guns | `1026f170` |
| `z_hunter_claw_force` | 240 | Force with which the claw shoves other zombies away |  |
| `z_hunter_claw_plays_hit_anims` | 0 | Play the new hit anims for debugging? |  |
| `z_hunter_claw_range` | 52 | Range of the Claw weapon |  |
| `z_hunter_claw_range_down` | 70 | Range of the Claw weapon when looking straight down, so it can reach your feet from your eyes. |  |
| `z_hunter_claw_swing_duration` | 0.2 |  |  |
| `z_hunter_claw_swing_interval` | 1 |  |  |
| `z_hunter_claw_swing_miss_interval` | 0.5 |  |  |
| `z_hunter_ground_normal` | 0.2 |  | `10218eb0` |
| `z_hunter_speed` | 300 |  |  |
| `z_infected_decals` | 0 |  | `10274490` `102744d0` |
| `z_infected_gib_count` | 0 |  | `10274240` |
| `z_infected_gib_max_speed` | 500 |  | `10274240` |
| `z_infected_gib_min_speed` | 300 |  | `10274240` |
| `z_infected_shadows` | 1 |  | `10275880` |
| `z_it_decay_rate` | 0.001 | How fast the IT effect dies | `10037830` |
| `z_lunge_cooldown` | 0.1 | Cooldown after lunge where zombies can't attack | `10223bb0` |
| `z_lunge_interval` | 0.1 |  | `10223bb0` |
| `z_lunge_power` | 600 |  |  |
| `z_lunge_reflect` | 0 | Reflects wall-kick lunges |  |
| `z_lunge_release` | 0 | Whether the hunter can release from a pounced survivor | `10223370` |
| `z_lunge_specials` | 0 | Zombies can shove special zombies with a lunge. | `1020f710` |
| `z_lunge_up` | 200 |  | `10218a80` |
| `z_max_player_zombies` | 0 | Max number of humans that can be controlling zombies at one time. | `1020fca0` |
| `z_min_ladder_mount_dot` | 0.5 | Minimum 2D dot product from player's view to a ladder to be able to grab it | `100e9c30` |
| `z_minigun_debug` | 0 |  |  |
| `z_minigun_fire_anim` | 0 | survivors use a separate anim when firing miniguns |  |
| `z_minigun_fire_anim_speed` | 1 | cycle rate for main sequence when firing miniguns | `101dd9e0` |
| `z_minigun_firing_speed` | 15 |  | `102662f0` `10267320` |
| `z_minigun_spin_down_speed` | 4 |  | `10267320` |
| `z_minigun_spin_up_speed` | 15 | Heat | `10267320` |
| `z_minigun_spread` | 7 | Minigun spray in degrees from forward |  |
| `z_minigun_stand_distance` | 36 |  | `10267460` |
| `z_mob_simple_shadows` | 0 | Control shadow quality on mobs: 0 - full shadows, 1 - blobby, 2 - no shadows |  |
| `z_nightvision_b` | 128 |  | `101df820` |
| `z_nightvision_g` | 192 |  | `101df820` |
| `z_nightvision_r` | 255 |  | `101df820` |
| `z_noise_level_fade_rate` | 40 | How much noise we lose each tick, after hold_time expires. | `101fdfc0` |
| `z_noise_level_hold_time` | 0.5 | How long we hold a given noise level before it starts to fade. | `101fdfc0` |
| `z_noise_level_max` | 135 | The highest the noise level can go. |  |
| `z_player_lunge_up` | 150 |  | `10218a80` |
| `z_pounce_crouch_delay` | 1 |  |  |
| `z_pounce_damage_range_max` | 1000 |  | `10203740` |
| `z_pounce_damage_range_min` | 500 |  | `10203740` |
| `z_puking_eye_height` | 44 |  |  |
| `z_ragdoll_impact_strength` | 500 |  | `10035c00` |
| `z_round_start_attack_prevention` | 1 | How many seconds after round start until a player is allowed to use a weapon. | `10210c30` `1021c730` |
| `z_scrimmage_a_far` | 255 |  | `1020a590` |
| `z_scrimmage_a_near` | 10 |  | `1020a590` |
| `z_scrimmage_b` | 0 |  | `1020a590` |
| `z_scrimmage_creep_delay` | 30 |  | `1020f250` |
| `z_scrimmage_creep_rate` | 50 |  | `1020f250` |
| `z_scrimmage_debug` | 0 |  | `1020a4f0` |
| `z_scrimmage_far` | 3000 |  | `1020a590` |
| `z_scrimmage_g` | 255 |  | `1020a590` |
| `z_scrimmage_height` | 50 |  | `1020a590` |
| `z_scrimmage_r` | 0 |  | `1020a590` |
| `z_scrimmage_reset` | 0 |  | `1020b7d0` |
| `z_scrimmage_yardline_a_far` | 25 |  | `1020a590` |
| `z_scrimmage_yardline_a_near` | 15 |  | `1020a590` |
| `z_scrimmage_yardline_b` | 0 |  | `1020a590` |
| `z_scrimmage_yardline_far` | 500 |  | `1020a590` |
| `z_scrimmage_yardline_g` | 255 |  | `1020a590` |
| `z_scrimmage_yardline_height` | 6 |  | `1020a590` |
| `z_scrimmage_yardline_r` | 0 |  | `1020a590` |
| `z_scrimmage_yardline_width` | 12 |  | `1020a590` |
| `z_show_swings` | 0 |  | `1026dbb0` `10272750` |
| `z_sidespeed` | 450 |  | `1010fcd0` `101128d0` `10210440` |
| `z_spawn_speed` | 450 |  |  |
| `z_spec_nightvision` | 1 | In-eye spectators will see 0=no glow, 1=normal glow, 2=weak glow | `101df820` |
| `z_speed` | 250 |  | `1020f7f0` |
| `z_splatter_delay` | 0.25 |  | `10238e70` |
| `z_splatter_limit` | 10 | Max number of splatters on-screen at once | `10238a30` |
| `z_splatteralpha` | 200 |  |  |
| `z_splatterdistance` | 100 |  |  |
| `z_splatterfade` | 1 |  |  |
| `z_splatterhold` | 3 |  |  |
| `z_tank_grenade_roll` | -10 |  | `102199c0` |
| `z_tank_speed` | 210 |  | `1020f7f0` |
| `z_tank_speed_vs` | 210 |  | `1020f7f0` |
| `z_tank_throw_fail_interval` | 0 | How soon a tank can try again after failing to throw. |  |
| `z_tank_throw_interval` | 5 | How often a tank can throw. |  |
| `z_tank_walk_speed` | 100 |  | `1020eb10` |
| `z_throwforce` | 2500 |  |  |
| `z_transitioning_player_addon_rebuild` | 1 |  |  |
| `z_use_tolerance` | 0.98 |  | `1001ed10` |
| `z_view_distance` | 0 | Default camera distance when in-eye (non-zero is pulled back like staggering, hanging, etc) | `1020fca0` |
| `z_vomit_debug` | 0 | Visualize the vomit damage. | `102260d0` |
| `z_vomit_duration` | 1.5 | How long a puker continuously pukes for. |  |
| `z_vomit_fade_duration` | 5 | How long the fade takes |  |
| `z_vomit_fade_start` | 5 | When the vomit starts to fade away |  |
| `z_vomit_fatigue` | 3000 | Stamina impact of puking. High number will pin in place for a long time, lower will just slow. |  |
| `z_vomit_interval` | 30 | How often a puker can puke. | `10225ee0` |
| `z_vomit_range` | 300 |  |  |
| `z_vomit_slide_mult` | 0.5 | Multiplier for second texture slide rate |  |
| `z_vomit_slide_rate` | 0.1 | Percentage of screen height per second |  |
| `z_vomit_target_dot` | 0.6 |  | `102260d0` |
| `z_vomit_target_range` | 280 |  | `102260d0` |
| `z_walk_speed` | 85 |  | `1020eb10` |
| `z_witch_rage_smoothing` | 0.9 |  | `102762a0` |
| `z_zombie_knockoff_death` | 0 | Knocking off a pouncing zombie instantly kills them. |  |
| `zoom_sensitivity_ratio` | 1.0 | Additional mouse sensitivity scale factor applied when FOV is zoomed in. | `10178440` |
