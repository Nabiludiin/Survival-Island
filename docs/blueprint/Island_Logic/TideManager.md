# Blueprint: TideManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/TideManager.md`
**Deskripsi Utama:** Mengatur naik turunnya air laut menurut jam game, sebagai alasan pantai berbahaya untuk tidur, menyimpan barang, dan menunggu kargo.
**Issue:** #91
**Rujukan GDD:** Bagian 4 (air pasang di pasir pantai dan peringatan Diary Bab 1). Siklus, tinggi air, dan akibatnya adalah usulan, karena GDD tidak merincinya.

## 1. Arsitektur & Variabel Utama

`TideManager` adalah manager di scene `IslandPhase`, tanpa status tersimpan. Nilai air dihitung dari jam game, jadi selalu benar setelah tidur, jeda, dan load.

| Variabel | Tipe | Fungsi |
|---|---|---|
| `Level` | `float` | Tingkat air: 0 surut, 1 pasang malam, sampai 1,25 saat badai |
| `WaterY` | `float` | Tinggi air dunia saat ini |
| `IsHigh` / `IsLow` | `bool` | `Level >= highThreshold` / `Level <= lowThreshold` |
| `seaVisual` | `Transform` | Mesh laut, digerakkan halus |
| `seaTrigger` | `Transform` | Collider layer `Water` milik laut, digerakkan per langkah |
| `tideZone` | `Bounds` | Area pantai yang diperiksa |
| `safePoints` | `TideSafePoint[]` | Titik di atas garis air tertinggi, untuk barang yang tidak boleh hilang |
| `surge` | `float` | Tambahan level dari badai |

Hanya laut yang bergerak. Sungai dan danau di scene region tetap diam.

