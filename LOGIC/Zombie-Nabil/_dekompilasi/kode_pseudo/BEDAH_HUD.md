# Bedah HUD (client.dll)

Lokasi seperti `bagian_003.c:63536` mengacu ke folder `client\`. Path file seperti `scripts\hudlayout.res` mengacu ke `Zombie-Nabil\Zombie\`.

**Yang sudah pasti:** nama kelas, file yang dimuat, isi file, dan hasil perbandingan dengan folder AnkerGames.
**Yang masih perkiraan:** peran fungsi bertanda "(perkiraan)".
Belum ada yang diuji di dalam game.

## Masalah yang ditemukan: nama internal rusak karena penggantian nama

Semua DLL utama yang dibandingkan (`gameui.dll`, `engine.dll`, `vgui2.dll`, `materialsystem.dll`, `steam_api.dll`) dan `Zombie.exe` identik dengan versi asli (SHA-256 sama). Jadi kodenya masih meminta nama lama, sedangkan file di disk sudah diganti.

### 1. File menu tidak ditemukan

`gameui.dll` menyebut `l4d360` 403 kali dan `zombie360` 0 kali. Sebanyak 14 file yang dimintanya ada di versi asli, tetapi hilang di mod ini:

- `resource/L4D360UIScheme.res`
- `Resource/UI/L4D360UI/MainMenu.res`
- `Resource/UI/L4D360UI/InGameMainMenu.res`
- `Resource/UI/L4D360UI/MainMenu_ServerBrowser.res`
- `Resource/UI/L4D360UI/Achievements.res`
- `Resource/UI/L4D360UI/AchievementListItem.res`
- `Resource/UI/L4D360UI/Addons.res`
- `Resource/UI/L4D360UI/AddonListItem.res`
- `Resource/UI/L4D360UI/CustomCampaignListItem.res`
- `Resource/UI/L4D360UI/FoundGameListItem.res`
- `Resource/UI/L4D360UI/LeaderboardListItem.res`
- `Resource/UI/L4D360UI/QuickJoinPanelItem.res`
- `Resource/UI/L4D360UI/SliderControl.res`
- `resource/UI/L4D360UI/LoadingPosterDefault.res`

File-file ini sekarang ada dengan nama `zombie360ui`. **Menu utama kemungkinan rusak atau tidak tampil normal.** Cara kegagalannya baru pasti kalau game dites.

### 2. Teks terjemahan tidak cocok

Semua 1.018 kunci terjemahan `L4D_*` diganti menjadi `ZOMBIE_*`. File `.res` ikut diganti (217 label `#ZOMBIE_`), jadi keduanya saling cocok. Masalahnya ada pada kunci yang tertulis langsung di DLL:

| DLL | Kunci `l4d_` yang diminta | Ada di versi asli | Ada di mod ini | Hilang akibat penggantian |
|---|---|---|---|---|
| `client.dll` | 292 | 200 | 0 | 200 |
| `server.dll` | 92 | 28 | 0 | 28 |
| `gameui.dll` | 1 | 1 | 0 | 1 |

Contoh yang hilang: `l4d_class_boomer_name`, `l4d_class_tank_name`, `l4d_chat_prompt_team_survivor`, `l4d_difficultyeasy`, `l4d_scenario_finale_won`.

Teks seperti ini kemungkinan tampil sebagai kunci mentah atau kosong (perkiraan). Hitungan "diminta" dideteksi otomatis dari isi DLL, jadi bisa sedikit meleset; yang "hilang akibat penggantian" sudah dibandingkan dengan file asli.

### 3. Cara memperbaiki

Nama yang dilihat pemain cukup diubah pada **nilai** teks dan judul di `gameinfo.txt`. Nama internal, yaitu folder `L4D360UI`, file `L4D360UIScheme.res`, dan kunci `L4D_*`, sebaiknya dikembalikan ke nama asli. Daftar penggantian yang pernah dilakukan ada di `_cadangan_sebelum_rename\daftar_rename.json`, tetapi daftar itu perlu dicek karena belum tentu mencakup isi file.

## Cara kerja HUD

Pola yang sama muncul di banyak kelas HUD:

```
Konstruktor kelas:
  daftarkan elemen HUD dengan nama, contoh "HudFrustrationMeter"
  buat panel anak, contoh "FrustrationBar"
  muat tata letak dari file .res
```

| Lapisan | Mengatur | Letak |
|---|---|---|
| Kode | Logika dan kapan tampil | `client.dll` |
| `scripts\hudlayout.res` | Posisi dan ukuran tiap elemen | File teks, bisa diedit |
| `resource\ui\hud\*.res` | Isi panel: label, bar, gambar | File teks, bisa diedit |
| `scripts\hudanimations.txt` | 56 event animasi | File teks, bisa diedit |
| `scripts\hud_textures.txt`, `scripts\mod_textures.txt` | Peta ikon | File teks, bisa diedit |
| `resource\zombie_english.txt` | Teks | File teks, bisa diedit |

