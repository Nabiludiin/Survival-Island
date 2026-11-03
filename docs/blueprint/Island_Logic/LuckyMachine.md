# Blueprint: LuckyMachine.cs
**Lokasi:** `docs/blueprint/Island_Logic/LuckyMachine.md`
**Deskripsi Utama:** Mesin gacha peninggalan peneliti. Aktif lewat tumbal musuh tertentu di kandang vakum, atau gratis selama 30 detik saat bencana besar. Hadiahnya berasal dari data.
**Issue:** #72 (melengkapi #1)
**Rujukan GDD:** Bagian 7 (dua cara aktivasi, jenis hadiah, serta contoh 5 Zombie Militer atau 3 Beruang Kutub). Lokasi mesin, tabel peluang, batas putaran, dan arti "permanen" adalah usulan.

## 1. Arsitektur & Variabel Utama

| Kelas | Jenis | Fungsi |
|---|---|---|
| `LuckyMachine` | Turunan `IslandInteractable`, `ISaveable` lewat `RegionStateStore` (#88), dengan `saveId` dari `SaveIdTag`, bukan lewat `Register` di `WorldStateSave` | Status mesin, kuota, putaran, dan pemberian hadiah |
| `LuckyMachineCage` | Komponen trigger | Deteksi, gerbang, dan vakum |
| `LuckyMachineData` | ScriptableObject | Kuota dan tabel hadiah |
| `LuckyRewardData` | ScriptableObject | Satu hadiah |
| `WeaponModifierData` | ScriptableObject | Pengali senjata, dibaca `ToolRepairSystem` (#113) |

Blueprint ini menggantikan isi lama: `AddEnergy`, `energyCost`, dan array `GameObject` per tier tidak dipakai lagi.

```csharp
public enum LuckyRewardKind { WeaponModifier, Skin, PermanentBuff, SpawnItem }
public enum LuckyTier { B, A, S }
public enum CageGateState { Open, Closed }

[Serializable] public class SacrificeQuota
{
    public string enemyId;   // EnemyData.enemyId
    public int required;     // Jumlah untuk 1 putaran
}

[CreateAssetMenu(menuName = "Survival/Lucky Reward")]
public class LuckyRewardData : ScriptableObject
{
    public string rewardId;
    public LuckyRewardKind kind;
    public LuckyTier tier;
    public float weight;              // Bobot dalam tier
    public bool isUnique;             // Keluar dari undian setelah didapat
    public int maxGrants;             // 0 berarti tanpa batas
    public WeaponModifierData modifier;
    public string skinId, skinTargetItemId;
    public StatusEffectData buff;     // durationUnit = Permanent
    public ItemData item;
    public int amount;
}

[CreateAssetMenu(menuName = "Survival/Weapon Modifier")]
public class WeaponModifierData : ScriptableObject
{
    public string modifierId;
    public float damageMultiplier = 1f, speedMultiplier = 1f, durabilityMultiplier = 1f;
    public int magazineBonus;
    public bool indestructible;       // inst.durability = -1 (#113)
}
```

### Data kuota awal (`LuckyMachineData.quotas`)

| `enemyId` | `required` | Sumber |
|---|---|---|
| `zombie_militer` | 5 | GDD bagian 7 |
| `hewan_beruang` | 3 | GDD bagian 7 |

Tiap baris dihitung terpisah, dan tidak dijumlahkan. Musuh lain bisa ditambah di data tanpa mengubah kode.

### Tabel hadiah awal

| `rewardId` | Jenis | Tier | Isi | Batas |
|---|---|---|---|---|
| `lucky_laser` | `SpawnItem` | S | `Reward_LaserPistol`, dengan modifier `mod_indestructible` | `isUnique` |
| `lucky_fireaxe` | `SpawnItem` | S | `Reward_FireAxe`, dengan `maxDurability = -1` | `isUnique` |
| `lucky_hp` | `PermanentBuff` | A | `Buff_LuckyHP`, `maxHpMultiplier` 1,1 | `maxGrants` 3 |
| `lucky_stamina` | `PermanentBuff` | A | `Buff_LuckyStamina`, `maxStaminaMultiplier` 1,1 | `maxGrants` 3 |
| `lucky_mod_damage` | `WeaponModifier` | A | `mod_damage_25`, damage x1,25 | Tanpa batas |
| `lucky_mod_speed` | `WeaponModifier` | B | `mod_speed_15`, kecepatan x1,15 | Tanpa batas |
| `lucky_skin_gold` | `Skin` | B | `skin_gold` untuk `Base_Pistol` | `isUnique` |
| `lucky_ammo` | `SpawnItem` | B | `Ammo_9mm` x20 | Tanpa batas |
| `lucky_wood` | `SpawnItem` | B | `Mat_Wood` x30 | Tanpa batas |

`Med_Vaccine` sengaja tidak ada, karena GDD bagian 3 menyatakan vaksin baru hanya dari material Region 2.

### Variabel `LuckyMachine`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `quotaProgress` | `int[]` | Nol | Sejajar dengan `quotas` |
| `storedSpins` | `int` | 0 | Putaran berbayar yang sudah terkumpul |
| `freeWindowRemaining` | `float` | 0 | Sisa detik mode gratis |
| `freeWindowEndGameHour` | `float` | -1 | Jam game total saat mode gratis habis. Dipakai untuk save |
| `freeSpinsUsed` | `int` | 0 | Putaran gratis pada jendela ini |
| `isSpinning` | `bool` | `false` | Sedang berputar |
| `grantCounts` | `Dictionary<string, int>` | Kosong | Berapa kali tiap hadiah diberikan |

### Angka (`GameBalanceConfig.luckyMachine`, bagian baru)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `freeWindowSeconds` | 30 | GDD bagian 7 |
| `maxFreeSpinsPerWindow` | 3 | Batas putaran gratis |
| `maxStoredSpins` | 1 | Batas tabungan putaran |
| `spinSeconds` | 4 | Lama animasi putaran |
| `vacuumSeconds` | 1,2 | Lama musuh tersedot |
| `tierWeightS` / `tierWeightA` / `tierWeightB` | 5 / 25 / 70 | Dari blueprint lama |
| `gateReopenSeconds` | 2 | Jeda gerbang terbuka lagi |
| `cageRewardOffset` | 2 | Jarak munculnya hadiah dari mesin |

### Lokasi

**Usulan (#85), belum disetujui developer.** Dua mesin, supaya contoh erupsi Region 1 di GDD bagian 7 bisa dimainkan.

| Mesin | Scene | Posisi | Kuota tumbal | Mode gratis |
|---|---|---|---|---|
| Utama | `Region3_Snow` | (350, 2050), sekitar 160 meter dari `S3` dan 180 meter dari Helipad | Ya | Ya |
| Bencana | `Region1_Volcano` | Ditentukan di Editor, dekat area base awal, dalam sekitar 240 meter lari (30 detik x `sprintSpeed` 8) | Tidak. Prefab tanpa `LuckyMachineCage`, jadi `storedSpins` selalu 0 | Ya |

Alasannya ada di bagian 5.

## 2. Detail Logika & Fungsi

### Kandang

Prefab mesin memiliki `LuckyMachineCage`: `BoxCollider` trigger 6 x 4 x 4 meter di layer `ZoneTrigger`, `Rigidbody` kinematic supaya `OnTriggerEnter` terpanggil, serta gerbang dengan `NavMeshObstacle` (carve).

Kandang berbentuk lorong dengan dua sisi terbuka, supaya musuh yang mengejar pemain melewatinya. Kapsul `Enemy` bertabrakan dengan `ZoneTrigger`, sedangkan `EnemyHitbox` dan `Ragdoll` tidak, jadi satu musuh hanya memberi satu kejadian masuk.

**`void OnTriggerEnter(Collider other)`**:

1. Lewati kalau tag bukan `Zombie` atau `Animal` (sesuai `PhysicsLayers.md`).
2. `var inst = other.GetComponentInParent<EnemyInstance>()`. Lewati kalau `null` atau `inst.IsSacrificing`.
3. Lewati kalau `!EnemyHealth.IsAlive`, atau state `BurnToAshes`.
4. Cari `enemyHealth.data.enemyId` di `quotas`. Lewati kalau tidak ada.
5. Lewati kalau `storedSpins >= maxStoredSpins`, atau `gateState == Closed`.
6. `inst.IsSacrificing = true`, lalu `StartVacuum(inst, quotaIndex)`.

**Vakum**, `StartVacuum`:

1. `EnemyHealth.SetInvulnerable(true)`, supaya tembakan pada saat itu tidak membuat dua jalur kematian.
2. Matikan `EnemyAI` atau `WildlifeAI`, `agent.enabled = false`, dan panggil `ThreatSystem.Unregister`.
3. Tambah ke daftar `vacuums`.

**`Update`** memajukan tiap vakum dengan `Time.deltaTime`: posisi menuju `vacuumPoint`, skala menuju 0,2, partikel dan suara dari `VFXManager` serta `AudioManager`. Saat `vacuumSeconds` selesai, panggil `FinishSacrifice`.

**`private void FinishSacrifice(EnemyInstance inst, int quotaIndex)`**:

1. `quotaProgress[quotaIndex]++`.
2. Terbitkan `EnemySacrificedEvent { enemyId }`.
3. `SpawnManager.Instance.Despawn(inst.gameObject, DespawnReason.Sacrificed)`. Tidak ada `EnemyKilledEvent`, drop, atau `RefundBudget`.
4. Kalau `quotaProgress[quotaIndex] >= required`: `quotaProgress[quotaIndex] = 0`, `storedSpins++`, nyalakan lampu mesin, lalu kirim notifikasi.
5. Kalau `storedSpins >= maxStoredSpins`, `gateState = Closed`. Gerbang tertutup sampai pemain memutar, jadi musuh berikutnya tidak terbuang.

**Tidak terhitung ganda:** `IsSacrificing` menolak masuk kedua, dan hitungan terjadi hanya di akhir vakum. `Despawn` adalah jalur tunggal kembali ke pool (`SpawnManager.md`).

### Mode gratis

**`private void HandleMajorDisaster(MajorDisasterStartedEvent e)`**:

1. `freeWindowRemaining = freeWindowSeconds`, `freeSpinsUsed = 0`. Bencana kedua pada saat mode gratis mengisi ulang waktu dan batas.
2. `freeWindowEndGameHour = jam game total + freeWindowSeconds * time.timeMultiplier / 3600`.
3. Nyalakan efek listrik statis pada mesin dan buka gerbang.
4. Terbitkan `LuckyMachineFreeWindowChangedEvent { isActive = true, seconds = freeWindowSeconds }`.

`durationSeconds` milik bencana tidak dipakai, karena GDD menetapkan 30 detik.

**`Update`**: `freeWindowRemaining -= Time.deltaTime`. Saat habis, matikan efek, lalu terbitkan event dengan `isActive = false`.

**Indikator waktu:** `HUDManager` menampilkan hitung mundur dari event ini, dan menghitung sendiri dengan `Time.deltaTime`. Panel dan layar di mesin menampilkan `freeWindowRemaining`. Lima detik terakhir berkedip dan berbunyi.

### Memutar

**`protected override bool OnInteract(in InteractContext ctx)`** membuka panel `UIPanelId.LuckyMachine` lewat `UIFocusStack.Push`. Panel tidak menjeda game (`UIFocusStack.md`). Panel menampilkan kuota, `storedSpins`, sisa waktu gratis, dan tombol putar.

**`public bool TrySpin()`**:

1. Tolak kalau `isSpinning`.
2. Kalau `freeWindowRemaining > 0` dan `freeSpinsUsed < maxFreeSpinsPerWindow`: `freeSpinsUsed++`.
3. Kalau tidak, dan `storedSpins > 0`: `storedSpins--`, lalu `gateState = Open` setelah `gateReopenSeconds`.
4. Kalau tidak keduanya, tolak.
5. `isSpinning = true`. Putaran yang sudah dimulai tetap selesai, walau waktu gratis habis.
6. Setelah `spinSeconds`, panggil `RollReward()`, `GrantReward()`, lalu `isSpinning = false`.

**`private LuckyRewardData RollReward()`**:

1. `ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.LuckyMachine)`.
2. Pilih tier memakai `tierWeightS/A/B`.
3. Buang hadiah yang tidak sah: `isUnique` yang sudah didapat, atau `maxGrants` yang sudah tercapai.
4. Kalau tier kosong, turun ke tier di bawahnya. Tier B selalu berisi hadiah tanpa batas.
5. Pilih lewat `rng.PickWeighted`.

### Memberi hadiah

**`private void GrantReward(LuckyRewardData r)`**, lalu tambah `grantCounts`, dan terbitkan `LuckyMachineSpinResolvedEvent { rewardId }`:

| Jenis | Langkah |
|---|---|
| `SpawnItem` | `WorldItemManager.Spawn(inst, amount, rewardPoint, despawns: false)`. Tidak hilang, dan tidak bergantung pada inventori penuh. Kalau `modifier` terisi, pasang ke `inst` lebih dulu |
| `WeaponModifier` | Target: item yang dipegang kalau senjata, lalu `Base_Pistol` di inventori. Isi `inst.luckyModifierId`. Kalau `modifier.indestructible`, `inst.durability = -1`. Panggil `EquipmentManager.RefreshEquipped()` |
| `Skin` | Cari `skinTargetItemId` di inventori. Isi `inst.skinId`. `EquipmentManager` mengganti material. Kalau item tidak ada, simpan di `pendingSkins`, lalu pasang saat item didapat |
| `PermanentBuff` | `StatusEffectManager.Instance.Apply(r.buff, "lucky_machine")`. Aset memakai `Permanent`, `Intensify`, dan `maxStacks = 3` |

**Hadiah ganda:**

- **Item unik dan skin:** keluar dari undian setelah didapat.
- **Buff:** bertumpuk sampai 3 kali.
- **Modifier:** satu per senjata. Modifier baru menggantikan yang lama kalau `damageMultiplier * speedMultiplier` lebih besar. Kalau tidak, pemain mendapat `lucky_ammo` sebagai ganti.

### Tak bisa rusak

- **Fire Axe:** `Reward_FireAxe` memakai `ItemData.maxDurability = -1`. `DurabilitySystem.UseItem` selesai lebih awal kalau nilainya `-1`. Saat pukulan `MeleeCombatSystem` mengenai target, panggil `Flammable.TryIgnite(fire.fireAxeIgniteChance, true)` (#97).
- **Laser Pistol:** `GunController` memakai `WeaponData` khusus dan `mod_indestructible`. `inst.durability = -1` (#58, #113).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `EnemySacrificedEvent` (sudah ada), `LuckyMachineFreeWindowChangedEvent` (baru), `LuckyMachineSpinResolvedEvent` (baru).

**Event yang didengar:** `MajorDisasterStartedEvent`.

| Sistem | Hubungan |
|---|---|
| `DisasterManager` (#70) | Sumber event. Deteksi bencana di luar issue ini |
| `SpawnManager` (#62) | `Despawn` dengan `DespawnReason.Sacrificed` |
| `EnemyHealth`, `ThreatSystem` (#59, #108) | Kebal selama vakum, dan keluar dari daftar target |
| `SpecialZombieAI`, `WildlifeAI` (#25, #61) | Sumber `enemyId`. Pengaman macet tidak berjalan di kandang |
| `ThrowableSystem` (#96) | Umpan membantu memancing, kalau disetujui |
| `RunSeedManager` (#89) | Aliran `LuckyMachine` berurutan |
| `StatusEffectManager` (#20) | Buff `Permanent` |
| `ToolRepairSystem` (#113), `GunController` (#58), `EquipmentManager` (#60) | Modifier dan skin |
| `UIFocusStack` (#83), `HUDManager` | Panel dan hitung mundur |
| `RegionStreaming`, `WorldStateSave` (#88, #80) | Status lewat `RegionStateStore` |
| `LocalStatsManager` (#57) | Boleh mendengar event putaran |
| `MeleeCombatSystem`, `FireSystem` (#97) | Fire Axe menyalakan target |
| `ProgressionSafety` (#116) | `IsProtected(item)` |

## 4. Kasus Khusus

- **Pool dipakai ulang:** `EnemyInstance.OnSpawned` mengosongkan `IsSacrificing`, mengembalikan skala 1, menyalakan AI, dan mematikan kebal. `EnemyHealth` membuka kebal lewat `OnSpawned`-nya.
- **Game dijeda:** vakum, waktu gratis, dan animasi putaran memakai `Time.deltaTime`, jadi berhenti saat jeda. Panel mesin sendiri tidak menjeda.
- **Region dilepas saat vakum:** saat `Unloading`, semua vakum langsung `FinishSacrifice`, supaya hitungan tidak hilang.
- **Region mesin tidak dimuat saat bencana:** mesin tidak ada di memori, jadi mode gratis hangus. Pemain memang tidak bisa mencapainya dalam 30 detik.
- **Region dilepas saat mode gratis:** `CaptureState` menyimpan `freeWindowEndGameHour`. Saat dimuat lagi, sisa dihitung dari jam saat itu. Kalau sudah lewat, mode gratis berakhir.
- **Save saat berputar:** putaran belum dianggap dibayar sampai hadiah keluar. Save menyimpan `storedSpins` sebelum dipotong, supaya tidak hilang. Hasil tetap sama, karena status aliran acak ikut tersimpan (#89).
- **Zombie Militer terbakar di kandang saat fajar:** kalau belum masuk `IsSacrificing`, `SunlightExposure` berjalan normal dan tidak dihitung. Kalau sudah, vakum selesai dulu.
- **Beruang tidak mau jauh dari teritori:** batas beruang 25 meter (`WildlifeAI.md`). Mesin harus diletakkan dalam jangkauan sarang, dan ini dicek di Editor.
- **Pemain masuk kandang:** tag `Player` diabaikan. Saat gerbang tertutup, gerbang tidak menjebak pemain, karena pemain tetap bisa keluar lewat sisi lain.
- **Hadiah di `rewardPoint` tertimpa:** `WorldItem` boleh bertumpuk, dan `Interactable` tetap bisa dipilih.
- **Yang disimpan (lewat `RegionStateStore`, dengan `saveId` dari `SaveIdTag`):** `quotaProgress`, `storedSpins`, `freeWindowEndGameHour`, `freeSpinsUsed`, `gateState`, `grantCounts`, dan `pendingSkins`. Buff tersimpan di `player_effects` (#20). Modifier dan skin tersimpan pada `ItemInstance`.
- **Item terlindung (#116):** mesin tidak boleh mengonsumsi item yang memenuhi `ProgressionSafety.IsProtected(item)`. Versi saat ini tidak memakai item sebagai biaya. Aturan ini mengikat kalau biaya item ditambahkan.
- **Permadeath:** semua dihapus lewat `RunLifecycle` (#82). Buff, skin, modifier, dan kuota tidak terbawa ke run berikutnya.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** kuota dihitung saat masuk kandang, dan `EnemyAI` tidak memanggil mesin (`GameEventBus.md`). `EnemySacrificedEvent`, tag `Zombie` dan `Animal`, layer `ZoneTrigger`, aliran `LuckyMachine`, panel tanpa jeda, `Reward_FireAxe`, serta urutan modifier (#113).

**Usulan yang menunggu developer:**

1. **Nasib `AddEnergy`:** dihapus. Energi dari tiap kematian bertentangan dengan GDD, yang meminta musuh tertentu dijebak.
2. **Arti "permanen":** selama satu run. GDD bagian 3 menyatakan game diulang sepenuhnya dari awal, dan skin ikut hilang.
3. **Lokasi mesin:** rekomendasi dua mesin, sesuai tabel Lokasi. Mesin Region 3 memegang kuota, karena dua contoh GDD, Zombie Militer dan Beruang Kutub, hidup di Region 3. Region 1 dan Region 3 tidak bersebelahan (#88), dan musuh tidak menyeberang ke region yang belum dimuat, jadi mesin di Region 1 tidak bisa memenuhi kuota. Mesin kedua di Region 1 khusus mode gratis, karena satu mesin di Region 3 membuat contoh erupsi GDD tidak bisa dimainkan dari Region 1: `S1` di (1500, 1000) berjarak lurus sekitar 1.550 meter dari (350, 2050), sedangkan lari 30 detik hanya menempuh 240 meter. Pilihan lain: tetap satu mesin, dan developer mengubah contoh di GDD bagian 7. **Kalau dua mesin disetujui:** `grantCounts` dan `pendingSkins` harus dipakai bersama kedua mesin, karena `isUnique` dan `maxGrants` berlaku per run. Bentuk penyimpanan bersamanya belum ditulis di `WorldStateSave.md`.
4. **Batas putaran gratis:** 3 per jendela, sedangkan GDD hanya menyebut 30 detik. Tanpa batas, pemain bisa mendapat sampai 7 putaran.
5. **Tabungan putaran:** 1, dan gerbang menutup setelah kuota penuh.
6. **Kuota terpisah:** 5 Zombie Militer dan 3 Beruang Kutub dihitung sendiri-sendiri, dan masing-masing memberi 1 putaran.
7. **Peluang tier:** S 5 persen, A 25 persen, dan B 70 persen, dari blueprint lama.
8. **Hadiah ganda:** item unik tidak berulang, buff bertumpuk 3 kali, dan modifier mengikuti aturan di bagian 2.
9. **Hadiah tingkat B berupa barang biasa:** GDD hanya menyebut skin, buff, dan senjata ajaib. `SpawnItem` berisi peluru dan kayu ada karena issue meminta jenis ini.
10. **Blood Moon tidak memicu mode gratis:** GDD hanya menyebut *Sudden Event*.

**Perubahan kontrak yang diperlukan:** lihat daftar laporan, untuk `SpawnManager`, `DamagePipeline`, `StatusEffectManager`, `VitalsRules`, `ItemUseSystem`, `EquipmentManager`, dan `DurabilitySystem`.

## 6. Skenario Tes

1. Pancing 1 Zombie Militer ke kandang saat malam. **Hasil:** musuh tersedot 1,2 detik, kembali ke pool, `quotaProgress` 1, tanpa `EnemyKilledEvent` dan tanpa drop.
2. Masukkan `zombie_standar`. **Hasil:** musuh melewati kandang tanpa tersedot.
3. Tembak zombie saat tersedot. **Hasil:** tanpa damage, dan hitungan tetap 1.
4. Masukkan 2 Zombie Militer bersamaan. **Hasil:** hitungan naik tepat 2.
5. Capai 5 Zombie Militer. **Hasil:** `storedSpins` 1, lampu menyala, dan gerbang tertutup.
6. Masukkan 3 Beruang Kutub saat siang. **Hasil:** satu putaran terkumpul, dan beruang berada dalam batas teritorinya saat dipancing.
7. Putar dengan `storedSpins` 1. **Hasil:** hadiah keluar setelah 4 detik, `storedSpins` 0, dan gerbang terbuka.
8. Picu `MajorDisasterStartedEvent` lewat `DevConsole`. **Hasil:** HUD menghitung mundur dari 30, dan tombol gratis aktif.
9. Putar 4 kali dalam jendela. **Hasil:** hanya 3 putaran gratis.
10. Mulai putaran pada detik terakhir. **Hasil:** hadiah tetap keluar.
11. Jeda game saat mode gratis. **Hasil:** hitung mundur berhenti.
12. Paksa hadiah `lucky_fireaxe`, lalu pakai 200 kali. **Hasil:** tidak berkurang, dan tidak muncul di tab Perbaiki.
13. Paksa `lucky_laser` dua kali. **Hasil:** pemberian kedua diganti hadiah lain.
14. Paksa `lucky_hp` 4 kali. **Hasil:** `MaxHP` berhenti pada 130.
15. Pindah region saat kuota 3 dari 5, lalu kembali. **Hasil:** hitungan tetap 3.
16. Simpan, tutup paksa, muat, lalu putar. **Hasil:** hasil sama seperti sebelum dimuat (#89).
17. Mati, lalu mulai run baru. **Hasil:** kuota nol, buff hilang, skin hilang, dan `Reward_FireAxe` tidak ada.
18. Pukul zombie dengan `Reward_FireAxe` berulang. **Hasil:** sekitar 25 persen pukulan menyalakan target.
19. Cari pemanggilan `Register` di `LuckyMachine`. **Hasil:** tidak ada. Lepas region mesin, lalu muat lagi. **Hasil:** kuota dan hadiah tetap sama.
20. Kalau dua mesin disetujui: berdiri di area base Region 1, picu `MajorDisasterStartedEvent`, lalu lari ke mesin Bencana. **Hasil:** mesin tercapai dalam 30 detik, dan putaran gratis bisa dipakai.
21. Kalau dua mesin disetujui: pancing Zombie Militer ke mesin Bencana. **Hasil:** tidak ada kandang, dan kuota tidak berubah.
22. Kalau dua mesin disetujui: dapatkan `lucky_laser` dari mesin Bencana, lalu putar mesin Utama. **Hasil:** `lucky_laser` tidak keluar lagi.
