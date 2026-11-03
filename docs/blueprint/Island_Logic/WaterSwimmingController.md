# Blueprint: SwimmingController.cs
**Lokasi:** `docs/blueprint/Island_Logic/WaterSwimmingController.md`
**Deskripsi Utama:** Mengatur pemain saat masuk laut, sungai, atau danau: mengarungi, berenang, menyelam, kehabisan napas, dan tenggelam.
**Issue:** #13
**Rujukan GDD:** Bagian 3 (HP habis menyebabkan mati) dan bagian 5 (air di Region 4). Mekanik berenang adalah usulan dari issue ini, karena GDD tidak merincinya.

## 1. Arsitektur & Variabel Utama

| Kelas | Bentuk | Letak | Fungsi |
|---|---|---|---|
| `SwimmingController` | `MonoBehaviour` | Pemain Fase 2 | Status air, gerak renang, stamina, dan napas |
| `WaterVolume` | `MonoBehaviour` | Trigger layer `Water` | Tinggi permukaan dan jenis air |

Fase 1 tidak memakai kelas ini. Air di kapal milik `ShipHazardManager` (#75).

```csharp
public enum WaterKind { Sea, River, Lake }
public enum WaterMode { None, Wading, Swimming }

public class WaterVolume : MonoBehaviour
{
    public WaterKind kind;
    public float SurfaceY { get; }   // Laut: TideManager (#91). Lainnya: posisi Y objek
}
```

**Variabel `SwimmingController`:**

| Nama | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Mode` | `WaterMode` | `None` | Keadaan pemain |
| `currentWater` | `WaterVolume` | `null` | Air yang sedang dimasuki |
| `breathSeconds` | `float` | `maxBreathSeconds` | Sisa napas |
| `IsUnderwater` | `bool` | `false` | Mata berada di bawah permukaan |
| `isSinking` | `bool` | `false` | Stamina habis saat berenang |
| `waterCheckTimer` | `float` | 0 | Jeda pemeriksaan |

**Angka penyetelan (`GameBalanceConfig.swimming`, kecuali disebut lain):**

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `vitals.swimStaminaPerSec` | 6 | Sudah ada. Stamina penuh habis dalam 16,7 detik |
| `vitals.drownDamagePerSec` | 10 | Sudah ada |
| `idleStaminaPerSec` | 2 | Mengapung diam |
| `swimSpeedMultiplier` | 0,5 | Pengali `Water`, jadi 2,5 m/s dari `walkSpeed` 5 |
| `wadeSpeedMultiplier` | 0,7 | Air setinggi paha sampai dada |
| `wadeEnterDepth` | 0,5 | Kedalaman mulai mengarungi, dalam meter |
| `swimEnterDepth` | 1,3 | Kedalaman mulai berenang |
| `swimExitDepth` | 1,0 | Kedalaman kembali mengarungi |
| `maxBreathSeconds` | 30 | Napas penuh |
| `breathRecoverPerSec` | 10 | Pemulihan di atas air |
| `sinkingBreathMultiplier` | 2 | Napas habis lebih cepat saat stamina nol |
| `sinkSpeed` | 0,5 | Kecepatan turun saat stamina nol |
| `verticalSwimSpeed` | 1,5 | Naik dan menyelam |
| `surfaceEyeOffset` | 0,3 | Tinggi mata di atas permukaan |
| `waterCheckSeconds` | 0,1 | Jeda pemeriksaan kedalaman |
| `landingProbeDistance` | 30 | Jarak cek air saat jatuh |

Dengan angka ini, stamina penuh hanya cukup untuk sekitar 42 meter. Sungai dan danau bisa diseberangi, tetapi laut berbahaya.

## 2. Detail Logika & Fungsi

### Mendeteksi air

`Player` bertabrakan dengan layer `Water` (`PhysicsLayers.md`), sehingga `OnTriggerEnter` dan `OnTriggerExit` memberikan `currentWater`.

**Pengecualian query trigger (#87):** air memang berupa trigger, jadi dua pemeriksaan di bawah sengaja memakai `QueryTriggerInteraction.Collide`, lewat `PhysicsQuery.RaycastWater` dan `PhysicsQuery.OverlapWater`.

**`private void UpdateMode()`**, setiap `waterCheckSeconds`:

1. Kalau `currentWater == null`, atau objeknya sudah nonaktif, ubah ke `None`.
2. `depth = currentWater.SurfaceY - posisiKaki.y`.
3. `depth >= swimEnterDepth` berarti `Swimming`.
4. `Mode == Swimming` dan `depth > swimExitDepth` berarti tetap `Swimming`. Selisih dua angka ini mencegah status berganti-ganti.
5. `depth >= wadeEnterDepth` berarti `Wading`. Lainnya `None`.

### Mengubah mode

**`private void SetMode(WaterMode next)`:**

| Perubahan | Langkah |
|---|---|
| Ke `Wading` | `SetSpeedModifier(Water, wadeSpeedMultiplier)`. Jalan biasa, dan lari serta lompat tetap mengikuti `SurvivalStats` |
| Ke `Swimming` | `PlayerTraversal.SetMode(Swimming)`, `SetSpeedModifier(Water, swimSpeedMultiplier)`, `EquipmentManager.PushLock(EquipLock.Swimming)`, dan percikan lewat `VFXManager` (#15) |
| Keluar dari `Swimming` | `PlayerTraversal.ClearMode(Swimming)`, `PopLock(EquipLock.Swimming)`, `isSinking = false`, dan `SetDrowning(false)` |
| Ke `None` | `ClearSpeedModifier(Water)` |

Setiap perubahan menerbitkan `SwimStateChangedEvent { isSwimming, isUnderwater, breathRatio }`.

### Gerak renang

Saat `Swimming`, `IslandPlayerController` tidak memakai gravitasi, dan `SwimmingController` memanggil `CharacterController.Move`.

**`private void TickSwim(float dt)`:**

1. `speed = walkSpeed * SpeedMultiplier`. Tidak ada lari saat berenang.
2. Arah mendatar diambil dari input `Move` dan arah kamera.
3. **Vertikal:**
   - `isSinking`: `y = -sinkSpeed`. Input naik diabaikan.
   - Tombol `Crouch` ditahan: `y = -verticalSwimSpeed`, yaitu menyelam.
   - Lainnya: `y` menuju `SurfaceY + surfaceEyeOffset - tinggiMata`, dibatasi `verticalSwimSpeed`. Pemain naik sendiri, dan mengapung di permukaan.
4. Panggil `CharacterController.Move`.
5. Jalankan `TickStamina(dt)` dan `TickBreath(dt)`.

**Keluar dari air:** pantai harus landai, sehingga kedalaman turun di bawah `swimExitDepth` saat pemain berjalan ke darat. Tebing tidak bisa dipanjat dari air, kecuali ada `ClimbableSurface` (#101).

### Stamina

**`private void TickStamina(float dt)`:**

1. `cost = (ada input Move ? vitals.swimStaminaPerSec : idleStaminaPerSec) * dt`.
2. `SurvivalStats.SpendStamina(cost, StaminaUse.Swim)`. Fungsi ini juga mengisi `isExerting` untuk haus (#43), dan menahan regenerasi stamina selama berenang.
3. Kalau `Stamina <= 0`, `isSinking = true`, dan kirim notifikasi `Kehabisan tenaga` (#22).

Saat `isSinking`, pemain masih bisa bergerak mendatar, dengan `speed` yang sama. Dasar yang dangkal atau pantai yang dekat masih bisa menyelamatkan pemain. `isSinking` hanya hilang setelah keluar dari mode `Swimming`.

### Napas dan tenggelam

**`private void TickBreath(float dt)`:**

1. `IsUnderwater = posisiMata.y < currentWater.SurfaceY`.
2. Kalau `IsUnderwater`: `breathSeconds -= dt * (isSinking ? sinkingBreathMultiplier : 1)`.
3. Kalau tidak: `breathSeconds = Min(maxBreathSeconds, breathSeconds + breathRecoverPerSec * dt)`.
4. `SurvivalStats.SetDrowning(IsUnderwater && breathSeconds <= 0)`.
5. Terbitkan `SwimStateChangedEvent` 4 kali per detik selama `breathRatio < 1`, dan saat `IsUnderwater` berubah.

`SurvivalStats` menghitung damage `Drown` 10 per detik. Alasan kematian adalah `Drowned`, sesuai `VitalsRules.md`.

**Waktu sampai mati dari stamina penuh saat berenang:** 16,7 detik sampai stamina habis, sekitar 2 detik sampai mata tenggelam, 15 detik napas, lalu 10 detik HP. Totalnya sekitar 44 detik dan 110 meter gerak.

### Jatuh ke air

`PlayerTraversal.OnLanded` tidak memberi damage kalau `isOverWater` atau #13 aktif. **`private void UpdateLandingProbe()`**, saat pemain tidak `isGrounded`:

1. `PhysicsQuery.RaycastWater(posisi, Vector3.down, landingProbeDistance, out hit)`.
2. Baca tanah di bawah titik air lewat `PhysicsQuery.Raycast` dengan `PhysicsMasks.Ground`.
3. `PlayerTraversal.SetOverWater(kedalaman >= swimEnterDepth)`.

Air dangkal tidak menyelamatkan pemain dari damage jatuh.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `SwimStateChangedEvent` (baru, sticky).

**Event yang didengar:** `PlayerPinnedEvent` (berhenti berenang saat `Locked`), `RunEndedEvent`.

| Sistem | Hubungan |
|---|---|
| `SurvivalStats` (#111) | `SpendStamina(Swim)` dan `SetDrowning` |
| `PlayerTraversal` (#101) | Mode `Swimming`, `SpeedModifierId.Water`, dan `SetOverWater` |
| `EquipmentManager` (#60) | `EquipLock.Swimming`. Item disimpan saat berenang |
| `HeavyItemSystem` (#69) | Melepas barang saat `isSwimming` |
| `WarmthSystem` (#65) | Air menjadi sumber dingin, dan obor padam |
| `TideManager` (#91) | Tinggi air laut |
| `RegionGateSystem` (#68) | `GateBarrier` membatasi renang di sekitar seam |
| `PostProcessController` (#23), `AmbientAudio` (#52), `FirstPersonCamera` (#102) | Blur, suara bawah air, dan tinggi mata |
| `HUDManager` | Bar napas dari `breathRatio` |
| `SunlightExposure` (#63), `EnemyAI` | NavMesh tidak melewati air dalam, sehingga musuh menunggu di tepi |

## 4. Kasus Khusus

- **Objek pool:** pemain bukan objek pool. Percikan dan gelembung memakai `VFXManager`, dan kembali setelah selesai.
- **Game dijeda:** `Time.deltaTime` berhenti, jadi stamina, napas, dan gerak ikut berhenti. Input terkunci (#83).
- **Save:** tidak ada data milik kelas ini. Posisi pemain disimpan `SaveSystem`. `isDrowning` tidak disimpan (`VitalsRules.md`), dan autosave ditahan saat tenggelam (`WorldStateSave.md`).
- **Load di dalam air:** `OnTriggerEnter` belum tentu berjalan. `RestoreState` dan `Start` memanggil `PhysicsQuery.OverlapWater(posisi, 0,5, buffer)` untuk mengisi `currentWater`, dan `breathSeconds` dimulai penuh.
- **Permadeath:** tidak ada data yang tersisa. Run baru mulai dengan `Mode = None`.
- **Scene region dilepas saat di air:** `OnTriggerExit` tidak terpanggil untuk objek yang dihapus. Langkah 1 di `UpdateMode` menangani `currentWater` yang nonaktif. Laut berada di `IslandPhase`, jadi tidak terpengaruh.
- **Air pasang (#91):** `SurfaceY` berubah, dan pemain yang berdiri di pantai bisa berubah mode tanpa bergerak. Ini sudah benar, karena kedalaman dihitung dari `SurfaceY`.
- **Diterkam saat berenang:** mode `Locked` lebih tinggi dari `Swimming`, jadi `TickSwim` berhenti. Napas tetap berjalan.
- **Pemain di air saat zombie mengejar:** musuh tidak bisa mencapai pemain, tetapi stamina terus berkurang, sehingga air bukan tempat aman selamanya. Musuh memakai `TargetUnreachable` atau aturan #94.
- **Membawa Heavy Item:** `Wading` tetap boleh. Masuk `Swimming` melepaskan barang (#69).
- **Dua `WaterVolume` tumpang tindih:** `currentWater` memakai volume dengan `SurfaceY` tertinggi, misalnya muara sungai dan laut.
- **Tidur:** tidak bisa tidur di air, karena `Interact` pada kasur ditolak oleh jarak.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- Biaya `swimStaminaPerSec` 6, dan stamina habis menjadi kondisi tenggelam (`VitalsRules.md`).
- `drownDamagePerSec` 10 setelah oksigen habis (`VitalsRules.md`).
- Item terkunci saat berenang (`EquipmentManager.md`).

**Usulan yang menunggu developer (dicatat di #85):**

1. **Menyelam:** rekomendasi tetap ada, dengan napas 30 detik. `VitalsRules.md` dan #23 sudah mengacu pada oksigen dan menyelam. Pada versi awal menyelam belum memberi hadiah, dan bisa ditambah lewat #106.
2. **Kecepatan berenang:** rekomendasi 2,5 m/s, sama dengan membawa Heavy Item.
3. **Regenerasi saat berenang:** rekomendasi tidak ada, termasuk saat mengapung. Tanpa ini, pemain bisa mengapung tanpa batas dan aman dari horde.
4. **Musuh di air:** rekomendasi tidak bisa berenang. Hanya Zombie Desa yang masuk air untuk bersembunyi (#63).
5. **Air laut sebagai sumber bahaya lain:** rekomendasi tidak ada ikan atau hewan laut pada versi awal, karena tidak ada di GDD.

## 6. Skenario Tes

1. Berjalan pelan dari pantai ke laut. **Hasil:** kecepatan 3,5 m/s pada kedalaman 0,5 meter, lalu 2,5 m/s dan mode `Swimming` pada 1,3 meter.
2. Berenang tanpa berhenti. **Hasil:** stamina habis sekitar 16,7 detik, dan notifikasi muncul.
3. Tetap di laut dalam. **Hasil:** pemain tenggelam, napas habis 15 detik kemudian, HP turun 10 per detik, lalu mati dengan alasan `Drowned`.
4. Saat tenggelam, berenang ke pantai. **Hasil:** `isSinking` hilang saat kedalaman di bawah 1 meter, dan stamina mulai pulih.
5. Mengapung diam. **Hasil:** stamina turun 2 per detik.
6. Tahan `Crouch`. **Hasil:** menyelam, efek bawah air aktif, dan bar napas tampil. Lepas. **Hasil:** naik sendiri, dan napas pulih 10 per detik.
7. Pegang Pistol lalu masuk air dalam. **Hasil:** Pistol disimpan, dan kembali setelah keluar.
8. Bawa suku cadang ke air setinggi paha, lalu ke air dalam. **Hasil:** tetap dibawa di air dangkal, dan barang kembali ke darat saat berenang.
9. Jatuh dari tebing 15 meter ke air dalam. **Hasil:** tanpa damage. Ulangi ke air 0,5 meter. **Hasil:** damage jatuh normal.
10. Masuk air di Region 3 tanpa pelindung. **Hasil:** `Warmth` turun cepat, dan pemain membeku (#65).
11. Lepas `Region4_Village` saat pemain di sungai lewat `DevConsole`. **Hasil:** mode kembali `None`, tanpa error.
12. Simpan di darat, lalu muat di dalam air lewat `teleport`. **Hasil:** mode `Swimming` dalam 0,1 detik.
13. Jeda saat tenggelam. **Hasil:** HP dan napas berhenti, lalu berlanjut setelah jeda.
