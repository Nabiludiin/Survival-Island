# Blueprint: LoreCollectibles.cs & NPCBarks.cs
**Lokasi:** `docs/blueprint/Island_Logic/LoreCollectibles.md`
**Deskripsi Utama:** Temuan cerita opsional yang tersebar per region dan bisa dibaca ulang, serta ucapan singkat NPC yang menunjukkan kondisinya tanpa membuka menu.
**Issue:** #100
**Rujukan GDD:** Bagian 2 (pulau bekas eksperimen dan jejak militer) dan bagian 5 (Region 4 sebagai pusat lore). Sebaran temuan, petunjuk, dan ucapan NPC adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

- `LoreCollectibles` adalah manager di scene `IslandPhase`, jadi dibuat ulang setiap run.
- `LorePoint` adalah objek tetap di scene region dan mewarisi `IslandInteractable` (#17).
- `NPCBarks` adalah manager di scene `IslandPhase`. Ia mendengar event dan memeriksa NPC secara berkala.
- Diary utama dan tujuan pemain tetap milik `ObjectiveManager` (#81).

```csharp
public enum LoreKind { Note, Recording, Board }           // Catatan, rekaman, papan
public enum LoreSource { Researcher, Military, Experiment }
public enum LoreHintKind { None, PartLocation }
public enum BarkCategory { Greeting, Hurt, Hungry, ZombieIncoming }

[CreateAssetMenu(menuName = "Survival/Lore Entry")]
public class LoreEntry : ScriptableObject
{
    public string loreId;          // lore_r4_archive_01
    public string titleKey;        // lore.<loreId>.title
    public string bodyKey;         // lore.<loreId>.body
    public RegionType region;
    public LoreKind kind;
    public LoreSource source;
    public AudioClip recordingClip; // Hanya untuk Recording
    public LoreHintKind hint;       // Nilai awal None
    public int sortOrder;
}

[Serializable] public struct BarkLine { public string textKey; public AudioClip clip; } // clip boleh kosong

[CreateAssetMenu(menuName = "Survival/Bark Set")]
public class BarkSet : ScriptableObject { public BarkLine[] greeting, hurt, hungry, zombieIncoming; }
```

| Variabel | Pemilik | Tipe | Nilai awal | Fungsi |
|---|---|---|---|---|
| `allEntries` | `LoreCollectibles` | `LoreEntry[]` | Sekitar 20 aset | Daftar temuan |
| `collectedThisRun` | `LoreCollectibles` | `List<string>` | Kosong | Sudah ditemukan di run ini |
| `grantedHints` | `LoreCollectibles` | `List<string>` | Kosong | Petunjuk yang sudah diberikan |
| `archive` | `LoreCollectibles` | `HashSet<string>` | Dari file | Pernah ditemukan di run mana pun |
| `npcStates` | `NPCBarks` | `Dictionary<string,BarkRuntime>` | Kosong | Jeda dan riwayat per NPC |
| `lastGlobalBarkTime` | `NPCBarks` | `float` | -999 | Jeda antar-NPC |
| `overlapBuffer` | `NPCBarks` | `Collider[32]` | Dibuat sekali | Buffer `PhysicsQuery.OverlapSphere` |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.lore.maxPartHintsPerRun` | 2 | Batas petunjuk suku cadang per run |
| `GameBalanceConfig.barks.checkSeconds` | 2 | Jeda pemeriksaan satu NPC |
| `GameBalanceConfig.barks.globalCooldownSeconds` | 6 | Jeda antar-ucapan semua NPC |
| `GameBalanceConfig.barks.npcCooldownSeconds` | 20 | Jeda antar-ucapan satu NPC |
| `GameBalanceConfig.barks.categoryCooldownSeconds` | 90 | Jeda kategori yang sama per NPC |
| `GameBalanceConfig.barks.hearRadius` | 25 | Ucapan di luar jarak ini dilewati |
| `GameBalanceConfig.barks.greetRadius` | 6 | Jarak menyapa |
| `GameBalanceConfig.barks.greetAwaySeconds` | 120 | Lama berpisah sebelum menyapa lagi |
| `GameBalanceConfig.barks.hurtHpRatio` | 0,4 | Batas HP untuk mengeluh luka |
| `GameBalanceConfig.barks.zombieAlertRadius` | 15 | Jarak NPC menyadari zombie |

## 2. Detail Logika & Fungsi

### Sebaran temuan

Titik bersifat tetap, jadi tidak diacak `RunSeedManager` (#89). Isi naskah ditulis sebagai data, dan tidak boleh membocorkan Diary sebelum waktunya.

| Region | Catatan peneliti | Log militer | Arsip eksperimen | Tempat |
|---|---|---|---|---|
| `Volcano` | 3 catatan (pengamatan hewan dan gunung) | 1 papan pengumuman | - | Kemah rusak dan pos pengamatan |
| `Amazon` | 3 catatan (tumbuhan dan jamur) | - | 1 rekaman | Kemah peneliti dan jembatan lama |
| `Snow` | - | 4 log dan 1 papan | - | Pangkalan Militer dan pos jaga |
| `Logistic` | - | 2 log (evakuasi gagal) | 4 arsip dan 1 rekaman | Rumah runtuh (#112), balai desa, dan ruang eksperimen |

Total 20 temuan, dan Region 4 paling banyak sesuai GDD bagian 5.

### Menemukan temuan

**`LorePoint.OnInteract(in InteractContext ctx)`** memanggil `LoreCollectibles.Instance.Collect(entry)`. `HoldSeconds` bernilai 0, dan `blockedWhileCarrying = false`, karena membaca tidak butuh tangan kosong.

**`public void Collect(LoreEntry e)`:**
1. `bool isNew = !collectedThisRun.Contains(e.loreId)`.
2. Kalau baru, tambahkan ke `collectedThisRun`, lalu panggil `GrantHint(e)`.
3. `bool firstEver = archive.Add(e.loreId)`. Kalau `true`, panggil `LoreArchiveFile.Save(archive)`.
4. Kalau `isNew`, terbitkan `LoreFoundEvent { loreId, region, isFirstEver }`.
5. Panggil `UIFocusStack.Instance.Push(loreReaderPanel)` dengan isi `e`. Panel ini menjeda game.

**`private void GrantHint(LoreEntry e)`:** keluar kalau `hint = None`, atau `grantedHints` sudah memuat ID-nya. Untuk `PartLocation`, keluar kalau jumlah petunjuk sudah `maxPartHintsPerRun`. Lalu catat dan terbitkan `LoreHintGrantedEvent { loreId, hintKind, region }`.

**Petunjuk suku cadang:** `HeavyItemSystem` (#69) mendengar event itu dan membuat satu barang `Lying` yang belum `isDiscovered` menjadi `isDiscovered = true`, sehingga penanda kompas muncul. Pilihannya adalah indeks terkecil, supaya hasilnya pasti untuk seed yang sama. Teks catatan ditulis umum, karena posisi barang diacak #89.

**Rekomendasi aset petunjuk:** hanya 2 log militer Region 4 memakai `PartLocation`.

### Membaca ulang

**`LoreReaderPanel : UIPanel`** memakai pengaturan `LoreReader` di #83. Judul dan isi diambil lewat `LocalizationManager.Get`.

- **`Note` dan `Board`:** teks dengan `ScrollRect`.
- **`Recording`:** teks transkrip tampil di panel, dan `recordingClip` diputar lewat `sfxSource`, yang tetap bunyi saat jeda (`ignoreListenerPause`). `OnClosed` menghentikannya.

**Menu koleksi:** `DiaryArchivePanel` (#81, tombol `OpenDiary`) mendapat tab `Arsip`:
1. Isi tab adalah `collectedThisRun` ditambah `archive`, dikelompokkan per `RegionType`, lalu urut `sortOrder`.
2. Judul tiap kelompok menampilkan jumlah, misalnya `3/7`.
3. Temuan yang belum pernah ditemukan tidak tampil.
4. Memilih satu baris membuka `LoreReader`, tanpa memberi petunjuk lagi.

`MainMenuController` juga bisa menampilkan isi `archive` lewat `LoreArchiveFile.Load()`, sebagai tampilan di dalam panel `MainMenu`. Menu utama tidak memakai `DiaryArchivePanel` (`UIPanelId.DiaryArchive`), karena panel itu hanya ada di scene `IslandPhase`.

**`LorePoint` setelah dibaca:** objeknya tetap ada dan bisa dibaca lagi dari dunia. Prompt berubah menjadi `prompt.read_again`.

### Ucapan NPC

**Pemeriksaan berkala:** coroutine `TickLoop` berjalan tiap `checkSeconds`, dan memeriksa satu NPC per putaran secara bergiliran, supaya biaya tetap kecil. Hanya NPC dengan `NPCRecord.companion != null` yang diperiksa. NPC yang dimatikan (#76) tidak berbicara.

| Kategori | Pemicu | Prioritas |
|---|---|---|
| `ZombieIncoming` | `NightfallEvent`, atau `PhysicsQuery.OverlapSphere(npcPos, zombieAlertRadius, mask layer Enemy, overlapBuffer)` menemukan tag `Zombie` | 5 |
| `Hurt` | `NPCRecord.hp / maxHp < hurtHpRatio` | 4 |
| `Hungry` | `VitalWarningChangedEvent` memuat `HungerLow` atau `Starving` milik pemain | 2 |
| `Greeting` | `PlayerBaseStateChangedEvent { isInside: true }`, atau pemain masuk `greetRadius` setelah berpisah `greetAwaySeconds` | 1 |

**Catatan `Hungry`:** `NPCRoster.md` menyatakan NPC tidak punya `Hunger`. Karena itu, ucapan ini mengomentari lapar pemain, misalnya `Kamu kelihatan lemas. Kita cari makan dulu`. Lihat bagian 5.

**`private bool TrySpeak(NPCCompanion npc, BarkCategory c)`:**
1. Keluar kalau `UIFocusStack.IsPaused` bernilai `true`, atau run sedang berakhir.
2. Keluar kalau jarak ke pemain di atas `hearRadius`.
3. Keluar kalau kategori masih dalam `categoryCooldownSeconds`.
4. Kecuali `ZombieIncoming`, keluar kalau masih dalam `globalCooldownSeconds` atau `npcCooldownSeconds`.
5. Ambil baris dari `BarkSet` milik `NPCData`. Baris diambil dari urutan acak, dan tidak mengulang sebelum semua baris terpakai. Baris terakhir tidak boleh menjadi baris pertama pada putaran berikutnya.
6. Terbitkan `SubtitleRequestedEvent { textKey, speakerKey = displayNameKey, priority = 1, sourceKind = Bark }`.
7. Kalau `clip` ada, putar pada `AudioSource` milik `NPCCompanion`. Suara rekaman bersifat opsional.
8. Catat semua waktu. Waktu memakai `Time.time`, sehingga jeda ikut berhenti saat game dijeda.

Kalau beberapa pemicu aktif bersamaan, hanya prioritas tertinggi yang diucapkan. `TickLoop` memakai `WaitForSeconds`, jadi berhenti saat `timeScale = 0`.

### Teks untuk #104

Semua teks masuk tabel kunci `LocalizationManager`:

| Kunci | Isi |
|---|---|
| `lore.<loreId>.title`, `lore.<loreId>.body` | Judul dan isi temuan |
| `bark.common.<category>.<n>` | Ucapan bersama, minimal 3 baris per kategori |
| `bark.<npcId>.<category>.<n>` | Ucapan khusus NPC, dan opsional |
| `prompt.read`, `prompt.listen`, `prompt.read_again` | Prompt `LorePoint` |

Contoh baris awal `id`:

| Kategori | Contoh |
|---|---|
| `Greeting` | `Syukurlah kamu kembali.` |
| `Hurt` | `Lukaku belum menutup...` |
| `Hungry` | `Kamu pucat. Sudah makan belum?` |
| `ZombieIncoming` | `Mereka datang! Masuk, cepat!` |

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `LoreFoundEvent`, `LoreHintGrantedEvent`, dan `SubtitleRequestedEvent` (#104).

**Event yang didengar:** `NightfallEvent`, `VitalWarningChangedEvent`, `PlayerBaseStateChangedEvent`, `NPCDiedEvent`, dan `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `IslandInteractable` (#17) | `LorePoint` mewarisinya |
| `UIFocusStack` (#83) | Panel `LoreReader` |
| `ObjectiveManager` (#81) | `DiaryArchivePanel` mendapat tab `Arsip` |
| `HeavyItemSystem` (#69) | Mendengar `LoreHintGrantedEvent` |
| `NPCRoster` (#76) | Membaca `NPCRecord`, dan `NPCData.barkSet` |
| `LocalizationManager` (#104) | Teks dan subtitle |
| `WorldStateSave` (#80) | `world_lore` dengan `LoadOrder` 110 |
| `LocalStatsManager` (#57) | Boleh mendengar `LoreFoundEvent` |

**Batas pekerjaan:** Diary 3 bab dan tujuan pemain ada di #81. Naskah lengkap dan rekaman suara ditulis saat produksi konten.

## 4. Kasus Khusus

- **Objek pool:** `LorePoint` adalah objek scene dan `NPCCompanion` tidak memakai `ObjectPooler` untuk ucapan, jadi tidak ada reset pool. Baris subtitle dibuat sekali di #104.
- **Region dilepas:** `LorePoint.CaptureState` mengembalikan JSON kosong. Saat dimuat lagi, tampilan dibaca dari `collectedThisRun`. Data tidak bergantung pada region aktif.
- **Game dijeda:** `LoreReader` menjeda game, jadi membaca aman. `NPCBarks` berhenti.
- **Save:** `world_lore` menyimpan `collectedThisRun` dan `grantedHints`. Jeda ucapan tidak disimpan, jadi NPC bisa berbicara sesaat setelah load. `OnRestoreFinished` tidak menerbitkan `LoreFoundEvent` lagi.
- **Permadeath:** `collectedThisRun` dan `grantedHints` hilang bersama save, sehingga petunjuk tersedia lagi di run baru. `lore_archive.json` tidak dihapus `DeleteRunSave`.
- **File `lore_archive.json` rusak:** dianggap kosong, lalu dicatat. Run tetap berjalan.
- **Semua suku cadang sudah ditemukan:** `HeavyItemSystem` mengabaikan petunjuk, dan teksnya tetap bisa dibaca.
- **NPC mati di tengah suara:** `NPCDiedEvent` menghentikan `AudioSource` dan menghapus `BarkRuntime`.
- **Zombie Desa siang hari:** `ZombieIncoming` hanya terpicu kalau zombie aktif terdeteksi, bukan karena layer `Enemy` saja. Musuh dengan state `Submerged` atau `Dormant` dilewati (#63).
- **Tidur:** `SleepSystem` (#67) mempercepat waktu, jadi `TrySpeak` juga keluar saat pemain sedang tidur.
- **Cinematic:** `UIFocusChangedEvent` tidak menjeda `Cinematic`, jadi `TrySpeak` juga keluar kalau `TopPanel.id = Cinematic`.

## 5. Keputusan Desain

**Keputusan yang dipakai di blueprint ini:**

- `LoreReader` menjeda game, sesuai tabel `UIFocusStack.md`.
- Ucapan NPC disalurkan lewat subtitle #104.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Sebaran:** 20 temuan dengan pembagian di tabel. Region 4 paling banyak.
2. **Petunjuk gameplay:** ya, tetapi sangat terbatas. 2 log militer Region 4 menandai 1 suku cadang masing-masing. Temuan lain murni cerita. Alasan: ada alasan untuk mencari, tanpa membuat temuan menjadi wajib.
3. **Permadeath:** teks tetap terbuka di `lore_archive.json`, seperti `stats.json` di #57, dan bisa dibaca dari menu utama. Temuan di dunia dan petunjuknya tetap harus dicari lagi. Alasan: GDD meminta game mengulang dari awal, dan teks tidak memberi keuntungan permainan.
4. **Arti ucapan "lapar":** mengomentari lapar pemain, karena `NPCRoster.md` menyatakan NPC tidak punya `Hunger`. Pilihan lainnya adalah membuat kebutuhan makan NPC di #76.
5. **Bahasa ucapan dan suara:** versi awal hanya teks. Suara ditambahkan kalau ada rekaman.
6. **Nilai jeda ucapan:** seperti tabel angka penyetelan.

## 6. Skenario Tes

1. Bidik `LorePoint` catatan, lalu tekan `E`. **Hasil:** `LoreReader` terbuka, game terjeda, dan `LoreFoundEvent` terkirim sekali.
2. Tutup, lalu baca lagi. **Hasil:** teks sama, tanpa event dan petunjuk baru.
3. Buka tab `Arsip`. **Hasil:** temuan tampil pada Region yang benar, dengan jumlah `1/4`.
4. Temukan rekaman. **Hasil:** suara terdengar saat jeda, transkrip tampil, dan suara berhenti saat panel ditutup.
5. Baca log `PartLocation`. **Hasil:** satu suku cadang mendapat penanda kompas. Log ketiga dengan petunjuk tidak menambah penanda.
6. Save, keluar, lalu lanjutkan. **Hasil:** koleksi dan petunjuk tetap sama, dan tidak ada event ulang.
7. Lepas Region 1, lalu kembali. **Hasil:** prompt `LorePoint` tetap `prompt.read_again`.
8. Mati, lalu mulai run baru. **Hasil:** `LorePoint` kembali `prompt.read`, petunjuk tersedia lagi, dan arsip di menu utama tetap ada.
9. Turunkan HP NPC ke 30 persen. **Hasil:** NPC mengeluh, lalu tidak mengeluh lagi sebelum 90 detik.
10. Masuk malam dengan 3 NPC di dekat pemain. **Hasil:** `ZombieIncoming` tampil, dan NPC lain tidak saling menimpa kategori selain itu dalam jeda global.
11. Buat `Hunger` pemain rendah. **Hasil:** NPC mengomentarinya, tanpa ada nilai `Hunger` di NPC.
12. Ulangi satu kategori berkali-kali dengan jeda dipendekkan. **Hasil:** baris tidak berulang sebelum semua terpakai.
13. Jeda saat NPC baru saja berbicara. **Hasil:** tidak ada ucapan baru, dan jeda tidak berkurang.
14. Biarkan NPC berada jauh dari pemain. **Hasil:** ucapan tidak tampil.
