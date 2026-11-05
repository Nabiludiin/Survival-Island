# Blueprint: NPCRoster.cs & NPCData
**Lokasi:** `docs/blueprint/Island_Logic/NPCRoster.md`
**Deskripsi Utama:** Data NPC dari kapal, skill pasif, perintah pemain, dan kehidupan NPC di base. Melengkapi `NPCCompanion.md`.
**Issue:** #76 (melengkapi #29)
**Rujukan GDD:** Bagian 3 (NPC punya skill khusus, misalnya mempercepat crafting atau ahli medis) dan bagian 4 (base sebagai tempat NPC berlindung). Daftar NPC, angka skill, perintah, dan kebutuhan NPC adalah usulan.

## 1. Arsitektur & Variabel Utama

| Kelas | Jenis | Fungsi |
|---|---|---|
| `NPCData` | ScriptableObject | Data tetap per NPC |
| `NPCRoster` | Manager per scene `IslandPhase`, `ISaveable` (`SaveId = world_npcs`, `LoadOrder = 120`) | Daftar NPC hidup, skill aktif, dan penyimpanan |
| `NPCCompanion` | MonoBehaviour di prefab NPC | FSM, `IInteractable`, `ITargetable`, dan `IDamageable` |

```csharp
public enum NPCSkill { None, Medic, Crafter, Cook }
public enum NPCCommand { Follow, Wait, ReturnToBase }
// NPCState mengikuti GLOSSARY: Follow, Attack, Idle, StayAtBase, Work, TakeCover

[CreateAssetMenu(menuName = "Survival/NPC Data")]
public class NPCData : ScriptableObject
{
    public string npcId;           // npc_medic, sama dengan ID di ShipInteractable
    public string displayNameKey;  // Kunci teks untuk #104
    public int weight = 4;         // Berat di kapal (#74)
    public float maxHP = 100f;
    public NPCSkill skill;
    public WeaponData weapon;      // null berarti tidak menembak
    public GameObject prefab;
    public BarkSet barkSet;        // Ucapan NPC (#100). NPCBarks membaca maxHP untuk rasio luka
}
```

### Daftar NPC awal

| `npcId` | Nama | Berat | HP | `skill` | Senjata | Tempat kerja |
|---|---|---|---|---|---|---|
| `npc_medic` | Medis | 4 | 80 | `Medic` | `NPC_Pistol` | Tidak perlu |
| `npc_crafter` | Tukang | 4 | 120 | `Crafter` | `NPC_Pistol` | `CraftingStation` terdekat |
| `npc_cook` | Koki | 4 | 100 | `Cook` | `NPC_Pistol` | `CampfireManager` terdekat |

