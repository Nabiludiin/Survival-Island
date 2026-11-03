# Blueprint: GameBalanceConfig.cs (ScriptableObject)
**Lokasi:** `docs/blueprint/Core_Logic/GameBalanceConfig.md`
**Deskripsi Utama:** Satu sumber angka penyetelan, dengan target pengalaman bermain yang jelas.
**Issue:** #84
**Rujukan GDD:** Bagian 1, 3, 5, dan 6. Semua angka di sini adalah usulan awal, dan harus disetel lewat playtest.

## 1. Arsitektur & Variabel Utama

```csharp
[CreateAssetMenu(menuName = "Survival/Game Balance Config")]
public class GameBalanceConfig : ScriptableObject
{
    public TimeBalance time;
    public VitalsBalance vitals;
    public ResourceBalance resource;
    public RegionBalance region;
    public WeatherBalance weather;
    public DifficultyProfile[] difficulties;
}
```

- `GameManager.Instance.Balance` menyimpan aset yang aktif. Sistem lain membacanya, bukan menulis angka sendiri.
- Tiap bagian adalah kelas `[Serializable]`, supaya mudah dibaca di Inspector.
- Sistem baru menambah bagiannya sendiri dan menulis namanya di blueprint, misalnya `GameBalanceConfig.infection`.

**Aturan angka:** Angka di blueprint lain ditulis sebagai `GameBalanceConfig.<bagian>.<nama>`, beserta nilai awalnya. Tabel di bawah menampung angka inti yang saling memengaruhi.

## 2. Detail Logika & Fungsi

### Target durasi (usulan)

| Target | Nilai | Alasan |
|---|---|---|
| Panjang satu hari game | 15 menit | Siang dan malam masing-masing sekitar 7,5 menit |
| Total 60 hari | 15 jam | Batas terlama satu run |
| Run menang umum | 10 sampai 15 jam | Pemain cepat selesai sebelum hari ke-60 |

Sebelumnya `timeMultiplier = 50` menghasilkan 28,8 menit per hari dan 28,8 jam total. Angka itu terlalu panjang untuk game permadeath yang mengulang dari awal.

### Angka inti

| Bagian | Nama | Nilai lama | Nilai usulan | Hasilnya |
|---|---|---|---|---|
| `time` | `timeMultiplier` | 50 | 96 | 1 hari = 900 detik |
| `time` | `dayLengthSeconds` | Tidak ada | 900 | Hanya informasi |
| `vitals` | `hungerDrainPerSec` | 0,3 | 0,11 | Lapar habis dalam 1 hari game |
| `vitals` | `thirstDrainPerSec` | Tidak ada | 0,15 | Haus habis dalam sekitar 0,75 hari game |
| `vitals` | `sprintThirstMultiplier` | Tidak ada | 1,5 | Berlari menguras air lebih banyak |
| `vitals` | `starvationDamagePerSec` | 1,5 | 0,5 | HP penuh habis dalam 200 detik |
| `vitals` | `dehydrationDamagePerSec` | Tidak ada | 0,5 | HP penuh habis dalam 200 detik |
| `vitals` | `frostbiteStaminaDrainPerSec` | Tidak ada | 5 | Stamina penuh habis dalam 20 detik |
| `vitals` | `frostbiteDamagePerSec` | 5 | 1 | Setelah stamina habis, HP penuh habis dalam 100 detik |
| `resource` | `treeDropMin` / `treeDropMax` | 2 / 4 | 8 / 12 | Rata-rata 10 kayu per pohon |
| `resource` | `axeDecayPerUse` | 5 | 1 | 100 ayunan per kapak, sekitar 50 pohon |
| `region` | `bridgeWoodCost` | 500 | 500 | Mengikuti keputusan #85 |
| `region` | `heavyItemSpeedMultiplier` | 0,5 | 0,5 | Sesuai `Claude.md` |
| `region` | `helicopterPartCount` | Tidak ada | 5 | Disetel bersama #69 |
| `weather` | `weatherIntervalGameHours` | 300 detik nyata | 8 jam game | 3 kali per hari game |

**Waktu cuaca:** cuaca memakai waktu game, bukan waktu nyata. Dengan begitu cuaca ikut melambat saat game dijeda dan ikut cepat saat tidur.

### Target makan dan minum

- **Makanan:** 3 kali per hari game, dengan pemulihan sekitar 33 per porsi.
- **Minuman:** 4 kali per hari game, dengan pemulihan sekitar 25 per porsi.
- **Sumber makanan yang perlu dicek:** satu sesi memancing sebaiknya menghasilkan 1 porsi dalam sekitar 60 detik, sehingga 3 porsi butuh 180 detik atau 40 persen waktu siang. Berburu mengisi sisanya dengan risiko lebih tinggi.

### Biaya bangun terhadap hasil panen

