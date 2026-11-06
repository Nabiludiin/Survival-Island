# Blueprint: WeatherManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/WeatherManager.md`
**Deskripsi Utama:** Menampilkan cuaca di tempat pemain lewat partikel dan audio. Penentuan cuaca dan efeknya milik `RegionalWeather` (#115).
**Issue:** #115 (melengkapi #65, #89, #92, dan #101)
**Rujukan GDD:** Bagian 5 (salju Region 3) dan bagian 6 (Badai Tropis). Tampilan per cuaca adalah usulan.

## 1. Arsitektur & Variabel Utama

Singleton di scene `IslandPhase`. `WeatherManager` tidak lagi mengacak cuaca dan tidak memegang sumber kebenaran. Enum cuaca memakai `WeatherType` dari `GLOSSARY.md`: `Clear`, `Rain`, `Fog`, `Storm`, `Snow`, dan `Blizzard`.

**Batas tanggung jawab:**

| Pekerjaan | Pemilik |
|---|---|
| Cuaca tiap region, override bencana, dan `WeatherChangedEvent` | `RegionalWeather` (#115) |
| Fog, cahaya, skybox, dan post-processing | `EnvironmentDirector` (#92) |
| Partikel suasana region (`Fx_AshLight`, `Fx_Spores`, `Fx_SnowDust`, `Fx_Mist`) | `EnvironmentDirector` (#92), lewat `ambientParticleTag` |
| Kecepatan pemain dan pengali deteksi musuh | `RegionalWeather`, ke `IslandPlayerController` dan `AISensoryHub` |
| `Warmth`, frostbite, dan padamnya obor | `WarmthSystem` dan `TorchController` (#65) |
| Lampu tak beratap padam saat badai | `LightSourceSystem` (#98) |
| Suara latar region yang diredam saat cuaca buruk | `AmbientAudio` (#52) |
| **Partikel dan suara cuaca yang menempel pada kamera** | **`WeatherManager`** |

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `WeatherManager` | `null` | Singleton |
| `followTarget` | `Transform` | Kamera utama | Tempat partikel menempel |
| `rainParticles`, `stormLeavesParticles` | `ParticleSystem` | Dari scene, anak `followTarget` | Partikel hujan dan daun badai. Selalu ada, jadi tidak memakai pool |
| `snowPoolTag` | `string` | `Fx_Snow` | Partikel salju dari `ObjectPooler` (usulan nama tag) |
| `blizzardPoolTag` | `string` | `Fx_Blizzard` | Partikel badai salju dari `ObjectPooler`, sesuai `RegionalWeather.md` |
| `rainLoop`, `stormLoop`, `windLightLoop`, `windStrongLoop` | `AudioClip` | Dari Inspector | Klip loop audio cuaca |
| `deckA`, `deckB` | `AudioSource` | Dari prefab | Dua sumber loop untuk crossfade. `loop = true`, `spatialBlend = 0`, keluar ke grup mixer `Ambient` |
| `shown` | `WeatherType` | `Clear` | Yang sedang tampil. Bukan sumber kebenaran |
| `isInside` | `bool` | `false` | Hasil panggilan `Present` terakhir |
| `isUnderwater` | `bool` | `false` | Dari `SwimStateChangedEvent.isUnderwater` |
| `channels` | `WeatherChannel[4]` | Bobot 0 | Hujan, daun, salju, dan badai salju |
| `blendSeconds` | `float` | 6 | Lama transisi yang sedang berjalan |

```csharp
// Satu saluran partikel. Disimpan dalam array tetap, jadi tanpa alokasi saat crossfade
struct WeatherChannel
{
    public ParticleSystem system;   // null untuk saluran pool yang belum diambil
    public string poolTag;          // Kosong untuk partikel scene
    public float baseRate;          // rateOverTime asli dari prefab
    public float weight, target;    // 0 sampai 1
}
```

Grup `Ambient` dipilih supaya audio cuaca ikut slider volume suasana, peredaman tidur, dan low-pass bawah air yang sudah ada di `AmbientAudio.md`. Tidak perlu parameter mixer baru.

### Tabel tampilan per cuaca

Bobot adalah target emisi relatif terhadap `baseRate` prefab.

| Cuaca | Hujan | Daun | Salju | Badai salju | Audio |
|---|---|---|---|---|---|
| `Clear` | 0 | 0 | 0 | 0 | Diam |
| `Rain` | 1 | 0 | 0 | 0 | `rainLoop` |
| `Fog` | 0 | 0 | 0 | 0 | `windLightLoop`, volume 0,4 |
| `Storm` | 1,5 | 1 | 0 | 0 | `stormLoop` |
| `Snow` | 0 | 0 | 1 | 0 | `windLightLoop` |
| `Blizzard` | 0 | 0 | 0,5 | 1 | `windStrongLoop` |

**Angka penyetelan:**

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.weather.presentBlendSeconds` | 6 | Sudah ada (#115). Lama crossfade, dikirim `RegionalWeather` |
| `GameBalanceConfig.weather.audioVolume` | 0,8 | Volume dasar audio cuaca (usulan baru, disetujui developer) |
| `GameBalanceConfig.weather.insideVolumeMultiplier` | 0,3 | Pengali volume di dalam `InteriorZone` atau bangunan (usulan baru, disetujui developer) |
| `GameBalanceConfig.weather.insideBlendSeconds` | 1,5 | Transisi masuk dan keluar ruangan, sama dengan `environment.interiorBlendSeconds` (usulan baru, disetujui developer) |

Pengali kualitas partikel memakai `VFXManager.densityMultiplier` (#15), supaya pengaturan grafis rendah juga mengurangi hujan.

**Dihapus:** `RandomizeWeather`, `weatherTimer`, `weatherChangeInterval`, `currentWeather`, `directionalLight`, semua penulisan `RenderSettings.fog` dan intensitas cahaya, enum `WeatherType` lokal, `SetWeather`, dan pemutaran `rainSFX` lewat `AudioManager.PlaySFX`. `PlaySFX` hanya untuk suara sekali putar, jadi audio loop memakai `deckA` dan `deckB`.

## 2. Detail Logika & Fungsi

### Menerima perintah tampil

**`public void Present(WeatherType w, float blendSeconds, bool isPlayerInside)`**, dipanggil `RegionalWeather.ApplyActive`:

1. Kalau `w == shown` dan `isPlayerInside == isInside`, keluar. Panggilan ulang dengan nilai sama tidak memulai crossfade baru.
2. Simpan `weatherChanged = w != shown`, lalu `shown = w` dan `isInside = isPlayerInside`.
3. Kalau `!weatherChanged` (hanya status ruangan yang berubah), `this.blendSeconds = insideBlendSeconds`. Kalau tidak, `this.blendSeconds = Max(blendSeconds, 0,01)`, supaya tidak ada pembagian dengan nol.
4. Isi `target` tiap saluran dari tabel tampilan. Kalau `isInside` atau `isUnderwater`, semua `target = 0`.
5. Untuk saluran pool dengan `target > 0` dan `system == null`, panggil `AcquirePooled(i)`.
6. Panggil `SwitchAudio(ClipFor(w), VolumeFor(w))`.

**`private void AcquirePooled(int i)`**:

1. `go = ObjectPooler.Instance.SpawnFromPool(poolTag, followTarget.position, followTarget.rotation)`. Kalau `null`, keluar tanpa error. Cuaca tetap jalan tanpa partikel itu.
2. Jadikan anak `followTarget`, dengan posisi lokal nol.
3. `system = go.GetComponent<ParticleSystem>()`, `baseRate` dibaca dari emisi prefab, dan `weight = 0`.

### Crossfade partikel

**`void Update()`**:

1. `step = Time.deltaTime / blendSeconds`.
2. Untuk tiap saluran: `weight = MoveTowards(weight, target, step)`.
3. Tulis `emission.rateOverTimeMultiplier = baseRate * weight * VFXManager.Instance.densityMultiplier`.
4. Kalau saluran pool punya `weight == 0` dan `target == 0`, panggil `ReleasePooled(i)`.
5. Kalau semua saluran sudah mencapai target dan audio selesai berpindah, `enabled = false` sampai `Present` berikutnya. Komponen tidak berjalan setiap frame saat cuaca tetap.

Partikel yang sudah jatuh dibiarkan habis sendiri. Yang diturunkan hanya emisi, jadi hujan tidak hilang mendadak.

**`private void ReleasePooled(int i)`**: lepaskan dari `followTarget`, `ObjectPooler.Instance.ReturnToPool(poolTag, go)`, lalu `system = null`.

### Crossfade audio

**`private void SwitchAudio(AudioClip clip, float volume)`**:

1. Pilih deck yang sedang diam atau sedang memudar sebagai deck baru, lalu deck lain sebagai deck lama.
2. Kalau `clip` sama dengan klip deck lama, cukup ubah target volume deck lama. Tidak perlu memulai ulang loop.
3. Kalau berbeda: deck baru memutar `clip` dari volume 0, dan deck lama menuju 0.
4. Target volume = `volume * audioVolume`, dikali `insideVolumeMultiplier` kalau `isInside`.
5. Volume digeser di `Update` dengan langkah yang sama dengan partikel. Deck yang mencapai 0 dipanggil `Stop()`, dan klipnya dikosongkan.

Kalau `Present` dipanggil lagi di tengah crossfade, deck yang sedang memudar dipakai ulang. Hasilnya paling banyak dua loop berbunyi bersamaan.

**`ClipFor(w)` dan `VolumeFor(w)`** membaca kolom audio di tabel tampilan. `Clear` mengembalikan `null` dan volume 0.

### Di bawah air

**`HandleSwim(SwimStateChangedEvent e)`**: simpan `isUnderwater = e.isUnderwater`, lalu hitung ulang `target` partikel seperti langkah 4 `Present`. Audio tidak diubah, karena low-pass bawah air sudah diterapkan pada grup `Ambient` oleh `AmbientAudio` (usulan: partikel hujan tidak tampil di bawah air).

### Membersihkan

**`void OnDisable()`** dan **`void OnDestroy()`**: panggil `ReleasePooled` untuk semua saluran pool, lalu hentikan kedua deck. Partikel pool yang masih menjadi anak kamera akan ikut hancur saat scene `IslandPhase` dilepas, sehingga `ObjectPooler` menyimpan referensi rusak. Langkah ini mencegahnya.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada. `WeatherChangedEvent` milik `RegionalWeather`.

**Event yang didengar:** `SwimStateChangedEvent` (sticky). Didaftar di `OnEnable`, membaca `Last`, dan dilepas di `OnDisable`, sesuai pola #107.

| Sistem | Hubungan |
|---|---|
| `RegionalWeather` (#115) | Satu-satunya pemanggil `Present` |
| `ObjectPooler` | `Fx_Snow` dan `Fx_Blizzard` |
| `VFXManager` (#15) | Membaca `densityMultiplier` |
| `AudioManager`, `AmbientAudio` (#52) | Grup mixer `Ambient`, jeda, tidur, dan low-pass bawah air |
| `SwimmingController` (#13) | `SwimStateChangedEvent.isUnderwater` |
| `EnvironmentDirector` (#92) | Pemilik fog, cahaya, dan partikel region. Tidak saling memanggil |
| `WarmthSystem`, `TorchController` (#65) | Tidak dipanggil. Obor tetap menyala saat `Storm` dan `Blizzard` (keputusan developer). `WeatherManager` tidak memadamkan sumber cahaya apa pun |
| `LightSourceSystem` (#98) | Tidak dipanggil. Lampu tak beratap padam lewat aturan #98 sendiri |

## 4. Kasus Khusus

- **Pool dipakai ulang:** `Fx_Snow` dan `Fx_Blizzard` mengosongkan partikel, mengembalikan emisi ke nilai prefab, dan memanggil `Play(true)` di `OnSpawned`. `OnReturned` memanggil `Stop(true, StopEmittingAndClear)`. `WeatherManager` menyetel `weight = 0` setiap kali mengambil objek, jadi bobot lama tidak terbawa.
- **Pool habis:** `SpawnFromPool` mengembalikan `null`. Saluran itu tetap kosong, dan audio tetap berjalan.
- **Game dijeda:** crossfade memakai `Time.deltaTime`, jadi berhenti. Partikel berhenti bersama `Time.timeScale`. Audio dijeda lewat `AudioListener.pause` (`AudioManager.md`).
- **Tidur (#67):** `RegionalWeather` bisa memanggil `Present` berkali-kali dengan cepat. Crossfade tidak dipercepat, dan deck dipakai ulang, jadi tidak ada lebih dari dua loop.
- **Masuk `InteriorZone` atau bangunan:** `RegionalWeather` memanggil `Present` lagi dengan `isPlayerInside = true`. Emisi turun dalam `insideBlendSeconds`, dan volume turun ke 30 persen, sehingga hujan masih terdengar dari dalam.
- **Badai saat bencana:** override `TropicalStorm` datang lewat `RegionalWeather` sebagai `Storm` atau `Blizzard`. `WeatherManager` tidak tahu bedanya.
- **Kamera kematian (#53):** partikel tetap menempel pada `followTarget`. Saat run berakhir, scene dilepas dan `OnDisable` mengembalikan pool.
- **Save dan load:** tidak ada data tersimpan, karena cuaca dihitung dari seed. Setelah load, `RegionalWeather` memanggil `Present` dengan nilai `blendSeconds` normal. Usulan: kalau dipanggil saat layar masih hitam setelah load, `RegionalWeather` boleh mengirim `blendSeconds = 0,01` supaya partikel langsung sesuai.
- **Permadeath:** objek hilang bersama scene `IslandPhase`. Pool sudah dikembalikan di `OnDisable`, dan `ObjectPooler.ResetForNewRun` memastikan tidak ada sisa.
- **Fase 1:** `WeatherManager` tidak ada di scene `ShipPhase`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** cuaca per region dari seed, efek cuaca, `presentBlendSeconds`, dan `Fx_Blizzard` (`RegionalWeather.md`), serta fog dan cahaya (`EnvironmentProfile.md`). Obor tetap menyala saat `Storm` dan `Blizzard` (keputusan developer, `WarmthSystem.md`).

**Usulan yang disetujui developer sebagai nilai awal (#85):**

1. **Tampilan per cuaca di tabel:** rekomendasi pakai sebagai nilai awal, lalu setel saat playtest.

**Usulan baru (disetujui developer sebagai nilai awal):**

2. **Audio loop dua deck di grup `Ambient`:** menggantikan `PlaySFX`, tanpa parameter mixer baru.
3. **Volume di dalam ruangan 30 persen:** hujan tetap terdengar dari dalam base.
4. **Partikel hujan dan salju mati di bawah air.**
5. **Nama tag `Fx_Snow`:** dibedakan dari `Fx_SnowDust` milik suasana Region 3.
6. **Tiga angka baru di `GameBalanceConfig.weather`:** `audioVolume`, `insideVolumeMultiplier`, dan `insideBlendSeconds`.

## 6. Skenario Tes

1. Panggil `Present(Rain, 6, false)`. **Hasil:** hujan masuk bertahap selama 6 detik.
2. Lanjutkan dengan `Present(Clear, 6, false)`. **Hasil:** hujan dan audio memudar, dan `Update` berhenti setelah selesai.
3. Panggil `Present(Blizzard, 6, true)`. **Hasil:** partikel mati, dan audio teredam ke 30 persen.
4. Cari `RenderSettings`, `directionalLight`, dan `Random` di skrip. **Hasil:** tidak ada.
5. Biarkan 3 hari game berjalan. **Hasil:** `WeatherManager` tidak pernah memilih cuaca sendiri.
6. Muat save saat `Storm`. **Hasil:** hujan dan daun tampil, dan `stormLoop` berbunyi.
7. Panggil `Present(Rain, 6, false)` dua kali berturut-turut. **Hasil:** crossfade tidak mulai ulang, dan loop tidak terputus.
8. Panggil `Present` dengan `Rain`, `Snow`, lalu `Blizzard` dalam 2 detik. **Hasil:** paling banyak dua `AudioSource` berbunyi, dan tidak ada error.
9. Berganti dari `Snow` ke `Clear`, lalu tunggu crossfade selesai. **Hasil:** `Fx_Snow` kembali ke pool, dan jumlah objek aktif pool kembali seperti semula.
10. Atur kualitas grafis ke rendah saat `Rain`. **Hasil:** laju hujan turun ke 50 persen.
11. Menyelam saat `Rain`. **Hasil:** partikel hujan hilang, dan audio terdengar teredam.
12. Tahan obor saat `Storm` di Region 1, lalu saat `Blizzard` di Region 3. **Hasil:** obor tetap menyala. `WeatherManager` tidak memanggil sistem obor.
13. Mati saat `Blizzard`, lalu mulai run baru. **Hasil:** tidak ada peringatan pool tersisa dari `ObjectPooler.ResetForNewRun`.
14. Jeda game selama crossfade. **Hasil:** emisi berhenti di tengah, lalu lanjut setelah jeda selesai.