`RunSeedManager.CurrentLayout.shipNpcIds` memilih NPC yang muncul di kapal dari daftar ini (#89). Jumlah per run ditentukan #74 dan #89.

### Variabel

| Variabel | Pemilik | Tipe | Nilai awal | Fungsi |
|---|---|---|---|---|
| `database` | `NPCRoster` | `NPCData[]` | Dari Inspector | Pencarian `npcId` |
| `members` | `NPCRoster` | `List<NPCRecord>` | Kosong | Satu baris per NPC di pulau |
| `companion` | `NPCRecord` | `NPCCompanion` | `null` | `null` kalau objeknya sedang dimatikan |
| `isAlive`, `hp`, `state`, `command`, `position`, `yaw` | `NPCRecord` | Berbagai | Dari data | Isi yang disimpan |
| `State` | `NPCCompanion` | `NPCState` | `Follow` | State saat ini |
| `Command` | `NPCCompanion` | `NPCCommand` | `Follow` | Perintah terakhir |
| `currentTarget` | `NPCCompanion` | `ITargetable` | `null` | Target tembak |
| `workPoint` | `NPCCompanion` | `Transform` | `null` | Tempat kerja |

### Angka (`GameBalanceConfig.npc`, bagian baru)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `thinkSeconds` | 0,25 | Jeda pembaruan FSM |
| `followDistance` | 5 | Dari `NPCCompanion.md` lama |
| `followIndoorDistance` | 2,5 | Jarak saat pemain di dalam base |
| `teleportDistance` | 40 | Jarak `Warp` saat `Follow` di luar pandangan |
| `detectRadius` | 15 | Jarak mencari musuh untuk `Attack` |
| `attackLeashDistance` | 12 | Jarak `Attack` dari pemain |
| `baseRegenPerSec` | 0,5 | HP pulih di base |
| `workSearchRadius` | 30 | Jarak mencari tempat kerja |
| `cookSpeedMultiplier` | 1,25 | Skill Koki |
| `builderRepairMultiplier` | 1,25 | Skill Tukang pada perbaikan |
| `avoidancePriority` | 70 | Agen NPC memberi jalan (#87) |

Nilai lain sudah ada: `infection.medicVaccineMultiplier` 1,25, dan `crafting.npcCrafterSpeedMultiplier` 1,5.

## 2. Detail Logika & Fungsi

### Kedatangan

**`public void SpawnRescued(IReadOnlyList<string> ids, Vector3 origin)`**, dipanggil `LifeboatExtraction` (#74):

1. Untuk tiap `id`, cari `NPCData`. Kalau tidak ada, catat peringatan dan lewati.
2. `Instantiate(data.prefab)` sekali per run, di dalam scene `IslandPhase`. NPC tidak memakai `ObjectPooler`, karena jumlahnya kecil dan tidak pernah muncul berulang.
3. Agen `Warp` ke `origin + indeks * ship.arrivalNpcSpacing`.
4. Isi `NPCRecord`, `State = Follow`, `Command = Follow`.
5. `RefreshSkills()`.

### Skill dan sistem terkait

**`public float GetSkillMultiplier(NPCSkill skill)`** mengembalikan pengali kalau syarat terpenuhi, dan 1 kalau tidak:

| Skill | Syarat aktif | Sistem | Efek |
|---|---|---|---|
| `Medic` | NPC hidup | `InfectionSystem` (#64) | Dosis vaksin x`infection.medicVaccineMultiplier` |
| `Crafter` | NPC hidup, dan `State == Work` | `CraftingManager` (#77) | Waktu crafting x`crafting.npcCrafterSpeedMultiplier` |
| `Crafter` | NPC hidup, dan di base | `BuildingIntegrity` (#47) | `repairHpRatioPerSwing` x`npc.builderRepairMultiplier` |
| `Cook` | NPC hidup, dan `State == Work` | `CampfireManager` (#44) | Laju masak x`npc.cookSpeedMultiplier` |

**`private void RefreshSkills()`** dipanggil saat state NPC berubah, NPC mati, atau load selesai. Saat tukang aktif atau berhenti, fungsi ini memanggil `CraftingManager.Instance.SetNpcSpeedMultiplier(GetSkillMultiplier(Crafter))`, yang bernilai `crafting.npcCrafterSpeedMultiplier` atau 1. Sistem lain membaca saat dibutuhkan.

### State

**`NPCCompanion.Think()`**, setiap `thinkSeconds`:

| State | Perilaku | Berpindah saat |
|---|---|---|
| `Follow` | Jika jarak `> followDistance`, `SetDestination(pemain)`. Di dalam base memakai `followIndoorDistance` | Musuh dalam `detectRadius` ke `Attack`. Perintah lain |
| `Attack` | `NPCGunUser.TryShootAt(target)` (#58). Berhenti berjalan selama garis pandang ada | Musuh mati, atau jarak ke pemain `> attackLeashDistance`, kembali ke `Command` |
| `Idle` | Diam di tempat dari perintah `Wait`. Tetap menembak dari tempat | Perintah lain |
| `StayAtBase` | Berjalan ke `homePoint` di dalam base, lalu berdiri. Pulih `baseRegenPerSec` | Siang, dan ada tempat kerja, ke `Work`. Malam ke `TakeCover` |
| `Work` | Berjalan ke `workPoint`, lalu animasi kerja. Pulih `baseRegenPerSec`, dan skill aktif | Malam, atau musuh dalam `detectRadius`, ke `TakeCover`. Tempat kerja hancur ke `StayAtBase` |
| `TakeCover` | Pilih titik di dalam base terjauh dari musuh, memakai `BaseNavigation.IsInsideBase`. Tidak keluar pintu. `ThreatWeight` 0,3 | `DaybreakEvent` kembali ke `Work` atau `StayAtBase` |

**`Medic`** tidak punya tempat kerja. Pada state `Work`, medis tetap di `homePoint`, karena skillnya hanya bergantung pada hidup.

**Perpindahan otomatis:** `NightfallEvent` dan `DaybreakEvent` hanya memengaruhi NPC dengan `Command == ReturnToBase`. NPC `Follow` tetap ikut pemain walaupun malam.

**Pendeteksian musuh:** memakai `PhysicsQuery.OverlapSphere(posisi, detectRadius, Layer Enemy, buffer)` lalu cek garis pandang dengan `PhysicsMasks.AIVision`. Buffer milik NPC, supaya tanpa alokasi.

**Menghindari api (#97):** `Think` memanggil `FireSystem.Instance.IsNearFire(pos, margin)` untuk menghindari `FireArea`. NPC tidak memasang `Flammable`.

**Pemain yang diterkam (#110):** NPC tidak menolong, sesuai GDD bagian 3 (pemain melepaskan diri sendiri). Selama `PlayerPinnedState.IsPinned`, NPC tidak memilih `PlayerPinnedState.Attacker` sebagai target, supaya penyerang tidak mati atau lepas karena NPC. NPC tetap boleh melawan musuh lain.

### Perintah pemain

`NPCCompanion : IInteractable`. `IslandInteractable.md` mencatat NPC membuka menu perintah. **Usulan:** tiap tekan `E` berganti ke perintah berikutnya, tanpa panel baru, dan prompt menampilkan perintah berikutnya.

**`public void IssueCommand(NPCCommand c)`**:

| Perintah | Hasil | Syarat |
|---|---|---|
| `Follow` | `State = Follow` | Selalu |
| `Wait` | `State = Idle` | Selalu |
| `ReturnToBase` | `StayAtBase`, lalu `Work` saat siang, atau `TakeCover` saat malam | `BaseNavigation.TryGetBaseAnchor` berhasil. Kalau tidak, tampilkan `Belum ada base` |

### Di dalam base

**`PlayerBaseStateChangedEvent { isInside }`** (#78):

- NPC `Follow` di dalam base memakai `followIndoorDistance`, dan tidak berdiri dalam radius 1,5 meter dari pintu, supaya tidak menghalangi pemain.
- NPC bertabrakan dengan pemain (#87). `avoidancePriority` yang lebih rendah membuatnya memberi jalan.
- Pintu tertutup dilewati lewat `npcLink` dan `RequestNpcPass` (#78). `TakeCover` tidak memakai link, supaya pintu tidak terbuka saat malam.
- NPC tetap menembak dari dalam kalau ada garis pandang lewat pintu terbuka atau lubang. Dinding menghalangi tembakan.

### Target musuh dan NPC mati

`NPCCompanion` memasang `ITargetable` dengan `Kind = NPC`, `ThreatWeight` 0,6 atau 0,3 saat `TakeCover`, dan `Damageable = this`. Zombie dan hewan bisa mengincarnya, sesuai skor di `ThreatSystem.md`. Peluru pemain tertahan tanpa damage (#59).

**`TakeDamage(in DamageInfo info)`**: kurangi `hp`, abaikan `DamageInfo.infection`, lalu kalau `hp <= 0`, panggil `Die(reason)`.

**`private void Die(DeathReason reason)`**:

1. `isAlive = false`, dan `IsTargetable = false`.
2. Matikan agen dan AI. Tulang ragdoll pindah ke layer `Ragdoll`.
3. Terbitkan `NPCDiedEvent { npcId, reason }`.
4. `NPCRoster.RefreshSkills()`. Skill hilang permanen sampai run berakhir.
5. Mayat tetap ada 60 detik, lalu dihapus.

### Fungsi lewat data

Fungsi berikut bekerja juga saat objek NPC nonaktif, karena membaca `NPCRecord`. `Die` di `NPCCompanion` bersifat private, jadi sistem lain memakai fungsi ini.

**`public void ApplyRemoteDamage(string npcId, float amount)`:** kurangi `NPCRecord.hp`, dan kalau habis, panggil `KillNpc(npcId, DeathReason.KilledByEnemy)`.

**`public void KillNpc(string npcId, DeathReason reason)`**, dipanggil `RemoteBaseSimulation` (#90):

1. Keluar kalau `NPCRecord.isAlive` sudah `false`.
2. Kalau objek aktif, jalankan `companion.Die(reason)` lewat fungsi internal, dan selesai.
3. Kalau objek nonaktif: `isAlive = false`, terbitkan `NPCDiedEvent { npcId, reason }`, lalu `RefreshSkills()`.

**`public IReadOnlyList<string> GetAliveNpcIds()`** membaca `NPCRecord.isAlive`. Dipakai `RescueEventManager.BuildEndingContext` (#14).

### Tanpa kebutuhan makan

NPC tidak punya `Hunger` atau `Thirst`. HP pulih hanya di base.

### Saat region dilepas

Sesuai `RegionStreaming.md`:

- **`Follow`:** kalau tertinggal, `Warp` ke titik NavMesh valid di belakang pemain.
- **`StayAtBase`, `Work`, `TakeCover`:** catat ke `NPCRecord`, lalu matikan objek. Saat region kembali, nyalakan dan `Warp`. `RemoteBaseSimulation` (#90) memanggil `ApplyRemoteDamage(npcId, amount)` untuk NPC yang dimatikan. HP nol berarti `KillNpc(npcId, DeathReason.KilledByEnemy)`.
- **Skill saat objek mati:** `GetSkillMultiplier` membaca `NPCRecord`, bukan objek.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NPCDiedEvent { npcId, reason }` dari `NPCCompanion` (sudah ada), dan dari `KillNpc` saat objek nonaktif, serta `NPCStateChangedEvent { npcId, state }` (baru).

**Event yang didengar:** `NightfallEvent`, `DaybreakEvent`, `PlayerBaseStateChangedEvent`, `StructureDestroyedEvent`, serta event `Unloading` `RegionStreaming`. `NPCRescuedEvent` tidak didengar, karena daftar dikirim lewat `SpawnRescued`.

| Sistem | Hubungan |
|---|---|
| `LifeboatExtraction` (#74) | Memanggil `SpawnRescued`. Membaca nama dan skill untuk ringkasan |
| `RunSeedManager` (#89) | `shipNpcIds` |
| `InfectionSystem` (#64) | `GetSkillMultiplier(Medic)` |
| `CraftingManager` (#77) | `SetNpcSpeedMultiplier` |
| `BuildingIntegrity` (#47) | `GetSkillMultiplier(Crafter)` |
| `CampfireManager` (#44) | `GetSkillMultiplier(Cook)` |
| `ThreatSystem` (#108) | `ITargetable`, dan `chaserCounts` per NPC |
| `DamagePipeline` (#59) | `IDamageable`, serta aturan friendly fire |
| `GunController` (#58) | `NPCGunUser` dan `NPC_Pistol` |
| `BaseNavigation` (#78) | `npcLink`, `RequestNpcPass`, `IsInsideBase`, dan `TryGetBaseAnchor` |
| `PlayerPinnedState` (#110) | Dibaca `IsPinned` dan `Attacker`, supaya NPC tidak menyerang penyerang. NPC tidak menolong |
| `RemoteBaseSimulation` (#90) | `ApplyRemoteDamage`, `KillNpc`, dan membaca `members` |
| `RescueEventManager` (#14) | `GetAliveNpcIds` |
| `FireSystem` (#97) | `IsNearFire` |
| `NPCBarks` (#100) | Membaca state, event, `NPCData.barkSet`, dan `maxHP`. Klip diputar lewat `AudioSource` `NPCCompanion` |

**Batas pekerjaan:** membawa NPC di kapal (#74), dan efek buff umum (#20).

## 4. Kasus Khusus

- **Pool dipakai ulang:** NPC tidak memakai pool. Saat objek dinyalakan kembali, `OnEnable` mengosongkan `currentTarget`, menyalakan agen, dan memanggil `Warp` sebelum `SetDestination`. Jika `!agent.isOnNavMesh`, `Warp` diulang lewat `NavMesh.SamplePosition`.
- **Game dijeda:** `Think` memakai `WaitForSeconds`, dan regen memakai `Time.deltaTime`, jadi keduanya berhenti saat jeda.
- **Tidur (#67):** NPC tetap berjalan. Regen ikut `Time.deltaTime`, tanpa percepatan.
- **Base hancur:** NPC `StayAtBase`, `Work`, atau `TakeCover` kembali ke `Follow` kalau `TryGetBaseAnchor` gagal.
- **Tempat kerja hancur:** `StructureDestroyedEvent` mengosongkan `workPoint`, lalu `State = StayAtBase`, dan skill `Crafter` atau `Cook` berhenti.
- **Dua NPC dan satu tempat kerja:** tempat kerja boleh dipakai lebih dari satu NPC, karena hanya ada satu NPC per jenis skill.
- **Blood Moon (#50):** NPC `TakeCover` tetap menjadi target kalau terlihat dalam 8 meter (#108).
- **NPC `Follow` saat pemain mati:** `RunEndedEvent` menghentikan semua. Tidak ada `NPCDiedEvent`.
- **Yang disimpan di `world_npcs` (`LoadOrder = 120`):** `npcId`, `isAlive`, `hp`, `state`, `command`, `position`, dan `yaw`. **Yang tidak disimpan:** target dan jalur. Setelah load, objek dibuat ulang dari data, NPC mati tidak dibuat, dan `RefreshSkills()` dipanggil di `OnRestoreFinished`.
- **Permadeath:** semua NPC dan data dihapus lewat `RunLifecycle` (#82). Pemain harus menyelamatkan NPC lagi dari kapal.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `NPCState` (`GLOSSARY.md`), berat NPC di kapal (#74), `GetSkillMultiplier(NPCSkill.Medic)` (#64), pengali tukang 1,5 (#77), `NPC_Pistol` (#58), `world_npcs` (#80), `ThreatWeight` 0,6 dan 0,3 (#108), serta friendly fire (#59).

**Keputusan developer:** NPC tidak bisa terinfeksi, dan #117 tidak dipakai. Gigitan zombie hanya mengurangi HP NPC, dan NPC tetap menjadi target zombie sesuai `ThreatSystem.md`. Vaksin hanya untuk pemain, dan skill `Medic` tetap memperkuatnya.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Daftar NPC:** Medis, Tukang, dan Koki. GDD hanya memberi contoh crafting dan medis.
2. **Skill Tukang:** mempercepat crafting saat `Work`, dan perbaikan 1,25 kali saat di base. Tukang tidak memperbaiki sendiri.
3. **Skill Koki:** masak 1,25 kali lebih cepat saat `Work`.
4. **NPC jadi target:** ya, untuk zombie dan hewan, dengan bobot lebih rendah dari pemain.
5. **NPC mati:** hilang permanen selama run, dan skillnya hilang.
6. **Makan:** NPC tidak butuh makan. GDD tidak menyebutnya, dan beban baru tidak diperlukan.
7. **Pemulihan:** hanya di base, 0,5 HP per detik. Tidak memakai item.
8. **Perintah:** `E` berganti antara ikuti, tunggu, dan kembali ke base, tanpa panel baru.
9. **Malam:** NPC yang diperintah kembali ke base otomatis berlindung, dan pulang kerja saat fajar. NPC `Follow` tetap ikut pemain.

**Perubahan kontrak yang diperlukan:** `NPCCompanion`, `BuildingIntegrity`, `CampfireManager`, dan `BaseNavigation`.

## 6. Skenario Tes

1. Selamatkan `npc_medic` di kapal, lalu masuk pulau. **Hasil:** NPC muncul di pantai dengan state `Follow`.
2. Pakai vaksin dengan medis hidup, lalu setelah medis mati. **Hasil:** efek 1,25 kali, lalu 1 kali.
3. Perintahkan `npc_crafter` kembali ke base saat siang. **Hasil:** berjalan ke `CraftingStation`, state `Work`, dan crafting 1,5 kali lebih cepat.
4. Perbaiki dinding dengan tukang di base. **Hasil:** HP per ayunan x1,25.
5. Perintahkan `npc_cook` kembali ke base dengan api unggun menyala. **Hasil:** waktu masak berkurang sesuai pengali.
6. Tunggu 18:00. **Hasil:** NPC berhenti kerja, `TakeCover` di dalam base, dan tidak membuka pintu.
7. Tunggu 06:00. **Hasil:** NPC kembali `Work`.
8. Tekan `E` berulang pada NPC. **Hasil:** prompt dan perintah berganti Ikuti, Tunggu, dan Base.
9. Pilih Base saat belum ada base. **Hasil:** pesan `Belum ada base`, dan state tidak berubah.
10. Masuk base bersama NPC `Follow`, lalu berjalan melewati pintu. **Hasil:** NPC memberi jalan, dan tidak diam di pintu.
11. Biarkan zombie mengincar NPC sampai mati. **Hasil:** `NPCDiedEvent` terkirim sekali, skill hilang, dan ragdoll tidak menghalangi jalan.
12. Tembak NPC. **Hasil:** peluru tertahan tanpa damage.
13. Tinggalkan NPC `Work` sampai regionnya dilepas, lalu kembali. **Hasil:** NPC ada di posisi tercatat, dan HP berkurang kalau #90 menghitung serangan.
14. Simpan dengan satu NPC mati, lalu muat. **Hasil:** NPC mati tidak muncul, dan skill sesuai.
15. Jeda game. **Hasil:** NPC berhenti berpikir dan berjalan.
16. Mati, lalu mulai run baru. **Hasil:** tidak ada NPC di pulau sebelum kapal selesai.
17. Biarkan NPC `Work` di region yang dilepas mati oleh `RemoteBaseSimulation`. **Hasil:** `KillNpc` menerbitkan `NPCDiedEvent` sekali walaupun objek nonaktif, dan skill hilang.
18. Biarkan zombie menggigit NPC, lalu tekan `E` pada NPC itu. **Hasil:** HP turun, tidak ada nilai infeksi, dan urutan `E` tetap Ikuti, Tunggu, dan Base.
19. Biarkan pemain diterkam dengan NPC `Follow` dalam 8 meter. **Hasil:** NPC tidak menyerang penyerang, dan pemain harus lepas sendiri. Setelah lepas, NPC boleh menyerang penyerang itu lagi.
20. Lempar Molotov di jalur NPC. **Hasil:** NPC menghindari `FireArea`.
21. Panggil `GetAliveNpcIds` dengan satu NPC mati. **Hasil:** ID NPC itu tidak ada di daftar.
22. Aktifkan dan hentikan `Work` tukang. **Hasil:** `CraftingManager.SetNpcSpeedMultiplier` menerima 1,5, lalu 1.
