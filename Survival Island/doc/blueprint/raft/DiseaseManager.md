# Blueprint: DiseaseManager.cs
**Lokasi:** `Survival Island/doc/blueprint/raft/DiseaseManager.md`
**Deskripsi Utama:** Penyakit sekunder di luar infeksi zombie: keracunan makanan, racun ular, pendarahan, dan infeksi luka, beserta pemicu, efek, dan obatnya.
**Issue:** #45
**Rujukan GDD:** Bagian 3 (air kotor berisiko sakit). Keracunan daging mentah 50 persen berasal dari checklist issue. Pendarahan, infeksi luka, dan obat-obatan adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pola

`DiseaseManager` berada di pemain Fase 2, di scene `IslandPhase`. Efek pada tubuh dijalankan `StatusEffectManager` (#20). `DiseaseManager` hanya menentukan kapan penyakit dimulai, kapan berubah, dan apa obatnya.

```csharp
public enum DiseaseType { FoodPoisoning, Venom, Bleeding, WoundInfection }

[Flags]
public enum DiseaseCure { None = 0, FoodPoisoning = 1, Venom = 2, Bleeding = 4, WoundInfection = 8 }
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `DiseaseManager` | `null` | Singleton per scene |
| `activeMask` | `DiseaseCure` | `None` | Penyakit yang aktif |
| `bleedingEndGameHour` | `float` | -1 | Waktu pendarahan berubah menjadi infeksi luka |
| `effects` | `DiseaseEffectTable` | Dari Inspector | Pasangan `DiseaseType` dan `StatusEffectData` |

### Tabel penyakit

| Penyakit | Pemicu | Durasi | HP per detik | Batas stamina | Regenerasi stamina | Kecepatan | HP pulih | Obat |
|---|---|---|---|---|---|---|---|---|
| `FoodPoisoning` | Makanan mentah, busuk, atau air kotor | 3 jam game | -0,25 (`Poison`) | x0,75 | x1 | x0,9 | Tidak | `Med_Norit` |
| `Venom` | Gigitan Ular (`DamageType.Poison`) | 2 jam game | -0,5 (`Poison`) | x0,6 | x0,5 | x0,8 | Tidak | `Med_Norit` |
| `Bleeding` | Luka besar dari musuh | 6 jam game, lalu menjadi infeksi luka | -0,3 (`Melee`) | x1 | x1 | x1 | Tidak | `Med_Bandage`, `Med_Medkit` |
| `WoundInfection` | Pendarahan tidak diobati | Sampai diobati | 0 | x0,85 | x0,7 | x1 | Tidak | `Med_Antibiotic` |

- Keracunan dari makanan menyerap sekitar 28 HP selama 112 detik, jadi tidak membunuh pemain sehat, tetapi berbahaya kalau HP rendah.
- **Infeksi luka tidak menguras HP.** Penyakit ini tidak mematikan sendiri, tetapi mencegah HP pulih sampai diobati.
- Pendarahan memakai `DamageType.Melee` supaya `DeathReason` tetap `KilledByEnemy`, tanpa menambah enum baru.

### Peluang pemicu

Mengikuti `ItemUseSystem.md`: peluang dihitung di sini, dengan `RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay)`.

| Sumber | Peluang | Sumber nilai |
|---|---|---|
| `Food_RawMeat` | 0,5 | `ItemData.poisonChance`, dari checklist issue |
| `Food_RawFish` | 0,5 | `ItemData.poisonChance`, disamakan dengan daging mentah |
| `Drink_DirtyWater` | 0,35 | `ItemData.poisonChance` (#43) |
| Item `Rotten` | `max(poisonChance, items.rottenPoisonChance)`, yaitu minimal 0,8 | `ItemUseSystem.md` |
| Item `Stale` atau `Fresh` | `poisonChance` item | `ItemUseSystem.md` |
| Serangan melee musuh, `amount >= bleedMinDamage` | `bleedChance` | Config di bawah |

### Item medis

`ItemData` mendapat field `curesDiseases : DiseaseCure`, dengan nilai awal `None`.

| Item | ID | Mengobati | HP | Pakai | Cara mendapat |
|---|---|---|---|---|---|
| Norit | `Med_Norit` | `FoodPoisoning`, `Venom` | 0 | 2 detik | Resep: 2 `Mat_Charcoal` (#44, #77), atau loot |
| Perban | `Med_Bandage` | `Bleeding` | +5 | 3 detik | Resep: 2 `Mat_Cloth`, atau loot |
| Kotak obat | `Med_Medkit` | `Bleeding` | +40 | 5 detik | Kapal (#118), loot |
| Antibiotik | `Med_Antibiotic` | `WoundInfection` | 0 | 2 detik | Resep di `MedicalBench`: 1 `Mat_TreeSap` dan 1 `Mat_Mushroom`, atau loot |

Antibiotik tidak mengubah `Infection` zombie. Vaksin tetap satu-satunya obat gigitan (#64).

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.disease.foodPoisonGameHours` | 3 | Durasi keracunan |
| `GameBalanceConfig.disease.venomGameHours` | 2 | Durasi racun ular |
| `GameBalanceConfig.disease.bleedingGameHours` | 6 | Lama pendarahan sampai menjadi infeksi luka |
| `GameBalanceConfig.disease.foodPoisonDamagePerSec` | 0,25 | HP per detik |
| `GameBalanceConfig.disease.venomDamagePerSec` | 0,5 | HP per detik |
| `GameBalanceConfig.disease.bleedingDamagePerSec` | 0,3 | HP per detik |
| `GameBalanceConfig.disease.bleedChance` | 0,25 | Peluang pendarahan per pukulan |
| `GameBalanceConfig.disease.bleedMinDamage` | 15 | Damage minimum yang bisa memicu pendarahan |
| `GameBalanceConfig.disease.overlayMaxAlpha` | 0,18 | Puncak warna hijau layar |
| `GameBalanceConfig.disease.overlayPulseSeconds` | 2,5 | Satu kedipan |

