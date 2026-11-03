# Blueprint: WeatherManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/WeatherManager.md`
**Deskripsi Utama:** Menampilkan cuaca di tempat pemain lewat partikel dan audio. Penentuan cuaca dan efeknya milik `RegionalWeather` (#115).
**Issue:** #115 (melengkapi #65, #89, #92, dan #101)
**Rujukan GDD:** Bagian 5 (salju Region 3) dan bagian 6 (Badai Tropis). Tampilan per cuaca adalah usulan.

## 1. Arsitektur & Variabel Utama

Singleton di scene `IslandPhase`. `WeatherManager` tidak lagi mengacak cuaca dan tidak memegang sumber kebenaran. Enum cuaca memakai `WeatherType` dari `GLOSSARY.md`: `Clear`, `Rain`, `Fog`, `Storm`, `Snow`, dan `Blizzard`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `WeatherManager` | `null` | Singleton |
| `rainParticles`, `stormLeavesParticles` | `ParticleSystem` | Dari scene | Partikel menempel pada kamera |
| `rainSFX`, `stormSFX` | `AudioClip` | Dari Inspector | Audio cuaca |
| `shown` | `WeatherType` | `Clear` | Yang sedang tampil. Bukan sumber kebenaran |

**Dihapus:** `RandomizeWeather`, `weatherTimer`, `weatherChangeInterval`, `currentWeather`, `directionalLight`, semua penulisan `RenderSettings.fog` dan intensitas cahaya, enum `WeatherType` lokal, dan `SetWeather`.

## 2. Detail Logika & Fungsi

**`public void Present(WeatherType w, float blendSeconds, bool isPlayerInside)`**, dipanggil `RegionalWeather.ApplyActive`:

1. `shown = w`.
2. Crossfade emisi partikel dan audio selama `blendSeconds`.
3. Kalau `isPlayerInside`, matikan emisi partikel dan redam audio.

| Cuaca | Partikel | Audio |
|---|---|---|
| `Clear` | Mati | Mati |
| `Rain` | `rainParticles` | `rainSFX` lewat `AudioManager` |
| `Fog` | Mati | Mati |
| `Storm` | `rainParticles` dan `stormLeavesParticles` | `stormSFX` |
| `Snow` | Partikel salju dari pool | Angin ringan |
| `Blizzard` | `Fx_Blizzard` dari `ObjectPooler` | Angin kencang |

**Yang pindah ke sistem lain:**

| Efek | Pemilik baru |
|---|---|
| Pengacakan dari seed, `Fork(RandomStreamId.Weather, (int)region * 10000 + slot)` | `RegionalWeather` |
| Fog dan cahaya | `EnvironmentDirector` (#92) |
| Penurunan `Warmth` | `WarmthSystem` (#65), membaca `WeatherChangedEvent` |
| Perlambatan badai, `SetSpeedModifier(SpeedModifierId.Weather, nilai)` | `RegionalWeather`, ke `IslandPlayerController` (#101) |

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim dan didengar:** tidak ada. `WeatherChangedEvent` milik `RegionalWeather`.

| Sistem | Hubungan |
|---|---|
| `RegionalWeather` (#115) | Memanggil `Present` |
| `AudioManager` | Audio cuaca |
| `ObjectPooler` | `Fx_Blizzard` dan partikel salju |
| `EnvironmentDirector` (#92) | Pemilik fog dan cahaya. Tidak saling memanggil |

## 4. Kasus Khusus

- **Pool dipakai ulang:** `Fx_Blizzard` mengosongkan partikel dan mengembalikan emisi di `OnSpawned`.
- **Game dijeda:** partikel dan crossfade memakai `Time.deltaTime`, dan audio dijeda `AudioManager`.
- **Masuk `InteriorZone` atau bangunan:** `RegionalWeather` memanggil `Present` lagi dengan `isPlayerInside = true`.
- **Save dan load:** tidak ada data tersimpan, karena cuaca dihitung dari seed. `RegionalWeather` memanggil `Present` setelah load.
- **Permadeath:** hilang bersama scene `IslandPhase`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** cuaca per region dari seed, efek cuaca, dan `Fx_Blizzard` (`RegionalWeather.md`), serta fog dan cahaya (`EnvironmentProfile.md`).

**Usulan yang menunggu developer (#85):**

1. **Tampilan per cuaca di tabel:** rekomendasi pakai sebagai nilai awal, lalu setel saat playtest.

## 6. Skenario Tes

1. Panggil `Present(Rain, 6, false)`. **Hasil:** hujan masuk bertahap selama 6 detik.
2. Lanjutkan dengan `Present(Clear, 6, false)`. **Hasil:** hujan dan audio memudar.
3. Panggil `Present(Blizzard, 6, true)`. **Hasil:** partikel mati, dan audio teredam.
4. Cari `RenderSettings`, `directionalLight`, dan `Random` di skrip. **Hasil:** tidak ada.
5. Biarkan 3 hari game berjalan. **Hasil:** `WeatherManager` tidak pernah memilih cuaca sendiri.
6. Muat save saat `Storm`. **Hasil:** partikel langsung sesuai.
