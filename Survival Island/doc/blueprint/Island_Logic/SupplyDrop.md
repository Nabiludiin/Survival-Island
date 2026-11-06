# Blueprint: SupplyDropManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/SupplyDrop.md`
**Deskripsi Utama:** Event acak siang hari. Pesawat melintas, peti bantuan turun dengan parasut di lokasi acak, dan suar asap merah memancing pemain keluar dari base.
**Issue:** #54
**Rujukan GDD:** Tidak dipakai, diputuskan developer. Disimpan sebagai arsip. Sistem lain tidak boleh bergantung pada blueprint ini.
**Status:** Tidak dipakai, diputuskan developer. Disimpan sebagai arsip.

## 1. Arsitektur & Variabel Utama

`SupplyDropManager` adalah manager per scene di `IslandPhase`, dan memakai `ISaveable` dengan `SaveId` `world_supply`, yang sudah tercatat di `WorldStateSave.md`.

| Kelas | Jenis | Fungsi |
|---|---|---|
| `SupplyDropManager` | Manager | Jadwal, lokasi, dan urutan event |
| `SupplyPlane` | Prefab pool, `IPoolable` | Visual dan suara pesawat. Tanpa collider |
| `SupplyCrate` | Prefab pool, turunan `IslandInteractable` | Peti, parasut, dan suar. Layer `Interactable` |

```csharp
public enum SupplyDropState { Idle, Scheduled, PlaneInbound, Descending, Landed }
```

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `state` | `SupplyDropState` | `Idle` | Tahap event. Hanya satu event aktif |
| `scheduledHour` | `int` | -1 | Jam jatuh hari ini |
| `lastDropDay` | `int` | -100 | Untuk jeda antar-event |
| `targetPoint` | `Vector3` | Nol | Titik jatuh |
| `landedGameHour` | `float` | -1 | Jam game total saat mendarat |
| `activeCrate` | `SupplyCrate` | `null` | Peti di dunia. `null` kalau regionnya tidak dimuat |

