# Blueprint: EnemyAttack.cs (AttackData)
**Lokasi:** `docs/blueprint/left-4-dead/EnemyAttack.md`
**Deskripsi Utama:** Waktu serangan jarak dekat musuh dengan tahapan Windup, Active, dan Recovery, supaya serangan terlihat lebih dulu, bisa dihindari, dan pemain yang dikepung masih bisa melawan.
**Issue:** #109
**Rujukan GDD:** Bagian 3 (serangan zombie menambah infeksi dan HP bisa habis karena diserang). Tahapan serangan, batas penyerang, dan semua angka waktu adalah usulan dari `docs/REFERENSI_MUSUH_L4D.md`, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`EnemyAttack` adalah komponen di objek utama musuh. `EnemyAI` memutuskan **kapan** menyerang, sedangkan `EnemyAttack` menjalankan **satu ayunan**. `OverlapSphere` di loop AI lama dihapus. Dulu damage bisa mencapai 75 per detik, karena setiap putaran 0,2 detik memberi 15 damage.

`AttackCoordinator` adalah manager per scene yang membagikan izin menyerang.

### `AttackData` (ScriptableObject)

| Field | `Zombie_Standard_Hit` | `Zombie_Ship_Bite` | `Zombie_Heavy_Hit` | Fungsi |
|---|---|---|---|---|
| `attackId` | Sama dengan nama | Sama dengan nama | Sama dengan nama | Pengenal |
| `damage` | 15 | 15 | 30 | Damage ke pemain atau NPC |
| `structureDamage` | 10 | 0 | 30 | Damage `Structural` ke bangunan |
| `infection` | `GameBalanceConfig.infection.gainStandard` (5) | 8 (`ShipHazardManager.md`) | `GameBalanceConfig.infection.gainMilitary` (10) | Masuk ke `DamageInfo.infection` |
| `range` | 2 | 2 | 2,5 | Jarak mulai menyerang |
| `hitRadius` | 0,9 | 0,9 | 1,2 | Radius overlap |
| `hitOffset` | 1 | 1 | 1,2 | Jarak titik overlap di depan musuh |
| `arcDegrees` | 120 | 120 | 150 | Sudut serangan |
| `windupSeconds` | 0,5 | 0,5 | 0,9 | Ancang-ancang |
| `trackingRatio` | 0,6 | 0,6 | 0,5 | Bagian Windup yang masih mengikuti target |
| `activeSeconds` | 0,15 | 0,15 | 0,25 | Lama jendela kena |
| `recoveryAfterHit` | 1,2 | 1,2 | 1,5 | Jeda setelah kena |
| `recoveryAfterMiss` | 0,8 | 0,8 | 1 | Jeda setelah meleset |
| `canBeInterrupted` | `true` | `true` | `false` | Bisa dibatalkan stagger |
| `usesToken` | `true` | `true` | `true` | Memakai izin `AttackCoordinator` |
| `animTrigger` | `Attack` | `Bite` | `HeavyAttack` | Trigger Animator |
| `telegraphClip` | `Zombie_Growl` | `Zombie_Growl` | `Zombie_Roar` | Suara tanda |

`Zombie_Heavy_Hit` memakai pola Tank L4D: 1,5 detik setelah kena dan 1,0 detik setelah meleset. Zombie Militer memakai aset ini. Hewan ditetapkan #61.

