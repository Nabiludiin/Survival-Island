# Blueprint: TutorialManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/TutorialManager.md`
**Deskripsi Utama:** Sistem onboarding berisi pop-up panduan pendek yang muncul saat pemain pertama kali melakukan atau mengalami sesuatu, misalnya pertama membuka inventori atau pertama menebang pohon.
**Issue:** #16
**Rujukan GDD:** Bagian 4 menetapkan Diary sebagai *guidebook* utama. Pop-up ini adalah pelengkap untuk kontrol dasar, dan tidak menggantikan Diary. Daftar pemicu di bawah adalah usulan.

## 1. Arsitektur & Variabel Utama

Singleton persisten, karena Fase 1 dan Fase 2 sama-sama memakainya. Manager ini memakai `IBootstrapService`. Tutorial tidak menghentikan waktu. Panel `TutorialHintPanel` hanya menampilkan teks dan tidak masuk `UIFocusStack`, jadi tidak mengambil kursor atau input.

```csharp
[CreateAssetMenu(menuName = "Survival/Tutorial Hint")]
public class TutorialHint : ScriptableObject
{
    public string hintId;              // Contoh: "first_inventory"
    public string textKey;             // Kunci LocalizationManager (#104)
    public string fallbackText;        // Dipakai selama #104 belum ada
    public int priority;               // Makin besar makin dahulu
    public bool allowDuringDanger;     // Boleh tampil saat pemain baru terkena serangan
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `hints` | `Dictionary<string, TutorialHint>` | Dari `Resources` | Semua data pop-up |
| `seen` | `HashSet<string>` | Dari `PlayerPrefs` | Pop-up yang sudah tampil |
| `queue` | `List<TutorialHint>` | Kosong, kapasitas 4 | Antrean, urut menurut `priority` |
| `current` | `TutorialHint` | `null` | Pop-up yang sedang tampil |
| `showTimer` / `gapTimer` | `float` | 0 | Sisa tampil dan jeda |
| `dangerTimer` | `float` | 0 | Sisa waktu "baru terkena serangan" |
| `isPausedUI` | `bool` | `false` | Dari `UIFocusChangedEvent.isPaused` |
| `SeenPrefsKey` | `const string` | `settings.tutorial.seen` | Kunci `PlayerPrefs` |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.tutorial.displaySeconds` | 6 | Lama pop-up tampil |
| `GameBalanceConfig.tutorial.minGapSeconds` | 3 | Jeda antar-pop-up |
| `GameBalanceConfig.tutorial.dangerHoldSeconds` | 4 | Penundaan setelah pemain terkena serangan |
| `GameBalanceConfig.tutorial.minSeenSeconds` | 2 | Lama tampil minimum agar dianggap sudah dibaca |
| `GameBalanceConfig.tutorial.queueLimit` | 4 | Batas antrean |
| `GameBalanceConfig.tutorial.lowVitalThreshold` | 50 | Batas `Hunger` dan `Thirst` untuk pop-up pertama |

## 2. Detail Logika & Fungsi

### Kapan pop-up muncul

Semua pemicu berasal dari event, sehingga sistem gameplay tidak perlu mengenal `TutorialManager`.

| `hintId` | Pemicu pertama | Isi singkat | Prioritas |
|---|---|---|---|
| `ship_loot` | `PhaseChangedEvent` ke `SinkingShip` | Tombol `Interact` mengambil barang dan NPC sebelum waktu habis | 90 |
| `island_vitals` | `PhaseChangedEvent` ke `IslandSurvival` | Cara membaca lapar, haus, HP, dan stamina | 80 |
| `first_inventory` | `UIPanelOpenedEvent` dengan `Inventory` | Cara memakai dan memindahkan item | 50 |
| `first_tree` | `ResourceNodeHitEvent` dengan `NodeType.Tree` | Kapak mempercepat tebangan, dan kayu keluar saat pohon tumbang | 50 |
| `first_rock` | `ResourceNodeHitEvent` dengan `NodeType.Rock` | Alat yang tepat untuk batu | 40 |
| `first_crafting` | `UIPanelOpenedEvent` dengan `Crafting` | Cara merakit item | 50 |
| `first_build` | `BuildModeChangedEvent` | Tidak bisa membangun di pasir pantai (GDD Bagian 4) | 60 |
| `first_kail` | `EquippedItemChangedEvent` dengan `Base_Kail` | Cara melempar dan menarik Kail | 50 |
| `first_pistol` | `EquippedItemChangedEvent` dengan `Base_Pistol` | Tombol reload dan peluru terbatas | 50 |
| `low_hunger` / `low_thirst` | `VitalsChangedEvent` di bawah `lowVitalThreshold` | Sumber makanan dan air bersih | 70 |
| `first_infection` | `VitalsChangedEvent.infection > 0` | Infeksi dari zombie dan vaksin | 85 |
| `first_night` | `NightfallEvent` | Zombie keluar saat malam, dan berlindung di base | 95 |
| `first_day` | `DaybreakEvent` pertama setelah `first_night` tampil | Zombie terbakar matahari, dan hewan buas aktif siang hari | 60 |
| `first_diary` | `DiaryChapterUnlockedEvent` | Diary berisi panduan lanjutan | 60 |
| `first_supply` | `BeachSupplyArrivedEvent` | Kargo tiba di pantai | 60 |
| `region_snow` | `RegionChangedEvent` ke `Snow` | Pakai pakaian hangat atau obor, kalau tidak terkena frostbite | 75 |
| `region_logistic` | `RegionChangedEvent` ke `Logistic` | Suku cadang berat memperlambat langkah | 75 |
| `first_heavy` | `HeavyItemChangedEvent` dengan `isCarried = true` | Tidak bisa lari dan lompat saat membawa barang | 75 |

