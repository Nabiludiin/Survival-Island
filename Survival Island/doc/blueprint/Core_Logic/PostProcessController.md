# Blueprint: PostProcessController.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/PostProcessController.md`
**Deskripsi Utama:** Mengatur efek layar URP secara dinamis dari status pemain: vignette merah saat sekarat, blur saat menyelam, chromatic aberration saat terinfeksi, serta profil suasana tiap region.
**Issue:** #23
**Rujukan GDD:** Mendukung bagian 3 (status dan infeksi) dan bagian 5 (suasana region). Bentuk efeknya adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`PostProcessController : MonoBehaviour` adalah singleton per scene, berada di prefab `PostFxRig` pada `ShipPhase` dan `IslandPhase`. Komponen ini hilang bersama scene, jadi tidak ada reset run. Prefab memuat tiga `Volume` global, di layer `Default`. `Volume Mask` pada kamera memuat `Default`.

| Volume | Priority | Isi | Bobot |
|---|---|---|---|
| `RegionVolumeA`, `RegionVolumeB` | 0 | `postProfile` dari #92 | Crossfade `A` ke `B` |
| `StatusVolume` | 10 | Salinan `StatusProfile` | Selalu 1. Parameter tiap efek yang berubah |

**Aturan profil region:** `postProfile` hanya boleh mengatur Bloom, Tonemapping, White Balance, `postExposure`, dan `contrast`. Vignette, Chromatic Aberration, Depth of Field, `saturation`, dan `colorFilter` milik `StatusVolume`, sehingga kedua lapisan tidak saling menimpa.

### Efek pada `StatusProfile`

| Komponen URP | Parameter yang diatur | Dipakai untuk |
|---|---|---|
| `Vignette` | `intensity`, `color` | HP rendah, luka, beku, kehabisan napas, tidur |
| `ChromaticAberration` | `intensity` | Infeksi |
| `DepthOfField` | Mode `Gaussian`, `gaussianStart`, `gaussianEnd` | Blur bawah air |
| `ColorAdjustments` | `saturation`, `colorFilter` | Infeksi, bawah air, mati |

