# Blueprint: FootstepAudio.cs
**Lokasi:** `docs/blueprint/Core_Logic/FootstepAudio.md`
**Deskripsi Utama:** Memutar suara langkah dan mendarat pemain sesuai permukaan yang diinjak, di kapal dan di pulau. Permukaan dibaca dengan raycast ke bawah lewat `SurfaceDetection`.
**Issue:** #21
**Rujukan GDD:** Mendukung bagian 3 (Fase 1 di kapal) dan bagian 5 (region). Sistem suara langkah sendiri adalah usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`FootstepAudio : MonoBehaviour` dipasang di prefab pemain Fase 1 dan Fase 2. Komponen ini hanya membaca `CharacterController`, jadi tidak bergantung pada `ShipPlayerController` atau `IslandPlayerController`. `PlayerTraversal` dibaca kalau ada, dan di kapal nilainya `null`.

```csharp
[CreateAssetMenu(menuName = "Survival/Surface Audio Set")]
public class SurfaceAudioSet : ScriptableObject
{
    public SurfaceAudioEntry[] entries;     // Satu baris per SurfaceType
}

[Serializable]
public struct SurfaceAudioEntry
{
    public SurfaceType type;
    public AudioClip[] steps;               // Minimal 3 klip, supaya tidak monoton
    public AudioClip[] landings;            // Boleh kosong, lalu memakai steps
    public float volume;                    // Nilai awal 1
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `audioSet` | `SurfaceAudioSet` | Diisi di prefab | Klip per permukaan |
| `source` | `AudioSource` | Di objek kaki | 2D (`spatialBlend = 0`), `playOnAwake = false`, masuk grup SFX di `AudioMixer` |
| `lookup` | `SurfaceAudioEntry[]` | Dibuat di `Awake` | Indeks `(int)SurfaceType`, tanpa `Dictionary` |
| `distanceSinceStep` | `float` | 0 | Jarak horizontal sejak langkah terakhir |
| `lastClipIndex` | `int` | -1 | Mencegah klip sama berturut-turut |
| `lastSurface` | `SurfaceType` | `Tanah` | Dipakai kalau raycast gagal |
| `airTime` | `float` | 0 | Lama tidak menyentuh tanah |
| `lastFallSpeed` | `float` | 0 | Kecepatan jatuh frame sebelumnya |
| `isCarryingHeavy` | `bool` | `false` | Dari `HeavyItemChangedEvent` |

### Klip per permukaan

Isi `SurfaceAudioSet` mengikuti delapan nilai `SurfaceType`.

| `SurfaceType` | Tempat umum | Karakter klip |
|---|---|---|
| `Kayu` | Lantai kapal, bangunan kayu pemain | Ketukan kayu berongga |
| `Logam` | Dek bawah kapal | Dentang logam |
| `Pasir` | Pantai | Gesekan lembut |
| `Rumput` | Region 1 dan Region 2 | Gemerisik |
| `Tanah` | Nilai cadangan | Langkah tumpul |
| `Batu` | Tebing, bangunan batu | Ketukan keras |
| `Salju` | Region 3 | Bunyi salju terinjak |
| `Lumpur` | Region 4 | Bunyi basah |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.footstep.stepDistance` | 1,75 | Jarak per langkah, dalam meter. Turunan dari `pi * walkSpeed / bobFrequency` (#102), sehingga langkah jatuh seirama head bob |
| `GameBalanceConfig.footstep.minSpeed` | 0,5 | Sama dengan batas head bob. Di bawah ini tidak ada langkah |
| `GameBalanceConfig.footstep.baseVolume` | 0,6 | Volume dasar |
| `GameBalanceConfig.footstep.crouchVolumeMultiplier` | 0,4 | Saat `PlayerTraversal.IsCrouching` |
| `GameBalanceConfig.footstep.sprintSpeedThreshold` | 6 | Di atas kecepatan ini langkah dianggap berlari |
| `GameBalanceConfig.footstep.sprintVolumeMultiplier` | 1,3 | Saat berlari |
| `GameBalanceConfig.footstep.heavyVolumeMultiplier` | 1,2 | Saat membawa Heavy Item |
| `GameBalanceConfig.footstep.pitchMin` / `pitchMax` | 0,93 / 1,07 | Variasi nada |
| `GameBalanceConfig.footstep.volumeJitter` | 0,1 | Variasi volume |
| `GameBalanceConfig.footstep.minLandingAirSeconds` | 0,25 | Lebih singkat dari ini, tidak ada suara mendarat |
| `GameBalanceConfig.footstep.landingFullFallSpeed` | 12 | Kecepatan jatuh yang menghasilkan volume penuh |

## 2. Detail Logika & Fungsi

**`void Awake()`**: isi `lookup` dari `audioSet.entries`. Baris yang kosong diisi dari `Tanah`, dan Editor mencatat peringatan.

**`void Update()`**:

```csharp
if (!controller.enabled) return;
Vector3 v = controller.velocity;
float speed = new Vector2(v.x, v.z).magnitude;
bool grounded = controller.isGrounded;

UpdateLanding(grounded, v.y);
if (!CanStep(grounded, speed)) { distanceSinceStep = 0f; return; }

distanceSinceStep += speed * Time.deltaTime;
if (distanceSinceStep >= stepDistance)
{
    distanceSinceStep -= stepDistance;
    PlayStep(speed);
}
```

Langkah dihitung dari jarak, bukan timer atau Animation Event. Lari otomatis lebih rapat karena jarak tempuhnya lebih cepat. Memakai `controller.velocity` juga berarti pemain yang menabrak dinding tidak bersuara, dan teleport tidak menghasilkan langkah palsu.

**`bool CanStep(bool grounded, float speed)`** mengembalikan `true` kalau:

1. `grounded` dan `speed >= minSpeed`.
2. `traversal == null`, atau `traversal.Mode` bernilai `Ground` atau `Crouching`.

Mode `Climbing`, `Sliding`, `Swimming`, dan `Locked` tidak menghasilkan langkah. Suara air milik #13.

**`void PlayStep(float speed)`**:

1. `feet = transform.position`, yaitu titik dasar `CharacterController`.
2. `surface = SurfaceDetection.Instance.TryGetSurfaceAt(feet, out sample) ? sample.type : lastSurface`.
3. Simpan `lastSurface = surface`.
4. Pilih klip acak dari `lookup[(int)surface].steps` dengan `UnityEngine.Random`, kecuali `lastClipIndex`. Langkah ini hanya tampilan, jadi tidak memakai `RunSeedManager` (#89).
5. Hitung volume, lalu `source.pitch = Random.Range(pitchMin, pitchMax)` dan `source.PlayOneShot(clip, volume)`.

```csharp
float vol = baseVolume * entry.volume * Random.Range(1f - volumeJitter, 1f);
if (traversal != null && traversal.IsCrouching) vol *= crouchVolumeMultiplier;
else if (speed >= sprintSpeedThreshold) vol *= sprintVolumeMultiplier;
if (isCarryingHeavy) vol *= heavyVolumeMultiplier;
```

Raycast hanya terjadi saat satu langkah berbunyi, bukan tiap frame, sehingga bebannya kecil. `SurfaceDetection` sudah memakai `PhysicsQuery.Raycast` dan `PhysicsMasks.Ground`, jadi komponen ini tidak menembakkan raycast sendiri.

**`void UpdateLanding(bool grounded, float velocityY)`**:

1. Kalau tidak `grounded`: `airTime += Time.deltaTime`, dan `lastFallSpeed = Mathf.Max(lastFallSpeed, -velocityY)`.
2. Kalau `grounded` dan `airTime >= minLandingAirSeconds`: panggil `PlayLanding(Mathf.Clamp01(lastFallSpeed / landingFullFallSpeed))`.
3. Saat `grounded`, atur `airTime = 0` dan `lastFallSpeed = 0`.

**`void PlayLanding(float impact01)`** membaca permukaan seperti `PlayStep`, memakai `landings` atau `steps`, lalu memutar dengan volume `baseVolume * entry.volume * Mathf.Lerp(0.5f, 1.3f, impact01)`. Saat itu `distanceSinceStep` direset, supaya langkah pertama setelah mendarat tidak menumpuk.

**Rute dari kapal ke pantai:** tidak ada logika khusus. Permukaan dibaca dari `SurfaceTag` di kapal, dan dari peta Terrain di pulau.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** `HeavyItemChangedEvent` (sticky, dibaca saat `OnEnable`).

| Sistem | Hubungan |
|---|---|
| `SurfaceDetection` (#86) | `TryGetSurfaceAt(feet, out sample)`. Sudah tercantum di tabel pemakainya |
| `PlayerTraversal` (#101) | Membaca `Mode` dan `IsCrouching`. Dip kamera saat mendarat tetap milik #101 |
| `AudioManager` | Menyediakan grup SFX di `AudioMixer` dan volume dari #19. Komponen ini memakai `AudioSource` sendiri, karena `PlaySFX` tidak bisa mengubah pitch |
| `NoiseEmitter`, `AISensory` (#49) | **Terpisah.** Radius suara untuk musuh dihitung #49 dengan coroutine sendiri. Komponen ini tidak mengirim `NoiseEmittedEvent`, supaya musuh tidak mengikuti ritme langkah audio |
| `FirstPersonCamera` (#102) | Tidak ada panggilan. `stepDistance` diselaraskan lewat angka |
| `HeavyItemSystem` (#69) | Sumber `HeavyItemChangedEvent` |
| `SwimmingController` (#13) | Mengambil alih suara saat `Mode == Swimming` |

**Batas pekerjaan:** deteksi permukaan di #86, suara air di #13, dan suara langkah musuh di luar issue ini.

## 4. Kasus Khusus

- **Objek pool:** tidak memakai `ObjectPooler`. Hanya ada satu pemain, dan `AudioSource` memakai `PlayOneShot` tanpa objek baru.
- **Game dijeda:** `Time.deltaTime` bernilai 0, jadi jarak dan `airTime` tidak bertambah. `AudioListener.pause` (#83) menahan klip yang sedang berbunyi, dan klip lanjut setelah jeda. `source.ignoreListenerPause` tetap `false`.
- **Panel menahan input tanpa jeda:** pemain berhenti, jadi `speed` di bawah `minSpeed`, dan langkah berhenti.
- **Permadeath:** tidak ada data. Komponen hilang bersama scene, dan langganan dilepas di `OnDisable`.
- **Save dan load:** tidak ada yang disimpan. `HeavyItemChangedEvent` dikirim ulang lewat `ISaveable` (#80).
- **Pemain mati:** `CharacterController` dimatikan oleh #53, sehingga `Update` langsung keluar.
- **Raycast gagal, misalnya region belum dimuat:** memakai `lastSurface`, sehingga tetap ada suara.
- **Seam dua Terrain:** nilai dominan dipakai, dan `SurfaceDetection` memastikan permukaan tidak berkedip.
- **Bangunan di atas pasir:** raycast mengenai `Structure`, sehingga suaranya `Kayu`.
- **Kapal miring:** ray tetap lurus ke bawah dunia, sesuai #86.
- **Air dangkal:** suara mengikuti dasar air selama mode masih `Ground`. Lihat usulan 3.
- **Klip kosong pada satu permukaan:** memakai baris `Tanah`. Kalau `Tanah` juga kosong, tidak ada suara dan tidak ada error.
- **Tidur:** input terkunci, sehingga kecepatan 0.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SurfaceType` dan cara membaca permukaan (`GLOSSARY.md`, #86), noise musuh (#49), dan `UnityEngine.Random` untuk efek tampilan (#89).

**Keputusan teknis di blueprint ini:**

- Tidak ada raycast sendiri. Raycast ke bawah yang diminta issue dijalankan `SurfaceDetection`, satu kali per langkah.
- `FootstepAudio` tidak mengirim noise.
- Tiap permukaan diakses lewat larik, tanpa alokasi saat bermain.

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Pemicu langkah:** rekomendasi berbasis jarak, bukan Animation Event. Kamera orang pertama tidak menampilkan kaki, dan cara ini juga berlaku sebelum animasi #12 ada.
2. **Nilai awal:** `stepDistance` 1,75 m, jongkok 40 persen, dan berlari 130 persen.
3. **Air dangkal:** rekomendasi tidak ada suara khusus di versi awal. Suara cipratan bisa ditambah bersama #13 dan #91.
4. **Suara langkah musuh:** rekomendasi tidak termasuk di sini. Kalau nanti dibutuhkan, zombie memakai komponen 3D terpisah yang di-reset lewat `IPoolable`.

## 6. Skenario Tes

Belum diuji di Unity, karena proyek Unity belum dibuat.

1. Berjalan di lantai kapal dengan `SurfaceTag` `Kayu`. **Hasil:** suara kayu, dan `surfacedebug 1` menunjukkan `Kayu`.
2. Pindah ke dek dengan `SurfaceTag` `Logam`. **Hasil:** suara berganti pada langkah berikutnya.
3. Di pulau, berjalan dari rumput ke pantai. **Hasil:** suara berganti dari `Rumput` ke `Pasir`, tepat di tempat `surfacedebug` berganti.
4. Berjalan 17,5 meter tanpa berhenti. **Hasil:** tepat sekitar 10 langkah, sama saat berlari.
5. Bandingkan langkah dan head bob. **Hasil:** satu langkah per setengah siklus bob, dan tidak bergeser.
6. Jongkok, lalu lari. **Hasil:** volume mengecil saat jongkok dan membesar saat lari.
7. Berjalan menabrak dinding. **Hasil:** tidak ada langkah.
8. Melompat kecil, lalu jatuh dari tebing 5 meter. **Hasil:** lompatan bersuara pelan dan jatuh bersuara keras, sesuai permukaan.
9. Berjalan melewati lantai bangunan di atas rumput. **Hasil:** suara `Kayu`.
10. Masuk air sampai berenang, lalu naik tangga. **Hasil:** tidak ada langkah di dua mode itu.
11. Ambil Heavy Item. **Hasil:** volume naik, dan jeda langkah melebar karena pemain melambat.
12. Tekan Esc saat berjalan. **Hasil:** suara berhenti, dan tidak ada langkah tambahan selama jeda.
13. Dengarkan 20 langkah di satu permukaan. **Hasil:** klip yang sama tidak terdengar dua kali berturut-turut.
14. Kosongkan baris `Salju`, lalu berjalan di salju. **Hasil:** memakai `Tanah`, dan Editor mencatat peringatan.
15. Buka Profiler selama 60 detik sambil berjalan. **Hasil:** tidak ada alokasi GC dari `FootstepAudio`.
