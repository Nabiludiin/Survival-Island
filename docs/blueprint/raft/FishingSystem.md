# Blueprint: FishingSystem.cs
**Lokasi:** `docs/blueprint/raft/FishingSystem.md`
**Deskripsi Utama:** Perilaku klik kiri `Base_Kail`: melempar kail ke air, menunggu ikan menggigit secara acak, lalu menarik ikan lewat Tension Bar tanpa memutus benang.
**Issue:** #55
**Rujukan GDD:** Bagian 3 (Lapar diisi dengan memancing memakai Kail) dan bagian 4 (Diary Bab 1: memakai kail di tebing karang). Tension Bar dan semua angka adalah usulan.

## 1. Arsitektur & Variabel Utama

`FishingSystem : EquippableBehaviour` berada di prefab viewmodel `Base_Kail`, sesuai tabel item `EquipmentManager.md`. Klik kiri lempar dan tarik, klik kanan batal. Tidak ada manager global, karena hanya ada satu Kail.

```csharp
public enum FishingState { Idle, Charging, LureFlying, Waiting, BiteWindow, Reeling, Intercepted, Recovering }
public enum FishingEndReason { Caught, FishEscaped, LineBroken, Cancelled, Interrupted, WaterGone, TooFar, InterceptedByTarget }

public interface IKailCastInterceptor   // Diisi #106 kalau disetujui. Tanpa ini, #55 tetap utuh
{
    bool IsTargetAt(Vector3 landing);                        // Untuk teks penanda
    bool TryIntercept(Vector3 landing, FishingSystem kail);  // true berarti kail mengait target lain
    void TickIntercept(bool isHeld, float dt);               // Tiap frame selama Intercepted
    void CancelIntercept();                                  // Saat sesi dibatalkan dari sisi Kail
}
```

