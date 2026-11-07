# Blueprint: CameraShake.cs (Cinemachine Impulse)
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/CameraShake.md`
**Deskripsi Utama:** Satu pintu untuk getaran kamera saat menembak, terkena ledakan, digigit zombie, dan bencana. Getaran dibuat oleh Cinemachine Impulse, lalu diserahkan ke `FirstPersonCamera` sebagai offset.
**Issue:** #24
**Rujukan GDD:** Mendukung bagian 3 (kapal dan combat), bagian 5 (erupsi), dan bagian 6 (gempa). Kekuatan getaran adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`CameraShake : MonoBehaviour, ICameraOffsetSource` berada di prefab rig kamera (#102), jadi ada di `ShipPhase` dan `IslandPhase`, dan hilang bersama scene.

**Hubungan dengan Cinemachine:** rig tidak memakai Virtual Camera atau `CinemachineBrain`, karena mouse look dan penggabungan rotasi milik #102. `CameraShake` memakai dua bagian Cinemachine saja:

1. `CinemachineImpulseSource` untuk membuat impuls.
2. `CinemachineImpulseManager.Instance.GetImpulseAt(...)` untuk membaca hasilnya, tiap frame.

Hasilnya diberikan lewat `EulerOffset` dan `PositionOffset`, sesuai `FirstPersonCamera.md`.

```csharp
public enum CameraShakeKind : byte
{
    GunFire, PlayerHit, ZombieBite, Explosion, FireballImpact,
    Earthquake, Pinned, ShipRumble, ShipCollapse, ShipFinalShudder
}

public enum ShakeChannel { Aim = 1, Visual = 2 }  // Nilai bitmask channel Impulse

[Serializable]
public struct CameraShakePreset
{
    public CameraShakeKind kind;
    public CinemachineImpulseDefinition.ImpulseShapes shape;
    public float duration;
    public Vector3 direction;       // Arah dasar, kalau pemanggil tidak memberi arah
    public float force;
    public float cooldownSeconds;
    public float falloffRadius;     // 0 berarti tanpa jarak
    public ShakeChannel channel;
}

