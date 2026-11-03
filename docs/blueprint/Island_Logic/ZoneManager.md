# Blueprint: ZoneManager.cs
**Lokasi:** `docs/blueprint/Island_Logic/ZoneManager.md`
**Deskripsi Utama:** Ditempelkan pada *BoxCollider* (IsTrigger) raksasa di tiap area untuk menentukan region aktif pemain dan menerbitkan `RegionChangedEvent`.
**Issue:** #92 (melengkapi #16, #65, #88, dan #111)
**Rujukan GDD:** Bagian 5 (4 Region). Hitungan zona dan pengalihan efek ke sistem lain adalah usulan arsitektur.

## 1. Arsitektur & Variabel Utama

Trigger zona berada di scene `IslandPhase`, bukan di region scene (#88). `ZoneManager` tidak memutuskan pemuatan scene, karena itu milik `RegionStreaming`.

```csharp
public enum RegionType { Volcano, Amazon, Snow, Logistic }   // Enum resmi GLOSSARY
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `myRegion` | `RegionType` | Dari Inspector | Region yang diwakili trigger ini |

`ZoneTracker` adalah kelas statis yang dipakai semua `ZoneManager`. Isinya ditulis di `EnvironmentProfile.md`.

**Dihapus:** `frostbiteCoroutine`, pengecekan `Warmth`, HP -5 per detik, penulisan Post-Processing, dan UI Tutorial.

## 2. Detail Logika & Fungsi

**`void OnTriggerEnter(Collider other)`**: kalau `other.CompareTag("Player")`, panggil `ZoneTracker.Enter(myRegion)`.

**`void OnTriggerExit(Collider other)`**: kalau `other.CompareTag("Player")`, panggil `ZoneTracker.Exit(myRegion)`.

**`RegionChangedEvent { from, to }`** hanya dikirim saat region aktif hasil `ZoneTracker` berubah. Event ini sticky.

**Yang pindah ke sistem lain:**

| Efek lama | Pemilik baru |
|---|---|
| Frostbite dan cek `Warmth` | `WarmthSystem` (#65) menentukan `isFreezing`, dan `SurvivalStats` (#111) menghitung akibatnya |
| Post-Processing dan suasana | `EnvironmentDirector` (#92) |
| Pesan barang berat | `TutorialManager` (#16), pop-up `region_logistic`, lewat `RegionChangedEvent` |
| Pemuatan scene region | `RegionStreaming` (#88) |

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `RegionChangedEvent { from, to }`.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `ZoneTracker` (#92) | Penghitung zona dan region aktif |
| `RegionStreaming` (#88) | Tidak saling memanggil. `GetRegionAt` berbeda dengan event ini |
| `WarmthSystem` (#65), `EnvironmentDirector` (#92), `RegionalWeather` (#115), `SpawnManager` (#62), `TutorialManager` (#16), `AmbientAudio` | Mendengar `RegionChangedEvent` |
| `PhysicsLayers` (#87) | Trigger memakai layer `ZoneTrigger`, dan semua raycast mengabaikan trigger |

## 4. Kasus Khusus

- **Pool dipakai ulang:** trigger tidak memakai pool.
- **Game dijeda:** tidak ada `Update`. Fisika berhenti, jadi trigger tidak terpanggil.
- **Zona bertumpuk:** `ZoneTracker` memakai hitungan zona, sehingga keluar dari satu zona tidak mengubah region kalau masih ada di zona lain.
- **Celah antar-trigger:** region aktif tidak berubah saat pemain tidak berada di zona mana pun.
- **Pendengar terlambat:** membaca `EventChannel<RegionChangedEvent>.Last`.
- **Save dan load:** tidak ada data tersimpan. Pemain dipulihkan di dalam trigger, jadi `OnTriggerEnter` mengisi ulang `ZoneTracker`. Hitungan statis dikosongkan saat scene `IslandPhase` dimuat.
- **Permadeath:** hitungan dikosongkan bersama pemuatan ulang scene.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** trigger di `IslandPhase` (`RegionStreaming.md`), `ZoneTracker` (`EnvironmentProfile.md`), alur frostbite (`VitalsRules.md`), dan pesan barang berat (`TutorialManager.md`).

**Usulan yang menunggu developer:** tidak ada yang baru. Usulan region aktif saat zona bertumpuk dicatat di `EnvironmentProfile.md` bagian 5.

## 6. Skenario Tes

1. Pindah dari Region 1 ke Region 2. **Hasil:** satu `RegionChangedEvent { Volcano, Amazon }`.
2. Bolak-balik di area tumpuk dua zona. **Hasil:** event hanya saat region aktif berubah.
3. Berdiri di Region 3 tanpa pakaian hangat. **Hasil:** `ZoneManager` tidak menulis HP dan tidak memulai coroutine.
4. Masuk Region 4 pertama kali. **Hasil:** pop-up datang dari `TutorialManager`.
5. Keluar dari zona ke celah. **Hasil:** region aktif tetap.
6. Muat save di Region 3. **Hasil:** region aktif benar, dan pendengar menerima `Last`.
7. Lepas `Region3_Snow`, lalu cek trigger. **Hasil:** trigger tetap ada di `IslandPhase`.
