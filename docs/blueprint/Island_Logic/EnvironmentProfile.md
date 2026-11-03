# Blueprint: EnvironmentDirector.cs & RegionEnvironmentProfile (ScriptableObject)
**Lokasi:** `docs/blueprint/Island_Logic/EnvironmentProfile.md`
**Deskripsi Utama:** Menggabungkan suasana region, jam, cuaca, dan event menjadi satu hasil, lalu menjadi satu-satunya sistem yang menulis fog dan cahaya.
**Issue:** #92
**Rujukan GDD:** Bagian 5 (suasana tiap region, Region 2 gelap meski siang) dan bagian 3 (siang dan malam). Nilai awal adalah usulan.

## 1. Arsitektur & Variabel Utama

`EnvironmentDirector` adalah manager di scene `IslandPhase`. Sistem lain tidak boleh menulis `RenderSettings.fog*`, `RenderSettings.ambientLight`, `RenderSettings.skybox`, atau `Light` matahari.

### `RegionEnvironmentProfile`

| Field | Fungsi |
|---|---|
| `fogColor`, `fogDensity` | Dasar fog, mode `ExponentialSquared` |
| `nightFogMultiplier` | Pengali fog malam |
| `ambientColor`, `ambientByHour` (`Gradient`) | Warna dasar dan pengali per jam |
| `sunColorByHour` (`Gradient`), `sunIntensity`, `sunIntensityByHour` (`AnimationCurve`) | Cahaya matahari dan bulan |
| `minAmbientLuminance` | Batas bawah terang |
| `skybox` | Tekstur langit |
| `ambientParticleTag` | Tag pool partikel region |
| `postProfile` | `VolumeProfile` untuk #23 |

### Nilai awal region

Pada fog `ExponentialSquared`, jarak pandang sekitar `2 / fogDensity`.

| Region | `fogDensity` (jarak) | Warna fog | Ambient | `sunIntensity` | `nightFogMultiplier` | Partikel | Post-processing |
|---|---|---|---|---|---|---|---|
| 1 `Volcano` | 0,004 (500 m) | `#B8C4C8` | `#8C9AA6` | 1,1 | 1 | `Fx_AshLight` | `PP_Volcano`, sedikit hangat |
| 2 `Amazon` | 0,03 (66 m) | `#2E4034` | `#33443A` | 0,35 | 0,6 | `Fx_Spores` | `PP_Amazon`, hijau dengan vignette |
| 3 `Snow` | 0,012 (166 m) | `#D3E0EA` | `#A9C0D6` | 0,9 | 0,8 | `Fx_SnowDust` | `PP_Snow`, kebiruan |
| 4 `Logistic` | 0,02 (100 m) | `#4A4F4A` | `#4C5450` | 0,5 | 0,8 | `Fx_Mist` | `PP_Village`, pudar |

### `WeatherEnvironmentLayer`

Satu baris per `WeatherType`, disimpan sebagai `GameBalanceConfig.environment.weatherLayers`.

| Cuaca | Tambahan `fogDensity` | Pengali matahari | Pengali ambient | Warna fog (bobot) |
|---|---|---|---|---|
| `Clear` | 0 | 1 | 1 | Tanpa perubahan |
| `Rain` | 0,006 | 0,7 | 0,8 | `#6B7780` (0,4) |
| `Fog` | 0,03 | 0,75 | 0,85 | `#9AA3A8` (0,6) |
| `Storm` | 0,012 | 0,35 | 0,6 | `#3C4348` (0,6) |
| `Snow` | 0,01 | 0,8 | 0,95 | `#D9E4EC` (0,5) |
| `Blizzard` | 0,07 (sekitar 25 m) | 0,3 | 0,65 | `#E4ECF2` (0,9) |

`Clear` tidak menyimpan nilai kembali, jadi fog otomatis kembali ke dasar region. Ini menyelesaikan masalah `WeatherManager` lama.

### Nilai config