Mode Gaussian dipilih, karena lebih ringan dari Bokeh.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `PostProcessController` | `null` | Singleton per scene |
| `statusProfile` | `VolumeProfile` | `Instantiate(sharedProfile)` di `Awake` | Salinan, supaya aset tidak berubah saat Play Mode |
| `hpTarget`, `hpCurrent` | `float` | 0 | HP rendah |
| `woundTarget`, `woundCurrent` | `float` | 0 | Tepi merah dari luka |
| `hitPulse` | `float` | 0 | Kilas saat terkena damage |
| `freezeTarget`, `freezeCurrent` | `float` | 0 | Tepi biru |
| `breathTarget`, `breathCurrent` | `float` | 0 | Tepi gelap saat napas menipis |
| `infectionTarget`, `infectionCurrent` | `float` | 0 | Dari `stageVision` |
| `underwaterTarget`, `underwaterCurrent` | `float` | 0 | Blur dan warna air |
| `sleepTarget`, `sleepCurrent` | `float` | 0 | Tepi gelap saat tidur |
| `deathTarget`, `deathCurrent` | `float` | 0 | Warna hilang saat mati |
| `activeWounds` | `int` | 0 | Jumlah `Bleeding` dan `WoundInfection` yang aktif |
| `snapNextFrame` | `bool` | `true` | Efek langsung tiba di target, tanpa transisi |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.postProcess.lowHpStartRatio` | 0,4 | Vignette mulai tampil di bawah 40 persen HP |
| `GameBalanceConfig.postProcess.lowHpMaxVignette` | 0,5 | Intensitas saat HP mendekati 0 |
| `GameBalanceConfig.postProcess.lowHpPulseHz` | 1,2 | Denyut saat `LowHP` |
| `GameBalanceConfig.postProcess.lowHpPulseAmount` | 0,08 | Kekuatan denyut |
| `GameBalanceConfig.postProcess.woundVignette` | 0,25 | Tepi merah dari luka |
| `GameBalanceConfig.postProcess.hitPulseAmount` | 0,2 | Kilas saat kena damage |
| `GameBalanceConfig.postProcess.hitPulseDecayPerSec` | 1 | Hilangnya kilas |
| `GameBalanceConfig.postProcess.freezeVignette` | 0,4 | Tepi biru |
| `GameBalanceConfig.postProcess.breathVignette` | 0,55 | Tepi gelap saat napas habis |
| `GameBalanceConfig.postProcess.breathStartRatio` | 0,4 | Mulai di bawah 40 persen napas |
| `GameBalanceConfig.postProcess.sleepVignette` | 0,6 | Tepi gelap saat tidur |
| `GameBalanceConfig.postProcess.infectionSaturationMin` | -35 | Saturasi saat `stageVision` bernilai 1 |
| `GameBalanceConfig.postProcess.underwaterBlurStart` | 0 | Awal blur, dalam meter |
| `GameBalanceConfig.postProcess.underwaterBlurEnd` | 8 | Akhir blur, dalam meter |
| `GameBalanceConfig.postProcess.changePerSec` | 1,5 | Kecepatan efek umum, dalam nilai 0 sampai 1 per detik |
| `GameBalanceConfig.postProcess.underwaterChangePerSec` | 6 | Masuk dan keluar air lebih cepat |
| `GameBalanceConfig.postProcess.deathDesaturateSeconds` | 2 | Warna hilang saat mati |

Warna (`lowHpColor`, `freezeColor`, `breathColor`, `infectionTint`, dan `underwaterTint`) berada di prefab, karena perlu dilihat langsung di Editor.

Nilai `GameBalanceConfig.infection.stageVision` tetap milik #64: `[0; 0,15; 0,4; 0,8]`.

## 2. Detail Logika & Fungsi

**`void Awake()`**:

1. Salin profil: `statusProfile = Instantiate(statusVolume.sharedProfile)`, lalu `statusVolume.sharedProfile = statusProfile`.
2. Ambil komponen lewat `statusProfile.TryGet(out vignette)` dan seterusnya, satu kali, lalu simpan referensinya.
3. Pastikan `overrideState = true` pada parameter yang diubah.

**`void OnEnable()`** berlangganan, lalu membaca nilai `Last` dari event sticky dan mengatur `snapNextFrame = true`. Setelah load, efek langsung sesuai status, tanpa fade.

### Sumber nilai

Semua handler hanya mengisi target. Tidak ada perubahan Volume di dalam handler.

| Event | Target |
|---|---|
| `VitalsChangedEvent` | `hpTarget = Mathf.InverseLerp(lowHpStartRatio * SurvivalStats.MaxHP, 0f, e.hp)`. `MaxHP` dibaca langsung, karena buff Lucky Machine bisa mengubahnya |
| `VitalWarningChangedEvent` | `isPulsing = flags.HasFlag(LowHP)`. Tanda `Drowning` memastikan `breathTarget = 1` |
| `PlayerDamagedEvent` | `hitPulse = hitPulseAmount`, kecuali jenis damage berkala: `Starvation`, `Dehydration`, `Infection`, `Frost`, `Poison`, `Drown` |
| `DiseaseChangedEvent` | `activeWounds` berubah untuk `Bleeding` dan `WoundInfection`. `woundTarget = activeWounds > 0 ? 1 : 0` |
| `WarmthChangedEvent` | `freezeTarget = e.isFreezing ? 1 : 0` |
| `SwimStateChangedEvent` | `underwaterTarget = e.isUnderwater ? 1 : 0`. `breathTarget = InverseLerp(breathStartRatio, 0, e.breathRatio)` |
| `InfectionStageChangedEvent` | `infectionTarget = stageVision[e.stage]` |
| `SleepStateChangedEvent` | `sleepTarget = e.isSleeping ? 1 : 0` |
| `PlayerDiedEvent` | `deathTarget = 1` |

### Pembaruan per frame

**`void Update()`** memakai `Time.deltaTime`, sehingga efek ikut berhenti saat jeda:

1. Dekatkan semua nilai `Current` ke target dengan `Mathf.MoveTowards`. Kalau `snapNextFrame`, langsung samakan.
2. Hitung vignette:

```csharp
float hp = hpCurrent * lowHpMaxVignette;
bool reduceFlashing = LocalizationManager.Instance.Accessibility.reduceFlashing;
if (isPulsing && !reduceFlashing) hp += Mathf.Sin(Time.time * lowHpPulseHz * Mathf.PI * 2f) * lowHpPulseAmount;
float red = Mathf.Max(hp, woundCurrent * woundVignette) + hitPulse;
float blue = freezeCurrent * freezeVignette;
float dark = Mathf.Max(breathCurrent * breathVignette, sleepCurrent * sleepVignette);

