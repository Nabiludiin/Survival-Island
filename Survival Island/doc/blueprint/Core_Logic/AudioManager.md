# Blueprint: AudioManager.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/AudioManager.md`
**Deskripsi Utama:** Pemilik `AudioMixer` utama, volume, efek suara global, dan musik adegan. Pemilihan musik siang, malam, dan pertempuran pindah ke `CombatMusicDirector` (#114).
**Issue:** #82, #83, #19, #52, #99, #114
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Mendukung suasana siang dan malam di bagian 3.

## 1. Arsitektur & Variabel Utama

`AudioManager` adalah manager persisten (`IBootstrapService`, #82). `Awake` hanya mengisi `Instance`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `AudioManager` | `null` | Singleton |
| `mixer` | `AudioMixer` | Dari prefab | Mixer utama, dengan `updateMode = UnscaledTime` |
| `bgmSource` | `AudioSource` | Dari prefab | Musik adegan (#99). `ignoreListenerPause = true` |
| `sfxSource` | `AudioSource` | Dari prefab | Suara UI dan global. `ignoreListenerPause = true` |
| `warningSource` | `AudioSource` | Dari prefab | Keluar ke grup `Warning` |
| `normalSnapshot`, `pausedSnapshot` | `AudioMixerSnapshot` | Dari mixer | Campuran normal dan jeda |
| `masterVolume`, `bgmVolume`, `sfxVolume` | `float` | Dari `SettingsManager` saat `Initialize()` | Volume yang sedang berlaku |

**Struktur mixer:** grup `Music`, `Ambient` (anak `AmbientA` dan `AmbientB`), `EnemyCue`, `Warning`, dan `SFX`. Parameter terekspos: `AmbientA_Vol`, `AmbientB_Vol`, dan `Ambient_Lowpass`.

**Dihapus (#114):** `dayBGM`, `nightBGM`, dan `dangerBGM` pindah ke aset `MusicSet` milik `CombatMusicDirector` (`calmDay`, `calmNight`, `combatStem`). `PlayDayMusic`, `PlayNightMusic`, `CrossfadeBGM`, dan langganan `DaybreakEvent` serta `NightfallEvent` untuk musik juga dihapus.

## 2. Detail Logika & Fungsi

**`public void Initialize()`**: baca volume dari `SettingsManager`, lalu terapkan ke mixer.

**`public void ResetForNewRun()`**: kembali ke snapshot `Normal`. Volume tidak berubah.

**`public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)`**: `sfxSource.PlayOneShot(clip, volumeMultiplier * sfxVolume * masterVolume)`.

**`public void PlayWarning(AudioClip clip)`**: putar klip lewat grup `Warning` (#52).

**`public void UpdateVolume(float master, float bgm, float sfx)`**: hanya menerapkan volume. Fungsi ini tidak menulis `PlayerPrefs`, karena penyimpanan milik `SettingsManager` (#19). `PlayerPrefs.DeleteAll()` dilarang (#82).

**`public void StopWorldAudio()`**, dipanggil `RunLifecycle` saat reset:

1. `AmbientAudio.Instance.StopImmediate()`.
2. `CombatMusicDirector.Instance.StopImmediate()`.
3. Hentikan `bgmSource`, `warningSource`, dan sumber dunia lain.

**`public void SetPausedMix(bool paused)`**: beralih ke snapshot `Paused` atau `Normal` selama `GameBalanceConfig.ui.pauseAudioFadeSeconds`.

**`public void SetCinematicMix(bool active, float fadeSeconds)`**: menurunkan suara dunia, atau mengembalikannya, selama `fadeSeconds` (#99).

**`public void PlayMusicClip(AudioClip clip, float fadeSeconds)`**: memutar musik adegan di `bgmSource`. Fade memakai `Time.unscaledDeltaTime`. `clip == null` berarti memudarkan musik yang berjalan.

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mendengar event waktu lagi. Kalau kelak perlu mendengar event, langganan dipasang di `OnEnable` dan dilepas di `OnDisable`, memakai `EventChannel<T>`.

| Sistem | Hubungan |
|---|---|
| `SettingsManager` (#19) | Menyimpan volume, lalu memanggil `UpdateVolume` |
| `UIFocusStack` (#83) | Memanggil `SetPausedMix` |
| `RunLifecycle` (#82) | Memanggil `StopWorldAudio` |
| `AmbientAudio` (#52), `CombatMusicDirector` (#114) | Memakai grup mixer, dan punya `StopImmediate` |
| `NarrativeSequencer` (#99) | Memanggil `SetCinematicMix` dan `PlayMusicClip` |
| `CombatFeedbackUI` (#51), `DisasterManager` (#70) | Memakai `PlaySFX` dan grup `Warning` |

## 4. Kasus Khusus

- **Objek pool:** suara 3D sekali putar memakai `PooledAudioSource` (#52), bukan kelas ini.
- **Game dijeda:** `AudioListener.pause` mengikuti `UIFocusStack`. `sfxSource` dan `bgmSource` memakai `ignoreListenerPause = true`, jadi UI dan musik adegan tetap terdengar. Mixer memakai `UnscaledTime`, jadi snapshot tetap berpindah saat `Time.timeScale` bernilai 0.
- **Permadeath:** volume tidak berubah, karena disimpan `SettingsManager` terpisah dari save run.
- **Save dan load:** tidak ada data di save run.
- **`AmbientAudio` atau `CombatMusicDirector` belum tersedia:** `StopWorldAudio` melewati yang `null`.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** `SetPausedMix` (`UIFocusStack.md`). `StopWorldAudio` (`RunLifecycle.md`). Pemindahan BGM (`CombatMusicDirector.md`). Penyimpanan volume (`SettingsManager.md`).

**Keputusan teknis di blueprint ini:** `bgmSource` tetap ada khusus untuk musik adegan #99, karena musik pertempuran sudah pindah ke `CombatMusicDirector`.

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Ubah volume di menu, tutup game, lalu buka lagi. **Hasil:** volume tetap, dan `AudioManager` tidak menulis `PlayerPrefs`.
2. Buka `Pause` saat bermain. **Hasil:** campuran berpindah selama 0,2 detik walaupun waktu berhenti.
3. Tekan tombol UI saat `Pause`. **Hasil:** suara tetap terdengar.
4. Mati saat suara latar berjalan, lalu mulai run baru. **Hasil:** suara dunia berhenti saat reset, dan tidak ada suara ganda.
5. Putar adegan #99. **Hasil:** suara dunia turun, musik adegan masuk, lalu kembali setelah selesai.
6. Tunggu fajar. **Hasil:** `AudioManager` tidak mengganti musik. Pergantian dilakukan `CombatMusicDirector`.

## Catatan Implementasi (Sistem pendukung)

- Kode: `Assets/LOGIC/Core_Logic/AudioManager.cs`. Tidak ada config baru; `SetPausedMix` membaca `ui.pauseAudioFadeSeconds`.
- Volume ke mixer: blueprint tidak menyebut nama parameternya, jadi dipakai parameter terekspos `Master_Vol`, `Music_Vol`, dan `SFX_Vol` (nama bisa diganti di Inspector), dikonversi ke dB (0 menjadi -80 dB). Kalau `Master_Vol` tidak ada di mixer, volume dikalikan di sumber (`PlaySFX` seperti teks blueprint, dan volume `bgmSource`). Kalau ada, pengali di sumber menjadi 1 supaya volume tidak dikali dua kali.
- `SetCinematicMix` memakai snapshot ketiga opsional `cinematicSnapshot`. Snapshot `Paused` didahulukan kalau jeda dan adegan aktif bersamaan.
- `StopWorldAudio`: `AmbientAudio`, `CombatMusicDirector`, dan `PooledAudioSource` belum ada (TODO).
- Isi TODO (sistem pendukung): `PlaySFXAt(AudioClip, Vector3, float volumeMultiplier = 1)`: suara 3D sekali putar lewat `PooledAudioSource.PlayAt` (pool `Audio_OneShot3D`) dan grup mixer `sfxSource`. Kalau pool tidak ada, klip diputar 2D lewat `PlaySFX`. Dipakai `ResourceNode`, `PlayerShove`, dan `LuckyMachine`.
