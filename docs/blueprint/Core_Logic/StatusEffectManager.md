# Blueprint: StatusEffectManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/StatusEffectManager.md`
**Deskripsi Utama:** Pengatur buff dan debuff sementara pada pemain. Satu daftar efek aktif, satu aturan tumpuk, dan satu tempat menggabungkan pengali ke `SurvivalStats` serta kecepatan gerak.
**Issue:** #20
**Rujukan GDD:** Bagian 7 menyebut buff HP dan Stamina permanen dari Lucky Machine. Efek sementara, termasuk contoh suplemen di issue ini, adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`StatusEffectManager` berada di pemain Fase 2, di scene `IslandPhase`. Data efek berada di ScriptableObject, jadi efek baru tidak butuh kode baru.

```csharp
public enum EffectDurationUnit { Seconds, GameHours, Permanent }
public enum EffectStackRule { Refresh, Extend, Intensify, Reject }

[CreateAssetMenu(menuName = "Survival/Status Effect")]
public class StatusEffectData : ScriptableObject
{
    public string effectId;                  // Contoh: Buff_Supplement
    public string displayNameKey;            // Kunci teks untuk #104
    public Sprite icon;
    public bool isBuff;

    public EffectDurationUnit durationUnit;
    public float duration;                   // Mengikuti durationUnit
    public EffectStackRule stackRule;
    public int maxStacks = 1;

    public float moveSpeedMultiplier = 1f;
    public float maxStaminaMultiplier = 1f;
    public float staminaRegenMultiplier = 1f;
    public bool blocksHPRegen;

    public float damagePerSecond;            // Damage kondisi. 0 berarti tanpa damage
    public DamageType damageType;
}

public struct ActiveEffect
{
    public StatusEffectData data;
    public string sourceId;                  // itemId, jenis penyakit, atau sistem pemanggil
    public int stacks;
    public float remainingSeconds;           // Dipakai saat Seconds
    public float endGameHour;                // Dipakai saat GameHours
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `active` | `List<ActiveEffect>` | Kosong, kapasitas `maxActive` | Efek yang berjalan |
| `database` | `StatusEffectDatabase` | Dari Inspector | Pencarian `effectId`, dipakai saat load |
| `cachedSpeed`, `cachedMaxStamina`, `cachedRegen` | `float` | 1 | Hasil gabungan terakhir |
| `cachedBlockRegen` | `bool` | `false` | Hasil gabungan terakhir |
| `Instance` | `StatusEffectManager` | `null` | Singleton per scene |

### Daftar efek awal

| `effectId` | Pemberi | Durasi | Tumpuk | Pengaruh |
|---|---|---|---|---|
| `Buff_Supplement` | `Med_Supplement` | 60 detik | `Refresh` | Kecepatan lari dan jalan x1,2, sesuai contoh issue |
| `Debuff_FoodPoisoning` | #45 | 3 jam game | `Refresh` | Isi di `DiseaseManager.md` |
| `Debuff_Venom` | #45 | 2 jam game | `Refresh` | Isi di `DiseaseManager.md` |
| `Debuff_Bleeding` | #45 | 6 jam game | `Refresh` | Isi di `DiseaseManager.md` |
| `Debuff_WoundInfection` | #45 | Permanen sampai diobati | `Reject` | Isi di `DiseaseManager.md` |

Buff permanen Lucky Machine (#72) memakai `Permanent`, dengan arti "selama satu run" kalau #72 memutuskan demikian.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.statusEffects.maxActive` | 8 | Batas efek aktif |
| `GameBalanceConfig.statusEffects.supplementSeconds` | 60 | Durasi `Buff_Supplement` |
| `GameBalanceConfig.statusEffects.supplementSpeedMultiplier` | 1,2 | Kecepatan `Buff_Supplement` |
| `GameBalanceConfig.statusEffects.expiringWarningSeconds` | 5 | Ikon berkedip menjelang habis |

Nilai `StatusEffectData` yang tidak ada di tabel ini berada di asetnya sendiri, dan dapat diganti lewat `GameBalanceConfig` oleh pemilik efeknya.

## 2. Detail Logika & Fungsi

### Memberi efek

**`public bool Apply(StatusEffectData data, string sourceId)`**:

1. Cari efek dengan `effectId` yang sama.
2. Kalau ada, jalankan aturan tumpuk:

| Aturan | Akibat |
|---|---|
| `Refresh` | Durasi kembali penuh, dan `stacks` tetap |
| `Extend` | Durasi bertambah sebesar `duration` |
| `Intensify` | `stacks` naik sampai `maxStacks`, dan durasi kembali penuh |
| `Reject` | Tidak berubah. Kembalikan `false` |

