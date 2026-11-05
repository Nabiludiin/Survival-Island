# Blueprint: DeathCinematic.cs
**Lokasi:** `docs/blueprint/Core_Logic/DeathCinematic.md`
**Deskripsi Utama:** Transisi visual saat pemain mati: ragdoll, kamera yang lepas dari tubuh, lalu fade menuju layar `GameOver`.
**Issue:** #53
**Rujukan GDD:** Bagian 1 dan bagian 3 (Permadeath). Tampilan adegan adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`DeathCinematic` adalah komponen pada prefab pemain Fase 1 dan Fase 2. `Instance` diisi di `OnEnable` dan dikosongkan di `OnDisable`, karena pemain hidup di scene. Kalau `Instance == null`, `RunLifecycle` melewati langkah ini.

```csharp
public enum DeathStyle { Ragdoll, Collapse, Sink, Zombify, Blackout }

[Serializable]
public class DeathProfile
{
    public DeathReason reason;
    public DeathStyle style;
    public string messageKey;     // Teks lewat LocalizationManager (#104)
    public Color screenTint;
    public AudioClip sting;
}
```

**Susunan prefab pemain:**

```text
Player (CharacterController, IslandPlayerController atau ShipPlayerController)
├── CameraRig (FirstPersonCamera)
└── PlayerBodyRig (nonaktif selama hidup)
    ├── SkinnedMeshRenderer, Animator
    └── Tulang: Rigidbody kinematik, Collider, CharacterJoint
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `DeathCinematic` | `null` | Akses dari `RunLifecycle` |
| `profiles` | `DeathProfile[]` | Tabel di bawah | Gaya tiap penyebab |
| `bones` | `Rigidbody[]` | Dari `PlayerBodyRig`, di `Awake` | Tulang ragdoll |
| `focusBone` | `Transform` | Tulang dada | Titik pandang kamera |
| `lastHitDir`, `lastHitPoint` | `Vector3` | `zero` | Dari `PlayerDamagedEvent` terakhir |
| `isPlaying` | `bool` | `false` | Mencegah adegan ganda |

### Penyebab dan gaya

| `DeathReason` | Gaya | Tampilan |
|---|---|---|
| `KilledByEnemy`, `Fall`, `Explosion`, `Fire`, `Poison` | `Ragdoll` | Tubuh terlempar sesuai arah pukulan. `Fire` memakai `VFXManager` di tubuh |
| `Starvation`, `Dehydration`, `Frostbite` | `Collapse` | Ragdoll tanpa gaya. Tubuh roboh perlahan, dan layar memudar lebih lama |
| `Drowned` | `Sink` | Ragdoll dengan `drag` tinggi, dan kamera tetap di bawah air |
| `Zombified` | `Zombify` | Tanpa ragdoll, sesuai `InfectionSystem.md`. Pandangan hijau penuh, kontrol dilepas, napas berubah menjadi erangan, lalu fade, dengan durasi `GameBalanceConfig.infection.zombifyCinematicSeconds` (4) |
| `LeftOnShip` | `Blackout` | Tanpa ragdoll. Air naik, lalu layar hitam |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.deathCinematic.ragdollForce` | 300 | Gaya dasar pukulan |
| `GameBalanceConfig.deathCinematic.explosionForceMultiplier` | 3 | Pengali untuk `Explosion` |
| `GameBalanceConfig.deathCinematic.sinkDrag` | 4 | `Rigidbody.drag` saat `Sink` |
| `GameBalanceConfig.deathCinematic.slowMoScale` | 0,35 | Skala waktu saat tubuh jatuh |
| `GameBalanceConfig.deathCinematic.slowMoSeconds` | 1,2 | Lama gerak lambat, dalam waktu nyata |
| `GameBalanceConfig.deathCinematic.cameraRiseSeconds` | 1,5 | Lama kamera naik |
| `GameBalanceConfig.deathCinematic.cameraHeight` | 2,5 | Tinggi kamera di atas tubuh |
| `GameBalanceConfig.deathCinematic.cameraDistance` | 3 | Jarak kamera dari tubuh |
| `GameBalanceConfig.deathCinematic.orbitDegreesPerSecond` | 12 | Putaran kamera |
| `GameBalanceConfig.deathCinematic.holdSeconds` | 3,5 | Lama melihat tubuh sebelum fade |
| `GameBalanceConfig.deathCinematic.collapseHoldSeconds` | 5 | Pengganti `holdSeconds` untuk `Collapse` |
| `GameBalanceConfig.deathCinematic.fadeSeconds` | 1,5 | Fade menuju hitam |
| `GameBalanceConfig.deathCinematic.messageSeconds` | 2 | Teks penyebab di layar hitam |
| `GameBalanceConfig.deathCinematic.skipAfterSeconds` | 2 | `Skip` boleh dipakai setelah waktu ini |

