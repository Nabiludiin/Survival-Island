# Blueprint: HUDManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/HUDManager.md`
**Deskripsi Utama:** Menampilkan informasi *real-time* kepada pemain (darah, stamina, lapar, haus, infeksi, peluru, hotbar, tujuan, dan penanda Fase 1) di layar. HUD hanya mendengar event atau membaca data.
**Issue:** #18, #17, #51, #58, #60, #74, #81, #96, #111
**Rujukan GDD:** Bagian 3 (status karakter dan timer 60 detik Fase 1). Tata letak dan tanda tambahan adalah usulan.

## 1. Arsitektur & Variabel Utama

`HUDManager` berada di scene `ShipPhase` dan `IslandPhase` (#82). `Awake` hanya mengisi `Instance`.

| Variabel | Tipe | Fungsi |
|---|---|---|
| `Instance` | `HUDManager` | Singleton per scene |
| `healthBar`, `staminaBar`, `hungerBar`, `thirstBar`, `infectionBar` | `Slider` | Status pemain |
| `ammoText` | `TextMeshProUGUI` | Isi magazine dan cadangan |
| `interactionPromptText` | `TextMeshProUGUI` | Teks dari `PlayerInteractor` |
| `interactionProgress` | `Image` | Cincin kemajuan interaksi tahan |
| `crosshairRoot` | `RectTransform` | Pemilik crosshair. `MarkerRoot` milik `CombatFeedbackUI` (#51) menjadi anaknya, sehingga ikut tersembunyi |
| `hotbarView` | `HotbarView` | Tampilan hotbar (#60) |
| `throwableIcon`, `throwableCount` | `Image`, `TextMeshProUGUI` | Barang lempar di dekat peluru (#96) |
| `objectiveTitle`, `objectiveProgress`, `daysLeftText` | `TextMeshProUGUI` | Tujuan aktif dan sisa hari (#81) |
| `partsText`, `engineText` | `TextMeshProUGUI` | Suku cadang terpasang dan hitung mundur mesin helikopter (#14) |
| `luckyFreeText` | `TextMeshProUGUI` | Hitung mundur mode gratis Lucky Machine (#72) |
| `luckyFreeRemaining` | `float` | Sisa detik yang dihitung HUD sendiri dengan `Time.deltaTime` (#72) |
| `breathBar`, `warmthBar` | `Slider` | Napas di air (#13) dan suhu (#65) |
| `craftQueueIcon` | `Image` | Antrean crafting yang berjalan saat panel tertutup (#77) |
| `statusEffectBar` | `StatusEffectBar` | Ikon efek status (#20) |
| `loadingAreaText` | `TextMeshProUGUI` | Pesan `Memuat area` saat `RegionStreaming.IsStalled` (#88) |
| `shipHandWeight`, `lifeboatMarker`, `safeZoneLine` | Komponen UI | Hanya aktif di `ShipPhase` (#74) |

## 2. Detail Logika & Fungsi

| Event | Tindakan |
|---|---|
| `VitalsChangedEvent` | Ubah bar status. `infectionBar` lebih merah saat infeksi di atas 80. Fungsi `UpdateVitals` tidak lagi dipanggil langsung (#111) |
| `VitalWarningChangedEvent` | Tampilkan tanda peringatan status (#111) |
| `AmmoChangedEvent` | Panggil `UpdateAmmo(inMagazine, reserve)`. Baca `HasLast` dan `Last` di `OnEnable` |
| `HotbarChangedEvent`, `EquippedItemChangedEvent`, `InventoryChangedEvent` | Perbarui `HotbarView` |
| `InputBindingsChangedEvent` | Minta ulang teks prompt yang sedang tampil |
| `ThrowableChangedEvent` | Tampilkan ikon dan jumlah barang lempar |
| `ActiveObjectiveChangedEvent` | Tampilkan judul dan progres, dengan sorotan selama `objective.hudFlashSeconds` |
| `DayChangedEvent` | Sampai hari ke-60: `Sisa hari = GameManager.TargetDay - newDay + 1`. Setelah itu: `Target hari ke-60 terlewat`. `GameManager.PublishDayState` menerbitkan event ini saat tiba di pulau (hari 1) dan setelah restore (`isReplay = true`), jadi teks langsung terisi. HUD adalah pendengar tampilan, jadi tidak mengabaikan `isReplay`. `OnEnable` juga membaca `HasLast` dan `Last` |
| `DayLimitPassedEvent` | Tulis `Target hari ke-60 terlewat`. Saat `OnEnable` setelah load, baca `GameManager.IsPastTargetDay` |
| `ShipHandChangedEvent` | Tampilkan berat tangan di `ShipPhase` |
| `HelicopterPartInstalledEvent` | Tulis `Suku cadang installed / total` (#14, #69). Sticky, dibaca di `OnEnable` |
| `HelicopterEngineChangedEvent` | Saat `WarmingUp`, tulis `Mesin memanas m:ss` dari `secondsLeft`. State lain menyembunyikan teks (#14) |
| `LuckyMachineFreeWindowChangedEvent` | `isActive`: isi `luckyFreeRemaining = seconds`, lalu kurangi dengan `Time.deltaTime` dan tampilkan. Dua mesin aktif bersamaan tetap satu hitung mundur, karena dimulai dari bencana yang sama. Teks disembunyikan saat `luckyFreeRemaining <= 0` (#72) |
| `SwimStateChangedEvent` | Tampilkan `breathBar` dari `breathRatio` selama `breathRatio < 1` (#13) |
| `WarmthChangedEvent` | `warmthBar` tampil kalau `Warmth < 100` atau region aktif `Snow`. Biru muda di `<= 50`, biru tua berkedip saat `isFreezing` (#65) |
| `CraftQueueChangedEvent` | Tampilkan `craftQueueIcon` selama `queueCount > 0` (#77) |
| `StatusEffectChangedEvent` | Perbarui `statusEffectBar`, aturannya di `StatusEffectManager.md` (#20) |
| `UIFocusChangedEvent` | Sembunyikan crosshair selama `isGameplayBlocked` (#83) |

`loadingAreaText` membaca `RegionStreaming.Instance.IsStalled` 4 kali per detik, karena tidak ada event untuk status ini (#88). Cincin meronta milik `PinnedPromptUI` (#110), bukan sistem ini.

**`public void UpdateAmmo(int inMagazine, int reserve)`**: tulis `inMagazine / reserve`. Saat keduanya 0, teks berkedip.

**`public void ShowInteraction(string message)`** dan **`HideInteraction()`**: menerima teks jadi dari `PlayerInteractor` (#17). Teks memakai `InputManager.Instance.GetBindingDisplay(GameInputAction.Interact)`, bukan teks tombol tetap.

**`public void SetInteractionProgress(float value)`**: mengisi cincin interaksi tahan, 0 sampai 1.

**Fase 1:** penanda sekoci dan garis area aman ditampilkan sesuai status dari `LifeboatExtraction` (#74).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:** semua event di tabel bagian 2.

| Sistem | Hubungan |
|---|---|
| `PlayerInteractor` (#17) | Memanggil `ShowInteraction`, `HideInteraction`, dan `SetInteractionProgress` |
| `InputManager` (#18) | Sumber `GetBindingDisplay` |
| `CombatFeedbackUI` (#51) | `MarkerRoot` menjadi anak `crosshairRoot` |
| `LifeboatExtraction` (#74) | Sumber status penanda Fase 1 |
| `GameManager` | `PublishDayState` menerbitkan `DayChangedEvent` saat tiba di pulau dan setelah restore |
| `RescueEventManager` (#14), `LuckyMachine` (#72) | Sumber event suku cadang, mesin, dan mode gratis |
| `SwimmingController` (#13), `WarmthSystem` (#65), `StatusEffectManager` (#20), `CraftingManager` (#77) | Sumber bar napas, suhu, ikon efek, dan ikon antrean |
| `RegionStreaming` (#88) | `IsStalled` untuk pesan `Memuat area` |

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool.
- **Game dijeda:** event tetap diterima. Animasi sorotan dan kedipan memakai waktu berskala, jadi berhenti saat jeda.
- **Permadeath:** HUD hilang bersama scene.
- **Save dan load:** tidak disimpan. Nilainya kembali lewat event sticky di `OnRestoreFinished`.
- **Aktif terlambat:** event sticky dibaca lewat `HasLast` dan `Last` di `OnEnable`.
- **Crosshair disembunyikan:** `MarkerRoot` ikut tersembunyi karena menjadi anak `crosshairRoot`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** teks prompt (`InputManager.md`, `IslandInteractable.md`). UI hanya membaca (`GameEventBus.md`). Kedip peluru (`GunController.md`). Rumus sisa hari (`ObjectiveManager.md`).

**Menunggu developer:** tata letak HUD final dan warna tanda peringatan adalah usulan.

## 6. Skenario Tes

1. Turunkan lapar lewat `setvital`. **Hasil:** bar berubah lewat `VitalsChangedEvent`, tanpa pemanggilan `UpdateVitals`.
2. Ganti `Interact` ke `T`, lalu arahkan ke objek. **Hasil:** prompt menampilkan `T`.
3. Tahan `E` pada objek interaksi tahan. **Hasil:** cincin terisi.
4. Habiskan peluru. **Hasil:** teks berkedip saat magazine dan cadangan 0.
5. Ganti item hotbar. **Hasil:** `HotbarView` berubah.
6. Ambil barang di kapal. **Hasil:** berat tangan berubah, dan penanda sekoci tampil.
7. Sembunyikan crosshair. **Hasil:** penanda serangan ikut tersembunyi.
8. Pada hari ke-10 dengan target 60. **Hasil:** Sisa hari tertulis 51.
9. Pada hari ke-61 dan ke-100. **Hasil:** teks `Target hari ke-60 terlewat`, tanpa angka negatif.
10. Tiba di pulau pada run baru. **Hasil:** `Sisa hari: 60` langsung tampil, tanpa menunggu tengah malam.
11. Simpan pada hari ke-12, keluar, lalu lanjutkan. **Hasil:** `Sisa hari: 49` langsung tampil setelah load.
12. Pasang satu suku cadang, lalu nyalakan mesin. **Hasil:** `Suku cadang 1 / 5`, lalu hitung mundur `Mesin memanas` saat pemanasan.
13. Picu `MajorDisasterStartedEvent` di dekat Lucky Machine. **Hasil:** hitung mundur 30 detik tampil dan berhenti saat jeda.
14. Menyelam, lalu masuk Region 3. **Hasil:** bar napas tampil saat `breathRatio < 1`, dan bar suhu tampil di Region 3.
