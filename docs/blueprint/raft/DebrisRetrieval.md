# Blueprint: DebrisRetrieval.cs
**Lokasi:** `docs/blueprint/raft/DebrisRetrieval.md`
**Deskripsi Utama:** Menarik barang hanyut dari laut memakai `Base_Kail`, dengan tombol yang sama seperti memancing.
**Issue:** #106
**Rujukan GDD:** Usulan, belum ada di GDD. Sistem ini nonaktif sampai developer menyetujui. GDD bagian 3 menulis Kail "seperti di game Raft", tetapi hanya menyebut kail untuk memancing. **Rekomendasi:** dipakai dengan prioritas rendah, setelah #55 dan #71 selesai. Biayanya kecil, karena memakai lemparan #55 dan satu pool. Sistem ini juga cocok dengan cerita kapal karam, dan membuat pantai lebih hidup di antara kargo mingguan.

## 1. Arsitektur & Variabel Utama

- **`DebrisRetrieval : MonoBehaviour, IKailCastInterceptor, ISaveable`:** singleton di `IslandPhase`. Mengatur kemunculan, daftar barang, dan tarikan.
- **`DebrisItem : MonoBehaviour, IInteractable, IPoolable`:** satu barang hanyut. Pool `Debris_Generic`, mesh diganti dari `ItemData.worldMesh`, dan layer `Interactable`. Bergerak lewat `Transform`, tanpa Rigidbody.
- **`DebrisLane : MonoBehaviour`:** jalur arus di scene region. Berisi `Vector3[] points` dari laut lepas ke titik hilang, dan `RegionType region`. Mendaftar di `OnEnable` dan melepas diri di `OnDisable`, sehingga ikut aktif bersama region (#88).
- **Sakelar:** kalau `GameBalanceConfig.debris.enabled` bernilai `false`, `OnEnable` tidak mendaftar ke `FishingSystem` dan tidak memunculkan barang. Nilai awalnya `false` sampai developer menyetujui.

**Aturan membuat jalur:** titik pertama dan terakhir berada minimal 60 meter dari pantai terdekat, supaya tidak terlihat muncul atau hilang. Bagian tengah jalur melewati 10 sampai 20 meter dari garis pantai, sehingga masuk jangkauan `castMaxDistance`.

| Variabel | Tipe | Nilai awal | Disimpan | Fungsi |
|---|---|---|---|---|
| `Instance` | `DebrisRetrieval` | `null` | Tidak | Singleton per scene |
| `lanes` | `List<DebrisLane>` | Kosong | Tidak | Jalur di region yang dimuat |
| `active` | `List<DebrisItem>` | Kosong | Tidak | Barang yang sedang hanyut |
| `nextSpawnGameHour` | `float` | Sekarang + interval | Ya | Jadwal kemunculan |
| `hooked` | `DebrisItem` | `null` | Tidak | Barang yang tersangkut |
| `kail` | `FishingSystem` | `null` | Tidak | Sesi yang sedang menarik |
| `tideIsHigh` | `bool` | `false` | Tidak | Dari `TideChangedEvent` |

**Variabel `DebrisItem`:** `item` (`ItemData`), `amount`, `lane`, `segment`, `segmentT`, dan `isHooked`.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.debris.enabled` | `false` | Sakelar sistem |
| `GameBalanceConfig.debris.maxActive` | 4 | Barang sekaligus |
| `GameBalanceConfig.debris.spawnIntervalGameHours` | 3 | Jeda dasar, 8 barang per hari game |
| `GameBalanceConfig.debris.spawnCheckSeconds` | 1 | Jeda pemeriksaan jadwal, detik |
| `GameBalanceConfig.debris.stormIntervalMultiplier` | 0,5 | Pengali jeda saat `Storm` |
| `GameBalanceConfig.debris.rainIntervalMultiplier` | 0,75 | Pengali jeda saat `Rain` |
| `GameBalanceConfig.debris.highTideIntervalMultiplier` | 0,75 | Pengali jeda saat air pasang |
| `GameBalanceConfig.debris.spawnPlayerMaxDistance` | 150 | Hanya jalur sedekat ini dari pemain yang dipakai |
| `GameBalanceConfig.debris.driftSpeed` | 1,2 | Laju arus, meter per detik |
| `GameBalanceConfig.debris.hookRadius` | 2,5 | Radius kait di sekitar titik jatuh kail |
| `GameBalanceConfig.debris.pullSpeed` | 4 | Laju tarik saat tombol ditahan |
| `GameBalanceConfig.debris.collectDistance` | 3 | Jarak barang masuk inventori |
| `GameBalanceConfig.debris.pullMaxDistance` | 32 | Jarak maksimum tarikan, sama dengan `fishing.leashDistance` |

Lontaran memakai `fishing.castMinDistance` (6), `fishing.castMaxDistance` (25), dan `fishing.lureFlightSeconds` (0,8), supaya kedua fungsi Kail terasa sama.

### Isi barang

`LootTable` (#103) bernama `Debris_Default`:

| Item | Jumlah | Bobot |
|---|---|---|
| `Mat_Wood` | 3 | 55 |
| `Mat_Cloth` | 1 | 30 |
| `Drink_CleanWater` | 1 | 7 |
| `Food_Can` | 1 | 5 |
| `Ammo_9mm` | 3 | 3 |

### Keseimbangan sumber daya (#84)

Kalau semua dari 8 barang per hari tertangkap dalam cuaca cerah:

| Sumber | Debris per hari | Pembanding | Akibat |
|---|---|---|---|
| Kayu | 13,2 | Satu pohon 10 kayu per 25 detik | Hanya 1,3 pohon. Jembatan 500 kayu tidak bisa dilewati lewat debris |
| Makanan | 0,4 porsi | Target 3 porsi per hari | Sekitar 13 persen |
| Air | 0,56 porsi | Target 4 porsi per hari | Sekitar 14 persen |
| Peluru | 0,72 | Kargo mingguan (#71) | Tambahan kecil, kargo tetap sumber utama |

Satu tarikan butuh sekitar 8 detik, tetapi pemain harus berada di pantai dan menunggu barang lewat. Angka final mengikuti playtest.

## 2. Detail Logika & Fungsi

### Tombol: memancing atau menarik

Tidak ada tombol baru. Klik kiri tetap melempar Kail, dan targetnya menentukan fungsi:

1. Selama `Charging`, `FishingSystem` memanggil `IsTargetAt(landing)`. Kalau `true`, penanda berubah menjadi `Tarik`.
2. Saat kail mendarat, `FishingSystem` memanggil `TryIntercept(landing, this)`. Kalau ada `DebrisItem` dalam `hookRadius`, kail mengait barang.
3. Kalau tidak, memancing berjalan seperti biasa (#55).

Kail yang sudah menunggu ikan tidak mengait barang yang lewat. Pemain harus melempar ulang.

**`public bool IsTargetAt(Vector3 landing)`:** cari di `active` dengan jarak XZ. Pencarian daftar berisi paling banyak 4 barang ini tidak memakai query fisika.

**`public bool TryIntercept(Vector3 landing, FishingSystem k)`**:

1. Ambil barang terdekat dalam `hookRadius` yang belum `isHooked`. Kalau tidak ada, kembalikan `false`.
2. `hooked = item`, `kail = k`, `item.isHooked = true`, lalu kembalikan `true`.
3. `FishingSystem` tetap menggambar benang ke `hooked.transform`.

### Kemunculan

**`void Update()`:** tambah timer dengan `Time.deltaTime`. Setiap `spawnCheckSeconds`, panggil `TrySpawn()`. Jadwal tidak memakai `HourChangedEvent`, karena jeda seperti 1,5 atau 2,25 jam akan terbulatkan ke atas, dan pengali cuaca kehilangan efeknya.

**`private void TrySpawn()`**:

1. Kalau `TotalGameHours < nextSpawnGameHour`, selesai.
2. Jadwalkan ulang: `nextSpawnGameHour = TotalGameHours + CurrentInterval()`.
3. Kalau `active.Count >= maxActive`, selesai.
4. Pilih jalur acak dalam `spawnPlayerMaxDistance`, memakai aliran `Gameplay`. Kalau tidak ada, selesai.
5. Undi isi: `Debris_Default` dengan `rng.PickWeighted(weights)`.
6. `ObjectPooler.Instance.SpawnFromPool("Debris_Generic", lane.points[0], Quaternion.identity)`, lalu `Setup(item, amount, lane)`.

**`private float CurrentInterval()`**, dengan cuaca jalur terdekat dari pemain lewat `RegionalWeather.Instance.GetWeather(region)`:

```csharp
float i = b.spawnIntervalGameHours;
if (w == WeatherType.Storm) i *= b.stormIntervalMultiplier;
else if (w == WeatherType.Rain) i *= b.rainIntervalMultiplier;
if (tideIsHigh) i *= b.highTideIntervalMultiplier;
return i;
```

`tideIsHigh` dibaca dari `TideChangedEvent`, termasuk `Last` saat aktif. Kalau #91 belum ada, nilainya tetap `false`. Karena jeda dihitung saat penjadwalan, cuaca yang berubah berlaku pada jadwal berikutnya.

### Hanyut dan titik hilang

**`DebrisItem.Update()`**, kalau `!isHooked`:

1. Maju sepanjang `lane.points` sejauh `driftSpeed * Time.deltaTime`.
2. Tinggi Y mengikuti titik jalur ditambah `Mathf.Sin` kecil untuk efek mengapung. Jalur tidak mengikuti tinggi pasang, supaya tidak ada raycast tiap frame.
3. Saat melewati titik terakhir, panggil `DebrisRetrieval.Instance.Despawn(this)`.

Dengan `driftSpeed` 1,2, jalur 200 meter menahan barang sekitar 167 detik, atau sekitar 4,4 jam game. Barang yang tidak ditarik akan hilang sendiri.

### Tarikan

**`public void TickIntercept(bool isHeld, float dt)`**, dipanggil `FishingSystem` tiap frame selama `Intercepted`:

1. Kalau `hooked == null`, panggil `kail.EndIntercept()`, lalu selesai.
2. Kalau `isHeld`, gerakkan barang menuju pemain di bidang XZ sejauh `pullSpeed * dt`, dengan Y tetap. Kalau tidak, barang diam di tempat.
3. Kalau jarak pemain ke barang melebihi `pullMaxDistance`, panggil `ReleaseHooked()`, lalu `kail.EndIntercept()`.
4. Kalau jarak kurang dari `collectDistance`, panggil `Collect()`.

Tarikan tidak memakai Tension Bar, karena ini barang, bukan ikan. Klik kanan membatalkan sesi di `FishingSystem`, yang memanggil `CancelIntercept()`.

**`private void ReleaseHooked()`:** `hooked.isHooked = false`. Barang mencari titik terdekat pada jalurnya, lalu hanyut lagi. `hooked = null`.

**`public void CancelIntercept()`:** panggil `ReleaseHooked()`, tanpa memanggil `EndIntercept()`, karena `FishingSystem` sudah mengakhiri sesinya.

**`private void Collect()`**:

1. `int sisa = inventory.AddItem(hooked.item, hooked.amount)`.
2. Kalau `sisa == 0`: terbitkan `DebrisCollectedEvent`, lalu `Despawn(hooked)`, dan `kail.EndIntercept()`.
3. Kalau `sisa > 0`: `hooked.amount = sisa`, tampilkan `Inventori penuh`, dan barang tetap mengapung di tempat. Pemain bisa mengambilnya dengan `E` lewat `IInteractable` selama kail belum dilepas. `Collect()` tidak dicoba lagi sampai `InventoryChangedEvent` menunjukkan ada ruang. Saat kail dilepas, barang hanyut lagi.

**`public void Despawn(DebrisItem d)`:** hapus dari `active`, lalu `ObjectPooler.Instance.ReturnToPool("Debris_Generic", d.gameObject)`. Kalau `d == hooked`, atur `hooked = null` lebih dulu, lalu `TickIntercept` berikutnya mengakhiri sesi.

**Pada `FishingSystem`:** hasil akhir tetap `InterceptedByTarget`, sehingga jeda dan state mengikuti #55.

## 3. Hubungan dengan Sistem Lain

**Event dikirim:** `DebrisCollectedEvent`.
**Event didengar:** `TideChangedEvent` (sticky) dan `InventoryChangedEvent`, hanya saat barang tertahan karena inventori penuh.

| Sistem | Hubungan |
|---|---|
| `FishingSystem` (#55) | Lontaran, input, dan benang. Sistem ini mendaftar lewat `RegisterInterceptor` |
| `ObjectPooler` | Pool `Debris_Generic` |
| `RegionalWeather` (#115) | Cuaca per region jalur |
| `TideManager` (#91) | `TideChangedEvent.isHigh` |
| `RegionStreaming` (#88) | Mengaktifkan dan mematikan `DebrisLane` |
| `RunSeedManager` (#89) | Aliran `Gameplay` |
| `RegionLootTable` (#103) | Bentuk `LootTable` |
| `InventoryManager` | Menerima barang |
| `BeachSupplyManager` (#71) | Sumber terpisah. Debris tidak mengubah kargo |
| `WorldStateSave` (#80) | Menyimpan `nextSpawnGameHour` |

## 4. Kasus Khusus

- **Dipakai ulang dari pool:** `OnSpawned` dan `Setup` mengatur ulang `item`, `amount`, `lane`, `segment`, `segmentT`, `isHooked = false`, dan mesh. `OnReturned` mengosongkan semua rujukan, supaya tidak menunjuk jalur lama.
- **Game dijeda:** barang tidak bergerak, karena memakai `Time.deltaTime`. Kemunculan juga berhenti, karena jam game berhenti.
- **Tidur:** jam game cepat, tetapi barang hanyut dalam detik nyata. `maxActive` membatasi jumlahnya, jadi barang tidak menumpuk.
- **Region dilepas:** `DebrisLane.OnDisable` mengembalikan semua barang di jalur itu ke pool. Kalau salah satunya sedang tersangkut, kail dilepas lebih dulu.
- **Kail dibatalkan:** `FishingSystem.OnUnequip`, serangan langsung, atau terlalu jauh memanggil `CancelIntercept()`. Barang hanyut lagi.
- **Dua target berdekatan:** barang terdekat yang dipilih.
- **Barang lewat di dalam tebing:** jalur dibuat di laut lepas. `LayerAuditWindow` tidak memeriksanya, jadi jalur harus dicek manual di scene.
- **Barang ditembak:** layer `Interactable` masuk mask `Shoot`, tetapi `DebrisItem` tidak punya `IDamageable`, sehingga tidak terjadi apa-apa.
- **Save dan load:** hanya `nextSpawnGameHour` yang disimpan, di bawah kunci `world_debris`. Barang yang sedang hanyut tidak disimpan, karena nilainya kecil dan pool kembali kosong setelah load.
- **Permadeath:** `nextSpawnGameHour` dihapus bersama save. Pool dibersihkan saat scene dimuat ulang.
- **Sistem nonaktif:** `FishingSystem` berjalan seperti biasa tanpa interceptor.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `Base_Kail` adalah item inti. Klik kiri dipakai item aktif (`GLOSSARY.md`). Memancing ikan ada di #55, dan kargo mingguan ada di #71.

**Usulan yang menunggu developer:**

1. **Fungsi ini dipakai atau tidak:** rekomendasi dipakai dengan prioritas rendah, setelah #55 dan #71. Kalau ditolak, issue ini bisa ditutup, dan `FishingSystem` tidak perlu berubah.
2. **Isi barang:** terutama kayu dan kain. Makanan, air, dan peluru dibuat jarang, supaya kargo mingguan tetap berharga.
3. **Pengaruh cuaca dan pasang:** `Storm` 0,5, `Rain` 0,75, dan pasang 0,75 sebagai pengali jeda. Badai membuat barang lebih banyak, dan pemain harus memilih antara berisiko keluar atau tidak.
4. **Tarikan tanpa Tension Bar:** cukup tahan klik kiri.
5. **Barang tidak disimpan:** hilang saat load.
6. **Laju kemunculan:** 8 barang per hari game.

## 6. Skenario Tes

Atur `debris.enabled = true` lewat `balance` di `DevConsole`, kecuali tes 1.

1. Biarkan `enabled = false`, lempar Kail ke laut. **Hasil:** memancing normal, dan tidak ada barang muncul.
2. Tunggu 3 jam game di pantai. **Hasil:** satu barang muncul di awal jalur dan bergerak mengikuti titik.
3. Tunggu hingga empat barang aktif. **Hasil:** barang kelima tidak muncul sampai ada yang hilang.
4. Biarkan satu barang sampai titik terakhir. **Hasil:** kembali ke pool, dan `active` berkurang.
5. Arahkan penanda ke barang. **Hasil:** teks berubah menjadi `Tarik`.
6. Lempar tepat ke barang. **Hasil:** benang tersambung, tanpa `Waiting`.
7. Lempar ke air kosong. **Hasil:** memancing biasa berjalan.
8. Tahan klik kiri. **Hasil:** barang mendekat 4 meter per detik, dan masuk inventori dalam jarak 3 meter.
9. Lepas tombol di tengah. **Hasil:** barang diam. Klik kanan: barang hanyut lagi di jalurnya.
10. Tarik saat inventori penuh. **Hasil:** muncul `Inventori penuh`, barang tetap ada, dan `E` bisa mengambilnya setelah ada ruang.
11. Berjalan menjauh melewati 32 meter. **Hasil:** barang lepas.
12. Paksa `Storm` lewat `DevConsole`. **Hasil:** jeda berikutnya 1,5 jam game.
13. Kirim `TideChangedEvent` dengan `isHigh = true` saat `Clear`. **Hasil:** jeda 2,25 jam game.
14. Tinggalkan region saat barang tersangkut. **Hasil:** kail kembali, dan tidak ada barang tertinggal aktif.
15. Pakai ulang barang dari pool. **Hasil:** isi, posisi, dan `isHooked` sesuai barang baru.
16. Jeda game. **Hasil:** semua barang berhenti.
17. Save, lalu load. **Hasil:** `nextSpawnGameHour` sama, dan barang lama tidak muncul.
18. Tarik semua barang satu hari game dan catat hasilnya di lembar playtest. **Hasil:** kayu sekitar 13.
