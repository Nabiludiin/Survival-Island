# Blueprint: BuildingIntegrity.cs
**Lokasi:** `docs/blueprint/raft/BuildingIntegrity.md`
**Deskripsi Utama:** Perbaikan struktur dengan palu, upgrade dari kayu ke batu, dan tampilan bangunan yang rusak atau sudah di-upgrade.
**Issue:** #47
**Rujukan GDD:** Bagian 3 (zombie menyerang base) dan bagian 6 (gempa merusak base yang dindingnya belum di-upgrade, dan pemain perlu cadangan material untuk perbaikan). Angka perbaikan dan biaya upgrade adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`BuildingIntegrity` adalah manager di scene `IslandPhase`. HP tetap milik `BaseHealth` (`DamagePipeline.md`). Sistem ini hanya mengatur syarat, biaya, dan tampilan.

| Kelas | Jenis | Tugas |
|---|---|---|
| `BuildingIntegrity` | Manager | `TryRepairSwing`, `TryUpgrade`, dan `GetRepairPreview` |
| `StructureVisual` | Komponen di prefab struktur | Mengganti material dan mesh sesuai tingkat kerusakan |
| `BuildableData` | ScriptableObject (#79) | Mendapat field tambahan di bawah |

### Tambahan pada `BuildableData`

```csharp
public enum StructureTier { Wood, Stone }

[Serializable]
public struct DamageVisualStage
{
    public float maxHpRatio;       // Tahap ini berlaku kalau HpRatio <= nilai ini
    public Material material;      // Material bersama. Tanpa salinan per objek
    public Mesh mesh;              // Boleh null, artinya memakai mesh asli
    public string enterFxPoolTag;  // Efek saat masuk tahap ini. Boleh kosong
}
```

| Field | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `tier` | `StructureTier` | `Wood` | Tingkat material |
| `upgradeTo` | `BuildableData` | `null` | Hasil upgrade. `null` berarti tidak bisa di-upgrade |
| `upgradeCost` | `ItemCost[]` | Kosong | Biaya upgrade |
| `repairItemId` | `string` | `Mat_Wood` | Bahan perbaikan |
| `repairCostPerSwing` | `int` | 1 | Bahan per ayunan |
| `survivesEarthquake` | `bool` | `false` | Lolos dari damage gempa. Mengikuti GDD bagian 6 |
| `damageVisuals` | `DamageVisualStage[]` | Tiga tahap | Diurutkan dari `maxHpRatio` terbesar |

### Data bangunan

| ID | Tier | HP | Bahan perbaikan | Upgrade ke | Biaya upgrade | Tahan gempa | `Fire` |
|---|---|---|---|---|---|---|---|
| `Wall_Wood` | `Wood` | 300 | `Mat_Wood` | `Wall_Stone` | 10 `Mat_Stone` | Tidak | 1 |
| `Wall_Stone` | `Stone` | 900 | `Mat_Stone` | Tidak ada | Tidak ada | Ya | 0 |
| `Door_Wood` | `Wood` | 200 | `Mat_Wood` | `Door_Stone` | 8 `Mat_Stone` | Tidak | 1 |
| `Door_Stone` | `Stone` | 600 | `Mat_Stone` | Tidak ada | Tidak ada | Ya | 0 |
| `Floor_Wood`, `Roof_Wood`, `Stairs_Wood` | `Wood` | Sesuai #79 | `Mat_Wood` | Tidak ada | Tidak ada | Tidak | 1 |

Checklist hanya meminta dinding, jadi lantai dan atap tidak bisa di-upgrade pada versi awal. Pintu ikut, karena GDD menganggap dinding yang belum di-upgrade sebagai titik lemah, dan pintu kayu akan menjadi lubang pada base batu.

Pengali `Fire` di atas masuk ke `BuildableData.resistances`. Pengali `Bullet`, `Melee`, `Explosion`, dan `Structural` tetap mengikuti `DamagePipeline.md`.

### Variabel `BuildingIntegrity`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `BuildingIntegrity` | `null` | Singleton per scene |
| `nextSwingTime` | `float` | 0 | Waktu ayunan perbaikan berikutnya |
| `upgradeTimer` | `float` | 0 | Waktu tombol upgrade ditahan |
| `upgradeTargetId` | `string` | Kosong | Struktur yang sedang di-upgrade |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.integrity.repairRange` | 4 | Jarak perbaikan dan upgrade |
| `GameBalanceConfig.integrity.repairSwingSeconds` | 0,6 | Jeda antar-ayunan |
| `GameBalanceConfig.integrity.repairHpRatioPerSwing` | 0,15 | Bagian HP maksimum per ayunan |
| `GameBalanceConfig.integrity.repairBlockedAfterDamageSeconds` | 2 | Jeda setelah struktur menerima damage |
| `GameBalanceConfig.integrity.upgradeHoldSeconds` | 1 | Lama menahan klik kiri untuk upgrade |
| `GameBalanceConfig.integrity.upgradeBlockedAfterDamageSeconds` | 5 | Jeda upgrade setelah menerima damage |
| `GameBalanceConfig.integrity.hammerDecayPerRepairSwing` | 0,5 | Durabilitas palu per ayunan |
| `GameBalanceConfig.integrity.hammerDecayPerUpgrade` | 2 | Durabilitas palu per upgrade |
| `GameBalanceConfig.integrity.noiseRadiusPerSwing` | 15 | Radius suara palu (#49) |

**Hasil dari angka di atas untuk `Wall_Wood`:** 7 ayunan, 7 kayu, dan sekitar 4,2 detik dari hampir hancur sampai penuh. Membangun dinding baru butuh 12 kayu, jadi memperbaiki tetap lebih hemat.

## 2. Detail Logika & Fungsi

### Palu perbaikan

Mode `Repair` adalah mode awal palu (#79). Klik kiri dibaca `EquipmentManager`, lalu diteruskan lewat `HammerEquippable`.

**`public RepairResult TryRepairSwing()`**, dipanggil saat `Pressed` dan `Held`:

1. Kalau `Time.time < nextSwingTime`, selesai tanpa pesan.
2. `StructureRegistry.TryGetAimedStructure(out node)`, dengan batas `repairRange`. Kalau tidak ada, kembalikan `NoTarget`.
3. Kalau `health.HpRatio >= 1`, kembalikan `FullHP`, dengan teks `Sudah utuh`.
4. Kalau `Time.time - health.LastDamageTime < repairBlockedAfterDamageSeconds`, kembalikan `UnderAttack`.
5. Kalau `!InventoryManager.Instance.HasItem(repairItemId, repairCostPerSwing)`, kembalikan `NoMaterial`, dan sebut nama bahannya.
6. Kurangi bahan dengan `RemoveItem(repairItemId, repairCostPerSwing)`.
7. `health.Repair(data.maxHP * repairHpRatioPerSwing * NPCRoster.GetSkillMultiplier(NPCSkill.Crafter))` (#76). Fungsi ini sudah ditetapkan `DamagePipeline.md`, dan nilainya dibatasi `maxHP`.
8. Kurangi durabilitas palu sebesar `hammerDecayPerRepairSwing`.
9. Atur `nextSwingTime = Time.time + repairSwingSeconds`.
10. Mainkan animasi ayun, suara, dan efek serpihan dari pool `FX_RepairHit`. Kirim `NoiseEmittedEvent` lewat `NoiseEmitter` palu dengan radius `noiseRadiusPerSwing`.
11. Terbitkan `StructureRepairedEvent { structureId, hpRatio }`.

**Prompt HUD:** saat membidik struktur rusak dalam mode `Repair`, HUD menampilkan bar HP dan bahan, misalnya `Perbaiki (1 Kayu) 62%`. Bidikan diperiksa lewat `StructureRegistry` setiap 0,1 detik, bukan tiap frame.

**Kena damage saat diperbaiki:** langkah 4 berlaku. Pemain tidak bisa mengimbangi zombie yang terus memukul dinding yang sama, sehingga harus membunuh zombie lebih dulu.

### Upgrade

Mode `Upgrade` menahan klik kiri selama `upgradeHoldSeconds`.

**`public void TickUpgrade(ActionPhase phase)`**:

| `phase` | Tindakan |
|---|---|
| `Pressed` | Periksa syarat. Kalau lolos, catat `upgradeTargetId`, dan `upgradeTimer = 0` |
| `Held` | Tambah `upgradeTimer`. Target berubah berarti batal. Saat mencapai `upgradeHoldSeconds`, panggil `TryUpgrade(node)` |
| `Released` | Batal |

**Syarat upgrade:**

| No | Syarat | Pesan saat gagal |
|---|---|---|
| 1 | Ada target dalam `repairRange` | Tidak ada |
| 2 | `data.upgradeTo != null` | `Tidak bisa ditingkatkan` |
| 3 | Sudah membuka resep upgrade lewat `upgradeTo.unlockNodeId` (`TechTreeManager`) | `Belum dipelajari` |
| 4 | `Time.time - health.LastDamageTime >= upgradeBlockedAfterDamageSeconds` | `Sedang diserang` |
| 5 | Semua `upgradeCost` tersedia | `Butuh <bahan>` |

**`private bool TryUpgrade(StructureNode node)`**:

1. Periksa ulang syarat.
2. Kurangi `upgradeCost`.
3. Hitung `ratio = health.HpRatio`.
4. Panggil `StructureRegistry.ReplaceInstance(node, data.upgradeTo)`. Fungsi ini membuat prefab baru pada posisi dan rotasi yang sama, memakai `structureId` yang sama, memindahkan isi grid, memanggil `Initialize(data.upgradeTo, id, data.upgradeTo.maxHP * ratio)`, memindahkan status pintu kalau ada, lalu menghapus objek lama.
5. Kurangi durabilitas palu sebesar `hammerDecayPerUpgrade`.
6. Mainkan efek dari pool `FX_UpgradeDust` dan suara.
7. Terbitkan `StructureUpgradedEvent { structureId, buildableId }`.

**Rasio HP tetap:** dinding kayu berisi 50 persen menjadi dinding batu berisi 50 persen. Pemain tidak mendapat perbaikan gratis dari upgrade.

**NavMesh:** prefab baru memuat `NavMeshObstacle` yang aktif pada frame yang sama, dan objek lama dihapus di akhir frame. Lubang di dinding tidak sempat terbuka (#78).

**`BreachSlots`:** tetap sah, karena `structureId` tidak berubah. Zombie yang sedang menyerang memakai target baru lewat `StructureRegistry.Find(structureId)`. Upgrade sendiri ditolak selama struktur baru saja diserang.

### Tampilan rusak

**Perbandingan cara:**

| Cara | Kelebihan | Kekurangan |
|---|---|---|
| `MaterialPropertyBlock` dengan nilai retak | Bertahap dan halus | Struktur tidak bisa digabung SRP Batcher, dan batch naik |
| Tiga `sharedMaterial` per tier | Batch tetap rendah, sesuai batas #88 | Perubahan bertahap per tahap |
| Prefab terpisah per tahap | Bentuk bisa jauh berbeda | Mengganti objek berarti mengganti collider dan obstacle |

**Pilihan: `sharedMaterial` per tahap, dan mesh khusus pada tahap terakhir.** Collider tidak berubah, jadi NavMesh dan peluru tetap sama.

| Tahap | `maxHpRatio` | Material | Mesh | Efek saat masuk |
|---|---|---|---|---|
| Utuh | 1 | Bersih | Asli | Tidak ada |
| Retak | 0,66 | Retak halus | Asli | `FX_StructureCrack` |
| Hancur sebagian | 0,33 | Retak besar | Berlubang secara visual | `FX_StructureCrack` |

**`StructureVisual`:**

| Fungsi | Tugas |
|---|---|
| `OnEnable` | Berlangganan `health.HpRatioChanged`, lalu `Apply(health.HpRatio, playFx: false)` |
| `OnDisable` | Melepas langganan |
| `Apply(float ratio, bool playFx)` | Mencari tahap dengan `maxHpRatio` terkecil yang masih `>= ratio`. Kalau tahapnya sama, selesai. Kalau beda, ganti `sharedMaterial` dan `mesh`, lalu mainkan efek saat turun tahap |

`HpRatioChanged` adalah event per objek, seperti `Damaged` di `EnemyHealth`. Event global `StructureDamagedEvent` tidak dipakai di sini, supaya satu pukulan tidak membangunkan 400 struktur.

**Dinding batu:** memakai satu set material dan mesh batu sendiri di `damageVisuals`, jadi model berubah saat di-upgrade. Tiap prefab punya `SurfaceTag` sendiri (#86), sehingga suara langkah dan efek peluru ikut berubah.

### Gempa

GDD bagian 6 menyebut gempa merusak base yang dindingnya belum di-upgrade. `DisasterManager` (#70) melewati struktur dengan `data.survivesEarthquake == true` saat gempa, baik struktur yang aktif maupun yang berada di region tidak dimuat. Hal ini dipakai juga oleh #90.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `StructureRepairedEvent` (`structureId`, `hpRatio`, baru), `StructureUpgradedEvent` (`structureId`, `buildableId`, baru), `NoiseEmittedEvent` (lewat `NoiseEmitter`).

**Event yang didengar:** tidak ada. `StructureVisual` mendengar `HpRatioChanged` per objek.

| Sistem | Hubungan |
|---|---|
| `BaseHealth` (#59) | `Repair`, `HpRatio`, `HpRatioChanged`, `LastDamageTime`, dan `Initialize` |
| `BuildMenu`, `StructureRegistry` (#79) | `HammerEquippable`, `TryGetAimedStructure`, `ReplaceInstance`, dan `Find` |
| `BaseNavigation` (#78) | Carving dan `BreachSlots` tetap sah setelah upgrade |
| `InventoryManager` | `HasItem` dan `RemoveItem` |
| `ItemInstance` (#66) | Durabilitas palu |
| `DisasterManager` (#70), `RemoteBaseSimulation` (#90) | Membaca `survivesEarthquake` dan `tier` |
| `TechTreeManager` | Membuka upgrade |
| `SurfaceDetection` (#86) | Membaca `SurfaceTag` prefab baru |
| `AISensory` (#49) | Mendengar suara palu |
| `TutorialManager` (#16), `LocalStatsManager` (#57) | Boleh mendengar `StructureRepairedEvent` dan `StructureUpgradedEvent` |

**Batas pekerjaan:** menu dan bongkar di #79, jalur musuh di #78, dan perbaikan alat di #113.

## 4. Kasus Khusus

- **Objek pool:** struktur tidak memakai pool. `FX_RepairHit`, `FX_UpgradeDust`, dan `FX_StructureCrack` memakai `ObjectPooler`. Saat dipakai ulang, `OnSpawned` memutar ulang partikel, dan `OnReturned` menghentikannya.
- **Game dijeda:** `Time.time` berhenti, jadi jeda ayunan dan jeda setelah damage tidak maju. Input juga mati (#83).
- **Permadeath:** tier dan HP berada di `StructureRecord` (#79), dan terhapus bersama save run (#82).
- **Save dan load:** `buildableId` sudah menunjukkan tier, dan `currentHP` menunjukkan kerusakan. `StructureVisual` menghitung tahap saat `OnEnable`, tanpa efek. Tidak disimpan: `nextSwingTime`, `upgradeTimer`, dan `upgradeTargetId`. `LastDamageTime` diatur ke saat dimuat dikurangi jeda terpanjang, supaya perbaikan langsung tersedia setelah load.
- **Struktur di region tidak dimuat:** `BaseHealth` tetap menerima damage dari #90. Tampilan menyusul saat `OnEnable`.
- **Struktur hancur saat ditahan untuk upgrade:** `StructureRegistry.Find` mengembalikan `null`, lalu proses batal tanpa mengurangi bahan.
- **Bahan habis di tengah perbaikan:** langkah 5 menghentikan ayunan berikutnya. HP yang sudah pulih tetap ada.
- **Palu rusak saat memperbaiki:** alur item hilang di #60 memanggil `OnUnequip`, dan hitungan batal.
- **Inventori berisi bahan di peti:** hanya inventori pemain yang dihitung. Peti (#46) tidak dipakai langsung.
- **Upgrade dinding bertingkat:** grid dan `level` dipindahkan tanpa mengubah dukungan, jadi tidak ada runtuhan. `ReplaceInstance` tidak memicu `StructureDestroyedEvent`.
- **HP hasil upgrade:** rasio mengikuti `currentHP` lama, dan dibatasi `maxHP` baru.
- **Bongkar setelah upgrade:** biaya kembali memakai `cost` tier baru, yaitu data `Wall_Stone` dengan `cost` 12 `Mat_Wood` dan 10 `Mat_Stone`. Dengan begitu, nilai upgrade ikut dihitung.
- **Banyak struktur turun tahap sekaligus:** efek masuk lewat pool dengan batas `maxCollapseVfxPerSecond` (#79).
- **Pengaruh terhadap base di dalam air atau pantai:** tidak ada, karena bangunan tidak berdiri di pasir.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** gempa merusak dinding yang belum di-upgrade (GDD bagian 6). Fungsi `BaseHealth.Repair(float amount)` milik issue ini (`DamagePipeline.md`). Klik kiri dipakai item aktif (`GLOSSARY.md`).

**Keputusan teknis di blueprint ini:**

- Tampilan memakai `sharedMaterial` per tahap, supaya batch tetap rendah.
- Upgrade mengganti prefab, tetapi mempertahankan `structureId`.
- `StructureVisual` mendengar event per objek.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Biaya perbaikan:** 1 bahan per ayunan, dan tiap ayunan memulihkan 15 persen HP maksimum.
2. **Perbaikan saat diserang:** ditolak selama 2 detik setelah menerima damage. Rekomendasi: pakai, supaya pemain tidak bisa menahan satu dinding hanya dengan menekan klik kiri.
3. **Tingkat material:** dua tingkat, kayu dan batu. Dinding batu berisi 900 HP, tahan api, dan tahan gempa.
4. **Biaya upgrade:** dinding 10 batu, dan pintu 8 batu.
5. **Pintu ikut bisa di-upgrade.** Checklist hanya menyebut dinding. Rekomendasi: pakai, supaya base batu tidak punya satu titik lemah kayu.
6. **Lantai, atap, dan tangga tidak bisa di-upgrade** pada versi awal.
7. **HP setelah upgrade:** mengikuti rasio HP sebelumnya.
8. **Syarat resep:** upgrade terbuka lewat `TechTreeManager`, dengan node awal mengikuti #77. Sampai #77 selesai, `unlockNodeId` dikosongkan.

## 6. Skenario Tes

1. Rusak dinding kayu ke 40 persen lewat `DevConsole`, bawa palu dan kayu, lalu bidik dinding. **Hasil:** prompt `Perbaiki (1 Kayu) 40%` tampil.
2. Tahan klik kiri. **Hasil:** tiap 0,6 detik HP naik 15 persen dan kayu berkurang 1, sampai HP penuh, lalu teks `Sudah utuh`.
3. Coba perbaiki tanpa kayu. **Hasil:** ditolak, dengan nama bahan yang kurang.
4. Biarkan zombie memukul dinding sambil memperbaiki. **Hasil:** perbaikan ditolak selama 2 detik setelah tiap pukulan.
5. Bunuh zombie, tunggu 2 detik, lalu perbaiki. **Hasil:** berhasil.
6. Turunkan HP dinding dari 100 persen ke 10 persen. **Hasil:** model berubah pada 66 persen dan 33 persen, masing-masing dengan efek satu kali.
7. Perbaiki sampai penuh. **Hasil:** model kembali bersih tanpa efek retak.
8. Pilih mode Upgrade, lalu tahan klik kiri pada dinding kayu selama 1 detik dengan 10 batu. **Hasil:** menjadi dinding batu, 10 batu berkurang, dan `StructureUpgradedEvent` terkirim sekali.
9. Lepas klik kiri pada 0,5 detik. **Hasil:** batal, tanpa biaya.
10. Upgrade dinding berisi 50 persen. **Hasil:** dinding batu berisi 450 HP dengan tahap retak yang sesuai.
11. Upgrade saat NavMesh debug terbuka. **Hasil:** tidak ada celah jalur pada frame apa pun.
12. Upgrade dinding penopang atap. **Hasil:** atap tidak runtuh.
13. Jalankan gempa lewat `DevConsole` pada base campuran. **Hasil:** dinding kayu rusak, dan dinding batu tidak berubah.
14. Tembak dinding batu dan kayu. **Hasil:** efek impact berbeda sesuai `SurfaceTag`, dan HP tetap tidak berubah.
15. Simpan dengan dinding batu ber-HP 60 persen, keluar, lalu lanjutkan. **Hasil:** tier, HP, dan tampilan sama, tanpa efek retak saat load.
16. Mati, lalu mulai run baru. **Hasil:** tidak ada bangunan lama.
17. Pukul satu dinding di base 200 struktur sambil membuka Profiler. **Hasil:** hanya satu `StructureVisual` bekerja, tanpa alokasi GC.
