# Blueprint: SurvivalStats.cs
**Lokasi:** `docs/blueprint/raft/SurvivalStats.md`
**Deskripsi Utama:** Pemilik satu-satunya `HP`, `Stamina`, `Hunger`, `Thirst`, dan `Infection`. Menghitung laju turun dan menerima damage. Aturan rinci ada di `VitalsRules.md`, dan jalur damage ada di `DamagePipeline.md`.
**Issue:** #59, #82, #111
**Rujukan GDD:** Bagian 3 (lapar, haus, infeksi, dan Game Over saat mati). Angka laju adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`SurvivalStats : MonoBehaviour, IDamageable` berada di objek pemain.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `HP`, `MaxHP` | `float` | 100 | Darah |
| `Stamina`, `MaxStamina` | `float` | 100 | Pindah dari `IslandPlayerController` (#111) |
| `Hunger`, `MaxHunger` | `float` | 100 | Lapar |
| `Thirst`, `MaxThirst` | `float` | 100 | Haus |
| `Infection`, `MaxInfection` | `float` | 0, 100 | Infeksi |
| `isDead` | `bool` | `false` | Penanda mati, membuat `TriggerDeath` idempoten (#82) |
| `isRunEnded` | `bool` | `false` | Diisi saat `RunEndedEvent` |
| `lastDamageType` | `DamageType` | Belum terisi | Jenis damage terakhir, sumber alasan kematian (#59) |

**Angka penyetelan:** semua dibaca dari `GameBalanceConfig.vitals` (#84), sesuai `VitalsRules.md`.

`postProcessVolume` dihapus. Efek layar milik #23, lewat `VitalWarningChangedEvent`.

### Fungsi publik

Daftar lengkap ada di `VitalsRules.md`.

| Fungsi | Kegunaan |
|---|---|
| `TakeDamage(in DamageInfo)` | Damage dari luar (#59) |
| `ApplyVitalDamage(float amount, DamageType type)` | Damage kondisi, tanpa `PlayerDamagedEvent` |
| `RestoreHP`, `RestoreHunger`, `RestoreThirst`, `RestoreStamina` | Pemulihan langsung. Menggantikan `Heal(float, string)` |
| `QueueRestore(float hunger, float thirst, float seconds)` | Pemulihan bertahap |
| `AddInfection`, `ReduceInfection` | Mengubah infeksi |
| `SpendStamina`, `CanUseStamina` | Biaya stamina |
| `SetDrowning`, `SetPenalty`, `SetDrainMultiplier`, `SetDrainEnabled`, `SetInvulnerable` | Kondisi dari sistem lain |
| `SetMaxBonus(float hpMultiplier)` | Pengali `MaxHP` dari buff `Permanent` Lucky Machine, lewat `StatusEffectManager` (#20, #72) |
| `SetHPRegenBlocked(bool)` | Menahan pemulihan HP alami, dari penyakit lewat `StatusEffectManager` (#20, #45) |

## 2. Detail Logika & Fungsi

**`void Update()`**, kalau `!isDead` dan `!isRunEnded`, berurutan: `TickDrains`, `TickRestore`, `TickStamina`, `TickHP`, lalu penerbitan event. Rumus dan tabel prioritas ada di `VitalsRules.md`.

**`public void TakeDamage(in DamageInfo info)`** mengikuti `DamagePipeline.md`:

1. Kalau `isDead` atau `isRunEnded`, abaikan.
2. Terapkan aturan jalur #59, termasuk `AddInfection(info.infection)`.
3. Panggil `ReduceHP(amount, info.type, info)`.

**`private void ReduceHP(float amount, DamageType type, in DamageInfo info)`** menyimpan `lastDamageType = type`, lalu mengurangi HP. Kalau HP habis, panggil `TriggerDeath(DeathReasonMap.From(type))`.

**`private void TriggerDeath(DeathReason reason)`**:

1. Kalau `isDead`, selesai. Pemanggilan kedua diabaikan (#82).
2. `isDead = true`.
3. Matikan gerak dan jalankan urutan jatuh (`DeathCinematic`).
4. Terbitkan `PlayerDiedEvent`. Penghapusan save dan pemuatan scene hanya dilakukan `RunLifecycle` (#82). `PlayerPrefs.DeleteAll()` dilarang, karena juga menghapus pengaturan pemain.

`Infection >= MaxInfection` tidak memanggil `TriggerDeath` langsung. Infeksi hanya membunuh saat HP habis lewat baris HP 5 di `VitalsRules.md`, dengan alasan `Zombified`, sesuai GDD bagian 3.

**`RunEndedEvent`:** didengar di `OnEnable` dan dilepas di `OnDisable`. Saat terkirim, `isRunEnded = true`, sehingga pengurangan status di `Update` berhenti dan damage baru diabaikan (#82).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `PlayerDiedEvent`, `PlayerDamagedEvent`, `VitalsChangedEvent`, dan `VitalWarningChangedEvent`.

**Event yang didengar:** `RunEndedEvent`, `WarmthChangedEvent`, dan `HeavyItemChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `VitalsRules` (#111) | Aturan yang dijalankan kelas ini |
| `DamagePipeline` (#59) | `IDamageable`, `DamageInfo`, dan `DeathReasonMap` |
| `GameManager`, `RunLifecycle` (#82) | Menerima `PlayerDiedEvent`, dan mengirim `RunEndedEvent` |
| `ItemUseSystem` (#66) | `RestoreHP`, `RestoreHunger`, `RestoreThirst`, dan `QueueRestore` |
| `InfectionSystem` (#64) | `AddInfection`, `ReduceInfection`, dan `SetPenalty` |
| `SleepSystem` (#67) | `SetDrainMultiplier` dan `RestoreStamina` |
| `HUDManager`, efek layar (#23) | Mendengar event status |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** semua laju memakai `Time.deltaTime`, jadi berhenti.
- **Permadeath:** nilai kembali ke awal saat run baru, lewat `RunLifecycle` (#82).
- **Save dan load:** nilai status disimpan lewat sistem save pemain (#80). `isDead` dan `isRunEnded` tidak disimpan.
- **Damage beruntun:** `isDead` menahan `TriggerDeath` kedua, jadi `DeathReason` memakai damage yang pertama membunuh.
- **Menang saat kondisi kritis:** setelah `RunEndedEvent`, lapar, haus, dan damage berhenti, jadi pemain tidak mati saat urutan menang.
- **Lewat hari ke-60:** tidak mengubah status. Bukan Game Over.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** pemilik status (`GLOSSARY.md`), tabel prioritas HP (`VitalsRules.md`), `DeathReasonMap` (`DamagePipeline.md`), dan penghapusan save (`RunLifecycle.md`).

**Keputusan teknis di blueprint ini:** blueprint ini merangkum kontrak. Kalau ada perbedaan rincian, `VitalsRules.md` dan `DamagePipeline.md` yang dipakai.

**Usulan yang disetujui developer sebagai nilai awal:** angka laju turun perlu playtest (`VitalsRules.md`).

## 6. Skenario Tes

1. Biarkan pemain diam. **Hasil:** `Hunger` dan `Thirst` turun sesuai `GameBalanceConfig.vitals`.
2. Terima serangan zombie. **Hasil:** `HP` turun, `Infection` naik, dan `PlayerDamagedEvent` terkirim.
3. Mati karena lapar. **Hasil:** alasan `Starvation`, dan `PlayerDiedEvent` sekali.
4. Terima dua damage mematikan pada frame yang sama. **Hasil:** `TriggerDeath` berjalan sekali, dengan alasan dari damage pertama.
5. Isi `Infection` sampai 100. **Hasil:** pemain tidak langsung mati, dan HP turun sesuai `VitalsRules.md`. Saat HP habis, alasan `Zombified`.
6. Kirim `RunEndedEvent` lewat `DevConsole`, lalu serang pemain. **Hasil:** status tidak turun, dan damage diabaikan.
7. Pakai makanan dan vaksin. **Hasil:** `RestoreHunger` dan `ReduceInfection` terpanggil.
8. Mati, lalu periksa pengaturan. **Hasil:** pengaturan pemain tetap ada.

Belum diuji di Unity, karena blueprint ini baru rancangan.