Lucky Machine tidak punya pop-up, karena mesin itu sengaja menjadi kejutan pemain. Isi Diary juga tidak diulang di sini, supaya Diary tetap punya nilai.

### Fungsi

**`public void Initialize()`**: muat semua `TutorialHint`, lalu baca `SeenPrefsKey`. Format simpanannya adalah daftar `hintId` dipisah koma.

**`public void Request(string hintId)`**:

1. Lewati kalau `SettingsManager.Instance.tutorialEnabled` bernilai `false`.
2. Lewati kalau `hintId` sudah ada di `seen`, sudah di `queue`, atau sedang menjadi `current`.
3. Kalau antrean penuh, buang pop-up dengan prioritas terendah. Pop-up itu belum dianggap sudah tampil, jadi bisa muncul lagi pada pemicu berikutnya.
4. Masukkan ke `queue` menurut `priority`.

**`private void Update()`**, memakai `Time.deltaTime`:

1. Kalau `current != null`, kurangi `showTimer`. Saat habis, `Finish()`.
2. Kalau tidak, kurangi `gapTimer`, dan kurangi `dangerTimer`.
3. Kalau `gapTimer <= 0`, `!isPausedUI`, dan ada `queue`, ambil pop-up pertama yang boleh tampil: `dangerTimer <= 0` atau `allowDuringDanger`. Lalu panggil `ShowNext(hint)`.

**`private void ShowNext(TutorialHint h)`**:

