# Blueprint: InputManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/InputManager.md`
**Deskripsi Utama:** Satu pintu input memakai Unity New Input System. Mengatur pemetaan tombol, pemisahan Action Map `Gameplay` dan `UI`, serta penggantian tombol oleh pemain.
**Issue:** #18
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Tombol interaksi `E` dan klik kiri untuk item aktif mengikuti keputusan #85 di `GLOSSARY.md`.

## 1. Arsitektur & Variabel Utama

### Pola

`InputManager` adalah manager persisten (`IBootstrapService`, urutan ke-2 di #82). Pemetaan disimpan di aset `GameInput.inputactions`, dengan kelas `GameInputActions` hasil generate Unity.

- **Pengaturan proyek:** `Active Input Handling` bernilai `Input System Package (New)`. Pemanggilan `Input.GetKey` atau `Input.GetAxis` yang tertinggal langsung memunculkan error.
- **Update:** `InputSettings.updateMode` bernilai `ProcessEventsInDynamicUpdate`. Semua pembaca tombol memakai `Update`, bukan `FixedUpdate`.
- **Saat kehilangan fokus:** `InputSettings.backgroundBehavior` bernilai `ResetAndDisableNonBackgroundDevices`, supaya tombol tidak tersangkut setelah pindah jendela.
- **UI:** `EventSystem` persisten memakai `InputSystemUIInputModule`, yang terhubung ke Action Map `UI`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `InputManager` | `null` | Singleton |
| `actions` | `GameInputActions` | Dibuat di `Initialize()` | Semua Action Map |
| `rebindOperation` | `RebindingOperation` | `null` | Proses ganti tombol yang sedang berjalan |
| `BindingsPrefsKey` | `const string` | `settings.input.bindings` | Kunci `PlayerPrefs` untuk tombol pilihan pemain |

### Action Map `Gameplay`

Aktif saat tidak ada panel yang memblokir input.

| Action | Tipe | Tombol awal | Pemakai |
|---|---|---|---|
| `Move` | Value, Vector2 | `W`, `A`, `S`, `D` | `ShipPlayerController`, `IslandPlayerController` |
| `Look` | Value, Vector2 | Mouse delta | `FirstPersonCamera` (#102) |
| `Jump` | Button | `Space` | `IslandPlayerController` |
| `Sprint` | Button, ditahan | `Left Shift` | `IslandPlayerController` |
| `Crouch` | Button, ditahan | `Left Ctrl` | `PlayerTraversal` (#101) |
| `Interact` | Button | `E` | `PlayerInteractor` (#17) |
| `PrimaryAction` | Button | Klik kiri | Item aktif lewat `IEquippable` (#60). Di `ShipPhase`: `ShipShove`, lewat `ShipPlayerController` (#75) |
| `SecondaryAction` | Button | Klik kanan | Item aktif lewat `IEquippable` (#60) |
| `Reload` | Button | `R` | `GunController` (#58) |
| `BuildRotate` | Button | `R` | `BuildManager`, saat mode bangun |
| `HotbarSelect` | Value, Integer | `1` sampai `8`, jumlah final (usulan #60) | `EquipmentManager` (#60) |
| `HotbarScroll` | Value, Float | Scroll mouse | `EquipmentManager` (#60) |
| `Flashlight` | Button | `F` | `EquipmentManager.ToggleOffHand` (#60) |
| `Throw` | Button | `G` | `ThrowableSystem` (#96) |
| `DropHeavyItem` | Button | `Q` | `HeavyItemSystem` (#69) |
| `Struggle` | Button | `Space` | `PlayerPinnedState` (#110) |
| `Shove` | Button | `V` | `PlayerShove` (#95) |
| `OpenInventory` | Button | `Tab` | Pemilik panel `Inventory` |
| `OpenCrafting` | Button | `C` | Pemilik panel `Crafting` (#77) |
| `OpenBuildMenu` | Button | `B` | `BuildMenu` (#79) |
| `OpenDiary` | Button | `J` | `DiaryManager` |
| `Pause` | Button | `Esc` | Diteruskan ke `UIFocusStack.HandleEscape()` |

Klik kiri tidak dipakai untuk interaksi, sesuai keputusan #85.

### Action Map `UI`

Aktif saat ada panel yang memblokir input.

| Action | Tipe | Tombol awal | Pemakai |
|---|---|---|---|
| `Point` | Value, Vector2 | Posisi mouse | `InputSystemUIInputModule` |
| `Click` | Button | Klik kiri | `InputSystemUIInputModule` |
| `RightClick` | Button | Klik kanan | `InputSystemUIInputModule`, transfer cepat di #46 |
| `MiddleClick` | Button | Klik tengah | `InputSystemUIInputModule` |
| `ScrollWheel` | Value, Vector2 | Scroll mouse | `InputSystemUIInputModule` |
| `Navigate` | Value, Vector2 | Tombol panah | `InputSystemUIInputModule` |
| `Submit` | Button | `Enter` | `InputSystemUIInputModule` |
| `Cancel` | Button | `Esc` | Diteruskan ke `UIFocusStack.HandleEscape()` |
| `ToggleInventory` | Button | `Tab` | `toggleAction` panel `Inventory` |
| `ToggleCrafting` | Button | `C` | `toggleAction` panel `Crafting` |
| `ToggleBuildMenu` | Button | `B` | `toggleAction` panel `BuildMenu` |
| `ToggleDiary` | Button | `J` | `toggleAction` panel `Diary` |
| `Skip` | Button | `Space` | `NarrativeSequencer` (#99) |

### Action Map `Debug`

Hanya dibuat di Editor dan build pengembangan, dan selalu aktif.

| Action | Tipe | Tombol awal | Pemakai |
|---|---|---|---|
| `ToggleConsole` | Button | `` ` `` atau `~` | `DevConsole` (#56) |

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.input.rebindTimeoutSeconds` | 5 | Batas tunggu saat menekan tombol pengganti |

Sensitivitas mouse dan pilihan membalik sumbu Y disimpan di #19, dan diterapkan oleh #102.

### Tipe pendukung

- `GameInputAction`: enum berisi nama action yang dipakai di luar `InputManager`, misalnya `Interact` dan `Reload`. Fungsinya supaya #19 dan teks tombol tidak memakai string.
- `RebindResult`: struct hasil ganti tombol, berisi `status` (`Done`, `Canceled`, `Conflict`, atau `Rejected`) dan `conflictingAction`.

## 2. Detail Logika & Fungsi

### Akses dari sistem lain

Sistem gameplay membaca properti, lalu `InputManager` mengambil nilainya dari action. Cara ini tanpa garbage, dan tidak butuh langganan yang harus dilepas.

```csharp
public Vector2 MoveInput => actions.Gameplay.Move.ReadValue<Vector2>();
public Vector2 LookDelta => actions.Gameplay.Look.ReadValue<Vector2>();
public bool JumpPressed => actions.Gameplay.Jump.WasPressedThisFrame();
public bool SprintHeld => actions.Gameplay.Sprint.IsPressed();
public bool CrouchHeld => actions.Gameplay.Crouch.IsPressed();
public bool InteractPressed => actions.Gameplay.Interact.WasPressedThisFrame();
public bool InteractHeld => actions.Gameplay.Interact.IsPressed();
public bool PrimaryPressed => actions.Gameplay.PrimaryAction.WasPressedThisFrame();
public bool PrimaryHeld => actions.Gameplay.PrimaryAction.IsPressed();
public bool PrimaryReleased => actions.Gameplay.PrimaryAction.WasReleasedThisFrame();
public bool SecondaryPressed => actions.Gameplay.SecondaryAction.WasPressedThisFrame();
public bool SecondaryHeld => actions.Gameplay.SecondaryAction.IsPressed();
public bool SecondaryReleased => actions.Gameplay.SecondaryAction.WasReleasedThisFrame();
public bool ReloadPressed => actions.Gameplay.Reload.WasPressedThisFrame();
public bool FlashlightPressed => actions.Gameplay.Flashlight.WasPressedThisFrame();
public bool OpenDiaryPressed => actions.Gameplay.OpenDiary.WasPressedThisFrame();
public bool DropHeavyItemPressed => actions.Gameplay.DropHeavyItem.WasPressedThisFrame();
public bool SkipPressed => actions.UI.Skip.WasPressedThisFrame();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
public bool ConsoleTogglePressed => actions.Debug.ToggleConsole.WasPressedThisFrame();
#endif
```

Properti lain mengikuti pola yang sama untuk setiap action di tabel. Action yang dimatikan mengembalikan nilai kosong dan `false`, jadi pembaca tidak perlu mengecek menu terbuka.

**`LookDelta` tidak dikali `Time.deltaTime`,** karena nilai mouse delta sudah berupa jarak gerak per frame.

### Penggantian Action Map

`InputManager` berlangganan di `OnEnable` dan melepas di `OnDisable`:

```csharp
void OnEnable()
{
    EventChannel<UIFocusChangedEvent>.Raised += HandleFocus;
    if (actions != null)
    {
        ApplyMaps(EventChannel<UIFocusChangedEvent>.HasLast
            && EventChannel<UIFocusChangedEvent>.Last.isGameplayBlocked);
        BindEscape();
    }
}

void OnDisable()
{
    EventChannel<UIFocusChangedEvent>.Raised -= HandleFocus;
    if (actions != null) { UnbindEscape(); actions.Gameplay.Disable(); actions.UI.Disable(); }
}

void HandleFocus(UIFocusChangedEvent e) => ApplyMaps(e.isGameplayBlocked);

void ApplyMaps(bool gameplayBlocked)
{
    if (gameplayBlocked) { actions.Gameplay.Disable(); actions.UI.Enable(); }
    else { actions.UI.Disable(); actions.Gameplay.Enable(); }
}
```

- Hanya satu dari `Gameplay` dan `UI` yang aktif, jadi menekan `Esc` tidak dibaca dua kali.
- Action tipe Button tidak langsung aktif untuk tombol yang sudah tertekan saat map dinyalakan. Jadi `Esc` yang membuka Pause tidak langsung menutupnya. `UIFocusStack.HandleEscape()` juga punya pengaman per frame (#83).
- `InputManager` tidak mengatur kursor dan waktu. Keduanya milik `UIFocusStack`.

### Esc

`BindEscape()` memasang `HandleEscapePerformed` ke `Gameplay.Pause.performed` dan `UI.Cancel.performed`, lalu `UnbindEscape()` melepasnya. `HandleEscapePerformed` memanggil `UIFocusStack.Instance.HandleEscape()`. Ini satu-satunya tempat yang meneruskan Esc.

### Inisialisasi

**`public void Initialize()`**:

1. `actions = new GameInputActions()`.
2. `LoadBindingOverrides()`.
3. Hubungkan `actions.UI` ke `InputSystemUIInputModule`.
4. Di Editor dan build pengembangan, aktifkan `actions.Debug`.
5. Jalankan isi `OnEnable` untuk Esc dan Action Map, karena `OnEnable` pertama terjadi sebelum `actions` dibuat.

**`public void ResetForNewRun()`** membatalkan `rebindOperation` yang masih berjalan. Tombol pilihan pemain tidak ikut direset.

### Ganti tombol

Menu ada di #19. `InputManager` menyediakan fungsinya:

**`public void StartRebind(GameInputAction action, int bindingIndex, Action<RebindResult> onDone)`**

1. Tolak kalau action tidak bisa diganti. `Pause`, `Cancel`, `Submit`, `Point`, `Click`, dan `ToggleConsole` tetap tetap.
2. Matikan action tersebut selama proses.
3. Jalankan `PerformInteractiveRebinding(bindingIndex)` dengan `WithCancelingThrough("<Keyboard>/escape")`, `WithControlsExcluding("<Mouse>/position")`, `WithControlsExcluding("<Mouse>/delta")`, dan batas `rebindTimeoutSeconds`.
4. Saat selesai, periksa bentrok dengan `FindConflict`. Kalau bentrok, batalkan penggantian dan kirim hasil `Conflict` berisi action lawan. Menu #19 yang menanyakan pilihan pemain.
5. Simpan lewat `SaveBindingOverrides()`, nyalakan lagi action, buang `rebindOperation`, lalu terbitkan `InputBindingsChangedEvent`.

**`FindConflict`** hanya memeriksa action di map yang sama. Dua pasang ini boleh berbagi tombol, karena tidak pernah berlaku bersamaan:

| Pasangan | Alasan |
|---|---|
| `Jump` dan `Struggle` | Pemain tidak bisa melompat saat diterkam (#110) |
| `Reload` dan `BuildRotate` | Tergantung item aktif (#60) |

Action `Toggle*` di `UI` mengikuti tombol `Open*` di `Gameplay`. Keduanya diganti bersamaan oleh `StartRebind`, supaya tombol buka dan tutup tetap sama.

**`public void ResetBindings()`** memanggil `actions.RemoveAllBindingOverrides()`, menyimpan, lalu menerbitkan `InputBindingsChangedEvent`.

**`SaveBindingOverrides` dan `LoadBindingOverrides`** memakai `actions.SaveBindingOverridesAsJson()` dan `LoadBindingOverridesFromJson()`, dengan kunci `BindingsPrefsKey`. JSON yang rusak diabaikan dan tombol kembali ke nilai awal.

### Teks tombol di layar

**`public string GetBindingDisplay(GameInputAction action)`** mengembalikan nama tombol aktif, misalnya `E`. `HUDManager`, `ShipInteractable`, #16, #104, dan #110 memakai fungsi ini. Penulisan tombol langsung seperti `Press [E]` dilarang.

Hasilnya disimpan di cache per action, dan cache dibuang saat `InputBindingsChangedEvent` terkirim. Teks yang sudah tampil mendengar event itu supaya ikut berubah.

### Contoh pemakaian

```csharp
void Update()
{
    // Tidak perlu mengecek menu. Action otomatis mati saat panel memblokir input.
    if (InputManager.Instance.FlashlightPressed) ToggleOffHand();
}
```

### Kunci khusus di luar panel

Kunci yang berasal dari kondisi gameplay ada pada sistem pemilik kondisinya, bukan di `InputManager`. Contohnya `IslandPlayerController` mengabaikan `Sprint` dan `Jump` saat membawa Heavy Item, dan #110 mengunci gerakan saat pemain diterkam. Adegan memakai panel `Cinematic` (#83).

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `InputBindingsChangedEvent`.

**Event yang didengar:** `UIFocusChangedEvent`.

| Sistem | Hubungan |
|---|---|
| `UIFocusStack` (#83) | Menerima Esc dari `InputManager`, dan menentukan map lewat event. Issue ini mengatur tombol dan Action Map. Issue #83 mengatur panel yang punya fokus |
| `SettingsManager` (#19) | Menyediakan menu ganti tombol |
| `FirstPersonCamera` (#102) | Membaca `LookDelta` dan menerapkan sensitivitas |
| `EquipmentManager` (#60) | Membaca `PrimaryAction`, `SecondaryAction`, dan hotbar, lalu meneruskan ke item aktif |
| `PlayerInteractor` (#17) | Membaca `InteractPressed` dan `InteractHeld` |
| `ShipPlayerController`, `IslandPlayerController` | Mengganti `Input.GetAxis` dan `Input.GetKey`. `ShipPlayerController` tidak lagi membaca `Interact` |
| `FlashlightController` | Tombol `F` kini dibaca `EquipmentManager.ToggleOffHand` |
| `UIManager`, `DiaryManager` | Tidak lagi membaca `Escape` |
| `DevConsole` (#56) | Membaca `Debug/ToggleConsole`, lalu `UIFocusStack.Push` |
| `LocalizationManager` (#104) | Memakai `GetBindingDisplay` dalam teks |

## 4. Kasus Khusus

- **Objek pool:** tidak menyimpan referensi ke action dan tidak berlangganan callback input. Objek yang butuh input membaca properti `InputManager`, jadi tidak ada status yang perlu direset.
- **Game dijeda:** Action Map `UI` tetap menerima input, karena Input System tidak mengikuti `Time.timeScale`. `Gameplay` mati selama panel menjeda.
- **Permadeath:** tombol pilihan pemain tidak dihapus, karena disimpan di `PlayerPrefs` terpisah dari save run. `RunLifecycle` tidak boleh memakai `PlayerPrefs.DeleteAll()` (#82).
- **Save dan load:** tidak ada data di save run.
- **Reset di #82:** `managersRoot.SetActive(false)` memanggil `OnDisable`, yang melepas langganan dan mematikan map. `SetActive(true)` memanggil `OnEnable`. Karena nilai sticky sudah dihapus, map kembali ke `Gameplay` sampai `UIFocusStack` menerbitkan event barunya pada urutan yang sama.
- **Mengetik di konsol atau kolom teks:** `Gameplay` mati karena panel memblokir input, jadi `E` dan `Tab` hanya masuk ke kolom teks.
- **Tombol ditahan saat panel terbuka:** `Gameplay.Disable()` membatalkan action. `SprintHeld` bernilai `false` sampai tombol ditekan lagi setelah panel tertutup.
- **Lanjut setelah Alt-Tab:** `backgroundBehavior` mereset perangkat, dan #83 membuka Pause.
- **Mouse melonjak setelah fokus kembali:** `FirstPersonCamera` mengabaikan `LookDelta` pada frame pertama setelah `Gameplay` aktif kembali.
- **Bentrok tombol antar-map:** diperbolehkan, karena `Gameplay` dan `UI` tidak aktif bersamaan.
- **Fase 1:** memakai map yang sama. Action yang tidak dipakai di kapal tidak punya pembaca, jadi tidak berpengaruh. Keputusan #75: Pistol dan Pisau tidak dipakai di kapal, dan `PrimaryPressed` menjadi aksi dorong `ShipShove`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `E` untuk interaksi di dua fase, dan klik kiri untuk item aktif (`GLOSSARY.md`). `~` membuka konsol (#56). `F` untuk senter (`FlashlightController.md`, tertulis sebagai contoh).

**Keputusan teknis di blueprint ini:**

- Sistem gameplay memakai pembacaan properti, bukan callback, supaya tidak ada langganan tambahan.
- Hanya Esc yang memakai callback, karena harus diteruskan ke satu pengatur.
- `Debug` dipisah supaya tidak ikut ke build rilis.

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Tombol awal selain `E` dan klik kiri:** mengikuti tabel Action Map.
2. **Gamepad:** versi awal hanya memakai skema `KeyboardMouse`. Aset disiapkan agar skema `Gamepad` bisa ditambah nanti, tanpa mengubah pembaca tombol.
3. **Lari dan jongkok:** ditahan, bukan toggle. Pilihan toggle bisa ditambah lewat #19.
4. **Ganti tombol:** semua tombol boleh diganti, termasuk `E`, kecuali daftar tombol tetap. `E` tetap nilai awalnya.

## 6. Skenario Tes

1. Jalan, menengok, lari, dan melompat di Fase 2. **Hasil:** semua action terbaca, dan tidak ada error `Input` lama di Console.
2. Buka inventori dengan `Tab`, lalu klik kiri pada slot. **Hasil:** hanya UI yang merespons, dan senjata tidak menembak.
3. Tutup inventori dengan `Tab`. **Hasil:** `Gameplay` aktif kembali, dan gerakan normal.
4. Tahan `W` dan `Left Shift`, buka panel, lalu tutup sambil tetap menahan. **Hasil:** pemain tidak lari sebelum tombol ditekan ulang.
5. Tekan `Esc` saat bermain. **Hasil:** Pause terbuka sekali dan tidak langsung tertutup.
6. Ganti `Interact` ke `T`, tutup game, lalu buka lagi. **Hasil:** `T` tetap berlaku, dan prompt menampilkan `T`.
7. Ganti `Reload` ke `Tab`. **Hasil:** bentrok dengan `OpenInventory` dilaporkan, dan tombol lama tetap berlaku.
8. Ganti `OpenInventory` ke `I`. **Hasil:** `I` membuka dan menutup inventori.
9. Mati, lalu mulai run baru. **Hasil:** tombol pilihan tetap ada.
10. Klik Reset Tombol di #19. **Hasil:** semua tombol kembali ke nilai awal, dan prompt ikut berubah.
11. Tekan `~` di Editor. **Hasil:** konsol terbuka, dan mengetik `e` tidak memicu interaksi.
12. Buat build rilis lalu tekan `~`. **Hasil:** tidak terjadi apa-apa.
13. Rusak isi kunci `settings.input.bindings` secara manual. **Hasil:** game memakai tombol awal tanpa error fatal.
14. Buka Profiler dan gerakkan pemain selama 60 detik. **Hasil:** pembacaan properti `InputManager` tidak menghasilkan alokasi GC.
