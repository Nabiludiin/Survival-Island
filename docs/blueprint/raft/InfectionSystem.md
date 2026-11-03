# Blueprint: InfectionSystem.cs
**Lokasi:** `docs/blueprint/raft/InfectionSystem.md`
**Deskripsi Utama:** Alur infeksi zombie dari gigitan sampai pengobatan: nilai infeksi per jenis musuh, empat tahap penalti, stok dan resep vaksin, pemakaian vaksin, serta urutan kematian `Zombified`.
**Issue:** #64
**Rujukan GDD:** Bagian 3 (infeksi naik karena serangan zombie, formula dan stok vaksin sangat sedikit, HP habis karena infeksi), bagian 4 (Diary Bab 2: vaksin dari getah pohon dan jamur Region 2), dan bagian 5 (Region 2: grinding material medis). Tahap, penalti, dan semua angka adalah usulan.

## 1. Arsitektur & Variabel Utama

### Pola

`InfectionSystem` adalah komponen di pemain Fase 2, di scene `IslandPhase`. `SurvivalStats` tetap satu-satunya pemilik nilai `Infection` (`GLOSSARY.md`). `InfectionSystem` hanya membaca nilainya, menentukan tahap, memasang penalti, dan mengatur efek vaksin.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `CurrentStage` | `int` | 1 | Tahap 1 sampai 4 |
| `stats` | `SurvivalStats` | Dari pemain | Sumber dan tujuan nilai infeksi |
| `warnedCritical` | `bool` | `false` | Peringatan `criticalWarningAt` sudah tampil |

Tahap ditulis sebagai `int`, sesuai `InfectionStageChangedEvent.stage` di `GameEventBus.md`.

### Tahap infeksi

Batas tahap 4 sama dengan `GameBalanceConfig.vitals.infectionDamageThreshold` (75), dan batas tahap 3 sama dengan `regenMaxInfection` (50), sesuai `raft/VitalsRules.md`.