1. Teks: `LocalizationManager` memakai `textKey`, atau `fallbackText`.
2. Ganti token `{action:Interact}` lewat `InputManager.Instance.GetBindingDisplay(...)`. Teks tombol tertulis langsung seperti `Press [E]` dilarang (#18).
3. `TutorialHintPanel.Show(teks)`, dan `showTimer = displaySeconds`.

**`private void Finish()`**: sembunyikan panel, `gapTimer = minGapSeconds`, dan `current = null`. Kalau pop-up sudah tampil selama `minSeenSeconds`, panggil `MarkSeen`.

**`private void MarkSeen(string id)`**: tambah ke `seen`, tulis `PlayerPrefs`, lalu `PlayerPrefs.Save()`. Tulisan terjadi paling banyak sekitar 20 kali sepanjang umur game.

**`public void ResetSeen()`**: mengosongkan `seen` dan kunci `PlayerPrefs`. Dipanggil tombol "Ulang tutorial" di menu Settings (#19).

### Tampilan

`TutorialHintPanel` berada di canvas persisten, di atas HUD dan di bawah panel menjeda. Panel tidak menangkap klik (`raycastTarget = false`), sehingga inventori di bawahnya tetap bisa dipakai. Pop-up tetap terlihat saat panel `Inventory` terbuka, karena panel itu tidak menjeda.

## 3. Hubungan dengan Sistem Lain

**Event yang didengar:** semua event pada tabel pemicu, ditambah `PlayerDamagedEvent` (`dangerTimer = dangerHoldSeconds`), `UIFocusChangedEvent`, `InputBindingsChangedEvent` (menulis ulang teks yang sedang tampil), dan `RunEndedEvent`. Blueprint ini tidak mengirim event.

**Event baru yang dibutuhkan:** `UIPanelOpenedEvent` dari `UIFocusStack`, dan `ResourceNodeHitEvent` dari `ResourceNode`.

| Sistem | Hubungan |
|---|---|
| `SettingsManager` (#19) | `tutorialEnabled`, dan tombol `ResetSeen` |
| `InputManager` (#18) | `GetBindingDisplay` |
| `LocalizationManager` (#104) | Teks, dengan `fallbackText` selama #104 belum ada |
| `ObjectiveManager` (#81), `DiaryManager` | Tidak saling memanggil. Diary tetap sumber panduan cerita |
| `NotificationManager` (#22) | Sistem terpisah untuk pesan singkat seperti `+5 Wood` |
| `ZoneManager` | Pesan barang berat dipindah ke pop-up `region_logistic` |

**Batas pekerjaan:** penanda arah objektif di #81, teks Diary di #81, dan subtitle di #104.

## 4. Kasus Khusus

- **Game dijeda:** `Update` memakai `Time.deltaTime`, jadi semua timer berhenti. Saat `isPaused`, panel disembunyikan sementara dan muncul lagi setelah jeda. Pop-up baru tidak dimulai saat jeda.
- **Sticky event setelah load:** `DayNightDirector` dan pemilik event lain mengirim ulang event sticky di `OnRestoreFinished`. Pop-up yang sudah ada di `seen` diabaikan, dan yang belum memang pantas muncul.
- **Fase 1 yang singkat:** hanya `ship_loot` yang muncul di kapal, supaya 60 detik tidak habis untuk membaca. Pop-up itu punya `allowDuringDanger = true`.
- **Banyak pemicu bersamaan:** misalnya pindah region saat malam tiba. Antrean menampilkan satu per satu menurut prioritas, dengan jeda `minGapSeconds`.
- **Dipotong sebelum dibaca:** pop-up yang tampil kurang dari `minSeenSeconds` karena run berakhir tidak ditandai, sehingga muncul lagi lain kali.
- **Objek pool:** sistem ini tidak memakai pool.
- **Permadeath:** `RunEndedEvent` dan `ResetForNewRun` mengosongkan `queue` dan `current`, serta menyembunyikan panel. Isi `seen` **tidak** dihapus, karena disimpan di `PlayerPrefs` (`settings.tutorial.seen`), terpisah dari save run. `PlayerPrefs.DeleteAll()` dilarang (#82).
- **Save:** tidak ada data di `survival_save.json`.
- **Data rusak:** `hintId` yang tidak dikenal di `PlayerPrefs` diabaikan. Pop-up baru dari update game tetap tampil.

## 5. Keputusan Desain

**Sudah mengikuti GDD:** Diary tetap *guidebook* utama (Bagian 4). Pop-up tidak mengulang isi lore atau resep vaksin dari Diary.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Permadeath dan pop-up:** pop-up yang sudah dibaca tidak muncul lagi di run berikutnya. Pemain mengulang dari awal cukup sering, jadi pengulangan akan mengganggu. Developer bisa memilih mengulang semua pop-up per run dengan memindahkan `seen` ke save run.
2. **Bentuk tampilan:** pop-up kecil tanpa jeda, bukan layar penuh yang menghentikan game. Permainan ini berbasis tekanan waktu, dan Fase 1 hanya 60 detik.
3. **Daftar pemicu:** 19 pop-up pada tabel di atas. Isinya bisa dikurangi tanpa mengubah kode, karena semua berbasis data.
4. **Pilihan matikan tutorial:** tersedia di menu Settings, dengan nilai awal menyala.

## 6. Skenario Tes

1. Hapus kunci `settings.tutorial.seen`, lalu buka inventori pertama kali. **Hasil:** `first_inventory` tampil 6 detik, dan inventori tetap bisa dipakai.
2. Tutup, lalu buka inventori lagi. **Hasil:** pop-up tidak tampil lagi.
3. Tebang pohon pertama. **Hasil:** `first_tree` tampil setelah pukulan pertama.
4. Picu dua pop-up sekaligus lewat `DevConsole`. **Hasil:** prioritas tinggi lebih dulu, dengan jeda 3 detik.
5. Terkena serangan hewan sesaat sebelum pop-up. **Hasil:** pop-up menunggu 4 detik.
6. Ubah tombol `Interact` menjadi `F`, lalu masuk Fase 1. **Hasil:** teks menampilkan `F`.
7. Buka Pause saat pop-up tampil. **Hasil:** pop-up hilang sementara, lalu muncul dengan sisa waktu yang sama.
8. Mati, lalu mulai run baru. **Hasil:** antrean kosong, dan pop-up lama tidak muncul lagi. Setting tetap sama.
9. Matikan tutorial di Settings. **Hasil:** tidak ada pop-up baru.
10. Tekan "Ulang tutorial". **Hasil:** `first_inventory` muncul lagi saat inventori dibuka.
11. Masuk Region 3 lalu Region 4. **Hasil:** `region_snow` dan `region_logistic` masing-masing tampil sekali.