[CreateAssetMenu(menuName = "Survival/Camera Shake Profile")]
public class CameraShakeProfile : ScriptableObject
{
    public CameraShakePreset[] presets;  // Satu baris per kind
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `CameraShake` | `null` | Singleton per scene, dipakai fungsi statis |
| `profile` | `CameraShakeProfile` | Diisi di prefab | Preset |
| `sources` | `CinemachineImpulseSource[]` | Satu per `CameraShakeKind`, dibuat di prefab | Sumber impuls |
| `lookup` | `CameraShakePreset[]` | Dibuat di `Awake` | Indeks `(int)kind` |
| `nextAllowedTime` | `float[]` | 0 | Cooldown per jenis |
| `EulerOffset`, `PositionOffset` | `Vector3` | Nol | Dibaca `FirstPersonCamera` |
| `isDead` | `bool` | `false` | Menolak getaran baru setelah pemain mati |

### Preset awal

Nilai berikut berada di aset `CameraShakeProfile`, sehingga bisa disetel tanpa mengubah kode. `force` adalah besar sinyal Impulse sebelum dikali `degreesPerUnit`.

| `CameraShakeKind` | Dipicu oleh | `shape` | `duration` | `force` | `cooldownSeconds` | `falloffRadius` | `channel` |
|---|---|---|---|---|---|---|---|
| `GunFire` | `WeaponFiredEvent` | `Recoil` | 0,15 | 0,4 | 0 | 0 | `Visual` |
| `PlayerHit` | `PlayerDamagedEvent` | `Bump` | 0,2 | 0,5 | 0,15 | 0 | `Aim` |
| `ZombieBite` | `PlayerDamagedEvent` | `Bump` | 0,35 | 1 | 0,25 | 0 | `Aim` |
| `Explosion` | `ThrowableSystem` | `Explosion` | 0,8 | 2,5 | 0,1 | 25 | `Aim` |
| `FireballImpact` | `DisasterManager` | `Explosion` | 0,7 | 2 | 0,1 | 40 | `Aim` |
| `Earthquake` | `DisasterManager` | `Rumble` | 3 | 1,2 | 0 | 0 | `Aim` |
| `Pinned` | `PlayerPinnedState` | `Bump` | 0,5 | 1,5 | 0,5 | 0 | `Aim` |
| `ShipRumble` | `ShipHazardManager` | `Rumble` | 2 | 0,8 | 0 | 0 | `Aim` |
| `ShipCollapse` | `ShipHazardManager` | `Explosion` | 1 | 2 | 0 | 0 | `Aim` |
| `ShipFinalShudder` | `ShipHazardManager` | `Rumble` | 2,5 | 2 | 0 | 0 | `Aim` |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.shake.degreesPerUnit` | 2 | Sinyal Impulse menjadi derajat |
| `GameBalanceConfig.shake.metersPerUnit` | 0,02 | Sinyal Impulse menjadi meter |
| `GameBalanceConfig.shake.rollPerUnit` | 1 | Sinyal Impulse menjadi roll |
| `GameBalanceConfig.shake.maxDegrees` | 8 | Batas getaran, lebih kecil dari `camera.maxOffsetDegrees` (15) |
| `GameBalanceConfig.shake.maxMeters` | 0,12 | Batas posisi |
| `GameBalanceConfig.shake.hitReferenceDamage` | 25 | Damage yang menghasilkan skala 1 |
| `GameBalanceConfig.shake.hitScaleMin` / `hitScaleMax` | 0,5 / 1,5 | Batas skala menurut damage |

### Pilihan aksesibilitas dari #104

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `LocalizationManager.Accessibility.cameraShakeScale` | 1 | 0 sampai 1. Nilai 0 mematikan getaran. Recoil dari #58 tetap berjalan, karena memengaruhi permainan |

## 2. Detail Logika & Fungsi

### Fungsi publik

Semua fungsi statis, supaya pemanggil tidak perlu mengecek `Instance`. Kalau belum ada rig, seperti di menu, fungsi tidak melakukan apa-apa.

**`public static void Play(CameraShakeKind kind, float scale = 1f)`**: getaran tanpa jarak. Bentuk ini sama dengan `CameraShake.Play(Earthquake, scale)` di `DisasterManager.md`.

**`public static void PlayAt(CameraShakeKind kind, Vector3 worldPos, float scale = 1f)`**: getaran dengan jarak, memakai `falloffRadius`.

**`public static void PlayDirectional(CameraShakeKind kind, Vector3 worldDirection, float scale = 1f)`**: getaran dari arah serangan.

**`void Emit(CameraShakeKind kind, float scale, Vector3 localDirection)`**:

1. `settingScale = LocalizationManager.Instance.Accessibility.cameraShakeScale`. Kalau 0, keluar.
2. Kalau `Time.time < nextAllowedTime[kind]`, keluar. Lalu `nextAllowedTime[kind] = Time.time + preset.cooldownSeconds`.
3. `velocity = localDirection * preset.force * scale * settingScale`.
4. `sources[kind].GenerateImpulseAt(cameraPosition, velocity)`, dengan `ImpulseDefinition` yang sudah memakai `shape`, `duration`, dan `channel` preset.

**Jarak** tidak memakai dissipation bawaan, supaya satu aturan berlaku di semua jenis. Dengan `d = jarak ke worldPos`, kalau `d >= falloffRadius`, keluar. Kalau tidak, `scale *= 1f - (d / falloffRadius)`. Impuls selalu dibuat di posisi kamera.

### Membaca sinyal

**`void LateUpdate()`**, dengan `[DefaultExecutionOrder(150)]`, berjalan sebelum `FirstPersonCamera` (200):

```csharp
var mgr = CinemachineImpulseManager.Instance;
mgr.GetImpulseAt(camPos, false, (int)ShakeChannel.Aim, out Vector3 aimPos, out Quaternion aimRot);
mgr.GetImpulseAt(camPos, false, (int)ShakeChannel.Visual, out Vector3 visPos, out Quaternion _);

Vector3 euler = new Vector3(-aimPos.y * degreesPerUnit, aimPos.x * degreesPerUnit,
                            (aimPos.x + visPos.x) * rollPerUnit);
euler += NormalizeEuler(aimRot.eulerAngles);   // Nol kalau sinyal hanya berisi posisi

EulerOffset = ClampPerAxis(euler, maxDegrees);
PositionOffset = Vector3.ClampMagnitude((aimPos + visPos) * metersPerUnit, maxMeters);
```

**Dua channel:** channel `Aim` boleh menggeser pitch dan yaw, sedangkan channel `Visual` hanya mengubah posisi dan roll. Raycast tembakan #58 memakai `WorldCamera`, jadi getaran pitch dan yaw akan ikut menggeser peluru. Dengan `GunFire` di channel `Visual`, bidikan tetap hanya berubah oleh `AddRecoil`. Titik tengah layar tidak berpindah oleh roll.

### Getaran dari event

**`void HandleWeaponFired(WeaponFiredEvent e)`**: kalau `e.byPlayer`, ambil `WeaponData.cameraShakeScale` dari `ItemDatabase`, lalu `Play(GunFire, scale)`. Arah dasarnya sedikit ke atas, dengan sisi kiri atau kanan acak dari `UnityEngine.Random`.

**`void HandleDamaged(PlayerDamagedEvent e)`** menentukan jenis dari `DamageInfo`:

| `DamageInfo` | Hasil |
|---|---|
| `type == Melee` dan `infection > 0` | `ZombieBite`, dengan arah `hitDirection` |
| `type == Melee` atau `Bullet` | `PlayerHit`, dengan arah `hitDirection` |
| `Explosion` | Tidak ada. Pengirim ledakan sudah memanggil `PlayAt`, supaya tidak dobel |
| `Fall` | Tidak ada. Dip kamera milik #101 |
| `Fire`, `Frost`, `Starvation`, `Dehydration`, `Infection`, `Structural`, `Drown`, `Poison` | Tidak ada. Damage berkala akan membuat layar terus bergetar |

Skala: `Mathf.Clamp(e.info.amount / hitReferenceDamage, hitScaleMin, hitScaleMax)`.

Damage hewan, termasuk serudukan babi hutan, ikut jalur `PlayerHit` ini.

### Pemanggil langsung

| Sistem | Pemanggilan |
|---|---|
| `DisasterManager` (#70) | `Play(Earthquake, scale)`, `PlayAt(FireballImpact, titik)` |
| `ThrowableSystem` (#96) | `PlayAt(Explosion, pos)` |
| `PlayerPinnedState` (#110) | `Play(Pinned)` |
| `ShipHazardManager` (#75) | `Play(ShipRumble)` pada `T=30`, `Play(ShipCollapse)` saat koridor runtuh, `Play(ShipFinalShudder)` pada `T=0` |

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `WeaponFiredEvent` (#58), `PlayerDamagedEvent`, `PlayerDiedEvent`.

| Sistem | Hubungan |
|---|---|
| `FirstPersonCamera` (#102) | `RegisterOffsetSource(this)` pada `OnEnable`, dan `UnregisterOffsetSource(this)` pada `OnDisable`. Batas `maxOffsetDegrees` tetap berlaku |
| `GunController` (#58) | Recoil bidikan tetap lewat `AddRecoil`. Getaran layar di sini |
| `DamagePipeline` (#59) | Isi `DamageInfo` |
| `LocalizationManager` (#104) | `Accessibility.cameraShakeScale`, dibaca setiap kali getaran dibuat. Menu pengaturannya ada di #19 |
| `DevConsole` (#56) | Perintah `shake <kind> [scale]` |

**Batas pekerjaan:** recoil di #58 dan #102, dip mendarat di #101, ship sway di #75, dan tampilan terguncang panel UI milik `ShipTimerManager`.

## 4. Kasus Khusus

- **Objek pool:** tidak memakai `ObjectPooler`. Sumber impuls jumlahnya tetap, dan `CinemachineImpulseManager` mengelola data impuls sendiri.
- **Game dijeda:** `CinemachineImpulseManager.Instance.IgnoreTimeScale = false` diatur saat `Awake`, sehingga waktu impuls mengikuti `Time.time` dan berhenti saat jeda. Tidak ada `Play` baru dari pemain selama jeda. Getaran lanjut setelah jeda.
- **Permadeath:** tidak ada data. `OnDisable` memanggil `CinemachineImpulseManager.Instance.Clear()`, karena manager itu statis dan bisa membawa impuls lama ke scene berikutnya.
- **Pemain mati:** `HandleDied` memanggil `Clear()` dan menolkan offset, sesuai `DetachForDeath` di #102. Komponen tidak menerima getaran baru selama `isDead`.
- **Save dan load:** tidak ada yang disimpan. Getaran yang sedang berjalan tidak dilanjutkan.
- **Pindah fase:** rig baru dibuat, dan `Clear()` sudah membersihkan impuls lama.
- **Banyak getaran bersamaan:** sinyal dijumlahkan, lalu dibatasi `maxDegrees` dan `maxMeters`, sehingga kamera tidak terbalik.
- **Tembakan cepat:** impuls `GunFire` tidak menumpuk berlebihan, karena durasinya pendek dan hasilnya dibatasi.
- **Ledakan di luar radius:** tidak ada impuls.
- **Interaksi saat bergetar:** `targetGraceSeconds` di #17 sudah meredam perubahan singkat.
- **Tidur saat gempa:** getaran tetap diputar. Tidur dibatalkan oleh #67, kalau aturannya berlaku.
- **`Cinematic` memakai kamera lain:** offset tetap dihitung, tetapi tidak terlihat.
- **Sumber atau preset hilang:** `Play` keluar tanpa error, dan Editor mencatat peringatan.
- **Versi paket:** nama kelas di bagian ini mengikuti Cinemachine 2.x. Proyek memakai Cinemachine 3.1.6, dengan namespace `Unity.Cinemachine` dan sebagian nama properti berbeda. Periksa API yang benar lewat `unity_reflect` di Unity, sesuaikan kode saat Langkah yang memuat `CameraShake` ditulis, lalu perbarui bagian ini.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** getaran masuk lewat `ICameraOffsetSource`, dan hasil Cinemachine Impulse diubah menjadi offset itu (`FirstPersonCamera.md`). Pilihan `cameraShakeScale` (`LocalizationManager.md`). Recoil lewat `AddRecoil` (`GunController.md`). Getaran `T=30` di kapal (`60-seconds/README.md`).

**Keputusan teknis di blueprint ini:**

- Tidak ada Virtual Camera. Sinyal dibaca lewat `GetImpulseAt`.
- Fungsi `Play` statis, sehingga aman dipanggil dari mana pun.
- Jarak dihitung sendiri, bukan lewat dissipation.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Alat getaran:** rekomendasi tetap memakai Cinemachine Impulse seperti diminta issue, hanya untuk sinyalnya. Kalau paket tidak cocok, sinyal bisa diganti dengan Perlin sendiri tanpa mengubah pemanggil.
2. **Getaran tembak tidak menggeser bidikan:** rekomendasi `GunFire` memakai channel `Visual`, supaya akurasi hanya diatur `AddRecoil` dan spread.
3. **Kekuatan per weapon:** rekomendasi field baru `WeaponData.cameraShakeScale`, dengan nilai awal `Base_Pistol` 0,6.
4. **Damage berkala:** rekomendasi tanpa getaran.
5. **Nilai awal preset:** semua angka pada tabel preset harus disetel lewat playtest.

## 6. Skenario Tes

Belum diuji di Unity, karena proyek Unity belum dibuat.

1. Tembak `Base_Pistol` berkali-kali di depan satu titik. **Hasil:** layar bergetar ringan. Bidikan hanya bergeser oleh recoil, lalu kembali.
2. Dibandingkan dengan menonaktifkan `CameraShake`. **Hasil:** titik tembakan sama, karena `GunFire` berada di channel `Visual`.
3. Biarkan zombie menggigit. **Hasil:** getaran lebih kuat dari pukulan hewan, dari arah penyerang.
4. Biarkan babi hutan menyeruduk pemain. **Hasil:** getaran `PlayerHit`, dengan skala sesuai damage. Turunkan HP lewat `setvital`. **Hasil:** tidak ada getaran.
5. Ledakkan barang lempar pada 5, 15, dan 30 meter. **Hasil:** kuat, lemah, dan tanpa getaran. Damage ledakan tidak menghasilkan getaran kedua.
6. `disaster Earthquake 1`, lalu `disaster Earthquake 2`. **Hasil:** getaran gempa ada, dan skala 2 lebih kuat tetapi kamera tidak terbalik.
7. Jalani kapal sampai `T=30`, lalu sampai `T=0`. **Hasil:** getaran `ShipRumble`, lalu `ShipFinalShudder`. Ship sway tetap berjalan.
8. Biarkan pemain diterkam. **Hasil:** satu getaran `Pinned`, dan kamera tetap menghadap penyerang.
9. Tembak saat kena ledakan di kapal miring. **Hasil:** semua efek tergabung, dan offset tidak melewati batas.
10. Atur `cameraShakeScale` ke 0,5, lalu 0. **Hasil:** getaran setengah, lalu hilang. Recoil tetap ada.
11. Tekan Esc di tengah gempa. **Hasil:** kamera berhenti di tempat, lalu lanjut setelah jeda.
12. Mati di tengah getaran. **Hasil:** getaran berhenti, dan kamera kematian tetap stabil.
13. Mati di tengah gempa, lalu mulai run baru. **Hasil:** tidak ada getaran terbawa ke scene baru.
14. Lapar nol selama 30 detik. **Hasil:** HP turun tanpa getaran.
15. Pakai `shake Explosion 1` 20 kali dalam 1 detik. **Hasil:** tidak ada error, dan cooldown mengurangi jumlah getaran.
16. Buka Profiler selama 60 detik sambil menembak. **Hasil:** catat alokasi GC dari Cinemachine di lembar playtest. Kode `CameraShake` sendiri tidak mengalokasi.

## Catatan Implementasi (Sistem pendukung)

- Kode: `CameraShake.cs` dan `CameraShakeProfile.cs` (enum `CameraShakeKind`, `ShakeChannel`, struct `CameraShakePreset`). Config di `GameBalance.FondasiA.cs`.
- Cinemachine 3.x (namespace `Unity.Cinemachine`): nama kelas sama dengan blueprint. Properti definisi yang dipakai: `ImpulseDefinition.ImpulseChannel`, `ImpulseShape`, `ImpulseDuration`, dan `ImpulseType = Uniform` (tanpa dissipation, karena jarak dihitung sendiri). Pembacaan memakai `CinemachineImpulseManager.Instance.GetImpulseAt(pos, false, mask, out pos, out rot)`, `IgnoreTimeScale`, dan `Clear()`. Sudah lolos kompilasi terhadap `Unity.Cinemachine.dll` proyek, belum diuji di Unity.
- Sinyal dianggap ruang lokal kamera: `PlayDirectional` mengubah arah dunia ke lokal `WorldCamera` lewat `InverseTransformDirection`.
- `sources` yang kosong dibuat otomatis (`AddComponent`) di objek ini saat `Awake`, satu per jenis yang punya preset.
- Arah dasar preset: tabel blueprint tidak memberi nilai. `CameraShakeProfile.Reset()` memakai `(0,3; 1; 0)` untuk `GunFire` (sisi x diacak) dan `Vector3.down` (bawaan Cinemachine) untuk lainnya; disetel lewat playtest.
- Perintah `shake` milik `DevConsole` belum ada (TODO).
