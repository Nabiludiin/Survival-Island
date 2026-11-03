# Blueprint: UIFocusStack.cs
**Lokasi:** `docs/blueprint/Core_Logic/UIFocusStack.md`
**Deskripsi Utama:** Satu pengatur fokus UI, tombol Esc, kursor, audio jeda, dan `Time.timeScale`. Panel lain hanya meminta dibuka atau ditutup, dan tidak mengubah nilai-nilai itu sendiri.
**Issue:** #83
**Rujukan GDD:** Usulan arsitektur, belum ada di GDD. Mendukung Diary di bagian 4 dan Game Over di bagian 3. Jeda saat Diary mengikuti `Claude.md` Tahap 5.

## 1. Arsitektur & Variabel Utama

### Pola

`UIFocusStack` adalah manager persisten (`IBootstrapService`, urutan ke-6 di #82). Stack disimpan sebagai `List<UIPanel>`, karena sistem lain bisa menutup panel yang tidak berada di puncak.

```csharp
public enum UIPanelId
{
    MainMenu, Loading, Cinematic, Pause, Settings, ConfirmDialog, RunSummary,
    Diary, LoreReader, Inventory, Crafting, Storage, Campfire, BuildMenu,
    WorldMap, LuckyMachine, GameOver, WinScreen, DevConsole
}

public enum UICloseReason { User, Forced, RunEnded, SceneUnloaded }

[Serializable]
public struct UIPanelSettings
{
    public bool pausesTime;          // Membuat Time.timeScale = 0
    public bool releasesCursor;      // Melepas dan menampilkan kursor
    public bool blocksGameplayInput; // Mematikan Action Map Gameplay (#18)
    public bool closableByEsc;       // Esc boleh menutup panel ini
    public bool isLocked;            // Tidak ikut tertutup oleh CloseAllUnlocked
    public bool rejectsNewPanels;    // Selama di puncak, panel lain tidak bisa masuk
}

public abstract class UIPanel : MonoBehaviour
{
    public UIPanelId id;
    public UIPanelSettings settings;
    public InputActionReference toggleAction;     // Tombol yang juga menutup panel, boleh kosong

    protected abstract void Show();
    protected abstract void Hide();
    public virtual void OnFocusGained() { }       // Menjadi puncak stack
    public virtual void OnFocusLost() { }         // Tertutup panel baru
    public virtual void OnClosed(UICloseReason reason) { }  // Mengembalikan barang yang dipegang, dll.
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `stack` | `List<UIPanel>` | Kosong, kapasitas 8 | Panel terbuka. Indeks terakhir adalah puncak |
| `gameplayTimeScale` | `float` | 1 | Skala waktu saat tidak dijeda |
| `IsPaused` | `bool` | `false` | Ada panel yang menjeda |
| `IsGameplayInputBlocked` | `bool` | `false` | Ada panel yang memblokir input gameplay |
| `TopPanel` | `UIPanel` | `null` | Puncak stack |
| `pausePanel` | `UIPanel` | Dari `UIManager` | Panel yang dibuka Esc saat stack kosong |

### Pengaturan tiap panel

| Panel | Menjeda waktu | Lepas kursor | Blokir input | Tutup dengan Esc | Terkunci | Tolak panel baru |
|---|---|---|---|---|---|---|
| `MainMenu` | Tidak | Ya | Ya | Tidak | Ya | Tidak |
| `Loading` | Ya | Tidak | Ya | Tidak | Ya | Ya |
| `Cinematic` | Tidak | Tidak | Ya | Tidak | Ya | Ya |
| `Pause` | Ya | Ya | Ya | Ya | Tidak | Tidak |
| `Settings` | Ya | Ya | Ya | Ya | Tidak | Tidak |
| `ConfirmDialog` | Ya | Ya | Ya | Ya | Tidak | Tidak |
| `RunSummary` (#74) | Ya | Ya | Ya | Tidak | Ya | Tidak |
| `Diary` | Ya | Ya | Ya | Ya | Tidak | Tidak |
| `LoreReader` (#100) | Ya | Ya | Ya | Ya | Tidak | Tidak |
| `Inventory` | Tidak | Ya | Ya | Ya | Tidak | Tidak |
| `Crafting` (#77) | Tidak | Ya | Ya | Ya | Tidak | Tidak |
| `Storage` (#46) | Tidak | Ya | Ya | Ya | Tidak | Tidak |
| `Campfire` (#44) | Tidak | Ya | Ya | Ya | Tidak | Tidak |
| `BuildMenu` (#79) | Tidak | Ya | Ya | Ya | Tidak | Tidak |
| `WorldMap` (#105, nonaktif) | Tidak | Ya | Ya | Ya | Tidak | Tidak |
| `LuckyMachine` (#72) | Tidak | Ya | Ya | Ya | Tidak | Tidak |
| `GameOver` | Ya | Ya | Ya | Tidak | Ya | Ya |
| `WinScreen` | Ya | Ya | Ya | Tidak | Ya | Ya |
| `DevConsole` (#56) | Ya | Ya | Ya | Ya | Tidak | Tidak |

**Alasan pembagian jeda:** `Claude.md` menjeda Diary, jadi panel baca ikut menjeda. Inventori, crafting, peti, api unggun, dan menu bangun berjalan tanpa jeda, supaya pemain tetap merasakan tekanan survival dan tidak bisa menyusun barang dengan aman di tengah serangan. `LuckyMachine` tidak menjeda, karena GDD bagian 7 memberi jatah 30 detik saat bencana.

`DevConsole` boleh masuk walaupun `Cinematic`, `GameOver`, dan `Loading` berada di puncak. Ini satu-satunya pengecualian `rejectsNewPanels`, dan hanya ada di build pengembangan.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.ui.panelFadeSeconds` | 0,15 | Lama fade panel, dengan waktu tak terskala |
| `GameBalanceConfig.ui.pausedMusicVolumeDb` | -12 | Musik saat dijeda |
| `GameBalanceConfig.ui.pauseAudioFadeSeconds` | 0,2 | Lama transisi audio jeda |

## 2. Detail Logika & Fungsi

### Fungsi utama

**`public bool Push(UIPanel panel)`**

1. Kalau `panel` sudah ada di stack, kembalikan `false` tanpa perubahan.
2. Kalau `TopPanel.settings.rejectsNewPanels` bernilai `true`, kembalikan `false`. Pengecualian hanya untuk `DevConsole`.
3. Panggil `TopPanel.OnFocusLost()`, kalau ada.
4. Tambahkan `panel`, catat `openedFrame = Time.frameCount`, lalu panggil `panel.Show()` dan `panel.OnFocusGained()`.
5. Panggil `ApplyState()`.

**`public bool Close(UIPanel panel, UICloseReason reason)`**

1. Cari `panel`. Kalau tidak ada, kembalikan `false`.
2. Catat apakah `panel` adalah puncak, lalu hapus dari stack.
3. Panggil `panel.Hide()` dan `panel.OnClosed(reason)`.
4. Kalau `panel` tadi puncak dan masih ada panel lain, panggil `OnFocusGained()` pada puncak baru.
5. Panggil `ApplyState()`.

**`public bool Pop(UICloseReason reason = UICloseReason.User)`** menutup panel teratas lewat `Close(TopPanel, reason)`, dan mengembalikan `false` kalau stack kosong.

**`public void CloseAllUnlocked(UICloseReason reason)`** menutup semua panel dengan `isLocked == false`, dari puncak ke bawah.

**`public void ClearForNewRun()`** menutup semua panel termasuk yang terkunci, dengan alasan `Forced`, lalu mengatur `gameplayTimeScale = 1`. Hanya dipanggil `RunLifecycle` (#82).

**`public void SetGameplayTimeScale(float value)`** dipakai #53 untuk gerak lambat dan #56 untuk perintah konsol. Nilainya hanya berlaku saat tidak dijeda.

### Esc hanya menutup panel teratas

`InputManager` (#18) meneruskan `Gameplay/Pause` dan `UI/Cancel` ke satu fungsi. Karena hanya satu Action Map aktif, satu tekanan tidak mengenai dua pembaca.

**`public void HandleEscape()`**

1. Kalau `Time.frameCount == lastEscapeFrame`, abaikan. Lalu simpan `lastEscapeFrame`.
2. Kalau stack kosong dan `GameManager.Instance.IsRunEnding` bernilai `false`, panggil `Push(pausePanel)`.
3. Kalau stack tidak kosong dan `TopPanel.settings.closableByEsc` bernilai `true`, panggil `Close(TopPanel, UICloseReason.User)`.
4. Selain itu, abaikan.

`UIManager` dan `DiaryManager` tidak lagi membaca Esc. `TogglePause()` dihapus, jadi Game Over tidak bisa dibatalkan lewat jalur itu.

### Tombol buka dan tutup yang sama

Tombol seperti `Tab` berada di dua Action Map (#18). `Gameplay/OpenInventory` dibaca pemilik panel untuk membuka panel. `UI/ToggleInventory` dibaca `UIFocusStack.Update()`:

```csharp
void Update()
{
    var top = TopPanel;
    if (top == null || top.toggleAction == null) return;
    if (Time.frameCount != openedFrame && top.toggleAction.action.WasPressedThisFrame())
        Close(top, UICloseReason.User);
}
```

### Satu tempat menulis waktu, kursor, dan audio

**`private void ApplyState()`** adalah satu-satunya penulis `Time.timeScale`, `Cursor.lockState`, `Cursor.visible`, dan `AudioListener.pause`:

```csharp
void ApplyState()
{
    bool paused = false, blocked = false, cursorFree = false;
    for (int i = 0; i < stack.Count; i++)
    {
        var s = stack[i].settings;
        paused |= s.pausesTime;
        blocked |= s.blocksGameplayInput;
        cursorFree |= s.releasesCursor;
    }

    Time.timeScale = paused ? 0f : gameplayTimeScale;
    Cursor.lockState = cursorFree ? CursorLockMode.None : CursorLockMode.Locked;
    Cursor.visible = cursorFree;
    AudioListener.pause = paused;
    AudioManager.Instance.SetPausedMix(paused);

    bool changed = paused != IsPaused || blocked != IsGameplayInputBlocked;
    IsPaused = paused;
    IsGameplayInputBlocked = blocked;
    if (changed) EventChannel<UIFocusChangedEvent>.Publish(new UIFocusChangedEvent(blocked, paused));
}
```

Karena nilainya dihitung dari seluruh isi stack, menutup Diary saat Pause masih terbuka tidak menjalankan waktu kembali.

`OnEnable` milik `UIFocusStack` menerbitkan event ini sekali dengan status saat ini, karena `GameEventRegistry.ClearAll()` (#82) menghapus nilai sticky-nya.

### Tanda input gameplay

Ada dua lapis pengaman:

| Lapis | Cara kerja | Pemakai |
|---|---|---|
| **Action Map** | `InputManager` mendengar `UIFocusChangedEvent` dan mematikan `Gameplay` | Semua pembaca tombol. Tombol gameplay otomatis tidak terbaca |
| **`IsGameplayInputBlocked`** | Dibaca langsung atau lewat event | `PlayerInteractor` (prompt dan raycast), `EquipmentManager` (kunci ganti item, #60), `BuildManager` (sembunyikan hologram), dan `HUDManager` (sembunyikan crosshair) |

### Animasi UI saat game dijeda

| Komponen | Aturan |
|---|---|
| `Animator` di panel | `updateMode = AnimatorUpdateMode.UnscaledTime` |
| Fade dan gerak buatan kode | `Time.unscaledDeltaTime` |
| Coroutine UI | `WaitForSecondsRealtime`, bukan `WaitForSeconds` |
| `ParticleSystem` di UI | `main.useUnscaledTime = true` |
| Bar loading | `Time.unscaledDeltaTime` (#82) |

Elemen HUD yang bukan panel boleh memakai waktu biasa, karena HUD memang ikut berhenti saat dijeda. Waktu berhenti tepat saat `Push`, tanpa menunggu fade.

### Audio saat game dijeda

| Jenis suara | Saat dijeda | Cara |
|---|---|---|
| SFX dunia, ambience, dan suara musuh | Berhenti, lalu lanjut dari posisi yang sama | `AudioListener.pause = true` |
| Musik latar | Tetap main, dengan volume turun | `bgmSource.ignoreListenerPause = true`, dan snapshot `AudioMixer` |
| SFX tombol UI | Tetap bunyi | `sfxSource.ignoreListenerPause = true` |

**`AudioManager.SetPausedMix(bool paused)`** beralih antara snapshot `Normal` dan `Paused` selama `pauseAudioFadeSeconds`, dengan `AudioMixer.updateMode = UnscaledTime`. Coroutine `CrossfadeBGM` juga memakai `Time.unscaledDeltaTime`, supaya musik tidak tersangkut di tengah transisi. #114 mengikuti aturan ini.

### Panel tertutup oleh sistem lain

| Kejadian | Tindakan | Hasil |
|---|---|---|
| Pemain mati saat `Inventory` terbuka | `RunLifecycle` memanggil `CloseAllUnlocked(RunEnded)`, lalu mendorong `Cinematic` | `OnClosed` mengembalikan barang yang sedang digeser. Fokus berpindah ke `Cinematic` |
| Pemain diterkam (`PlayerPinnedEvent`, `isPinned = true`) | `UIFocusStack` memanggil `CloseAllUnlocked(Forced)` | Input gameplay kembali untuk meronta (#110) |
| Scene diganti saat panel scene masih terbuka | `UIPanel.OnDestroy` memanggil `Close(this, SceneUnloaded)`. `OnDisable` tidak dipakai, karena reset di #82 mematikan sebentar panel persisten seperti `Loading` | Tidak ada referensi mati di stack |
| Panel di tengah stack ditutup sistem lain | `Close` menghapusnya tanpa menyentuh puncak | Puncak tidak menerima `OnFocusGained` berulang |
| Pemain terkena serangan saat `Inventory` terbuka | Panel tetap terbuka | Pemain menutupnya sendiri |

### Panel tidak boleh mengubah hal lain

Panel hanya memanggil `Push` dan `Close`. Pemeriksaan teks `Time.timeScale`, `Cursor.lockState`, dan `Cursor.visible` di luar `UIFocusStack` menjadi bagian dari review. `SetGameplayTimeScale` menjadi jalur pengubahan satu-satunya untuk efek lain.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `UIFocusChangedEvent` (`isGameplayBlocked`, `isPaused`), sticky.

**Event yang didengar:** `PlayerPinnedEvent`.

| Sistem | Hubungan |
|---|---|
| `InputManager` (#18) | Memanggil `HandleEscape()`, dan mengganti Action Map dari `UIFocusChangedEvent` |
| `RunLifecycle` (#82) | Memakai `Push`, `Close`, `CloseAllUnlocked`, dan `ClearForNewRun` |
| `UIManager` | Memiliki panel persisten `Pause`, `Loading`, `Cinematic`, `GameOver`, dan `WinScreen`. Tidak lagi mengatur waktu, kursor, atau Esc |
| `DiaryManager` | `OpenDiary` memanggil `Push(diaryPanel)`, dan tombol Close memanggil `Close`. Variabel `isReading` dan pembacaan Esc dihapus |
| `AudioManager` | Menyediakan `SetPausedMix` |
| #44, #46, #72, #77, #79, #100, #105 | Panelnya mewarisi `UIPanel` |
| #53, #56 | Memakai `SetGameplayTimeScale` |
| #99 | Memakai panel `Cinematic` untuk adegan |

## 4. Kasus Khusus

- **Objek pool:** tidak berhubungan langsung. Objek pool tidak boleh menjadi `UIPanel`.
- **Game dijeda:** `UIFocusStack.Update()` tetap berjalan, karena `Update` tidak bergantung pada `Time.timeScale`. Hanya fisika, coroutine berskala, dan `Time.deltaTime` yang berhenti.
- **Permadeath:** `ClearForNewRun()` mengosongkan stack dan mengembalikan `gameplayTimeScale`. Tidak ada data yang tertinggal di run baru.
- **Save dan load:** stack tidak disimpan. Run yang dimuat selalu mulai dari stack kosong, setelah panel `Loading` ditutup.
- **Dua panel dibuka pada frame yang sama:** `Push` memprosesnya berurutan. Yang kedua menjadi puncak, kecuali ditolak `rejectsNewPanels`.
- **Panel sama dibuka dua kali:** `Push` kedua diabaikan.
- **Tombol toggle pada frame pembukaan:** `openedFrame` mencegah panel langsung tertutup.
- **Klik tombol Close lalu menembak:** klik UI selesai saat tombol mouse dilepas, sehingga `PrimaryAction.WasPressedThisFrame` tidak aktif saat `Gameplay` kembali menyala.
- **Esc di `Settings` di atas `Pause`:** hanya `Settings` yang tertutup. Waktu tetap berhenti.
- **Game kehilangan fokus jendela:** `OnApplicationFocus(false)` mendorong `pausePanel` kalau stack kosong dan pemain sedang bermain. `OnApplicationFocus(true)` memanggil `ApplyState()`, supaya kursor terkunci kembali.
- **Esc saat `RunEnding`:** stack kosong tetapi run sedang berakhir, jadi Pause tidak dibuka. Kondisi ini hanya terjadi sekejap sebelum `Cinematic` masuk.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** Diary menjeda game (`Claude.md` Tahap 5). `GameOver` tidak bisa ditutup Esc (checklist issue ini).

**Keputusan teknis di blueprint ini:**

- Stack memakai `List`, supaya panel di tengah bisa ditutup sistem lain.
- Waktu dan kursor dihitung dari seluruh isi stack, bukan hanya puncak.
- `Loading` menjeda waktu, jadi dunia tidak berjalan saat pemain belum bisa bergerak.

**Usulan yang menunggu developer (dicatat di #85):**

1. **Panel yang menjeda:** `Pause`, `Settings`, `ConfirmDialog`, `RunSummary`, `Diary`, `LoreReader`, `GameOver`, `WinScreen`, `Loading`, dan `DevConsole`. **Inventori, crafting, peti, api unggun, dan menu bangun tidak menjeda.**
2. **Gerakan saat panel tanpa jeda terbuka:** pemain berhenti, karena seluruh `Gameplay` dimatikan. Pilihan lain adalah tetap bisa berjalan, tetapi itu membutuhkan pemisahan Action Map tambahan.
3. **Audio saat dijeda:** SFX dunia berhenti, musik turun 12 dB, dan suara tombol tetap bunyi.
4. **Jeda otomatis:** game membuka `Pause` saat jendela kehilangan fokus.
5. **Serangan saat inventori terbuka:** panel tetap terbuka, kecuali pemain diterkam.

## 6. Skenario Tes

1. Buka Diary lalu tekan Esc. **Hasil:** Diary tertutup, Pause tidak terbuka, dan waktu kembali berjalan.
2. Buka Pause, buka `Settings`, lalu tekan Esc dua kali. **Hasil:** `Settings` tertutup lebih dulu, lalu Pause. Waktu baru berjalan setelah Esc kedua.
3. Buka Diary, paksa Pause lewat `DevConsole`, lalu tutup Diary dari tengah stack. **Hasil:** `Time.timeScale` tetap 0 selama Pause terbuka.
4. Tampilkan `GameOver`, lalu tekan Esc berkali-kali. **Hasil:** tidak ada perubahan, dan `Time.timeScale` tetap 0.
5. Buka inventori saat zombie mendekat. **Hasil:** waktu tetap berjalan, zombie menyerang, kursor bebas, dan klik kiri tidak menembak.
6. Tekan `Tab` untuk membuka lalu `Tab` lagi. **Hasil:** inventori terbuka lalu tertutup, tanpa berkedip pada frame yang sama.
7. Pegang barang di inventori, lalu turunkan HP ke 0. **Hasil:** barang kembali ke slot, inventori tertutup, dan `Cinematic` aktif tanpa kursor.
8. Buka Pause saat musik berganti dari siang ke malam. **Hasil:** crossfade tetap selesai, dan SFX dunia berhenti.
9. Tekan tombol di Pause. **Hasil:** suara klik terdengar, dan animasi panel berjalan walaupun waktu berhenti.
10. Tutup panel Pause dengan klik. **Hasil:** pistol tidak menembak pada klik itu.
11. Pindah ke jendela lain saat bermain, lalu kembali. **Hasil:** Pause terbuka, dan menutupnya mengunci kursor lagi.
12. Cari `Time.timeScale` di semua script. **Hasil:** hanya ada penulisan di `UIFocusStack.ApplyState()`.
