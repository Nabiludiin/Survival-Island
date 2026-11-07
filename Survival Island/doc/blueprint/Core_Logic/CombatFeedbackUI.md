# Blueprint: CombatFeedbackUI.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/CombatFeedbackUI.md`
**Deskripsi Utama:** Umpan balik pertempuran di HUD: indikator arah damage di pinggir layar, Hit Marker, dan Kill Marker. Sistem ini hanya mendengar event dan tidak mengubah status gameplay.
**Issue:** #51
**Rujukan GDD:** Bagian 3 (ancaman zombie dan hewan), secara tidak langsung. Bentuk tampilannya adalah usulan, karena GDD tidak mengatur HUD pertempuran.

## 1. Arsitektur & Variabel Utama

`CombatFeedbackUI` adalah komponen di canvas HUD persisten (anak `HUDManager`), bukan singleton. Fungsinya terpisah dari `PostProcessController` (#23), yang mengatur vignette layar penuh.

```text
HUD Canvas
├── DamageRing (RectTransform di tengah layar, induk indikator)
└── CrosshairRoot (milik HUDManager)
    ├── Crosshair
    └── MarkerRoot
        ├── HitMarkerImage (empat garis diagonal)
        └── KillMarkerImage (bentuk sama, warna merah, ukuran lebih besar)
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `playerCamera` | `Transform` | Dari `FirstPersonCamera` (#102) | Acuan arah pandang |
| `active` | `DamageIndicator[]` | Kosong, panjang `maxIndicators` | Indikator yang sedang tampil |
| `markerTimer`, `killTimer` | `float` | 0 | Sisa waktu marker |
| `lastTinkTime` | `float` | `-999` | Pembatas suara Tink |
| `hitMarkerClip`, `killTinkClip` | `AudioClip` | Dari Inspector | Suara marker |
| `feedbackEnabled` | `bool` | `true` | `false` selama adegan atau run berakhir |

`DamageIndicator` adalah objek pool dengan tag `UI_DamageIndicator` dan `IPoolable`. Isinya satu `Image` berbentuk busur darah di pinggir layar, dengan data `sourcePosition`, `timer`, dan `strength`.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.combatFeedback.maxIndicators` | 6 | Batas indikator serentak, sekaligus ukuran pool |
| `GameBalanceConfig.combatFeedback.indicatorSeconds` | 1,2 | Lama tampil setelah pukulan terakhir |
| `GameBalanceConfig.combatFeedback.indicatorMergeAngle` | 30 | Dua pukulan dalam sudut ini memakai satu indikator |
| `GameBalanceConfig.combatFeedback.minAlpha` | 0,35 | Kecerahan untuk pukulan kecil |
| `GameBalanceConfig.combatFeedback.fullAlphaDamage` | 30 | Damage yang menghasilkan kecerahan penuh |
| `GameBalanceConfig.combatFeedback.sourceOffsetMeters` | 10 | Jarak titik sumber dari pemain |
| `GameBalanceConfig.combatFeedback.telegraphSeconds` | 0,6 | Lama penanda serangan yang akan datang |
| `GameBalanceConfig.combatFeedback.hitMarkerSeconds` | 0,12 | Lama Hit Marker |
| `GameBalanceConfig.combatFeedback.headshotMarkerScale` | 1,3 | Skala marker saat kena kepala |
| `GameBalanceConfig.combatFeedback.killBlinkCount` | 2 | Jumlah kedipan merah |
| `GameBalanceConfig.combatFeedback.killBlinkSeconds` | 0,3 | Total lama kedipan |
| `GameBalanceConfig.combatFeedback.tinkMinInterval` | 0,08 | Jeda terpendek antar-suara Tink |

Warna ada di prefab: Hit Marker putih, Kill Marker merah, busur damage merah darah, dan penanda serangan kuning pucat.

## 2. Detail Logika & Fungsi

### Directional Damage

**`HandlePlayerDamaged(PlayerDamagedEvent e)`**

1. Keluar kalau `feedbackEnabled == false`.
2. Tentukan arah lewat `TryResolveSource(e.info, out Vector3 sourcePos)`:
   - `hitDirection` tidak nol: `sourcePos = posisiPemain - hitDirection.normalized * sourceOffsetMeters`. Nilai ini adalah arah dari musuh ke target (`EnemyAttack.md`), jadi sumbernya berada di arah sebaliknya.
   - `hitDirection` nol dan `info.source` masih ada: pakai `info.source.transform.position`.
   - Keduanya tidak ada, misalnya jatuh: tidak ada indikator, karena tidak ada arah yang bisa ditunjukkan.
3. Hitung `strength = Lerp(minAlpha, 1, damage / fullAlphaDamage)`.
4. Cari indikator aktif yang selisih sudutnya kurang dari `indicatorMergeAngle`. Kalau ada, perbarui `sourcePosition`, atur `timer = indicatorSeconds`, dan pakai `strength` yang lebih besar.
5. Kalau tidak ada, ambil `SpawnFromPool("UI_DamageIndicator", ...)` dan pasang di `DamageRing`. Kalau `active` penuh, pakai ulang indikator dengan `timer` terkecil.

Damage kondisi seperti lapar, haus, dan frostbite tidak mengirim `PlayerDamagedEvent` (#59), sehingga tidak memunculkan indikator.

**`LateUpdate()`** memperbarui tiap indikator:

1. `toSource = sourcePosition - posisiPemain`, lalu ratakan ke bidang horizontal. Kalau panjangnya di bawah 0,1 meter, pakai sudut terakhir.
2. `angle = Vector3.SignedAngle(flatCameraForward, toSource, Vector3.up)`.
3. `rectTransform.localEulerAngles = (0, 0, -angle)`, sehingga busur bergeser saat pemain menoleh. Sumber di belakang pemain tampil di pinggir bawah.
4. `timer -= Time.deltaTime`. Alpha `= strength * Clamp01(timer / (indicatorSeconds * 0,5))`.
5. Kalau `timer <= 0`, kembalikan ke pool lewat `ReturnToPool`.

**`HandleTelegraph(EnemyAttackTelegraphEvent e)`** memeriksa `playerCamera.WorldToViewportPoint(e.position)`. Kalau posisinya di dalam pandangan, tidak ada yang ditampilkan. Kalau di luar, munculkan indikator kuning selama `telegraphSeconds`. Indikator ini mengikuti batas `maxIndicators`, dan tidak menggantikan busur damage.

### Hit Marker

**`HandleEnemyHit(EnemyHitEvent e)`**

1. Keluar kalau `e.byPlayer == false`, sehingga tembakan NPC tidak memberi marker.
2. Keluar kalau `IsPositionInView(e.position) == false`. Jebakan (#26) mengirim `byPlayer = true`, dan pemeriksaan ini mencegah marker dari jebakan di luar pandangan.
3. Kalau `e.isKill`, panggil `PlayKillMarker()`, lalu keluar.
4. `markerTimer = hitMarkerSeconds`, aktifkan `HitMarkerImage`, dan atur skala `headshotMarkerScale` kalau `bodyPart == Head`, atau 1 kalau tidak.
5. Putar `hitMarkerClip` lewat `AudioManager.PlaySFX`.

Marker berlaku untuk semua serangan pemain, yaitu pistol, melee, dan barang lempar. Banyak kena dalam satu frame hanya menyetel ulang timer, sehingga hanya satu marker yang tampil.

### Kill Marker

**`PlayKillMarker()`**

1. `killTimer = killBlinkSeconds`, dan matikan `HitMarkerImage`.
2. `Update` menghitung kedipan: `tampil = Floor(progress * killBlinkCount * 2) % 2 == 0`.
3. Kalau `Time.time - lastTinkTime >= tinkMinInterval`, putar `killTinkClip` lewat `AudioManager.PlaySFX`, lalu simpan `lastTinkTime`.

Kill Marker selalu mengalahkan Hit Marker yang sedang tampil.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:**

| Event | Kegunaan |
|---|---|
| `PlayerDamagedEvent` | Indikator arah |
| `EnemyAttackTelegraphEvent` | Penanda serangan dari luar pandangan (#109) |
| `EnemyHitEvent` | Hit Marker dan Kill Marker |
| `UIFocusChangedEvent` | Menyembunyikan `MarkerRoot` saat input diblokir |
| `CinematicStateChangedEvent` | Mematikan umpan balik selama adegan (#114) |
| `PlayerDiedEvent`, `RunEndedEvent` | Membersihkan semua indikator |

| Sistem | Hubungan |
|---|---|
| `DamagePipeline` (#59) | Sumber `DamageInfo` dan `EnemyHitEvent` |
| `HUDManager` | Pemilik `CrosshairRoot` |
| `AudioManager` | Memutar suara marker lewat `PlaySFX` |
| `ObjectPooler` | Pool `UI_DamageIndicator` |
| `FirstPersonCamera` (#102) | Arah pandang |
| `CameraShake` (#24), `PostProcessController` (#23) | Boleh mendengar event yang sama, dan tidak dipanggil dari sini |

## 4. Kasus Khusus

- **Pakai ulang dari pool:** `OnSpawned` mengatur `timer = 0`, alpha 0, warna merah, dan rotasi nol. `OnReturned` mengosongkan slot di `active`, sehingga `ObjectPooler.ReturnAllActive()` tidak meninggalkan referensi lama.
- **Game dijeda:** semua timer memakai `Time.deltaTime`, jadi indikator membeku dan berlanjut saat game kembali. Canvas panel jeda menutupinya.
- **Panel tanpa jeda:** saat Inventori terbuka, damage tetap berjalan. Busur tetap tampil, tetapi `MarkerRoot` disembunyikan bersama crosshair.
- **Pemain mati:** `ClearAll()` mengembalikan semua indikator ke pool dan mematikan marker, supaya `DeathCinematic` (#53) bersih.
- **Sumber kembali ke pool:** indikator memakai salinan posisi, jadi tidak ada referensi ke musuh yang sudah nonaktif.
- **Permadeath dan save:** tidak ada data yang disimpan. Saat load tidak ada indikator yang dipulihkan. `OnEnable` membaca `UIFocusChangedEvent.Last` kalau ada.
- **Rasio layar:** busur berputar pada lingkaran di tengah layar, dengan radius 45 persen sisi pendek, sehingga tetap terlihat di semua rasio.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `DamageInfo.hitDirection` untuk #51, damage kondisi tanpa `PlayerDamagedEvent` (#59), dan penanda serangan dari luar pandangan (#109).

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Hit Marker untuk semua serangan pemain.** Issue hanya menyebut peluru. Rekomendasi: berlaku juga untuk melee dan barang lempar, supaya umpan baliknya konsisten.
2. **Marker jebakan.** Rekomendasi: hanya tampil kalau titik kena berada dalam pandangan, tanpa mengubah `EnemyHitEvent`.
3. **Marker headshot lebih besar 1,3x.** Issue tidak memintanya. Rekomendasi: dipakai, karena headshot 2x perlu terasa berbeda.
4. **Pengaturan mematikan marker.** Rekomendasi: tambahkan `showHitMarkers` di `SettingsManager` (#19) saat #19 ditulis, dengan nilai awal aktif.

## 6. Skenario Tes

1. Biarkan zombie memukul dari kiri. **Hasil:** busur merah muncul di pinggir kiri, lalu memudar dalam sekitar 1,2 detik.
2. Menoleh 90 derajat ke kiri saat busur masih tampil. **Hasil:** busur bergeser ke arah atas layar, menunjuk sumber yang sama.
3. Serang dari belakang. **Hasil:** busur muncul di pinggir bawah.
4. Tiga zombie memukul dari arah yang hampir sama. **Hasil:** hanya satu busur, dan timernya diperbarui.
5. Enam belas pukulan dari segala arah. **Hasil:** paling banyak 6 busur, tanpa alokasi baru di Profiler.
6. Biarkan `Hunger` habis sampai HP turun. **Hasil:** tidak ada busur.
7. Tembak badan zombie. **Hasil:** marker putih selama 0,12 detik dan suara klik.
8. Tembak kepala tanpa membunuh. **Hasil:** marker putih lebih besar.
9. Bunuh zombie. **Hasil:** marker merah berkedip 2 kali, dan Tink terdengar sekali.
10. NPC membunuh zombie, atau zombie mati di jebakan di belakang pemain. **Hasil:** tidak ada marker.
11. Buka menu Pause saat busur tampil. **Hasil:** busur membeku, lalu berlanjut setelah menu ditutup.
12. Mati saat busur tampil, lalu mulai run baru. **Hasil:** tidak ada busur tersisa, dan jumlah pendengar tidak bertambah.

## Catatan Implementasi (Sistem pendukung)

Kode: `Assets/LOGIC/Core_Logic/CombatFeedbackUI.cs` dan `DamageIndicator.cs` (komponen prefab pool `UI_DamageIndicator`). Belum diuji di Unity.

- `playerCamera` kosong diisi dari `FirstPersonCamera.Instance.WorldCamera`. Posisi pemain untuk arah memakai posisi kamera.
- Penanda serangan (`EnemyAttackTelegraphEvent`) hanya memakai slot kosong. Kalau `active` penuh, penanda tidak tampil, supaya tidak menggantikan busur damage. Penggabungan sudut hanya berlaku antar-busur damage.
- Alpha memudar pada setengah akhir lama tampil masing-masing indikator (`indicatorSeconds` atau `telegraphSeconds`).
- `OnDisable` mengembalikan semua indikator ke pool, karena indikator adalah anak canvas HUD milik scene (aturan `IPoolable`: objek pool tidak boleh tertinggal sebagai anak objek scene).
- Setelah `PlayerDiedEvent` atau `RunEndedEvent`, `CinematicStateChangedEvent { isPlaying = false }` tidak menyalakan umpan balik lagi sampai scene diganti.
- Belum ada: `SettingsManager.showHitMarkers` (usulan 4). `SettingsManager` sudah ditulis tanpa field itu, jadi marker selalu aktif.
- Isi TODO (sistem pendukung): `SettingsManager.showHitMarkers` (usulan 4) sudah ada, nilai awal aktif, dengan toggle opsional `showHitMarkersToggle` di `SettingsPanel`. Tafsiran: pilihan ini mematikan Hit Marker dan Kill Marker beserta suaranya; indikator arah damage tetap tampil. Catatan "Belum ada: showHitMarkers" di atas tidak berlaku lagi.
