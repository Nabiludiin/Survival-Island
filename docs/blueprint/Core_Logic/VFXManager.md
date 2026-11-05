# Blueprint: VFXManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/VFXManager.md`
**Deskripsi Utama:** Satu pintu untuk efek visual sekali muncul: cipratan darah, debu kayu dan batu, percikan api gacha, muzzle flash, serta impact peluru. Manager ini mengatur pemanggilan, batas jumlah, dan pengembalian ke pool.
**Issue:** #15
**Rujukan GDD:** Tidak disebut langsung. Mendukung Bagian 3 (combat), Bagian 5 (menebang dan menambang), dan Bagian 7 (Lucky Machine).

## 1. Arsitektur & Variabel Utama

Singleton persisten (`DontDestroyOnLoad`), sesuai masa hidup di `RunLifecycle.md`. Manager memakai `IBootstrapService`, dan efeknya berasal dari `ObjectPooler`.

**Yang tidak diatur di sini:** efek yang menempel pada prefab pemiliknya, seperti `SunlightExposure.burnEffect`, dan `BurnEffect` (`FX_FireBurn`) milik `FireSystem`. Keduanya sudah punya reset sendiri.

```csharp
public class PooledEffect : MonoBehaviour, IPoolable   // Dari GunController, pindah ke sini
{
    public string PoolTag { get; }
    public int Generation { get; }     // Bertambah setiap kembali ke pool
    public void OnSpawned();
    public void OnReturned();
}

public readonly struct VFXHandle       // Pegangan efek loop, tanpa garbage
{
    public readonly PooledEffect effect;
    public readonly int generation;
    public bool IsValid => effect != null && effect.Generation == generation;
}

[Serializable]
public struct VFXEntry
{
    public string poolTag;
    public float lifetimeSeconds;     // 0 berarti loop, dan pemanggil wajib menghentikannya
    public int maxActive;
    public float cullDistance;        // 0 berarti selalu tampil
    public float minIntervalSeconds;
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `entries` | `Dictionary<string, VFXEntry>` | Dari `GameBalanceConfig.vfx.catalog` | Aturan tiap efek |
| `activeCount` | `Dictionary<string, int>` | 0 | Jumlah efek aktif per tag |
| `lastPlayTime` | `Dictionary<string, float>` | -999 | Waktu terakhir per tag |
| `activeEffects` | `List<PooledEffect>` | Kapasitas 128 | Untuk `ReturnAll` |
| `impactBySurface` | `string[]` | Tabel di bawah | Tag menurut `SurfaceType` |
| `densityMultiplier` | `float` | 1 | Dari `SettingsManager.qualityPreset` |

### Katalog efek (`GameBalanceConfig.vfx.catalog`)

| `poolTag` | Isi pool | Umur | `maxActive` | `cullDistance` | `minInterval` |
|---|---|---|---|---|---|
| `VFX_Muzzle`, `VFX_LaserMuzzle` | 8 | 0,5 | 8 | 0 | 0 |
| `VFX_Impact` (Batu, Logam) | 24 | 0,5 | 24 | 60 | 0 |
| `VFX_ImpactDust` (Tanah, Pasir, Lumpur, Rumput, Salju) | 24 | 0,5 | 24 | 60 | 0 |
| `VFX_ImpactWood` (Kayu) | 16 | 0,5 | 16 | 60 | 0 |
| `VFX_LaserImpact` | 8 | 0,5 | 8 | 60 | 0 |
| `VFX_Blood` | 32 | 0,8 | 32 | 40 | 0,02 |
| `VFX_BloodHeavy` (kill atau headshot) | 12 | 1,2 | 12 | 40 | 0 |
| `VFX_MetalSpark` (armor) | 12 | 0,4 | 12 | 40 | 0 |
| `VFX_WoodDust`, `VFX_StoneDust`, `VFX_LeafBurst` | 8 per tag | 0,8 | 8 | 40 | 0,05 |
| `VFX_GachaSpark` | 4 | 2 | 4 | 0 | 0 |
| `VFX_Splash` | 8 | 1 | 8 | 40 | 0,1 |
| `VFX_Explosion` (tong, usulan `EnvironmentalHazard.md`) | 4 | 2 | 4 | 0 | 0 |
| `VFX_ShipPickup` (ambil barang kapal, usulan `ShipInteractable.md`) | 4 | 0,5 | 4 | 0 | 0 |

Setiap efek berbasis `ParticleSystem`, dengan `Stop Action = None`, tanpa collider, dan `Simulation Space = World`. Isi pool didaftarkan di daftar `pools` milik `ObjectPooler`, dan tidak boleh lebih kecil dari `maxActive`. Dengan begitu, antrean tidak pernah membuat objek baru.

Angka lain: `GameBalanceConfig.vfx.lowQualityDensity` 0,5 dan `mediumQualityDensity` 0,75. Keduanya mengecilkan `maxActive` dan laju partikel.

## 2. Detail Logika & Fungsi

**`public VFXHandle Play(string poolTag, Vector3 pos, Quaternion rot, Transform follow = null, bool viewmodel = false)`**:

1. Ambil `VFXEntry`. Kalau tidak ada, catat peringatan sekali, lalu kembalikan handle kosong.
2. Lewati kalau `Time.time - lastPlayTime < minIntervalSeconds`. Horde tidak membuat ratusan efek dalam satu frame.
3. Lewati kalau `activeCount >= Mathf.CeilToInt(maxActive * densityMultiplier)`. Efek baru dibuang, dan efek yang masih hidup tidak dipakai ulang, sesuai aturan `ObjectPooler`.
4. Lewati kalau `cullDistance > 0`, dan jarak ke `Camera.main` lebih besar.
5. `SpawnFromPool(poolTag, pos, rot)`. Kalau `null`, kembalikan handle kosong.
6. Isi `PooledEffect`: umur, `follow`, dan layer. Kalau `viewmodel`, renderer memakai layer `PlayerViewmodel`. Efek NPC tetap di layer dunia, sesuai `GunController`.
7. Catat di `activeEffects`, tambah `activeCount`, perbarui `lastPlayTime`, lalu kembalikan handle.

**`public VFXHandle PlayImpact(in RaycastHit hit, string overrideTag = null)`**:

1. Kalau layer `hit.collider` adalah `EnemyHitbox`, kembalikan handle kosong. Darah sudah datang lewat `EnemyHitEvent`, sesuai `SurfaceDetection.md`.
2. Tag: `overrideTag` kalau senjata punya impact khusus (`VFX_LaserImpact`). Kalau tidak, `impactBySurface[(int)SurfaceDetection.Instance.GetSurfaceFromHit(hit)]`. Fungsi ini tidak menembakkan raycast baru.
3. `Play(tag, hit.point, Quaternion.LookRotation(hit.normal))`.

**`public void Stop(VFXHandle handle)`**: kalau `handle.IsValid`, `ParticleSystem.Stop(true, StopEmitting)`, lalu efek kembali ke pool setelah partikel habis.

**`public void ReturnAll()`**: kembalikan semua `activeEffects`, dan nolkan `activeCount`.

**`private void HandleEnemyHit(EnemyHitEvent e)`**: tag `VFX_BloodHeavy` kalau `e.isKill` atau `e.bodyPart == BodyPart.Head`. Kalau tidak, `VFX_Blood`. Efek menghadap kamera, karena event tidak membawa normal permukaan.

**`private void HandleStructureDestroyed(StructureDestroyedEvent e)`**: `Play(VFX_WoodDust, e.position, ...)`.

### `PooledEffect`

- **`OnSpawned`:** terapkan skala laju partikel dari `densityMultiplier`, lalu `Play(true)`.
- **`Update`:** kurangi umur dengan `Time.deltaTime`. Saat habis, panggil `VFXManager.Instance.Release(this)`.
- **`LateUpdate`:** kalau `follow != null`, salin posisi dan rotasinya. Kalau `follow` sudah tidak aktif atau hilang, `follow = null`, dan efek berhenti memancar.
- **`OnReturned`:** `Stop(true, StopEmittingAndClear)`, `follow = null`, kembalikan layer dan skala prefab, serta `Generation++`.

**Efek tidak pernah di-`SetParent`.** Cara ini mencegah efek ikut hancur bersama scene, atau ikut masuk pool bersama zombie yang ditempeli.

### Siapa memanggil apa

| Efek | Pemicu | Cara |
|---|---|---|
| Cipratan darah | Peluru dan pukulan mengenai musuh | `EnemyHitEvent`, tanpa pemanggilan langsung |
| Debu kayu, batu, dan daun | `ResourceNode.ReceiveHit` di titik kena | `Play`, tag dari `NodeType` |
| Percikan gacha | `LuckyMachine` saat tumbal selesai, dan saat mesin berputar | `Play` |
| Muzzle flash | `GunController` langkah 8 | `Play(tag, pos, rot, muzzlePoint, viewmodel: true)` |
| Impact peluru | `GunController` | `PlayImpact(hit, overrideTag)`. `overrideTag` berisi `impactTag` senjata, atau `null` kalau tag itu `VFX_Impact` |
| Percikan armor | `EnemyHitReaction` | `Play(VFX_MetalSpark, ...)` |
| Percikan air | `FishingSystem`, `SwimmingController` | `Play(VFX_Splash, ...)` |
| Hit perangkap | `BaseTrap` (#26) | `Play(hitVfxPoolTag, ...)` |
| Debu bangunan | `StructureDestroyedEvent`, `RegionGateSystem` | Event dan `Play` |

## 3. Hubungan dengan Sistem Lain

**Event yang didengar:** `EnemyHitEvent`, `StructureDestroyedEvent`, dan `SettingsChangedEvent` (memperbarui `densityMultiplier`). Blueprint ini tidak mengirim event.

| Sistem | Hubungan |
|---|---|
| `ObjectPooler` | `SpawnFromPool` dan `ReturnToPool` |
| `SurfaceDetection` (#86) | `GetSurfaceFromHit` |
| `SettingsManager` (#19) | `qualityPreset` |
| `RunLifecycle` (#82) | `Initialize` dan `ResetForNewRun` |
| `AudioManager` | Suara tetap dipanggil oleh sistem pemicu, bukan dari `VFXManager` |

**Batas pekerjaan:** post-processing di #23, getaran kamera di #24, dan hit marker di #51.

## 4. Kasus Khusus

- **Dipakai ulang dari pool:** `OnReturned` membersihkan partikel, sehingga efek lama tidak terlihat pada posisi baru. `Generation` membuat handle lama tidak valid, sehingga `Stop` tidak menghentikan efek milik pemanggil lain.
- **Efek loop:** efek dengan umur 0 wajib dihentikan pemiliknya, misalnya saat `OnUnequip` atau `OnDisable`. Kalau pemilik lupa, `ReturnAll` tetap membersihkannya saat run berakhir.
- **Game dijeda:** partikel memakai waktu berskala (`useUnscaledTime = false`), dan umur memakai `Time.deltaTime`. Efek berhenti dan lanjut dari posisi yang sama.
- **Pindah scene:** `RunEndedEvent` dan `ResetForNewRun` memanggil `ReturnAll`. Pindah region tidak membersihkan efek, karena umurnya pendek dan `follow` menangani target yang hilang.
- **Jarak dan pool penuh:** efek dibuang tanpa log, karena ini perilaku normal saat horde. Peringatan hanya untuk tag yang tidak dikenal.
- **`Camera.main` kosong:** saat loading, langkah jarak dilewati. Kamera disimpan di cache dan diperbarui saat scene berganti.
- **Permadeath dan save:** tidak ada data yang disimpan. Efek yang sedang aktif tidak dibawa ke save.

## 5. Keputusan Desain

**Keputusan arsitektur di blueprint ini:**

- `poolTag` menjadi pengenal efek, jadi `WeaponData.muzzleFlashTag` dan `impactTag` tetap dipakai.
- Efek baru dibuang saat batas tercapai, tanpa mengambil efek yang masih hidup.
- Efek mengikuti lewat `follow`, tanpa `SetParent`.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Warna darah:** satu warna untuk zombie dan hewan pada versi awal. `EnemyHitEvent` tidak membawa jenis musuh, dan dampak visualnya kecil.
2. **Impact per permukaan:** tiga kelompok (`VFX_Impact`, `VFX_ImpactDust`, `VFX_ImpactWood`), bukan satu per `SurfaceType`. Kelompok ini cukup terbaca dan menghemat aset.

## 6. Skenario Tes

1. Tembak dinding kayu, batu, dan tanah. **Hasil:** impact sesuai permukaan, dan objek kembali ke pool setelah 0,5 detik.
2. Tembak kepala zombie, lalu badan. **Hasil:** kepala memunculkan `VFX_BloodHeavy`, dan badan memunculkan `VFX_Blood`. Tidak ada impact tanah di titik itu.
3. Tebang pohon, tambang batu, dan petik tanaman. **Hasil:** debu kayu, batu, dan daun di titik kena.
4. Jalankan putaran Lucky Machine. **Hasil:** `VFX_GachaSpark` muncul di mesin.
5. Tembak sambil berlari dengan Pistol. **Hasil:** muzzle flash menempel pada laras, tanpa tertinggal.
6. Bunuh 30 zombie dalam 2 detik sambil membuka Profiler. **Hasil:** `activeCount` tidak melewati `maxActive`, dan tidak ada `Instantiate`.
7. Tempelkan efek pada zombie, lalu kembalikan zombie ke pool. **Hasil:** efek berhenti, dan tidak ikut masuk pool zombie.
8. Jeda game saat darah masih terlihat. **Hasil:** partikel berhenti, dan lanjut setelah jeda.
9. Mati saat banyak efek aktif, lalu mulai run baru. **Hasil:** `activeEffects` kosong, dan tidak ada `MissingReferenceException`.
10. Pilih kualitas `Low` di `SettingsManager`. **Hasil:** batas efek turun menjadi separuh.
11. Simpan handle efek loop, biarkan efek kembali, lalu panggil `Stop`. **Hasil:** tidak ada efek lain yang berhenti.
