# Blueprint: LocalStatsManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/LocalStatsManager.md`
**Deskripsi Utama:** Mencatat statistik gaya bermain di file lokal `stats.json`, untuk menu utama dan layar akhir. Data akumulatif tidak terhapus saat permadeath.
**Issue:** #57
**Rujukan GDD:** Usulan, belum ada di GDD. Mendukung siklus Permadeath di bagian 1 dan 3.

## 1. Arsitektur & Variabel Utama

`LocalStatsManager` adalah singleton persisten dan `IBootstrapService`, urutan ke-17 pada bootstrap (#82). Sistem ini hanya mendengar event dan tidak memengaruhi gameplay. Data tidak dikirim ke mana pun, walaupun judul issue memakai kata "Telemetry".

```csharp
[Serializable]
public class RunCounters
{
    public uint seed;
    public bool isCommitted;     // Sudah masuk total
    public bool isTainted;       // Memakai cheat (#56)
    public int zombiesKilled;
    public int animalsKilled;
    public int shotsFired;
    public int shotsMissed;
    public int headshots;
    public int npcsRescued;
    public int dayReached;
    public float playSeconds;
}

[Serializable] public class DeathCount { public string reason; public int count; }

[Serializable]
public class LocalStatsData
{
    public int schemaVersion = 1;
    public int totalRuns, totalWins;
    public int totalZombiesKilled, totalAnimalsKilled;
    public int totalShotsFired, totalShotsMissed, totalHeadshots;
    public int longestDaySurvived;
    public float totalPlaySeconds;
    public List<DeathCount> deaths = new();
    public string lastDeathReason;
    public uint lastSeed;
    public RunCounters currentRun = new();
}
```

`deaths` memakai `string`, bukan angka enum, supaya menambah nilai `DeathReason` tidak mengacaukan file lama. Bentuknya daftar, karena `JsonUtility` tidak bisa menyimpan `Dictionary`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `LocalStatsManager` | `null` | Singleton |
| `Data` | `LocalStatsData` | Dibaca dari file | Isi `stats.json` |
| `isDirty` | `bool` | `false` | Ada perubahan yang belum ditulis |
| `flushTimer` | `float` | 0 | Waktu nyata sejak tulis terakhir |

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.stats.flushIntervalSeconds` | 60 | Jeda tulis berkala |

### File

| File | Lokasi | Saat permadeath |
|---|---|---|
| `stats.json` | `Application.persistentDataPath` | **Tetap** |
| `stats.json.tmp` | Sama | Sementara saat menulis |
| `stats.json.bak` | Sama | Salinan terakhir yang valid |

File ini terpisah dari `survival_save.json`. `WorldStateSave.DeleteRunSave()` tidak menyentuhnya (`WorldStateSave.md`).

## 2. Detail Logika & Fungsi

### Metrik

| Metrik | Sumber | Cara hitung |
|---|---|---|
| Zombie dibunuh | `EnemyKilledEvent` | `byPlayer == true` dan `enemyId` berawalan `zombie_` |
| Hewan dibunuh | `EnemyKilledEvent` | `byPlayer == true` dan `enemyId` lainnya |
| Tembakan | `WeaponFiredEvent` | `byPlayer == true` |
| Tembakan meleset | `ShotResolvedEvent` (baru) | `hitEnemy == false` |
| Headshot | `ShotResolvedEvent` | `isHeadshot == true` |
| NPC selamat | `NPCRescuedEvent` | +1 |
| Kargo dibuka | `BeachCrateOpenedEvent` (#71) | +1 |
| Hari terlama bertahan | `RecordRunEnd` | `Max(longestDaySurvived, day)` |
| Penyebab kematian terbanyak | `RecordRunEnd` | `deaths` terbesar. Kalau seri, yang terakhir terjadi |
| Waktu bermain | `Update` | `Time.deltaTime`, hanya saat `RunStartedEvent.HasLast` |

**Kenapa perlu `ShotResolvedEvent`:** `EnemyHitEvent` juga dikirim oleh serangan melee dan tidak membawa jenis senjata, jadi selisih tembakan dan hit tidak akurat. Satu tembakan senapan dengan banyak peluru dihitung satu hasil.

### Fungsi utama

**`public void Initialize()`** membaca `stats.json`:

1. Kalau file tidak ada: `Data = new`.
2. Kalau rusak: pindahkan ke `stats.corrupt.json`, lalu coba `stats.json.bak`. Kalau gagal juga, `Data = new`.
3. Kalau `schemaVersion` lebih baru dari versi game: baca sebagai `new`, dan jangan menimpa file sampai ada perubahan yang dicatat.

**`public void ResetForNewRun()`** tidak menghapus apa pun. `RunStartedEvent` yang mengatur penghitung run.

**`HandleRunStarted(RunStartedEvent e)`**:

1. Kalau `e.isNewRun == false`: lanjutkan `currentRun`, tanpa perubahan.
2. Kalau ada `currentRun` yang belum `isCommitted` dan bukan `isTainted`: panggil `CommitCounters()`. Ini run yang ditinggalkan pemain lewat Run Baru, jadi angkanya masuk total, tanpa menambah `deaths`.
3. `currentRun = new RunCounters { seed = e.seed }`.
4. `isDirty = true`.

**`public void RecordRunEnd(RunResult result, DeathReason reason, int day, uint seed)`**, dipanggil `RunLifecycle.RunEndSequence` dan `RecoverInterruptedEnd`:

1. Kalau `currentRun.seed == seed` dan `currentRun.isCommitted`: selesai. Ini mencegah catatan ganda saat game mati di antara langkah 6 dan 7 (#82). Run baru dengan seed sama tidak terpengaruh, karena `currentRun` sudah dibuat ulang.
2. `currentRun.dayReached = day`.
3. Kalau `DevConsole.IsRunTainted` atau `currentRun.isTainted`: `isCommitted = true`, tulis, lalu selesai (#56).
4. `CommitCounters()`, `totalRuns++`.
5. `Won`: `totalWins++`. Selain itu: tambah `deaths[reason]`, dan isi `lastDeathReason`.
6. `longestDaySurvived = Max(longestDaySurvived, day)`, `lastSeed = seed`.
7. `Flush()`.

**`private void CommitCounters()`** menambah penghitung run ke total, lalu `currentRun.isCommitted = true`.

**`private void Flush()`** menulis secara atomik:

1. Tulis JSON ke `stats.json.tmp`.
2. Kalau `stats.json` ada: `File.Replace(tmp, stats.json, stats.json.bak)`. Kalau belum ada: `File.Move`.
3. `IOException` ditangkap dan dicatat, dan `isDirty` tetap `true`. Kegagalan statistik tidak boleh menghentikan permadeath.

**Kapan menulis:** setiap `flushIntervalSeconds` waktu nyata kalau `isDirty`, saat `SaveCompletedEvent`, saat `Application.wantsToQuit`, dan di `RecordRunEnd`.

**Saat `Flush`, `currentRun.isTainted`** diisi dari `DevConsole.IsRunTainted`, karena nilai itu kembali `false` saat `ResetForNewRun`, sebelum `RunStartedEvent` berikutnya.

### Fungsi baca dan tampilan

| Fungsi | Dipakai oleh | Isi |
|---|---|---|
| `DeathReason? GetTopDeathReason()` | `StatsPanel` | Penyebab terbanyak |
| `float GetAccuracy()` | `StatsPanel` | `(fired - missed) / fired`, atau 0 kalau `fired == 0` |
| `RunCounters CurrentRun` | Layar `GameOver` dan `WinScreen` | Angka run yang baru selesai |
| `uint LastSeed` | Laporan bug (#89) | Seed terakhir |
| `public void ResetAllStats()` | Tombol di `StatsPanel`, lewat `ConfirmDialog` | Mengosongkan `Data`, lalu `Flush` |

**Menu utama:** tombol `Statistik` membuka `StatsPanel` lewat `UIFocusStack.Push`. Panel menampilkan jumlah run, menang, hari terlama, zombie dibunuh, akurasi, dan penyebab kematian terbanyak.

**Layar akhir:** menampilkan `CurrentRun`: hari, zombie dibunuh, dan tembakan meleset. Kalau `isTainted`, angka tetap tampil dengan tanda `[CHEAT]`, dan tidak masuk total.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `EnemyKilledEvent`, `WeaponFiredEvent`, `ShotResolvedEvent` (baru), `NPCRescuedEvent`, `BeachCrateOpenedEvent`, `RunStartedEvent`, `SaveCompletedEvent`.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `Initialize`, `ResetForNewRun`, dan `RecordRunEnd` |
| `GunController` (#58) | Menerbitkan `ShotResolvedEvent` |
| `EnemyHealth` (#59) | Menerbitkan `EnemyKilledEvent` |
| `DevConsole` (#56) | `IsRunTainted`, dan perintah `stats`, `statsreset` |
| `RunSeedManager` (#89) | Seed untuk laporan bug |
| `WorldStateSave` (#80) | Tidak menyentuh `stats.json` |

**Batas pekerjaan:** koleksi lore yang bertahan setelah permadeath ada di #100, dan catatan adegan yang sudah ditonton ada di #99.

## 4. Kasus Khusus

- **Objek pool:** musuh yang kembali ke pool tanpa dibunuh tidak dihitung, karena hanya `EnemyKilledEvent` yang dipakai. `killall` lewat konsol memakai `DebugReturnAll`, jadi tidak menambah statistik.
- **Game dijeda:** `playSeconds` memakai `Time.deltaTime`, sehingga berhenti saat jeda. Tulis berkala memakai waktu nyata, dan tetap aman.
- **Permadeath:** `stats.json` tetap. `RunLifecycle` tidak pernah memanggil `ResetAllStats`. `PlayerPrefs.DeleteAll()` dilarang dan tidak relevan, karena data ini berada di file sendiri.
- **Lanjutkan run:** `currentRun` berada di `stats.json`, jadi penghitung run bertahan tanpa `ISaveable`. Kalau proses mati sebelum tulis berkala, paling banyak 60 detik statistik hilang.
- **Save lama dimuat setelah crash:** angka run bisa sedikit lebih tinggi dari save. Ini diterima, karena statistik tidak memengaruhi gameplay.
- **Game ditutup paksa saat sinematik:** `RecoverInterruptedEnd` memanggil `RecordRunEnd`, dengan `currentRun` dari file (#82).
- **Mati di Fase 1:** dicatat seperti biasa, dengan `day == 1`, dan alasan seperti `LeftOnShip`.
- **Tembakan oleh NPC:** `byPlayer == false`, jadi diabaikan.
- **File dihapus pemain:** dimulai dari nol, tanpa error.
- **`DeathReason.None`:** tidak pernah masuk `deaths`, karena hanya dipakai saat `Won`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- File `stats.json`, urutan bootstrap 13, dan `RecordRunEnd(result, reason, currentDay, seed)` (`RunLifecycle.md`).
- Run dengan cheat tidak masuk statistik (`DevConsole.md`).

**Usulan yang menunggu developer (dicatat di #85):**

1. **Sistem ini secara keseluruhan:** belum ada di GDD, tetapi `RunLifecycle.md` sudah memakainya. Rekomendasi dipakai, karena biayanya kecil dan memberi alasan mencoba lagi setelah permadeath.
2. **Kata "Telemetry":** rekomendasi lokal saja, tanpa pengiriman data.
3. **Run yang ditinggalkan:** rekomendasi angkanya masuk total, tetapi tidak dihitung sebagai run atau kematian.
4. **Tombol hapus statistik:** rekomendasi ada, dengan `ConfirmDialog`.
5. **Metrik tambahan:** rekomendasi `animalsKilled`, `headshots`, `npcsRescued`, dan `playSeconds`, di luar empat metrik checklist.

## 6. Skenario Tes

1. Hapus `stats.json`, lalu buka game. **Hasil:** panel `Statistik` menampilkan nol, tanpa error.
2. Bunuh 3 zombie dengan pistol, lalu mati. **Hasil:** `zombiesKilled` bertambah 3, dan `totalRuns` bertambah 1.
3. Tembak 5 kali ke udara. **Hasil:** `shotsMissed` bertambah 5.
4. Tembak kepala zombie sekali. **Hasil:** `headshots` bertambah 1, dan `shotsMissed` tetap.
5. Bunuh zombie dengan pisau. **Hasil:** `shotsFired` tidak berubah.
6. Mati, lalu periksa folder. **Hasil:** `survival_save.json` hilang, dan `stats.json` tetap dengan isi baru.
7. Isi `deaths` di `stats.json` uji dengan `Frostbite` 2 dan `Drowned` 1, lalu buka panel `Statistik`. **Hasil:** penyebab terbanyak `Frostbite`.
8. Pakai `additem`, lalu mati. **Hasil:** total tidak berubah, dan layar akhir menampilkan `[CHEAT]`.
9. Matikan proses setelah `RecordRunEnd`, sebelum `ClearRunEndMarker`. **Hasil:** saat boot berikutnya, `totalRuns` tidak bertambah dua kali.
10. Rusakkan isi `stats.json`. **Hasil:** file dipindah ke `stats.corrupt.json`, data dipulihkan dari `.bak`.
11. Keluar ke menu, lalu Lanjutkan. **Hasil:** `currentRun.zombiesKilled` tetap.
12. Run Baru saat ada run berjalan. **Hasil:** `totalZombiesKilled` menerima angka run lama, dan `totalRuns` tidak bertambah.
13. Hari ke-20 mati, lalu run berikutnya hari ke-5 mati. **Hasil:** `longestDaySurvived` tetap 20.
