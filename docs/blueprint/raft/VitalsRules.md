# Blueprint: VitalsRules (Aturan untuk SurvivalStats.cs)
**Lokasi:** `docs/blueprint/raft/VitalsRules.md`
**Deskripsi Utama:** Satu aturan bersama untuk pemulihan dan pengurangan status pemain: siapa pemiliknya, kondisi mana yang berlaku lebih dulu, biaya stamina, dan penalti tiap status.
**Issue:** #111
**Rujukan GDD:** Bagian 3 (status Lapar, Haus, dan Infeksi, serta HP habis karena serangan, lapar, haus, atau infeksi) dan bagian 5 (Frostbite: stamina dulu, lalu HP). Urutan prioritas, pemulihan HP alami, dan biaya stamina adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

### Pemilik

`SurvivalStats` adalah satu-satunya pemilik `HP`, `Stamina`, `Hunger`, `Thirst`, dan `Infection`, sesuai `GLOSSARY.md`. Sistem lain membaca properti atau memanggil fungsi publik di bawah ini. Tidak ada kelas `VitalsRules` di kode. File ini berisi aturan yang dijalankan `SurvivalStats`.

| Hal lama | Aturan baru |
|---|---|
| Stamina di `IslandPlayerController` | Pindah ke `SurvivalStats`. Pengontrol hanya memanggil `SpendStamina` |
| Regenerasi stamina di `IslandPlayerController` | Pindah ke `SurvivalStats.TickStamina` |
| Coroutine frostbite di `ZoneManager` | Dihapus. `WarmthSystem` (#65) menentukan `isFreezing`, dan `SurvivalStats` menghitung akibatnya |
| `Heal(float, string)` | Diganti fungsi pemulihan per status, di tabel fungsi publik |
| `postProcessVolume` di `SurvivalStats` | Dihapus. Efek layar milik #23, lewat `VitalWarningChangedEvent` |

### Fungsi publik `SurvivalStats`

| Fungsi | Dipanggil oleh | Kegunaan |
|---|---|---|
| `TakeDamage(in DamageInfo)` | Lihat #59 | Damage dari luar |
| `ApplyVitalDamage(float amount, DamageType type)` | `SurvivalStats`, #20 | Publik. Damage kondisi, tanpa `PlayerDamagedEvent` |
| `RestoreHP(float)`, `RestoreHunger(float)`, `RestoreThirst(float)` | `ItemUseSystem` (#66), #67 | Pemulihan langsung |
| `RestoreStamina(float)` | `SleepSystem` (#67) | Pemulihan stamina langsung |
| `QueueRestore(float hunger, float thirst, float seconds)` | `ItemUseSystem` (#66) | Pemulihan bertahap |
| `AddInfection(float)`, `ReduceInfection(float)` | #59, `InfectionSystem` (#64) | Mengubah infeksi |
| `bool SpendStamina(float amount, StaminaUse use)` | Pengontrol, melee, #13, #101 | Mengurangi stamina. Mengembalikan `false` kalau ditolak |
| `bool CanUseStamina(StaminaUse use)` | Pengontrol, melee | Pemeriksaan tanpa mengurangi |
| `SetDrowning(bool)` | `SwimmingController` (#13), `ShipHazardManager` (#75) | Kondisi tenggelam |
| `SetPenalty(VitalPenaltySource, float maxStaminaMultiplier, float staminaRegenMultiplier)` | #64, #69, #20 | Penalti dari sistem lain |
| `SetMaxBonus(float hpMultiplier)` | `StatusEffectManager` (#20), untuk buff `Permanent` Lucky Machine (#72) | Pengali naik untuk `MaxHP`. Nilai awal 1. Bonus `MaxStamina` lewat `SetPenalty(StatusEffect, ...)`. Nama fungsi adalah usulan |
| `SetHPRegenBlocked(bool)` | `StatusEffectManager` (#20), dari `blocksHPRegen` penyakit (#45) | Menahan pemulihan HP alami (baris HP 6). Regenerasi stamina tidak terpengaruh |
| `SetDrainMultiplier(float)` | `SleepSystem` (#67) | Pengali lapar dan haus. Nilai awal 1 |
| `SetDrainEnabled(bool)` | `LifeboatExtraction.PreGameRoutine` (#74) | Mematikan lapar dan haus di Fase 1 |
| `SetInvulnerable(bool)` | `DevConsole` (#56), #74 | Menahan HP di 1, dan infeksi di 99 |
| `DebugSetVital(string name, float value)` | `DevConsole` (#56) | Dibungkus `#if UNITY_EDITOR \|\| DEVELOPMENT_BUILD`. Mengisi satu status lewat perintah `setvital` |
| `DebugKill(DeathReason reason)` | `DevConsole` (#56) | Dibungkus `#if UNITY_EDITOR \|\| DEVELOPMENT_BUILD`. Meneruskan ke `TriggerDeath` |

```csharp
public enum StaminaUse { Sprint, Jump, Melee, Swim, Climb, Struggle }
public enum VitalPenaltySource { Hunger, Thirst, Infection, Cold, Heavy, StatusEffect }

[Flags]
public enum VitalWarningFlags
{
    None = 0, LowHP = 1, Starving = 2, Dehydrated = 4, Freezing = 8,
    Drowning = 16, Exhausted = 32, InfectionHigh = 64, HungerLow = 128, ThirstLow = 256
}
```

### Angka penyetelan

Semua berada di `GameBalanceConfig.vitals`. Nilai yang sudah ada di `GameBalanceConfig.md` tidak berubah.

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `hungerDrainPerSec` | 0,11 | Sudah ada |
| `thirstDrainPerSec` | 0,15 | Sudah ada |
| `sprintThirstMultiplier` | 1,5 | Sudah ada |
| `starvationDamagePerSec` | 0,5 | Sudah ada |
| `dehydrationDamagePerSec` | 0,5 | Sudah ada |
| `frostbiteStaminaDrainPerSec` | 5 | Sudah ada |
| `frostbiteDamagePerSec` | 1 | Sudah ada |
| `drownDamagePerSec` | 10 | HP penuh habis dalam 10 detik setelah oksigen habis (#13) |
| `infectionDamagePerSec` | 0,3 | Damage saat infeksi tinggi, termasuk saat `Infection` 100. Nilai awal keputusan developer |
| `infectionDamageThreshold` | 75 | Awal damage infeksi. Selaras dengan tahap 4 di #64 |
| `regenPerSec` | 0,25 | HP penuh pulih dalam 400 detik, sekitar 0,45 hari game |
| `regenMinHunger` | 50 | Lapar minimum untuk pulih |
| `regenMinThirst` | 50 | Haus minimum untuk pulih |
| `regenMaxInfection` | 50 | Infeksi maksimum untuk pulih |
| `regenDelayAfterDamageSeconds` | 12 | Jeda setelah diserang |
| `sprintStaminaPerSec` | 10 | Lari penuh 10 detik, sekitar 80 meter |
| `jumpStaminaCost` | 8 | Biaya sekali lompat |
| `meleeStaminaCost` | 15 | Sama dengan nilai `MeleeCombatSystem` lama |
| `swimStaminaPerSec` | 6 | Biaya berenang (#13) |
| `climbStaminaPerSec` | 5 | Biaya memanjat (#101) |
| `staminaRegenPerSec` | 15 | Dari 0 ke penuh dalam sekitar 7 detik |
| `staminaRegenDelaySeconds` | 1,5 | Jeda setelah stamina dipakai |
| `exhaustedRecoverThreshold` | 25 | Stamina yang harus dicapai untuk keluar dari kelelahan |
| `exhaustedMeleeCooldownMultiplier` | 1,5 | Melee saat lelah lebih lambat |
| `exhaustedMeleeDamageMultiplier` | 0,7 | Melee saat lelah lebih lemah |
| `lowVitalThreshold` | 25 | Batas lapar atau haus rendah |
| `lowHungerMaxStaminaMultiplier` | 0,75 | Batas stamina saat lapar rendah |
| `lowThirstRegenMultiplier` | 0,5 | Regenerasi stamina saat haus rendah |
| `heavyStaminaRegenMultiplier` | 0,75 | Regenerasi stamina saat membawa Heavy Item |
| `lowHPWarningRatio` | 0,25 | Peringatan HP rendah |
| `vitalsPublishIntervalSeconds` | 0,25 | `VitalsChangedEvent`, 4 kali per detik |

## 2. Detail Logika & Fungsi

### Tabel prioritas HP

Setiap frame, `SurvivalStats.TickHP()` memeriksa dari atas. Hanya baris pertama yang cocok yang berlaku. Kerusakan kondisi **tidak bertumpuk**.

| Urutan | Kondisi | Akibat ke HP | `DamageType` |
|---|---|---|---|
| 1 | `isDrowning` bernilai `true` | `-drownDamagePerSec` | `Drown` |
| 2 | `isFreezing` dan `Stamina <= 0` | `-frostbiteDamagePerSec` | `Frost` |
| 3 | `Hunger <= 0` | `-starvationDamagePerSec` | `Starvation` |
| 4 | `Thirst <= 0` | `-dehydrationDamagePerSec` | `Dehydration` |
| 5 | `Infection >= infectionDamageThreshold` | `-infectionDamagePerSec` | `Infection` |
| 6 | Syarat pemulihan terpenuhi | `+regenPerSec` | Tidak ada |
| 7 | Selain itu | Tidak berubah | Tidak ada |

```csharp
void TickHP(float dt)
{
    if (isDrowning) ApplyVitalDamage(b.drownDamagePerSec * dt, DamageType.Drown);
    else if (isFreezing && Stamina <= 0f) ApplyVitalDamage(b.frostbiteDamagePerSec * dt, DamageType.Frost);
    else if (Hunger <= 0f) ApplyVitalDamage(b.starvationDamagePerSec * dt, DamageType.Starvation);
    else if (Thirst <= 0f) ApplyVitalDamage(b.dehydrationDamagePerSec * dt, DamageType.Dehydration);
    else if (Infection >= b.infectionDamageThreshold) ApplyVitalDamage(b.infectionDamagePerSec * dt, DamageType.Infection);
    else if (CanRegenHP()) HP = Mathf.Min(HP + b.regenPerSec * dt, MaxHP);
}
```

**Yang tetap berjalan bersamaan:** tabel ini hanya mengatur HP. Pengurasan stamina karena frostbite, lapar, haus, dan infeksi tetap berjalan terpisah. Damage dari serangan musuh juga di luar tabel, jadi pemain kelaparan yang digigit tetap menerima keduanya.

**Akibat pada `DeathReason`:** baris yang sedang berlaku menjadi `lastDamageType`. Pemain lapar dan haus yang mati tercatat `Starvation`, karena baris itu lebih tinggi.

### Syarat pemulihan HP

`CanRegenHP()` bernilai `true` kalau semua syarat ini terpenuhi:

- `Hunger >= regenMinHunger`
- `Thirst >= regenMinThirst`
- `Infection < regenMaxInfection`
- `Time.time - lastDamageTime >= regenDelayAfterDamageSeconds`
- Tidak ada baris 1 sampai 5 yang aktif
- `SetHPRegenBlocked(true)` tidak aktif (penyakit dengan `blocksHPRegen`, #20 dan #45)

Damage kondisi tidak mengubah `lastDamageTime`. Pemulihan sudah tertahan oleh syarat terakhir. #67 dapat mengalikan `regenPerSec` saat tidur.

### Lapar dan haus

**`TickDrains(float dt)`**, dilewati kalau `drainEnabled` bernilai `false`:

```csharp
Hunger -= b.hungerDrainPerSec * drainMultiplier * difficulty.hungerMultiplier * dt;
float thirstRate = b.thirstDrainPerSec * drainMultiplier * difficulty.thirstMultiplier;
float activity = 1f;
if (isSprinting) activity = Mathf.Max(activity, b.sprintThirstMultiplier);
if (isExerting) activity = Mathf.Max(activity, hydration.exertionThirstMultiplier);
thirstRate *= activity;
Thirst -= thirstRate * dt;
```

Nilai dijaga pada rentang 0 sampai maksimum. `isSprinting` diisi otomatis saat `SpendStamina` dipanggil dengan `StaminaUse.Sprint` pada frame itu, dan `isExerting` saat dipanggil dengan `StaminaUse.Swim` atau `StaminaUse.Climb` (#43). Kalau keduanya aktif, pengali terbesar yang dipakai. Isi hidrasi lain, seperti hujan dan air kotor, ada di #43.

**Pemulihan bertahap:** `QueueRestore` menambah sisa pemulihan. `TickRestore(dt)` menyalurkan sisa itu selama `seconds`. Rincian pemakaiannya ada di #66.

### Biaya stamina

| Aksi | Biaya | Pemanggil | Syarat |
|---|---|---|---|
| Lari | `sprintStaminaPerSec` per detik | `IslandPlayerController` | `Stamina > 0`, tidak lelah, dan tidak membawa Heavy Item |
| Lompat | `jumpStaminaCost` sekali | `IslandPlayerController` | `Stamina >= biaya`, tidak lelah, dan tidak membawa Heavy Item |
| Melee | `meleeStaminaCost` sekali | `MeleeCombatSystem` | Tetap boleh saat lelah, dengan pengali |
| Berenang | `swimStaminaPerSec` per detik | `SwimmingController` (#13) | Stamina habis menjadi kondisi tenggelam |
| Memanjat | `climbStaminaPerSec` per detik saat bergerak | `PlayerTraversal` (#101) | Stamina habis berarti pelan, tanpa jatuh |
| Aksi lepas | Menurut #110 | `PlayerPinnedState` (#110) | `SpendStamina` boleh mengembalikan `false` saat stamina 0, tanpa menghentikan aksi lepas |

**`public bool SpendStamina(float amount, StaminaUse use)`**:

1. Kalau `use` adalah `Sprint` atau `Jump`, dan `IsExhausted`, kembalikan `false`.
2. Kalau `use` adalah `Jump` dan `Stamina < amount`, kembalikan `false`. Kalau `use` adalah `Struggle` dan `Stamina <= 0`, kembalikan `false`.
3. `Stamina = Mathf.Max(0, Stamina - amount)`, lalu `lastStaminaUseTime = Time.time`.
4. Kalau `Stamina <= 0`, `IsExhausted = true`.
5. Kembalikan `true`.

Melee saat lelah tetap berhasil, supaya pemain tidak kehilangan pilihan bertahan. `MeleeCombatSystem` membaca `IsExhausted`, lalu memakai `exhaustedMeleeCooldownMultiplier` dan `exhaustedMeleeDamageMultiplier`.

### Pemulihan stamina dan kelelahan

**`TickStamina(float dt)`**:

1. Kalau `isFreezing`, kurangi `frostbiteStaminaDrainPerSec * dt`, atur `lastStaminaUseTime`, lalu selesai.
2. Hitung `regenMultiplier`, yaitu hasil kali semua penalti aktif.
3. Kalau `Time.time - lastStaminaUseTime >= staminaRegenDelaySeconds`, tambahkan `staminaRegenPerSec * regenMultiplier * dt`.
4. Batasi `Stamina` sampai `MaxStamina * maxStaminaMultiplier`. Nilai ini sudah memuat buff `StatusEffect` di atas 1, termasuk buff stamina Lucky Machine.
5. Kalau `IsExhausted` dan `Stamina >= exhaustedRecoverThreshold`, `IsExhausted = false`.

**Efek kelelahan:** lari dan lompat ditolak, suara napas berat (#23 dan #52) dimulai dari `VitalWarningFlags.Exhausted`, serta melee melemah. Pemain masih bisa berjalan.

### Penalti tiap status

`SurvivalStats` menyimpan satu pasang nilai per `VitalPenaltySource`, lalu menghitung `regenMultiplier` sebagai hasil kali. `VitalPenaltySource.StatusEffect` dikecualikan dari perhitungan nilai terkecil: `maxStaminaMultiplier = (terkecil dari sumber lain) x nilai StatusEffect`, supaya buff di atas 1 tidak hilang (#20).

**Bonus maksimum (#72):** satu jalur per status. Bonus `MaxStamina` masuk lewat `SetPenalty(StatusEffect, ...)`, yang dikecualikan dari nilai terkecil seperti di atas. Bonus `MaxHP` memakai `SetMaxBonus(hpMultiplier)`, karena `SetPenalty` tidak mengatur HP. Saat bonus naik, HP dan stamina saat ini tidak ikut naik otomatis.

| Sumber | Syarat | Batas stamina | Regenerasi stamina | Penalti lain | Diisi oleh |
|---|---|---|---|---|---|
| Lapar | `Hunger < lowVitalThreshold` | x `lowHungerMaxStaminaMultiplier` | Tetap | Baris HP 3 saat nol | `SurvivalStats` |
| Haus | `Thirst < lowVitalThreshold` | Tetap | x `lowThirstRegenMultiplier` | Saat nol, regenerasi 0 (#43), dan baris HP 4 | `SurvivalStats` |
| Infeksi | Tahap dari #64 | Menurut #64 | Menurut #64 | Penglihatan (#23), dan baris HP 5 | `InfectionSystem` (#64) |
| Dingin | `isFreezing` | Tetap | 0 | Stamina terkuras, lalu baris HP 2 | `SurvivalStats`, dari `WarmthChangedEvent` |
| Beban | `HeavyItemChangedEvent.isCarried` | Tetap | x `heavyStaminaRegenMultiplier` | Lari dan lompat mati (#69) | `SurvivalStats` |
| Buff | Dari #20 | Menurut #20 | Menurut #20 | Tidak ada | `StatusEffectManager` (#20) |

Penalti kecepatan gerak berada di `PlayerSpeedModifiers` (#101), dengan pengali dari #69 dan cuaca.

### Zona hanya mengirim kondisi

`ZoneManager` tidak lagi memulai coroutine atau mengurangi HP. Alurnya:

```text
ZoneManager --RegionChangedEvent--> WarmthSystem (#65) --WarmthChangedEvent { warmth, isFreezing }--> SurvivalStats
```

`SurvivalStats` hanya membaca `isFreezing`. Pakaian dan obor yang berubah saat di dalam zona langsung berpengaruh, karena `WarmthSystem` memeriksa secara berkala.

### Sinyal untuk HUD dan efek layar

- **`VitalsChangedEvent`:** setiap `vitalsPublishIntervalSeconds`, dengan `hp`, `stamina`, `hunger`, `thirst`, dan `infection`. Dipakai `HUDManager`.
- **`VitalWarningChangedEvent { flags }`:** hanya saat kombinasi berubah, dan sticky. Dipakai #23 dan HUD untuk ikon. Nilainya dihitung di `RefreshWarnings()`, bersamaan dengan `VitalsChangedEvent`.

| Tanda | Syarat |
|---|---|
| `LowHP` | `HP / MaxHP < lowHPWarningRatio` |
| `HungerLow`, `ThirstLow` | Nilai di bawah `lowVitalThreshold` |
| `Starving`, `Dehydrated` | Nilai nol |
| `Freezing` | `isFreezing` |
| `Drowning` | `isDrowning` |
| `Exhausted` | `IsExhausted` |
| `InfectionHigh` | `Infection >= infectionDamageThreshold` |

### Urutan dalam satu frame

`SurvivalStats.Update()`, kalau `!isDead`: `TickDrains`, `TickRestore`, `TickStamina`, `TickHP`, lalu penerbitan event kalau sudah waktunya. Tidak ada pemeriksaan kematian dari nilai `Infection`, karena GDD bagian 3 menetapkan pemain mati saat HP habis.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `VitalsChangedEvent`, `VitalWarningChangedEvent`, `PlayerDamagedEvent`, dan `PlayerDiedEvent`.

**Event yang didengar:** `WarmthChangedEvent`, `HeavyItemChangedEvent`, dan `RunEndedEvent` (berhenti menghitung).

| Sistem | Hubungan |
|---|---|
| `DamagePipeline` (#59) | Mengatur damage dari luar dan `DeathReason` |
| `InfectionSystem` (#64) | Mengisi penalti tahap infeksi dan mengurangi infeksi |
| `WarmthSystem` (#65) | Menentukan `isFreezing` |
| `SwimmingController` (#13) | Memanggil `SetDrowning` dan `SpendStamina` |
| `PlayerTraversal` (#101) | Memanggil `SpendStamina` untuk memanjat |
| `IslandPlayerController`, `MeleeCombatSystem` | Tidak lagi punya stamina sendiri |
| `SleepSystem` (#67) | Memakai `SetDrainMultiplier` dan menaikkan pemulihan |
| `HydrationSystem` (#43) | Mengisi sumber air |
| `ItemUseSystem` (#66) | Memanggil pemulihan |
| `PostProcessController` (#23), `HUDManager` | Mendengar sinyal |
| `StatusEffectManager` (#20) | Mengisi `SetPenalty` lewat `StatusEffect`, termasuk buff dengan nilai di atas 1 |

**Batas pekerjaan:** data buff di #20, isi hidrasi di #43, jalur damage di #59, dan angka final di #84.

## 4. Kasus Khusus

- **Objek pool:** `SurvivalStats` hanya ada di pemain, dan tidak memakai pool.
- **Game dijeda:** `Time.deltaTime` bernilai 0, dan `Time.time` berhenti, jadi lapar, stamina, jeda pemulihan, dan event tidak maju. Panel tanpa jeda seperti inventori tetap menjalankan semuanya, sesuai #83.
- **Permadeath:** semua nilai direset saat scene pemain dibuang. Save run yang memuat status dihapus `RunLifecycle` (#82). Saat run baru dimulai, semua status kembali ke maksimum, kecuali `Infection` ke 0.
- **Save dan load:** disimpan `HP`, `Stamina`, `Hunger`, `Thirst`, `Infection`, `IsExhausted`, dan sisa `QueueRestore` lewat `ISaveable` dengan `SaveId = player_vitals`. Tidak disimpan: `isFreezing` dan `isDrowning`, karena dihitung ulang. `lastDamageTime` diatur ke saat dimuat, jadi pemain menunggu jeda penuh.
- **Pindah dari Fase 1:** `HP` dan `Infection` dibawa lewat `GameManager.shipOutcome` (#74). `Stamina`, `Hunger`, dan `Thirst` mulai penuh, karena GDD tidak mengaturnya.
- **Fase 1:** `drainEnabled = false`. Pemain tidak lari, jadi stamina tidak dipakai. Baris HP 1 tetap aktif untuk air di kapal (#75).
- **Tenggelam dan membeku bersamaan:** baris 1 berlaku. Stamina tetap terkuras oleh dingin.
- **Stamina tertahan batas:** saat `maxStaminaMultiplier` turun, stamina langsung dipotong ke batas baru dan tidak kembali otomatis saat batas naik.
- **Mati saat `SetInvulnerable(true)`:** HP tertahan di 1 dan infeksi di 99, jadi tidak ada `PlayerDiedEvent`.
- **Dua kondisi berganti cepat:** `DeathReason` memakai `lastDamageType` pada frame HP habis.
- **HP penuh tetapi syarat lain gagal:** tidak ada efek, dan `lastDamageTime` tidak berubah.
- **Mati di frame penerbitan event:** `isDead` menghentikan semua tick setelah `TriggerDeath`.
- **Tidur:** `SetDrainMultiplier` berlaku pada lapar dan haus. `Time.timeScale` tidak berubah, jadi stamina tetap memakai waktu nyata.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SurvivalStats` memegang semua status (`GLOSSARY.md`). Frostbite menguras stamina dulu, lalu HP (`GLOSSARY.md`). Haus menjadi status inti (`GLOSSARY.md`). Angka lapar, haus, dan frostbite (`GameBalanceConfig.md`). Dehidrasi menghentikan regenerasi stamina (#43).

**Keputusan developer:** saat `Infection` mencapai 100, HP turun bertahap dengan laju awal 0,3 per detik lewat baris 5, dan pemain tidak mati seketika. Kematiannya tetap `Zombified` saat HP habis, sesuai GDD bagian 3.

**Keputusan teknis di blueprint ini:**

- `VitalsRules` tidak menjadi kelas terpisah, supaya pemilik status tetap satu.
- Pengurasan stamina karena dingin berada di luar tabel HP.
- Damage kondisi tidak mengubah `lastDamageTime`.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Kerusakan tidak bertumpuk:** hanya kondisi tertinggi yang berlaku, mengikuti pola Raft di `CATATAN_MEKANIK.md`. Pilihan lain adalah bertumpuk, tetapi HP bisa habis terlalu cepat saat beberapa status buruk bersamaan.
2. **Urutan prioritas:** tenggelam, frostbite, kelaparan, dehidrasi, infeksi, lalu pemulihan, sesuai checklist issue.
3. **Pemulihan HP alami:** ada, 0,25 per detik, dengan syarat lapar dan haus minimal 50, infeksi di bawah 50, dan 12 detik tanpa diserang.
4. **Awal damage infeksi:** mulai `Infection` 75, dengan laju yang sama seperti saat `Infection` 100.
5. **Biaya stamina:** lari 10 per detik, lompat 8, melee 15, berenang 6 per detik, dan memanjat 5 per detik.
6. **Kelelahan:** setelah stamina habis, lari dan lompat terkunci sampai stamina 25. Melee tetap bisa, dengan damage 70 persen dan jeda 1,5x.
7. **Penalti lapar dan haus rendah:** di bawah 25, batas stamina menjadi 75 persen, atau regenerasi stamina setengah.
8. **Status awal di pulau:** `Hunger`, `Thirst`, dan `Stamina` penuh.

## 6. Skenario Tes

1. Atur `Hunger` 0 lewat `setvital`. **Hasil:** HP turun 0,5 per detik, dan layar akhir menulis `Starvation`.
2. Atur `Hunger` dan `Thirst` 0. **Hasil:** HP tetap turun 0,5 per detik, bukan 1, dan alasan kematian `Starvation`.
3. Masuk Region 3 tanpa pakaian hangat. **Hasil:** stamina habis sekitar 20 detik, lalu HP turun 1 per detik. Alasan kematian `Frostbite`.
4. Pakai pakaian hangat saat HP sedang turun. **Hasil:** HP berhenti turun dalam satu pemeriksaan `WarmthSystem`, dan stamina mulai pulih.
5. Terima pukulan zombie saat lapar 80, haus 80, infeksi 0. **Hasil:** HP tidak pulih selama 12 detik, lalu naik 0,25 per detik.
6. Atur `Infection` 60. **Hasil:** tidak pulih dan tidak ada damage. Atur 80. **Hasil:** HP turun 0,3 per detik. Atur 100 dengan HP penuh. **Hasil:** pemain tidak langsung mati, dan HP tetap turun 0,3 per detik.
7. Lari sampai stamina habis. **Hasil:** lari berhenti, ikon lelah tampil, dan pemain baru bisa lari lagi setelah stamina 25.
8. Tekan lompat saat stamina 5. **Hasil:** lompatan ditolak.
9. Pukul zombie saat stamina 0. **Hasil:** serangan tetap terjadi, dengan damage 70 persen dan jeda lebih panjang.
10. Atur `Thirst` 10. **Hasil:** regenerasi stamina setengah. Atur `Thirst` 0. **Hasil:** stamina tidak pulih.
11. Bawa Heavy Item. **Hasil:** lari dan lompat mati, serta regenerasi stamina 75 persen.
12. Pantau `eventlog 1` selama 10 detik. **Hasil:** sekitar 40 `VitalsChangedEvent`. `VitalWarningChangedEvent` hanya muncul saat kondisi berubah.
13. Simpan saat `Hunger` 40, keluar, lalu lanjutkan. **Hasil:** `Hunger` 40, dan ada jeda penuh sebelum HP pulih.
14. Buka Pause selama 30 detik. **Hasil:** semua status tidak berubah.
15. Cari `Stamina` di `ZoneManager` dan `IslandPlayerController`. **Hasil:** tidak ada penulisan langsung, dan tidak ada coroutine frostbite.
16. Beri `Debuff_Bleeding` saat lapar 80, haus 80, infeksi 0, dan tanpa serangan selama 12 detik. **Hasil:** HP tidak pulih, sedangkan regenerasi stamina tetap normal.
17. Berenang 100 detik dengan stamina dijaga lewat `setvital`. **Hasil:** `Thirst` turun sekitar 18,75 (pengali 1,25). Lari sambil memanjat tidak menumpuk: pengali tetap 1,5.
18. Dapat `Buff_LuckyStamina` 1 kali saat lapar normal. **Hasil:** batas stamina 110. Dapat `Buff_LuckyHP` 1 kali. **Hasil:** `MaxHP` 110, dan HP saat ini tidak berubah.
