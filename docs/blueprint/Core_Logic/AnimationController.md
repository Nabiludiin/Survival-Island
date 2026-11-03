# Blueprint: AnimationController.cs
**Lokasi:** `docs/blueprint/Core_Logic/AnimationController.md`
**Deskripsi Utama:** Satu pembungkus `Animator` untuk tangan pemain, zombie, dan hewan. Blueprint ini mengatur state machine, parameter, dan transisi untuk jalan, lari, reload senjata, dan serangan musuh.
**Issue:** #12
**Rujukan GDD:** Tidak disebut langsung. Mendukung Bagian 3 (combat dan zombie terbakar) dan Bagian 5 (musuh per region). Animasi terbakar mengikuti `Claude.md` Pilar 3.

## 1. Arsitektur & Variabel Utama

**Prinsip utama:** animasi hanya tampilan. Timer gameplay tetap berada di sistem pemiliknya, seperti `GunController.reloadEndTime` dan `EnemyAttack.phaseTimer`. Semua `Animator` memakai `applyRootMotion = false`, karena gerak dimiliki `NavMeshAgent` dan `CharacterController`.

```csharp
public static class AnimIds   // Hash dibuat sekali, tanpa string per frame
{
    public static readonly int Speed = Animator.StringToHash("Speed");
    public static readonly int Reload = Animator.StringToHash("Reload");
    public static readonly int ReloadSpeed = Animator.StringToHash("ReloadSpeed");
    // Fire, Swing, Equip, CancelAction, Carry, Stagger, Knockback, Die, Burn, Emerge, Dormant, Alert
}
```

