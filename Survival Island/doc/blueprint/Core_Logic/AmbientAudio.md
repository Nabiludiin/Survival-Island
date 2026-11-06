# Blueprint: AmbientAudio.cs
**Lokasi:** `Survival Island/doc/blueprint/Core_Logic/AmbientAudio.md`
**Deskripsi Utama:** Suara latar berdasarkan tempat pemain berdiri. Area Pantai, Hutan, dan region lain dikenali lewat trigger, lalu suaranya berganti dengan crossfade lewat Audio Mixer.
**Issue:** #52
**Rujukan GDD:** Bagian 5 (suasana tiap region) dan bagian 6 (pantai), secara tidak langsung. Pemetaan suaranya adalah usulan.

## 1. Arsitektur & Variabel Utama

`AmbientAudio` adalah manager persisten di `PersistentRoot` dan memakai `IBootstrapService`. Sistem ini berjalan hanya saat `GamePhase.IslandSurvival`.

```csharp
[CreateAssetMenu(menuName = "Survival/Ambient Profile")]
public class AmbientProfile : ScriptableObject
{
    public string profileId;          // ambient_pantai, ambient_hutan, ...
    public int priority;              // Nilai besar menang saat area bertumpuk
    public AmbientLayer[] layers;     // Maksimal 3 loop
    public AmbientOneShot[] oneShots; // Suara acak, misalnya camar
}

[Serializable] public class AmbientLayer
{
    public AudioClip clip;
    public float dayVolume = 1f, nightVolume = 1f;
    public bool followsTide;          // Volume mengikuti TideChangedEvent.level
    public bool mutedByBadWeather;    // Hilang saat Rain, Storm, atau Blizzard
    public bool mutedByThreatEvent;   // Hilang saat bencana dan Blood Moon
}

[Serializable] public class AmbientOneShot
{
    public AudioClip[] clips;
    public float minInterval = 8f, maxInterval = 20f;
    public float radius = 25f;        // Jarak munculnya suara dari pemain
    public bool dayOnly;
}

public class AmbientZone : MonoBehaviour   // BoxCollider trigger di layer ZoneTrigger
{
    public AmbientProfile profile;
}
```

| Variabel `AmbientAudio` | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `Instance` | `AmbientAudio` | `null` | Singleton |
| `mixer` | `AudioMixer` | Mixer utama | Sama dengan `AudioManager` |
| `deckA`, `deckB` | `AmbientDeck` | 3 `AudioSource` per deck | Dua jalur crossfade |
| `zoneCounts` | `Dictionary<AmbientProfile, int>` | Kosong | Jumlah trigger yang sedang dimasuki per profil |
| `regionProfiles` | `AmbientProfile[]` | Per `RegionType` | Suara saat tidak berada di area khusus |
| `currentProfile` | `AmbientProfile` | `null` | Profil aktif |
| `pendingProfile`, `pendingTimer` | | `null`, 0 | Penundaan keluar area |
| `isNight`, `currentRegion`, `currentWeather` | | Dari event sticky | Kondisi saat ini |

### Area yang disiapkan

| Profil | Trigger | Loop | One-shot | Prioritas |
|---|---|---|---|---|
| `ambient_pantai` | Beberapa `BoxCollider` memanjang mengikuti garis pantai | Ombak (`followsTide`), angin laut | Camar, `dayOnly` | 20 |
| `ambient_hutan` | `BoxCollider` di area pohon Region 1 dan 2 | Angin daun (siang 1, malam 0,6), jangkrik (siang 0, malam 1, `mutedByThreatEvent`) | Burung, `dayOnly` | 10 |
| `ambient_volcano` | Tanpa trigger, dari region | Gemuruh rendah, angin | Tidak ada | 0 |
| `ambient_amazon` | Tanpa trigger, dari region | Dengung serangga, tetes air | Burung tropis | 0 |
| `ambient_snow` | Tanpa trigger, dari region | Angin dingin | Tidak ada | 0 |
| `ambient_village` | Tanpa trigger, dari region | Angin lembah, air menetes | Gagak jauh | 0 |