| Tahap | `Infection` | Batas stamina | Regenerasi stamina | HP | Penglihatan (#23) | Tanda untuk pemain |
|---|---|---|---|---|---|---|
| 1 | 0 sampai kurang dari 25 | x1 | x1 | Pulih normal | 0 | Bar infeksi saja |
| 2 | 25 sampai kurang dari 50 | x0,9 | x1 | Pulih normal | 0,15 | Notifikasi `Mulai demam` |
| 3 | 50 sampai kurang dari 75 | x0,75 | x0,75 | Tidak pulih | 0,4 | Ikon infeksi dan napas berat |
| 4 | 75 sampai kurang dari 100 | x0,5 | x0,5 | -0,3 per detik | 0,8 | Bar berkedip dan peringatan |

Penglihatan adalah angka intensitas 0 sampai 1 untuk `PostProcessController` (#23). Bentuk efeknya milik #23.

### Kenaikan infeksi per jenis musuh

Nilai ini mengisi `AttackData.infection` (#109). `DamageInfo.infection` membawanya ke `SurvivalStats.TakeDamage`, lalu `AddInfection` (#59).

| Musuh | `enemyId` usulan | Nama config | Nilai awal | Alasan |
|---|---|---|---|---|
| Zombie standar | `zombie_standar` | `GameBalanceConfig.infection.gainStandard` | 5 | 20 pukulan sampai 100 |
| Zombie Tribal | `zombie_tribal` | `GameBalanceConfig.infection.gainTribal` | 6 | Region 2 |
| Zombie Lumut | `zombie_lumut` | `GameBalanceConfig.infection.gainMoss` | 8 | Menyergap, serangan jarang |
| Zombie Militer | `zombie_militer` | `GameBalanceConfig.infection.gainMilitary` | 10 | Musuh terkuat |
| Zombie Beku | `zombie_beku` | `GameBalanceConfig.infection.gainFrozen` | 5 | Ancamannya dari ledakan (#25) |
| Zombie Desa | `zombie_desa` | `GameBalanceConfig.infection.gainVillage` | 6 | Cepat dan sering memukul |
| Hewan buas | Semua `AnimalData` | Tetap 0 | 0 | GDD hanya menyebut serangan zombie |

Runner, Tank, dan Spitter mengikuti keputusan #25. Serangan yang mengenai dinding (`Structural`) tidak menambah infeksi.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.infection.stage2At` | 25 | Awal tahap 2 |
| `GameBalanceConfig.infection.stage3At` | 50 | Awal tahap 3 |
| `GameBalanceConfig.infection.stage4At` | 75 | Awal tahap 4 |
| `GameBalanceConfig.infection.stageMaxStamina` | `[1; 0,9; 0,75; 0,5]` | Batas stamina per tahap |
| `GameBalanceConfig.infection.stageStaminaRegen` | `[1; 1; 0,75; 0,5]` | Regenerasi stamina per tahap |
| `GameBalanceConfig.infection.stageVision` | `[0; 0,15; 0,4; 0,8]` | Intensitas untuk #23 |
| `GameBalanceConfig.infection.selfProgressPerGameHour` | 0 | Kenaikan sendiri. 0 berarti hanya naik saat diserang |
| `GameBalanceConfig.infection.selfProgressMinInfection` | 50 | Dipakai kalau nilai di atas lebih dari 0 |
| `GameBalanceConfig.infection.startingVaccineDoses` | 2 | Stok awal |
| `GameBalanceConfig.infection.vaccineReduction` | 50 | Infeksi yang turun per dosis |
| `GameBalanceConfig.infection.vaccineInjectSeconds` | 4 | Durasi suntik, mengisi `Med_Vaccine.useSeconds` |
| `GameBalanceConfig.infection.medicVaccineMultiplier` | 1,25 | Pengali vaksin kalau NPC medis hidup (#76) |
| `GameBalanceConfig.infection.criticalWarningAt` | 90 | Peringatan terakhir |
| `GameBalanceConfig.infection.zombifyCinematicSeconds` | 4 | Durasi adegan `Zombified` di #53 |

## 2. Detail Logika & Fungsi

### Menentukan tahap

**`void HandleVitals(VitalsChangedEvent e)`**, sekitar 4 kali per detik:

1. `int stage = GetStage(e.infection)`.
2. Kalau `stage != CurrentStage`, panggil `ApplyStage(stage)`.
3. Kalau `e.infection >= criticalWarningAt` dan `!warnedCritical`, tampilkan `Infeksi kritis, segera suntik vaksin` (#22), lalu `warnedCritical = true`.
4. Kalau `e.infection < criticalWarningAt`, `warnedCritical = false`.

**`int GetStage(float value)`** memeriksa `stage4At`, `stage3At`, lalu `stage2At`, dan mengembalikan 1 kalau tidak ada yang tercapai.

**`private void ApplyStage(int stage)`**:

1. Simpan `CurrentStage = stage`.
2. Panggil `stats.SetPenalty(VitalPenaltySource.Infection, stageMaxStamina[stage - 1], stageStaminaRegen[stage - 1])`.
3. Terbitkan `InfectionStageChangedEvent { stage }`.
4. Kalau tahap naik, tampilkan notifikasi tahap (#22).

`SurvivalStats` menghitung sendiri HP tahap 3 dan 4 dari `Infection`, lewat baris HP 5 dan `regenMaxInfection` di `VitalsRules.md`. Blueprint ini tidak mengurangi HP.

### Naik sendiri atau hanya saat diserang

**Rekomendasi: hanya saat diserang.** GDD dan `Claude.md` hanya menyebut kenaikan karena serangan. Tekanan waktu sudah datang dari tahap 4, yang menguras HP terus sampai pemain memakai vaksin.

Kalau developer memilih infeksi naik sendiri, cukup ubah `selfProgressPerGameHour`. **`void HandleHour(HourChangedEvent e)`**: kalau nilainya lebih dari 0 dan `Infection >= selfProgressMinInfection`, panggil `stats.AddInfection(selfProgressPerGameHour)`.

### Stok vaksin awal

- `IslandArrival` (#74) memberi `Med_Vaccine` sebanyak `startingVaccineDoses`, sesudah item inti dan sebelum sisa barang kapal (`CarryCapacity.ApplyShipLoot`).
- **Vaksin dari kapal menambah stok itu.** Vaksin kapal adalah barang biasa di `savedShipLoot`, dan bertumpuk di slot yang sama sampai `carry.foodStack`.
- Run tanpa barang tetap punya 2 dosis, sesuai GDD: stok sangat sedikit, tetapi ada.

### Formula dan resep vaksin

GDD menyebut MC memulai dengan formula. Jadi resep `rec_vaccine` memakai `startsUnlocked = true` di #77, dan Diary Bab 2 hanya menunjukkan tempat bahannya.

| Field `CraftingRecipe` | Nilai |
|---|---|
| `recipeId` | `rec_vaccine` |
| `ingredients` | 2 `Mat_TreeSap`, 2 `Mat_Mushroom` |
| `result`, `resultAmount` | `Med_Vaccine`, 1 |
| `craftSeconds` | 10 |
| `station` | `MedicalBench` |

Bahan itu sesuai `GameBalanceConfig.md`, dan sekitar 60 detik panen per dosis. `CraftingManager` mengatur antrean dan ruang inventori (#77).

### Node `MedicalHerb`

Tipe `NodeType.MedicalHerb` di `ResourceNode` punya dua varian. Aturan tumbuh kembali ada di #73.

| Varian | Prefab | Hasil | Alat minimum | Pukulan | Lokasi | Tumbuh kembali |
|---|---|---|---|---|---|---|
| Jamur | `Node_Mushroom` | 1 sampai 2 `Mat_Mushroom` | Tangan | 1 | Hanya Region 2, di bawah pohon rapat | 1 hari game |
| Pohon getah | `Node_SapTree` | 1 sampai 2 `Mat_TreeSap` | `Base_Pisau` | 2 | Hanya Region 2, tidak ditebang | 2 hari game |

Pisau selalu dimiliki pemain, jadi bahan vaksin tidak pernah terkunci karena alat.

### Memakai vaksin

Alurnya memakai `ItemUseSystem` (#66). `Med_Vaccine` berisi `isConsumable = true`, `useSeconds = vaccineInjectSeconds`, `restoreSeconds = 0`, dan `infectionDelta = -vaccineReduction`.

**`public bool CanUseMedicine(ItemData item)`**, dipanggil `TryUse` langkah 2: kembalikan `false` kalau `Infection <= 0`, dengan notifikasi `Tidak terinfeksi`.

Selama 4 detik suntik, pemain bergerak setengah kecepatan. Terkena serangan, berganti item, mulai lari, atau diterkam membatalkan suntikan, dan **vaksin tidak berkurang** (#66).

**`public void ApplyMedicine(ItemData item)`**, dipanggil `CompleteUse` langkah 6:

1. `float amount = -item.infectionDelta`.
2. Kalikan dengan `NPCRoster.Instance.GetSkillMultiplier(NPCSkill.Medic)`. Fungsi itu mengembalikan `medicVaccineMultiplier` kalau NPC medis hidup, dan 1 kalau tidak (#76).
3. `stats.ReduceInfection(amount)`.
4. Terbitkan `VaccineUsedEvent { reduction = amount, infectionAfter }`.
5. Panggil `HandleVitals` lewat nilai terbaru, supaya tahap langsung turun tanpa menunggu 0,25 detik.

Vaksin mengubah infeksi secara langsung, sesuai `ItemUseSystem.md`. Satu dosis menurunkan infeksi 80 ke 30, yaitu tahap 4 ke tahap 2.

### Peran Vaksin dan Antibiotik

| Item | Mengobati | Tidak mengobati | Pemilik |
|---|---|---|---|
| `Med_Vaccine` | `Infection` zombie | Penyakit #45 | Blueprint ini |
| `Med_Antibiotic` | `WoundInfection` (luka bakteri) | `Infection` zombie | #45 |

Checklist #45 menulis `Antibiotik untuk infeksi`. Maksudnya infeksi luka, bukan infeksi zombie, karena GDD menyatakan vaksin sebagai obat gigitan. `Med_Antibiotic.infectionDelta` selalu 0.

### Urutan kematian `Zombified`

1. `SurvivalStats.Update()` menemukan `Infection >= MaxInfection`, lalu memanggil `TriggerDeath(DeathReason.Zombified)`. Baris HP 5 di `VitalsRules.md` juga berakhir di alasan yang sama, lewat `DeathReasonMap`.
2. `SurvivalStats` menerbitkan `PlayerDiedEvent { Zombified }`.
3. `GameManager.TriggerGameOver` menerbitkan `RunEndedEvent`, lalu `RunLifecycle.RunEndSequence` berjalan (#82).
4. `DeathCinematic.Play(DeathReason.Zombified)` (#53): pandangan menjadi hijau penuh, kontrol dilepas, napas berubah menjadi erangan, lalu fade selama `zombifyCinematicSeconds`. Ragdoll tidak dipakai.
5. Layar `GameOver` menulis penyebab `Zombified`, dan pemain kembali ke Fase 1.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `InfectionStageChangedEvent`, `VaccineUsedEvent`.

**Event yang didengar:** `VitalsChangedEvent`, `HourChangedEvent`, `PlayerDiedEvent` (berhenti menghitung).

| Sistem | Hubungan |
|---|---|
| `SurvivalStats`, `VitalsRules` (#111) | Pemilik nilai. Menerima `SetPenalty`, `AddInfection`, dan `ReduceInfection` |
| `DamagePipeline` (#59), `EnemyAttack` (#109), `SpecialZombieAI` (#25) | Mengirim `DamageInfo.infection` dari `AttackData` |
| `ItemUseSystem` (#66) | Memanggil `CanUseMedicine` dan `ApplyMedicine` |
| `DiseaseManager` (#45) | Pemilik Antibiotik dan penyakit lain |
| `CraftingStation` (#77), `ResourceRespawn` (#73), `ResourceNode` | Resep, stasiun, dan node bahan |
| `NPCRoster` (#76) | `GetSkillMultiplier(NPCSkill.Medic)` |
| `PostProcessController` (#23) | Membaca `stage`, lalu `stageVision` |
| `DeathCinematic` (#53), `RunLifecycle` (#82) | Urutan kematian |
| `LifeboatExtraction` (#74), `CarryCapacity` (#118) | Stok awal |
| `NotificationManager` (#22), `HUDManager` | Pesan dan bar |

**Batas pekerjaan:** keracunan dan pendarahan di #45, efek layar di #23, dan bentuk serangan di #25 serta #109.

## 4. Kasus Khusus

- **Objek pool:** `InfectionSystem` tidak memakai pool. Zombie dari pool mengambil nilai infeksi dari `AttackData`, jadi tidak ada status per objek yang perlu direset.
- **Game dijeda:** `VitalsChangedEvent` dan `HourChangedEvent` berhenti karena waktu berhenti. Panel `Inventory` tidak menjeda, jadi menyuntik sambil membuka inventori tetap memakan waktu.
- **Permadeath:** nilai `Infection` dan isi inventori terhapus bersama save run (#82). Run baru kembali ke 0, dengan vaksin awal lagi.
- **Save dan load:** `Infection` disimpan `SurvivalStats`, dan vaksin disimpan `InventoryManager`. `CurrentStage` tidak disimpan. Saat dimuat, `FinishRestore` (#80) mengirim `VitalsChangedEvent`, lalu tahap dan penalti dihitung ulang, dan `InfectionStageChangedEvent` dikirim ulang.
- **Pindah dari Fase 1:** `Infection` terbawa lewat `shipOutcome` (#74). Tahap langsung dihitung saat `InfectionSystem` aktif di pulau.
- **Infeksi turun lewat batas:** tahap turun dan penalti langsung diringankan. Stamina yang sudah terpotong tidak naik otomatis (#111).
- **Mati dan divaksin pada frame yang sama:** `isDead` di `SurvivalStats` menahan efek berikutnya.
- **Godmode (#56):** `SetInvulnerable` menahan infeksi di 99, jadi tahap 4 tetap aktif dan pemain tidak mati.
- **Vaksin melebihi nilai:** `ReduceInfection` membatasi hasil ke 0.
- **NPC medis mati saat menyuntik:** pengali dibaca saat `CompleteUse`, jadi nilai terbaru yang dipakai.
- **Infeksi tinggi saat tidur:** tahap 4 tetap menguras HP lewat `SurvivalStats`. Pembatasan tidur ada di #67.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SurvivalStats` pemilik `Infection` (`GLOSSARY.md`). Bahan vaksin dari Region 2 (GDD bagian 4 dan 5). Batas 50 dan 75 (`VitalsRules.md`). `Zombified` sebagai alasan kematian (`GLOSSARY.md`).

**Keputusan teknis di blueprint ini:**

- Tahap dihitung dari `VitalsChangedEvent`, bukan setiap frame.
- Penalti dipasang lewat `SetPenalty`, sehingga aturan gabungan tetap di `SurvivalStats`.
- Vaksin memakai alur `ItemUseSystem`, tanpa jalur pakai baru.

**Usulan yang menunggu developer:**

1. **Kenaikan infeksi:** hanya saat diserang. `selfProgressPerGameHour` bernilai 0, dan bisa dinaikkan tanpa mengubah kode.
2. **Tahap dan penalti:** empat tahap sesuai tabel.
3. **Nilai per musuh:** 5 sampai 10 per pukulan, sesuai tabel. Hewan 0.
4. **Stok awal:** 2 dosis, dan vaksin dari kapal menambah stok.
5. **Dosis:** menurunkan 50, suntik 4 detik, dan bisa batal saat diserang tanpa kehilangan vaksin.
6. **Resep:** terbuka sejak awal, di `MedicalBench`, 10 detik.
7. **Node medis:** jamur dengan tangan, getah dengan Pisau, hanya di Region 2.
8. **NPC medis:** vaksin 25 persen lebih kuat selama NPC hidup, tanpa syarat jarak.
9. **Peran Antibiotik:** hanya untuk infeksi luka, tidak menyembuhkan gigitan zombie.

## 6. Skenario Tes

1. Biarkan zombie standar memukul pemain sekali. **Hasil:** `Infection` naik 5, dan `PlayerDamagedEvent` terkirim sekali.
2. Biarkan Serigala menggigit. **Hasil:** HP turun, dan `Infection` tetap.
3. `setvital infection 30`. **Hasil:** `InfectionStageChangedEvent { stage = 2 }`, batas stamina 90, dan notifikasi tampil.
4. `setvital infection 60`. **Hasil:** tahap 3, HP tidak pulih, dan regenerasi stamina 75 persen.
5. `setvital infection 80`. **Hasil:** tahap 4, HP turun 0,3 per detik, dan batas stamina 50.
6. Diam 2 menit di `Infection` 30. **Hasil:** nilai tetap 30, selama `selfProgressPerGameHour` 0.
7. `balance infection.selfProgressPerGameHour 1`, lalu `setvital infection 60`, dan tunggu 3 jam game. **Hasil:** naik sekitar 3.
8. Selesaikan Fase 1 tanpa barang. **Hasil:** ada 2 `Med_Vaccine` di inventori.
9. Selesaikan Fase 1 dengan 1 vaksin. **Hasil:** ada 3 `Med_Vaccine`, dalam satu tumpukan.
10. Suntik di `Infection` 80. **Hasil:** setelah 4 detik, infeksi 30, tahap 2, dan vaksin berkurang 1.
11. Mulai suntik, lalu biarkan zombie memukul. **Hasil:** suntikan batal, dan vaksin tetap.
12. Suntik saat `Infection` 0. **Hasil:** ditolak dengan notifikasi.
13. Suntik dengan NPC medis hidup di `Infection` 80. **Hasil:** infeksi 17,5.
14. Pukul `Node_SapTree` dengan tangan. **Hasil:** ditolak dengan pesan `Butuh Pisau`. Pukul dengan Pisau. **Hasil:** `Mat_TreeSap` jatuh setelah 2 pukulan.
15. `additem Mat_TreeSap 2`, `additem Mat_Mushroom 2`, lalu rakit di `MedicalBench`. **Hasil:** 1 vaksin baru setelah 10 detik.
16. Minum `Med_Antibiotic` saat `Infection` 50. **Hasil:** ditolak kalau tidak ada `WoundInfection`, dan infeksi tetap.
17. `setvital infection 100`. **Hasil:** `RunEndedEvent` dengan `Zombified`, adegan hijau, layar `GameOver`, lalu Ulangi dari Kapal.
18. Biarkan HP habis di tahap 4 tanpa serangan. **Hasil:** alasan kematian `Zombified`.
19. Simpan di `Infection` 60, keluar, lalu lanjutkan. **Hasil:** nilai 60, tahap 3, dan penalti aktif tanpa menunggu pukulan.
20. Mati, lalu mulai run baru. **Hasil:** `Infection` 0, tahap 1, dan penalti hilang.