float total = red + blue + dark;
vignette.active = total > 0.001f;
if (vignette.active)
{
    vignette.intensity.value = Mathf.Clamp01(Mathf.Max(red, Mathf.Max(blue, dark)));
    vignette.color.value = (lowHpColor * red + freezeColor * blue + breathColor * dark) / total;
}
```

Warna dibobot, supaya pemain yang sekarat di salju melihat tepi ungu kemerahan, dan sumber kuat tetap dominan.

3. Infeksi: `chromatic.intensity.value = infectionCurrent * caAllowed`, dengan `caAllowed` bernilai 1, atau 0 kalau `SettingsManager.chromaticAberrationEnabled` mati. Infeksi tetap tampil lewat warna.
4. Warna: `saturation = infectionCurrent * infectionSaturationMin`, lalu dikurangi menuju -100 lewat `deathCurrent`. `colorFilter` adalah `Color.Lerp` dari putih menuju `infectionTint` lalu `underwaterTint`.
5. Blur: `dof.active = underwaterCurrent > 0.001f`. `gaussianEnd` berjalan dari 200 ke `underwaterBlurEnd`, mengikuti `underwaterCurrent`.
6. `hitPulse = Mathf.MoveTowards(hitPulse, 0f, hitPulseDecayPerSec * Time.deltaTime)`.

Efek yang bernilai 0 memakai `active = false`, supaya tidak menambah beban render. Tidak ada alokasi, karena komponen sudah disimpan.

### Profil region

**`public void SetRegionProfile(VolumeProfile profile, float blendSeconds)`** dipanggil `EnvironmentDirector` (#92):

1. Tentukan volume yang tidak aktif. Isi `sharedProfile`-nya dengan `profile`.
2. Jalankan transisi: bobot lama turun dan bobot baru naik selama `blendSeconds`, memakai `Time.deltaTime`.
3. Kalau dipanggil di tengah transisi, mulai dari bobot saat ini, supaya tidak melompat.
4. `blendSeconds <= 0` langsung selesai. Dipakai saat load.
5. `profile == null` berarti bobot region turun ke 0.

### Fungsi tambahan

**`public void ClearStatusEffects()`** mengatur semua target ke 0 dan `snapNextFrame = true`. Dipakai `DeathCinematic` (#53) atau `NarrativeSequencer` (#99) kalau adegan membutuhkan layar bersih.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `VitalsChangedEvent`, `VitalWarningChangedEvent`, `PlayerDamagedEvent`, `PlayerDiedEvent`, `WarmthChangedEvent`, `InfectionStageChangedEvent`, `SwimStateChangedEvent` (#13), `DiseaseChangedEvent` (#45), `SleepStateChangedEvent` (#67).

| Sistem | Hubungan |
|---|---|
| `VitalsRules` (#111) | `postProcessVolume` sudah dihapus dari `SurvivalStats`, dan efek layar pindah ke sini |
| `InfectionSystem` (#64) | Menyediakan `stage` dan `stageVision` |
| `SwimmingController` (#13) | `isUnderwater` dan `breathRatio` |
| `WarmthSystem` (#65) | `isFreezing` |
| `DiseaseManager` (#45) | `Bleeding` dan `WoundInfection`. `DiseaseOverlay` hijau tetap di HUD |
| `EnvironmentDirector` (#92) | `SetRegionProfile` |
| `FirstPersonCamera` (#102) | `WorldCamera` dan `ViewmodelCamera` mengaktifkan Post Processing |
| `SettingsManager` (#19) | `chromaticAberrationEnabled` |
| `LocalizationManager` (#104) | `reduceFlashing`, sesuai rujukan `DiseaseManager.md` |
| `ShipHazardManager` (#75), `ShipTimerManager` | Tanda `Drowning` lewat event. `vignetteOverlay` timer tetap berupa UI |
| `DevConsole` (#56) | Perintah `postfx <efek> <0..1>` untuk memaksa target |

**Batas pekerjaan:** indikator arah serangan di #51, overlay hijau di #45, dan layar hitam adegan di #53 dan #99.

## 4. Kasus Khusus

- **Objek pool:** tidak memakai `ObjectPooler`. Hanya ada satu rig per scene.
- **Game dijeda:** `Time.deltaTime` dan `Time.time` berhenti, jadi efek dan denyut membeku, lalu lanjut setelah jeda.
- **Permadeath:** tidak ada data. Rig hilang bersama scene, `Destroy(statusProfile)` dipanggil di `OnDestroy`, dan event sticky dibersihkan oleh #82. Run baru mulai bersih.
- **Save dan load:** tidak ada yang disimpan. Manager mengirim ulang event sticky lewat `ISaveable` (#80), dan `snapNextFrame` membuat efek langsung sesuai.
- **Pindah scene:** rig `ShipPhase` terbuang, dan rig `IslandPhase` mulai dari event sticky yang ada.
- **Beberapa efek bersamaan:** vignette memakai bobot warna, sehingga HP rendah, beku, dan napas habis tetap terbaca.
- **Keluar air saat napas tipis:** `breathTarget` turun mengikuti `breathRatio`, sehingga tepi gelap hilang bertahap.
- **Region dilepas di tengah transisi:** profil adalah aset, bukan isi scene region, sehingga referensinya tetap sah.
- **Aset profil rusak atau komponen hilang:** `TryGet` gagal, efek itu dilewati, dan Editor mencatat peringatan.
- **Perubahan parameter saat Play Mode:** hanya mengubah salinan, sehingga aset tidak ikut berubah.
- **Tangan dan efek layar:** Camera Stack (#102) perlu dicek di Unity, karena blur juga harus mengenai `ViewmodelCamera` saat menyelam.
- **Menu utama:** tidak ada rig, sehingga tidak ada efek.
- **`stage` di luar batas larik:** dibatasi dengan `Mathf.Clamp`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SetRegionProfile` (`EnvironmentProfile.md`), `stageVision` (`InfectionSystem.md`), URP dengan Camera Stack (usulan di `FirstPersonCamera.md`), serta efek layar yang lewat `VitalWarningChangedEvent` (`VitalsRules.md`).

