# Blueprint: RegionalWeather.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/RegionalWeather.md`
**Deskripsi Utama:** Menentukan cuaca setiap region secara terpisah dan dari seed, termasuk `Snow` dan `Blizzard` di Region 3. `WeatherManager` hanya menampilkan cuaca di tempat pemain.
**Issue:** #115
**Rujukan GDD:** Bagian 5 (Region 3 bersalju, dan ancaman cuaca di perjalanan) dan bagian 6 (Badai Tropis). Tabel cuaca adalah usulan.

## 1. Arsitektur & Variabel Utama

`RegionalWeather` adalah manager di scene `IslandPhase`. Cuaca tiap region dihitung sebagai fungsi murni dari seed, region, dan waktu. Karena itu cuaca region yang tidak dimuat tetap benar, dan tidak perlu disimpan.

```csharp
public enum WeatherType { Clear, Rain, Fog, Storm, Snow, Blizzard }   // Enum resmi GLOSSARY

[CreateAssetMenu(menuName = "Survival/Regional Weather Table")]
public class RegionWeatherTable : ScriptableObject
{
    public RegionType region;
    public WeatherWeight[] weights;    // WeatherType dan bobot
}

public struct WeatherEffects
{
    public float speedMultiplier, aiSightMultiplier, aiHearingMultiplier, warmthLossPerSec;
}
```

| Variabel | Fungsi |
|---|---|
| `current[4]` | Hasil tiap `RegionType` pada slot saat ini |
| `currentSlot` | `floor(TotalGameHours / weatherIntervalGameHours)` |
| `overrides` | Daftar `WeatherOverride` dari bencana |
| `activeRegion` | Dari `RegionChangedEvent` |
| `isPlayerInside` | Dari `InteriorZoneChangedEvent` |

### Tabel cuaca dan peluang

| Region | `Clear` | `Rain` | `Fog` | `Storm` | `Snow` | `Blizzard` | Alasan |
|---|---|---|---|---|---|---|---|
| 1 `Volcano` | 50 | 25 | 15 | 10 | 0 | 0 | Sama dengan peluang `WeatherManager` lama |
| 2 `Amazon` | 25 | 50 | 15 | 10 | 0 | 0 | Hutan tropis lebat |
| 3 `Snow` | 30 | 0 | 5 | 0 | 45 | 20 | Tidak ada hujan tropis di puncak es |
| 4 `Logistic` | 35 | 25 | 35 | 5 | 0 | 0 | Lembah gelap yang lembab |

### Efek per cuaca

Disimpan di `GameBalanceConfig.weather.effects`, satu baris per `WeatherType`.

| Cuaca | `speedMultiplier` | `aiSightMultiplier` | `aiHearingMultiplier` | `warmthLossPerSec` |
|---|---|---|---|---|
| `Clear` | 1 | 1 | 1 | 0 |
| `Rain` | 1 | 0,9 | 0,85 | 0,3 |
| `Fog` | 1 | 0,6 | 1 | 0 |
| `Storm` | 0,9 | 0,7 | 0,6 | 0,5 |
| `Snow` | 0,95 | 0,85 | 0,9 | 0,5 |
| `Blizzard` | 0,75 | 0,45 | 0,6 | 2 |