| Komponen | Letak | Tugas |
|---|---|---|
| `AnimationController` | Di objek yang sama dengan `Animator` | Menulis parameter, meneruskan event animasi, dan reset dari pool |
| `PlayerAnimDriver` | Rig tangan di viewmodel (#102) | Mengisi `Speed` tangan dan mengganti set animasi item |
| `EnemyAnimDriver` | Prefab zombie dan hewan | Mengisi `Speed` dari `NavMeshAgent` dan menerjemahkan state AI |

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `animator` | `Animator` | `GetComponent` di `Awake` | Target semua perintah |
| `referenceSpeed` | `float` | 1 | Kecepatan yang berarti `Speed = 1` |
| `attackRelay` | `EnemyAttack` | Parent, boleh kosong | Tujuan `AE_AttackHit` |
| `animSets` | `ViewmodelAnimSet[]` | Dari prefab rig | Pasangan `itemId` dan `AnimatorOverrideController` |

**Aset Animator**

| Controller | Dipakai | Layer |
|---|---|---|
| `AC_PlayerArms` | Tangan Fase 1 dan Fase 2 | `Base` untuk gerak, `Action` (override) untuk tembak, reload, ayun, dan ganti item |
| `AC_ZombieBase` | Semua zombie | `Base` |
| `AC_AnimalBase` | Semua hewan | `Base` |
| `AC_NPC` | NPC (#76) | `Base` |

Variasi musuh memakai `AnimatorOverrideController` pada prefab, misalnya Zombie Militer memakai klip jalan yang lebih berat. Jumlah state tetap satu set, jadi tidak ada state machine per jenis.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.animation.locomotionDampSeconds` | 0,1 | Waktu peredam `Speed` |
| `GameBalanceConfig.animation.enemySpeedSampleSeconds` | 0,1 | Jeda baca kecepatan agen |
| `GameBalanceConfig.animation.minReloadAnimSpeed` / `maxReloadAnimSpeed` | 0,5 / 2 | Batas kecepatan klip reload |

Durasi transisi disimpan di aset Animator, karena dibaca oleh Animator, bukan oleh kode.

## 2. Detail Logika & Fungsi

### State machine tangan pemain

`Speed` pada tangan: 0 diam, 1 jalan (`walkSpeed` 5), dan 2 lari (`sprintSpeed` 8).

| Dari | Ke | Syarat | Lama transisi |
|---|---|---|---|
| `Idle` | `Walk` | `Speed > 0,1` | 0,15 detik |
| `Walk` | `Idle` | `Speed < 0,05` | 0,2 detik |
| `Walk` | `Run` | `Speed > 1,2` | 0,15 detik |
| `Run` | `Walk` | `Speed < 1,1` | 0,2 detik |

Layer `Action`, dari `Empty`:

| Trigger | State | Selesai | Dibatalkan oleh |
|---|---|---|---|
| `Fire` | `Fire` | Klip habis, kembali ke `Empty` | `Fire` berikutnya (masuk ulang) |
| `Reload` | `Reload` | Klip habis | `CancelAction` |
| `Swing` | `Swing` | Klip habis | Tidak bisa |
| `Equip` | `Equip` | Klip habis | `Equip` berikutnya |

Pemain berlari tetap boleh menembak (`GunController`), karena layer `Action` tidak bergantung pada `Base`. `Carry` (bool) menahan tangan pada pose memegang barang berat selama `HeavyItemChangedEvent.isCarried`.

### State machine musuh

`Speed` musuh: `velocity.magnitude / referenceSpeed`. `referenceSpeed` diisi `ZombieData.moveSpeed` atau `AnimalData.chaseSpeed`.

| `AIState` | Perintah Animator | Pemanggil |
|---|---|---|
| `Idle`, `Chase`, `SeekHiding` | Hanya `Speed` | `EnemyAnimDriver` |
| `AttackPlayer`, `AttackBuilding` | Trigger dari `AttackData.animTrigger`: `Attack`, `Bite`, atau `HeavyAttack` | `EnemyAttack` (#109) |
| `Stagger` / `Knockback` | Trigger `Stagger` / `Knockback` | `EnemyHitReaction` (#95) |
| `BurnToAshes` | Trigger `Burn` | `SunlightExposure` (#63) |
| `Submerged`, `Dormant` | Bool `Dormant = true` | `SunlightExposure`, `SpecialZombieAI` |
| `Emerging` | `Dormant = false`, trigger `Emerge` | `SunlightExposure` |

| `AnimalState` | Perintah Animator |
|---|---|
| `Wander`, `ReturnToTerritory`, `FleeToDen`, `Chase` | Hanya `Speed` |
| `Alert`, `TargetUnreachable` | Bool `Alert = true`, dan `Speed` |
| `Attack` | Trigger dari `AttackData.animTrigger` |

Transisi musuh ke serangan, `Stagger`, `Knockback`, `Burn`, dan `Die` berasal dari `Any State`, dengan lama 0,1 detik. `Burn` dan `Die` tidak punya jalur keluar. Keduanya hanya hilang lewat reset pool.

### Fungsi `AnimationController`

**`public void SetLocomotion(float speed)`**: `animator.SetFloat(AnimIds.Speed, speed, locomotionDampSeconds, Time.deltaTime)`.

**`public void Play(int triggerHash)`**: `animator.SetTrigger(triggerHash)`. Fungsi ini tidak memeriksa apakah Animator aktif, sesuai aturan pada bagian 4.

**`public void PlayAttack(string animTrigger)`**: membuat hash lewat `Animator.StringToHash`, lalu memanggil `Play`.

**`public void PlayReload(float reloadSeconds, float clipSeconds)`**:

1. `ReloadSpeed = Mathf.Clamp(clipSeconds / reloadSeconds, minReloadAnimSpeed, maxReloadAnimSpeed)`.
2. `Play(AnimIds.Reload)`.

Klip mengikuti timer `GunController`, bukan sebaliknya. Kalau batas kecepatan membuat klip lebih panjang dari timer, `GunController` tetap selesai tepat waktu, dan trigger `CancelAction` memotong sisa klipnya.

**`public void CancelAction()`**: `ResetTrigger(Reload)`, lalu `Play(AnimIds.CancelAction)`.

**`public void AE_AttackHit()`**: dipanggil klip musuh, lalu diteruskan ke `attackRelay?.AE_AttackHit()`. Unity hanya mengirim event animasi ke objek yang memiliki `Animator`, sedangkan `EnemyAttack` bisa berada di parent.

### `PlayerAnimDriver`

**`LateUpdate`**:

1. Baca kecepatan datar dari `CharacterController.velocity` milik pemain. Sumber ini berlaku di kedua fase, karena `PlayerTraversal` hanya ada di Fase 2.
2. Kalau `v <= walkSpeed`, hasilnya `v / walkSpeed`. Kalau lebih, hasilnya `1 + (v - walkSpeed) / (sprintSpeed - walkSpeed)`.
3. Panggil `SetLocomotion(hasil)`. Membawa barang berat pada 2,5 m/s menghasilkan 0,5, sehingga langkah tangan tampak lambat.

**`HandleEquipped(EquippedItemChangedEvent e)`**: cari `animSets` menurut `e.itemId`, pasang `animator.runtimeAnimatorController = override`, lalu `Play(AnimIds.Equip)`. Kalau tidak ada, pakai set `Empty`. Di `ShipPhase`, tanpa `EquipmentManager`, tangan tetap memakai set `Empty`.

### `EnemyAnimDriver`

**`Update`**: tiap `enemySpeedSampleSeconds`, panggil `SetLocomotion(agent.velocity.magnitude / referenceSpeed)`. Jeda ini hemat untuk sekitar 50 zombie. Saat `agent.enabled == false`, nilainya 0.

## 3. Hubungan dengan Sistem Lain

**Event yang didengar:** `EquippedItemChangedEvent` (sticky, dibaca saat `OnEnable`), `HeavyItemChangedEvent`, dan `PlayerDiedEvent` (menghentikan pembaruan tangan). Blueprint ini tidak mengirim event.

| Sistem | Hubungan |
|---|---|
| `GunController` (#58) | Memanggil `Play(Fire)`, `PlayReload`, dan `CancelAction` pada tangan |
| `MeleeCombatSystem` | Memanggil `Play(Swing)`. Hit tetap mengikuti alurnya sendiri |
| `EnemyAttack` (#109) | Memanggil `PlayAttack`, dan menerima `AE_AttackHit` |
| `EnemyHitReaction` (#95) | Memanggil `Stagger`, `Knockback`, dan `Die`. Ragdoll mematikan Animator |
| `SunlightExposure` (#63) | `Burn`, `Dormant`, dan `Emerge` |
| `HordeCrowdManager` (#93), `OptimizationManager` | Pemilik `animator.enabled` dan `cullingMode`. Blueprint ini tidak mengubahnya saat gameplay |
| `FirstPersonCamera` (#102) | Pemilik rig viewmodel. Sway dan bob tetap di sana |
| `UIFocusStack` (#83) | Animator UI memakai `UnscaledTime`, di luar blueprint ini |
| `FootstepAudio` (#21) | Tidak memakai event animasi dari tangan, karena tangan tidak punya kaki |

**Batas pekerjaan:** getaran kamera di #24, efek partikel di #15, dan ragdoll di #95.

## 4. Kasus Khusus

- **Dipakai ulang dari pool:** `OnSpawned` memanggil `animator.enabled = true`, `animator.Rebind()`, lalu `animator.Update(0)`. `Rebind` menghapus trigger, bool `Dormant`, dan state `Burn` atau `Die` yang tersisa. `referenceSpeed` diisi ulang dari data. Fungsi ini aman dipanggil di urutan mana pun, karena `HordeCrowdManager` mengatur tingkatannya setelah objek muncul. `OnReturned` tidak perlu melepas apa pun selain langganan event.
- **Animator dimatikan LOD:** trigger pada Animator mati akan menunggu sampai `animator.Update` berikutnya, jadi tidak hilang. Pada tingkat `Culled`, klip tidak terlihat, dan damage memakai timer cadangan #109.
- **Game dijeda:** `updateMode = Normal`, jadi Animator berhenti pada `timeScale = 0`, dan lanjut dari frame yang sama. `SetFloat` memakai `Time.deltaTime` yang bernilai 0, jadi nilainya tidak berubah.
- **Reload dibatalkan:** ganti senjata, `PlayerPinnedEvent`, dan masuk air memanggil `CancelAction`, sehingga tangan tidak tertahan di pose reload.
- **Ganti controller:** mengganti `runtimeAnimatorController` mereset state dan parameter. `HandleEquipped` menulis ulang `Carry` setelah pergantian.
- **Klip belum ada:** Animator boleh memakai state kosong. Gameplay tetap jalan karena timer tidak bergantung pada klip, sehingga blueprint ini aman selama aset masih placeholder.
- **Permadeath dan save:** tidak ada data yang disimpan atau dihapus. Setelah load, state Animator dibangun dari state AI, dan `EquippedItemChangedEvent` dikirim ulang lewat `ISaveable` (#80).

## 5. Keputusan Desain

**Keputusan arsitektur di blueprint ini:**

- Animasi tidak pernah menjadi sumber kebenaran gameplay.
- Root motion mati di semua karakter.
- Satu state machine per kelompok karakter, dengan variasi lewat `AnimatorOverrideController`.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Model pemain:** pakai tangan first-person saja, tanpa tubuh penuh. Rekomendasi ini mengikuti GDD yang berpandangan first-person dan mengurangi kebutuhan aset. Konsekuensinya, pemain tidak punya bayangan tubuh.
2. **Aset animasi:** pakai placeholder sampai aset final dipilih. Semua nama state dan parameter di sini tetap sama saat klip diganti.

## 6. Skenario Tes

1. Jalan lalu lari di `IslandPhase`. **Hasil:** tangan berpindah `Idle`, `Walk`, dan `Run` tanpa loncatan.
2. Tekan reload pada Pistol dengan `reloadSeconds` berbeda dari panjang klip. **Hasil:** klip selesai bersama timer, dan peluru terisi tepat di akhir timer.
3. Reload, lalu ganti ke Pisau di tengah klip. **Hasil:** pose reload hilang, dan peluru tidak terisi.
4. Tembak sambil berlari. **Hasil:** layer `Action` berjalan, dan layer `Base` tetap di `Run`.
5. Bawa suku cadang. **Hasil:** `Carry` aktif, dan tetap aktif setelah item berganti.
6. Biarkan zombie menyerang pemain. **Hasil:** trigger sesuai `AttackData.animTrigger`, dan damage masuk pada `AE_AttackHit`.
7. Matikan Animator lewat `OptimizationManager` saat zombie menyerang. **Hasil:** damage tetap masuk lewat timer.
8. Tunggu fajar sampai zombie terbakar, kembalikan ke pool, lalu pakai ulang 20 kali. **Hasil:** zombie baru mulai dari `Idle`, tanpa pose terbakar atau `Dormant`.
9. Jeda game saat klip reload berjalan. **Hasil:** klip berhenti dan lanjut dari frame yang sama.
10. Spawn 50 zombie sambil membuka Profiler. **Hasil:** tidak ada alokasi GC dari `SetLocomotion`.