**Keputusan teknis di blueprint ini:**

- Profil status disalin saat `Awake`, supaya aset aman.
- Handler event hanya mengisi target, dan Volume hanya berubah di `Update`.
- Efek bernilai nol dimatikan.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Masa hidup:** rekomendasi prefab per scene, bukan manager persisten, supaya efek tidak perlu reset dan tidak mengubah tabel #82.
2. **Bentuk efek:** rekomendasi vignette untuk HP dan napas, Chromatic Aberration ditambah saturasi untuk infeksi, serta DOF Gaussian ditambah warna air untuk menyelam.
3. **Batas HP:** rekomendasi efek mulai pada 40 persen, dan denyut baru mulai saat tanda `LowHP` di 25 persen.
4. **Pilihan pemain:** Chromatic Aberration bisa dimatikan di #19, karena mengganggu sebagian pemain.
5. **`DiseaseOverlay`:** rekomendasi tetap di HUD, karena sudah memenuhi #45 dan tidak butuh Volume.
6. **Layer Volume:** rekomendasi `Default`, supaya tidak perlu layer baru di `PhysicsLayers.md`.

## 6. Skenario Tes

Belum diuji di Unity, karena proyek Unity belum dibuat.

1. `setvital hp 35`. **Hasil:** tepi merah mulai terlihat, tanpa denyut.
2. `setvital hp 10`. **Hasil:** tepi merah lebih kuat, dan berdenyut.
3. Isi HP penuh. **Hasil:** efek hilang bertahap, dan `vignette.active` menjadi `false`.
4. Terima serangan zombie. **Hasil:** kilas merah singkat. Saat lapar nol dan HP turun perlahan: tidak ada kilas.
5. Menyelam. **Hasil:** blur dan warna air masuk dengan cepat. Naik: hilang dengan cepat. FPS dicatat di lembar playtest.
6. Tahan napas sampai 30 persen. **Hasil:** tepi gelap bertambah, dan hilang bertahap setelah naik.
7. Naikkan infeksi melewati tiap tahap. **Hasil:** Chromatic Aberration dan warna mengikuti `0; 0,15; 0,4; 0,8`.
8. Matikan `chromaticAberrationEnabled`. **Hasil:** garis warna hilang, tetapi warna layar tetap berubah.
9. Aktifkan `reduceFlashing` dengan HP rendah. **Hasil:** tepi merah tetap, tanpa denyut.
10. Masuk Region 3 tanpa pakaian hangat. **Hasil:** tepi biru saat `isFreezing`.
11. HP rendah di area beku. **Hasil:** warna tepi campuran, dan intensitas mengikuti yang terkuat.
12. Pindah dari Region 1 ke Region 2 dan kembali di tengah transisi. **Hasil:** suasana tidak melompat.
13. Jeda saat HP rendah. **Hasil:** denyut berhenti, lalu lanjut.
14. Simpan dengan infeksi tinggi saat di bawah air, lalu lanjutkan. **Hasil:** efek langsung sesuai status, tanpa fade.
15. Mati. **Hasil:** warna hilang selama 2 detik. Run baru mulai dengan layar bersih.
16. Keluar dari Play Mode setelah efek aktif. **Hasil:** aset `StatusProfile` tidak berubah.
17. Buka Profiler selama 60 detik. **Hasil:** tidak ada alokasi GC dari `Update`.

## Catatan Implementasi (Sistem pendukung)

- Kode: `PostProcessController.cs`. Config di `GameBalance.FondasiA.cs`.
- `InfectionSystem` mengirim tahap 1 sampai 4, jadi indeks `stageVision` adalah `stage - 1` (dibatasi), bukan `stageVision[e.stage]`.
- `SetRegionProfile`: profil baru diisi ke volume yang bobotnya lebih kecil. Kalau dipanggil di tengah transisi, bobot berlanjut dari nilai saat ini (isi volume yang diisi ulang bisa sedikit berubah, karena profil satu volume diganti).
- `gaussianEnd` saat tidak menyelam (200 m) berada di field prefab `dofClearEndMeters`.
- `colorAdjust.active` menyala saat infeksi, bawah air, atau mati.
- Belum ada (TODO): `SwimStateChangedEvent` (#13) dan `SleepStateChangedEvent` (#67) belum dideklarasikan pemiliknya, jadi efek bawah air, napas dari renang, dan tidur belum tersambung (tanda `Drowning` sudah). Perintah `postfx` di `DevConsole`. `screenTint` milik `DeathCinematic` tidak punya API di blueprint ini.