| Nilai config | Awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.tide.lowWaterY` / `highWaterY` | 0 m / 1,6 m | Tinggi air pada level 0 dan 1 |
| `GameBalanceConfig.tide.dayAmplitude` / `nightAmplitude` | 0,6 / 1 | Puncak pasang siang dan malam |
| `GameBalanceConfig.tide.highThreshold` / `lowThreshold` | 0,7 / 0,2 | Batas `IsHigh` dan `IsLow` |
| `GameBalanceConfig.tide.warnLevel` | 0,55 | Level peringatan pemain di pantai |
| `GameBalanceConfig.tide.stormSurgeLevel` | 0,25 | Tambahan level saat `Storm` |
| `GameBalanceConfig.tide.surgeRatePerGameHour` | 0,125 | Kecepatan surge naik dan turun |
| `GameBalanceConfig.tide.colliderStepMeters` | 0,15 m | Jarak gerak collider per langkah |
| `GameBalanceConfig.tide.wadeDepth` | 0,35 m | Kedalaman mulai terasa berat |
| `GameBalanceConfig.tide.wadeStaminaPerSec` | 2 | Stamina hilang saat mengarungi air |
| `GameBalanceConfig.tide.highAreaCost` | 20 | Biaya Area NavMesh `Tidal` saat `IsHigh` |

## 2. Detail Logika & Fungsi

### Rumus siklus

Pasang terjadi dua kali sehari, pukul 00:00 dan 12:00. Surut pukul 06:00 dan 18:00. Pasang malam lebih tinggi, supaya tidur di pantai memang berbahaya, seperti peringatan Diary Bab 1.

```csharp
float BaseLevel(float h)   // h = jam 0 sampai 24
{
    float amp = Mathf.Lerp(dayAmplitude, nightAmplitude, 0.5f + 0.5f * Mathf.Cos(2f * Mathf.PI * h / 24f));
    return amp * (0.5f + 0.5f * Mathf.Cos(2f * Mathf.PI * h / 12f));
}
```

| Jam | 00:00 | 02:10 | 06:00 | 12:00 | 18:00 | 21:50 |
|---|---|---|---|---|---|---|
| `Level` tanpa badai | 1,0 | 0,7, `IsHigh` selesai | 0 | 0,6 | 0 | 0,7, `IsHigh` mulai |

Pasang siang tampak nyata, tetapi hanya menjadi `IsHigh` kalau ada badai (0,6 + 0,25). Dengan satu hari game 900 detik, pasang malam `IsHigh` berlangsung sekitar 2,7 menit nyata.

### Fungsi utama

**`void Update()`**, setiap 0,25 detik `Time.deltaTime`:

1. `h = DayNightDirector.Instance.currentTimeOfDay`.
2. `surge = Mathf.MoveTowards(surge, surgeTarget, surgeRatePerGameHour * jamBerlalu)`.
3. `Level = BaseLevel(h) + surge`, lalu `WaterY = lowWaterY + Level * (highWaterY - lowWaterY)`.
4. Geser `seaVisual` setiap frame ke `WaterY` secara halus.
5. Kalau `|WaterY - colliderY| >= colliderStepMeters`, panggil `StepCollider()`.
6. Terbitkan `TideChangedEvent { level, isHigh }` kalau `Level` berubah minimal 0,05 atau `IsHigh` berubah.
7. Panggil `TickPlayer()`.

**`void StepCollider()`** memindahkan `seaTrigger`, mengisi `Shader.SetGlobalFloat("_TideWaterY", WaterY)`, lalu memanggil `WorldItemManager.Instance.SweepBelow(tideZone, WaterY, safePoints)`.

**`void TickPlayer()`**, hanya kalau pemain berada di `tideZone`:

1. `SurfaceDetection.Instance.TryGetTerrainSurfaceAtXZ(posisi, out s)`, lalu `depth = WaterY - s.groundHeight`.
2. Kalau `depth >= wadeDepth` dan `SwimmingController.Mode != WaterMode.Swimming`: `SpendStamina(wadeStaminaPerSec * dt, StaminaUse.Swim)`. `SwimmingController` (#13) menjadi satu-satunya pemilik `SpeedModifierId.Water`, jadi sistem ini tidak menulis pengali kecepatan.
3. Kalau `Level >= warnLevel`, air sedang naik, dan belum diperingatkan malam ini, kirim notifikasi `Air mulai pasang, jauhi pantai` (#22).

Pemain tidak didorong dan isi inventori tidak hanyut. Air yang lebih dalam diserahkan ke `SwimmingController`.

**API baca:**

- `float WaterYAtLevel(float level)`.
- `float MaxWaterY`, yaitu tinggi pada level `1 + stormSurgeLevel`.
- `bool IsPointSubmerged(Vector3 p)`, memakai `TryGetTerrainSurfaceAtXZ` dan membandingkan `groundHeight` dengan `WaterY`. Tanpa raycast.

### Area terendam dan #86

Area yang terendam dihitung dari tinggi Terrain, bukan collider buatan tangan. `TideZoneAuditWindow` di Editor mengambil sampel grid 2 meter di `tideZone` dan melaporkan:

- Titik dengan `groundHeight < MaxWaterY` yang `GetSandWeight` kurang dari 0,5. Titik ini harus dicat pasir, supaya semua tanah yang bisa terendam ditolak `BuildManager`.
- Titik seperti itu yang berada di luar `NavMeshModifierVolume` Area `Tidal`.
- `TideSafePoint` yang berada di bawah `MaxWaterY`.

### Barang di pantai

**`WorldItemManager.SweepBelow(Bounds zone, float waterY, TideSafePoint[] safe)`** memeriksa `WorldItem` dalam `zone` yang berada di bawah `waterY`:

| Barang | Akibat |
|---|---|
| `despawns == true` | Mengikuti aturan air yang sudah ada di #66: tenggelam, lalu dihapus setelah 1 jam game |
| `despawns == false` | Dipindah ke `TideSafePoint` terdekat, dan notifikasi `Barang terbawa ke atas pantai` sekali |
| `KeyItem` atau `isCoreItem` | Sama dengan `despawns == false`, karena tidak boleh hilang (#116) |

`HeavyItem` yang tertinggal di pantai dipindah `HeavyItemSystem` (#69) ke titik aman yang sama.

**Kargo pantai (#71)** tidak diperiksa di sini. `BeachSupplyManager` memilih titik di atas `WaterYAtLevel(crateMinLevel)`, lalu menghapus kargo saat puncak pasang malam.

### NPC dan musuh

NavMesh tidak pernah di-bake ulang. Saat `IsHigh` berubah, panggil `NavMesh.SetAreaCost(tidalArea, IsHigh ? highAreaCost : 1)`, sehingga:

- Agen yang menghitung jalur baru menghindari pantai yang terendam, tetapi tetap bisa lewat kalau itu satu-satunya jalan.
- NPC dan musuh tidak menerima damage air dan tidak tenggelam. Ini sejalan dengan Zombie Desa yang berendam di air (GDD bagian 5).
- `SpawnManager` (#62) menolak titik spawn yang `IsPointSubmerged`.

### Area saat surut

`LowTideSpot : IslandInteractable` adalah titik karang kecil di Region 1. `IsAvailable` bernilai `TideManager.Instance.Level <= lowThreshold && !harvestedToday`, sehingga bisa dipanen sekitar pukul 06:00 dan 18:00. Hasilnya 1 `Food_RawFish` lewat aliran `Gameplay` (#89).

Memancing di tebing karang tetap bisa kapan saja. Diary Bab 1 mengajarkan cara itu sebagai dasar, jadi tidak boleh tertutup oleh air.

### Tanda di dunia

- **Garis air:** busa di tepi mesh laut, dan pasir basah dari `_TideWaterY` di shader Terrain.
- **Suara:** `AmbientAudio` (#52) membaca `TideChangedEvent.level` untuk volume ombak.
- **Diary:** Bab 1 (#81) sudah memuat peringatan tidak tidur di pantai. Tambahkan kalimat tentang pasang malam.
- **Menu bangun:** pesan air pasang di `BuildMenu.md` tetap dipakai.

### Optimasi

| Hal | Cara |
|---|---|
| Mesh laut | Hanya `Transform` yang bergerak, sekitar setiap frame |
| Collider laut | Bergerak per 0,15 meter, sekitar sekali per 20 detik pada 96x waktu |
| NavMesh | Tidak di-bake ulang dan tanpa carving. Hanya `SetAreaCost` dua kali per pasang |
| Pemeriksaan barang | Hanya saat collider melangkah, dalam `tideZone` |

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `TideChangedEvent` (sticky).

**Event yang didengar:** `WeatherChangedEvent`, hanya untuk `RegionType.Volcano` karena pantai ada di selatan Region 1. `surgeTarget = stormSurgeLevel` kalau `Storm`, selain itu 0.

| Sistem | Hubungan |
|---|---|
| `DayNightDirector` | Sumber jam |
| `SurfaceDetection` (#86) | `TryGetTerrainSurfaceAtXZ`, `GetSandWeight` |
| `SleepSystem` (#67) | Mendengar `isHigh`, lalu membangunkan pemain dengan `TideRising` |
| `SwimmingController` (#13), `FishingSystem` (#55) | Membaca air lewat collider yang bergerak |
| `DisasterManager` (#70) | Badai Tropis memaksa `Storm` lewat `RegionalWeather`, jadi air ikut naik |
| `BeachSupplyManager` (#71) | Memakai `WaterYAtLevel` saat memeriksa titik |
| `DebrisRetrieval` (#106) | Membaca `isHigh` |
| `BuildManager`, `BuildMenu` (#79) | Tetap melarang pasir. `TideManager` tidak merusak bangunan |

## 4. Kasus Khusus

- **Pool:** `TideManager` tidak dipool. `WorldItem` mengikuti aturan reset di #66.
- **Game dijeda:** `Time.deltaTime` dan jam berhenti, jadi air diam.
- **Tidur:** jam dipercepat, sehingga air naik lebih cepat dengan rumus yang sama.
- **Permadeath dan save:** tidak ada data tersimpan. Setelah load, `Update` pertama menghitung nilai dari jam, `surge` langsung mengikuti cuaca, dan `TideChangedEvent` dikirim ulang.
- **`LowTideSpot`:** `harvestedToday` disimpan lewat `SaveIdTag` di `RegionStateStore`, dan diulang saat `DayChangedEvent` dengan `isReplay = false`. Kiriman ulang setelah load (`isReplay = true`) diabaikan, supaya keluar lalu lanjutkan tidak membuka panen lagi.
- **Region dilepas:** laut berada di `IslandPhase` dan tetap aktif. `LowTideSpot` ikut scene region.
- **Pemain tenggelam karena tidur:** `SleepSystem` membangunkan pemain lebih dulu. Stamina hilang 30, sesuai `sleep.beachTideStaminaLoss`.

## 5. Keputusan Desain

**Sudah mengikuti blueprint lain:** bangunan dilarang di pasir, sehingga air pasang tidak merusak bangunan (`BuildMenu.md`).

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Siklus:** dua kali pasang per hari, dengan puncak malam lebih tinggi. Rekomendasi: pakai, karena GDD mengaitkan pantai dengan bahaya tidur.
2. **Bahaya saat pasang:** stamina berkurang dan gerak melambat, tanpa dorongan dan tanpa item hanyut dari inventori. Rekomendasi: pakai, karena dorongan melawan `CharacterController` dan terasa tidak adil pada game permadeath.
3. **`WorldItem` dan kargo:** barang biasa hanyut, barang penting pindah ke titik aman, dan kargo hilang saat puncak pasang malam (#71).
4. **NPC dan musuh:** tanpa damage, hanya menghindari Area `Tidal`.
5. **Area saat surut:** ada `LowTideSpot` kecil tanpa barang wajib. Rekomendasi: pakai sebagai hadiah kecil. Memancing di tebing karang tidak ikut ditutup.
6. **Badai menaikkan air:** ya, tambahan level 0,25.

## 6. Skenario Tes

1. Jalankan `settide auto` dan cek jam 06:00, 12:00, dan 00:00. **Hasil:** `Level` sekitar 0, 0,6, dan 1.
2. Berdiri di pasir rendah pukul 22:00. **Hasil:** stamina turun, gerak melambat, dan ada satu peringatan.
3. Jatuhkan item biasa dan `Base_Pisau` di pasir rendah, lalu tunggu pasang. **Hasil:** item biasa tenggelam dan hilang setelah 1 jam game, sedangkan pisau pindah ke titik aman.
4. Tidur dekat pantai, lalu `settide high`. **Hasil:** bangun dengan `TideRising`, stamina berkurang 30.
5. Picu `Storm` Region 1 pada pukul 12:00. **Hasil:** `Level` naik ke sekitar 0,85, dan `IsHigh` bernilai `true`.
6. Saat `IsHigh`, suruh zombie mengejar pemain melintasi pantai. **Hasil:** memutar kalau ada jalur lain, dan tetap lewat kalau tidak ada.
7. Buka Profiler selama satu kali pasang. **Hasil:** tidak ada bake NavMesh, dan tidak ada GC dari `Update`.
8. Simpan pukul 23:00, lalu load. **Hasil:** tinggi air sama, tanpa lompatan besar.
9. Datangi `LowTideSpot` pukul 12:00, lalu 18:00. **Hasil:** prompt hanya muncul pada pukul 18:00.
10. Jalankan `TideZoneAuditWindow` dengan rumput di bawah `MaxWaterY`. **Hasil:** titik itu masuk laporan.
