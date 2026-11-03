# Blueprint: ResourceRespawn.cs
**Lokasi:** `docs/blueprint/raft/ResourceRespawn.md`
**Deskripsi Utama:** Aturan alat panen, status habis pada `ResourceNode`, jadwal tumbuh kembali berbasis hari game, kepadatan node per region, dan item jatuh yang memakai pool.
**Issue:** #73
**Rujukan GDD:** Bagian 1 (target 60 hari, dan run bisa berlanjut sesudahnya), bagian 5 (kayu dan batu Region 1, jembatan 500 kayu, material medis Region 2). Tabel alat dan jadwal tumbuh adalah usulan.

## 1. Arsitektur & Variabel Utama

### Pola

`ResourceNode` tetap berada di tiap pohon, batu, dan tanaman yang diletakkan di scene region, memakai layer `Obstacle`, dan menjadi `ISaveable` dengan `SaveIdTag` (#80). `Destroy(gameObject)` dihapus: node hanya berganti status. `ResourceRespawn` adalah manager di `IslandPhase` yang memeriksa node habis di region yang sedang dimuat.

```csharp
public enum ToolClass { None, Axe, Pickaxe, Knife }
public enum NodeStatus { Standing, Depleted }

[Serializable]
public struct ToolRule { public ToolClass tool; public int damage; }  // 0 berarti ditolak

[CreateAssetMenu(menuName = "Survival/Resource Node")]
public class ResourceNodeData : ScriptableObject
{
    public string nodeTypeId;        // Contoh: Node_Pine
    public NodeType nodeType;        // Tree, Rock, MedicalHerb
    public int maxHits = 5;
    public ToolRule[] toolRules;
    public ItemData dropItem;
    public int dropMin, dropMax;
    public bool keepColliderWhenDepleted;
    public RegrowRule[] regrow;      // Hari per region
}

[Serializable]
public struct RegrowRule { public RegionType region; public float days; }

public interface IRegrowable
{
    bool IsDepleted { get; }
    float RegrowAtGameHour { get; }
    Vector3 Position { get; }
    void Regrow();
}
```

`ItemData` mendapat field `toolClass`, dengan nilai awal `None`. `ToolClass.None` berarti tangan kosong, atau item yang bukan alat panen seperti Kail.

| Variabel `ResourceNode` | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Status` | `NodeStatus` | `Standing` | Status node |
| `currentHits` | `int` | `maxHits` | Sisa pukulan |
| `RegrowAtGameHour` | `float` | -1 | Waktu tumbuh kembali, dalam `TotalGameHours` |
| `standingVisual`, `depletedVisual` | `GameObject` | Dari prefab | Pohon utuh, serta tunggul atau puing |

### Tabel alat dan node

Isi sel adalah damage per ayunan. Nilai 0 berarti ditolak, tanpa pengurangan durabilitas.

| Node | `maxHits` | Tangan | Kapak | Beliung | Pisau | Alat minimum |
|---|---|---|---|---|---|---|
| Pohon biasa | 5 | 1 | 3 | 1 | 1 | Tidak ada |
| Batu besar | 6 | 0 | 1 | 3 | 0 | Kapak atau Beliung |
| `Node_Mushroom` | 1 | 1 | 1 | 1 | 1 | Tidak ada |
| `Node_SapTree` | 2 | 0 | 0 | 0 | 1 | Pisau |

Kapak butuh 2 ayunan per pohon, sesuai `GameBalanceConfig.md`. Pohon getah tidak bisa ditebang. Batu pertama untuk membuat kapak diambil dari `PickupInteractable` (#17), bukan dari node ini.

### Jadwal tumbuh kembali

| Node | Region 1 | Region 2 | Region 3 | Region 4 | Saat habis |
|---|---|---|---|---|---|
| Pohon biasa | 5 hari | 7 hari | 10 hari | 8 hari | Tunggul, collider tetap |
| Batu besar | 10 hari | 10 hari | 12 hari | 10 hari | Puing, collider tetap |
| `Node_Mushroom` | Tidak ada | 1 hari | Tidak ada | Tidak ada | Mesh dan collider mati |
| `Node_SapTree` | Tidak ada | 2 hari | Tidak ada | Tidak ada | Pohon tetap, batang tampak kering, dan tidak bisa dipanen |

Node `Node_SapTree` adalah satu-satunya yang memakai `depletedVisual` berbentuk sama, karena pohonnya tidak ditebang.

### Kepadatan per region

| Region | Pohon | Batu besar | Jamur | Pohon getah | Alasan |
|---|---|---|---|---|---|
| 1 | Minimal 120 | Minimal 40 | 0 | 0 | 50 pohon untuk jembatan, sisanya untuk base dan alat |
| 2 | Minimal 60 | Minimal 20 | 10 | 8 | Material medis tersebar supaya pemain harus berkeliling |
| 3 | Minimal 20 | Minimal 40 | 0 | 0 | Kayu langka di area salju |
| 4 | Minimal 30 | Minimal 20 | 0 | 0 | Cukup untuk perbaikan darurat |

Dari 120 pohon Region 1, minimal 80 berada dalam 200 meter dari titik mulai. Dengan rata-rata 10 kayu per pohon, panen pertama menghasilkan 1.200 kayu, dan jembatan butuh 500. Satu siklus 5 hari kembali menyediakan kayu, jadi Region 1 tidak bisa kehabisan permanen. `ResourceAuditWindow` di Editor menghitung node per region dan membandingkannya dengan tabel ini, memakai `resource.minNodesPerRegion`.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.resource.treeDropMin` / `treeDropMax` | 8 / 12 | Sudah ada |
| `GameBalanceConfig.resource.rockDropMin` / `rockDropMax` | 4 / 6 | `Mat_Stone` per batu |
| `GameBalanceConfig.resource.axeDecayPerUse` | 1 | Sudah ada |
| `GameBalanceConfig.resource.pickaxeDecayPerUse` | 1 | Durabilitas beliung |
| `GameBalanceConfig.resource.knifeDecayPerHarvest` | 0,5 | Durabilitas pisau saat memanen |
| `GameBalanceConfig.resource.coconutRespawnDays` | 3 | Dipakai `HydrationSystem.md` |
| `GameBalanceConfig.resource.regrowJitterHours` | 6 | Variasi waktu tumbuh |
| `GameBalanceConfig.resource.regrowMinPlayerDistance` | 25 | Jarak minimum dari pemain, dalam meter |
| `GameBalanceConfig.resource.regrowClearRadius` | 3 | Radius bebas bangunan |
| `GameBalanceConfig.resource.regrowChecksPerFrame` | 20 | Batas pemeriksaan per frame |
| `GameBalanceConfig.resource.dropChunkSize` | 4 | Isi maksimum satu `WorldItem` |
| `GameBalanceConfig.resource.dropSettleSeconds` | 3 | Waktu sebelum posisi item diperiksa |
| `GameBalanceConfig.resource.dropMaxFallMeters` | 4 | Batas turun item dari pangkal node |
| `GameBalanceConfig.resource.minNodesPerRegion` | Tabel kepadatan | Dibaca `ResourceAuditWindow` |

## 2. Detail Logika & Fungsi

### Memanen

**`public HitResult ReceiveHit(ToolClass tool, Vector3 hitPoint)`** menggantikan `ReceiveHit(int, string)`. Yang memanggil adalah `MeleeCombatSystem`, setelah `PhysicsMasks.Melee` mengenai node.

1. Kalau `Status == Depleted`, kembalikan `Rejected`. Mainkan suara tumpul.
2. Cari damage alat di `toolRules`. Kalau 0, tampilkan notifikasi `Butuh alat lain` (#22), lalu kembalikan `Rejected`.
3. `currentHits -= damage`, lalu mainkan partikel, suara, dan goyangan.
4. Kalau `currentHits <= 0`, panggil `Deplete()`.
5. Kembalikan `Hit`. `MeleeCombatSystem` mengurangi durabilitas hanya saat hasilnya `Hit`.

### Status habis

**`private void Deplete()`**, pengganti `ShatterNode()`:

1. Hitung jumlah drop dengan `ref RunSeedManager.GetStream(RandomStreamId.Gameplay)`.
2. Panggil `SpawnDrops(total)`.
3. `Status = Depleted`. Tukar `standingVisual` ke `depletedVisual`. Kalau `keepColliderWhenDepleted == false`, matikan collider.
4. `RegrowAtGameHour = TotalGameHours + hari * 24 + Range(0, regrowJitterHours)`, memakai aliran yang sama. Kalau tidak ada aturan untuk region ini, nilainya -1, dan node tidak tumbuh.
5. Panggil `ResourceRespawn.Instance.MarkDepleted(this)`.

**NavMesh tidak berubah:** tunggul dan puing mempertahankan collider, sehingga bake NavMesh tetap benar tanpa carving. Jamur tidak ikut bake.

**`public void ForceDeplete(bool withDrops)`** tersedia untuk #70 dan #97, supaya bencana atau api bisa menghabiskan node.

### Item jatuh

**`private void SpawnDrops(int total)`**:

1. Bagi `total` menjadi beberapa tumpukan berisi maksimum `dropChunkSize`. Pohon berhasil 10 kayu menjadi 3 objek, bukan 10.
2. Tiap tumpukan memakai `WorldItemManager.Spawn(new ItemInstance(dropItem), amount, pos, despawns: true)`, dengan pool `WorldItem_Generic`. `Instantiate` tidak dipakai.
3. Beri impuls kecil ke arah acak lewat `Rigidbody`.
4. Isi `ResourceDropGuard.Arm(safePoint)`, dengan `safePoint` berada di pangkal node.

**`ResourceDropGuard : MonoBehaviour, IPoolable`** ada di prefab `WorldItem_Generic`, dan mati kalau tidak di-`Arm`:

- **Masuk air:** `OnTriggerEnter` pada layer `Water` memindahkan item ke `safePoint` dan menghentikan kecepatannya. Node selalu berada di darat, jadi titik ini aman.
- **Tertimbun:** setelah `dropSettleSeconds`, kalau `y` item lebih rendah dari `safePoint.y - dropMaxFallMeters`, item dipindahkan ke `safePoint`. Item yang menembus Terrain akan terus jatuh, jadi pemeriksaan ini menangkapnya tanpa query fisika. Item yang menggelinding jauh di lereng curam juga kembali, dan itu tidak merugikan pemain.
- **`OnSpawned` dan `OnReturned`:** kosongkan `safePoint`, timer, dan status aktif. Pemeriksaan selesai setelah satu kali, lalu komponen mati.

### Tumbuh kembali

`ResourceRespawn` menyimpan `depleted`, yaitu `List<IRegrowable>` milik region yang dimuat.

**`void HandleHour(HourChangedEvent e)`** memulai `CheckRoutine()` kalau belum berjalan:

1. Periksa paling banyak `regrowChecksPerFrame` node per frame.
2. Lewati node dengan `RegrowAtGameHour < 0` atau yang waktunya belum tiba.
3. Panggil `CanRegrow(node)`. Kalau boleh, panggil `node.Regrow()`, lalu keluarkan dari daftar. Kalau tidak, node diperiksa lagi pada jam berikutnya.

**`private bool CanRegrow(IRegrowable node)`**:

1. Tolak kalau jarak ke pemain di bawah `regrowMinPlayerDistance`.
2. Jalankan `PhysicsQuery.OverlapSphere(node.Position, regrowClearRadius, PhysicsMasks.RegrowBlock, buffer)`. Tolak kalau ada hasil. Mask ini berisi `Structure`, `Player`, `NPC`, dan `HeavyItem`, sehingga node tidak pernah tumbuh di dalam base.

**`ResourceNode.Regrow()`**: `Status = Standing`, `currentHits = maxHits`, tampilkan `standingVisual`, nyalakan collider, dan `RegrowAtGameHour = -1`.

**Kelapa:** `CoconutPickup : PickupInteractable, IRegrowable` menjadwalkan `coconutRespawnDays` saat diambil. `Regrow()` mengosongkan `isConsumed`, lalu menampilkan buahnya.

**Batu kecil dan ranting (#116):** `StonePickup : PickupInteractable, IRegrowable` memakai pola yang sama, dengan jadwal `resource.stoneRespawnDays`. Usulan nilai awal 2 hari, belum ada di `GameBalanceConfig.md`.

- **`public int CountActiveStones(RegionType region)`** menghitung `StonePickup` yang masih tersedia di region itu.
- **`public void ForceRegrowStones(RegionType region, int minimum)`** memanggil `Regrow()` sampai jumlah aktif mencapai `minimum`.

**Api (#97):** `ResourceNode` pohon dan tanaman memasang `Flammable` dengan profil `Flame_Vegetation`. Akhir bakar memanggil `ForceDeplete(false)`.

### Mengejar waktu

Status memakai jam absolut. Saat region kembali, `RestoreState` mengisi `Status` dan `RegrowAtGameHour`, lalu node yang habis mendaftar lewat `MarkDepleted`. `MarkDepleted` memulai `CheckRoutine()` pada frame berikutnya kalau belum berjalan, jadi node yang waktunya sudah lewat langsung tumbuh. Region dimuat saat pemain masih jauh (#88), sehingga pemain tidak melihat pohon muncul mendadak.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `HourChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `MeleeCombatSystem` | Memanggil `ReceiveHit`, lalu mengurangi durabilitas saat `Hit` |
| `DurabilitySystem`, `ToolRepairSystem` (#113) | Tujuan pengurangan durabilitas |
| `ItemData` | Field `toolClass` |
| `WorldItemManager` (#66), `ObjectPooler` | Item jatuh |
| `RegionStreaming` (#88), `WorldStateSave` (#80) | `ISaveable` dan `SaveIdTag` lewat `RegionStateStore` |
| `DayNightDirector` | `TotalGameHours` dan `HourChangedEvent` |
| `RunSeedManager` (#89) | Aliran `Gameplay` |
| `HydrationSystem` (#43) | Kelapa lewat `coconutRespawnDays` |
| `InfectionSystem` (#64) | Varian node, sesuai tabel `InfectionSystem.md` |
| `DisasterManager` (#70), `FireSystem` (#97) | `ForceDeplete`, dan `Flammable` pada pohon dan tanaman |
| `ProgressionSafety` (#116) | `CountActiveStones` dan `ForceRegrowStones` |
| `NoiseEmitter` (#49) | Dipakai `MeleeCombatSystem` saat memukul node |

**Batas pekerjaan:** loot di titik tetap milik `LootSpawnerSystem`.

## 4. Kasus Khusus

- **Objek pool:** node tidak memakai pool. `WorldItem` memakai pool, dengan reset `ResourceDropGuard` di `OnSpawned` dan `OnReturned`, serta data item dari #66.
- **Game dijeda:** `HourChangedEvent` tidak terbit karena jam berhenti, dan timer `ResourceDropGuard` memakai `Time.deltaTime`. `CheckRoutine` yang sedang berjalan boleh selesai, karena hanya mengubah node yang jauh.
- **Permadeath:** `RegionStateStore` terhapus bersama save run (#82). Semua node kembali `Standing`.
- **Save dan load:** tiap node menyimpan `{ status, currentHits, regrowAtGameHour }` dengan `SaveId` stabil dari `SaveIdTag`. Daftar `depleted` tidak disimpan, karena dibangun ulang. `currentHits` pohon yang dipukul sebagian tetap tersimpan.
- **Region dilepas:** `OnDisable` melepas node dari `depleted`. Item jatuh ditulis oleh `WorldItemManager.CaptureRegion`, dan `ResourceDropGuard` berhenti.
- **Tidur:** `HourChangedEvent` terbit lebih cepat, jadi node ikut tumbuh selama tidur (#67). Node dalam 25 meter dari pemain tetap menunggu sampai pemain menjauh.
- **Struktur di atas jamur:** collider jamur mati saat habis, sehingga pemain bisa membangun di atasnya. Node tertahan oleh `CanRegrow` sampai bangunan dibongkar.
- **Tunggul menghalangi bangunan:** tunggul tetap `Obstacle`, sehingga hologram merah (#79) sampai pohon tumbuh. Membersihkan tunggul belum dirancang.
- **Item jatuh di pantai saat pasang:** diatur #91. `ResourceDropGuard` hanya mengatasi item dari node.
- **Melebihi `maxLooseWorldItems`:** mengikuti aturan #66, jadi `WorldItem` lepas tertua kembali ke pool.
- **Kapak hancur saat memukul:** `ReceiveHit` sudah selesai, dan durabilitas dikurangi setelahnya.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** hasil dan alat `Node_Mushroom` serta `Node_SapTree`, dengan jadwal 1 dan 2 hari (`InfectionSystem.md`). Hasil pohon 8 sampai 12, dan kapak 2 ayunan (`GameBalanceConfig.md`). Jembatan 500 kayu (`GLOSSARY.md`).

**Keputusan teknis di blueprint ini:**

- Node tidak dihancurkan. Status berganti, dan tunggul serta puing mempertahankan NavMesh.
- Jam absolut menggantikan `GetElapsedGameHours`, sehingga node tidak perlu menghitung selisih.
- Item jatuh digabung menjadi tumpukan, sehingga jumlah objek pool tetap rendah.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Tabel alat:** damage per alat sesuai bagian 1. Pemukulan pohon dengan tangan tetap diperbolehkan.
2. **Alat minimum:** batu besar butuh Kapak atau Beliung, dan pohon getah butuh Pisau.
3. **Jadwal tumbuh kembali:** pohon 5 sampai 10 hari, dan batu 10 sampai 12 hari, tergantung region.
4. **Aturan tumbuh:** jarak 25 meter dari pemain, dan radius 3 meter bebas bangunan.
5. **Kepadatan per region:** tabel minimum, termasuk 120 pohon di Region 1.
6. **Tanaman medis:** hanya di Region 2, dengan jumlah terbatas (10 jamur dan 8 pohon getah) yang tersebar, dan tumbuh cepat. Pemain harus kembali berkali-kali, tetapi bahan tidak pernah habis permanen. Ini menghasilkan sekitar 3 dosis per hari jika semua pohon getah dipanen, dan harus disetel bersama #64.
7. **Item masuk air:** kembali ke pangkal node, tanpa hilang.

## 6. Skenario Tes

1. Pukul pohon dengan tangan. **Hasil:** tumbang setelah 5 pukulan, tanpa durabilitas berkurang.
2. Tebang pohon dengan kapak. **Hasil:** tumbang setelah 2 ayunan, muncul tunggul, dan item jatuh 8 sampai 12 kayu dalam maksimum 3 objek.
3. Pukul batu dengan tangan. **Hasil:** notifikasi `Butuh alat lain`, dan node tidak berubah.
4. Pukul `Node_SapTree` dengan kapak. **Hasil:** ditolak, dan durabilitas kapak tetap.
5. Panen `Node_SapTree` dengan Pisau. **Hasil:** 1 sampai 2 `Mat_TreeSap`, dan pohon tetap berdiri dengan tampilan kering.
6. Tunggu 5 hari setelah menebang di Region 1 dan pergi lebih dari 25 meter. **Hasil:** pohon utuh kembali.
7. Berdiri di dekat tunggul saat waktunya tiba. **Hasil:** pohon belum muncul, dan muncul setelah pemain menjauh.
8. Bangun lantai di atas jamur yang habis. **Hasil:** jamur tidak tumbuh. Bongkar lantai. **Hasil:** tumbuh pada pemeriksaan berikutnya.
9. Tebang pohon di tepi sungai sampai item jatuh ke air. **Hasil:** item kembali ke pangkal pohon.
10. Dorong item menembus Terrain lewat `DevConsole`. **Hasil:** setelah 3 detik item kembali ke pangkal pohon.
11. Tinggalkan Region 2, tunggu 2 hari, lalu kembali. **Hasil:** jamur dan pohon getah tersedia tanpa muncul mendadak di depan pemain.
12. Simpan saat pohon sudah dipukul 1 kali dan satu pohon lain habis, lalu lanjutkan. **Hasil:** sisa pukulan dan jadwal sama.
13. Mati dan mulai run baru. **Hasil:** semua node berdiri kembali.
14. Tebang 30 pohon sambil membuka Profiler. **Hasil:** tidak ada `Instantiate` dari node, dan objek item berasal dari pool.
15. Jalankan `ResourceAuditWindow`. **Hasil:** tiap region memenuhi jumlah minimum, atau peringatan muncul.
16. Tebang 50 pohon di Region 1, sesuai lembar playtest. **Hasil:** sekitar 500 kayu dalam sekitar 3 siang penuh.