| Nilai config lain | Awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.weather.weatherIntervalGameHours` | 8 | Sudah ada di #84. Tiga slot per hari game |
| `GameBalanceConfig.weather.minSevereDay` | 2 | Hari pertama tanpa cuaca berat |
| `GameBalanceConfig.weather.presentBlendSeconds` | 6 | Transisi partikel dan audio |

## 2. Detail Logika & Fungsi

### Menghitung cuaca

**`private WeatherType DrawRaw(RegionType r, int slot)`**, sesuai `RunSeedManager.md`:

```csharp
var s = RunSeedManager.Instance.Fork(RandomStreamId.Weather, (int)r * 10000 + slot);
return s.PickWeighted(table[r].weights);
```

**`private WeatherType Scheduled(RegionType r, int slot)`**:

1. `w = DrawRaw(r, slot)`.
2. Kalau `w` berat (`Storm` atau `Blizzard`) dan hari untuk slot itu kurang dari `minSevereDay`, turunkan `Storm` ke `Rain` dan `Blizzard` ke `Snow`.
3. Kalau `w` berat dan `DrawRaw(r, slot - 1)` juga berat, turunkan juga, supaya cuaca berat tidak berlangsung lebih dari satu slot (8 jam game, sekitar 5 menit nyata).

Langkah 3 memakai `DrawRaw` dan bukan `Scheduled`, jadi tidak ada rekursi dan hasilnya tetap murni.

**`public WeatherType GetWeather(RegionType r)`** mengembalikan override terbaru yang mencakup `r`. Kalau tidak ada, kembalikan `current[r]`. Fungsi ini sah untuk semua region, dimuat atau tidak.

**`public WeatherEffects GetEffects(RegionType r)`** membaca baris efek. Kalau `r == activeRegion` dan `isPlayerInside`, efek pada pemain menjadi netral (`speedMultiplier = 1`, `warmthLossPerSec = 0`). Efek AI tetap.

### Pergantian slot

Interval dihitung dengan jam game, jadi berhenti saat jeda dan ikut cepat saat tidur, sesuai #84.

**`HandleHour(HourChangedEvent e)`**:

1. Hitung `slot`. Kalau sama dengan `currentSlot`, selesai.
2. Untuk tiap region, hitung `Scheduled`. Kalau hasil akhir (`GetWeather`) berubah, terbitkan `WeatherChangedEvent { region, weather }`.
3. Region aktif diterbitkan paling akhir, supaya `Last` berisi region pemain.
4. Panggil `ApplyActive()`.

Pendengar yang terlambat aktif untuk region tertentu harus memanggil `GetWeather(region)`, karena `Last` hanya menyimpan satu region.

**`private void ApplyActive()`**:

1. `w = GetWeather(activeRegion)`, dan `fx = GetEffects(activeRegion)`.
2. `WeatherManager.Instance.Present(w, presentBlendSeconds, isPlayerInside)`.
3. `IslandPlayerController.SetSpeedModifier(SpeedModifierId.Weather, fx.speedMultiplier)`.
4. `AISensoryHub.SetWeatherModifiers(fx.aiSightMultiplier, fx.aiHearingMultiplier)`.

### Transisi saat berpindah region

Cuaca tiap region tetap jalan terus, tetapi hanya region aktif yang tampil.

**`HandleRegionChanged(RegionChangedEvent e)`** mengisi `activeRegion = e.to`, lalu memanggil `ApplyActive()`. `WeatherManager` melakukan crossfade emisi partikel dan audio selama `presentBlendSeconds`. `EnvironmentDirector` (#92) melakukan transisi fog dan cahayanya sendiri.

Contoh: pemain berjalan dari Region 4 yang hujan ke Region 3 yang `Blizzard`. Hujan memudar dan salju menebal dalam sekitar 6 detik, tanpa ada hujan di puncak es.

### Blizzard

| Efek | Nilai | Pelaksana |
|---|---|---|
| Jarak pandang | Sekitar 25 m | `EnvironmentDirector`, tambahan fog 0,07 |
| Kecepatan | x0,75 | `SetSpeedModifier(Weather)` |
| `Warmth` | -2 per detik | `WarmthSystem` (#65) membaca `GetEffects(region).warmthLossPerSec` |
| Partikel dan angin | `Fx_Blizzard` menempel pada kamera | `WeatherManager`, dari `ObjectPooler` |
| Deteksi musuh | Pandang x0,45, dengar x0,6 | `AISensory` (#49) |

Pengali kecepatan ini digabung dengan `heavyItemSpeedMultiplier`. Pemain yang membawa suku cadang saat `Blizzard` bergerak 5 x 0,5 x 0,75 = 1,875 m/s. Area dingin 150 meter menjadi sekitar 80 detik, bukan 60 detik, sehingga menunggu badai reda menjadi pilihan yang berarti.

### Deteksi musuh

`AISensory` memakai rumus yang sudah ada, ditambah dua pengali baru:

- `range = sightRange * data.sightMultiplier * hub.PlayerVisibility * hub.WeatherSight`.
- `r = e.radius * data.hearingMultiplier * hub.WeatherHearing`.

Pengali berlaku untuk musuh di region aktif, karena musuh hanya aktif di sekitar pemain. Blood Moon tetap mengabaikan stealth (#49).

### Sistem yang memakai cuaca region

| Sistem | Cara membaca | Hasil |
|---|---|---|
| `RainwaterCollector` (#43) | `GetWeather(homeRegion)` | `Rain` dan `Storm` mengisi, `Snow` dan `Blizzard` tidak |
| `CampfireManager` (#44) | `GetWeather(homeRegion)` | Mengikuti aturan #44 |
| `FishingSystem` (#55) | `GetWeather(lureRegion)` | Hujan mempercepat gigitan |
| `DebrisRetrieval` (#106) | `GetWeather(region)` | Jeda kemunculan |
| `TideManager` (#91) | `WeatherChangedEvent` untuk `Volcano` | Surge air |

Semua tetap benar walaupun pemain berada di region lain.

### Override dari bencana

```csharp
public struct WeatherOverride { public byte regionMask; public WeatherType weather; public WeatherType snowWeather; }
public WeatherOverrideHandle PushOverride(in WeatherOverride o);
public void PopOverride(WeatherOverrideHandle h);
```

- `snowWeather` berlaku untuk Region 3. Badai Tropis (#70) menjadi `Storm` di Region 1, 2, dan 4, serta `Blizzard` di Region 3.
- `PushOverride` dan `PopOverride` menerbitkan `WeatherChangedEvent` untuk region yang hasilnya berubah, lalu memanggil `ApplyActive()`.
- Cuaca terjadwal tetap dihitung di belakang, jadi setelah override dilepas cuaca kembali ke jadwal.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `WeatherChangedEvent` (sticky).

**Event yang didengar:** `HourChangedEvent`, `RegionChangedEvent`, dan `InteriorZoneChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `RunSeedManager` (#89) | `Fork(Weather, key)` |
| `DayNightDirector` | `TotalGameHours` |
| `WeatherManager` | Dipanggil `Present(...)`. Tidak lagi mengacak cuaca |
| `EnvironmentDirector` (#92) | Mengatur gabungan fog dan cahaya |
| `DisasterManager` (#70) | `PushOverride`, `PopOverride` |
| `WarmthSystem` (#65) | Membaca `warmthLossPerSec`. Efek suhu milik #65 |
| `AISensory` (#49) | `SetWeatherModifiers` |
| `ZombieLair` (#112) | `InteriorZoneChangedEvent` meredam suara dan efek |

## 4. Kasus Khusus

- **Pool:** `Fx_Blizzard` dan partikel cuaca lain mengosongkan partikel dan mengembalikan emisi di `OnSpawned`, lalu kembali ke pool saat cuaca berganti.
- **Game dijeda:** jam tidak maju, jadi slot tidak berganti. Partikel berhenti bersama `Time.timeScale`.
- **Permadeath dan save:** cuaca tidak disimpan, karena cukup dari seed dan `TotalGameHours`. Override dipasang lagi oleh `DisasterManager` saat `RestoreState`. Setelah load, `HourChangedEvent` dari `world_clock` membuat `RegionalWeather` menghitung ulang. Seed baru pada run baru memberi cuaca baru.
- **Tidur:** beberapa slot bisa lewat dengan cepat. Tiap perubahan tetap dihitung, tetapi `presentBlendSeconds` tidak dipercepat.
- **Region dilepas:** `GetWeather` tidak membaca scene region.
- **`setday` di `DevConsole`:** hasil cuaca sama untuk slot yang sama, karena fungsinya murni.
- **Slot negatif:** `slot - 1` pada slot 0 dilewati.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `WeatherType` di GLOSSARY, rumus `Fork` di #89, dan interval jam game di #84.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Cuaca dihitung per region:** semua region selalu punya cuaca, walaupun hanya region aktif yang ditampilkan. Rekomendasi: pakai, karena penampung hujan butuh cuaca di tempatnya, dan fungsi murni tidak menambah beban.
2. **Tabel peluang di atas.** Rekomendasi: pakai sebagai nilai awal.
3. **Cuaca berat tidak berurutan, dan tidak ada pada hari pertama.** Rekomendasi: pakai, karena game ini permadeath.
4. **Efek Blizzard:** x0,75 kecepatan, -2 `Warmth` per detik, dan jarak pandang sekitar 25 m. Skala `Warmth` menunggu #65.
5. **Cuaca mengurangi deteksi musuh.** Rekomendasi: pakai, supaya badai bisa dimanfaatkan untuk lewat diam-diam.
6. **Efek pada pemain hilang di dalam `InteriorZone`.** Bangunan pemain mengikuti #65.

## 6. Skenario Tes

1. Catat `GetWeather` semua region selama satu hari game, lalu ulangi dengan seed sama. **Hasil:** sama persis.
2. Catat 300 slot Region 3. **Hasil:** tidak pernah `Rain` atau `Storm`, dan `Blizzard` tidak berurutan.
3. Catat hari pertama semua region. **Hasil:** tidak ada `Storm` atau `Blizzard`.
4. Hari ke-5, pakai `DevConsole` agar Region 4 `Rain` dan Region 3 `Blizzard`. Berjalan melewati seam. **Hasil:** hujan memudar, salju menebal, dan kecepatan turun ke x0,75.
5. Pasang penampung hujan di Region 1 yang `Rain`, lalu berdiri di Region 3 yang `Clear`. **Hasil:** penampung tetap terisi.
6. Berdiri 20 m dari zombie saat `Clear`, lalu ubah ke `Blizzard`. **Hasil:** zombie tidak melihat pemain, karena jarak pandang turun ke sekitar 10 m.
7. Tembak saat `Storm`. **Hasil:** radius dengar turun x0,6.
8. Saat `Blizzard`, masuk `InteriorZone`. **Hasil:** partikel hilang, suara teredam, dan kecepatan kembali normal.
9. Picu `disaster TropicalStorm`. **Hasil:** tiga region menjadi `Storm`, Region 3 `Blizzard`, lalu kembali ke jadwal setelah selesai.
10. Jeda selama 2 menit nyata. **Hasil:** cuaca tidak berganti.
11. Simpan saat `Blizzard`, lalu load. **Hasil:** cuaca sama.