### Angka (`GameBalanceConfig.supplyDrop`, bagian baru)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `enabled` | `false` | Saklar sampai usulan disetujui |
| `dailyChance` | 0,10 | Peluang per hari, dari contoh issue |
| `minDay` | 3 | Hari paling awal |
| `cooldownDays` | 3 | Jeda minimum antar-event |
| `dropHourMin` / `dropHourMax` | 8 / 14 | Rentang jam jatuh, supaya pemain sempat pulang sebelum 18:00 |
| `minDistanceFromPlayer` / `maxDistanceFromPlayer` | 120 / 260 | Cincin pencarian titik jatuh |
| `minDistanceFromBase` | 80 | Jarak minimum dari `BaseAnchor` (#78), supaya pemain harus keluar |
| `maxSlopeDegrees` | 25 | Kemiringan tanah maksimum |
| `clearanceRadius` | 4 | Area kosong di titik jatuh |
| `locationAttempts` | 20 | Percobaan mencari titik |
| `planeAltitude` | 220 | Tinggi pesawat, di atas puncak tertinggi |
| `planeSpeed` | 60 | m/s |
| `planeLeadDistance` | 900 | Jarak awal pesawat dari titik jatuh |
| `descentSpeed` | 6 | m/s. Sekitar 36 detik turun dari 220 meter |
| `flareGameHours` | 4 | Lama suar menyala, sekitar 150 detik nyata |
| `crateExpireGameHours` | 24 | Peti hilang setelah satu hari game |
| `crateOpenHoldSeconds` | 2 | Lama tahan `E` |

**Tag pool:** `Plane_Supply` ukuran 1, `Crate_Supply` ukuran 1, `VFX_RedFlare` ukuran 1.

**Aliran acak:** `RandomStreamId.SupplyDrop`, berkunci nomor hari. Lihat `contractChanges` di bagian 5.

## 2. Detail Logika & Fungsi

### RNG pemicu

**`private void HandleDaybreak(DaybreakEvent e)`**, memakai `e.day`:

1. Selesai kalau `!enabled`, `state != Idle`, atau `GameManager.Instance.currentPhase != GamePhase.IslandSurvival`.
2. Selesai kalau `e.day < minDay`, atau `e.day - lastDropDay <= cooldownDays`.
3. Selesai kalau `e.day % 7 == 0`, karena hari itu milik Weekly Beach Supply (#71).
4. `var rng = RunSeedManager.Instance.Fork(RandomStreamId.SupplyDrop, e.day)`.
5. Kalau `rng.NextFloat01() >= dailyChance`, selesai.
6. `scheduledHour = rng.Range(dropHourMin, dropHourMax + 1)`, lalu `state = Scheduled`.

Harapan: sekitar 5 event tiap 60 hari, dan undian tetap berjalan setelah hari ke-60. Karena berkunci hari, memuat ulang save tidak mengubah hasil undian.

**`private void HandleHour(HourChangedEvent e)`**: kalau `state == Scheduled` dan `e.hour >= scheduledHour`:

- Saat bencana aktif (#70), tunda 1 jam: `scheduledHour = e.hour + 1`. Kalau melewati `dropHourMax`, batalkan untuk hari ini.
- Saat lain, panggil `BeginDrop()`.

### Penentuan koordinat

**`private bool TryFindDropPoint(ref RandomStream rng, out Vector3 point)`**, diulang sampai `locationAttempts`:

1. Ambil sudut acak dan jarak `rng.Range(min, max)` dari posisi pemain.
2. `RegionStreaming.GetRegionAt(kandidat)` harus memenuhi `IsLoaded(region)` dan `RegionGateSystem.IsUnlocked(region)`.
3. `PhysicsQuery.Raycast` ke bawah dari `planeAltitude`, dengan `PhysicsMasks.Ground`. Kena harus pada layer `Terrain`. Kena `Structure` atau `Obstacle` ditolak.
4. Tolak kalau sudut normal `> maxSlopeDegrees`.
5. Tolak kalau `SurfaceDetection.IsNearSand(hit.point, clearanceRadius)`, karena pantai milik #71.
6. Tolak kalau `hit.point` berada di bawah permukaan air region.
7. Tolak kalau jarak ke `BaseAnchor` `< minDistanceFromBase`.
8. `PhysicsQuery.OverlapSphere(hit.point, clearanceRadius, PhysicsMasks.BuildOverlap, buffer)` harus kosong.
9. `NavMesh.SamplePosition` dalam 3 meter, lalu `NavMesh.CalculatePath` dari pemain harus `PathComplete`. Syarat ini menolak seberang jurang yang belum berjembatan.

Kalau semua gagal, event hari itu batal dan dicatat lewat `eventlog`. Kuota hari itu tidak diganti.

### Urutan event

**`private void BeginDrop()`**:

1. Fork kedua `Fork(SupplyDrop, GameManager.currentDay * 31 + 1)` dipakai untuk lokasi. Kalau `TryFindDropPoint` gagal, `state = Idle`.
2. Kalau pemain sedang tidur (`SleepSystem.IsSleeping`), lewati pesawat dan parasut, lalu panggil `Land()` langsung.
3. Ambil `Plane_Supply`. Arah terbang acak, dari `target - dir * planeLeadDistance` ke `target + dir * planeLeadDistance`, pada `planeAltitude`. `state = PlaneInbound`.
4. Kirim notifikasi `Terdengar pesawat di kejauhan` lewat `NotificationManager` (#22).

**`SupplyPlane.Update`**: maju `planeSpeed * Time.deltaTime`. Suara mesin 3D lewat `AudioManager`. Saat melewati proyeksi `target`, panggil `SupplyDropManager.ReleaseCrate()`. Saat jarak terbang habis, `ReturnToPool`. Lama total 30 detik.

**`ReleaseCrate()`**: ambil `Crate_Supply` di ketinggian pesawat tepat di atas `target`, lalu `state = Descending`. `SupplyCrate` belum bisa diinteraksi.

**`SupplyCrate.Update` saat turun**:

- Y turun `descentSpeed * Time.deltaTime`. Tanpa `Rigidbody`, supaya tidak ada risiko menembus tanah.
- Ayunan parasut: `sin(t)` dengan amplitudo 0,5 meter pada mesh saja, bukan pada posisi logika.
- Saat Y sampai `target.y`, panggil `SupplyDropManager.Land()`.

**`Land()`**:

1. Parasut menjalankan animasi kempis selama 1,5 detik, lalu disembunyikan.
2. Ambil `VFX_RedFlare` di samping peti. Asap merah setinggi sekitar 40 meter, supaya terlihat melewati pohon.
3. `NoiseEmitter` memancarkan suara mendarat, dengan radius 30.
4. `landedGameHour = jam game total`, `lastDropDay = hari ini`, `state = Landed`.
5. Terbitkan `SupplyDropLandedEvent { position }`.

### Membuka peti

`SupplyCrate : IslandInteractable`, dengan `holdSeconds = crateOpenHoldSeconds` dan prompt `Buka peti suplai`.

**`OnInteract`**:

1. Ambil isi dari `LootTable` `supply_drop` (#103), dengan `Fork(SupplyDrop, day * 31 + 2)`.
2. Untuk tiap hasil: `WorldItemManager.Spawn(inst, amount, posisi sekitar peti, despawns: true)`.
3. Hentikan suar, buka tutup peti, lalu kembalikan peti ke pool setelah pemain menjauh 30 meter atau 60 detik berlalu.
4. `state = Idle`.

**Isi yang disarankan untuk `supply_drop`:** `Ammo_9mm`, makanan kaleng, air bersih, dan obat dasar. Tidak berisi `Med_Vaccine`, karena GDD bagian 3 menyatakan vaksin baru hanya dari material Region 2.

### Kedaluwarsa

Setiap `HourChangedEvent` saat `Landed`:

- Lewat `flareGameHours`: suar kembali ke pool.
- Lewat `crateExpireGameHours`: peti kembali ke pool saat di luar pandangan kamera, lalu `state = Idle`. Pemain tidak melihat peti lenyap.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `SupplyDropLandedEvent { position }`.

**Event yang didengar:** `DaybreakEvent`, `HourChangedEvent`, `RunStartedEvent`. Kejadian `Unloading` dari `RegionStreaming` (#88).

| Sistem | Hubungan |
|---|---|
| `RunSeedManager` (#89) | `Fork(SupplyDrop, key)` |
| `RegionStreaming` (#88), `RegionGateSystem` (#68) | Region dimuat dan sudah dibuka |
| `SurfaceDetection` (#86) | Menolak pantai |
| `BaseNavigation` (#78) | Posisi `BaseAnchor`. Tanpa base, syarat jarak diabaikan |
| `IslandInteractable` (#17) | Interaksi tahan `E` |
| `RegionLootTable` (#103) | Tabel `supply_drop` |
| `DisasterManager` (#70) | Menunda event. Membaca status bencana aktif |
| `AudioManager`, `VFXManager` (#15), `NotificationManager` (#22) | Suara, asap, dan pesan |
| `AISensory` (#49) | Mendengar `NoiseEmitter` saat mendarat. Hewan di sekitar bisa datang |
| `BeachSupplyManager` (#71) | Tidak bertabrakan, karena hari kelipatan 7 dilewati |

**Batas pekerjaan:** kargo pantai terjadwal milik #71. Isi tabel loot milik #103.

## 4. Kasus Khusus

- **Pool dipakai ulang:** `SupplyCrate.OnSpawned` mengisi ulang parasut, menutup tutup peti, mematikan interaksi, dan mengosongkan rujukan suar. `OnReturned` mengembalikan suar yang masih aktif ke pool. `SupplyPlane.OnSpawned` memutar ulang audio, sedangkan `OnReturned` menghentikannya.
- **Game dijeda:** semua gerakan memakai `Time.deltaTime`, sehingga berhenti. Waktu kedaluwarsa memakai jam game, sehingga ikut berhenti.
- **Tidur:** pesawat dilewati, dan peti langsung mendarat. Waktu kedaluwarsa ikut cepat, sesuai jam game.
- **Region peti dilepas:** saat `Unloading`, pesawat dan parasut dibatalkan, data menjadi `Landed` di `targetPoint`, lalu peti kembali ke pool. Saat region dimuat lagi, `SupplyDropManager` mengambil peti dari pool di `targetPoint`. Peti tidak hilang karena streaming.
- **Peti masih tertutup saat event berikutnya:** undian dilewati, karena `state != Idle`.
- **Pemain berdiri di titik jatuh:** peti tidak melukai. Kapsul pemain dan peti tidak bertabrakan selama turun, karena layer `Interactable` tidak bertabrakan dengan `Player`.
- **Blood Moon (#50):** event terjadi siang, sedangkan Blood Moon malam. Peti yang belum diambil tetap ada saat malam, jadi pemain harus memilih.
- **Save saat `PlaneInbound` atau `Descending`:** disimpan sebagai `Landed` di `targetPoint`, supaya tidak perlu menyimpan posisi pesawat.
- **Yang disimpan di `world_supply`:** `state`, `scheduledHour`, `lastDropDay`, `targetPoint`, `landedGameHour`, dan hari event. **Yang tidak disimpan:** objek pesawat dan VFX. Setelah load, suar muncul lagi kalau `flareGameHours` belum lewat.
- **Permadeath:** data dihapus oleh `RunLifecycle` (#82). Run baru mulai dari `Idle`. Tidak ada yang terbawa.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** nama file dan kelas (`GLOSSARY.md`), `SupplyDropLandedEvent` (`GameEventBus.md`), `world_supply` (`WorldStateSave.md`), tombol `E` (#85), dan batas dengan #71.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Pemakaian sistem:** pakai, dengan peluang 10 persen per hari sesuai contoh issue. GDD belum menyebutnya sebagai mekanik.
2. **Hanya siang:** pesawat hanya terbang pada jam 08:00 sampai 14:00.
3. **Lokasi dekat pemain:** cincin 120 sampai 260 meter dari pemain, bukan dari seluruh pulau, karena hanya region yang dimuat yang punya Terrain dan NavMesh (#88). Sisi buruknya, pemain di region buntu bisa mendapat titik yang lebih terbatas.
4. **Tanpa penjaga tambahan:** peti tidak memunculkan musuh baru. Risikonya berasal dari jarak dan hewan teritorial. Kalau terasa terlalu aman, tambahkan `RequestBurst` kecil setelah playtest.
5. **Kedaluwarsa 24 jam game:** peti bertahan satu malam, supaya pemain punya pilihan menunggu sampai pagi.
6. **Isi tanpa vaksin:** mengikuti GDD bagian 3.
7. **Tidak bertabrakan dengan #71:** hari kelipatan 7 dilewati.

**Perubahan kontrak yang diperlukan:** aliran `SupplyDrop` di `RunSeedManager`, dan `BaseAnchor` di `BaseNavigation`.

## 6. Skenario Tes

Sebelum tes, atur `balance supplyDrop.enabled 1`, dan `balance supplyDrop.dailyChance 1`.

1. `setday 3`, lalu tunggu fajar. **Hasil:** `state = Scheduled`, dan jam jatuh di antara 8 dan 14.
2. Tunggu sampai jam itu. **Hasil:** notifikasi tampil, pesawat melintas sekitar 30 detik, dan suara mengikuti posisinya.
3. Amati peti. **Hasil:** peti turun sekitar 36 detik, parasut kempis, asap merah terlihat dari 200 meter, dan `SupplyDropLandedEvent` terkirim.
4. Tahan `E` selama 2 detik. **Hasil:** item muncul sebagai `WorldItem`, tanpa `Med_Vaccine`.
5. Jalankan `TryFindDropPoint` 100 kali di Region 1 sebelum jembatan selesai. **Hasil:** tidak ada titik di Region 2, air, pantai, atau dalam 80 meter dari base.
6. Ulangi dua kali dengan seed dan hari yang sama. **Hasil:** hasil undian dan jam jatuh sama.
7. Jauhi peti sampai regionnya dilepas, lalu kembali. **Hasil:** peti ada di titik yang sama.
8. Simpan saat peti turun, lalu muat. **Hasil:** peti sudah mendarat, dan bisa dibuka.
9. Biarkan 24 jam game. **Hasil:** peti hilang saat di luar pandangan, dan `state = Idle`.
10. Jeda game saat pesawat melintas. **Hasil:** pesawat dan suara berhenti, lalu lanjut.
11. Mati saat peti masih ada, lalu mulai run baru. **Hasil:** tidak ada peti, dan `lastDropDay` kembali -100.
12. Biarkan `enabled = false`. **Hasil:** tidak ada undian sama sekali.