| Target | Perhitungan | Hasil |
|---|---|---|
| Satu pohon | 2 ayunan kapak (`maxHits` 5, damage 3) dan 20 detik berjalan serta memungut | Sekitar 25 detik per 10 kayu |
| Jembatan | 500 kayu = 50 pohon, 50 x 25 detik | 1.250 detik, sekitar 3 siang penuh |
| Base awal | 4 dinding, 1 pintu, 1 lantai, dan 1 atap. Sekitar 80 kayu | Sekitar 200 detik |
| Vaksin | 2 getah dan 2 jamur Region 2 per dosis. Sekitar 60 detik panen | Harus disetel bersama #64 |
| Kapak | 50 pohon menghabiskan 1 kapak penuh | Satu kapak cukup untuk jembatan |

Jembatan bisa selesai sekitar hari ke-5 sampai hari ke-8, kalau pemain juga makan, minum, dan membangun base. Angka ini memberi cukup waktu untuk region berikutnya.

### Perjalanan Region 4 ke Region 3

Asumsi: jarak 400 meter, dan 150 meter di antaranya berada di area dingin. Jarak final mengikuti #88.

| Hal | Perhitungan | Hasil |
|---|---|---|
| Kecepatan membawa barang | `walkSpeed` 5 x 0,5 | 2,5 m/s |
| Perjalanan berangkat | 400 / 2,5 | 160 detik |
| Perjalanan kembali | 400 / 5 | 80 detik |
| Satu kali bolak-balik | 160 + 80 | 240 detik |
| Semua suku cadang | 5 x 240 detik | 1.200 detik, sekitar 1,3 hari game tanpa pertempuran |

### Ketahanan frostbite

- **Area dingin saat membawa barang:** 150 / 2,5 = 60 detik.
- **Tanpa pakaian hangat:** stamina habis dalam 20 detik, lalu HP turun 40 selama 40 detik sisanya. Pemain selamat, tetapi tidak bisa bolak-balik berkali-kali tanpa perlindungan.
- **Nilai lama:** `frostbiteDamagePerSec = 5` membunuh pemain dari HP penuh dalam 20 detik, jadi satu kali perjalanan pasti mematikan.

### Profil kesulitan

`DifficultyProfile` berisi pengali lapar, haus, damage musuh, jumlah musuh, dan hasil panen. Versi awal hanya punya satu profil, `Normal`, dengan semua pengali bernilai 1.

### Lembar catatan playtest

Setiap sesi uji mengisi tabel ini di `docs/PLAYTEST_LOG.md`, yang dibuat saat Unity siap:

| Tanggal | Versi | Hari tercapai | Penyebab mati | Porsi makan per hari | Hari jembatan selesai | Waktu angkut suku cadang | Catatan |
|---|---|---|---|---|---|---|---|

## 3. Hubungan dengan Sistem Lain

- **Tidak mengirim atau mendengar event.** Config hanya dibaca.
- **`DayNightDirector`, `SurvivalStats`, `ResourceNode`, `DurabilitySystem`, `IslandPlayerController`, `WeatherManager`, dan `RegionGateSystem` (#68):** membaca angka dari config, bukan variabel `public` sendiri.
- **#27 `AIDirector`:** mengatur kurva zombie per hari di bagian config-nya sendiri.
- **#56 `DevConsole`:** perintah `balance <bagian>.<nama> <nilai>` mengubah angka saat playtest.

## 4. Kasus Khusus

- **Perubahan aset saat bermain:** di Editor, ScriptableObject yang diubah saat Play Mode tetap tersimpan. `DevConsole` mengubah salinan sementara yang dibuat `Instantiate` saat run mulai.
- **Pool:** objek pool membaca angka saat `OnEnable`, jadi perubahan lewat konsol berlaku pada kemunculan berikutnya.
- **Game dijeda:** config tidak berubah.
- **Permadeath dan save:** config tidak disimpan. Save hanya mencatat nama profil kesulitan.
- **Tidur:** tidur mempercepat `timeMultiplier` lewat `SleepSystem` (#67), tanpa mengubah nilai dasar di config.

## 5. Keputusan Desain

**Keputusan developer:** `bridgeWoodCost` bernilai awal 500.

**Usulan yang menunggu developer (dicatat di #85):**

- Panjang satu hari game 15 menit, dan total 15 jam.
- Hasil panen 8 sampai 12 kayu per pohon.
- `frostbiteDamagePerSec` diturunkan dari 5 ke 1.
- Jumlah suku cadang 5.
- Semua nilai lain di tabel angka inti.

## 6. Skenario Tes

1. Jalankan satu hari penuh. **Hasil:** berlangsung sekitar 900 detik, dan `DayChangedEvent` terkirim sekali.
2. Biarkan lapar turun dari 100 tanpa makan. **Hasil:** nol setelah sekitar 900 detik.
3. Tebang pohon sampai 500 kayu dan catat waktunya. **Hasil:** sekitar 3 siang penuh. Catat selisihnya di lembar playtest.
4. Angkut satu suku cadang dari Region 4 ke Region 3 tanpa pakaian hangat. **Hasil:** pemain sampai dengan HP berkurang sekitar 40.
5. Ubah `timeMultiplier` lewat `DevConsole`. **Hasil:** jam berubah, dan aset asli tidak ikut berubah.
