# Blueprint: SettingsManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/SettingsManager.md`
**Deskripsi Utama:** Pemilik preferensi pemain: Mouse Sensitivity, volume Master, BGM, dan SFX, serta kualitas grafis (Shadows dan Textures). Datanya disimpan terpisah dari save file utama, jadi tidak ikut hilang saat permadeath.
**Issue:** #19
**Rujukan GDD:** Tidak disebut langsung. Permadeath di Bagian 1 dan 3 hanya mereset run, bukan preferensi pemain.

## 1. Arsitektur & Variabel Utama

Singleton persisten, urutan bootstrap ke-3 di `RunLifecycle.md`, dan memakai `IBootstrapService`. Penyimpanan memakai `PlayerPrefs`, sesuai `RunLifecycle.md` dan `WorldStateSave.md`. Menu memakai panel `UIPanelId.Settings` yang sudah ada di #83, dibuka dari `MainMenu` dan `Pause`.

```csharp
public enum ShadowQuality { Off, Low, Medium, High }
public enum TextureQuality { Quarter, Half, Full }
public enum QualityPreset { Low, Medium, High, Custom }

[Serializable]
public class SettingsData    // Disimpan sebagai satu JSON
{
    public int version = 1;
    public float mouseSensitivity = 0.12f;
    public bool invertY = false;
    public float fieldOfView = 70f;
    public bool headBobEnabled = true;
    public bool chromaticAberrationEnabled = true;   // Dibaca PostProcessController tiap frame (#23)
    public bool holdToStruggle = false;              // Pilihan aksesibilitas #110
    public bool weaponSwayEnabled = true;
    public bool fovKickEnabled = true;
    public float masterVolume = 1f;
    public float bgmVolume = 1f;
    public float sfxVolume = 1f;
    public QualityPreset qualityPreset = QualityPreset.High;
    public ShadowQuality shadowQuality = ShadowQuality.High;
    public TextureQuality textureQuality = TextureQuality.Full;
    public float brightness = 1f;
    public bool tutorialEnabled = true;
}
```

Kolom di atas dibuka sebagai properti baca, misalnya `SettingsManager.Instance.mouseSensitivity`. Nama dan nilai awalnya mengikuti `FirstPersonCamera.md` bagian "Pengaturan pemain dari #19".

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `data` | `SettingsData` | Dari `PlayerPrefs`, atau bawaan | Nilai aktif |
| `applied` | `SettingsData` | Salinan `data` | Nilai terakhir yang tersimpan, dipakai saat Batal |
| `isDirty` | `bool` | `false` | Ada perubahan yang belum tersimpan |
| `PrefsKey` | `const string` | `settings.data` | Kunci `PlayerPrefs`. Kunci tombol tetap `settings.input.bindings` milik #18 |

### Angka penyetelan (`GameBalanceConfig.settings`)

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `sensitivityMin` / `sensitivityMax` | 0,02 / 0,5 | Batas slider |
| `fovMin` / `fovMax` | 60 / 100 | Batas FOV |
| `brightnessMin` / `brightnessMax` | 0,8 / 1,2 | Batas sempit, supaya gelapnya Region 2 tidak hilang |

### Pemetaan kualitas

| `ShadowQuality` | `QualitySettings.shadows` | `shadowResolution` | `shadowDistance` |
|---|---|---|---|
| `Off` | `Disable` | Tetap | 0 |
| `Low` | `HardOnly` | `Low` | 40 |
| `Medium` | `All` | `Medium` | 70 |
| `High` | `All` | `High` | 120 |

| `TextureQuality` | `QualitySettings.globalTextureMipmapLimit` |
|---|---|
| `Full` / `Half` / `Quarter` | 0 / 1 / 2 |

| `QualityPreset` | Shadows | Textures |
|---|---|---|
| `Low` | `Low` | `Quarter` |
| `Medium` | `Medium` | `Half` |
| `High` | `High` | `Full` |
| `Custom` | Pilihan pemain | Pilihan pemain |