Trigger diletakkan di scene `IslandPhase`, bukan di scene region, supaya tidak hilang saat region dilepas (#88). Layer-nya `ZoneTrigger`, sesuai `PhysicsLayers.md`.

### Angka penyetelan

| Nama | Nilai awal | Fungsi |
|---|---|---|
| `GameBalanceConfig.ambientAudio.crossfadeSeconds` | 3 | Lama crossfade antar-area |
| `GameBalanceConfig.ambientAudio.zoneExitGraceSeconds` | 1,5 | Jeda sebelum perubahan area diterapkan |
| `GameBalanceConfig.ambientAudio.retargetDuckSeconds` | 0,2 | Menurunkan deck sebelum klipnya diganti |
| `GameBalanceConfig.ambientAudio.dayNightBlendSeconds` | 20 | Transisi volume siang dan malam |
| `GameBalanceConfig.ambientAudio.underwaterLowpassHz` | 800 | Suara teredam saat menyelam |
| `GameBalanceConfig.ambientAudio.sleepDuckDb` | -6 | Suara saat tidur |
| `GameBalanceConfig.ambientAudio.runEndFadeSeconds` | 3 | Fade saat run berakhir |

## 2. Detail Logika & Fungsi

### Trigger area

**`AmbientZone.OnTriggerEnter(Collider other)`:** kalau `other.CompareTag("Player")`, panggil `AmbientAudio.Instance.EnterZone(profile)`. `OnTriggerExit` memanggil `ExitZone(profile)`.

**`AmbientZone.OnDisable()`:** kalau pemain masih tercatat di dalam, panggil `ExitZone(profile)`. Unity tidak mengirim `OnTriggerExit` saat trigger dimatikan.

Satu area boleh terdiri dari banyak `BoxCollider`. Hitungannya per profil, sehingga melewati sambungan dua kotak pantai tidak mengganti suara.

**`EnterZone` / `ExitZone`** menambah atau mengurangi `zoneCounts[profile]`, lalu memanggil `ResolveTarget()`.

**`ResolveTarget()`**

1. Pilih profil dengan `zoneCounts > 0` dan `priority` terbesar. Pantai mengalahkan hutan saat keduanya bertumpuk.
2. Kalau tidak ada, pakai `regionProfiles[currentRegion]`.
3. Kalau hasilnya sama dengan `currentProfile`, kosongkan `pendingProfile`.
4. Kalau berbeda, simpan `pendingProfile` dan `pendingTimer = zoneExitGraceSeconds`. Masuk ke area baru dan keluar dari area memakai jeda yang sama, supaya berjalan di garis batas tidak membuat suara bolak-balik.

`Update` mengurangi `pendingTimer` dengan `Time.deltaTime`, lalu memanggil `BeginCrossfade(pendingProfile, crossfadeSeconds)` saat waktunya habis.

### Audio Mixer Blending

Mixer punya grup `Ambient` dengan dua anak, yaitu `AmbientA` dan `AmbientB`. Parameter yang diekspos adalah `AmbientA_Vol`, `AmbientB_Vol`, dan `Ambient_Lowpass`. Tiap `AudioSource` deck keluar ke grup masing-masing dengan `spatialBlend = 0` dan `loop = true`.

**`BeginCrossfade(profile, seconds)`**

1. Deck dengan bobot lebih kecil menjadi deck masuk. Kalau bobotnya di atas 0,05, turunkan ke 0 selama `retargetDuckSeconds`, supaya penggantian klip tidak terdengar terputus.
2. `incoming.Load(profile)` mengisi klip, mengacak `AudioSource.time`, dan menghitung volume layer.
3. Kecepatan fade berasal dari `seconds`. Tiap frame, `t = MoveTowards(t, 1, Time.deltaTime / seconds)`.
4. Pakai kurva daya sama: `incomingWeight = Sin(t * PI / 2)` dan `outgoingWeight = Cos(t * PI / 2)`, supaya volume total tidak turun di tengah transisi.
5. Tulis ke mixer dalam dB: `mixer.SetFloat(param, 20 * Log10(Max(weight, 0.0001f)))`.
6. Setelah `t == 1`, hentikan deck keluar dan set `currentProfile = profile`.

Target baru saat crossfade berjalan mengulang langkah 1 dari bobot saat itu, sehingga tidak ada lonjakan volume.

### Volume layer

**`UpdateLayerVolumes()`** berjalan tiap frame pada deck aktif, dan mengubah `AudioSource.volume` secara halus dengan `MoveTowards`:

```text
target = isNight ? nightVolume : dayVolume
kalau followsTide: target *= Lerp(0,6, 1, TideChangedEvent.level)
kalau mutedByBadWeather dan cuaca Rain, Storm, atau Blizzard: target = 0
kalau mutedByThreatEvent dan ada bencana atau Blood Moon: target = 0
```

Kecepatannya berasal dari `dayNightBlendSeconds`. Hilangnya jangkrik saat Blood Moon sengaja membuat malam terasa sunyi dan mengancam.

### One-shot

Tiap `AmbientOneShot` punya timer acak antara `minInterval` dan `maxInterval`. Saat habis, kalau `dayOnly` terpenuhi dan cuaca tidak buruk:

1. Ambil `SpawnFromPool("Audio_OneShot3D", posisi, ...)`, dengan posisi acak pada lingkaran `radius` di sekitar pemain, naik 5 sampai 15 meter.
2. `PooledAudioSource.Play(clip, mixerGroup = AmbientA atau B)`. Suaranya 3D, sehingga camar terdengar dari arah tertentu.

Random memakai `UnityEngine.Random` biasa. Suara tidak memengaruhi gameplay, sehingga tidak perlu seed (#89).

### Pengubah lain

- **Bawah air:** `SwimStateChangedEvent.isUnderwater` mengubah `Ambient_Lowpass` dari 22000 ke `underwaterLowpassHz`.
- **Tidur:** `SleepStateChangedEvent.isSleeping` menurunkan grup `Ambient` sebesar `sleepDuckDb`.
- **Run berakhir:** `RunEndedEvent` memulai fade `runEndFadeSeconds`, dan one-shot berhenti. `AudioManager.StopWorldAudio()` memanggil `StopImmediate()` kalau fade belum selesai.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** tidak ada.

**Event yang didengar:**

| Event | Kegunaan |
|---|---|
| `RegionChangedEvent` | Memilih profil cadangan |
| `PhaseChangedEvent`, `RunEndedEvent` | Mulai dan berhenti |
| `DaybreakEvent`, `NightfallEvent` | Volume siang dan malam |
| `WeatherChangedEvent` | Meredam layer dan one-shot, hanya kalau `region == currentRegion` |
| `TideChangedEvent` | Volume ombak |
| `SwimStateChangedEvent`, `SleepStateChangedEvent` | Suara teredam |
| `DisasterStartedEvent`, `DisasterEndedEvent`, `BloodMoonStartedEvent`, `BloodMoonEndedEvent` | `mutedByThreatEvent` |

| Sistem | Hubungan |
|---|---|
| `AudioManager` | Pemilik mixer dan pemanggil `StopImmediate()` |
| `ZoneManager` | Pengirim `RegionChangedEvent`. Trigger region tidak dipakai ulang |
| `ObjectPooler` | Pool `Audio_OneShot3D` dengan `PooledAudioSource`, dipakai bersama #114 |
| `CombatMusicDirector` (#114) | Mengatur musik. Tidak ada panggilan langsung antar keduanya |
| `FootstepAudio` (#21) | Suara langkah. Di luar sistem ini |
| `DisasterManager` (#70), `HordeManager` (#50) | Pemilik suara bencana dan peringatan |

## 4. Kasus Khusus

- **Pakai ulang dari pool:** `PooledAudioSource.OnSpawned` mengatur ulang klip, `pitch = 1`, `volume = 1`, dan `outputAudioMixerGroup`. `Update` mengembalikannya ke pool saat `isPlaying == false` dan `AudioListener.pause == false`. `OnReturned` memanggil `Stop()` dan mengosongkan klip.
- **Game dijeda:** `UIFocusStack` mengatur `AudioListener.pause = true`, sehingga suara latar ikut berhenti. Timer dan crossfade memakai `Time.deltaTime`, jadi ikut berhenti.
- **Panel tanpa jeda:** suara tetap berjalan saat Inventori atau menu bangun terbuka.
- **Fase 1:** deck diam, karena suara kapal milik `ShipHazardManager` (#75).
- **Mulai run dan load:** semua nilai berasal dari event sticky, lalu `BeginCrossfade(target, 0,5)` berjalan tanpa `zoneExitGraceSeconds`. Kalau pemain muncul di dalam trigger, `OnTriggerEnter` tetap terpanggil setelah langkah fisika pertama.
- **Teleport:** `DevConsole` dan load tidak membuat kasus khusus, karena `OnTriggerExit` dan `OnTriggerEnter` tetap berjalan.
- **Permadeath dan save:** tidak ada data yang disimpan. `ResetForNewRun()` mengosongkan `zoneCounts`, `currentProfile`, dan `pendingProfile`, mengembalikan parameter mixer, serta menghentikan kedua deck.
- **Kartu suara berubah:** Unity memuat ulang audio, jadi `AudioSettings.OnAudioConfigurationChanged` memanggil `Load` ulang deck aktif.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** layer `ZoneTrigger` juga dipakai trigger audio (#87), `AudioListener.pause` diatur `UIFocusStack` (#83), dan langganan event dipasang di `OnEnable` (#107).

**Usulan yang disetujui developer sebagai nilai awal:**

1. **Area di luar Pantai dan Hutan.** Issue hanya meminta dua area. Rekomendasi: empat profil cadangan per `RegionType`, supaya tidak ada area yang sunyi.
2. **Prioritas Pantai di atas Hutan.** Rekomendasi: Pantai menang saat bertumpuk, karena ombak paling mudah dikenali.
3. **Lama crossfade 3 detik dan jeda keluar 1,5 detik.** Rekomendasi: dipakai sebagai nilai awal, lalu disetel lewat playtest.
4. **Jangkrik hilang saat Blood Moon dan bencana.** Rekomendasi: dipakai, karena membuat suasana lebih mencekam.

## 6. Skenario Tes

1. Berjalan dari hutan ke pantai. **Hasil:** setelah sekitar 1,5 detik suara berganti dengan crossfade 3 detik, tanpa jeda sunyi atau lonjakan.
2. Berdiri di batas pantai dan melangkah bolak-balik setiap 1 detik. **Hasil:** suara tidak berganti-ganti.
3. Berjalan melewati sambungan dua `BoxCollider` pantai. **Hasil:** suara tetap sama.
4. Masuk ke tumpukan area hutan dan pantai. **Hasil:** suara pantai yang menang.
5. Di hutan, percepat waktu ke malam dengan `DevConsole`. **Hasil:** jangkrik muncul bertahap dan angin daun melemah.
6. Di pantai saat siang, tunggu 30 detik. **Hasil:** camar terdengar dari arah yang berbeda-beda. Saat malam, camar berhenti.
7. Ubah pasang lewat `DevConsole`. **Hasil:** volume ombak berubah halus.
8. Menyelam. **Hasil:** suara latar teredam, lalu normal kembali setelah muncul.
9. Buka Pause di tengah crossfade. **Hasil:** suara berhenti, lalu crossfade berlanjut dari titik yang sama.
10. Masuk ke Region 3. **Hasil:** suara berganti ke `ambient_snow`.
11. Mati di pantai, lalu mulai run baru. **Hasil:** suara memudar, Fase 1 tanpa ambient pulau, dan di pulau hanya suara area saat itu yang aktif.
12. Biarkan one-shot berbunyi 10 menit sambil melihat Profiler. **Hasil:** tidak ada `Instantiate` baru, dan jumlah objek aktif `Audio_OneShot3D` kembali turun.
