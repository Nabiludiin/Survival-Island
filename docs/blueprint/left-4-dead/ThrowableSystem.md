# Blueprint: ThrowableSystem.cs
**Lokasi:** `docs/blueprint/left-4-dead/ThrowableSystem.md`
**Deskripsi Utama:** Barang lempar khas L4D untuk mengendalikan kerumunan: Molotov, Bom Rakitan, Batu, dan Suar.
**Issue:** #96
**Rujukan GDD:** Usulan, belum ada di GDD. Sistem ini nonaktif sampai developer menyetujui. Rekomendasi: sebaiknya dipakai, karena peluru sangat terbatas dan GDD bagian 7 mengharuskan pemain memancing musuh ke kandang Lucky Machine.

## 1. Arsitektur & Variabel Utama

`ThrowableSystem` adalah manager di scene `IslandPhase`. `ThrowableProjectile` adalah komponen prefab proyektil dari `ObjectPooler`. Fase 1 tidak memakainya.

```csharp
public enum ThrowEffect { Fire, Explosion, NoiseOnly, Lure }

[CreateAssetMenu(menuName = "Survival/Throwable Data")]
public class ThrowableData : ScriptableObject
{
    public string itemId;             // Item di inventori
    public string poolTag;            // Tag proyektil di ObjectPooler
    public ThrowEffect effect;
    public float throwForce;          // m/s
    public float upwardBias;          // Tambahan arah atas
    public bool detonateOnImpact;     // true berarti langsung saat menyentuh
    public float fuseSeconds;         // Sumbu kalau tidak langsung
    public float radius;
    public float damage;              // Fire: per detik. Explosion: total
    public float durationSeconds;     // Lama api atau pancingan
    public float noiseRadius;         // Radius Noise Sphere
    public float noiseIntervalSeconds;// 0 berarti sekali saja
}
```

### Daftar barang lempar

Semua nilai berasal dari `GameBalanceConfig.throwable.<nama>`, dan disalin ke `ThrowableData` saat `OnEnable`.

| Item | `itemId` | Efek | Gaya | Sumbu | Radius | Damage | Durasi | Noise |
|---|---|---|---|---|---|---|---|---|
| Molotov | `Throw_Molotov` | `Fire` | 15 | Langsung | 3,5 | 15 per detik | 8 detik | 15 sekali |
| Bom Rakitan | `Throw_Bomb` | `Explosion` | 13 | 3 detik | 6 | 120 | Sesaat | 40 sekali |
| Batu | `Throw_Rock` | `NoiseOnly` | 18 | Langsung | 0 | 0 | Sesaat | 12 sekali |
| Suar | `Throw_Flare` | `Lure` | 15 | 0,5 detik | 0 | 0 | 15 detik | 25 tiap 1 detik |

Nama config: `molotovForce`, `molotovRadius`, `molotovDamagePerSec`, `molotovDuration`, `molotovNoise`, `bombForce`, `bombFuse`, `bombRadius`, `bombDamage`, `bombNoise`, `rockForce`, `rockNoise`, `flareForce`, `flareFuse`, `flareDuration`, `flareNoise`, dan `flareNoiseInterval`.