**Usulan batas lampu (#98):**

| `ShadowQuality` | `lights` | `shadows` |
|---|---|---|
| `Off` | 3 | 0 |
| `Low` | 4 | 0 |
| `Medium` | 5 | 1 |
| `High` | 6 | 1 |

## 2. Detail Logika & Fungsi

**`public void Initialize()`**:

1. `Load()`.
2. `ClampAll()`, memakai `GameBalanceConfig.settings`.
3. `ApplyGraphics()`.
4. Audio tidak diterapkan di sini, karena `AudioManager` baru siap di urutan ke-4. `AudioManager.Initialize()` membaca volume dari manager ini.

**`private void Load()`**:

1. Kalau `PrefsKey` tidak ada, pakai `new SettingsData()`.
2. Baca JSON lewat `JsonUtility.FromJsonOverwrite(json, data)`, jadi kolom yang belum ada di versi lama tetap memakai nilai bawaan.
3. Kalau JSON rusak, tangkap exception, catat peringatan, lalu pakai bawaan. Save run tidak terpengaruh.
4. `applied = salinan data`.

**`public void SetMouseSensitivity(float value)`**, `SetInvertY`, `SetFieldOfView`, `SetHeadBob`, `SetWeaponSway`, `SetFovKick`, `SetChromaticAberration`, `SetHoldToStruggle`, `SetBrightness`, dan `SetTutorialEnabled`: batasi nilainya, ubah `data`, lalu `isDirty = true`. `FirstPersonCamera` membaca nilainya tiap frame, sehingga perubahan langsung terasa.

**`public void SetVolume(AudioChannel channel, float value)`**:

1. Batasi ke 0 sampai 1, lalu ubah `data`.
2. `AudioManager.Instance.UpdateVolume(master, bgm, sfx)`.
3. Saat pemain melepas slider SFX, mainkan suara contoh lewat sumber UI yang memakai `ignoreListenerPause = true`, karena `AudioListener.pause` aktif selama panel ini terbuka (#83).

**`public void SetQualityPreset(QualityPreset preset)`**: isi `shadowQuality` dan `textureQuality` dari tabel, kecuali `Custom`. Tampilan belum diterapkan.

**`public void SetShadowQuality(ShadowQuality q)`** dan **`SetTextureQuality(TextureQuality q)`**: ubah nilainya, lalu `qualityPreset = Custom`. Tampilan belum diterapkan.

**`public void ApplyGraphics()`**: tulis ke `QualitySettings`, panggil `LightSourceSystem.ApplyQuality(int lights, int shadows)` kalau `Instance` ada (#98), lalu terbitkan `SettingsChangedEvent { category = Graphics }`. Perubahan tekstur bisa membuat game tersendat sesaat, jadi grafis baru berubah saat pemain menekan **Terapkan**.

**`public void Commit()`**: `ApplyGraphics()` kalau ada perubahan grafis, `PlayerPrefs.SetString(PrefsKey, JsonUtility.ToJson(data))`, `PlayerPrefs.Save()`, `applied = salinan data`, `isDirty = false`, lalu terbitkan `SettingsChangedEvent { category = All }`. Penyimpanan tidak terjadi saat slider digeser.

**`public void Revert()`**: `data = salinan applied`, terapkan audio, dan `isDirty = false`.

**`public void ResetToDefaults()`**: `data = new SettingsData()`, lalu terapkan ke audio. Grafis menunggu Terapkan. Tombol pilihan pemain tidak ikut berubah, kecuali lewat tombol terpisah.

### `SettingsPanel : UIPanel`

Menu memuat toggle `chromaticAberrationEnabled` di samping `headBobEnabled`, dan `holdToStruggle` pada pilihan aksesibilitas.

| Tab | Isi | Pemilik |
|---|---|---|
| Kontrol | Sensitivity, Invert Y, daftar tombol | Manager ini, dan `InputManager.StartRebind` serta `ResetBindings` |
| Audio | Master, BGM, SFX | Manager ini |
| Grafis | Preset, Shadows, Textures, Brightness, tombol Terapkan | Manager ini |
| Tampilan | FOV, Head Bob, Weapon Sway, FOV Kick | Manager ini |
| Lainnya | Tutorial menyala, tombol "Ulang tutorial" | Manager ini, dan `TutorialManager.ResetSeen` |

**`OnClosed(reason)`**:

- `User`, dan `isDirty`: buka `ConfirmDialog` "Simpan perubahan?". Ya menjalankan `Commit`, Tidak menjalankan `Revert`.
- `Forced`, `RunEnded`, atau `SceneUnloaded`: `Commit` tanpa dialog, supaya pilihan pemain tidak hilang.

Menu hanya memanggil fungsi manager ini, dan tidak menulis `QualitySettings` atau `PlayerPrefs` sendiri.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `SettingsChangedEvent { category }`, sticky. Setelah `Initialize`, event ini langsung terbit.

**Event yang didengar:** `InputBindingsChangedEvent`, untuk menggambar ulang daftar tombol.

| Sistem | Hubungan |
|---|---|
| `FirstPersonCamera` (#102) | Membaca sensitivitas, FOV, dan pilihan efek tiap frame |
| `AudioManager` | Menerima `UpdateVolume`, dan membaca volume saat `Initialize` |
| `InputManager` (#18) | Menyediakan `StartRebind` dan `ResetBindings`. Tombol tetap disimpan oleh #18 |
| `UIFocusStack` (#83) | Mengatur jeda dan kursor panel `Settings` |
| `VFXManager` (#15) | Membaca `qualityPreset` lewat `SettingsChangedEvent` |
| `TutorialManager` (#16) | Membaca `tutorialEnabled` |
| `EnvironmentDirector` (#92) | Mengalikan ambient dengan `brightness`, karena hanya sistem itu yang boleh menulis `RenderSettings` |
| `LocalizationManager` (#104) | Pilihan bahasa dan aksesibilitas ditambahkan oleh #104 sebagai tab di panel ini |
| `WorldStateSave` (#80), `RunLifecycle` (#82) | Tidak menyentuh data ini |

## 4. Kasus Khusus

- **Permadeath:** semua setting tetap. `ResetForNewRun()` hanya menerbitkan ulang `SettingsChangedEvent`, karena `GameEventRegistry.ClearAll()` menghapus nilai sticky-nya. `DeleteRunSave()` tidak menyentuh `PlayerPrefs`. `PlayerPrefs.DeleteAll()` dilarang (#82).
- **Save dan load:** tidak ada data di `survival_save.json`. Melanjutkan run tidak mengubah setting.
- **Game dijeda:** panel `Settings` menjeda waktu. UI memakai waktu tanpa skala. Grafis dan audio tetap bisa berubah saat `timeScale = 0`.
- **Keluar paksa:** perubahan yang belum di-`Commit` hilang, sehingga game terbuka dengan setting terakhir yang tersimpan.
- **Versi lama:** `version` memungkinkan migrasi. Versi 1 belum memerlukannya.
- **Nilai di luar batas:** `ClampAll` memperbaiki hasil edit tangan atau perubahan batas.
- **Volume Master 0:** `AudioManager` tetap memutar dengan volume 0, sehingga posisi crossfade tidak rusak.
- **Objek pool:** tidak ada. Pool yang memakai kualitas membaca nilainya saat `OnSpawned`.
- **Banyak perangkat:** `PlayerPrefs` bersifat per perangkat, jadi setting tidak ikut berpindah.

## 5. Keputusan Desain

**Sudah ditetapkan issue dan blueprint lain:**

- Setting terpisah dari save file utama. Lokasinya `PlayerPrefs`, sesuai #80 dan #82.
- Nama dan nilai awal setting kamera mengikuti `FirstPersonCamera.md`.

**Usulan yang menunggu developer:**

1. **Volume dan sensitivitas langsung berlaku, grafis lewat tombol Terapkan.** Perubahan tekstur bisa membuat game tersendat, sedangkan slider perlu umpan balik langsung.
2. **Brightness dibatasi 0,8 sampai 1,2.** Batas sempit menjaga GDD Bagian 5, yaitu Region 2 yang gelap meski siang hari, dan tetap membantu layar yang redup.
3. **Render pipeline:** blueprint ini memakai `QualitySettings` untuk Built-in Render Pipeline, karena #23 menyebut Unity Post-Processing dan proyek Unity belum tersedia untuk diperiksa. Kalau proyek memakai URP, Shadows ditulis ke `UniversalRenderPipelineAsset`, dan API publik manager tetap sama.
4. **Versi Unity:** `globalTextureMipmapLimit` berlaku di Unity 2022.2 ke atas. Versi lebih lama memakai `masterTextureLimit`.
5. **Resolusi, layar penuh, dan VSync:** belum diminta issue, jadi tidak ikut pada versi awal. Rekomendasinya ditambah sebelum rilis pada tab Grafis.

## 6. Skenario Tes

1. Ubah sensitivitas, keluar game, lalu buka lagi. **Hasil:** nilai tetap.
2. Geser Master ke 0, BGM ke 0,5, dan SFX ke 0,2. **Hasil:** suara langsung berubah sesuai slider.
3. Saat Pause, lepas slider SFX. **Hasil:** suara contoh terdengar meski `AudioListener.pause` aktif.
4. Pilih Shadows `Off`, lalu Terapkan. **Hasil:** bayangan hilang, dan preset menjadi `Custom`.
5. Pilih Textures `Quarter`, lalu Terapkan. **Hasil:** tekstur lebih buram, dan tidak ada error.
6. Ubah setting, tutup panel, lalu pilih Tidak. **Hasil:** semua kembali ke nilai sebelumnya.
7. Mati, lalu mulai run baru. **Hasil:** setting dan tombol tidak berubah, dan save run terhapus.
8. Hapus `survival_save.json` secara manual. **Hasil:** setting tetap.
9. Isi `settings.data` dengan teks rusak lewat skrip Editor. **Hasil:** game terbuka dengan nilai bawaan, dan ada peringatan di log.
10. Isi sensitivitas 99 lewat edit `PlayerPrefs`. **Hasil:** dibatasi ke 0,5.
11. Buka Settings dari `MainMenu` dan dari `Pause`. **Hasil:** isi sama, dan Esc hanya menutup `Settings`.
12. Atur Brightness 1,2 di Region 2 malam hari. **Hasil:** lebih terang sedikit, dan kesan gelap tetap ada.