**Aset per varian (#25).** Field lain mengikuti `Zombie_Standard_Hit`, sampai disetel.

| Aset | `damage` | `infection` | Catatan |
|---|---|---|---|
| `Zombie_Tribal_Hit` | 10 | `gainTribal` (6) | Serangan utama Tribal |
| `Zombie_Tribal_Snare` | `tribalDamagePerSec` (5) | `gainTribal` (6) | Selama menahan, `SpecialZombieAI.md` |
| `Zombie_Lumut_Lunge` | 30 | `gainMoss` (8) | |
| `Zombie_Beku_Hit` | 10 | `gainFrozen` (5) | |
| `Zombie_Desa_Pounce` | 12 | `gainVillage` (6) | |
| `Zombie_Rock_Throw` | 10 | 0 | `windupSeconds` 1 |

**Infeksi (#64):** `AttackData.infection` diisi dari `GameBalanceConfig.infection.gain*` per jenis zombie (Standard 5, Tribal 6, Moss 8, Military 10, Frozen 5, Village 6), dan 0 untuk semua `AnimalData`. Serangan `Structural` tidak menambah infeksi. `EnemyData` mendapat field `AttackData primaryAttack`.

### Variabel `EnemyAttack`

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Phase` | `AttackPhase` | `None` | `None`, `Windup`, `Active`, atau `Recovery` |
| `current` | `AttackData` | `null` | Serangan berjalan |
| `phaseTimer` | `float` | 0 | Sisa waktu tahap, memakai `Time.deltaTime` |
| `targetDamageable` | `IDamageable` | `null` | Target saat ini |
| `isStructureAttack` | `bool` | `false` | Ayunan ke `currentBreach.target` |
| `swingHits` | `List<IDamageable>` | Kapasitas 8 | Target yang sudah kena pada ayunan ini |
| `hitAny` | `bool` | `false` | Penentu jeda kena atau meleset |
| `animEventFired` | `bool` | `false` | Animation Event sudah terpanggil |
| `nextAttackTime` | `float` | 0 | Serangan berikutnya paling cepat |

Komponen ini `enabled = false` saat `Phase == None`, jadi 50 zombie yang berjalan tidak menjalankan `Update`-nya.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.enemyAttack.maxAttackersOnPlayer` | 3 | Batas penyerang pemain sekaligus |
| `GameBalanceConfig.enemyAttack.maxAttackersOnNpc` | 2 | Batas penyerang satu NPC |
| `GameBalanceConfig.enemyAttack.waitRingRadius` | 3,5 | Jarak zombie yang menunggu giliran |
| `GameBalanceConfig.enemyAttack.tokenRetrySeconds` | 0,3 | Jeda meminta izin lagi |
| `GameBalanceConfig.enemyAttack.animEventGraceSeconds` | 0,1 | Toleransi telat Animation Event |
| `GameBalanceConfig.enemyAttack.interruptCooldownSeconds` | 0,6 | Jeda menyerang setelah dibatalkan |
| `GameBalanceConfig.enemyAttack.telegraphMinInterval` | 0,5 | Jeda global tanda arah |

### Hitungan damage

Satu zombie standar: `0,5 + 0,15 + 1,2 = 1,85` detik per pukulan, atau sekitar 8 damage per detik. Tiga penyerang: sekitar 24 damage per detik, sehingga HP penuh bertahan sekitar 4 detik kalau semua pukulan kena. Nilai lama hanya bertahan 1,3 detik dari satu zombie. Pemain yang berjalan 5 m/s bergerak 2,5 meter selama Windup, jadi pukulan bisa dihindari.

## 2. Detail Logika & Fungsi

### Memulai serangan

**`public bool TryStartAttack(ITargetable target)`**, dipanggil `EnemyAI` pada `AttackPlayer`:

1. Tolak kalau `Phase != None`, `Time.time < nextAttackTime`, atau `!target.IsTargetable`.
2. Tolak kalau jarak ke `target.Position` di atas `primaryAttack.range`.
3. Kalau `usesToken`, panggil `AttackCoordinator.TryAcquire(target, enemyId)`. Kalau gagal, kembalikan `false`.
4. `targetDamageable = target.Damageable`. Kalau `null`, lepas izin dan kembalikan `false`.
5. Panggil `BeginPhase(Windup)`.

**`public bool TryStartStructureAttack(BaseHealth wall)`**, dipanggil pada `AttackBuilding`, sama tetapi tanpa izin. Jumlah penyerang dinding sudah dibatasi `BreachSlots` (#78).

### Tahap Windup

**`BeginPhase(Windup)`**:

1. `phaseTimer = windupSeconds`, `swingHits.Clear()`, `hitAny = false`, dan `animEventFired = false`.
2. `agent.isStopped = true`, dan `anim.PlayAttack(animTrigger)` (#12). `AE_AttackHit()` diterima `AnimationController`, yang berada di objek Animator, lalu diteruskan ke `EnemyAttack.AE_AttackHit()`.
3. Jalankan tanda serangan.

**`Update`** selama Windup: putar musuh menghadap target sampai `trackingRatio` berlalu. Setelah itu arah terkunci, jadi pemain bisa menghindar ke samping.

### Masuk tahap Active

Damage terikat ke momen animasi, dengan cadangan timer:

| Kondisi | Masuk Active saat |
|---|---|
| Animator aktif dan klip punya event | `AE_AttackHit()` terpanggil, atau Windup habis ditambah `animEventGraceSeconds` |
| Animator dimatikan `OptimizationManager` atau #93 | Timer `windupSeconds` habis |

`Update` memeriksa `animator.isActiveAndEnabled` setiap frame, supaya pergantian di tengah ayunan tetap aman. `animEventFired` mencegah event dan timer berjalan dua kali. Event yang datang di luar Windup diabaikan.

### Tahap Active

**`private void DoHitCheck()`**, dijalankan setiap frame selama `activeSeconds`:

**Serangan ke pemain atau NPC:**

1. `center = transform.position + forward * hitOffset + Vector3.up`.
2. `count = PhysicsQuery.OverlapSphere(center, hitRadius, PhysicsMasks.AIAttack, buffer)`. Di Fase 2, `buffer` adalah `HordeCrowdManager.Instance.AttackBuffer` (#93). Di Fase 1, buffer statis 16 elemen.
3. `PhysicsQuery.CollectUniqueDamageables(buffer, count, results)`.
4. Untuk tiap hasil, lewati kalau berupa bangunan, sudah ada di `swingHits`, atau di luar `arcDegrees`.
5. Lewati kalau `PhysicsQuery.Raycast` dari dada musuh ke target terhalang `PhysicsMasks.AIVision`, supaya pukulan tidak menembus dinding.
6. Tambahkan ke `swingHits`, lalu `hitAny = true`.
7. Panggil `TakeDamage(new DamageInfo { amount = damage, type = Melee, source = gameObject, sourceId = data.enemyId, hitPoint, hitDirection = arah musuh ke target, bodyPart = Torso, infection, byPlayer = false })`.

`swingHits` menjamin satu ayunan hanya mengenai target yang sama satu kali. Ayunan yang lewat juga bisa mengenai NPC di samping target.

**Serangan ke bangunan:** satu kali per ayunan, langsung ke `currentBreach.target` dengan `amount = structureDamage`, `type = Structural`, dan `infection = 0`, sesuai `BaseNavigation.md`.

### Tahap Recovery

`phaseTimer = hitAny ? recoveryAfterHit : recoveryAfterMiss`. Musuh tetap diam. Setelah selesai:

1. `AttackCoordinator.Release(enemyId)`.
2. `nextAttackTime = Time.time`, lalu `Phase = None` dan `enabled = false`.
3. `EnemyAI` memilih langkah berikutnya: menyerang lagi, atau kembali ke `Chase` dengan `agent.isStopped = false`.

### Membatasi penyerang

**`AttackCoordinator`** menyimpan jumlah izin per `ITargetable.TargetId`, dalam array kecil tanpa `Dictionary` baru per panggilan.

| Fungsi | Isi |
|---|---|
| `bool TryAcquire(ITargetable t, int enemyId)` | Berhasil kalau pemegang izin target masih di bawah batas |
| `void Release(int enemyId)` | Melepas izin. Aman dipanggil berkali-kali |
| `void SetAttackerLimit(ITargetable t, int limit)` | Mengubah batas sementara. Dipakai #110 saat pemain tertahan. Nilai -1 mengembalikan batas awal |

**Zombie tanpa izin** tetap di `Chase`, tetapi `EnemyAI` mengarahkannya ke titik di lingkaran `waitRingRadius` di sekitar target, dengan sudut dari `enemyId`. Tiap `tokenRetrySeconds`, zombie meminta lagi kalau sudah sampai jangkauan. Zombie terlihat mengepung, tetapi hanya tiga yang memukul. Izin dipegang dari Windup sampai Recovery selesai.

### Tanda sebelum serangan

| Tanda | Isi |
|---|---|
| Animasi | Ancang-ancang jelas selama `windupSeconds`, misalnya lengan terangkat (#12) |
| Suara | `ThreatAudioEmitter.PlayTell(telegraphClip)` pada prefab musuh di awal Windup, supaya memakai sumber 3D dan batas suara (#114). Kalau prefab tidak punya emitter, lewati suara dan catat peringatan di Editor |
| Arah | Terbitkan `EnemyAttackTelegraphEvent { position, attackId }`, hanya kalau targetnya pemain, dan dibatasi `telegraphMinInterval`. `CombatFeedbackUI` (#51) menampilkan penanda kalau asalnya di luar pandangan |

### Dibatalkan oleh stagger

**`public void Interrupt(bool force)`**, dipanggil `EnemyHitReaction` (#95):

| Tahap | `canBeInterrupted = true` | `canBeInterrupted = false` |
|---|---|---|
| `Windup` | Batal, tanpa damage | Lanjut, kecuali `force` |
| `Active` | Batal. Damage yang sudah masuk tetap berlaku | Lanjut, kecuali `force` |
| `Recovery` | Berakhir, karena digantikan reaksi | Berakhir |

Saat batal: lepas izin, `nextAttackTime = Time.time + interruptCooldownSeconds`, `Phase = None`, dan `enabled = false`. `force` dipakai ledakan.

**`public void CancelAttack()`** menghentikan ayunan apa pun tanpa jeda tambahan. Dipakai saat mati, `BurnToAshes`, dan target mati.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `EnemyAttackTelegraphEvent` (baru).

**Event yang didengar:** tidak ada di `EnemyAttack`. `AttackCoordinator` mendengar `PlayerDiedEvent` dan `NPCDiedEvent`, lalu membatalkan izin target itu.

| Sistem | Hubungan |
|---|---|
| `EnemyAI` | Memanggil `TryStartAttack`, `TryStartStructureAttack`, dan `CancelAttack` |
| `DamagePipeline` (#59) | `IDamageable.TakeDamage` dan `DamageInfo` |
| `InfectionSystem` (#64) | Sumber nilai `infection` |
| `EnemyHitReaction` (#95) | Memanggil `Interrupt` |
| `ThreatSystem` (#108) | Menyediakan `ITargetable` |
| `BaseNavigation` (#78) | `currentBreach.target` dan `BreachSlots` |
| `SpecialZombieAI` (#25), `WildlifeAI` (#61) | Mengisi `AttackData`. Bentuk serangan khusus milik issue masing-masing |
| `PlayerPinnedState` (#110) | Memakai `SetAttackerLimit` |
| `OptimizationManager`, `HordeCrowdManager` (#93) | Bisa mematikan Animator. Timer cadangan tetap jalan |
| `AnimationController` (#12) | Klip dengan event `AE_AttackHit` |

**Batas pekerjaan:** serangan khusus varian di #25, hewan di #61, dan reaksi setelah terkena pukulan di #95.

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned` mengatur `Phase = None`, `enabled = false`, `nextAttackTime = 0`, dan mengosongkan `swingHits`. `OnReturned` memanggil `AttackCoordinator.Release`, supaya izin tidak bocor.
- **Game dijeda:** `phaseTimer` memakai `Time.deltaTime`, dan Animator juga berhenti, jadi tidak ada damage saat jeda.
- **Permadeath dan save:** tidak ada data yang disimpan. Musuh tidak disimpan (#88), dan izin hilang bersama scene. `AttackData` adalah aset.
- **Target pergi saat Windup:** ayunan tetap selesai dan meleset, sehingga memakai `recoveryAfterMiss`.
- **Target mati saat Windup:** `IsTargetable` bernilai salah, dan `CancelAttack` dipanggil.
- **Musuh mati saat ayunan:** `EnemyHealth.Died` memanggil `CancelAttack`, dan Animation Event yang telat diabaikan karena `Phase == None`.
- **Fajar:** `BurnToAshes` memanggil `CancelAttack`.
- **Pemain masuk base saat Windup:** raycast garis pandang gagal, sehingga pukulan meleset.
- **Dinding hancur saat ayunan:** `IsAlive` bernilai salah, sehingga tidak ada damage.
- **Godmode (#56):** `DamageInfo` tetap terkirim, dan `SurvivalStats` menahan HP.
- **Animasi lebih panjang dari Windup:** event terlambat tertutup oleh `animEventGraceSeconds`, jadi kecepatan klip harus disesuaikan #12. Damage tidak bergeser jauh.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** pukulan ke dinding memakai `Structural` (#59). Nilai infeksi per jenis musuh (#64). Mask `AIAttack` (#87).

**Keputusan teknis di blueprint ini:**

- Komponen hanya aktif saat ayunan berjalan.
- Damage memakai Animation Event dengan cadangan timer.
- Bangunan tidak dicari lewat overlap, tetapi langsung lewat `currentBreach.target`.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Batas penyerang:** 3 zombie sekaligus untuk pemain dan 2 untuk tiap NPC. Yang lain menunggu di lingkaran 3,5 meter.
2. **Waktu zombie standar:** Windup 0,5 detik, Active 0,15 detik, serta Recovery 1,2 detik setelah kena dan 0,8 detik setelah meleset.
3. **Pembatalan stagger:** stagger membatalkan Windup dan Active pada zombie biasa. Musuh besar hanya dibatalkan ledakan.
4. **Tanda arah:** penanda HUD hanya untuk serangan dari luar pandangan.

## 6. Skenario Tes

1. Berdiri diam di samping satu zombie selama 10 detik. **Hasil:** sekitar 5 pukulan dengan HP turun sekitar 75, dan `Infection` naik 25. Bukan HP habis dalam 1,3 detik.
2. Lihat zombie sampai lengan terangkat, lalu mundur. **Hasil:** pukulan meleset. Jeda sesudahnya sekitar 0,8 detik, bukan 1,2 detik.
3. Bergerak ke samping setelah 60 persen Windup. **Hasil:** zombie tidak lagi berputar, dan pukulan meleset.
4. Biarkan satu ayunan mengenai pemain yang berdiri dekat selama seluruh Active. **Hasil:** satu `PlayerDamagedEvent` per ayunan.
5. Matikan Animator lewat `OptimizationManager` saat zombie menyerang. **Hasil:** damage tetap masuk sesuai timer.
6. Atur klip supaya Animation Event terjadi tepat saat timer cadangan habis. **Hasil:** damage masuk satu kali.
7. Spawn 20 zombie di sekitar pemain lewat `DevConsole`. **Hasil:** paling banyak 3 yang berada di fase serangan, sisanya melingkar. HP pemain bertahan paling sedikit 4 detik.
8. Tembak zombie saat Windup sampai stagger. **Hasil:** ayunan batal, tanpa damage, dan zombie baru menyerang lagi setelah reaksi dan 0,6 detik.
9. Lakukan hal yang sama pada musuh dengan `Zombie_Heavy_Hit`. **Hasil:** ayunan tetap berlanjut. Ledakan membatalkannya.
10. Serang dari belakang pemain. **Hasil:** suara terdengar dari arah benar, dan `EnemyAttackTelegraphEvent` memicu penanda arah.
11. Berdiri di balik dinding, tepat dalam jangkauan. **Hasil:** tidak ada damage ke pemain.
12. Biarkan zombie memukul dinding. **Hasil:** `structureDamage` masuk sekali per ayunan, dan `Infection` tidak berubah.
13. Bunuh zombie yang memegang izin. **Hasil:** izin bebas, dan zombie lain segera mengisi tempatnya.
14. Pakai ulang zombie yang mati di tengah ayunan. **Hasil:** tahap `None`, dan langsung bisa menyerang normal.
15. Jeda game di tengah Windup. **Hasil:** tidak ada damage sampai jeda selesai.