3. Kalau tidak ada dan `active.Count >= maxActive`, tolak dan catat peringatan. Debuff dari sistem wajib selalu masuk: yang digantikan adalah buff dengan sisa durasi terpendek.
4. Kalau tidak ada, tambahkan `ActiveEffect` baru. `endGameHour = DayNightDirector.Instance.TotalGameHours + duration` untuk `GameHours`.
5. Panggil `Recalculate()`, lalu terbitkan `StatusEffectChangedEvent { effectId, isActive = true, stacks }`.

**`public bool ApplyById(string effectId, string sourceId)`** mencari lewat `database`. `ItemUseSystem.CompleteUse` memanggilnya kalau `ItemData.appliesEffectId` terisi.

**`public bool Remove(string effectId)`** menghapus efek, memanggil `Recalculate()`, dan menerbitkan event dengan `isActive = false`.

**`public bool Has(string effectId)`** dan **`public IReadOnlyList<ActiveEffect> Active`** dipakai untuk membaca. HUD membaca `Active` saat aktif, dan mendengar event untuk perubahan.

### Waktu

**`void Update()`**, kalau `active.Count > 0`:

1. `float dt = Time.deltaTime`. Kalau `dt <= 0`, selesai.
2. Dari akhir daftar ke awal, untuk tiap efek:
   - `Seconds`: `remainingSeconds -= dt`. Kalau habis, hapus.
   - `GameHours`: kalau `TotalGameHours >= endGameHour`, hapus.
   - `Permanent`: tidak berubah.
3. Kalau `damagePerSecond > 0`, panggil `SurvivalStats.ApplyVitalDamage(damagePerSecond * stacks * dt, damageType)`.
4. Kalau ada yang terhapus, panggil `Recalculate()`, lalu terbitkan event.

