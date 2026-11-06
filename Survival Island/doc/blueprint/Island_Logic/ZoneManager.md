# Blueprint: ZoneManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/ZoneManager.md`
**Deskripsi Utama:** Ditempelkan pada *BoxCollider* (IsTrigger) raksasa di tiap area untuk menentukan region aktif pemain dan menerbitkan `RegionChangedEvent`.
**Issue:** #92 (melengkapi #16, #65, #88, dan #111)
**Rujukan GDD:** Bagian 5 (4 Region, dan akses region dibuka bertahap). Hitungan zona dan pengalihan efek ke sistem lain adalah usulan arsitektur.

## 1. Arsitektur & Variabel Utama

Trigger zona berada di scene `IslandPhase`, bukan di region scene (#88). `ZoneManager` tidak memutuskan pemuatan scene, karena itu milik `RegionStreaming`.

**Batas tanggung jawab:**

| Pekerjaan | Pemilik |
|---|---|
| Mendeteksi pemain masuk dan keluar trigger | `ZoneManager` |
| Menghitung zona dan memilih region aktif | `ZoneTracker` (kelas statis, aturannya di `EnvironmentProfile.md`) |
| Menerbitkan `RegionChangedEvent` | `ZoneManager`, hanya saat `ZoneTracker` melaporkan perubahan |
| Region untuk pemuatan scene dan posisi benda | `RegionStreaming.GetRegionAt` (#88), dari `Bounds`, tanpa trigger |
| Syarat akses region (jembatan, gerbang) | `RegionGateSystem` (#68) |
| Efek region (dingin, suasana, spawn, cuaca, tutorial) | Pendengar `RegionChangedEvent` |

```csharp
public enum RegionType { Volcano, Amazon, Snow, Logistic }   // Enum resmi GLOSSARY
```

### Variabel `ZoneManager`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `myRegion` | `RegionType` | Dari Inspector | Region yang diwakili trigger ini |
| `zoneCollider` | `BoxCollider` | Dari objek yang sama | `isTrigger = true`, layer `ZoneTrigger` |
| `playerInside` | `bool` | `false` | Penjaga, supaya satu trigger tidak dihitung dua kali |
| `static all` | `List<ZoneManager>` | Kosong | Semua trigger aktif. Diisi di `OnEnable`, dikurangi di `OnDisable` |

### Isi `ZoneTracker`

Aturan pilihan region ditulis di `EnvironmentProfile.md` bagian "Zona bertumpuk". Bagian ini mencatat bentuk kodenya.

| Anggota | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `counts` | `int[4]` | Semua 0 | Jumlah trigger tiap `RegionType` yang sedang memuat pemain |
| `lastEntered` | `RegionType` | `Volcano` | Zona terakhir yang dimasuki |
| `Active` | `RegionType` | `Volcano` | Region aktif. Hanya sah kalau `HasActive` |
| `HasActive` | `bool` | `false` | `false` sampai pemain pertama kali masuk zona |
| `sceneHandle` | `int` | 0 | Scene `IslandPhase` yang memiliki hitungan ini |

Tidak ada angka penyetelan. `ZoneManager` tidak membaca `GameBalanceConfig`.

**Dihapus:** `frostbiteCoroutine`, pengecekan `Warmth`, HP -5 per detik, penulisan Post-Processing, dan UI Tutorial.

## 2. Detail Logika & Fungsi

### Trigger

**`void Awake()`**: panggil `ZoneTracker.EnsureScene(gameObject.scene)`. Kalau `sceneHandle` berbeda dengan scene ini, `ZoneTracker.Reset()` mengosongkan semua hitungan. Dengan begitu hitungan statis selalu milik scene `IslandPhase` yang sedang berjalan, tanpa perlu dipanggil sistem lain.

**`void OnTriggerEnter(Collider other)`**:

1. Kalau `!other.CompareTag("Player")` atau `playerInside`, keluar. Musuh, NPC, dan Heavy Item juga menyentuh layer `ZoneTrigger`, tetapi diabaikan.
2. `playerInside = true`.
3. `if (ZoneTracker.Enter(myRegion, out var from, out var to)) Publish(from, to)`.

**`void OnTriggerExit(Collider other)`**:

1. Kalau `!other.CompareTag("Player")` atau `!playerInside`, keluar.
2. `playerInside = false`.
3. `if (ZoneTracker.Exit(myRegion, out var from, out var to)) Publish(from, to)`.

**`private static void Publish(RegionType from, RegionType to)`**: `EventChannel<RegionChangedEvent>.Publish(new RegionChangedEvent { from = from, to = to })`. Event ini sticky.

### Pilihan region di `ZoneTracker`

**`public static bool Enter(RegionType r, out RegionType from, out RegionType to)`**:

1. `counts[r]++`, dan `lastEntered = r`.
2. Kalau `!HasActive`: `HasActive = true`, `Active = r`, isi `from = to = r`, lalu kembalikan `true`. Ini event pertama dalam scene.
3. Kalau `counts[Active] == 0` (pemain datang dari celah) dan `r != Active`: `from = Active`, `Active = r`, `to = r`, lalu kembalikan `true`.
4. Selain itu kembalikan `false`. Region aktif tidak langsung pindah saat pemain baru masuk zona tumpukan.

**`public static bool Exit(RegionType r, out RegionType from, out RegionType to)`**:

1. `counts[r] = Max(0, counts[r] - 1)`.
2. Kalau `r != Active` atau `counts[r] > 0`, kembalikan `false`.
3. Cari pengganti: `lastEntered` kalau hitungannya masih di atas 0. Kalau tidak, region pertama dengan hitungan di atas 0.
4. Kalau tidak ada pengganti, kembalikan `false`. Pemain berada di celah, dan region aktif tetap.
5. `from = Active`, `Active = pengganti`, `to = pengganti`, lalu kembalikan `true`.

**Hasilnya:** di zona tumpukan pada seam, region aktif baru pindah saat pemain benar-benar keluar dari zona lama. Bolak-balik di garis seam tidak memicu event berulang.

**`public static void Reset()`**: semua `counts = 0`, `HasActive = false`, dan `lastEntered = Volcano`. Juga dipanggil lewat `[RuntimeInitializeOnLoadMethod]`, untuk Editor dengan Domain Reload dimatikan (`GameEventBus.md`).

### Teleport dan pemulihan posisi

`IslandPlayerController.Teleport` mematikan `CharacterController`, memindahkan pemain, lalu menyalakannya lagi (#101). Unity tidak menjamin `OnTriggerExit` terkirim untuk collider yang dimatikan, jadi hitungan zona lama bisa tertinggal.

**`public static void ResyncAll(Vector3 playerPosition)`**:

1. Kosongkan `counts` tanpa mengubah `Active` dan `HasActive`.
2. Untuk tiap `ZoneManager` di `all`: `inside = (zoneCollider.ClosestPoint(p) - p).sqrMagnitude < 0,0001`. Isi `playerInside = inside`, dan tambah `counts[myRegion]` kalau `inside`.
3. Kalau `!HasActive` dan ada zona berisi pemain, pilih zona itu, lalu terbitkan event pertama.
4. Kalau `counts[Active] == 0` dan ada zona lain berisi pemain, pindah ke zona itu, lalu terbitkan `RegionChangedEvent { from, to }`.
5. Kalau pemain tidak berada di zona mana pun, region aktif tetap.

Perulangan ini hanya memeriksa beberapa trigger dan tidak memakai query fisika. Pemanggilnya: `IslandPlayerController.Teleport` setelah posisi baru dipasang. Dengan begitu `ProgressionSafety` (#116), `DevConsole tp` dan `tpregion`, serta pemulihan posisi saat load ikut tertangani.

### Yang pindah ke sistem lain

| Efek lama | Pemilik baru |
|---|---|
| Frostbite dan cek `Warmth` | `WarmthSystem` (#65) menentukan `isFreezing`, dan `SurvivalStats` (#111) menghitung akibatnya |
| Post-Processing dan suasana | `EnvironmentDirector` (#92) |
| Pesan barang berat | `TutorialManager` (#16), pop-up `region_logistic`, lewat `RegionChangedEvent` |
| Pemuatan scene region | `RegionStreaming` (#88) |

### Aturan penempatan trigger

Ini panduan level design, dan dicek oleh alat Editor `ZoneCoverageAudit` (usulan):

- Satu atau lebih `BoxCollider` per region. Boleh diputar, karena `ClosestPoint` tetap benar.
- Trigger dua region yang bertetangga harus bertumpuk di seam, supaya tidak ada celah di jalur utama. Usulan lebar tumpukan minimal 10 meter.
- Trigger mencakup ketinggian dari dasar laut sampai puncak tertinggi region, supaya pemain yang berenang, memanjat, atau jatuh tetap terhitung.
- Audit mengambil sampel titik NavMesh tiap 25 meter, lalu melaporkan titik yang tidak berada di zona mana pun, dan titik yang masuk zona dengan `myRegion` berbeda dari `RegionStreaming.GetRegionAt`.
- `OnDrawGizmos` menggambar kotak dengan warna per region.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `RegionChangedEvent { from, to }` (sticky).

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `ZoneTracker` (#92) | Penghitung zona dan region aktif |
| `RegionStreaming` (#88) | Tidak saling memanggil. `GetRegionAt` berbeda dengan event ini |
| `IslandPlayerController` (#101) | `Teleport` memanggil `ZoneManager.ResyncAll` |
| `RegionGateSystem` (#68) | Menahan pemain sebelum syarat akses terpenuhi. `ZoneManager` tidak memeriksa syarat akses |
| `WarmthSystem` (#65), `EnvironmentDirector` (#92), `RegionalWeather` (#115), `SpawnManager` (#62), `TutorialManager` (#16), `AmbientAudio` (#52), `WorldMapSystem` | Mendengar `RegionChangedEvent` |
| `PhysicsLayers` (#87) | Trigger memakai layer `ZoneTrigger` dan tag `Player`. Semua raycast mengabaikan trigger |
| `GameEventBus` (#107) | `EventChannel<RegionChangedEvent>`, dibersihkan `GameEventRegistry.ClearAll()` saat run berakhir |

## 4. Kasus Khusus

- **Pool dipakai ulang:** trigger tidak memakai pool.
- **Game dijeda:** tidak ada `Update`. Fisika berhenti, jadi trigger tidak terpanggil.
- **Tidur (#67):** pemain tidak bergerak, jadi region tidak berubah.
- **Zona bertumpuk:** `ZoneTracker` memakai hitungan zona, sehingga keluar dari satu zona tidak mengubah region kalau masih ada di zona lain.
- **Celah antar-trigger:** region aktif tidak berubah saat pemain tidak berada di zona mana pun.
- **Event pertama:** `from` sama dengan `to`. Pendengar yang memakai `from` (misalnya transisi suasana) memperlakukannya sebagai keadaan awal tanpa transisi.
- **Pendengar terlambat:** membaca `EventChannel<RegionChangedEvent>.Last`.
- **Teleport:** ditangani `ResyncAll`.
- **Pemain mati:** region tidak diubah. `DeathCinematic` (#53) tidak memindahkan kapsul ke zona lain.
- **Pemain berenang di laut antar-region:** trigger mencakup air dangkal di sekitar region. Di luar trigger, region aktif tetap.
- **Save dan load:** tidak ada data tersimpan. Scene `IslandPhase` dimuat ulang, `Awake` mengosongkan hitungan, lalu posisi pemain dipulihkan dari `player_transform` (#80). Kalau pemulihan memakai `Teleport`, `ResyncAll` menerbitkan event pertama. Kalau tidak, `OnTriggerEnter` pada langkah fisika pertama melakukan hal yang sama, karena hitungan sudah kosong.
- **Permadeath:** `RunLifecycle` (#82) memuat ulang scene, sehingga `EnsureScene` mengosongkan hitungan. `GameEventRegistry.ClearAll()` menghapus `Last`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** trigger di `IslandPhase` (`RegionStreaming.md`), `ZoneTracker` dan aturan zona bertumpuk (`EnvironmentProfile.md`), alur frostbite (`VitalsRules.md`), pesan barang berat (`TutorialManager.md`), dan layer `ZoneTrigger` (`PhysicsLayers.md`).

**Usulan yang disetujui developer sebagai nilai awal:** tidak ada yang baru. Usulan region aktif saat zona bertumpuk dicatat di `EnvironmentProfile.md` bagian 5.

**Usulan baru (disetujui developer sebagai nilai awal):**

1. **`Enter` tidak langsung memindah region:** region aktif pindah saat zona lama ditinggalkan, kecuali pemain datang dari celah. Ini tafsiran langsung dari aturan "hitungan untuk keabsahan, zona terakhir untuk memilih".
2. **Event pertama dengan `from == to`:** `RegionType` tidak punya nilai kosong di `GLOSSARY.md`.
3. **`ResyncAll` dipanggil `Teleport`:** perlu satu baris di `IslandPlayerController.md`.
4. **Hitungan statis dikosongkan per scene lewat `EnsureScene`.**
5. **Tumpukan seam minimal 10 meter, dan alat `ZoneCoverageAudit`.**

## 6. Skenario Tes

1. Pindah dari Region 1 ke Region 2. **Hasil:** satu `RegionChangedEvent { Volcano, Amazon }`, terkirim saat keluar dari trigger Region 1.
2. Bolak-balik di area tumpuk dua zona. **Hasil:** event hanya saat region aktif berubah.
3. Berdiri di Region 3 tanpa pakaian hangat. **Hasil:** `ZoneManager` tidak menulis HP dan tidak memulai coroutine.
4. Masuk Region 4 pertama kali. **Hasil:** pop-up datang dari `TutorialManager`.
5. Keluar dari zona ke celah. **Hasil:** region aktif tetap.
6. Masuk zona Region 2 langsung dari celah. **Hasil:** satu event ke `Amazon`.
7. Muat save di Region 3. **Hasil:** region aktif `Snow`, event pertama punya `from == to`, dan pendengar menerima `Last`.
8. Lepas `Region3_Snow`, lalu cek trigger. **Hasil:** trigger tetap ada di `IslandPhase`.
9. Dari Region 1, jalankan `tpregion Snow`. **Hasil:** satu event `{ Volcano, Snow }`, dan hitungan `Volcano` kembali 0.
10. Biarkan zombie dan Heavy Item masuk trigger Region 4 sementara pemain di Region 3. **Hasil:** tidak ada event.
11. Mati di Region 2, lalu mulai run baru. **Hasil:** event pertama run baru adalah `{ Volcano, Volcano }`, tanpa sisa hitungan run lama.
12. Jalankan Play Mode dua kali di Editor dengan Domain Reload dimatikan. **Hasil:** hitungan dimulai dari 0 setiap kali.
13. Jalankan `ZoneCoverageAudit` pada peta yang sengaja diberi celah 30 meter. **Hasil:** titik celah dilaporkan.
14. Jeda game saat berdiri di seam. **Hasil:** tidak ada event selama jeda.

## 7. Catatan Implementasi (Langkah 6)

- File: `Island_Logic/ZoneManager.cs` dan `Island_Logic/ZoneTracker.cs` (kelas statis). `RegionType` ditulis Langkah 4 dan `RegionChangedEvent` ditulis Langkah 5, jadi tidak dideklarasikan ulang.
- `ZoneTracker.EnsureScene` memakai `Scene.handle`. `Reset` juga dipanggil lewat `[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]`, begitu juga daftar `ZoneManager.all`.
- `ResyncAll` langkah 3-4 dikerjakan `ZoneTracker.ResolveAfterResync`, dengan zona pertama yang berisi pemain sebagai pengganti.
- `OnDisable` hanya mengeluarkan trigger dari `all` dan tidak menerbitkan event, karena `OnDisable` juga berjalan saat scene `IslandPhase` dilepas.
- `IslandPlayerController.Teleport` sudah memanggil `ZoneManager.ResyncAll(position)`.
- Alat Editor `ZoneCoverageAudit` (usulan) belum ditulis. `OnDrawGizmos` menggambar kotak berwarna per region.