Jadi banyak perubahan tampilan **tidak butuh** mengedit DLL.

## Elemen HUD utama

| Elemen di `hudlayout.res` | Kelas | Konstruktor | File isi |
|---|---|---|---|
| `HudFrustrationMeter` | `CHudFrustrationMeter` | bagian_003.c:63536 | `resource\ui\hud\frustrationmeter.res` |
| `HudFinaleMeter` | `CHudFinaleMeter` | bagian_003.c:63341 | `resource\ui\hud\finalemeter.res` |
| `HudZombieHealth` | `CHudZombieHealth` | bagian_003.c:80500 | `tankhealth.res`, `zombiehealthleft_*.res` (perkiraan: dipilih saat berjalan) |
| `HudProgressBar` | `CHudProgressBar` | bagian_003.c:63899 | Diatur di kode dan `hudlayout.res` |
| `HudCrosshair` | `CHudTerrorCrosshair` (perkiraan) | bagian_003.c:77417 | Warna kemampuan ada di `hudlayout.res` |
| `HudGhostPanel` | `CHudGhostPanel` | bagian_003.c:72428 | `resource\ui\hudghostpanel.res` |
| `HudZombiePanel` | `CHudZombiePanel` | bagian_003.c:80978 | `resource\ui\zombiepanel.res` |
| `CHudZombieTeamDisplay` | `CHudZombieTeamDisplay` | bagian_003.c:81216 | `resource\ui\hud\zombieteamdisplayplayer.res` |
| `CHudTeamDisplay` | `CHudTeamDisplay` | bagian_003.c:75627 | `resource\ui\hud\teamdisplayhud.res` |
| `CHudLocalPlayerDisplay` | `CHudLocalPlayerDisplay` | bagian_003.c:66025 | Belum dilacak |
| `CHudAbilityTimer` | `CHudAbilityTimer` | bagian_003.c:62998 | `resource\ui\hud\abilitytimerhud.res` |
| `HudPZDamageRecord` | `CHudPZDamageRecordPanel` | bagian_003.c:69760 | `resource\ui\hud\pzdamagerecordpanel.res` |
| `HudIntensityGraph` | `CHudIntensityGraph` | bagian_003.c:65475 | Grafik intensitas Director, kemungkinan alat debug |
| `CHudVote` | `CHudVote` | bagian_003.c:69644 | `resource\ui\hud\votehud.res` |

Fungsi lain yang memuat file `.res`: `resource\ui\radialmenu.res` di bagian_003.c:70692, `resource\ui\hud\targetid.res` di bagian_003.c:74708, dan papan skor versus di bagian_004.c:3445.

## Contoh yang terbaca dari file

### Meter frustrasi

Posisi di `scripts\hudlayout.res`: `xpos` 10, `ypos` c0, `wide` 300, `tall` 84.

Isi di `resource\ui\hud\frustrationmeter.res`: label `Countdown`, `Warning`, `Warning2`, panel `FrustrationBar` (lebar 150, tinggi 8), dan label `FrustrationLabel`.

### Darah Tank

Di `resource\ui\hud\tankhealth.res`: bar `Health` (lebar 278), angka `HealthNumber`, gambar latar `HUD/PZ_healthbar_3000`, dan ikon `hud/crouch_infected`. Posisi panelnya diatur `HudZombieHealth` di `hudlayout.res`: `r387`, `r100`.

### Warna indikator kemampuan

Di bagian `HudCrosshair` pada `hudlayout.res`: `ability_size` 17, `ability_ready_color` 255 255 255 255, `ability_charging_color` 127 127 127 255.

## Arti posisi di file .res

| Nilai | Arti |
|---|---|
| `10` | 10 unit dari kiri atau atas |
| `c-100` | 100 unit ke kiri atau atas dari tengah |
| `r125` | 125 unit dari kanan atau bawah |

Unit ini mengikuti skala layar 640x480 standar Source (perkiraan).

## Pengaturan HUD

`cl_drawhud`, `hud_fastswitch`, `hud_showtargetid`, `hud_targetid_health`, `hud_targetid_name_height`, `hud_targetid_name_height_boomer`, `hud_targetid_name_height_hunter`, `hud_targetid_name_height_smoker`, `hud_targetid_name_height_tank`, `hud_zombieteam`, `hud_zombieteam_showself`, `hud_saytext_time`, `hud_drawhistory_time`, `hud_deathnotice_bots`, `hud_deathnotice_threats`, `cl_drawprogressbar`, `cl_hidemenu_spawnmode`, `hud_autoreloadscript`, dan `cl_crosshair_*`.

`hud_reloadscheme` kemungkinan memuat ulang HUD tanpa restart, sehingga berguna saat mengedit file `.res` (perkiraan).

## Celah analisis

- Fungsi gambar tiap elemen belum dilacak; yang saya baca terutama konstruktornya.
- Cara game menangani file menu yang hilang belum diuji.
- `engine.dll` juga menyebut `left4dead` 2 kali; dampaknya belum dilacak.
