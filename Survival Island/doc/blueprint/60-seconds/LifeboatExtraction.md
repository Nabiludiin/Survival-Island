# Blueprint: LifeboatExtraction.cs
**Lokasi:** `Survival Island/doc/blueprint/60-seconds/LifeboatExtraction.md`
**Deskripsi Utama:** Sekoci sebagai titik aman dan titik setor barang, jeda sebelum Fase 1, aturan hasil saat waktu habis, ringkasan hasil, dan kedatangan di pantai.
**Issue:** #74
**Rujukan GDD:** Bagian 3 (Fase 1, Base Item, prioritas barang dan NPC, serta kembali ke Fase 1 saat kalah). Sekoci, aturan setor, dan akibat gagal adalah usulan, karena GDD belum punya padanan shelter (`Survival Island/doc/CATATAN_MEKANIK.md` bagian 1).

## 1. Arsitektur & Variabel Utama

### Pola

`LifeboatExtraction : MonoBehaviour, IInteractable` berada di objek Sekoci di scene `ShipPhase` (#82), pada layer `Interactable`. Komponen ini mengatur alur Fase 1 dari pra-game sampai ringkasan. Timer tetap milik `ShipTimerManager`, dan bahaya milik `ShipHazardManager` (#75).

```csharp
public enum ShipFlowState { PreGame, Scavenge, Resolving, Summary, Finished }
// ShipControlLevel didefinisikan di ShipPlayerController.md (pemilik menurut GLOSSARY.md), tidak di sini

[Serializable]
public struct SavedItemData { public string itemId; public int amount; }

[Serializable]
public class ShipOutcomeData
{
    public List<string> rescuedNpcIds;
    public List<string> lostNpcIds;
    public float hpOnExit;
    public float infectionOnExit;
    public float secondsLeftOnExit;
    public bool departedEarly;
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `LifeboatExtraction` | `null` | Singleton per scene |
| `State` | `ShipFlowState` | `PreGame` | Tahap alur |
| `depositedItems` | `List<SavedItemData>` | Kosong | Barang di sekoci |
| `rescuedNpcIds` | `List<string>` | Kosong | NPC di sekoci |
| `safePoint` | `Transform` | Dari scene | Titik tengah area aman |
| `runSummaryPanel` | `UIPanel` | Dari scene | Panel `RunSummary` (#83) |

`GameManager` menyimpan hasil lintas scene: `savedShipLoot` (`List<SavedItemData>`), `shipOutcome` (`ShipOutcomeData`), dan `hasArrivedOnIsland` (`bool`, nilai awal `false`).

### Tangan pemain

Data tangan berada di `InventoryManager` instance `ShipPhase`, sesuai `ShipInteractable.md`.

| Anggota | Fungsi |
|---|---|
| `int ShipHandWeight`, `int ShipHandCapacity` | Berat saat ini dan batasnya |
| `bool AddShipHandItem(string id, int weight, int amount)` | Menambah barang ke tangan |
| `List<ShipHandEntry> ShipHandItems` | Isi tangan, berupa `itemId`, `amount`, dan `isNpc` |
| `void ClearShipHand()` | Mengosongkan tangan |

Setiap perubahan menerbitkan `ShipHandChangedEvent { weight, capacity }`, untuk HUD.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.ship.durationSeconds` | 60 | Durasi scavenge, sesuai GDD bagian 3 |
| `GameBalanceConfig.ship.preGameSeconds` | 3 | Jeda sebelum kontrol dibuka, di luar 60 detik |
| `GameBalanceConfig.ship.sirenAtSeconds` | 1,5 | Sirene pada jeda pra-game |
| `GameBalanceConfig.ship.handCapacity` | 4 | Batas berat di tangan |
| `GameBalanceConfig.ship.lifeboatSafeRadius` | 6 | Radius area aman |
| `GameBalanceConfig.ship.lifeboatSafeHeight` | 2,5 | Selisih tinggi maksimum, supaya dek lain tidak terhitung |
| `GameBalanceConfig.ship.departHoldSeconds` | 1,5 | Waktu menahan `E` untuk berangkat |
| `GameBalanceConfig.ship.arrivalNpcSpacing` | 2 | Jarak antar-NPC di pantai |

## 2. Detail Logika & Fungsi

### Jeda pra-game

**`IEnumerator PreGameRoutine()`**, dimulai saat `RunLifecycle.State == InShip` dan panel `Cinematic` (#99) sudah tertutup:

1. `State = PreGame`.
2. `ShipPlayerController.SetControlLevel(ShipControlLevel.LookOnly)`. Pemain hanya bisa menengok, dan `PlayerInteractor` mati.
3. `SurvivalStats.SetDrainEnabled(false)`, karena lapar dan haus tidak berjalan di kapal (#111).
4. Timer menampilkan `60`, tanpa berjalan.
5. Tunggu `sirenAtSeconds`, lalu mainkan sirene dan tandai sekoci di HUD.
6. Tunggu sampai total `preGameSeconds`.
7. `SetControlLevel(Full)`, `ShipTimerManager.StartCountdown()`, `ShipHazardManager.Begin()`, lalu `State = Scavenge`.

Semua penantian memakai `Time.deltaTime`, sehingga panel `Pause` ikut menahannya. `ShipTimerManager` tidak lagi menghitung mundur sejak `Update` pertama.

### Sekoci sebagai titik setor

**`GetPrompt()`**: `Setor Barang` kalau `ShipHandWeight > 0`. Kalau kosong, `Tahan untuk Berangkat`.

**`HoldSeconds`**: 0 kalau tangan berisi, dan `departHoldSeconds` kalau tangan kosong.

**`CanInteract`**: `None` saat `State == Scavenge`. Selain itu `Locked`.

**`Interact(in InteractContext ctx)`**:

- **Tangan berisi:** panggil `DepositHand()`.
- **Tangan kosong:** panggil `RequestDepart()`.

**`private void DepositHand()`**, aturan setor mengikuti pola 60 Seconds:

1. Untuk tiap isi `ShipHandItems`:
   - Barang: gabungkan ke `depositedItems`, dengan menjumlahkan `amount` per `itemId`.
   - NPC: tambahkan ke `rescuedNpcIds`, lalu terbitkan `NPCRescuedEvent { npcId }`.
2. Panggil `InventoryManager.ClearShipHand()`. **Seluruh slot tangan kosong sekaligus.**
3. Mainkan SFX, lalu tampilkan isi sekoci di HUD.

Barang di sekoci aman, dan tidak bisa hilang karena bahaya.

### Warna highlight saat tangan penuh

`ShipInteractable.CanInteract` mengembalikan `HandsFull` kalau `ShipHandWeight + weightCost > ShipHandCapacity`. `InteractHighlight` (#17) lalu memakai warna `Blocked`, sehingga barang merah berarti tidak muat. Prompt menampilkan `Tangan penuh. Setor ke Sekoci`, dan HUD menyorot penanda sekoci.

### NPC

NPC diperlakukan sebagai barang dengan berat besar, seperti `Survival Island/doc/CATATAN_MEKANIK.md`.

- **Penempatan:** NPC muncul di titik dari `RunSeedManager.CurrentLayout.shipNpcIds` dan `shipNpcPointByNpc` (#89). `SpawnShipNpcs()` membuatnya saat scene mulai: untuk tiap NPC, `ObjectPooler.Instance.SpawnFromPool<ShipInteractable>("ShipNpc_Generic", titik.position, titik.rotation)`, lalu langsung `Setup(npcData)` (`ShipInteractable.md`). Data tidak diisi di `OnSpawned`.
- **Berat:** 4, sama dengan `ship.handCapacity`, sehingga satu NPC memenuhi tangan.
- **Saat dibawa:** model NPC disembunyikan, dan HUD menampilkan ikon NPC bersama pemain. Tidak ada agen tambahan yang bisa tersangkut atau diserang di kapal.
- **Setelah disetor:** masuk `rescuedNpcIds`, dan `NPCRescuedEvent` terkirim.
- **Tertinggal:** NPC di `shipNpcIds` yang tidak ada di `rescuedNpcIds` saat hasil dihitung, termasuk yang masih dibawa di luar area aman, masuk `lostNpcIds`, dan dihitung sebagai korban.

`NPCDiedEvent` tidak dikirim untuk korban kapal, karena mereka belum menjadi NPC aktif di pulau (#76). Daftar korban dipakai layar ringkasan dan #99.

### Pengecekan posisi saat waktu habis

**`public void HandleTimeUp()`**, dipanggil `ShipTimerManager.ExecuteTimeUp()`:

1. Kalau `State != Scavenge` atau `GameManager.Instance.IsRunEnding`, abaikan.
2. `State = Resolving`, dan `SetControlLevel(None)`.
3. `bool safe = IsPlayerInSafeZone()`.
4. **Aman:** panggil `DepositHand()`, lalu `ResolveOutcome(departedEarly: false)`.
5. **Tidak aman:** `State = Finished`, lalu `GameManager.Instance.TriggerGameOver(DeathReason.LeftOnShip)`.

```csharp
bool IsPlayerInSafeZone()
{
    Vector3 d = player.position - safePoint.position;
    if (Mathf.Abs(d.y) > lifeboatSafeHeight) return false;
    return new Vector2(d.x, d.z).sqrMagnitude <= lifeboatSafeRadius * lifeboatSafeRadius;
}
```

Pemeriksaan memakai jarak langsung, bukan trigger, supaya hasilnya pasti pada frame waktu habis. HUD menampilkan garis area aman di lantai dek sejak `T=15`.

**Barang di tangan saat waktu habis:** **ikut** kalau pemain berada di area aman, karena langkah 4 menyetorkannya otomatis. Kalau pemain tidak aman, hasilnya kalah, jadi barang tidak relevan.

**Akibat gagal mencapai sekoci:** **permadeath langsung,** dengan `DeathReason.LeftOnShip`. Alurnya sama seperti kematian lain di #82: sinematik, layar `GameOver`, lalu kembali ke `ShipPhase`. Biaya mengulang kecil, karena Fase 1 hanya 60 detik.

### Berangkat lebih awal

**`private void RequestDepart()`**, setelah `E` ditahan selama `departHoldSeconds` dengan tangan kosong:

1. `State = Resolving`, dan `SetControlLevel(None)`.
2. `ShipTimerManager.StopCountdown()`.
3. `ResolveOutcome(departedEarly: true)`.

Pemain boleh berangkat sebelum waktu habis. Barang dan NPC yang belum disetor tertinggal, jadi keputusan ini punya harga yang jelas. Tombol harus ditahan, supaya tidak terjadi tanpa sengaja saat menyetor.

### Menyusun hasil

**`private void ResolveOutcome(bool departedEarly)`**:

1. `SurvivalStats.SetInvulnerable(true)`, supaya serangan pada frame terakhir tidak mengubah hasil.
2. `ShipHazardManager.StopAll()`.
3. Isi `lostNpcIds`.
4. Isi `ShipOutcomeData`, termasuk `hpOnExit`, `infectionOnExit`, dan `secondsLeftOnExit`.
5. `GameManager.Instance.SaveShipOutcome(depositedItems, outcome)`.
6. `State = Summary`, lalu `UIFocusStack.Instance.Push(runSummaryPanel)`.

Fungsi `GameManager.SaveFase1Loot(List<Item>)` diganti `SaveShipOutcome`. `ShipTimerManager` tidak lagi menyimpan inventori dan tidak memuat scene.

### Ringkasan hasil Fase 1

`RunSummaryPanel : UIPanel` memakai panel `RunSummary` (#83): menjeda waktu, melepas kursor, tidak bisa ditutup Esc, dan terkunci.

| Bagian | Isi |
|---|---|
| Judul | `Sekoci Diturunkan` |
| Barang | Daftar item dan jumlah setelah konversi (#118) |
| Selamat | Nama dan skill NPC di `rescuedNpcIds` (#76) |
| Korban | Nama NPC di `lostNpcIds` |
| Kondisi | HP, dan infeksi kalau lebih dari 0 |
| Waktu | Sisa detik, kalau berangkat lebih awal |
| Tombol | `Lanjut` |

**Tombol `Lanjut`:** `State = Finished`, `UIFocusStack.Close(runSummaryPanel, UICloseReason.User)`, lalu `RunLifecycle.Instance.EnterIslandPhase()`. Panel memakai latar gelap penuh, jadi tidak perlu fade terpisah. Animasi memakai waktu tak terskala.

### Kedatangan di pantai

`IslandArrival` berada di scene `IslandPhase`, di pantai selatan Region 1 (peta di #88).

| Field | Fungsi |
|---|---|
| `landingPoint` | Posisi awal pemain, di pasir dekat bangkai sekoci |
| `lookTarget` | Arah pandang awal, menghadap hutan Region 1 |
| `npcOrigin` | Titik asal NPC, berjarak `arrivalNpcSpacing` |
| `overflowOrigin` | Titik asal barang sisa, di samping bangkai sekoci |
| `LandingPosition` | Properti untuk `RunLifecycle` |

**`public void Apply()`**, dipanggil `RunLifecycle.EnterIslandPhase()` setelah `LoadInitialRegions`, dan sebelum `SaveRequestedEvent`:

1. Kalau `GameManager.hasArrivedOnIsland` bernilai `true`, abaikan. Jalur `ContinueRun` tidak melewati fungsi ini.
2. Tempatkan pemain di `landingPoint`, lalu arahkan kamera ke `lookTarget`.
3. `SurvivalStats`: `HP = shipOutcome.hpOnExit`, `Infection = shipOutcome.infectionOnExit`, dan status lain penuh.
4. `InventoryManager`: tambahkan `Base_Kail`, `Base_Pistol`, dan `Base_Pisau`, lalu `Med_Vaccine` sebanyak `GameBalanceConfig.infection.startingVaccineDoses` (2) (#64), sebelum `ApplyShipLoot`. **Langkah ini satu-satunya pemilik item inti dan vaksin awal.** `ApplyShipLoot` hanya memasukkan isi `savedShipLoot`.
5. `CarryCapacity.ApplyShipLoot(savedShipLoot)` (#118). Yang tidak muat menjadi `WorldItem` di `overflowOrigin`, dan tidak hilang otomatis.
6. `NPCRoster.SpawnRescued(shipOutcome.rescuedNpcIds, npcOrigin)` (#76).
7. `GameManager.savedShipLoot.Clear()`, lalu `GameManager.hasArrivedOnIsland = true`. Daftar dikosongkan di sini supaya barang kapal tidak bisa diterapkan dua kali.
8. `SurvivalStats.SetInvulnerable(false)`, dan lapar serta haus aktif.

**Posisi awal berada di pantai,** karena GDD menyebut pemain berasal dari kapal yang karam, dan #71 memakai pantai untuk kargo. Pemain tetap tidak boleh membangun di pasir (GDD bagian 4).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NPCRescuedEvent`, dan `ShipHandChangedEvent` lewat `InventoryManager`.

**Event yang didengar:** `RunEndedEvent`, untuk menghentikan alur saat pemain mati. Jeda tidak perlu didengar, karena `Time.deltaTime` sudah mengikutinya.

| Sistem | Hubungan |
|---|---|
| `ShipTimerManager` | `StartCountdown`, `StopCountdown`, dan memanggil `HandleTimeUp` |
| `ShipHazardManager` (#75) | `Begin` dan `StopAll` |
| `ShipPlayerController` | `SetControlLevel` |
| `ShipInteractable` | `CanInteract`, dan `weightCost` dari `ItemData.shipWeight` |
| `PlayerInteractor`, `InteractHighlight` (#17) | Interaksi Sekoci dan warna `Blocked` |
| `InventoryManager` | Tangan pemain |
| `GameManager` | `SaveShipOutcome`, `TriggerGameOver`, dan data lintas scene |
| `RunLifecycle` (#82) | `EnterIslandPhase`, dan alur akhir run |
| `UIFocusStack` (#83) | Panel `RunSummary` |
| `RunSeedManager` (#89) | NPC dan titiknya |
| `CarryCapacity` (#118) | Konversi barang ke inventori pulau |
| `NPCRoster` (#76) | Data dan kemunculan NPC di pantai |
| `InfectionSystem` (#64) | Vaksin awal |
| `NarrativeSequencer` (#99) | Adegan sekoci, dan isi `lostNpcIds` untuk ending |
| `LootSpawnerSystem` | Memunculkan barang kapal |

**Batas pekerjaan:** bahaya di kapal di #75, dan perilaku NPC di pulau di #76.

## 4. Kasus Khusus

- **Objek pool:** `LootSpawnerSystem` di `ShipPhase` memakai pool `ShipLoot_Generic` berisi `ShipInteractable`, bukan `WorldItem`. `ShipInteractable.Interact` memanggil `ObjectPooler.ReturnToPool`, menggantikan `Destroy(gameObject)`. `OnReturned` mengembalikan collider, renderer, dan highlight.
- **Game dijeda:** `Pause` menjeda timer, bahaya, dan pra-game. Ringkasan sendiri menjeda waktu, jadi bahaya tidak berjalan.
- **Permadeath:** `savedShipLoot`, `shipOutcome`, dan `hasArrivedOnIsland` dibersihkan `GameManager.ResetForNewRun` (#82). `ShipPhase` tidak punya save.
- **Save dan load:** Fase 1 tidak disimpan. Save pertama dibuat setelah `IslandArrival.Apply()`. Yang tersimpan adalah `hasArrivedOnIsland`, `lostNpcIds`, dan `rescuedNpcIds` lewat `GameManager`, dan barang sudah berada di inventori atau `RegionStateStore`. `savedShipLoot` dikosongkan setelah `Apply`.
- **Mati pada frame yang sama dengan waktu habis:** `TriggerGameOver` memasang `IsRunEnding`, dan `HandleTimeUp` mengabaikan kalau `GameManager.IsRunEnding`. Sebaliknya, `SetInvulnerable(true)` mencegah kematian setelah hasil aman.
- **Air menutup sekoci:** sekoci berada di dek atas, dan air tidak mencapainya sebelum `T=0` (#75).
- **Zombie di dekat sekoci saat setor:** tetap bisa menyerang. Setor tidak membuat kebal.
- **Tangan penuh dan menahan `E` pada sekoci:** tekanan pertama menyetor. Tahan sekali lagi untuk berangkat, karena `holdTimer` direset.
- **Pemain keluar ke menu saat di kapal:** mengikuti `SaveAndQuitToMenu` di #82, dan run ini hilang.
- **Tidak ada NPC pada seed tertentu:** `lostNpcIds` kosong, dan bagian Korban disembunyikan.
- **Waktu habis saat `Pause` terbuka:** tidak mungkin, karena timer berhenti.
- **Menyetor barang kosong:** `Interact` tidak memanggil `DepositHand` saat tangan kosong.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** kalah mengembalikan pemain ke Fase 1 (GDD bagian 3). Item inti selalu ada (GDD bagian 3 dan `GLOSSARY.md`). `DeathReason.LeftOnShip` (`GLOSSARY.md`). `EnterIslandPhase`, panel `RunSummary`, dan tanpa save di Fase 1 (#82, #83). Berat makanan 1, senjata 2, dan NPC 4 (`ShipInteractable.md`).

**Keputusan teknis di blueprint ini:**

- Sekoci memakai `IInteractable`, seperti objek lain.
- Posisi aman dicek dengan jarak, bukan trigger.
- `IslandArrival` dijalankan `RunLifecycle`, supaya urutannya pasti sebelum save pertama.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Sekoci sebagai titik aman dan titik setor:** di dek atas. Menyetor mengosongkan seluruh tangan.
2. **Gagal mencapai sekoci:** **permadeath langsung** dengan `LeftOnShip`. Pilihan lain adalah tiba di pulau tanpa barang tambahan. Rekomendasi ini mengikuti 60 Seconds dan membuat waktu terasa berbahaya.
3. **Barang di tangan saat waktu habis:** ikut kalau pemain berada di area aman.
4. **Berangkat lebih awal:** boleh, dengan menahan `E` 1,5 detik saat tangan kosong. **Keputusan developer:** "persis 60 detik" di GDD bagian 3 dibaca sebagai batas waktu paling lama, bukan durasi wajib.
5. **Jeda pra-game:** 3 detik di luar 60 detik. Pemain hanya bisa menengok, sirene di detik ke-1,5, lalu kontrol dibuka.
6. **Kapasitas tangan:** 4 berat. Satu NPC memenuhi tangan.
7. **NPC dibawa:** model disembunyikan dan tampil sebagai ikon, bukan agen yang mengikuti.
8. **NPC tertinggal:** menjadi korban, dan namanya tampil di ringkasan.
9. **Kedatangan:** pemain di pantai selatan Region 1. Barang langsung di inventori, sisanya di samping bangkai sekoci, dan NPC di dekat pemain. HP dan infeksi dibawa, dan status lain penuh.
10. **Waktu awal di pulau:** jam 06:00 hari pertama, sesuai `DayNightDirector.md`.

## 6. Skenario Tes

1. Masuk `ShipPhase`. **Hasil:** selama 3 detik pemain hanya bisa menengok, sirene berbunyi di detik ke-1,5, dan timer `60` baru berjalan setelah kontrol dibuka.
2. Ambil barang berat 1 sebanyak 4 kali. **Hasil:** barang kelima menyala merah dengan pesan `Tangan penuh. Setor ke Sekoci`, dan tidak terambil.
3. Tekan `E` di sekoci. **Hasil:** tangan kosong, HUD berat kembali 0, dan isi sekoci bertambah.
4. Ambil NPC. **Hasil:** tangan penuh, NPC hilang dari dunia, dan ikon tampil. Setor. **Hasil:** `NPCRescuedEvent` terkirim.
5. Tunggu waktu habis di area aman dengan barang di tangan. **Hasil:** barang ikut, dan ringkasan tampil.
6. Tunggu waktu habis di dek bawah. **Hasil:** `RunEndedEvent` dengan `LeftOnShip`, layar `GameOver`, dan tombol Ulangi kembali ke `ShipPhase`.
7. Berdiri di dek tengah tepat di bawah sekoci saat waktu habis. **Hasil:** tidak aman, karena selisih tinggi melebihi 2,5 meter.
8. Tahan `E` di sekoci selama 1,5 detik dengan tangan kosong pada `T=40`. **Hasil:** timer berhenti, dan ringkasan menampilkan sisa 40 detik.
9. Lepas `E` pada 1 detik. **Hasil:** tidak berangkat.
10. Tinggalkan satu NPC. **Hasil:** namanya tampil di bagian Korban.
11. Tekan Esc di ringkasan. **Hasil:** tidak tertutup, dan `Time.timeScale` tetap 0.
12. Klik `Lanjut`. **Hasil:** `IslandPhase` dimuat, pemain berada di pantai menghadap hutan, item inti ada, barang kapal masuk inventori, dan NPC berada di dekat pemain.
13. Paksa barang melebihi slot. **Hasil:** sisanya berada di samping bangkai sekoci.
14. Terluka dan terinfeksi di kapal, lalu sampai di pulau. **Hasil:** HP dan infeksi sama dengan isi ringkasan.
15. Keluar ke menu setelah tiba, lalu lanjutkan. **Hasil:** barang tidak berlipat, dan `Apply` tidak berjalan lagi.
16. Mati di kapal, lalu mulai lagi lima kali. **Hasil:** `savedShipLoot` kosong tiap kali, dan barang di kapal muncul lagi lewat pool tanpa sisa status.
17. Turunkan HP ke 0 pada `T=0,1`. **Hasil:** alur mati berjalan sekali, tanpa ringkasan.
18. Setor satu `Med_Vaccine` dari kapal, lalu tiba di pulau. **Hasil:** item inti masing-masing tepat 1, `Med_Vaccine` berjumlah 3 (2 awal ditambah 1 dari kapal), dan `GameManager.savedShipLoot` kosong setelah `Apply`.

## 7. Catatan Implementasi (Langkah 2)

- `SpawnShipNpcs()` dan `NPCRoster.SpawnRescued` belum ditulis, karena `NPCData` dan `NPCRoster` (#76) belum ada. Daftar korban tetap dihitung dari `RunSeedManager.CurrentLayout.shipNpcIds`.
- Jalur "tidak aman" saat waktu habis juga memanggil `ShipHazardManager.StopAll()` sebelum `TriggerGameOver`, sesuai `ShipTimerManager.md` ("kedua jalur hasil memanggil `StopAll`").
- `RunSummaryPanel` menampilkan ID item dan NPC apa adanya sampai `LocalizationManager` (#104) dan `NPCData` ada. Tombol `Lanjut` memanggil `LifeboatExtraction.ContinueToIsland()`.
- `IslandArrival`: `RunLifecycle.EnterIslandPhase` langkah 6 sekarang memanggil `IslandArrival.Instance.Apply()`. `GameManager.MarkArrivedOnIsland()` (Langkah 1) mengosongkan `savedShipLoot` dan mengisi `hasArrivedOnIsland`.
- `GameBalanceConfig.infection.startingVaccineDoses` (2) ditambahkan lebih dulu oleh Langkah 2. Field `infection` lain tetap milik Langkah 4.
- **Celah kontrak:** `SurvivalStats` (#111) tidak punya fungsi untuk mengisi HP langsung, jadi `HP = shipOutcome.hpOnExit` belum bisa dijalankan. `FirstPersonCamera` (#102) juga belum punya fungsi publik untuk menyamakan yaw dan pitch ke `lookTarget`. Keduanya ditandai TODO dan perlu diputuskan saat Langkah 3 digabung.
- Sirene pra-game, SFX setor, penanda sekoci, dan garis area aman di HUD belum ditulis (TODO).