| Angka lain | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.throwable.enabled` | `false` | Sistem nonaktif sampai disetujui |
| `GameBalanceConfig.throwable.holdToAimSeconds` | 0,2 | Lama menahan `G` sebelum membidik |
| `GameBalanceConfig.throwable.throwCooldownSeconds` | 0,8 | Jeda antar-lemparan |
| `GameBalanceConfig.throwable.arcPointCount` | 30 | Titik garis perkiraan |
| `GameBalanceConfig.throwable.arcTimeStep` | 0,06 | Jarak waktu antar-titik |
| `GameBalanceConfig.throwable.maxLifetimeSeconds` | 25 | Batas umur proyektil |
| `GameBalanceConfig.throwable.maxActiveLures` | 2 | Batas Suar aktif sekaligus |

**Variabel `ThrowableSystem`:** `throwables` (`ThrowableData[]`), `selectedIndex`, `isAiming`, `gHeldTime`, `cooldownTimer`, `arcLine` (`LineRenderer`, 30 titik yang dibuat sekali), `landingMarker`, dan `arcPoints` (`Vector3[]`).

## 2. Detail Logika & Fungsi

### Tombol dan pilihan

Tombol `Throw` (`G`) sudah ada di `InputManager.md`. `Update()` berjalan hanya kalau `enabled` dan `EquipmentManager.locks == EquipLock.None`.

| Input | Hasil |
|---|---|
| Ketuk `G` kurang dari `holdToAimSeconds` | `SelectNext()`, yaitu pindah ke jenis berikutnya yang jumlahnya lebih dari 0 |
| Tahan `G` | `BeginAim()` menampilkan garis perkiraan |
| Lepas `G` saat membidik | `Throw()` |
| `SecondaryHeld` saat membidik | `CancelAim()`. Barang tidak terpakai |

Item aktif tidak berganti saat melempar. Viewmodel item aktif turun sebentar lewat `EquipmentManager`, lalu naik lagi.

### Garis perkiraan

**`private void UpdateArc()`** berjalan tiap frame saat membidik:

1. Hitung `v0 = camera.forward * throwForce + Vector3.up * upwardBias`.
2. Untuk tiap titik, hitung posisi berikutnya: `p = p0 + v0 * t + 0,5 * Physics.gravity * t * t`.
3. Periksa tiap ruas lewat `PhysicsQuery.Raycast` dengan `PhysicsMasks.Ground`. Kalau kena, berhenti di titik itu dan pasang `landingMarker`.
4. Isi `arcLine.SetPositions`. Tidak ada alokasi, karena array dipakai ulang.

### Melempar

**`private void Throw()`**:

1. Kalau `cooldownTimer > 0` atau `InventoryManager.HasItem(itemId, 1)` bernilai `false`, batalkan.
2. Panggil `InventoryManager.RemoveItem(itemId, 1)`.
3. Panggil `ObjectPooler.Instance.SpawnFromPool(poolTag, handPosition, rotation)`.
4. Panggil `projectile.Launch(data, v0, playerCollider)`, lalu isi `cooldownTimer`.

### Proyektil

`ThrowableProjectile : MonoBehaviour, IPoolable` memakai layer `Projectile`, `Rigidbody`, dan `Collider`.

- **`Launch`:** panggil `Physics.IgnoreCollision(col, playerCollider, true)`, setel kecepatan `Rigidbody`, dan mulai hitung `fuseTimer`.
- **`OnCollisionEnter`:** kalau `detonateOnImpact`, panggil `Detonate()`. Selain itu, biarkan fisika memantulkannya.
- **`Update`:** kurangi `fuseTimer`. Kalau habis, panggil `Detonate()`. Kalau umur melewati `maxLifetimeSeconds`, kembalikan ke pool.

**`private void Detonate()`** memeriksa efek:

| Efek | Langkah |
|---|---|
| `Fire` | `FireSystem.Instance.SpawnFireArea(pos, radius, durationSeconds, damage, byPlayer = true)`, lalu kirim noise, dan kembali ke pool |
| `Explosion` | `ExplosionDamage.Apply(pos, radius, info)` dengan `type = Explosion` dan `byPlayer = true`. Munculkan efek ledakan dari pool, panggil `CameraShake.PlayAt(CameraShakeKind.Explosion, pos)` (#24), kirim noise, dan kembali ke pool. Damage ledakan ke pemain tidak menghasilkan getaran kedua |
| `NoiseOnly` | Kirim noise sekali, lalu kembali ke pool |
| `Lure` | Nyalakan `Light` dan partikel. Kirim noise tiap `noiseIntervalSeconds` selama `durationSeconds`, lalu kembali ke pool |

Noise dikirim lewat `NoiseEmitter` di prefab: `NoiseEmittedEvent { position, radius, sourceKind = Throwable }`. Suar kelebihan dari `maxActiveLures` memadamkan Suar paling lama.

### Efek pancingan

Ada dua tingkat, sesuai `ThreatSystem.md` (#108):

1. **Suara (Batu, Bom, Molotov):** `AISensory` (#49) menerima Noise Sphere di dalam `radius`. Musuh tanpa target menuju titik itu dalam mode mencari, lalu kembali normal. Musuh yang sudah punya target tidak teralihkan.
2. **Umpan (Suar):** selain suara, `Detonate` memasang `LureTargetable : ITargetable` dengan `Kind = Lure`, `ThreatWeight = 1`, dan `Damageable = null`, lewat `ThreatSystem.Register`. Musuh dengan `reactsToLure` beralih ke Suar lewat override umpan, walau sudah punya target.
3. Saat Suar padam, panggil `ThreatSystem.Unregister`, lalu musuh memilih target secara normal.

Melempar Suar ke kandang Lucky Machine memancing musuh masuk trigger kandang. Kuota tetap dihitung #72.

### Damage ke pemain, NPC, dan bangunan

Semua lewat `IDamageable`, sesuai `DamagePipeline.md`.

| Sumber | Pemain | NPC | Bangunan | Musuh |
|---|---|---|---|---|
| Bom Rakitan | Dikali `playerExplosionSelfMultiplier` (0,5) | Dikali 0,5 | Penuh | Penuh, setelah armor Torso |
| Molotov | Dikali 0,5 | Dikali 0,5 | Penuh, tambahan bakar dari #97 | Penuh, sesuai `resistances` |
| Batu dan Suar | 0 | 0 | 0 | 0 |

### Reaksi per varian

Kebal api memakai `EnemyData.resistances`. Pancingan memakai `reactsToLure` di profil ancaman #108, dan `ThreatSystem` melewati musuh yang sedang `Dormant` atau `Submerged`.

| Varian | Pengali `Fire` | Terpancing Suar | Catatan |
|---|---|---|---|
| Zombie standar | 1 | Ya | Target utama |
| Zombie Tribal | 1 | Ya | |
| Zombie Lumut | 2 | Tidak selama `Dormant` | Setelah bangun (#25), ikut terpancing |
| Zombie Militer | 0,5 | Ya | Armor Torso berlaku pada ledakan |
| Zombie Beku | 1,5 | Ya | |
| Zombie Desa | 1 | Tidak selama `Submerged` | Saat `Submerged`, juga tidak terkena damage (#63) |
| Hewan | 1 | Tidak | `reactsToLure = false` |

### Resep

Bahan mengikuti `CraftingRecipe` (#77).

| Resep | Bahan | Hasil | Stasiun | Waktu |
|---|---|---|---|---|
| `rec_throw_rock` | 1 `Mat_Stone` | 2 `Throw_Rock` | `Hand` | 1 detik |
| `rec_throw_flare` | 1 `Mat_Charcoal`, 1 `Mat_Cloth`, 1 `Mat_Wood` | 1 `Throw_Flare` | `WoodBench` | 5 detik |
| `rec_throw_molotov` | 2 `Mat_TreeSap`, 1 `Mat_Cloth` | 1 `Throw_Molotov` | `WoodBench` | 6 detik |
| `rec_throw_bomb` | 2 `Mat_Scrap`, 2 `Mat_Charcoal`, 1 `Mat_Rope` | 1 `Throw_Bomb` | `WoodBench` | 10 detik |

### HUD

`ThrowableSystem` menerbitkan `ThrowableChangedEvent { itemId, count }` saat pilihan atau jumlahnya berubah. Jumlah dibaca dari `InventoryManager.GetCount(itemId)` setiap kali `InventoryChangedEvent` menyangkut barang lempar. `HUDManager` menampilkan ikon dan jumlah di dekat tampilan peluru, serta menyembunyikannya kalau jumlah semua jenis 0.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `ThrowableChangedEvent` (baru), `NoiseEmittedEvent` (lewat `NoiseEmitter`).

**Event yang didengar:** `InventoryChangedEvent`, `UIFocusChangedEvent`, `PlayerPinnedEvent`, `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | `Throw` dan `SecondaryHeld` |
| `InventoryManager`, `ItemData` | Jumlah dan pengurangan item berjenis `Consumable` |
| `EquipmentManager` (#60) | Membaca `locks`, dan menurunkan viewmodel |
| `ObjectPooler` | Proyektil dan efek |
| `PhysicsQuery`, `PhysicsMasks` (#87) | Garis perkiraan |
| `DamagePipeline` (#59) | `ExplosionDamage.Apply` dan `DamageInfo` |
| `FireSystem` (#97) | Area api Molotov |
| `AISensory`, `NoiseEmitter` (#49), `ThreatSystem` (#108) | Pancingan |
| `CraftingManager`, `CraftingStation` (#77) | Resep |
| `LuckyMachine` (#72) | Memancing musuh ke kandang |
| `CameraShake` (#24), `HUDManager` | Efek dan jumlah barang |

Batas issue: ledakan tong ada di `EnvironmentalHazard`, api menyebar di #97, dan jebakan di #26.

## 4. Kasus Khusus

- **Objek pool:** `OnSpawned` mengembalikan kecepatan `Rigidbody` ke 0, mematikan `isKinematic`, `Light`, dan partikel, serta mereset `fuseTimer` dan umur. `OnReturned` memanggil `Physics.IgnoreCollision(..., false)`, supaya pengabaian tidak terbawa.
- **Game dijeda:** `Time.deltaTime` dan fisika berhenti, jadi sumbu dan Suar ikut berhenti. `UIFocusChangedEvent.isGameplayBlocked` memanggil `CancelAim()`.
- **Permadeath dan save:** jumlah barang ikut `InventoryManager`. `selectedIndex` tersimpan lewat `ISaveable` dengan `SaveId = world_throwables` dan `LoadOrder = 60`, sesudah `player_inventory`. Proyektil yang masih di udara atau Suar yang menyala tidak disimpan. Semua terhapus saat permadeath.
- **Diterkam (#110) atau membawa Heavy Item:** `locks` tidak `None`, sehingga bidikan batal.
- **Jatuh ke air:** `Water` adalah trigger dan tidak bertabrakan dengan `Projectile`. Saat `Detonate`, periksa `Physics.CheckSphere(pos, 0,2, 1 << Water, QueryTriggerInteraction.Collide)`. Pengecualian ini mencari trigger air, sesuai aturan `PhysicsLayers.md`. Molotov dan Suar padam tanpa efek. Bom tetap meledak.
- **Proyektil jatuh ke region yang dilepas:** `maxLifetimeSeconds` mengembalikannya ke pool.
- **Hujan dan badai:** Molotov mengikuti aturan #97.
- **Stasiun dan region:** barang bisa dilempar di semua region. Tidak bisa saat `Swimming` atau `Climbing`, karena `locks`.
- **Sistem nonaktif:** `enabled = false` membuat `Throw` diabaikan, HUD disembunyikan, resep tidak didaftarkan, dan `Mat_Scrap` tetap berguna untuk kawat berduri (#26).
- **#97 belum tersedia:** Molotov memakai `DamagePipeline` langsung, dengan tick damage setiap 1 detik lewat `OverlapSphere`, sampai #97 selesai.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** tombol `Throw` di `G` (`InputManager.md`), `ExplosionDamage` (`DamagePipeline.md`), serta layer `Projectile` (`PhysicsLayers.md`).

**Usulan yang menunggu developer (dicatat di #85):**

1. **Sistem dipakai atau tidak.** Rekomendasi: dipakai. Nilai awal tetap `enabled = false` sampai disetujui.
2. **Daftar barang:** Molotov, Bom Rakitan, Batu, dan Suar. Rekomendasi: pakai keempatnya, dengan Batu sebagai pilihan murah untuk permainan senyap.
3. **Garis perkiraan dan tombol:** ketuk `G` memilih, tahan membidik, dan lepas melempar. Rekomendasi: pakai, supaya hotbar tidak berubah.
4. **Friendly fire api:** Molotov mengikuti baris ledakan buatan pemain. Rekomendasi: pakai.
5. **Reaksi varian:** tabel di bagian 2. Rekomendasi: pakai, lalu setel lewat playtest.
6. **Resep dan `Mat_Scrap`:** tabel di bagian 2. Rekomendasi: pakai.

## 6. Skenario Tes

1. Dengan `enabled = false`, tekan `G`. **Hasil:** tidak terjadi apa-apa, dan HUD tidak menampilkan barang lempar.
2. Dengan `enabled = true` dan 3 Batu, tahan `G`. **Hasil:** garis perkiraan muncul, dan penanda mendarat di tanah.
3. Lepas `G`. **Hasil:** Batu mendarat dekat penanda, jumlah jadi 2, dan HUD berubah.
4. Tahan `G`, lalu klik kanan. **Hasil:** bidikan batal, dan jumlah tetap.
5. Lempar Batu di dekat 3 zombie yang belum melihat pemain. **Hasil:** ketiganya memeriksa titik suara, lalu kembali normal.
6. Lempar Suar. **Hasil:** zombie berkumpul selama 15 detik. Zombie Lumut dan hewan tidak terpancing.
7. Ledakkan Bom di tengah 10 zombie. **Hasil:** satu kali damage per musuh, dan falloff berlaku. Berdiri di separuh radius. **Hasil:** pemain menerima sekitar 31 persen.
8. Lempar Molotov ke dinding kayu dan zombie. **Hasil:** area api 8 detik dan dinding terbakar. Zombie Lumut menerima 2x, Zombie Militer 0,5x.
9. Lempar Molotov ke air. **Hasil:** padam tanpa area api.
10. Lempar Suar ke kandang Lucky Machine saat ada 5 Zombie Militer. **Hasil:** zombie masuk, dan kuota #72 naik.
11. Lempar 30 barang berurutan sambil membuka Profiler. **Hasil:** tanpa `Instantiate` setelah pool terisi, dan proyektil yang dipakai ulang tidak membawa kecepatan lama.
12. Jeda saat sumbu Bom berjalan. **Hasil:** tidak meledak sampai jeda selesai.
13. Mati, lalu mulai run baru. **Hasil:** tidak ada Suar yang tersisa, dan pilihan kembali ke jenis pertama.