| Nilai | Awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.environment.regionBlendSeconds` | 8 | Transisi region |
| `GameBalanceConfig.environment.weatherBlendSeconds` | 12 | Transisi cuaca |
| `GameBalanceConfig.environment.eventBlendSeconds` | 4 | Transisi event |
| `GameBalanceConfig.environment.interiorBlendSeconds` | 1,5 | Masuk dan keluar `InteriorZone` |
| `GameBalanceConfig.environment.applyIntervalSeconds` | 0,1 | Jeda penulisan |
| `GameBalanceConfig.environment.maxFogDensity` | 0,1 | Batas fog, sekitar 20 m |
| `GameBalanceConfig.environment.defaultMinAmbientLuminance` | 0,035 | Batas bawah terang |
| `GameBalanceConfig.environment.eruptionRemoteWeight` | 0,35 | Bobot langit merah di luar Region 1 |

## 2. Detail Logika & Fungsi

### Urutan gabungan

`struct EnvState` berisi `fogColor`, `fogDensity`, `ambient`, `sunColor`, dan `sunIntensity`. Nilainya dihitung tanpa alokasi, setiap `applyIntervalSeconds`.

**`private void Compute()`**:

1. **Region:** `s = Lerp(FromProfile(prev), FromProfile(active), regionT)`.
2. **Jam:** dengan `t = jam / 24`, kalikan `ambient` dengan `ambientByHour(t)`, isi `sunColor = sunColorByHour(t)`, kalikan `sunIntensity` dengan `sunIntensityByHour(t)`, dan kalikan fog malam dengan `nightFogMultiplier`.
3. **Cuaca:** gabungkan lapisan cuaca lama dan baru dengan `weatherT`. Fog ditambah, cahaya dikalikan, dan warna fog di-lerp.
4. **Event:** ambil `EnvironmentEventLayer` dengan prioritas tertinggi, lalu lerp sebesar `bobot * eventT`.
5. **Interior:** kalau pemain berada di `InteriorZone`, lerp ke `lightProfile`. Kalau `isDark`, matahari menjadi 0.
6. **Batas:** `fogDensity = Min(fogDensity, maxFogDensity)`, dan naikkan `ambient` sampai luminansinya minimal `minAmbientLuminance`.

Hasilnya ditulis oleh **`private void Apply(in EnvState s)`** ke `RenderSettings.fogColor`, `RenderSettings.fogDensity`, `RenderSettings.ambientLight`, `sun.color`, dan `sun.intensity`, hanya kalau berubah lebih dari ambang kecil.

`RenderSettings.ambientLight` dikalikan `SettingsManager.Instance.brightness` (#19). Kalau luminansi hasilnya di bawah `minAmbientLuminance`, naikkan sampai batas itu.

Rotasi matahari ditulis dari `DayNightDirector.Instance.SunRotation`. `RenderSettings.ambientMode` tetap `Flat`, sehingga `DynamicGI.UpdateEnvironment` tidak dipanggil dan tidak ada lonjakan beban.

### Lapisan event

| ID | Prioritas | Warna fog | Tambahan fog | Pengali matahari | Warna langit | Transisi masuk / keluar | Pemanggil |
|---|---|---|---|---|---|---|---|
| `BloodMoon` | 30 | `#6B0F12` | 0,01 | 0,8, warna merah | `#801010` | 20 / 10 detik | `HordeManager` (#50) |
| `Eruption` | 20 | `#A5391F` | 0,008 | 0,6, warna jingga | Tidak ada | `eventBlendSeconds` | `DisasterManager` (#70) |

Warna langit dan transisi `BloodMoon` adalah usulan dari #50. `PushEventLayer` dan `PopEventLayer` tetap sama.

**`public void PushEventLayer(EnvEventId id, float weight)`** dan **`public void PopEventLayer(EnvEventId id)`** dipanggil langsung, karena pengirimnya memang bergantung pada director ini. Saat erupsi, `DisasterManager` memberi bobot 1 kalau region aktif `Volcano`, dan `eruptionRemoteWeight` di region lain.

Blood Moon menang atas erupsi, walaupun `DisasterManager` sudah mencegah keduanya terjadi pada hari yang sama.

### Transisi region

**`HandleRegionChanged(RegionChangedEvent e)`**:

1. `prev = nilai saat ini`, `active = profiles[e.to]`, dan `regionT = 0`. Karena memakai nilai saat ini, perpindahan bolak-balik di tengah transisi tidak melompat.
2. `regionT` naik selama `regionBlendSeconds` dengan `Time.deltaTime`.
3. **Skybox:** material `SkyboxBlended` memakai `_TexA`, `_TexB`, dan `_Blend`. Material dibuat sekali, dan hanya isinya yang berubah.
4. **Partikel:** ambil partikel baru dari `ObjectPooler`, naikkan emisi selama transisi, turunkan partikel lama, lalu kembalikan ke pool setelah selesai.
5. **Post-processing:** panggil `PostProcessController.SetRegionProfile(profile.postProfile, regionBlendSeconds)` (#23).

### Zona bertumpuk

`RegionChangedEvent` dikirim oleh `ZoneManager`, jadi director hanya menerima satu nilai yang sudah pasti. Masalah `OnTriggerExit` lama diselesaikan oleh `ZoneTracker`, yaitu kelas statis yang dipakai semua `ZoneManager`:

- **`Enter(RegionType r)`:** tambah hitungan `r`, dan tandai `r` sebagai zona terakhir yang dimasuki.
- **`Exit(RegionType r)`:** kurangi hitungan. Kalau `r` adalah region aktif dan hitungannya 0, pilih zona terakhir yang hitungannya masih di atas 0.
- **Tidak ada zona sama sekali:** region aktif tidak berubah, jadi celah antar-trigger tidak mengubah suasana.
- **Keputusan:** hitungan zona dipakai untuk keabsahan, dan zona terakhir untuk memilih.

`OnTriggerExit` tidak lagi menormalkan apa pun. Frostbite pindah ke `WarmthSystem` (#65).

### Senter di area gelap

Tujuannya: Region 2 dan malam terasa perlu senter, tetapi layar tidak hitam total.

- **Target luminansi ambient (bisa disetel):** Region 2 siang sekitar 0,18, Region 2 malam sekitar 0,04, dan Region 1 malam sekitar 0,06.
- **Batas bawah:** `minAmbientLuminance` mencegah layar hitam, termasuk saat `Storm` di malam hari.
- **Fog malam:** warna fog ikut pengali ambient. `nightFogMultiplier` 0,6 di Region 2 mengurangi kesan tembok fog yang terlihat terang saat disorot senter.
- **Exposure:** eye adaptation dimatikan di semua `postProfile`, supaya kegelapan tidak hilang sendiri.
- `FlashlightController` tidak berubah, dan tidak menulis fog atau ambient. Pengaturan kecerahan milik #19.

### Hubungan dengan bayangan #63

`SunlightExposure` memakai `DayNightDirector.SunDirection` dan raycast `PhysicsMasks.Sunlight`, bukan intensitas cahaya.

- Redupnya Region 2, cuaca, dan fog hanya visual. Zombie tetap terbakar sesuai aturan #63, karena GDD tidak memberi pengecualian.
- Di siang hari `sunIntensity` tidak pernah 0 di luar ruangan, sehingga bayangan yang terlihat tetap selaras dengan arah raycast.
- Hanya rotasi dari `DayNightDirector` yang dipakai, jadi arah bayangan dan raycast selalu sama.
- `Storm` meredupkan layar tanpa melindungi zombie, sesuai #63.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `RegionChangedEvent`, `WeatherChangedEvent` (hanya region aktif), dan `InteriorZoneChangedEvent`. Ketiganya sticky, jadi nilai `Last` dibaca saat aktif.

| Sistem | Hubungan |
|---|---|
| `DayNightDirector` | Memberi jam, `SunRotation`, dan `SunDirection`. Tidak lagi menulis `Light` |
| `RegionalWeather` (#115) | `GetWeather(region)` |
| `WeatherManager` | Hanya partikel dan audio. Tidak menulis fog dan cahaya |
| `ZoneManager`, `ZoneTracker` | Sumber region aktif |
| `SettingsManager` (#19) | `brightness` |
| `DisasterManager` (#70), `HordeManager` (#50) | `PushEventLayer`, `PopEventLayer` |
| `PostProcessController` (#23) | Isi efek layar, di luar issue ini |
| `AmbientAudio` (#52) | Audio area, di luar issue ini |
| `ZombieLair` (#112) | `lightProfile` dan `isDark` |
| `ObjectPooler` | Partikel region |

## 4. Kasus Khusus

- **Pool:** partikel region mengosongkan partikel lama dan memulihkan `emission.rateOverTime` di `OnSpawned`.
- **Game dijeda:** `Time.deltaTime` 0, jadi transisi berhenti. Nilai terakhir tetap tertulis.
- **Tidur:** jam dipercepat, sehingga warna langit bergerak lebih cepat. Rumusnya tetap sama.
- **Permadeath dan save:** tidak ada data tersimpan. `RenderSettings` milik scene, jadi hilang saat `IslandPhase` dimuat ulang. `ShipPhase` memakai pengaturannya sendiri.
- **Load:** semua `T` langsung 1 pada `Compute` pertama, supaya tidak ada transisi saat layar baru terbuka. Lapisan event dipasang lagi oleh `DisasterManager` dan `HordeManager` saat `RestoreState`.
- **Region dilepas:** profil adalah aset yang direferensikan director, bukan isi scene region.
- **Sumber profil hilang:** kalau `profiles[r]` kosong, pakai profil Region 1 dan catat error.

## 5. Keputusan Desain

**Keputusan issue yang mengikat:** hanya satu sistem menulis fog dan cahaya, sesuai `RegionStreaming.md`.

**Usulan yang menunggu developer:**

1. **Urutan gabungan:** region, jam, cuaca, event, interior. Rekomendasi: pakai.
2. **Region aktif saat zona bertumpuk:** hitungan zona, dengan pilihan zona terakhir. Rekomendasi: pakai.
3. **Nilai fog dan warna region.** Rekomendasi: pakai sebagai awal, lalu setel lewat playtest.
4. **Target luminansi senter.** Rekomendasi: disetel di Unity pada monitor rata-rata.
5. **Rotasi matahari:** director menulis rotasi yang dihitung `DayNightDirector`. Rekomendasi: pakai, supaya `Light` hanya punya satu penulis.

## 6. Skenario Tes

1. Pindah dari Region 1 ke Region 2. **Hasil:** fog dan cahaya berubah halus dalam sekitar 8 detik.
2. Bolak-balik di seam saat transisi. **Hasil:** tidak ada lompatan warna.
3. Tumpuk dua trigger zona, lalu masuk dan keluar satu per satu. **Hasil:** `RegionChangedEvent` hanya sekali per perubahan, dan efek tidak ternormalkan saat masih di zona lain.
4. Paksa `Fog`, lalu `Clear`. **Hasil:** `fogDensity` kembali ke nilai dasar region.
5. Cari lewat teks `RenderSettings.fog` dan `directionalLight` di semua script. **Hasil:** hanya ada di `EnvironmentDirector`.
6. Berdiri di Region 2 pada tengah malam saat `Storm`, dengan senter mati lalu hidup. **Hasil:** bentuk medan masih samar saat mati, dan area sorotan jelas saat hidup.
7. Picu erupsi di Region 3. **Hasil:** langit kemerahan dengan bobot 0,35. Pindah ke Region 1 dalam event yang sama. **Hasil:** bobot menjadi 1.
8. Picu Blood Moon saat erupsi lewat `DevConsole`. **Hasil:** warna Blood Moon menang. Setelah berakhir, kembali ke warna yang tersisa.
9. Saat fajar, taruh zombie di Region 2 dalam `Storm`. **Hasil:** aturan #63 tetap berlaku.
10. Simpan di Region 4 malam hari, lalu load. **Hasil:** suasana langsung benar, tanpa transisi.
11. Ubah `brightness` ke 0,8 dan 1,2 pada malam gelap. **Hasil:** ambient mengikuti, tanpa turun di bawah `minAmbientLuminance`.
12. Picu Blood Moon. **Hasil:** langit `#801010`, masuk dalam 20 detik, dan keluar dalam 10 detik.
