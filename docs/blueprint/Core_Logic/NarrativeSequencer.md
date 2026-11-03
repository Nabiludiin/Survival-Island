# Blueprint: NarrativeSequencer.cs
**Lokasi:** `docs/blueprint/Core_Logic/NarrativeSequencer.md`
**Deskripsi Utama:** Pemutar adegan cerita di awal, tengah, dan akhir run, termasuk fitur lewati, subtitle, audio, dan pemuatan scene di belakang adegan.
**Issue:** #99
**Rujukan GDD:** Bagian 2 (Insiden Kapal), bagian 3 (Fase 1 dan 2), dan bagian 5 Region 3. Isi adegan dan ending berbeda adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`NarrativeSequencer` adalah singleton persisten (`RunLifecycle.md`), dengan kanvas pada canvas persisten. Adegan adalah data, jadi menambah adegan tidak mengubah kode.

```csharp
[CreateAssetMenu(menuName = "Survival/Narrative Scene")]
public class NarrativeScene : ScriptableObject
{
    public string sceneId;
    public NarrativeBeat[] beats;
    public AudioClip music;
    public bool freezeWorld;       // true untuk adegan di tengah permainan
    public bool alwaysSkippable;   // Dipakai ending untuk pemain yang sudah menang
}

[Serializable]
public class NarrativeBeat
{
    public Sprite image;           // Boleh null untuk layar hitam
    public string textKey;         // Teks layar, lewat LocalizationManager (#104)
    public string subtitleKey;     // Subtitle untuk voice
    public AudioClip voice;
    public AudioClip sfx;
    public float seconds;          // 0 berarti mengikuti panjang voice
}

public struct EndingContext
{
    public int aliveNpcCount, rescuedCount, lostOnShipCount, day;
}
```

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `NarrativeSequencer` | `null` | Singleton |
| `scenes` | `NarrativeScene[]` | Daftar di bawah | Semua adegan |
| `IsPlaying` | `bool` | `false` | Ada adegan yang berjalan |
| `currentScene` | `NarrativeScene` | `null` | Adegan aktif |
| `ownsPanel` | `bool` | `false` | Panel `Cinematic` dibuka oleh sistem ini |
| `pendingEnding` | `EndingContext?` | `null` | Diisi #14 sebelum menang |

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.narrative.beatFadeSeconds` | 0,5 | Transisi antar-gambar |
| `GameBalanceConfig.narrative.musicFadeSeconds` | 1 | Masuk dan keluar musik adegan |
| `GameBalanceConfig.narrative.skipHintSeconds` | 3 | Lama petunjuk `Space untuk lewati` tampil |
| `GameBalanceConfig.narrative.watchdogExtraSeconds` | 10 | Tambahan batas waktu di atas total durasi adegan |
| `GameBalanceConfig.run.endingTimeoutSeconds` | 120 | Batas tunggu ending. Bagian dari `run` |

### Daftar adegan

| `sceneId` | Kapan | Pemicu | Isi | Kira-kira |
|---|---|---|---|---|
| `intro_ship` | Run baru | `RunLifecycle.StartNewRun`, selama `ShipPhase` dimuat | Zombie menyusup dan menggigit penumpang, kru panik, kapal menabrak gunung es (GDD bagian 2) | 25 detik |
| `lifeboat_to_beach` | Akhir Fase 1 | `RunLifecycle.EnterIslandPhase`, selama `IslandPhase` dimuat | Sekoci menjauh, kapal tenggelam, pantai pulau terlihat. Korban dari `lostNpcIds` disebut namanya | 15 detik |
| `helipad_found` | Tengah run | `HelipadFoundVolume` di `Region3_Snow`, pertama kali masuk | Kamera melihat helipad dan kerangka helikopter yang belum selesai | 8 detik, `freezeWorld` |
| `takeoff` | Menang | `PlayEnding` | Helikopter naik, pulau mengecil | 12 detik |
| `ending_alone` | Menang tanpa NPC hidup | `PlayEnding` | MC terbang sendiri | 15 detik |
| `ending_with_npc` | Menang dengan minimal 1 NPC hidup, ada yang hilang | `PlayEnding` | MC bersama yang tersisa | 15 detik |
| `ending_all_saved` | Menang, semua NPC kapal selamat | `PlayEnding` | Semua selamat | 20 detik |

### Pemilihan ending

| Syarat, diperiksa berurutan | Adegan |
|---|---|
| `aliveNpcCount == 0` | `ending_alone` |
| `aliveNpcCount == rescuedCount` dan `lostOnShipCount == 0` dan `rescuedCount > 0` | `ending_all_saved` |
| Lainnya | `ending_with_npc` |

Ending hanya mengubah adegan. Syarat menang dan isi `WinScreen` tidak berubah.

## 2. Detail Logika & Fungsi

### Alat pemutar

Versi awal memakai rangkaian teks dan gambar, sesuai `NarrativeBeat`. Pemutar berjalan dengan `Time.unscaledDeltaTime`, jadi tetap maju saat `freezeWorld`. Kalau nanti memakai Unity Timeline, `beats` bisa diganti `PlayableDirector` di balik fungsi publik yang sama, tanpa mengubah pemanggil.

### Fungsi utama

**`public IEnumerator Play(string sceneId)`**:

1. Cari adegan. Kalau tidak ada, catat peringatan dan selesai.
2. Kalau `IsPlaying`, tolak dan catat. Hanya satu adegan pada satu waktu.
3. `Begin(scene)`.
4. Jalankan tiap beat: tampilkan gambar dengan fade `beatFadeSeconds`, ambil teks dari `LocalizationManager.Get(textKey)`, mainkan `voice` dan `sfx`, lalu tunggu `seconds`. Kalau `CanSkip()` dan `InputManager.SkipPressed`, keluar dari perulangan.
5. `Finish(scene)`.

Batas waktu pengaman: kalau total waktu lewat dari jumlah `seconds` ditambah `watchdogExtraSeconds`, `Finish` dipaksa, supaya klip audio yang rusak tidak mengunci game.

**`private void Begin(NarrativeScene s)`**:

1. `IsPlaying = true`.
2. Kalau panel `Cinematic` belum ada di stack: `ownsPanel = UIFocusStack.Instance.Push(cinematicPanel)`. `RunLifecycle` sudah mendorongnya saat akhir run, jadi `ownsPanel` bernilai `false`.
3. Kalau `freezeWorld`: `UIFocusStack.Instance.SetGameplayTimeScale(0)`.
4. `AudioManager.Instance.SetCinematicMix(true, musicFadeSeconds)`, lalu `PlayMusicClip(s.music, musicFadeSeconds)`.
5. Terbitkan `NarrativeStateChangedEvent { sceneId, isPlaying = true }` dan `CinematicStateChangedEvent { isPlaying = true, keepMusic }` (#114). `keepMusic` bernilai `true` kalau adegan tidak punya musik sendiri.

**`private void Finish(NarrativeScene s)`** membalik semuanya dengan urutan terbalik, lalu `MarkSeen(s.sceneId)`, dan `IsPlaying = false`. Panel hanya ditutup kalau `ownsPanel`. Semua langkah ada di `finally`.

**`public IEnumerator PlayDuringLoad(string sceneId)`** dipakai `RunLifecycle`:

```text
var load = StartCoroutine(AsyncSceneLoader.LoadSingle(scene, holdActivation: true));
yield return NarrativeSequencer.Instance.PlayDuringLoad(sceneId);
yield return load;
```

1. Tutup panel `Loading` kalau terbuka, karena `Loading` menolak panel baru (`UIFocusStack.md`).
2. Jalankan `Play(sceneId)`.
3. Kalau `AsyncSceneLoader.Progress < 1`: dorong panel `Loading` kembali, supaya pemain yang melewati adegan melihat bar.
4. `AsyncSceneLoader.AllowActivation()`.

Pemuatan berjalan terus saat adegan berjalan, karena adegan hanya menunggu waktu dan tidak memegang thread utama. Lonjakan aktivasi scene tersembunyi di balik layar hitam. Selama `allowSceneActivation == false`, operasi async lain mengantre, jadi region baru dimuat setelah adegan selesai.

**`public void PrepareEnding(EndingContext ctx)`** dipanggil `RescueEventManager` (#14), tepat sebelum `TriggerWinSequence`.

**`public IEnumerator PlayEnding()`**, dipanggil `RunLifecycle.RunEndSequence`:

1. `Play("takeoff")`.
2. Pilih ending dari `pendingEnding`. Kalau `null` (uji lewat konsol), pakai `ending_alone`.
3. `Play(endingId)`.

### Fitur lewati

**`private bool CanSkip()`** = `currentScene.alwaysSkippable || HasSeen(currentScene.sceneId)`.

- Tombol `Skip` (`Space`) sudah terdaftar di `InputManager.md`.
- Petunjuk tampil selama `skipHintSeconds` di awal adegan, hanya kalau `CanSkip()`.
- Adegan yang belum pernah ditonton tidak bisa dilewati.

**Catatan adegan.** `HasSeen(id)` membaca `PlayerPrefs` kunci `narrative.seen.<sceneId>`, dan `MarkSeen` menulisnya. Data ini di luar data run, jadi tetap ada setelah permadeath (`RunLifecycle.md`). Adegan yang terputus karena proses mati tidak tercatat.

### Input dan fokus

Panel `Cinematic` (#83) memblokir input gameplay, tidak menjeda waktu, tidak bisa ditutup Esc, dan menolak panel baru. Sistem ini tidak mengubah `Time.timeScale` atau kursor secara langsung. `freezeWorld` memakai `SetGameplayTimeScale(0)`, jalur resmi untuk efek selain panel.

### Subtitle

`subtitleKey` dan `textKey` ditampilkan lewat `LocalizationManager.Get(key)` (#104). Tampilan subtitle mengikuti setting dari #104. Selama #104 belum ada, kunci mentah dianggap sebagai teks, sehingga adegan tetap berjalan.

### Audio

| Saat | Yang terjadi |
|---|---|
| Mulai | `SetCinematicMix(true)`. Suara dunia turun, dan musik adegan masuk |
| Berjalan | `CombatMusicDirector` (#114) berhenti memilih musik selama `NarrativeStateChangedEvent.isPlaying` |
| Selesai | `SetCinematicMix(false)`. Musik dunia kembali mengikuti siang atau malam |
| Mixer | `AudioMixer.updateMode = UnscaledTime`, sesuai `UIFocusStack.md` |

### `HelipadFoundVolume`

Trigger layer `ZoneTrigger` di `Region3_Snow`, sekaligus `ISaveable` lewat `RegionSceneRoot` (#88). Saat pemain masuk pertama kali: tandai `isConsumed`, lalu jalankan `Play("helipad_found")`. Jadi adegan sekali per run. Pemain yang sudah pernah menontonnya bisa melewatinya.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `NarrativeStateChangedEvent` dan `CinematicStateChangedEvent`.

**Event yang didengar:** tidak ada. Adegan dipicu lewat pemanggilan langsung dari pemilik alur.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `PlayDuringLoad` dan `PlayEnding` |
| `AsyncSceneLoader` | `Progress`, dan `AllowActivation` |
| `UIFocusStack` (#83) | Panel `Cinematic`, `SetGameplayTimeScale` |
| `InputManager` (#18) | `SkipPressed` |
| `LocalizationManager` (#104) | Teks dan subtitle |
| `AudioManager`, `CombatMusicDirector` (#114) | Audio |
| `RescueEventManager` (#14) | `PrepareEnding` |
| `LifeboatExtraction` (#74) | `lostNpcIds` untuk `lifeboat_to_beach`. `PreGameRoutine` menunggu panel `Cinematic` tertutup |
| `FirstPersonCamera` (#102) | Mode adegan saat panel `Cinematic` terbuka |

**Batas pekerjaan:** adegan kematian di #53, logika helikopter di #14, dan hasil Fase 1 di #74.

## 4. Kasus Khusus

- **Objek pool:** tidak memakai pool. Kanvas dan satu `AudioSource` voice hidup di canvas persisten.
- **Game dijeda:** `Pause` tidak bisa terbuka karena panel `Cinematic` menolak panel baru. `freezeWorld` menghentikan waktu dunia, jam, dan fisika, tetapi adegan memakai waktu nyata. Satu hari game tidak berkurang oleh adegan.
- **Permadeath:** catatan `narrative.seen.*` tetap, dan `ResetForNewRun` hanya mengosongkan `pendingEnding`, `IsPlaying`, dan `ownsPanel`.
- **Save dan load:** tidak ada save di sistem ini. `HelipadFoundVolume` menyimpan statusnya di region. Lanjutkan run tidak memutar ulang `intro_ship` atau `lifeboat_to_beach`.
- **Mati saat `helipad_found`:** tidak bisa, karena dunia beku dan damage tidak berjalan.
- **Scene gagal dimuat saat adegan:** `RunLifecycle` kembali ke `MainMenu`, dan `finally` melepas panel serta audio.
- **Skip sebelum loading selesai:** panel `Loading` tampil, sesuai langkah 3 di `PlayDuringLoad`.
- **Loading selesai sebelum adegan selesai:** scene menunggu `AllowActivation` di 0,9.
- **Gambar atau voice kosong:** beat tetap berjalan sesuai `seconds`. Kalau `seconds == 0` dan `voice == null`, dipakai `beatFadeSeconds`.
- **`DevConsole` dibuka saat adegan:** diizinkan, sesuai `UIFocusStack.md`.
- **Region dilepas saat adegan:** tidak terjadi pada `helipad_found`, karena pemain tidak bergerak.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:**

- Persisten, tombol `Skip`, panel `Cinematic`, dan `AllowActivation` (`RunLifecycle.md`, `InputManager.md`, `UIFocusStack.md`).
- Adegan yang sudah ditonton berada di luar data run (`RunLifecycle.md`).

**Usulan yang disetujui developer sebagai nilai awal (dicatat di #85):**

1. **Alat:** rekomendasi rangkaian teks dan gambar untuk versi awal, karena murah dan mudah diterjemahkan. Timeline bisa menyusul tanpa mengubah pemanggil.
2. **Daftar adegan:** rekomendasi tujuh adegan pada tabel.
3. **Ending:** rekomendasi tiga ending berdasarkan NPC, tanpa memengaruhi hadiah.
4. **Lewati:** rekomendasi hanya setelah pernah ditonton, sesuai checklist. Ending memakai aturan yang sama, kecuali diubah dengan `alwaysSkippable`.
5. **Penyimpanan catatan:** rekomendasi `PlayerPrefs`, karena satu nilai per adegan dan sudah aman dari permadeath.
6. **`helipad_found` membekukan dunia:** rekomendasi ya, supaya pemain tidak diserang saat kontrolnya terkunci.

## 6. Skenario Tes

1. Hapus kunci `narrative.seen.*`, lalu Run Baru. **Hasil:** `intro_ship` tampil, `Space` tidak melewati, dan `ShipPhase` aktif setelah adegan.
2. Mati, lalu Ulangi dari Kapal. **Hasil:** petunjuk lewati tampil, dan `Space` melewati adegan.
3. Pakai `streamdelay` atau muat scene besar, lalu lewati adegan. **Hasil:** bar `Loading` tampil sampai selesai.
4. Selesaikan Fase 1 dengan satu NPC tertinggal. **Hasil:** `lifeboat_to_beach` menyebut nama NPC, lalu pemain tiba di pantai.
5. Saat adegan, buka Profiler. **Hasil:** `AsyncSceneLoader.Progress` tetap naik sampai batas 0,9.
6. Tekan Esc dan gerakkan mouse saat adegan. **Hasil:** tidak terjadi apa-apa, dan pemain tidak bergerak.
7. Masuk ke `HelipadFoundVolume` saat zombie mendekat. **Hasil:** dunia beku selama adegan, zombie tetap di tempat, dan jam dunia tidak maju.
8. Keluar lalu masuk lagi ke volume. **Hasil:** adegan tidak terulang pada run yang sama.
9. Mulai run baru, masuk ke volume lagi. **Hasil:** adegan tampil lagi dan bisa dilewati.
10. `heli liftoff` tanpa NPC. **Hasil:** `takeoff`, lalu `ending_alone`.
11. Menang dengan semua NPC hidup dan tanpa korban kapal. **Hasil:** `ending_all_saved`.
12. Menang dengan satu NPC mati. **Hasil:** `ending_with_npc`.
13. Saat adegan, ganti bahasa di file uji (#104). **Hasil:** subtitle memakai bahasa aktif saat adegan dimulai.
14. Mulai adegan saat musik tempur berjalan. **Hasil:** musik tempur turun, dan kembali normal setelah adegan.
15. Kosongkan `voice` dan `seconds`. **Hasil:** adegan tidak macet.
16. Ending yang berjalan 60 detik. **Hasil:** `RunEndSequence` menunggu sampai selesai, tidak terpotong batas 15 detik.