Total durasi terlama sekitar 10 detik, di bawah `GameBalanceConfig.run.endSequenceTimeoutSeconds` (15).

## 2. Detail Logika & Fungsi

### Urutan bersama `RunLifecycle`

```text
SurvivalStats --PlayerDiedEvent--> sistem lain berhenti, GameManager.TriggerGameOver
GameManager --RunEndedEvent--> RunLifecycle.RunEndSequence
  2. WorldStateSave.MarkRunEnded        (penanda dibuat)
  3. CloseAllUnlocked, Push(Cinematic)  (input gameplay terkunci)
  4. yield DeathCinematic.Play(reason)  (blueprint ini)
  5. WorldStateSave.DeleteRunSave       (save dihapus)
  6. LocalStatsManager.RecordRunEnd     (statistik)
  8. Tutup Cinematic, dorong GameOver   (layar dramatis)
```

`DeathCinematic` tidak menyentuh file save. Penghapusan tetap satu pintu di `RunLifecycle`. Penanda sudah dibuat sebelum adegan mulai, jadi mematikan game saat adegan tidak bisa membatalkan permadeath (#82).

### Pemutar

**`public IEnumerator Play(DeathReason reason)`**:

1. Keluar kalau `isPlaying`. Set `isPlaying = true`, lalu terbitkan `CinematicStateChangedEvent { isPlaying = true, keepMusic = false }` (#114).
2. `yield return null`, supaya semua pendengar `PlayerDiedEvent` selesai lebih dulu: `HeavyItemSystem.Drop(Death)`, `PlayerPinnedState.Release`, `PlayerTraversal.EndClimb`, dan kunci `Dead` di `EquipmentManager`.
3. Ambil `profile`. Tampilkan `screenTint`, dan mainkan `sting`. Mulai turunkan musik dunia.
4. `DisableController()`.
5. Jalankan gaya sesuai tabel. `Ragdoll`, `Collapse`, dan `Sink` memanggil `EnableRagdoll(profile.style)`.
6. Kecuali `Zombify` dan `Blackout`: `FirstPersonCamera.Instance.DetachForDeath()`, lalu jalankan `CameraOrbit()` sebagai coroutine.
7. Tunggu `holdSeconds`, atau `collapseHoldSeconds`, memakai `WaitForSecondsRealtime`. Selama `skipAfterSeconds` sudah lewat, `InputManager.SkipPressed` memotong sisa tunggu.
8. Fade hitam `fadeSeconds` lewat `yield return UIManager.Instance.cinematicPanel.FadeToBlack(fadeSeconds)`, tampilkan `messageKey` lewat `ShowCaption(messageKey)` selama `messageSeconds`, lalu selesai (`UIManager.md`). Layar tetap hitam sampai panel `GameOver` tampil.
9. Di blok `finally`, walaupun terjadi kesalahan: `SetGameplayTimeScale(1)`, lalu terbitkan `CinematicStateChangedEvent { isPlaying = false, keepMusic = false }`.

### Melepas pengendali dan mengaktifkan ragdoll

**`private void DisableController()`**:

1. `PlayerTraversal.SetLocked(true)`.
2. Matikan `IslandPlayerController`, atau `ShipPlayerController`.
3. `CharacterController.enabled = false`. Tanpa langkah ini, kapsul tetap menabrak tulang dan tubuh terlempar.
4. Matikan audio langkah (#21) dengan menonaktifkan komponennya.

**`private void EnableRagdoll(DeathStyle style)`**:

1. `PlayerBodyRig.SetActive(true)`, dengan posisi dan arah kapsul.
2. `animator.enabled = false`.
3. Untuk tiap tulang: pindah ke layer `Ragdoll`, `isKinematic = false`, `useGravity = true`, `interpolation = Interpolate`, dan `collisionDetectionMode = ContinuousSpeculative`.
4. `Ragdoll`: `AddForceAtPosition(lastHitDir * force, lastHitPoint, Impulse)` pada tulang terdekat, dengan `force = ragdollForce`, dikali `explosionForceMultiplier` kalau `Explosion`.
5. `Collapse`: tanpa gaya.
6. `Sink`: `drag = sinkDrag` pada semua tulang.
7. `UIFocusStack.Instance.SetGameplayTimeScale(slowMoScale)`, lalu kembalikan ke 1 setelah `slowMoSeconds` waktu nyata. `Collapse` dan `Sink` tidak memakai gerak lambat.

Layer `Ragdoll` hanya menyentuh `Terrain`, `Structure`, dan `Obstacle` (`PhysicsLayers.md`). Tubuh tidak mendorong musuh, dan tidak tertahan oleh `Water`.

### Kamera mengambang

**`private IEnumerator CameraOrbit()`** menggerakkan `CameraRig`, yang sudah lepas dari pemain:

1. Simpan posisi mata sebagai titik awal.
2. Sudut awal diambil dari arah pandang terakhir, sehingga perubahan pandangan tidak terasa melompat.
3. Tiap frame: `angle += orbitDegreesPerSecond * Time.unscaledDeltaTime`. Titik tujuan `focusBone.position + (arah orbit * cameraDistance) + (Vector3.up * cameraHeight)`.
4. Selama `cameraRiseSeconds`, posisi berpindah dari mata menuju titik tujuan dengan `SmoothStep`.
5. Periksa halangan: `PhysicsQuery.Raycast(ray dari focusBone ke tujuan, out hit, jarak, PhysicsMasks.Ground)`. Kalau kena, kamera berhenti di `hit.point + hit.normal * 0.2f`.
6. `CameraRig.LookAt(focusBone)`.

Kamera memakai `unscaledDeltaTime`, jadi gerakannya tetap halus saat gerak lambat.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `CinematicStateChangedEvent`.

**Event yang didengar:** `PlayerDamagedEvent` (arah dan titik pukulan).

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `Play`, dan memegang batas waktunya |
| `FirstPersonCamera` (#102) | `DetachForDeath` |
| `PlayerTraversal` (#101) | `SetLocked` |
| `UIFocusStack` (#83) | Panel `Cinematic` dan `SetGameplayTimeScale` |
| `UIManager` | Fade hitam dan teks pada panel `Cinematic`, lewat `CinematicPanel.FadeToBlack`, `ShowCaption`, dan `ClearCaption` (`UIManager.md`) |
| `InputManager` (#18) | `SkipPressed` |
| `VFXManager` (#15), `AudioManager` | Efek dan suara |
| `LocalizationManager` (#104) | `messageKey` |
| `InfectionSystem` (#64) | Durasi `Zombified` |
| `HeavyItemSystem`, `PlayerPinnedState`, `EquipmentManager`, `ThreatSystem` | Mendengar `PlayerDiedEvent` secara mandiri |

**Batas pekerjaan:** layar `GameOver` dan hapus save di #82, ragdoll musuh di #95.

## 4. Kasus Khusus

- **Objek pool:** tubuh pemain tidak memakai pool. Tubuh hilang bersama scene saat `ResetRunState`.
- **Game dijeda:** panel `Cinematic` tidak menjeda waktu dan tidak bisa ditutup Esc, sehingga `Pause` tidak bisa terbuka. Dunia tetap berjalan di balik adegan. Semua penantian memakai waktu nyata.
- **Musuh setelah pemain mati:** `ThreatSystem` melepas semua target (`PlayerDiedEvent`), jadi tubuh tidak diserang.
- **Permadeath dan save:** adegan tidak menyimpan apa pun. Skala waktu kembali ke 1 di `finally`, dan `UIFocusStack.ClearForNewRun` menjadi pengaman terakhir.
- **Game ditutup paksa:** penanda `run_end.marker` menyelesaikan permadeath saat boot berikutnya (#82).
- **Batas waktu tercapai:** `RunLifecycle` melanjutkan alur, dan coroutine dihentikan saat scene diganti.
- **`PlayerBodyRig` belum tersedia:** `EnableRagdoll` dilewati, dan gaya menjadi `Blackout`. Kegagalan sinematik tidak boleh menghentikan permadeath.
- **Tidak ada `PlayerDamagedEvent` sebelumnya** (misalnya `kill` lewat konsol): `lastHitDir` bernilai `zero`, jadi tubuh roboh tanpa gaya.
- **Mati di Fase 1:** alur sama. `ShipPlayerController` dimatikan, dan kemiringan kapal tetap berlaku pada tubuh.
- **Tubuh jatuh menembus terrain atau jatuh jauh:** kamera tetap mengikuti, dan batas waktu mengakhiri adegan.
- **Mati saat membawa Heavy Item:** barang dijatuhkan lebih dulu, jadi tidak melayang di depan kamera.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- `Zombified` tanpa ragdoll, dengan pandangan hijau (`InfectionSystem.md`).
- `DetachForDeath` dan `SetGameplayTimeScale` (`FirstPersonCamera.md`, `UIFocusStack.md`).
- Urutan penanda, sinematik, hapus save, dan statistik (`RunLifecycle.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Model tubuh pemain:** rekomendasi `PlayerBodyRig` yang dinonaktifkan selama hidup, karena game first-person hanya menampilkan tangan. Alternatifnya, model mayat terpisah yang dibuat saat mati.
2. **Lewati adegan:** rekomendasi `Skip` boleh setelah 2 detik, karena permadeath membuat adegan ini sering terlihat.
3. **Gerak lambat:** rekomendasi 1,2 detik pada skala 0,35, hanya untuk kematian oleh pukulan.
4. **Gaya per penyebab:** rekomendasi lima gaya pada tabel bagian 1.
5. **Teks penyebab:** rekomendasi teks singkat di layar hitam, misalnya `Anda mati kedinginan`, sebelum `GameOver` tampil.

## 6. Skenario Tes

1. `kill KilledByEnemy` setelah terkena pukulan zombie. **Hasil:** tubuh terlempar searah pukulan, dan kamera naik lalu berputar mengelilingi tubuh.
2. `kill Starvation`. **Hasil:** tubuh roboh tanpa gaya, dan adegan lebih panjang.
3. `kill Drowned` di air. **Hasil:** tubuh tenggelam perlahan, dan tidak mengambang.
4. `kill Zombified`. **Hasil:** tidak ada ragdoll, pandangan hijau, dan adegan 4 detik.
5. Mati di samping dinding sempit. **Hasil:** kamera berhenti di depan dinding, dan tidak menembusnya.
6. Mati saat `CharacterController` aktif sambil berlari. **Hasil:** tubuh tidak terlempar oleh kapsul.
7. Tekan Esc berkali-kali saat adegan. **Hasil:** `Pause` tidak terbuka.
8. Tekan `Space` pada detik 1, lalu pada detik 3. **Hasil:** yang pertama diabaikan, yang kedua langsung memulai fade.
9. Pantau `Time.timeScale` di Inspector. **Hasil:** kembali ke 1 setelah gerak lambat.
10. Matikan proses saat adegan, lalu buka lagi. **Hasil:** Lanjutkan tidak tampil, sesuai #82.
11. Mati dengan `eventlog 1`. **Hasil:** log `RunEndSequence` menunjukkan adegan selesai dulu, baru hapus save, statistik, dan layar `GameOver`.
12. Mati saat membawa `Part_Engine`. **Hasil:** barang jatuh sebelum kamera lepas.
13. Hapus `PlayerBodyRig` dari prefab uji, lalu mati. **Hasil:** gaya `Blackout`, dan alur tetap selesai.
14. Mati lima kali berturut-turut. **Hasil:** skala waktu selalu 1 pada run baru.
