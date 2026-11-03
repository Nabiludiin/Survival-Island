# Blueprint: DevConsole.cs
**Lokasi:** `docs/blueprint/Core_Logic/DevConsole.md`
**Deskripsi Utama:** Konsol perintah tersembunyi untuk debugging dan testing, supaya developer bisa melompat ke kondisi tertentu tanpa bermain dari awal. Hanya ada di Editor dan build pengembangan.
**Issue:** #56
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Membantu menguji semua sistem, termasuk hari ke-60 dan helikopter di bagian 3.

## 1. Arsitektur & Variabel Utama

### Pola

`DevConsole` adalah manager persisten (`IBootstrapService`, urutan ke-18 di #82) dan sekaligus `UIPanel` dengan id `DevConsole` (#83).

Seluruh kelas berada di dalam `#if UNITY_EDITOR || DEVELOPMENT_BUILD`. Di build rilis, kelas ini tidak ada, dan Action Map `Debug` tidak dibuat (#18). `RunLifecycle` tidak memasukkannya ke `orderedServices` di prefab rilis.

```csharp
public delegate string ConsoleHandler(string[] args);

public sealed class ConsoleCommand
{
    public string name;        // Huruf kecil, tanpa spasi
    public string usage;       // Contoh: "additem <itemId> [jumlah]"
    public string help;        // Satu kalimat
    public int minArgs;
    public ConsoleHandler run; // Mengembalikan teks hasil
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `DevConsole` | `null` | Singleton |
| `commands` | `Dictionary<string, ConsoleCommand>` | Terisi saat `Initialize()` | Daftar perintah |
| `outputLines` | `string[]` | Panjang `console.maxOutputLines` | Riwayat keluaran, berupa ring buffer |
| `history` | `List<string>` | Kosong | Perintah yang pernah diketik |
| `historyIndex` | `int` | -1 | Posisi saat menekan panah |
| `IsRunTainted` | `bool` | `false` | Run ini memakai cheat |
| `inputField` | `TMP_InputField` | Dari prefab | Kolom ketik |
| `outputText` | `TMP_Text` | Dari prefab | Keluaran |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.console.maxOutputLines` | 200 | Baris yang disimpan |
| `GameBalanceConfig.console.maxHistory` | 50 | Perintah yang diingat |

### UI

Panel menempel di bagian atas layar, setinggi 40 persen, dengan latar hitam transparan. Isinya keluaran, kolom ketik, dan satu baris penanda `[CHEAT]` kalau `IsRunTainted`. Panel tidak punya tombol, dan tidak tampil di menu mana pun.

| Pengaturan `UIPanel` (dari #83) | Nilai |
|---|---|
| Menjeda waktu | Ya |
| Melepas kursor | Ya |
| Blokir input gameplay | Ya |
| Tutup dengan Esc | Ya |
| Terkunci | Tidak |
| Pengecualian `rejectsNewPanels` | Boleh masuk walau `Loading`, `Cinematic`, atau `GameOver` di puncak |

## 2. Detail Logika & Fungsi

### Membuka dan menutup

**`Update()`** membaca `InputManager.Instance.ConsoleTogglePressed` (Action Map `Debug`, tombol `` ` `` atau `~`):

- Kalau panel tertutup: `UIFocusStack.Instance.Push(this)`.
- Kalau panel terbuka: `UIFocusStack.Instance.Close(this, UICloseReason.User)`.

**`OnFocusGained()`** memanggil `inputField.ActivateInputField()`. **`Show()`** mengatur `onValidateInput`, supaya karakter `` ` `` dan `~` tidak ikut masuk ke kolom ketik.

Karena `Gameplay` mati saat panel terbuka, mengetik `e` tidak memicu interaksi. `UIFocusStack` tidak memakai `toggleAction` untuk panel ini, karena `Update` di atas sudah menanganinya, dan Action Map `Debug` selalu aktif.

**Tombol saat mengetik:**

| Tombol | Aksi |
|---|---|
| `Enter` | Jalankan perintah, lalu kosongkan kolom |
| Panah atas dan bawah | Riwayat perintah |
| `Tab` | Lengkapi nama perintah. Kalau lebih dari satu cocok, tampilkan daftarnya |
| `Esc` | Tutup panel, lewat `UIFocusStack.HandleEscape()` |

### Parser

**`private void Submit(string line)`**:

1. Pangkas spasi. Kalau kosong, selesai.
2. Tambahkan ke `history`, dan cetak `> line`.
3. `tokens = Tokenize(line)`.
4. `name = tokens[0].ToLowerInvariant()`.
5. Kalau `name` tidak ada di `commands`, cetak `Perintah tidak dikenal: <name>. Ketik help.`
6. `args` berisi sisa token. Kalau `args.Length < minArgs`, cetak `usage`.
7. Panggil `run(args)` di dalam `try/catch`. Pengecualian dicetak merah dan tidak menghentikan game. Hasil berupa teks dicetak.

**`private string[] Tokenize(string line)`** memisahkan dengan spasi, dan teks di dalam tanda kutip dianggap satu token. Contoh: `say "dua kata"` menghasilkan `say` dan `dua kata`.

**Pembacaan angka** memakai `float.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out value)`, supaya komputer dengan format Indonesia tetap membaca `0.5`. Boolean menerima `1`, `0`, `on`, `off`, `true`, dan `false`. Nilai yang salah mencetak `usage`.

### Pendaftaran perintah

**`public void Register(ConsoleCommand command)`** menambah perintah. Nama ganda dicatat sebagai peringatan, dan pendaftar terakhir yang berlaku.

Perintah bawaan didaftarkan di `Initialize()`. Sistem lain boleh mendaftarkan perintahnya sendiri, dengan `#if` yang sama. Perintah tidak boleh menyimpan referensi ke objek scene, karena `DevConsole` persisten dan scene bisa berganti. Handler mencari `Instance` saat dipanggil, lalu mencetak `Hanya di IslandPhase` kalau `null`.

### Perintah utama

Perintah yang mengubah kondisi permainan mengatur `IsRunTainted = true`.

| Perintah | Contoh | Hasil | Memakai |
|---|---|---|---|
| `help [nama]` | `help additem` | Daftar perintah atau rincian satu perintah | Registry |
| `clear` | `clear` | Mengosongkan keluaran | Internal |
| `godmode <0/1>` | `godmode 1` | HP tidak habis | `SurvivalStats.SetInvulnerable` |
| `additem <itemId> [jumlah]` | `additem Mat_Wood 50` | Menambah item. Sisa yang tidak muat menjadi `WorldItem` | `InventoryManager.AddItem`, `ItemDatabase` |
| `removeitem <itemId> [jumlah]` | `removeitem Mat_Wood 50` | Mengurangi item | `InventoryManager.RemoveItem` |
| `setday <n>` | `setday 59` | Melompat ke hari tertentu | `GameManager.DebugSetDay` |
| `settime <jam>` | `settime 17.5` | Mengatur jam | `DayNightDirector.DebugSetTime` |
| `setvital <nama> <nilai>` | `setvital hunger 0` | `hp`, `stamina`, `hunger`, `thirst`, atau `infection` | `SurvivalStats.DebugSetVital` |
| `kill [alasan]` | `kill Frostbite` | Membunuh pemain dengan `DeathReason` tertentu | `SurvivalStats.DebugKill` |
| `tp <x> <y> <z>` | `tp 500 40 1750` | Memindahkan pemain | `IslandPlayerController.Teleport`, `ResetFallTracking` |
| `tpregion <region>` | `tpregion Snow` | Pindah ke titik uji region | `RegionStreaming` |
| `spawn <enemyId> [jumlah]` | `spawn zombie_militer 5` | Memunculkan musuh di depan pemain | `SpawnManager.DebugSpawn` |
| `killall` | `killall` | Mengembalikan semua musuh ke pool | `SpawnManager.DebugReturnAll` |
| `timescale <nilai>` | `timescale 3` | Skala waktu gameplay | `UIFocusStack.SetGameplayTimeScale` |
| `balance <bagian>.<nama> <nilai>` | `balance vitals.hungerDrainPerSec 0.2` | Mengubah angka di salinan config | Lihat di bawah |
| `eventlog <0/1>` | `eventlog 1` | Mencetak event yang terkirim | `GameEventRegistry.LogHook` |
| `seed [XXXX-XXXX]` | `seed 7F3A-91C2` | Tampil, atau mengisi `forcedNextSeed` | `RunSeedManager` (#89) |
| `layout` | `layout` | Titik suku cadang, NPC, dan jadwal bencana | `RunSeedManager` (#89) |
| `streamstat`, `streamdelay <detik>` | `streamdelay 15` | Isi di #88 | `RegionStreaming` (#88) |
| `disaster <tipe> [skala]` | `disaster Eruption 2` | Memicu bencana | `DisasterManager` (#70) |
| `unlock <region>` | `unlock Amazon` | Membuka region | `RegionGateSystem` (#68) |
| `save`, `loadlast` | `save` | Memaksa save, atau kembali ke menu dan melanjutkan | `SaveRequestedEvent`, `RunLifecycle` |
| `savecorrupt` | `savecorrupt` | Mengubah satu byte file save utama, untuk tes #80 | `WorldStateSave` (#80) |
| `savedelay <detik>` | `savedelay 5` | Menunda penulisan save, untuk tes #80 | `WorldStateSave` (#80) |
| `spawnstat` | `spawnstat` | Musuh aktif, jumlah `Submerged`, dan kegagalan mencari titik spawn | `SpawnManager` (#62) |
| `settide <low\|high>` | `settide high` | Memaksa level air | `TideManager` (#91) |
| `notify <jenis> <kunci>` | `notify Warning notif.test` | Memanggil `NotificationManager.Instance.Show(kunci, jenis)` | `NotificationManager` (#22) |
| `postfx <efek> <0..1>` | `postfx infection 0.5` | Memaksa target efek | `PostProcessController` (#23) |
| `shake <kind> [scale]` | `shake Earthquake 2` | Memanggil `CameraShake.Play(kind, scale)` | `CameraShake` (#24) |

Sistem lain menambah `stress <jumlah>` (#93), `softlockcheck` (#116), dan sejenisnya.

### `godmode`

`godmode 1` memanggil `SurvivalStats.SetInvulnerable(true)`. HP tertahan di 1, infeksi tertahan di 99, dan damage tetap menampilkan efek. Nilainya tidak ikut save, dan `ResetForNewRun()` mematikannya, supaya uji permadeath tidak tertipu.

### `additem`

```csharp
string CmdAddItem(string[] args)
{
    var data = ItemDatabase.Get(args[0]);
    if (data == null) return $"Item tidak dikenal: {args[0]}";
    int amount = args.Length > 1 ? int.Parse(args[1]) : 1;
    int left = InventoryManager.Instance.AddItem(data, amount);
    if (left > 0) WorldItemManager.Instance.Spawn(new ItemInstance(data), left, inFrontOfPlayer, despawns: true);
    return $"Ditambah {amount - left}, jatuh {left}";
}
```

Issue menulis `additem wood 50`. Perintah menerima alias pendek lewat `ItemDatabase.ResolveAlias`, misalnya `wood` menjadi `Mat_Wood`. `Tab` melengkapi ID item setelah token pertama.

### `setday`

**`GameManager.DebugSetDay(int day)`**:

1. Tolak kalau `day` di luar rentang 1 sampai `MaxDay`. Hari ke-61 diuji dengan `setday 60` lalu menunggu jam melewati 24:00, sesuai #82.
2. `currentDay = day`.
3. Terbitkan `DayChangedEvent { newDay = day }` sekali.
4. Tidak memanggil `AdvanceDay()` berulang, jadi kargo mingguan (#71), bencana (#70), dan pembusukan tidak mengejar hari yang terlewati.

Sistem yang melihat hari lewat `DayChangedEvent` hanya bereaksi pada hari tujuan. Karena jadwal bencana dibuat dari seed (#89), `setday 59` tetap memberi jadwal yang sama.

**Untuk menguji helikopter:** `setday 59`, `tpregion Snow`, lalu perintah helikopter dari #14.

### `balance`

**`balance <bagian>.<nama> <nilai>`** memakai reflection, hanya di build pengembangan:

1. Ambil salinan dari `GameManager.Instance.Balance`, yang dibuat `Instantiate` saat run mulai.
2. Cari `FieldInfo` dari `bagian`, lalu `nama`. Kalau tidak ada, cetak nama yang mirip.
3. Ubah dengan tipe `float`, `int`, atau `bool`, lalu cetak nilai lama dan baru.

Aset asli di Editor tidak berubah, karena yang diubah adalah salinan. Objek pool mengambil nilai baru pada kemunculan berikutnya.

### Log event

`eventlog 1` memasang `GameEventRegistry.LogHook = AppendEventLine`. `EventChannel<T>.Publish` memanggil hook itu kalau tidak `null`, dengan `typeof(T).Name` dan nilai `ToString()` struct. Baris masuk ke `outputLines`. Fitur ini mengalokasi memori, dan hanya berjalan saat diaktifkan.

### Run yang memakai cheat

`IsRunTainted` diatur saat perintah mengubah kondisi. Akibatnya:

- `LocalStatsManager.RecordRunEnd` (#57) tidak mencatat run ini.
- Layar akhir menampilkan `[CHEAT]` di samping seed.
- `IsRunTainted` kembali ke `false` saat `ResetForNewRun()`.

Perintah baca seperti `help`, `seed` tanpa argumen, `layout`, dan `streamstat` tidak mengubahnya.

### Inisialisasi dan reset

**`public void Initialize()`** membangun `commands`, lalu memuat `history` dari `PlayerPrefs` dengan kunci `dev.console.history`.

**`public void ResetForNewRun()`** mematikan godmode, `eventlog`, dan `timescale`, serta mengatur `IsRunTainted = false`. `history` dan `forcedNextSeed` tetap ada.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak punya event sendiri. Perintah bisa menerbitkan `DayChangedEvent` lewat `GameManager` dan `SaveRequestedEvent`.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | Action Map `Debug`, dan `ConsoleTogglePressed` |
| `UIFocusStack` (#83) | `Push`, `Close`, dan `SetGameplayTimeScale` |
| `RunLifecycle` (#82) | Menjalankan `Initialize` dan `ResetForNewRun` |
| `GameEventBus` (#107) | `GameEventRegistry.LogHook`, untuk `eventlog` |
| `GameBalanceConfig` (#84) | Perintah `balance` |
| `RunSeedManager` (#89) | Perintah `seed` dan `layout` |
| `RegionStreaming` (#88) | Perintah `streamstat` dan `streamdelay` |
| `SurvivalStats` (#111), `InventoryManager`, `SpawnManager` (#62), #14, #68, #70 | Tujuan perintah |
| `LocalStatsManager` (#57) | Membaca `IsRunTainted` |
| #93, #116 | Mendaftarkan perintah uji sendiri |

Sistem gameplay tidak boleh bergantung pada `DevConsole`. Fungsi bernama `Debug*` di sistem lain ditutup `#if` yang sama, dan tidak dipanggil dari gameplay.

## 4. Kasus Khusus

- **Objek pool:** `spawn` dan `killall` memakai jalur `SpawnManager` dan `ObjectPooler` yang sama seperti permainan biasa, jadi reset dan `IPoolable` ikut teruji. Konsol sendiri tidak memakai pool.
- **Game dijeda:** panel menjeda waktu, dan `Update` tetap membaca tombol. Teks memakai waktu tak terskala. `settime` dan `setday` langsung berlaku, tetapi efek waktu baru berjalan setelah panel ditutup.
- **Permadeath:** `ResetForNewRun()` mematikan cheat. `history` dan `forcedNextSeed` tetap, karena dipakai run berikutnya.
- **Save dan load:** tidak ada data di save, kecuali satu penanda `wasTainted` di data `GameManager`, supaya run yang dilanjutkan tetap ditandai. Riwayat perintah berada di `PlayerPrefs`, terpisah dari save run.
- **Scene belum dimuat:** `Instance` per scene bernilai `null`, dan handler mencetak pesan, bukan error.
- **Membuka konsol saat `GameOver`:** diperbolehkan oleh #83. Perintah `setday` atau `kill` ditolak saat `RunLifecycle.State` bukan `InShip` atau `OnIsland`.
- **`kill` saat `godmode 1`:** `DebugKill` melewati `IsInvulnerable`, supaya uji alur mati tetap bisa dilakukan.
- **`timescale` saat panel menjeda:** nilainya tersimpan, dan berlaku saat panel tertutup (#83).
- **`Time.timeScale` langsung:** perintah `timescale` tidak boleh menulis ke `Time.timeScale`, sesuai #83.
- **Perintah melempar pengecualian:** dicetak merah, dan panel tetap berjalan.
- **Build rilis:** tombol `~` tidak melakukan apa pun.
- **Bentrok `Tab` dan `ToggleInventory`:** `UIFocusStack` hanya memeriksa `toggleAction` panel puncak, dan puncaknya `DevConsole`, jadi `Tab` tidak menutup inventori di bawahnya.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** tombol `~` dan Action Map `Debug` (#18). Panel menjeda waktu dan boleh masuk di atas panel terkunci (#83). Urutan bootstrap ke-14 (#82). Perintah `seed`, `layout`, `balance`, `eventlog`, `streamstat`, dan `streamdelay` (#84, #88, #89, #107).

**Keputusan teknis di blueprint ini:**

- Kelas dikecualikan penuh dari build rilis.
- Perintah didaftarkan lewat kode, bukan atribut dan pencarian reflection.
- Reflection hanya dipakai `balance`.
- `setday` tidak menjalankan hari yang terlewati.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Build rilis tanpa konsol:** pemain tidak bisa mengaktifkannya. Ini sejalan dengan #82, dan menjaga permadeath.
2. **`setday` tidak mengejar:** kargo, bencana, dan pembusukan pada hari yang terlewati tidak berjalan. Kalau uji butuh kondisi lengkap, perintah terpisah bisa ditambah.
3. **Penanda `[CHEAT]`:** run yang memakai cheat tidak masuk statistik (#57).
4. **Alias item:** nama pendek seperti `wood`, sesuai contoh issue.

## 6. Skenario Tes

1. Tekan `~` di Editor. **Hasil:** konsol terbuka, waktu berhenti, kursor bebas, dan kolom ketik aktif.
2. Ketik `e` dan `Tab` saat panel terbuka. **Hasil:** teks atau pelengkapan masuk, tanpa interaksi dan tanpa membuka inventori.
3. Tekan `~` lagi. **Hasil:** konsol tertutup, dan karakter `~` tidak tertinggal.
4. Ketik `godmode 1`, lalu biarkan zombie menyerang. **Hasil:** HP berhenti di 1, dan `[CHEAT]` tampil.
5. Ketik `additem wood 50`. **Hasil:** 50 `Mat_Wood` masuk. Saat inventori penuh, sisanya jatuh.
6. Ketik `setday 59`. **Hasil:** `currentDay` 59, dan `DayChangedEvent` terkirim sekali. Kargo hari ke-56 tidak muncul.
7. Ketik `setday 60`, lalu `settime 23.9`. **Hasil:** setelah jam lewat 24:00, alur `TimeExpired` berjalan.
8. Ketik `setday abc` dan `setday 100`. **Hasil:** pesan cara pakai atau rentang, tanpa error.
9. Ketik `balance vitals.hungerDrainPerSec 5`. **Hasil:** lapar turun cepat, dan aset asli tidak berubah.
10. Ketik `eventlog 1`, lalu bunuh zombie. **Hasil:** `EnemyKilledEvent` tercetak.
11. Ketik `seed 7F3A-91C2`, lalu Run Baru. **Hasil:** seed itu dipakai, dan peringatan tercatat kalau gagal validasi.
12. Ketik `kill Frostbite` saat `godmode 1`. **Hasil:** alur mati berjalan dengan alasan `Frostbite`.
13. Buka konsol di `GameOver`, lalu ketik `setday 5`. **Hasil:** ditolak dengan pesan.
14. Mati, lalu mulai run baru. **Hasil:** godmode mati, `[CHEAT]` hilang, dan riwayat perintah tetap ada.
15. Buka komputer dengan format angka Indonesia, lalu ketik `settime 17.5`. **Hasil:** jam menjadi 17:30.
16. Buat build rilis. **Hasil:** tombol `~` tidak bereaksi, dan tidak ada kelas `DevConsole` di build.
