# Blueprint: NotificationManager.cs (Notifikasi & Log Event)
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/NotificationManager.md`
**Deskripsi Utama:** Menampilkan pesan kecil di pojok layar, seperti `+5 Kayu`, `Inventori penuh`, atau `Suku cadang ditemukan`, serta menyimpan riwayat pesan selama satu run.
**Issue:** #22
**Rujukan GDD:** Mendukung bagian 3, 5, dan 6. Bentuk notifikasi dan log adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`NotificationManager` adalah manager persisten (`IBootstrapService`, urutan ke-10 di #82), dan memakai canvas persisten di atas HUD.

```csharp
public enum NotificationKind : byte { Info, Gain, Warning, Danger, Objective }

public struct NotificationEntry
{
    public string text;        // Teks akhir
    public string mergeKey;    // itemId untuk Gain, key untuk yang lain
    public NotificationKind kind;
    public int amount;         // Jumlah Gain, atau hitungan pesan yang sama
    public float remaining;    // Sisa waktu tampil, dalam detik
}

public struct NotificationLogEntry
{
    public string text;
    public NotificationKind kind;
    public int day, hour;
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `NotificationManager` | `null` | Singleton persisten |
| `rows` | `NotificationRow[]` | Sebanyak `maxVisible` | Baris UI yang dibuat satu kali di `Initialize()` |
| `visible` | `List<NotificationEntry>` | Kapasitas `maxVisible` | Pesan yang tampil |
| `queue` | `List<NotificationEntry>` | Kapasitas `queueCapacity` | Pesan menunggu |
| `log` | `NotificationLogEntry[]` | Ring buffer `logCapacity` | Riwayat run ini |
| `isHeld` | `bool` | `false` | Tampilan ditahan selama game dijeda atau ada adegan |
| `kindStyles` | `NotificationStyle[]` | Dari prefab | Warna, ikon, dan klip per `NotificationKind` |
| `announcedParts` | `HashSet<string>` | Kosong | Suku cadang yang sudah diumumkan, supaya menjatuhkan dan mengambil ulang tidak mengulang pesan |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.notification.maxVisible` | 4 | Baris yang tampil bersamaan |
| `GameBalanceConfig.notification.queueCapacity` | 12 | Batas antrean |
| `GameBalanceConfig.notification.infoSeconds` | 3 | Lama tampil `Info` dan `Gain` |
| `GameBalanceConfig.notification.warningSeconds` | 5 | Lama tampil `Warning`, `Danger`, dan `Objective` |
| `GameBalanceConfig.notification.fadeSeconds` | 0,3 | Muncul dan hilang |
| `GameBalanceConfig.notification.mergeWindowSeconds` | 2 | Waktu pesan sejenis digabung |
| `GameBalanceConfig.notification.logCapacity` | 100 | Jumlah riwayat |

### Gaya per jenis

| `NotificationKind` | Dipakai untuk | Warna | Suara |
|---|---|---|---|
| `Info` | Pesan umum, penolakan ringan | Putih | Tidak ada |
| `Gain` | `+5 Kayu` | Hijau muda | Klik pelan, hanya pada baris baru |
| `Warning` | `Inventori penuh`, `Kamu haus` | Kuning | Bunyi pendek |
| `Danger` | `Base terbakar`, peringatan bencana | Merah | Bunyi tegas |
| `Objective` | Suku cadang, region, diary | Biru muda | Bunyi tugas |

Warna tidak menjadi satu-satunya penanda, karena tiap jenis juga punya ikon.

## 2. Detail Logika & Fungsi

### Fungsi publik

**`public void Show(string key, NotificationKind kind = NotificationKind.Info)`**:

1. `text = LocalizationManager.Instance.Get(key)`, dengan kunci berawalan `notif.`, sesuai #104. Kalau hasilnya kosong, pesan dibuang, karena `Get` mengembalikan teks kosong di build rilis untuk kunci yang tidak ada.
2. Panggil `Enqueue(text, mergeKey: key, kind, amount: 1)`.

**Kunci dan isi teks:** tulisan pesan di blueprint lain, seperti `Inventori penuh`, adalah isi bahasa `id`. Kuncinya ditulis di tabel lokal, misalnya `notif.inventory_full`.

**`public void ShowFormat(string key, NotificationKind kind, string name, string value)`** sama, tetapi memakai `LocalizationManager.Format`. Overload dua pasangan tersedia untuk `{installed}` dan `{total}`. Dipakai untuk pesan seperti `Suku cadang terpasang 2/5`.

**`public void ShowGain(string itemId, int delta)`**: `Enqueue` dengan `mergeKey = itemId`, `kind = Gain`, dan `amount = delta`. Teks dibuat saat tampil: `+{amount} {nama}`, dengan nama dari kunci `item.<itemId>.name`.

**`public int GetLog(List<NotificationLogEntry> output)`** menyalin riwayat dari yang terbaru. Dipakai `NotificationLogView`.

### Menggabungkan pesan

**`void Enqueue(...)`**:

1. Tulis ke `log`. `Gain` yang digabung menimpa baris log terakhir dengan `itemId` yang sama, jadi log tidak dipenuhi `+1 Kayu`.
2. Kalau `visible` atau `queue` memuat `mergeKey` yang sama dan umurnya masih di bawah `mergeWindowSeconds`:
   - `Gain`: `amount += delta`, teks diperbarui, dan waktu tampil diulang.
   - Jenis lain: `amount += 1`, teks memakai akhiran `x2`, dan waktu tampil diulang.
   - Selesai tanpa baris baru.
3. Kalau `visible.Count < maxVisible` dan `!isHeld`, tampilkan baris baru.
4. Kalau tidak, masukkan ke `queue`. `Danger` dan `Objective` masuk di depan pesan `Info` dan `Gain`.
5. Kalau `queue` melebihi `queueCapacity`, buang `Info` atau `Gain` tertua. `Danger` dan `Objective` tidak dibuang. Yang dibuang tetap ada di log.

Kalau `Danger` datang saat baris penuh dan semua tidak ditahan, baris `Info` atau `Gain` tertua langsung masuk fase hilang, supaya peringatan tidak menunggu.

### Pembaruan per frame

**`void Update()`** memakai `Time.deltaTime`, jadi waktu tampil berhenti saat game dijeda:

1. Panggil `RefreshHeld()`. Kalau `isHeld`, lewati langkah berikutnya, termasuk pengurangan waktu selama `Cinematic`.
2. Kurangi `remaining` tiap baris. Mulai pudar saat `remaining <= fadeSeconds`.
3. Saat `remaining <= 0`, kosongkan baris dan isi dari `queue`.
4. Atur alpha `CanvasGroup` tiap baris, dan geser posisi dengan `Mathf.MoveTowards`.

`rows` dipakai ulang dengan `Bind(in NotificationEntry)`. Tidak ada `Instantiate`, karena baris berada di canvas persisten dan jumlahnya tetap.

### Tampilan saat ada panel

**`private void RefreshHeld()`** menghitung `isHeld`, dan dipanggil di awal setiap `Update()`:

```csharp
var stack = UIFocusStack.Instance;
var top = stack.TopPanel;
bool cinematic = top != null && top.id == UIPanelId.Cinematic;
bool held = stack.IsPaused || cinematic;
if (held != isHeld) { isHeld = held; canvasGroup.alpha = isHeld ? 0f : 1f; }
```

Status tidak diambil dari `UIFocusChangedEvent`, karena event itu hanya terbit kalau status jeda atau blokir input berubah (`UIFocusStack.ApplyState`). Contohnya, `Cinematic` yang didorong saat `Inventory` terbuka tidak mengubah keduanya, jadi tidak ada event. Pemeriksaan ini hanya membaca dua nilai dan tidak membuat garbage.

| Keadaan | Perilaku |
|---|---|
| Panel tanpa jeda, seperti `Inventory`, `Crafting`, dan `Storage` | Tetap tampil di atas panel, karena pesan penolakan seperti `Kosongkan tas dulu` muncul di sini |
| `Pause`, `Settings`, `Diary`, `LoreReader`, `Loading` | Disembunyikan dan masuk antrean, lalu muncul saat kembali. Waktu tampil tidak berkurang |
| `Cinematic` | Disembunyikan dan masuk antrean. `Time.deltaTime` tetap berjalan, jadi `Update` melewati pengurangan waktu saat `isHeld` |
| `GameOver`, `WinScreen` | `HandleRunEnded` sudah mengosongkan semua |

### Event yang menjadi pesan

| Event | Syarat | Pesan | Jenis |
|---|---|---|---|
| `InventoryChangedEvent` | `delta > 0` | `ShowGain(itemId, delta)` | `Gain` |
| `HeavyItemChangedEvent` | `isCarried == true`, dan `partId` belum ada di `announcedParts` | `Suku cadang ditemukan: {nama}` | `Objective` |
| `HelicopterPartInstalledEvent` | Selalu | `Suku cadang terpasang {installed}/{total}` | `Objective` |
| `HelicopterReadyEvent` | Selalu | `Helikopter siap` | `Objective` |
| `RegionUnlockedEvent` | Selalu | `Akses baru terbuka` | `Objective` |
| `DiaryChapterUnlockedEvent` | Selalu | `Halaman baru di Diary` | `Objective` |
| `NPCRescuedEvent`, `NPCDiedEvent` | Selalu | Nama NPC dari `NPCData.displayNameKey` | `Info`, `Danger` |
| `StructureBurningEvent` | `isBurning == true` | `Base terbakar` | `Danger` |
| `StructureDestroyedEvent` | Selalu | `Bangunan hancur` | `Warning` |

Semua handler di tabel ini keluar lebih dulu kalau `TopPanel.id == Loading`. Pesan langsung lewat `Show` tidak terkena aturan ini.

`delta` negatif tidak ditampilkan. Pemakaian bahan sudah terlihat di panel crafting, dan makanan yang dimakan sudah terlihat di bar status.

**Aturan sumber tunggal:** pesan di tabel ini hanya berasal dari event. Pengirim event tidak boleh memanggil `Show` untuk pesan yang sama. Pesan lain, seperti `Inventori penuh` atau `Kargo kapal terdampar di pantai selatan`, dipanggil langsung oleh pemiliknya.

### Log event

`NotificationLogView` adalah tampilan di dalam panel `Pause`, dibuka lewat tombol `Log`, dan tidak menjadi `UIPanel` sendiri. Isi per baris: `Hari 12, 21:00  Base terbakar`. Hari berasal dari `GameManager.currentDay`, dan jam dari `HourChangedEvent.Last`. Di Fase 1 jamnya ditulis `Kapal`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** semua event di tabel di atas, dan `RunEndedEvent`. Status jeda dan adegan dibaca langsung dari `UIFocusStack` lewat `RefreshHeld()`.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `Initialize()` dan `ResetForNewRun()` |
| `UIFocusStack` (#83) | Sumber `isPaused` dan `TopPanel` |
| `LocalizationManager` (#104) | Teks pesan |
| `ItemDatabase` (#66) | Memastikan `itemId` sah |
| `AudioManager` | `PlaySFX(clip)` untuk suara per jenis |
| Pemanggil langsung | `ItemUseSystem`, `EquipmentManager`, `CraftingStation`, `StorageChest`, `CarryCapacity`, `HeavyItemSystem`, `HydrationSystem`, `InfectionSystem`, `DiseaseManager`, `DisasterManager`, `HordeManager`, `BeachSupplyManager`, `SupplyDropManager`, `FishingSystem`, `ResourceRespawn`, `IslandInteractable`, `RemoteBaseSimulation`, `ObjectiveManager`, `ToolRepairSystem`, dan `WorldStateSave` |
| `UIManager` | Panel `Pause` memuat tombol `Log` |
| `DevConsole` (#56) | Perintah `notify <jenis> <kunci>` |

**Batas pekerjaan:** prompt `E` milik `HUDManager` dan #17, indikator arah serangan milik #51, dan daftar objektif milik #81.

## 4. Kasus Khusus

- **Objek pool:** baris tidak memakai `ObjectPooler`, karena jumlahnya tetap, berada di canvas persisten, dan #82 memanggil `ReturnAllActive()` saat reset. `Bind` menimpa teks, warna, ikon, alpha, dan posisi setiap kali dipakai ulang, jadi tidak ada sisa pesan lama.
- **Game dijeda:** tampilan disembunyikan dan waktu berhenti. Pesan baru tetap masuk antrean dan log.
- **Permadeath:** `HandleRunEnded` mengosongkan `visible` dan `queue`. `ResetForNewRun()` juga mengosongkan `log` dan `announcedParts`. Riwayat tidak ikut ke run baru.
- **Save dan load:** tidak ada yang disimpan. Pesan bersifat sesaat, dan objektif dibaca dari #81. Setelah load, log dan `announcedParts` mulai kosong, jadi satu suku cadang bisa diumumkan sekali lagi. Dampaknya kecil.
- **Event saat memuat:** event yang datang saat `TopPanel.id == Loading` diabaikan, sehingga pemindahan `savedShipLoot`, pemulihan save, dan pengiriman ulang event sticky lewat `ISaveable` (#80) tidak membanjiri layar. Aturan ini bergantung pada panel `Loading` yang menutup seluruh proses memuat, dan dicek lewat tes 12 dan 13.
- **Pindah fase:** pesan yang tertahan selama `Loading` tetap tampil setelah scene baru siap.
- **Dipanggil sebelum `Initialize()`:** pemanggil lain berjalan setelah urutan ke-10, kecuali `SettingsManager` dan layanan lain yang lebih dulu. Layanan itu tidak memanggil `Show` saat boot. Kalau tetap terjadi, pesan masuk `queue` dan tampil setelah siap.
- **Banyak pesan dalam satu frame:** penggabungan dan batas antrean mencegah layar penuh, misalnya saat 50 kayu diambil dari kargo.
- **Item tidak ditemukan di `ItemDatabase`:** baris dilewati dan Editor mencatat peringatan.
- **`reduceFlashing` (#104):** tidak ada kedipan pada sistem ini, jadi tidak perlu aturan tambahan.
- **Teks panjang:** baris memakai lebar tetap dan pindah baris paling banyak dua kali. Tinggi baris mengikuti isi.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** urutan bootstrap ke-10 dan canvas persisten (`RunLifecycle.md`). `Base terbakar` dari `StructureBurningEvent` (`FireSystem.md`).

**Keputusan teknis di blueprint ini:**

- Baris dibuat satu kali dan dipakai ulang.
- Pesan sejenis digabung dalam jendela waktu.
- Log berupa ring buffer di memori.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Pemanggilan langsung:** rekomendasi sistem lain memanggil `NotificationManager.Instance.Show(...)` secara langsung, seperti `AudioManager`, dan event hanya untuk tabel di atas. Blueprint yang ada juga menulisnya begitu. Pilihan lain adalah event permintaan, tetapi hasilnya lebih panjang tanpa manfaat besar.
2. **Log event:** rekomendasi tampil di panel `Pause`, tanpa `UIPanelId` baru. Issue menyebut "Event Log" pada judul, tetapi tidak merinci isinya.
3. **Posisi:** rekomendasi pojok kanan atas, dengan pesan terbaru di bawah.
4. **Nilai awal:** 4 baris, 3 detik untuk pesan biasa, dan 5 detik untuk pesan penting.
5. **Barang berkurang:** rekomendasi tidak ditampilkan.

## 6. Skenario Tes

Belum diuji di Unity, karena proyek Unity belum dibuat.

1. `additem Mat_Wood 5`. **Hasil:** tampil `+5 Kayu`, lalu hilang setelah 3 detik.
2. Ambil kayu 3 kali dalam 2 detik. **Hasil:** satu baris yang angkanya naik.
3. Isi semua slot, lalu ambil item. **Hasil:** `Inventori penuh` berwarna kuning. Ulangi 3 kali. **Hasil:** satu baris dengan `x3`.
4. Ambil Heavy Item. **Hasil:** `Suku cadang ditemukan` berjenis `Objective`.
5. Pasang suku cadang. **Hasil:** `Suku cadang terpasang 1/5`.
6. Kirim 20 kali `notify info` dalam satu frame. **Hasil:** paling banyak 4 baris tampil, antrean tidak melebihi 12, dan tidak ada error.
7. Dengan baris penuh pesan `Info`, kirim `notify danger`. **Hasil:** satu baris lama langsung pudar, dan peringatan tampil.
8. Tekan Esc saat ada pesan. **Hasil:** pesan disembunyikan. Tutup Pause. **Hasil:** pesan lanjut dengan sisa waktu yang sama.
9. Buka `Inventory`, lalu lepas tas yang terisi. **Hasil:** `Kosongkan tas dulu` tampil di atas panel.
10. Picu adegan `Cinematic`, lalu kirim pesan. **Hasil:** pesan muncul setelah adegan selesai.
11. Buka `Pause`, lalu tombol `Log`. **Hasil:** riwayat terlihat dari yang terbaru, lengkap dengan hari dan jam.
12. Pindah dari kapal ke pulau dengan barang hasil jarahan. **Hasil:** tidak ada banjir `+` saat memuat.
13. Simpan, keluar, lalu lanjutkan. **Hasil:** tidak ada banjir `+`, dan log kosong.
14. Mati, lalu mulai run baru. **Hasil:** tidak ada pesan lama, dan log kosong.
15. Buka Profiler selama 60 detik tanpa pesan baru. **Hasil:** tidak ada alokasi GC dari `Update`.
16. Buka `Inventory`, picu adegan `helipad_found`, lalu kirim `notify info`. **Hasil:** pesan disembunyikan selama adegan, dan tampil dengan sisa waktu penuh setelah adegan selesai.
