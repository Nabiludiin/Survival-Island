# Blueprint: LocalizationManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/LocalizationManager.md`
**Deskripsi Utama:** Sumber semua teks pemain lewat kunci, format angka dan jamak, subtitle, serta pilihan aksesibilitas (ukuran teks, getaran kamera, kilatan, dan penanda selain warna).
**Issue:** #104
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Mendukung Diary di bagian 4 dan peringatan bencana di bagian 6.

## 1. Arsitektur & Variabel Utama

Singleton persisten yang disiapkan di langkah 5 `RunLifecycle` (#82), sebelum UI tampil. Menu pengaturannya milik #19. Issue ini menyediakan data dan fungsi yang dipanggil menu itu.

### Perbandingan alat

| Hal | Paket Unity Localization | Tabel kunci sendiri |
|---|---|---|
| Dependensi | Paket tambahan, memakai Addressables | Tidak ada |
| Pemuatan | Asinkron | Sinkron, cocok untuk `GetPrompt()` |
| Jamak dan format | Smart Strings lengkap | Cukup ditulis sendiri untuk dua bahasa |
| Asset per bahasa | Tersedia | Hanya teks dan subtitle |

Rekomendasi: **tabel kunci sendiri**. Lihat bagian 5.

### Data

```csharp
[CreateAssetMenu(menuName = "Survival/Localization Table")]
public class LocalizationTable : ScriptableObject
{
    public string localeId;       // "id" atau "en"
    public string cultureName;    // "id-ID" atau "en-US"
    public LocalizedRow[] rows;   // Hasil impor CSV
}

[Serializable] public struct LocalizedRow { public string key; public string text; }

[Serializable]
public struct AccessibilityOptions
{
    public float textScale;          // Nilai awal 1
    public float cameraShakeScale;   // Nilai awal 1. 0 berarti mati
    public bool  reduceFlashing;     // Nilai awal false
    public bool  subtitlesEnabled;   // Nilai awal true
    public bool  showSpeakerName;    // Nilai awal true
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `LocalizationManager` | `null` | Singleton persisten |
| `tables` | `LocalizationTable[]` | Dari Inspector | Satu aset per bahasa |
| `CurrentLocale` | `string` | `id` | Bahasa aktif |
| `fallbackLocale` | `string` | `id` | Dipakai kalau kunci tidak ada |
| `current`, `fallback` | `Dictionary<string,string>` | Dibuat dari tabel | Pencarian kunci |
| `Accessibility` | `AccessibilityOptions` | Nilai di atas | Dibaca sistem visual |
| `missingLogged` | `HashSet<string>` | Kosong | Kunci hilang hanya dicatat sekali |

**Tempat simpan:** `PlayerPrefs` dengan kunci `settings.locale` dan `a11y.*`. `SettingsManager.headBobEnabled` tetap di #19, seperti yang sudah dipakai `FirstPersonCamera` (#102).

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.localization.textScaleMin` | 0,85 | Batas bawah ukuran teks |
| `GameBalanceConfig.localization.textScaleMax` | 1,5 | Batas atas ukuran teks |
| `GameBalanceConfig.localization.subtitleMinSeconds` | 1,5 | Lama tampil terpendek |
| `GameBalanceConfig.localization.subtitleSecondsPerChar` | 0,06 | Lama tampil per huruf |
| `GameBalanceConfig.localization.subtitleMaxRows` | 3 | Baris subtitle bersamaan |
| `GameBalanceConfig.localization.maxFlashesPerSecond` | 3 | Batas kilatan saat `reduceFlashing` aktif |

## 2. Detail Logika & Fungsi

### Kunci teks

Kunci berbentuk `<kelompok>.<nama>`: `ui.`, `prompt.`, `item.`, `diary.`, `lore.`, `bark.`, `sub.`, `notif.`, `build.error.`, dan `gameover.`.

**`public string Get(string key)`:**
1. Cari di `current`, lalu di `fallback`.
2. Kalau tidak ada, catat sekali ke `missingLogged`. Editor menampilkan `#key#`. Build rilis mengembalikan kunci mentah (#99).
3. Kembalikan teks yang tokennya sudah diganti lewat `ResolveInputTokens`.

`NarrativeSequencer` membaca setting tampil subtitle dari `Accessibility.subtitlesEnabled` (#99).

**`public string Format(string key, string name, string value)`** mengganti `{name}`. Overload dua dan tiga pasangan tersedia, supaya tidak perlu `params`.

**`private string ResolveInputTokens(string text)`** mengganti `{input:Interact}` dengan `InputManager.GetBindingDisplay(GameInputAction.Interact)`. Hasilnya di-cache per kunci, dan cache dibuang saat `InputBindingsChangedEvent` atau `LocaleChangedEvent` terkirim.

**`public void SetLocale(string localeId)`:** bangun ulang `current`, simpan ke `PlayerPrefs`, ganti `CultureInfo` format, lalu terbitkan `LocaleChangedEvent`.

**`LocalizedText`** (komponen pada `TMP_Text`) punya `[SerializeField] string key`. `OnEnable` mendengar `LocaleChangedEvent`, `InputBindingsChangedEvent`, dan `AccessibilityChangedEvent`, lalu mengisi teks dan ukuran font. `OnDisable` melepasnya. Teks yang dibuat lewat kode mendengar event yang sama.

### Mengganti teks langsung

| Lokasi lama | Teks lama | Pengganti |
|---|---|---|
| `ShipInteractable` | `Press [E] to grab: ...` | `prompt.format` berisi `[{input:Interact}] {action}`, dengan `prompt.grab` |
| `IslandInteractable.RefreshPrompt` | `[E] Buka Peti` | `prompt.format`, dan `promptKey` yang sudah ada |
| `UIManager.ShowGameOver(string reason)` | Teks alasan | `ShowGameOver(DeathReason)`, dengan `gameover.<DeathReason>` |
| `BuildMenu` | Pesan `PlacementError` | `build.error.<PlacementError>` |
| `ItemData.itemName` | Nama langsung | `item.<itemId>.name` |
| `DiaryEntry.title`, `body` | Teks di aset | `diary.ch<chapter>.title`, `diary.ch<chapter>.body` |
| `NPCInfection`, `FishingSystem`, `NotificationManager` | Pesan langsung | `bark.`, `prompt.`, dan `notif.` |

**`LocalizationAuditWindow`** (Editor) melaporkan kunci yang hilang di salah satu bahasa, kunci yang tidak terpakai, dan kata `Press [` di aset atau kode. Pemeriksaan ini wajib lulus sebelum build.

### Angka, hari, dan jamak

- **`FormatNumber(int n)`:** memakai `CultureInfo` aktif. `1250` menjadi `1.250` (id) dan `1,250` (en).
- **`FormatDay(int day)`:** memakai `hud.day`. `Hari ke-12` (id) dan `Day 12` (en).
- **`FormatClock(float hour)`:** format 24 jam di kedua bahasa, misalnya `06:30`.
- **`GetPlural(string key, int count)`:** memakai `key.one` kalau `count == 1`, selain itu `key.other`. Bahasa Indonesia hanya menulis `key.other`, dan fungsi ini memakainya sebagai cadangan. Hasilnya mengganti `{count}` dengan `FormatNumber(count)`.
- **HUD:** teks hanya dibuat ulang saat nilainya berubah, supaya tidak menghasilkan garbage per frame.

### Subtitle

**`SubtitlePresenter`** adalah komponen `Canvas` persisten dengan baris yang dibuat sekali sebanyak `subtitleMaxRows`, jadi tidak memakai `Instantiate`.

**`SubtitleRequestedEvent { textKey, speakerKey, priority, sourceKind }`:**
1. Keluar kalau `subtitlesEnabled` bernilai `false`, kecuali `sourceKind = Warning`. Peringatan bencana selalu tampil.
2. Hitung lama tampil: `Max(subtitleMinSeconds, jumlah huruf * subtitleSecondsPerChar)`.
3. Kalau baris penuh, ganti baris dengan prioritas terendah. Permintaan yang lebih rendah dari semua baris dibuang.
4. Tampilkan `[Nama] teks` kalau `showSpeakerName` aktif dan `speakerKey` tidak kosong.
5. Kurangi timer dengan `Time.deltaTime`, sehingga subtitle membeku saat game dijeda.

| Sumber | Pengirim | `sourceKind` | Prioritas |
|---|---|---|---|
| Adegan | `NarrativeSequencer` (#99) | `Scene` | 3 |
| Peringatan bencana | `DisasterManager` (#70) | `Warning` | 2 |
| Ucapan NPC | `NPCBarks` (#100) | `Bark` | 1 |

Rekaman di `LoreReader` (#100) menjeda game, jadi teksnya tampil di panel itu dan tidak memakai subtitle.

### Penanda selain warna

| Tempat | Warna saat ini | Tambahan bentuk |
|---|---|---|
| Hologram bangunan, valid (#79) | Hijau | Ikon centang di atas hologram |
| Hologram bangunan, ditolak (#79) | Merah | Material berpola garis miring, ikon silang, dan teks `build.error.<PlacementError>` |
| Highlight `Available` (#17) | Kuning muda | Outline utuh dan ikon tangan pada prompt |
| Highlight `Blocked` (#17) | Merah | Outline putus-putus, ikon gembok pada prompt, dan teks `InteractBlockReason` |
| Penanda pancing (#55) | Berubah warna | Teks yang sudah ada, ditambah ikon silang saat tidak valid |

`GhostStatusIcon` memakai satu ikon `Canvas` world-space pada hologram, dan hanya diganti saat `PlacementError` berubah. Pola itu selalu aktif, jadi tidak perlu pilihan menu.

### Pilihan aksesibilitas

**`public void SetAccessibility(in AccessibilityOptions o)`**, dipanggil menu #19: batasi `textScale` ke rentang config, batasi `cameraShakeScale` ke 0 sampai 1, simpan ke `PlayerPrefs`, lalu terbitkan `AccessibilityChangedEvent`.

| Pilihan | Nilai | Pembaca |
|---|---|---|
| `textScale` | 0,85 sampai 1,5 | `LocalizedText`, subtitle, Diary, `LoreReader` |
| `cameraShakeScale` | 0 sampai 1 | `CameraShake` (#24) mengalikan amplitudo |
| `SettingsManager.headBobEnabled` | Ya atau tidak | `FirstPersonCamera` (#102) |
| `reduceFlashing` | Ya atau tidak | `DiseaseManager`, `DisasterManager`, `PostProcessController` (#23), `VFXManager` (#15) |

**Layout:** panel panjang memakai `ScrollRect`. HUD memakai `TMP` Auto Size dengan batas tetap, supaya skala 1,5 tidak keluar dari kotak.

### Peringatan kilatan cahaya

1. Pada peluncuran pertama, `MainMenuController` membaca `a11y.flashWarningSeen`. Kalau belum ada, `Push` panel `ConfirmDialog` berisi `ui.flash_warning`.
2. Pilihan: **Lanjut**, atau **Kurangi kilatan**, yang mengisi `reduceFlashing = true`.
3. Simpan `a11y.flashWarningSeen = 1`. Teks yang sama selalu tersedia di menu #19.

**`FlashGuard.Allow()`** membatasi kilatan layar penuh sampai `maxFlashesPerSecond`. Selain itu, saat `reduceFlashing` aktif:
- Langit merah erupsi berubah perlahan, tanpa denyut.
- Kilat badai memakai satu perubahan cahaya lembut.
- Efek layar penyakit memakai warna tetap (#45).
- Muzzle flash tetap ada, tetapi intensitas cahayanya dibagi dua.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `LocaleChangedEvent` dan `AccessibilityChangedEvent` (sticky).

**Event yang didengar:** `InputBindingsChangedEvent` dan `SubtitleRequestedEvent`.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | `GetBindingDisplay` untuk token `{input:...}` |
| `SettingsManager` (#19) | Menu memanggil `SetLocale` dan `SetAccessibility` |
| `RunLifecycle` (#82) | Menyiapkan di langkah 5 Bootstrap |
| `UIFocusStack` (#83) | `ConfirmDialog` untuk peringatan kilatan |
| `NPCBarks`, `LoreCollectibles` (#100) | Mengisi kunci `bark.` dan `lore.` |
| `NPCRoster` (#76), `IslandInteractable` (#17) | Sudah memakai `displayNameKey` dan `promptKey` |

**Batas pekerjaan:** tata letak menu ada di #19, dan cara kerja getaran kamera ada di #24.

## 4. Kasus Khusus

- **Objek pool:** `LocalizedText` pada objek pool membaca teks di `OnEnable`, jadi bahasa yang berubah saat objek nonaktif tetap benar. Sistem ini sendiri tidak memakai pool.
- **Game dijeda:** `SetLocale` berjalan normal, karena biasanya dipanggil dari menu. Subtitle membeku dan lanjut setelah jeda.
- **`ClearAll` (#82):** `managersRoot` dimatikan lalu dinyalakan, sehingga `SubtitlePresenter` berlangganan ulang. `LocalizedText` yang aktif di luar akar itu harus membaca `CurrentLocale` lagi saat `RunStartedEvent`.
- **Permadeath dan save:** bahasa dan aksesibilitas ada di `PlayerPrefs`, jadi tetap ada. Tidak ada data di `survival_save.json`. Subtitle yang tampil dihapus saat `RunEndedEvent`.
- **Bahasa di `PlayerPrefs` tidak dikenal:** kembali ke `id`.
- **Kunci hilang di `en`:** memakai `id`, dan tetap dilaporkan `LocalizationAuditWindow`.
- **Token tombol tidak dikenal:** token dibiarkan terlihat di Editor dan dicatat sekali.
- **Teks panjang pada skala 1,5:** Diary dan `LoreReader` memakai `ScrollRect`, dan subtitle dibatasi dua baris per entri.

## 5. Keputusan Desain

**Keputusan yang dipakai di blueprint ini:**

- Semua penulisan tombol memakai `GetBindingDisplay`, sesuai `InputManager.md`.
- Bentuk atau ikon pada hologram dan highlight selalu aktif.

**Usulan yang menunggu developer:**

1. **Bahasa awal:** Indonesia sebagai bahasa awal dan cadangan, dengan struktur yang sudah siap untuk `en`. Tabel Inggris diisi setelah teks stabil. Alasan: sumber cerita proyek berbahasa Indonesia.
2. **Alat:** tabel kunci sendiri, diimpor dari CSV. Alasan: hanya dua bahasa, panggilan sinkron cocok dengan `GetPrompt()`, dan tidak menambah Addressables.
3. **Rentang pilihan:** `textScale` 0,85 sampai 1,5, dan `cameraShakeScale` 0 sampai 1.
4. **Peringatan kilatan:** tampil sekali saat peluncuran pertama.
5. **Peringatan bencana selalu tampil** walaupun subtitle dimatikan, karena menyangkut keselamatan pemain.
6. **Tempat `reduceFlashing`:** di `LocalizationManager.Accessibility`, seperti yang sudah dirujuk `DiseaseManager`. Pilihan lainnya adalah memindahkannya ke `SettingsManager`.

## 6. Skenario Tes

1. Jalankan dari `Bootstrap`. **Hasil:** menu utama tampil dalam Bahasa Indonesia tanpa `#key#`.
2. Ganti bahasa ke `en` dari menu saat game dijeda. **Hasil:** teks yang terlihat langsung berubah, dan pesan `Hari ke-3` menjadi `Day 3`.
3. Hapus satu baris `en`. **Hasil:** teks memakai `id`, dan `LocalizationAuditWindow` melaporkannya.
4. Ganti tombol interaksi ke `F`. **Hasil:** prompt berikutnya menampilkan `[F]`, tanpa memuat ulang scene.
5. Tampilkan `1250` dan jumlah `1` serta `5` kayu. **Hasil:** format angka mengikuti bahasa, dan jamak Inggris benar.
6. Arahkan hologram ke pasir dengan warna layar dibuat hitam putih. **Hasil:** status tetap terbaca dari pola, ikon, dan teks.
7. Bidik peti saat inventori penuh. **Hasil:** outline putus-putus dan ikon gembok muncul.
8. Picu erupsi saat subtitle mati. **Hasil:** peringatan tetap tampil, dan ucapan NPC tidak.
9. Jeda saat subtitle tampil selama 10 detik, lalu lanjutkan. **Hasil:** sisa waktu tidak berkurang saat jeda.
10. Setel `textScale` 1,5, lalu buka HUD, Diary, dan `LoreReader`. **Hasil:** teks tidak keluar kotak.
11. Setel `cameraShakeScale` 0 dan matikan head bob. **Hasil:** getaran dan head bob berhenti, dan tembakan tetap normal.
12. Hapus `a11y.flashWarningSeen`, lalu jalankan. **Hasil:** peringatan muncul sekali. Pilih **Kurangi kilatan**, lalu picu erupsi. **Hasil:** langit berubah perlahan.
13. Mati, lalu mulai run baru. **Hasil:** bahasa dan pilihan aksesibilitas tetap sama.
