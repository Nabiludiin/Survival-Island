# Blueprint: NPCCompanion.cs
**Lokasi:** `docs/blueprint/Island_Logic/NPCCompanion.md`
**Deskripsi Utama:** AI satu NPC teman di pulau. FSM, perintah, dan kematiannya ditulis lengkap di `NPCRoster.md`. Blueprint ini mencatat kontrak komponennya.
**Issue:** #76 (melengkapi #29, #58, #78, #108, dan #117)
**Rujukan GDD:** Bagian 3 (NPC punya skill khusus) dan bagian 4 (base sebagai tempat NPC berlindung). Perilaku tempur dan infeksi NPC adalah usulan.

## 1. Arsitektur & Variabel Utama

`NPCCompanion` adalah `MonoBehaviour` di prefab NPC, dengan `NavMeshAgent`, `IInteractable`, `ITargetable`, dan `IDamageable`. Rincian FSM, `IssueCommand`, dan `Die` ada di `NPCRoster.md`.

```csharp
// Enum resmi GLOSSARY. Tidak ditulis ulang sebagai enum lokal
public enum NPCState { Follow, Attack, Idle, StayAtBase, Work, TakeCover }
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `agent` | `NavMeshAgent` | Dari prefab | `areaMask` memuat Area `DoorLink` (#78) |
| `gunUser` | `NPCGunUser` | Dari prefab | Tembakan lewat `GunCore` dan aset `NPC_Pistol` (#58) |
| `voice` | `AudioSource` | Dari prefab | Klip ucapan `NPCBarks` (#100) |
| `infectionVisual` | `NPCInfectionVisual` | Dari prefab | Tanda infeksi per tahap (#117) |
| `State`, `Command`, `currentTarget`, `workPoint` | Lihat `NPCRoster.md` | | |

**Dihapus:** `enum NPCState { Follow, Attack, Idle }` versi lama, dan `playerTarget` yang dipanggil tiap `Update()`.

## 2. Detail Logika & Fungsi

**FSM:** `Think()` setiap `npc.thinkSeconds`, sesuai tabel state di `NPCRoster.md`.

**State `Attack`:** panggil `NPCGunUser.TryShootAt(ITargetable)`, memakai `GunCore` dan aset `NPC_Pistol`. Peluru NPC tidak memakai inventori pemain.

**`ITargetable`:** `Kind = TargetKind.NPC`, `ThreatWeight` 0,6, atau 0,3 saat `TakeCover`. Daftar ke `ThreatSystem` di `OnEnable`, dan lepas di `OnDisable`.

**Pintu (#78):** Area `DoorLink` ada pada `areaMask` agen NPC, supaya NPC bisa memakai `npcLink` dan `BaseDoor.RequestNpcPass`. Agen musuh tidak memuat area ini.

**`TakeDamage(in DamageInfo info)`:**

1. Kurangi `hp`.
2. `NPCInfection.Instance.AddFromDamage(npcId, in info)` (#117).
3. Kalau `hp <= 0`, panggil `Die(reason)`.

**Infeksi (#117):** kecepatan agen dikali `npcInfection.stage3SpeedMultiplier` dan `npcInfection.stage4SpeedMultiplier`. Tanda dibaca `NPCInfectionVisual`. Tahap 4 menolak state `Work`.

**Menghindari api (#97):** `Think` memanggil `FireSystem.Instance.IsNearFire(pos, margin)`. NPC tidak memasang `Flammable`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NPCDiedEvent` dan `NPCStateChangedEvent`, sesuai `NPCRoster.md`.

| Sistem | Hubungan |
|---|---|
| `NPCRoster` (#76) | Pemilik data. Memegang `NPCRecord` dan memanggil `Warp` |
| `ThreatSystem` (#108) | `Register` di `OnEnable`, dan `Unregister` di `OnDisable` |
| `GunController` (#58) | `NPCGunUser`, `GunCore`, dan `NPC_Pistol` |
| `BaseNavigation` (#78) | `DoorLink`, `npcLink`, dan `RequestNpcPass` |
| `NPCInfection` (#117) | `AddFromDamage`, pengali kecepatan, dan penolakan `Work` |
| `NPCBarks` (#100) | `AudioSource` |
| `FireSystem` (#97) | `IsNearFire` |

## 4. Kasus Khusus

- **Pool dipakai ulang:** NPC tidak memakai pool. `OnEnable` mengosongkan `currentTarget`, menyalakan agen, dan memanggil `Warp`.
- **Game dijeda:** `Think` memakai `WaitForSeconds`, jadi berhenti saat jeda.
- **Region dilepas:** objek dimatikan oleh `NPCRoster`. `OnDisable` melepas pendaftaran `ThreatSystem`, sehingga NPC tidak diincar.
- **Tahap 4 infeksi:** perintah yang menuju `Work` berakhir di `StayAtBase`.
- **Save dan load:** data ada di `world_npcs`, dengan `NPCRoster` sebagai pemiliknya. Komponen ini tidak mendaftar sendiri.
- **Permadeath:** semua objek hilang bersama scene `IslandPhase`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `NPCState` (`GLOSSARY.md`), `ThreatWeight` (#108), `NPC_Pistol` (#58), dan `DoorLink` (#78).

**Usulan yang menunggu developer (#85):**

1. **Infeksi NPC (#117):** seluruh aturan masih usulan, dan sistemnya nonaktif sampai disetujui. Saat nonaktif, `AddFromDamage` langsung keluar.

Daftar usulan lain ada di `NPCRoster.md` bagian 5.

## 6. Skenario Tes

1. NPC `Follow` melihat zombie. **Hasil:** berpindah ke `Attack`, menembak lewat `NPCGunUser`, dan peluru pemain tidak berkurang.
2. NPC melewati pintu tertutup saat `Follow`. **Hasil:** `RequestNpcPass` terpanggil. Zombie tidak bisa memakai `npcLink`.
3. NPC masuk `TakeCover`. **Hasil:** `ThreatWeight` menjadi 0,3.
4. Matikan objek NPC. **Hasil:** `ThreatSystem` tidak lagi memilihnya sebagai target.
5. Pukul NPC dengan zombie saat `NPCInfection` aktif, lalu nonaktif. **Hasil:** infeksi naik, lalu tidak berubah.
6. NPC tahap 4 diperintah kembali ke base. **Hasil:** state tidak menjadi `Work`.
7. Lempar Molotov di jalur NPC. **Hasil:** NPC memutar.