Pemakaian `GameHours` membuat penyakit ikut berjalan cepat saat tidur (#67) dan berhenti saat dijeda. Buff pendek memakai detik, supaya terasa sama setiap kali diminum.

Damage kondisi tidak mengirim `PlayerDamagedEvent`, jadi tidak membatalkan pemakaian obat dan tidak memunculkan indikator arah (#51). Damage ini berjalan bersama tabel prioritas HP di `VitalsRules.md`, karena efek ini bukan kondisi tubuh dasar.

### Menggabungkan pengali

**`private void Recalculate()`**, tanpa alokasi memori:

```csharp
float speed = 1f, maxStamina = 1f, regen = 1f;
bool blockRegen = false;
for (int i = 0; i < active.Count; i++)
{
    var e = active[i];
    for (int s = 0; s < e.stacks; s++)
    {
        speed *= e.data.moveSpeedMultiplier;
        maxStamina *= e.data.maxStaminaMultiplier;
        regen *= e.data.staminaRegenMultiplier;
    }
    blockRegen |= e.data.blocksHPRegen;
}
```

Lalu, hanya kalau nilainya berubah:

- `SurvivalStats.SetPenalty(VitalPenaltySource.StatusEffect, maxStamina, regen)`.
- `IslandPlayerController.SetSpeedModifier(SpeedModifierId.StatusEffect, speed)`.
- `SurvivalStats.SetRegenBlocked(blockRegen)`.

Semua efek yang aktif dikalikan, sehingga urutan pemberian efek tidak mengubah hasil. `VitalsRules.md` perlu aturan khusus untuk sumber `StatusEffect`, karena buff di atas 1 tidak boleh hilang oleh perhitungan nilai terkecil.

### Tampilan

`StatusEffectBar` adalah bagian `HUDManager`, tanpa `UIPanel`:

- Satu ikon untuk tiap efek, dengan warna buff hijau dan debuff merah.
- Lingkaran sisa waktu diperbarui 4 kali per detik.
- Ikon berkedip di bawah `expiringWarningSeconds`. Efek `Permanent` tidak menampilkan lingkaran.
- Angka tumpukan tampil kalau `stacks > 1`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `StatusEffectChangedEvent`.

**Event yang didengar:** `PlayerDiedEvent` (berhenti menghitung).

| Sistem | Hubungan |
|---|---|
| `SurvivalStats`, `VitalsRules` (#111) | Menerima `SetPenalty`, `SetRegenBlocked`, dan `ApplyVitalDamage` |
| `IslandPlayerController`, `PlayerTraversal` (#101) | Menerima `SetSpeedModifier(StatusEffect, ...)` |
| `ItemUseSystem` (#66) | Memanggil `ApplyById` saat item selesai dipakai |
| `DiseaseManager` (#45) | Memakai efek ini untuk semua penyakit |
| `LuckyMachine` (#72) | Memakai `Permanent` untuk buff hadiah |
| `HUDManager` | Menampilkan `StatusEffectBar` |
| `DayNightDirector` | Menyediakan `TotalGameHours` |
| `WorldStateSave` (#80) | Menyimpan daftar efek |

**Batas pekerjaan:** isi penyakit di #45, jenis hadiah di #72, dan efek layar di #23.

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool. Efek bukan `GameObject`, dan ikon HUD dipakai ulang dari daftar tetap sejumlah `maxActive`.
- **Game dijeda:** `Time.deltaTime` bernilai 0 dan `TotalGameHours` berhenti, jadi durasi dan damage tidak berjalan. Panel tanpa jeda, seperti inventori, tetap menjalankan waktu.
- **Permadeath:** semua efek hilang bersama scene dan save run (#82). Nilai penalti `StatusEffect` kembali ke 1, karena `SurvivalStats` baru dibuat.
- **Save dan load:** `SaveId = player_status_effects`. Disimpan daftar `{ effectId, sourceId, stacks, remainingSeconds, endGameHour }`. `RestoreState` berjalan setelah `DayNightDirector`, lalu memanggil `Recalculate()`. `effectId` yang tidak ditemukan dibuang, dengan peringatan.
- **Mati saat efek berjalan:** `PlayerDiedEvent` menghentikan `Update`, sehingga damage tidak berjalan setelah mati.
- **Efek habis tepat saat dimuat:** efek `GameHours` yang sudah lewat dihapus pada frame pertama.
- **Kecepatan melebihi batas:** `moveSpeedMultiplier` tetap dikalikan dengan pengali Heavy Item di `PlayerSpeedModifiers`. Membawa Heavy Item dengan suplemen menghasilkan 0,5 x 1,2 = 0,6.
- **Minum suplemen dua kali:** durasi kembali ke 60 detik, tanpa kecepatan bertumpuk.
- **Efek dari sistem yang dimatikan:** kalau #45 belum tersedia, efek penyakit tidak pernah diberikan. Tidak ada ketergantungan balik.
- **Daftar penuh:** debuff wajib menggantikan buff yang paling cepat habis, supaya penyakit tidak hilang.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** penalti gabungan lewat `SetPenalty` dan sumber `StatusEffect` (`VitalsRules.md`). `SpeedModifierId.StatusEffect` (`PlayerTraversal.md`). Hadiah buff permanen ada di GDD bagian 7, dan rinciannya di #72.

**Keputusan teknis di blueprint ini:**

- Efek disimpan sebagai data, dengan struct aktif di satu `List`.
- Durasi dipilih per efek: detik atau jam game.
- Damage dari efek memakai `ApplyVitalDamage`, tanpa `PlayerDamagedEvent`.
- Semua pengali dikalikan, tanpa urutan.

**Usulan yang menunggu developer:**

1. **Suplemen `Med_Supplement`:** kecepatan 20 persen selama 60 detik, dari issue ini. Item ini belum ada di GDD, dan sumbernya hanya loot (#103).
2. **Aturan tumpuk awal:** minum ulang hanya mengisi durasi.
3. **Batas 8 efek aktif.**

## 6. Skenario Tes

1. Minum `Med_Supplement`. **Hasil:** kecepatan naik 20 persen selama 60 detik, ikon tampil, lalu kecepatan kembali.
2. Minum lagi pada detik ke-30. **Hasil:** durasi kembali ke 60, dan kecepatan tetap 1,2 kali.
3. Bawa Heavy Item saat suplemen aktif. **Hasil:** kecepatan 0,6 kali normal.
4. Buka Pause selama 30 detik. **Hasil:** sisa waktu tidak berkurang.
5. Buka inventori selama 10 detik. **Hasil:** sisa waktu berkurang 10.
6. Beri `Debuff_FoodPoisoning` lewat #45, lalu tunggu 3 jam game. **Hasil:** efek habis setelah sekitar 112 detik, sesuai `timeMultiplier` 96.
7. Beri efek `GameHours`, lalu tidur (#67). **Hasil:** efek habis lebih cepat dalam waktu nyata.
8. Beri efek dengan `damagePerSecond` saat makan. **Hasil:** HP turun, dan makan tidak batal.
9. Beri efek `maxStaminaMultiplier` 1,2 saat lapar normal. **Hasil:** batas stamina 120. Saat lapar rendah. **Hasil:** batas stamina 90 (0,75 x 1,2).
10. Isi 8 efek, lalu beri debuff baru. **Hasil:** buff terdekat habis diganti, dan debuff aktif.
11. Simpan saat suplemen sisa 20 detik, keluar, lalu lanjutkan. **Hasil:** sisa 20 detik, dan kecepatan langsung aktif.
12. Mati saat efek aktif, lalu mulai run baru. **Hasil:** tidak ada efek, dan semua pengali 1.
13. Buka Profiler dengan 8 efek aktif. **Hasil:** `Update` tanpa alokasi GC.
