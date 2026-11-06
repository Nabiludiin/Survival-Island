# Blueprint: NPCCompanion.cs
**Lokasi:** `Survival Island/doc/blueprint/Island_Logic/NPCCompanion.md`
**Deskripsi Utama:** AI satu NPC teman di pulau. FSM, perintah, skill, dan kematiannya ditulis lengkap di `NPCRoster.md`. Blueprint ini mencatat kontrak komponennya: siklus hidup objek, pemilihan target, damage, dan interaksi.
**Issue:** #76 (melengkapi #29, #58, #78, #108, dan #110)
**Rujukan GDD:** Bagian 3 (NPC punya skill khusus, dan pemain melepaskan diri sendiri saat diterkam) dan bagian 4 (base sebagai tempat NPC berlindung). Perilaku tempur NPC adalah usulan.

## 1. Arsitektur & Variabel Utama

`NPCCompanion` adalah `MonoBehaviour` di prefab NPC, dengan `NavMeshAgent`, `IInteractable`, `ITargetable`, dan `IDamageable`. Objek dibuat sekali per run oleh `NPCRoster.SpawnRescued`, dan tidak memakai `ObjectPooler`.

**Batas tanggung jawab:**

| Pekerjaan | Pemilik |
|---|---|
| Data tetap NPC (`NPCData`), daftar NPC, skill, dan save `world_npcs` | `NPCRoster` |
| Tabel state FSM dan aturan perpindahannya | `NPCRoster.md` bagian "State" |
| Menjalankan FSM, memilih target, menerima damage, dan menerima perintah `E` | `NPCCompanion` |
| Tembakan | `NPCGunUser` dan `GunCore` (#58) |
| Pintu | `BaseNavigation` (#78) |
| Ucapan | `NPCBarks` (#100), lewat `voice` |

```csharp
// Enum resmi GLOSSARY. Tidak ditulis ulang sebagai enum lokal
public enum NPCState { Follow, Attack, Idle, StayAtBase, Work, TakeCover }
```

### Variabel

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `agent` | `NavMeshAgent` | Dari prefab | `areaMask` memuat Area `DoorLink` (#78). `avoidancePriority` dari `npc.avoidancePriority` |
| `gunUser` | `NPCGunUser` | Dari prefab | Tembakan lewat `GunCore` dan aset `NPC_Pistol` (#58) |
| `voice` | `AudioSource` | Dari prefab | Klip ucapan `NPCBarks` (#100) |
| `data` | `NPCData` | Diisi `Initialize` | `maxHP`, `skill`, dan `weapon` |
| `record` | `NPCRecord` | Diisi `Initialize` | Baris milik `NPCRoster`. `hp` dan `isAlive` dibaca dan ditulis langsung di sini |
| `State`, `Command`, `currentTarget`, `workPoint` | Lihat `NPCRoster.md` | `Follow`, `Follow`, `null`, `null` | |
| `homePoint` | `Vector3` | Dari `BaseNavigation.TryGetBaseAnchor` | Tempat berdiri di base |
| `enemyBuffer` | `Collider[16]` | Kosong | Buffer `OverlapSphere`, tanpa alokasi |
| `thinkRoutine` | `Coroutine` | `null` | Loop `Think` |
| `TargetId` | `int` | `GetInstanceID()` | Bagian `ITargetable` |

`hp` tidak disalin ke komponen. Dengan memakai `record.hp` langsung, HP tetap benar saat objek dimatikan dan `RemoteBaseSimulation` memanggil `ApplyRemoteDamage` (#90), tanpa langkah sinkronisasi.

### Angka penyetelan

Semua dari `GameBalanceConfig.npc` (`NPCRoster.md`): `thinkSeconds` 0,25, `followDistance` 5, `followIndoorDistance` 2,5, `teleportDistance` 40, `detectRadius` 15, `attackLeashDistance` 12, `baseRegenPerSec` 0,5, dan `avoidancePriority` 70.

| Nama baru (usulan) | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.npc.corpseSeconds` | 60 | Lama mayat sebelum dihapus. Angka ini sudah tertulis di `NPCRoster.md`, tetapi belum masuk config |
| `GameBalanceConfig.npc.followThreatWeight` | 0,6 | `ThreatWeight` normal (#108) |
| `GameBalanceConfig.npc.coverThreatWeight` | 0,3 | `ThreatWeight` saat `TakeCover` (#108) |

**Dihapus:** `enum NPCState { Follow, Attack, Idle }` versi lama, `playerTarget` yang dipanggil tiap `Update()`, dan variabel `hp` lokal.

## 2. Detail Logika & Fungsi

### Siklus hidup

**`public void Initialize(NPCData data, NPCRecord record)`**, dipanggil `NPCRoster` setelah `Instantiate`:

1. Simpan `data` dan `record`. Kalau `record.hp <= 0`, isi `record.hp = data.maxHP`.
2. `gunUser` memakai `data.weapon` (#58). `null` berarti NPC tidak menembak, dan state `Attack` dilewati.
3. `voice` diserahkan ke `NPCBarks`.

**`void OnEnable()`**:

1. Kalau `record == null` atau `!record.isAlive`, keluar.
2. `currentTarget = null`, `agent.enabled = true`, lalu `Warp` ke `record.position`. Kalau `!agent.isOnNavMesh`, ulangi dengan `NavMesh.SamplePosition`.
3. `ThreatSystem.Instance.Register(this)`.
4. Daftar `PlayerPinnedEvent` dan `RunEndedEvent`, lalu baca `Last` untuk `PlayerPinnedEvent`.
5. `thinkRoutine = StartCoroutine(ThinkLoop())`.

**`void OnDisable()`**:

1. Hentikan `thinkRoutine`.
2. `ThreatSystem.Instance.Unregister(this)`.
3. Lepas semua event.
4. Tulis `record.position`, `record.yaw`, `record.state`, dan `record.command`, supaya `NPCRoster` bisa menyimpan NPC yang sedang dimatikan.

**`private IEnumerator ThinkLoop()`**: selama `record.isAlive`, panggil `Think()`, lalu tunggu `WaitForSeconds(npc.thinkSeconds)`. Jeda acak 0 sampai `thinkSeconds` di awal, supaya beberapa NPC tidak berpikir pada frame yang sama.

**`void Update()`**: hanya regen. Kalau `State` adalah `StayAtBase`, `Work`, atau `TakeCover`, dan NPC berada di dalam base, `record.hp = Min(record.hp + baseRegenPerSec * Time.deltaTime, data.maxHP)`.

### `Think`

**`private void Think()`**, urutan tiap tick:

1. Kalau `FireSystem.Instance.IsNearFire(pos, margin)`, cari titik di luar api dan berjalan ke sana (#97). Tick ini selesai.
2. `currentTarget = SelectTarget()`.
3. Jalankan baris state di tabel `NPCRoster.md`, termasuk perpindahan state.
4. Kalau state berubah, perbarui `ThreatWeight`, terbitkan `NPCStateChangedEvent { npcId, state }`, dan minta `NPCRoster.RefreshSkills()`.

**`private ITargetable SelectTarget()`**:

1. `n = PhysicsQuery.OverlapSphere(pos, detectRadius, Layer Enemy, enemyBuffer)`.
2. Untuk tiap collider, ambil `ITargetable` lewat parent. Lewati yang `!IsTargetable`.
3. **Pemain diterkam (#110):** kalau `PlayerPinnedState.IsPinned` dan `target.Damageable == PlayerPinnedState.Attacker.DamageTarget`, lewati target ini.
4. Lewati target tanpa garis pandang, lewat `PhysicsQuery.Raycast` dengan `PhysicsMasks.AIVision` dari mata NPC ke `AimPoint`.
5. Pilih target terdekat. Target lama dipertahankan kalau masih sah, supaya NPC tidak berpindah-pindah.

Pemeriksaan langkah 3 dilakukan setiap tick, sebelum `TryShootAt`. Karena `Attack` hanya menembak dari dalam `Think`, tidak ada tembakan sengaja ke penyerang selama pemain tertahan.

**`HandlePinned(PlayerPinnedEvent e)`**: kalau `e.isPinned` dan `currentTarget` adalah penyerang itu, kosongkan `currentTarget` saat itu juga. Kalau NPC sedang `Attack` dan tidak ada target lain, kembali ke state dari `Command`. NPC tetap boleh melawan musuh lain. Setelah pemain lepas, penyerang itu kembali menjadi target biasa.

NPC tidak menambah `EscapeProgress`, tidak menembak penyerang, dan tidak menusuknya. Pemain hanya bisa melepaskan diri sendiri (keputusan developer).

### `ITargetable`

| Anggota | Nilai |
|---|---|
| `Kind` | `TargetKind.NPC` |
| `Position` | `transform.position` |
| `AimPoint` | Titik dada dari prefab |
| `ThreatWeight` | `coverThreatWeight` (0,3) saat `TakeCover`, selain itu `followThreatWeight` (0,6) |
| `IsTargetable` | `record.isAlive && isActiveAndEnabled` |
| `Damageable` | `this` |

### `IDamageable`

**`public bool IsAlive => record != null && record.isAlive`.**

**`public void TakeDamage(in DamageInfo info)`**:

1. Kalau `!IsAlive` atau `info.amount <= 0`, abaikan.
2. Terapkan friendly fire (`DamagePipeline.md`):

| Kondisi | Damage |
|---|---|
| `Bullet`, dari pemain atau NPC | 0. Peluru tertahan kapsul NPC |
| `Melee` dengan `byPlayer = true` | 0 |
| `Explosion` atau `Fire` dengan `byPlayer = true` | Dikali `damage.playerExplosionSelfMultiplier` (0,5) |
| Serangan musuh, ledakan lingkungan, Fireball, api lingkungan | Penuh |

3. Kalau hasilnya 0, keluar tanpa event.
4. `record.hp -= amount`. `DamageInfo.infection` diabaikan, karena NPC tidak bisa terinfeksi.
5. Kalau `record.hp <= 0`, panggil `Die(DeathReasonMap.From(info.type))`.

Langkah `Die` ada di `NPCRoster.md`. Mayat dihapus setelah `npc.corpseSeconds`, memakai `WaitForSeconds`.

### `IInteractable`

| Anggota | Nilai |
|---|---|
| `IsAvailable` | `IsAlive` |
| `HoldSeconds` | 0 |
| `GetPrompt()` | Nama perintah berikutnya: Ikuti, Tunggu, atau Base. Teks dari #104 |
| `CanInteract` | Ditolak saat `IsPinned` (input terkunci #110) atau NPC sedang `Attack` dengan target dalam 5 meter (usulan) |
| `Interact` | `IssueCommand(berikutnya)`, sesuai `NPCRoster.md`. Kembalikan `false` kalau `ReturnToBase` ditolak karena belum ada base |

Collider NPC berada di layer `NPC`, yang masuk mask `Interact` (#87).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NPCDiedEvent` dan `NPCStateChangedEvent`, sesuai `NPCRoster.md`.

**Event yang didengar:** `PlayerPinnedEvent` (sticky) dan `RunEndedEvent`. `NightfallEvent`, `DaybreakEvent`, `PlayerBaseStateChangedEvent`, dan `StructureDestroyedEvent` diteruskan lewat aturan di `NPCRoster.md`. Semua dilepas di `OnDisable`.

| Sistem | Hubungan |
|---|---|
| `NPCRoster` (#76) | Pemilik data. Memanggil `Initialize`, memegang `NPCRecord`, dan memanggil `Warp` |
| `ThreatSystem` (#108) | `Register` di `OnEnable`, dan `Unregister` di `OnDisable` |
| `PlayerPinnedState` (#110) | `IsPinned` dan `Attacker.DamageTarget`. NPC tidak menolong |
| `GunController` (#58) | `NPCGunUser`, `GunCore`, dan `NPC_Pistol`, dengan `byPlayer = false` |
| `DamagePipeline` (#59) | `IDamageable`, friendly fire, dan `DeathReasonMap` |
| `BaseNavigation` (#78) | `DoorLink`, `npcLink`, `RequestNpcPass`, `IsInsideBase`, dan `TryGetBaseAnchor` |
| `PlayerInteractor`, `IslandInteractable` (#17) | Perintah lewat `E` |
| `NPCBarks` (#100) | `AudioSource` |
| `FireSystem` (#97) | `IsNearFire` |
| `PhysicsLayers` (#87) | Layer `NPC`, mask `AIVision`, dan layer `Enemy` untuk deteksi |

## 4. Kasus Khusus

- **Pool dipakai ulang:** NPC tidak memakai pool. `OnEnable` mengosongkan `currentTarget`, menyalakan agen, dan memanggil `Warp`.
- **Game dijeda:** `Think` memakai `WaitForSeconds`, dan regen memakai `Time.deltaTime`, jadi keduanya berhenti saat jeda.
- **Region dilepas:** objek dimatikan oleh `NPCRoster`. `OnDisable` menulis posisi ke `record` dan melepas pendaftaran `ThreatSystem`, sehingga NPC tidak diincar.
- **Pemain diterkam:** NPC tidak menolong dan tidak menargetkan penyerang. Peluru NPC yang meleset dan mengenai penyerang tetap memberi damage, tetapi tidak melepas terkaman, karena `byPlayer = false`.
- **Penyerang kembali ke pool saat terkaman:** `IsTargetable` penyerang menjadi `false`, jadi NPC tidak memilihnya lagi.
- **NPC kena ledakan tong pemain:** menerima 50 persen setelah falloff (`EnvironmentalHazard.md`).
- **Run berakhir:** `RunEndedEvent` menghentikan `thinkRoutine` dan agen. Tidak ada `NPCDiedEvent`.
- **Save dan load:** data ada di `world_npcs`, dengan `NPCRoster` sebagai pemiliknya. Komponen ini tidak mendaftar sendiri. Target, jalur, dan status terkaman tidak disimpan.
- **Permadeath:** semua objek hilang bersama scene `IslandPhase`, dan `NPCRoster` mengosongkan `members`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `NPCState` (`GLOSSARY.md`), `ThreatWeight` (#108), `NPC_Pistol` (#58), `DoorLink` (#78), dan friendly fire (#59).

**Keputusan developer:**

- NPC tidak bisa terinfeksi, dan #117 tidak dipakai. NPC tetap menjadi target zombie dan menerima damage HP.
- NPC tidak menolong pemain yang diterkam, dan tidak menyerang penyerangnya selama pemain tertahan. Hanya damage pemain yang melepas terkaman atau jerat (`PlayerPinnedState.md`).

Daftar usulan umum ada di `NPCRoster.md` bagian 5.

**Usulan baru (disetujui developer sebagai nilai awal):**

1. **HP disimpan langsung di `NPCRecord`:** tidak ada salinan di komponen.
2. **`corpseSeconds`, `followThreatWeight`, dan `coverThreatWeight` masuk `GameBalanceConfig.npc`.**
3. **Perintah `E` ditolak saat NPC bertarung jarak dekat:** mencegah perintah tak sengaja di tengah pertempuran.
4. **Jeda acak awal `Think`:** menyebar beban beberapa NPC.

## 6. Skenario Tes

1. NPC `Follow` melihat zombie. **Hasil:** berpindah ke `Attack`, menembak lewat `NPCGunUser`, dan peluru pemain tidak berkurang.
2. NPC melewati pintu tertutup saat `Follow`. **Hasil:** `RequestNpcPass` terpanggil. Zombie tidak bisa memakai `npcLink`.
3. NPC masuk `TakeCover`. **Hasil:** `ThreatWeight` menjadi 0,3.
4. Matikan objek NPC. **Hasil:** `ThreatSystem` tidak lagi memilihnya sebagai target.
5. Pukul NPC dengan zombie. **Hasil:** HP turun, kecepatan dan state `Work` tidak berubah, dan tidak ada nilai infeksi.
6. Lempar Molotov di jalur NPC. **Hasil:** NPC memutar.
7. Biarkan Jaguar menerkam pemain di dekat NPC `Attack` yang sedang menembak Jaguar itu. **Hasil:** NPC berhenti menembak Jaguar pada tick berikutnya, dan pemain harus lepas sendiri.
8. Ulangi dengan zombie lain di dekat NPC. **Hasil:** NPC tetap menembak zombie lain.
9. Setelah pemain lepas, biarkan Jaguar tetap di dekat NPC. **Hasil:** NPC boleh menargetkan Jaguar lagi.
10. Tembak NPC dengan Pistol, lalu pukul dengan Pisau. **Hasil:** damage 0 dan tidak ada event.
11. Ledakkan Bom Rakitan di dekat NPC. **Hasil:** NPC menerima 50 persen damage setelah falloff.
12. Biarkan Fireball mengenai NPC. **Hasil:** damage penuh.
13. Bunuh NPC dengan zombie. **Hasil:** `NPCDiedEvent` dengan `KilledByEnemy`, dan mayat hilang setelah 60 detik.
14. Matikan objek NPC saat HP 50, panggil `ApplyRemoteDamage(npcId, 20)`, lalu nyalakan lagi. **Hasil:** HP 30.
15. Tekan `E` pada NPC saat zombie berada 3 meter darinya. **Hasil:** perintah ditolak.
16. Jeda game saat NPC berjalan. **Hasil:** `Think` dan regen berhenti.
17. Buka Profiler dengan tiga NPC aktif. **Hasil:** `Think` tidak mengalokasi GC, dan tidak berjalan pada frame yang sama.