`WaterBody : MonoBehaviour` dipasang pada setiap collider di layer `Water`. Isinya `kind` (`Sea`, `River`, atau `Lake`) dan `biteDelayMultiplier` (nilai awal 1). Tebing karang memakai collider `Water` sendiri dengan nilai 0,75.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `State` | `FishingState` | `Idle` | Dibaca `FishingTensionHUD` |
| `Tension` | `float` | 0 | 0 sampai 1. Mencapai 1 berarti benang putus |
| `CatchProgress` | `float` | 0 | 0 sampai 1. Mencapai 1 berarti ikan tertangkap |
| `chargeTimer` | `float` | 0 | Lama tombol ditahan sebelum melempar |
| `lureLanding` | `Vector3` | Nol | Titik jatuh kail |
| `biteTimer`, `stateTimer` | `float` | 0 | Sisa waktu tunggu dan waktu state |
| `surgeTimer`, `surgeLeft` | `float` | 0 | Jeda hentakan ikan dan sisa hentakan |
| `slackTimer` | `float` | 0 | Lama benang kendur |
| `holdThisFrame` | `bool` | `false` | Diisi `PrimaryAction(Held)`, dinolkan tiap frame |
| `waterRecheckTimer` | `float` | 0 | Jeda cek ulang air di bawah kail |
| `interceptor` | `IKailCastInterceptor` | `null` | Terisi lewat `RegisterInterceptor()` |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.fishing.castMinDistance` | 6 | Jarak lempar tanpa mengisi tenaga, meter |
| `GameBalanceConfig.fishing.castMaxDistance` | 25 | Jarak lempar terjauh, meter |
| `GameBalanceConfig.fishing.castChargeSeconds` | 1,2 | Waktu mengisi tenaga penuh |
| `GameBalanceConfig.fishing.castProbeDepth` | 40 | Panjang raycast ke bawah, cukup untuk tebing |
| `GameBalanceConfig.fishing.lureFlightSeconds` | 0,8 | Lama kail di udara |
| `GameBalanceConfig.fishing.leashDistance` | 32 | Pemain menjauh lebih dari ini, kail ditarik otomatis |
| `GameBalanceConfig.fishing.biteDelayMin` / `biteDelayMax` | 12 / 30 | Waktu tunggu acak, detik |
| `GameBalanceConfig.fishing.rainBiteMultiplier` | 0,85 | Pengali waktu tunggu saat `Rain` |
| `GameBalanceConfig.fishing.biteWindowSeconds` | 2 | Batas waktu menanggapi gigitan |
| `GameBalanceConfig.fishing.tensionStart` | 0,35 | Tegangan saat mulai menarik |
| `GameBalanceConfig.fishing.tensionRisePerSec` | 0,30 | Kenaikan saat tombol ditahan |
| `GameBalanceConfig.fishing.tensionFallPerSec` | 0,35 | Penurunan saat tombol dilepas |
| `GameBalanceConfig.fishing.surgeIntervalMin` / `surgeIntervalMax` | 1,5 / 3,5 | Jeda hentakan ikan, detik |
| `GameBalanceConfig.fishing.surgeSeconds` / `surgeRisePerSec` | 0,8 / 0,35 | Lama dan kekuatan hentakan |
| `GameBalanceConfig.fishing.progressPerSec` | 0,10 | Kemajuan saat tombol ditahan |
| `GameBalanceConfig.fishing.slackEscapeSeconds` | 2 | Tegangan 0 selama ini, ikan lepas |
| `GameBalanceConfig.fishing.recoverSeconds` | 1 | Jeda setelah berhasil, gagal, atau batal |
| `GameBalanceConfig.fishing.lineBreakRecoverSeconds` | 5 | Waktu mengikat ulang benang |
| `GameBalanceConfig.fishing.waterRecheckSeconds` | 1 | Jeda cek ulang air |

**Cek target `GameBalanceConfig.md` (1 porsi sekitar 60 detik):** lempar 2 detik + tunggu rata-rata 21 detik + reaksi 1 detik + menarik sekitar 19 detik + jeda 1 detik = sekitar 44 detik per percobaan. Dengan sebagian percobaan gagal, rata-ratanya mendekati 60 detik. Tombol ditahan sekitar 54 persen waktu (`0,35 / (0,30 + 0,35)`), sehingga kemajuan 0,054 per detik.

## 2. Detail Logika & Fungsi

### Input

| State | `PrimaryAction` | `SecondaryAction` (`Pressed`) |
|---|---|---|
| `Idle` | `Pressed` masuk `Charging` | Tidak ada |
| `Charging` | `Held` menambah `chargeTimer`, `Released` memanggil `Cast()` | `Cancel(Cancelled)` |
| `LureFlying` | Tidak ada | `Cancel(Cancelled)` |
| `Waiting` | `Pressed` menarik kail kosong, `Cancel(Cancelled)` | `Cancel(Cancelled)` |
| `BiteWindow` | `Pressed` masuk `Reeling` | `Cancel(Cancelled)` |
| `Reeling` | `Held` mengisi `holdThisFrame` | `Cancel(Cancelled)` |
| `Intercepted` | `Held` mengisi `holdThisFrame` | `Cancel(Cancelled)` |
| `Recovering` | Tidak ada | Tidak ada |

`EquipmentManager` hanya mengirim input saat tidak ada kunci. Karena `holdThisFrame` dinolkan tiap frame, panel tanpa jeda membuat tombol terbaca dilepas, tanpa kode khusus.

### Casting

Selama `Charging`, panggil `ComputeLanding()` tiap frame untuk penanda titik jatuh. Warna dan teksnya: `Pancing` kalau valid, `Tidak ada air` kalau tidak, dan `Tarik` kalau `interceptor.IsTargetAt` bernilai `true`.

**`private bool ComputeLanding(out Vector3 point, out WaterBody water)`**:

1. `float t = chargeTimer / castChargeSeconds`, dibatasi 0 sampai 1. `dist = Lerp(castMinDistance, castMaxDistance, t)`.
2. Ambil arah kamera yang diratakan ke bidang XZ, lalu `start = camera.position + flatForward * dist + Vector3.up * 2`.
3. `PhysicsQuery.RaycastIncludingTriggers(new Ray(start, Vector3.down), out hit, castProbeDepth, PhysicsMasks.CastLanding)`. **Alasan pengecualian trigger:** collider `Water` adalah trigger, sesuai `PhysicsLayers.md`. Mask ini tidak memuat `ZoneTrigger`, jadi zona raksasa tidak ikut terkena.
4. Hasil terdekat harus berlayer `Water` dan memiliki `WaterBody`. Kalau yang terdekat `Terrain`, `Structure`, atau `Obstacle`, hasilnya tidak valid.
5. `PhysicsQuery.Raycast` dari kamera ke titik itu dengan `PhysicsMasks.InteractBlock`. Kalau terhalang, hasilnya tidak valid.

Menembak ke bawah dari titik jatuh membuat pemain cukup mengatur arah dan tenaga, juga saat memancing dari tebing.

**`private void Cast()`**:

1. Kalau `ComputeLanding` gagal, tampilkan `Tidak ada air` lewat `NotificationManager` (#22), lalu kembali ke `Idle` tanpa jeda.
2. Simpan `lureLanding`, dan masuk `LureFlying`. Kail bergerak parabola dengan `Transform` selama `lureFlightSeconds`, bukan Rigidbody.
3. Saat mendarat, minta percikan ke `VFXManager` (#15). Kalau `interceptor?.TryIntercept(lureLanding, this)` bernilai `true`, state menjadi `Intercepted`. Setiap frame, `FishingSystem` memanggil `interceptor.TickIntercept(holdThisFrame, dt)`. Sistem itu mengakhiri sesi lewat `public void EndIntercept()`, yang memanggil `End(InterceptedByTarget)`. Kalau sesi dibatalkan dari sisi Kail, `Cancel` lebih dulu memanggil `interceptor.CancelIntercept()`.
4. Kalau tidak, panggil `RollBiteDelay()` dan masuk `Waiting`.

### RNG waktu gigit

**`private void RollBiteDelay()`**:

```csharp
ref var rng = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay);
float delay = rng.Range(b.biteDelayMin, b.biteDelayMax) * water.biteDelayMultiplier;
if (RegionalWeather.Instance.GetWeather(lureRegion) == WeatherType.Rain) delay *= b.rainBiteMultiplier;
biteTimer = delay;
```

`lureRegion` dibaca dari `RegionStreaming.GetRegionAt(lureLanding)`.

**`Waiting`:** kurangi `biteTimer` dengan `Time.deltaTime`. Setiap `waterRecheckSeconds`, ulangi raycast ke bawah dari atas kail. Kalau air hilang karena surut (#91), panggil `Cancel(WaterGone)`. Saat `biteTimer <= 0`, pelampung tenggelam, suara berbunyi, dan state menjadi `BiteWindow`.

**`BiteWindow`:** kalau `biteWindowSeconds` habis tanpa klik, ikan pergi. Tampilkan `Ikan lolos`, panggil lagi `RollBiteDelay()`, dan kembali ke `Waiting` tanpa melempar ulang.

### Tension Bar

**`private void TickReeling(float dt)`**, setiap frame saat `Reeling`:

```csharp
float rise = holdThisFrame ? b.tensionRisePerSec : -b.tensionFallPerSec;
if (surgeLeft > 0f) { rise += b.surgeRisePerSec; surgeLeft -= dt; }
else if ((surgeTimer -= dt) <= 0f) { surgeLeft = b.surgeSeconds; surgeTimer = NextSurgeInterval(); }