Pengali stamina dan kecepatan berada di aset `StatusEffectData` tiap penyakit, yang namanya tercatat di `StatusEffectManager.md`.

## 2. Detail Logika & Fungsi

### Dari makanan

**`public void OnConsumed(ItemData item, Freshness freshness)`**, dipanggil `ItemUseSystem.CompleteUse` langkah 7:

1. Kalau `item.curesDiseases != None`, panggil `CureFromItem(item)`, lalu selesai. Obat tidak meracuni.
2. Hitung `chance = item.poisonChance`. Kalau `freshness == Rotten`, `chance = Mathf.Max(chance, items.rottenPoisonChance)`.
3. Kalau `chance <= 0`, selesai.
4. `ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay)`. Kalau `rng.NextFloat01() < chance`, panggil `Apply(DiseaseType.FoodPoisoning, item.itemId)`.

Hasil acak dari aliran `Gameplay`, jadi keluar lalu masuk lagi tidak mengubah hasil (#89).

### Dari serangan

**`void HandleDamaged(PlayerDamagedEvent e)`**:

1. Kalau `e.info.type == DamageType.Poison`, panggil `Apply(DiseaseType.Venom, e.info.sourceId)`.
2. Kalau `type == Melee`, `!byPlayer`, `amount >= bleedMinDamage`, dan `!Has(Bleeding)`, ambil acak dengan `bleedChance`. Kalau kena, panggil `Apply(DiseaseType.Bleeding, sourceId)`.

### Memberi penyakit

**`public void Apply(DiseaseType type, string sourceId)`**:

1. Kalau `type == WoundInfection` dan sudah aktif, selesai.
2. Kalau `type == Bleeding`, `bleedingEndGameHour = TotalGameHours + bleedingGameHours`.
3. `StatusEffectManager.Instance.Apply(effects.Get(type), sourceId)`.
4. Tambah bit `activeMask`.
5. Terbitkan `DiseaseChangedEvent { type, isActive = true }`, lalu notifikasi (#22): `Kamu keracunan`, `Kamu berdarah`, dan seterusnya.

Efek yang sama memakai `EffectStackRule.Refresh`, jadi makan daging mentah lagi hanya mengisi durasi.

### Akhir penyakit

**`void HandleEffectChanged(StatusEffectChangedEvent e)`**: kalau `isActive == false` dan `effectId` milik penyakit:

1. Hapus bit `activeMask`.
2. Terbitkan `DiseaseChangedEvent { type, isActive = false }`.

**`void HandleHour(HourChangedEvent e)`**: kalau `Bleeding` aktif dan `TotalGameHours >= bleedingEndGameHour`:

1. `StatusEffectManager.Remove(Debuff_Bleeding)`.
2. `Apply(DiseaseType.WoundInfection, "wound")`.

**Pemilik akhir pendarahan:** aset `Debuff_Bleeding` memakai `EffectDurationUnit.Permanent` dan `Reject` (`StatusEffectManager.md`). Kalau durasinya diatur `StatusEffectManager`, efek itu sudah terhapus sebelum `HourChangedEvent` berikutnya, sehingga `Bleeding` tidak pernah berubah menjadi `WoundInfection`. Karena itu hanya `DiseaseManager` yang mengakhiri pendarahan, lewat `bleedingEndGameHour` di atas atau obat.

### Obat

**`public bool CanUseMedicine(ItemData item)`**, dipanggil `ItemUseSystem.TryUse` langkah 2:

- Kalau `(activeMask & item.curesDiseases) != None`, kembalikan `true`.
- Kalau `item.healAmount > 0` dan HP belum penuh, kembalikan `true`. Perban dan kotak obat tetap berguna tanpa pendarahan.
- Selain itu, tampilkan `Tidak diperlukan`, dan kembalikan `false`.

**`private void CureFromItem(ItemData item)`**: untuk tiap bit di `activeMask & item.curesDiseases`, panggil `StatusEffectManager.Remove(effectId)`. `HandleEffectChanged` mengurus sisanya. `bleedingEndGameHour = -1` kalau pendarahan sembuh.

HP dari obat ditangani `ItemUseSystem`, lewat `RestoreHP`. Obat memakai jalur pakai yang sama, jadi terkena serangan membatalkan pemakaian tanpa kehilangan obat.

### Efek layar dan UI

`DiseaseOverlay` adalah `Image` hijau penuh layar di canvas HUD, tanpa `UIPanel`:

```csharp
float t = Mathf.PingPong(Time.time / pulseSeconds, 1f);
overlay.alpha = isPoisoned ? Mathf.Lerp(0f, overlayMaxAlpha, t) : 0f;
```

- Aktif untuk `FoodPoisoning` dan `Venom`. Layar berkedip hijau lembut, sesuai checklist.
- `Bleeding` dan `WoundInfection` hanya memakai ikon (#20) dan efek tepi merah (#23).
- `Time.time` berhenti saat dijeda, sehingga kedipan juga berhenti.
- Kalau pengaturan aksesibilitas `reduceFlashing` aktif (#104), layar memakai warna tetap dengan alpha setengah, tanpa kedipan.
- `DiseaseOverlay` bisa dipindah ke `PostProcessController` (#23) setelah #23 selesai, tanpa mengubah sumber datanya.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `DiseaseChangedEvent`.

**Event yang didengar:** `PlayerDamagedEvent`, `StatusEffectChangedEvent`, dan `HourChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `StatusEffectManager` (#20) | Menjalankan efek, durasi, dan damage |
| `ItemUseSystem` (#66) | Memanggil `OnConsumed` dan `CanUseMedicine` |
| `InfectionSystem` (#64) | Pemilik infeksi zombie dan vaksin. Tidak saling mengubah |
| `HydrationSystem` (#43) | Air kotor |
| `CampfireManager` (#44) | Makanan matang, dan arang untuk Norit |
| `WildlifeAI` (#61) | Ular mengirim `DamageType.Poison` |
| `CraftingStation` (#77), `RegionLootTable` (#103) | Resep dan loot obat |
| `RunSeedManager` (#89) | Aliran `Gameplay` |
| `PostProcessController` (#23), `HUDManager` | Efek dan ikon |
| `LocalizationManager` (#104) | Pengaturan `reduceFlashing` |
| `WorldStateSave` (#80) | Menyimpan status |
| `NotificationManager` (#22) | Pesan |

**Batas pekerjaan:** infeksi zombie di #64, racun ular sebagai serangan di #61, dan efek post-processing di #23.

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool. Musuh dari pool hanya mengirim `DamageInfo`, dan tidak menyimpan rujukan ke penyakit.
- **Game dijeda:** durasi berhenti, karena efek memakai jam game. `HourChangedEvent` tidak terkirim. Panel tanpa jeda tetap menjalankan penyakit.
- **Permadeath:** `activeMask` dan efek hilang bersama scene dan save run (#82). Run baru mulai sehat.
- **Save dan load:** `SaveId = player_disease`. Disimpan `activeMask` dan `bleedingEndGameHour`. Sisa durasi disimpan `StatusEffectManager`, yang dipulihkan lebih dulu. `DiseaseManager` tidak memanggil `Apply` ulang saat dimuat, supaya durasi tidak penuh lagi.
- **Mati karena keracunan:** `lastDamageType = Poison`, jadi `DeathReason.Poison`, lewat `DeathReasonMap`.
- **Mati karena pendarahan:** `lastDamageType = Melee`, jadi `DeathReason.KilledByEnemy`.
- **Beberapa penyakit sekaligus:** semuanya berjalan, dan pengali dikalikan di #20. HP bisa turun lebih cepat, karena penyakit di luar tabel kondisi HP di `VitalsRules.md`.
- **Tabel kondisi HP:** efek penyakit berjalan bersama baris lapar dan haus. `blocksHPRegen` mencegah HP pulih selama penyakit aktif, lewat `SurvivalStats.SetHPRegenBlocked` (`VitalsRules.md`). Regenerasi stamina tidak ikut tertahan.
- **Godmode (#56):** HP tertahan di 1, dan penyakit tetap aktif.
- **Tidur saat sakit:** penyakit berjalan lebih cepat dalam waktu nyata, dan bisa membunuh saat tidur. Pembatasan tidur saat sakit ada di #67.
- **Fase 1:** `DiseaseManager` tidak ada di `ShipPhase`, jadi makanan kapal tidak diuji di sana. Makanan tidak dimakan di Fase 1 (#75).
- **Obat tanpa penyakit:** ditolak, kecuali item punya `healAmount` dan HP belum penuh.
- **`itemId` obat diganti:** `ItemDatabase` melaporkan saat load (#80).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** peluang 50 persen daging mentah (checklist issue ini). `rottenPoisonChance` (`ItemUseSystem.md`). `Zombified` hanya dari `Infection` (`GLOSSARY.md`). Aliran `Gameplay` (`RunSeedManager.md`).

**Keputusan teknis di blueprint ini:**

- Efek tubuh dijalankan `StatusEffectManager`, bukan kode sendiri.
- Racun ular dibaca dari `DamageType.Poison`, sehingga `WildlifeAI` tidak mengenal `DiseaseManager`.
- Pendarahan memakai `Melee`, tanpa enum baru.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Daftar penyakit:** keracunan makanan, racun ular, pendarahan, dan infeksi luka.
2. **Peran Antibiotik:** hanya untuk infeksi luka. Checklist issue menulis `Antibiotik untuk infeksi`, dan GDD menjadikan vaksin satu-satunya obat gigitan zombie.
3. **Norit juga mengobati racun ular,** supaya tidak perlu item penawar baru.
4. **Peluang pendarahan:** 25 persen pada pukulan melee dengan damage minimal 15.
5. **Infeksi luka tidak mematikan sendiri,** dan hanya menghentikan pemulihan HP serta menurunkan stamina.
6. **Air kotor:** peluang keracunan 35 persen.
7. **Efek layar:** kedipan hijau alpha sampai 0,18 per 2,5 detik, dengan pilihan tanpa kedip.
8. **Resep obat:** Norit dari arang, Perban dari kain, dan Antibiotik dari bahan Region 2.

## 6. Skenario Tes

1. Makan `Food_RawMeat` 20 kali. **Hasil:** sekitar 10 kali keracunan, sesuai aliran `Gameplay`.
2. Makan `Food_CookedMeat` 20 kali. **Hasil:** tidak pernah keracunan.
3. Saat keracunan, perhatikan layar. **Hasil:** berkedip hijau lembut, ikon tampil, kecepatan 90 persen, dan HP turun 0,25 per detik.
4. Tunggu 3 jam game. **Hasil:** semua efek hilang setelah sekitar 112 detik.
5. Buka Pause saat keracunan. **Hasil:** kedipan dan durasi berhenti.
6. Minum `Med_Norit` saat keracunan. **Hasil:** efek langsung hilang, dan `DiseaseChangedEvent { isActive = false }`.
7. Minum `Med_Norit` saat sehat. **Hasil:** ditolak, dan item tetap.
8. Biarkan Ular menggigit. **Hasil:** `Venom` aktif, dan HP turun 0,5 per detik.
9. `balance disease.bleedChance 1`, lalu biarkan zombie memukul dengan damage 15. **Hasil:** `Bleeding` aktif.
10. Biarkan 6 jam game, dengan pendarahan dimulai di tengah jam (misalnya 10:30). **Hasil:** `Debuff_Bleeding` tetap aktif sampai `HourChangedEvent` pertama setelah 16:30, lalu berubah menjadi `WoundInfection`, HP berhenti turun, dan HP tidak pulih. Selama pendarahan, HP juga tidak pulih.
11. Pakai `Med_Bandage` saat `Bleeding`. **Hasil:** sembuh, HP naik 5, dan tidak berubah menjadi `WoundInfection`.
12. Pakai `Med_Antibiotic` saat `WoundInfection`. **Hasil:** sembuh, dan HP mulai pulih.
13. Pakai `Med_Antibiotic` dengan `Infection` 60 tanpa infeksi luka. **Hasil:** ditolak, dan `Infection` tetap 60.
14. Mulai pakai obat, lalu biarkan zombie memukul. **Hasil:** batal, dan obat tetap.
15. Keracunan sambil makan. **Hasil:** makan tidak batal karena damage keracunan.
16. Biarkan HP habis saat keracunan. **Hasil:** `DeathReason.Poison`.
17. Simpan saat keracunan dengan sisa 1 jam game, keluar, lalu lanjutkan. **Hasil:** sisa tetap 1 jam game, dan kedipan kembali.
18. Aktifkan `reduceFlashing`. **Hasil:** warna hijau tetap, tanpa kedipan.
19. Mati, lalu mulai run baru. **Hasil:** tidak ada penyakit, dan overlay kosong.
