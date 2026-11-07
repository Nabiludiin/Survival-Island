# Blueprint: BaseTrap.cs (Base Defense & Traps)
**Lokasi:** `Survival Island/doc/blueprint/raft/BaseDefense.md`
**Deskripsi Utama:** Pertahanan markas pasif yang melukai musuh saat diinjak: Spike Trap, kawat berduri, dan api unggun.
**Issue:** #26
**Rujukan GDD:** Bagian 3 (horde menyerang base saat malam) dan bagian 4 (fungsi base). Jenis jebakan dan angkanya adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

Jebakan adalah struktur biasa. Pemain memasangnya lewat `BuildMenu` (#79) dengan `BuildableData.category = Defense`, jadi semua aturan pemasangan tetap dipakai. `BaseTrap` hanya mengatur efek saat diinjak.

```csharp
public enum TrapKind { Spike, BarbedWire, Campfire }

[CreateAssetMenu(menuName = "Survival/Trap Data")]
public class TrapData : ScriptableObject
{
    public string trapId;              // Sama dengan BuildableData.id
    public TrapKind kind;
    public DamageType damageType;      // Melee untuk paku dan kawat, Fire untuk api
    public float damagePerHit;
    public float hitIntervalSeconds;   // Jeda per target
    public float slowMultiplier = 1f;  // 1 berarti tanpa lambat
    public float slowSeconds;
    public float selfDamagePerHit;     // HP jebakan berkurang tiap kena musuh
    public float noiseRadius;
    public string hitVfxPoolTag;
}
```

### Susunan prefab

| Bagian | Layer | Fungsi |
|---|---|---|
| Root, `BaseHealth`, `StructureNode`, `BaseTrap` | `Structure`, tanpa tag `BaseWall` | Collider tipis setinggi 0,1 meter, supaya bisa dikenai palu dan pemain bisa melangkah di atasnya |
| Anak `TriggerZone` | `ZoneTrigger`, `Rigidbody` kinematic | `BoxCollider` trigger yang mendeteksi kapsul `Enemy` |
| Anak `NoiseEmitter` | Tanpa collider | Suara saat kena (#49) |

### Data jebakan

| `trapId` | Biaya | Stasiun atau node | HP (`maxHP`) |
|---|---|---|---|
| `Build_SpikeTrap` | 6 `Mat_Wood`, 2 `Mat_Stone` | Terbuka sejak awal | 60 |
| `Build_BarbedWire` | 4 `Mat_Scrap`, 2 `Mat_Wood` | Lewat `unlockNodeId` di `TechTreeManager` | 80 |
| `Build_Campfire` | Mengikuti #44 | Mengikuti #44 | Mengikuti #44 |

| Angka | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.trap.spikeDamagePerHit` | 25 | Damage paku |
| `GameBalanceConfig.trap.spikeHitIntervalSeconds` | 1,5 | Jeda paku per target |
| `GameBalanceConfig.trap.spikeSelfDamagePerHit` | 5 | Paku rusak setelah 12 kali kena |
| `GameBalanceConfig.trap.spikeNoiseRadius` | 6 | Radius suara paku |
| `GameBalanceConfig.trap.wireDamagePerHit` | 4 | Damage kawat |
| `GameBalanceConfig.trap.wireHitIntervalSeconds` | 1 | Jeda kawat per target |
| `GameBalanceConfig.trap.wireSlowMultiplier` | 0,5 | Musuh melambat di kawat |
| `GameBalanceConfig.trap.wireSlowSeconds` | 1,5 | Lama lambat setelah keluar |
| `GameBalanceConfig.trap.wireSelfDamagePerHit` | 2 | Kawat rusak setelah 40 kali kena |
| `GameBalanceConfig.trap.campfireDamagePerHit` | 8 | Damage api tiap detik |
| `GameBalanceConfig.trap.campfireHitIntervalSeconds` | 1 | Jeda api per target |
| `GameBalanceConfig.trap.tickIntervalSeconds` | 0,25 | Jeda pemeriksaan target |
| `GameBalanceConfig.trap.maxVictimsPerTrap` | 8 | Kapasitas daftar target per jebakan |
| `GameBalanceConfig.trap.maxTrapsPerRegion` | 30 | Batas jebakan per region |

**Variabel `BaseTrap`:** `data` (`TrapData`), `victims` (array `TrapVictim` sebesar `maxVictimsPerTrap`), `victimCount`, `tickTimer`, dan `campfire` (`CampfireManager`, hanya untuk `Campfire`).

## 2. Detail Logika & Fungsi

### Masuk dan keluar

**`OnTriggerEnter(Collider other)`** di `TriggerZone`, diteruskan ke `BaseTrap`:

1. Kalau tag bukan `Zombie` atau `Animal`, abaikan. Jadi pemain dan NPC tidak terkena.
2. Ambil `IDamageable` dan `EnemyAI` lewat `GetComponentInParent`.
3. Kalau `EnemyAI.IsActiveThreat == false`, atau `victims` sudah penuh, abaikan.
4. Simpan ke `victims` dengan `nextHitTime = Time.time`, lalu aktifkan `enabled = true`.

**`OnTriggerExit`** menghapus target dari `victims`, dengan menukarnya ke posisi terakhir supaya tidak mengalokasi memori. Saat `victimCount == 0`, set `enabled = false`, sehingga jebakan kosong tidak memakai `Update`.

### Tick

**`Update()`** menambah `tickTimer`. Setiap `tickIntervalSeconds`:

1. Untuk `Campfire`, kalau `!campfire.IsLit`, lewati seluruh tick.
2. Untuk tiap target, buang yang `!IsAlive` atau objeknya nonaktif. Ini menangani musuh yang kembali ke pool, karena `OnTriggerExit` tidak terpanggil.
3. Kalau `Time.time >= nextHitTime`, panggil `HitVictim(victim)`.

**`private void HitVictim(TrapVictim v)`**:

1. Isi `DamageInfo { amount = damagePerHit, type = data.damageType, source = gameObject, sourceId = data.trapId, hitPoint = posisi musuh, bodyPart = Leg, byPlayer = true }`, lalu panggil `v.target.TakeDamage`.
2. Kalau `slowMultiplier < 1`, panggil `v.ai.ApplySpeedModifier(SpeedSource.Trap, slowMultiplier, slowSeconds)`.
3. Isi `nextHitTime = Time.time + hitIntervalSeconds`.
4. Panggil `VFXManager.Instance.Play(hitVfxPoolTag, pos, rot)` (#15). `hitVfxPoolTag` harus terdaftar di `GameBalanceConfig.vfx.catalog`.
5. Kalau `noiseRadius > 0`, panggil `NoiseEmitter` dengan radius itu.
6. Kalau `selfDamagePerHit > 0`, kirim `DamageInfo { amount = selfDamagePerHit, type = Structural, byPlayer = false }` ke `BaseHealth` milik jebakan.
7. Untuk `Campfire`, panggil `Flammable.TryIgnite(fire.campfireTrapIgniteChancePerTick, byPlayer)` pada musuh di dalam zona, setiap tick damage (#97).

Damage memakai `IDamageable` kapsul musuh, sehingga melewati `HitboxModifier`. `bodyPart = Leg` hanya dipakai armor (`DamageResistanceTable`) dan #95. `byPlayer = true` membuat kematian masuk statistik pemain.

### Jenis jebakan

| Jenis | Perilaku | Musuh besar |
|---|---|---|
| Spike Trap | Damage besar berjarak, tanpa lambat | Kena damage, setelah armor |
| Kawat berduri | Damage kecil, dan melambat selama di dalam serta sesudahnya | Kena damage, tanpa lambat kalau `EnemyData.immuneToTrapSlow` |
| Api unggun | Damage `Fire` hanya kalau `IsLit`. Tidak memakai HP sendiri dan tidak membakar lebih cepat | Mengikuti `resistances` |

Menyalakan musuh dan api yang menjalar diatur #97. Jebakan ini hanya memberi damage saat musuh berada di dalam zona.

### Pemasangan dan perawatan

- **Pemasangan:** `BuildManager` memakai `slotKind = Cell`, `support = FloorOrGround`, dan `gridSize = (1, 1, 1)`. Restriksi pantai berlaku lewat `SurfaceDetection.IsBeachFootprint`.
- **Batas jumlah:** `BuildManager` menolak `Defense` di atas `maxTrapsPerRegion`, dengan pesan `MaxStructures`. Api unggun tidak dihitung.
- **NavMesh:** `navMode = None`, supaya musuh tetap berjalan melewatinya dan jalur tidak dihitung ulang. Musuh tidak menghindari jebakan.
- **Target serangan:** jebakan tidak memakai tag `BaseWall` atau `BaseDoor`, sehingga `AttackBuilding` (#78) tidak mengincarnya. Ledakan, Fireball, dan gempa tetap merusaknya.
- **Perbaikan dan bongkar:** palu memperbaiki HP lewat `BuildingIntegrity` (#47), dengan aturan biasa. Bongkar mengikuti `BuildMenu`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NoiseEmittedEvent` (lewat `NoiseEmitter`). `EnemyHitEvent`, `EnemyKilledEvent`, `StructureDamagedEvent`, dan `StructureDestroyedEvent` dikirim `EnemyHealth` dan `BaseHealth`.

**Event yang didengar:** tidak ada. `BaseTrap` hanya memakai trigger.

| Sistem | Hubungan |
|---|---|
| `BuildManager`, `BuildMenu` (#79) | Pemasangan, biaya, dan batas jumlah |
| `StructureRegistry` (#79) | Menyimpan posisi dan HP |
| `BuildingIntegrity` (#47) | Perbaikan |
| `DamagePipeline` (#59) | `DamageInfo`, `BaseHealth`, `resistances` |
| `CampfireManager` (#44) | Komponen `BaseTrap` di prefab `Build_Campfire`, dan `IsLit` |
| `EnemyAI`, `EnemyData` (#25) | `IsActiveThreat`, `ApplySpeedModifier`, `immuneToTrapSlow` |
| `AISensory`, `NoiseEmitter` (#49) | Suara jebakan bisa menarik musuh lain |
| `TechTreeManager` | Membuka kawat berduri |
| `RemoteBaseSimulation` (#90) | Bisa menghitung jebakan sebagai nilai pertahanan saat region tidak dimuat |
| `RegionLootTable` (#103) | Sumber `Mat_Scrap` |

## 4. Kasus Khusus

- **Objek pool:** jebakan memakai `Instantiate` seperti struktur lain, sesuai `BuildMenu.md`. Efeknya memakai pool. Musuh yang kembali ke pool dibuang dari `victims` pada tick berikutnya. Musuh yang dipakai ulang harus mereset `ApplySpeedModifier` di `OnSpawned`.
- **Game dijeda:** `Update` dan fisika berhenti, karena `Time.timeScale = 0`. Timer tidak maju.
- **Permadeath dan save:** posisi dan HP tersimpan di `StructureRecord` (`world_structures`). `victims` dan timer tidak disimpan. Semua terhapus bersama save run.
- **Save dimuat dengan musuh di dalam zona:** trigger terpanggil saat objek aktif, sehingga daftar terisi ulang.
- **Region dilepas:** objek nonaktif. `OnDisable` mengosongkan `victims`. `OnTriggerEnter` berikutnya mengisinya lagi.
- **Mati saat fajar:** zombie `BurnToAshes` dibuang, karena `IsActiveThreat` bernilai `false`.
- **Zombie `Submerged` atau `Dormant`:** tidak ikut, karena `IsActiveThreat` bernilai `false`.
- **Jebakan hancur saat ada target:** `OnDisable` mengosongkan daftar, dan pengali lambat berakhir sendiri setelah `slowSeconds`.
- **Banyak musuh sekaligus:** target di atas `maxVictimsPerTrap` diabaikan sampai ada yang keluar, supaya biaya pemeriksaannya tidak bertambah.
- **Pengali lambat bertumpuk:** `SpeedSource.Trap` hanya menyimpan satu nilai, dan waktunya diperbarui. Dua kawat tidak melipatgandakan lambat.
- **Api unggun padam:** tick dilewati, dan target tetap tercatat sampai keluar. Hujan dan badai mengikuti `CampfireManager`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** struktur memakai `Instantiate` (`BuildMenu.md`), `BuildCategory.Defense`, dan restriksi pantai (GDD bagian 4).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Daftar jebakan:** Spike Trap, kawat berduri, dan api unggun, sesuai issue. Rekomendasi: pakai ketiganya.
2. **Pemain dan NPC:** tidak terkena jebakan. Rekomendasi: pakai, karena GDD hanya membahas ancaman musuh dan base dipakai NPC berlindung.
3. **Hewan:** ikut terkena, karena tag `Animal` diperiksa. Rekomendasi: pakai, karena hewan bisa memasuki area base saat siang.
4. **Musuh tidak menghindar:** jalur NavMesh mengabaikan jebakan. Rekomendasi: pakai, supaya jebakan berguna dan murah untuk performa.
5. **Musuh tidak mengincar jebakan:** `AttackBuilding` hanya untuk dinding dan pintu. Rekomendasi: pakai.
6. **Keausan:** Spike Trap 12 kali kena dan kawat 40 kali, lewat HP struktur. Rekomendasi: pakai, supaya mendukung cadangan material di GDD bagian 6.
7. **Item baru `Mat_Scrap`:** bahan kawat dan bom rakitan (#96), dari reruntuhan Region 3 dan 4. Rekomendasi: pakai.
8. **Batas 30 jebakan per region.** Rekomendasi: pakai sebagai nilai awal, lalu setel lewat playtest.

## 6. Skenario Tes

1. Pasang Spike Trap di tanah. **Hasil:** biaya berkurang, hologram hijau, dan struktur muncul. Ulangi di pasir. **Hasil:** ditolak dengan `Beach`.
2. Jalankan zombie melewati Spike Trap. **Hasil:** damage 25 sekali, tanpa bonus headshot, dan `EnemyKilledEvent.byPlayer = true` kalau mati.
3. Tahan zombie di dalam zona selama 5 detik. **Hasil:** kena sekitar 3 kali, dengan jeda 1,5 detik.
4. Biarkan 12 kali kena. **Hasil:** HP jebakan nol, dan `StructureDestroyedEvent` terkirim.
5. Pasang kawat dan jalankan zombie. **Hasil:** kecepatan 50 persen di dalam dan sampai 1,5 detik setelah keluar. Ulangi dengan varian `immuneToTrapSlow`. **Hasil:** tidak melambat.
6. Biarkan zombie berdiri di api unggun menyala, lalu padamkan. **Hasil:** damage 8 per detik berhenti saat padam.
7. Jalan di atas jebakan sebagai pemain, dan suruh NPC lewat. **Hasil:** tanpa damage dan lambat.
8. Saat zombie di dalam zona, kirim kembali ke pool. **Hasil:** `victimCount` turun pada tick berikutnya, tanpa error. Pakai ulang zombie itu. **Hasil:** kecepatan normal.
9. Jeda game saat zombie di kawat. **Hasil:** damage dan lambat berhenti, lalu lanjut sesudahnya.
10. Simpan dan muat dengan jebakan HP 30. **Hasil:** posisi dan HP sama. Mati, lalu mulai run baru. **Hasil:** jebakan hilang.
11. Pasang 30 jebakan dengan 100 zombie, sambil membuka Profiler. **Hasil:** tanpa alokasi GC dari `BaseTrap`.

## Catatan Implementasi (Sistem pendukung)

- Kode: `BaseTrap.cs`, `BaseTrapZone.cs`, dan `TrapData.cs` di `Assets/LOGIC/raft/`. Config `trap` ada di `Core_Logic/GameBalance.GrupF.cs`.
- Angka diambil dari `GameBalanceConfig.trap` lewat `TrapData.GetStats`, lalu disimpan sebagai salinan per jebakan. Dengan begitu, aset tidak berubah saat bermain.
- `BaseTrapZone` dipasang di anak `TriggerZone`, lalu meneruskan `OnTriggerEnter` dan `OnTriggerExit` ke `BaseTrap`.
- Keausan jebakan dikirim paling akhir di `HitVictim`, karena bisa menghancurkan jebakan.
- `Flammable.TryIgnite` (#97) belum ada (TODO).
- Batas `maxTrapsPerRegion` belum dipakai `BuildManager`. TODO-nya ada di kode lama `BuildManager.cs`.