Tension = Mathf.Clamp01(Tension + rise * dt);
if (holdThisFrame) CatchProgress += b.progressPerSec * dt;
slackTimer = Tension <= 0f ? slackTimer + dt : 0f;

if (Tension >= 1f) End(LineBroken);
else if (CatchProgress >= 1f) End(Caught);
else if (slackTimer >= b.slackEscapeSeconds) End(FishEscaped);
holdThisFrame = false;
```

- `NextSurgeInterval()` memakai aliran `Gameplay`.
- Masuk `Reeling`: `Tension = tensionStart`, `CatchProgress = 0`, `slackTimer = 0`, dan jeda hentakan pertama diacak.
- Kail tidak mengubah Stamina pemain.

**`FishingTensionHUD`:** panel di HUD yang tampil saat `FishingStateChangedEvent.state == Reeling`. Selama tampil, panel membaca `Tension` dan `CatchProgress`. Bar berisi zona hijau di bawah 0,7, kuning sampai 0,9, dan merah di atasnya. Panel hanya membaca, sesuai `GameEventBus.md`.

### Mengakhiri sesi

**`private void End(FishingEndReason reason)`**:

| Alasan | Akibat | Jeda |
|---|---|---|
| `Caught` | `int sisa = inventory.AddItem(rawFish, 1)`. Sisa dijatuhkan `WorldItemManager` sebagai `WorldItem` di kaki pemain. Terbitkan `FishCaughtEvent` | `recoverSeconds` |
| `LineBroken` | Ikan hilang, suara putus, dan notifikasi `Benang putus`. Kail tidak hilang | `lineBreakRecoverSeconds` |
| `FishEscaped`, `Cancelled`, `Interrupted`, `WaterGone`, `TooFar` | Kail kembali tanpa hasil | `recoverSeconds` |
| `InterceptedByTarget` | Kail kembali setelah sistem penerima selesai | `recoverSeconds` |

Setelah jeda, state kembali `Idle`. Semua perubahan state menerbitkan `FishingStateChangedEvent`.

**`LateUpdate()`:** saat kail di air, gambar benang dengan `LineRenderer`. Kalau jarak pemain ke kail melebihi `leashDistance`, panggil `Cancel(TooFar)`.

**`public override void OnUnequip()`:** selain `Idle`, langsung akhiri dengan `Cancelled`, sesuai tabel pembatalan `EquipmentManager.md`. `CanSwitchNow` selalu `true`.

## 3. Hubungan dengan Sistem Lain

**Event dikirim:** `FishingStateChangedEvent`, `FishCaughtEvent`.
**Event didengar:** `PlayerDamagedEvent`. Selama kail di air, panggil `Cancel(Interrupted)` kalau jenis damage-nya `Melee`, `Bullet`, `Fire`, atau `Explosion`. Damage berkala seperti `Starvation`, `Dehydration`, `Infection`, `Frost`, dan `Poison` tidak membatalkan, supaya pemain yang kelaparan tetap bisa memancing.

| Sistem | Hubungan |
|---|---|
| `EquipmentManager` (#60) | Mengirim input dan `OnEquip`/`OnUnequip`. Kunci `HeavyItem`, `Climbing`, dan `Swimming` membatalkan lewat `OnUnequip` |
| `PhysicsQuery`, `PhysicsMasks` (#87) | Raycast air dan garis pandang |
| `RunSeedManager` (#89) | Aliran `Gameplay` |
| `RegionalWeather` (#115), `RegionStreaming` (#88) | Cuaca di titik kail |
| `InventoryManager`, `WorldItemManager` (#66) | Memberi `Food_RawFish` |
| `CampfireManager` (#44) | Memasak `Food_RawFish` lewat resep `cook_fish` |
| `DiseaseManager` (#45) | `Food_RawFish` memakai `poisonChance` 0,5 |
| `DebrisRetrieval` (#106) | Mendaftar sebagai `IKailCastInterceptor` kalau disetujui |
| `TideManager` (#91) | Tidak dipanggil. Air yang bergerak terbaca lewat cek ulang |
| `VFXManager` (#15), `NotificationManager` (#22) | Percikan dan pesan |

## 4. Kasus Khusus

- **Objek pool:** kail adalah satu objek milik viewmodel dan tidak dipool. Efek percikan memakai `ObjectPooler` lewat `VFXManager`.
- **Viewmodel dipakai ulang:** `OnEquip` mengatur semua timer, `Tension`, dan `CatchProgress` ke nilai awal, serta menyembunyikan kail dan benang.
- **Game dijeda:** semua timer memakai `Time.deltaTime`, jadi tidak maju. Input juga terkunci (#83).
- **Panel tanpa jeda:** tombol terbaca dilepas, jadi tegangan turun, dan ikan bisa lepas.
- **Tidur:** `SleepSystem` membuka panel, sehingga sesi berjalan seperti panel tanpa jeda. Waktu memancing memakai detik nyata, bukan jam game.
- **Inventori penuh:** ikan tetap didapat sebagai `WorldItem`.
- **Air di region yang dilepas:** kalau `WaterBody` nonaktif, cek ulang air gagal, dan sesi berakhir `WaterGone`.
- **Mati:** kunci `Dead` memanggil `OnUnequip`. Tidak ada hasil.
- **Save dan load:** tidak ada `ISaveable`. State sesi tidak disimpan, jadi setelah load kail berada di `Idle`. Posisi RNG tersimpan di `RunSeedManager`.
- **Permadeath:** tidak ada data yang bertahan. `Food_RawFish` hilang bersama inventori.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `Base_Kail` adalah item inti dan tidak bisa dibuang (`GLOSSARY.md`). Klik kiri lempar dan tarik, klik kanan batal (`EquipmentManager.md`). Aliran RNG `Gameplay` (`RunSeedManager.md`). Target 1 porsi sekitar 60 detik (`GameBalanceConfig.md`).

**Usulan yang menunggu developer:**

1. **Lokasi memancing:** boleh di semua `WaterBody` (laut, sungai, danau). Tebing karang lebih cepat dengan pengali 0,75, sesuai petunjuk Diary Bab 1.
2. **Akibat benang putus:** ikan hilang dan jeda 5 detik. Kail tidak rusak dan tidak butuh bahan, karena item inti tidak boleh hilang.
3. **Hasil tangkapan:** hanya `Food_RawFish` pada versi awal, tanpa jenis ikan dan sampah.
4. **Serangan saat memancing:** serangan langsung membatalkan sesi, supaya memancing tetap berisiko. Damage berkala tidak membatalkan.
5. **Nilai `Food_RawFish`:** Hunger +12, busuk dalam 12 jam game, mengikuti daging mentah.

## 6. Skenario Tes

1. Pegang Kail, arahkan ke laut, tahan lalu lepas klik kiri. **Hasil:** kail mendarat di air dan state menjadi `Waiting`.
2. Arahkan ke tanah. **Hasil:** penanda merah, muncul `Tidak ada air`, dan tidak ada jeda.
3. Berdiri di tepi tebing, lempar ke laut di bawah. **Hasil:** titik jatuh valid.
4. Masuk zona region dan lempar ke air di dalamnya. **Hasil:** raycast tidak berhenti di `ZoneTrigger`.
5. Catat 20 waktu gigit pada `WaterBody` biasa. **Hasil:** semuanya 12 sampai 30 detik. Dengan seed dan urutan aksi yang sama, nilainya sama.
6. Saat `BiteWindow`, jangan klik. **Hasil:** setelah 2 detik muncul `Ikan lolos`, dan menunggu lagi.
7. Tahan terus saat `Reeling`. **Hasil:** benang putus sekitar 2 detik, jeda 5 detik, dan `Base_Kail` tetap ada.
8. Lepas terus saat `Reeling`. **Hasil:** tegangan 0, lalu ikan lepas 2 detik kemudian.
9. Atur tahan dan lepas sampai selesai. **Hasil:** tertangkap dalam sekitar 20 detik, dan 1 `Food_RawFish` masuk.
10. Tangkap ikan dengan inventori penuh. **Hasil:** `WorldItem` muncul di kaki pemain.
11. Tekan klik kanan saat `Waiting`, lalu ganti slot saat `Reeling`. **Hasil:** keduanya batal tanpa hasil.
12. Jeda game saat `Reeling`. **Hasil:** `Tension` dan `CatchProgress` tidak berubah.
13. Berjalan menjauh melewati 32 meter. **Hasil:** kail kembali dengan alasan `TooFar`.
14. Terima serangan `Melee` saat `Waiting`. **Hasil:** sesi batal. Biarkan `Hunger` 0 saat `Reeling`. **Hasil:** sesi tetap berjalan.
15. Paksa `Rain`, ulangi tes 5. **Hasil:** waktu gigit menjadi 10,2 sampai 25,5 detik.
16. Save saat `Waiting`, lalu load. **Hasil:** kail di tangan dengan state `Idle`.
17. Ukur 10 sesi di lembar playtest. **Hasil:** rata-rata 1 porsi sekitar 60 detik.
