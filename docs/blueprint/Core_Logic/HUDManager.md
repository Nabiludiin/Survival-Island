# Blueprint: HUDManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/HUDManager.md`
**Deskripsi Utama:** Menampilkan informasi *real-time* kepada pemain (darah, stamina, lapar, haus, infeksi, peluru, hotbar, tujuan, dan penanda Fase 1) di layar. HUD hanya mendengar event atau membaca data.
**Issue:** #18, #17, #51, #58, #60, #74, #81, #96, #111
**Rujukan GDD:** Bagian 3 (status karakter) dan bagian 1 (timer Fase 1). Tata letak dan tanda tambahan adalah usulan.

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
| `DayChangedEvent` | `Sisa hari = time.runDayLimit - newDay + 1` |
| `ShipHandChangedEvent` | Tampilkan berat tangan di `ShipPhase` |

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
8. Pada hari ke-10 dengan batas 60. **Hasil:** Sisa hari tertulis 51.
