# 🏝️ Survival Island - Project Repository & AI Developer Workflow

Selamat datang di repositori proyek **Survival Island**. File `README.md` ini adalah gerbang utama dan panduan alur kerja untuk developer dan AI Assistant (Claude) dalam merancang, menyatukan, dan menulis kode game ini.

Jika Anda adalah Claude, **baca dokumen ini dengan sangat teliti sebelum mengeksekusi atau menulis kode apapun.**

**Status proyek:** tahap desain. Logika game ditulis dulu sebagai blueprint `.md` mengikuti `GDD.md`. Proyek Unity dan *script* C# belum dibuat. Nama resmi tercatat di `Survival Island/doc/blueprint/GLOSSARY.md`.

## 📍 Struktur Direktori Utama

```text
Survival-Island/
├── README.md                     # Alur kerja proyek (file ini)
├── Claude.md                     # Cara kerja dan standar teknis Claude untuk seluruh proyek
├── LOGIC/                        # Bahan referensi game asli, bukan kode proyek
└── Survival Island/
    ├── README.md                 # Cerita, feel, dan gambaran game
    ├── Claude.md                 # Perintah khusus Claude saat menulis kode C# (Mode 3)
    ├── doc/                      # Semua dokumentasi desain
    │   ├── GDD.md                # Aturan game dan sumber utama desain
    │   ├── CATATAN_MEKANIK.md    # Catatan mekanik 60 Seconds!, Raft, dan L4D
    │   ├── REFERENSI_MUSUH_L4D.md # Referensi perilaku dan angka musuh
    │   ├── USULAN_TERBUKA.md     # Keputusan developer dan keputusan yang masih terbuka
    │   └── blueprint/
    │       ├── GLOSSARY.md       # Nama resmi scene, manager, enum, tag, dan layer
    │       ├── 60-seconds/       # Fase 1: kapal tenggelam
    │       ├── raft/             # Status, gathering, dan base building
    │       ├── left-4-dead/      # Siklus ancaman, AI musuh, spawn, dan gunplay
    │       ├── Core_Logic/       # Sistem yang dipakai lintas fase
    │       └── Island_Logic/     # Sistem khusus Fase 2 di pulau
    └── LOGIC/
        └── README.md             # Lokasi kode gabungan C#, struktur folder, dan status tiap Langkah
```

`LOGIC/` di root dan `Survival Island/LOGIC/` adalah dua folder berbeda. Yang pertama hanya bahan analisis dari game referensi. Yang kedua adalah tempat *script* C# proyek ini.

### Tempat menaruh blueprint

| Folder | Isi | Contoh |
|---|---|---|
| `Survival Island/doc/blueprint/60-seconds/` | Logika yang hanya berjalan di Fase 1 | `ShipTimerManager.md` |
| `Survival Island/doc/blueprint/raft/` | Status tubuh, resource, dan bangunan | `SurvivalStats.md`, `BuildManager.md` |
| `Survival Island/doc/blueprint/left-4-dead/` | Siklus siang dan malam, AI musuh, spawn, dan gunplay | `EnemyAI.md`, `DayNightDirector.md` |
| `Survival Island/doc/blueprint/Core_Logic/` | Sistem umum yang dipakai beberapa fase | `GameManager.md`, `SaveSystem.md` |
| `Survival Island/doc/blueprint/Island_Logic/` | Mekanik khusus pulau dan region | `ZoneManager.md`, `LuckyMachine.md` |

## 📚 Urutan Prioritas Dokumen

Kalau dua dokumen berbeda, ikuti urutan ini:

| Urutan | Dokumen | Peran |
|---|---|---|
| 1 | `Survival Island/doc/GDD.md` | Sumber utama aturan game |
| 2 | `Claude.md` | Standar teknis dan cara kerja |
| 3 | `Survival Island/doc/blueprint/` | Rancangan logika tiap sistem, harus mengikuti GDD |
| 4 | Issue GitHub | Daftar pekerjaan dan batas tiap pekerjaan |
| 5 | `Survival Island/doc/CATATAN_MEKANIK.md`, `Survival Island/doc/REFERENSI_MUSUH_L4D.md` | Bahan referensi dan usulan, bukan aturan |
| 6 | `LOGIC/` | Bahan analisis |

Jangan memutuskan sendiri kalau aturannya bertentangan. Catat di bagian "Masih terbuka" pada `Survival Island/doc/USULAN_TERBUKA.md`, lalu minta keputusan developer. Keputusan yang sudah diambil dipindah ke bagian "Keputusan utama" di file yang sama.

## ⚙️ AI Developer Workflow (Alur Kerja Claude)

Untuk merakit game ini dengan benar tanpa kehilangan konteks naratif dan mekanik, setiap sistem mengikuti alur berikut secara berurutan:

```text
GDD.md → Issue GitHub → Blueprint .md → Pull Request → Script C# → Tes di Unity
```

### TAHAP 1: Inisialisasi & Pemahaman Konteks (Baca File Dasar)

Sebelum menyentuh folder kode atau logika, baca file berikut agar memahami dunia game dan batasan-batasannya:

1. **`Survival Island/README.md`:** Memahami cerita dan *feel* game.
2. **`Survival Island/doc/GDD.md` (Game Design Document):** Dokumen paling krusial. Berisi seluruh aturan game, narasi (insiden kapal, diary), mekanik 4 Region (Gunung Berapi, Amazon, Es, Desa), mekanik siang/malam (Hewan vs Zombie terbakar), dan *Win Condition* (merakit Helikopter dan kabur, dengan target Hari ke-60. Melewati Hari ke-60 membuat zombie semakin kuat, tetapi bukan Game Over).
3. **`README.md` (File ini):** Memahami alur kerja, struktur folder, dan instruksi penulisan.
4. **`Claude.md`:** Berisi cara kerja, perintah eksekusi, dan standar teknis yang harus dipatuhi.

### TAHAP 2: Pilih Pekerjaan dari Issue

1. Cek daftar [Issues](https://github.com/Nabiludiin/Survival-Island/issues) dan folder `Survival Island/doc/blueprint/`, termasuk issue yang sudah ditutup.
2. Kalau sistemnya belum ada, buat issue baru dengan [format issue](#-format-issue).
3. Baca bagian "Kenapa dibutuhkan", "Batasan", dan "Checklist" pada issue.
4. Baca blueprint dan issue lain yang disebut, supaya pekerjaan tidak tumpang tindih.

Saat diminta membuat issue saja, berhenti setelah issue terbit. Jangan langsung mengerjakan blueprint-nya.

### TAHAP 3: Ekstraksi Logika dari Referensi

Game ini menggabungkan tiga pilar mekanik utama. Baca `Survival Island/doc/CATATAN_MEKANIK.md` dulu, lalu buka sub-folder di `LOGIC/` hanya kalau butuh detail tambahan.

**Tulis ulang logikanya dengan kata-kata sendiri. Jangan menyalin kode atau asset dari game asli.**

* **A. Logika "60 Seconds!" (Fase Awal/Scavenging)**

  * **Yang harus diekstrak:** Sistem *timer* (hitung mundur 60 detik di kapal), logika *looting* cepat (ambil peluru, makanan), pembatasan slot *inventory* awal, dan interaksi memilih/menyelamatkan NPC.

  * **Tujuan:** Membuat transisi *state* yang sempurna dari kapal tenggelam menuju pantai pulau dengan membawa barang *default* beserta barang ekstra hasil *looting*.

* **B. Logika "Raft" (Fase Survival, Status, & Crafting)**

  * **Yang harus diekstrak:** Sistem degradasi bar *Hunger* (lapar) dan *Thirst/Infection*, sistem *gathering* (menebang kayu, menambang batu), memancing menggunakan kail, dan sistem *Base-Building* modular (penempatan lantai, dinding, meja *crafting*).

  * **Aturan Khusus:** Implementasikan restriksi penempatan *collider* di mana pemain tidak bisa membangun struktur *base* di area pasir pantai.

* **C. Logika "Left 4 Dead" (Fase Combat & Horde)**

  * **Yang harus diekstrak:** Mekanik FPS *gunplay* (menembak, *recoil*, *reload*), sistem AI musuh (Pathfinding, *aggro*), dan sistem *spawn manager* dinamis.

  * **Aturan Khusus:** Implementasikan siklus Siang & Malam.

    * *Siang:* *Spawn* Hewan Buas (Serigala, Jaguar, dll). Zombie terbakar jika terkena cahaya matahari.

    * *Malam:* Hentikan *spawn* hewan, picu *Horde System* di mana gerombolan zombie dengan *class* berbeda (*Special Infected* per region) akan memburu pemain.

### TAHAP 4: Tulis Blueprint

1. Buat branch dari `main` dengan nama `fitur-<nama-sistem>`.
2. Tulis blueprint di folder yang sesuai, memakai [format blueprint](#-format-blueprint).
3. Periksa blueprint dengan [syarat blueprint selesai](#-syarat-blueprint-selesai).
4. Kalau blueprint baru mengubah kontrak sistem lain, perbarui blueprint yang terdampak di branch yang sama.

### TAHAP 5: Review & Pull Request

1. Commit dengan format `add: blueprint <nama sistem> (closes #<nomor>)`, memakai nomor issue yang benar.
2. Buka Pull Request ke `main`, dan tulis keputusan desain yang dibuat.
3. Developer memeriksa dan melakukan merge.

Commit, push, Pull Request, dan menutup issue hanya dilakukan saat developer memintanya.

### TAHAP 6: Sintesis & Penyatuan Kode (Output Eksekusi)

Mulai tahap ini setelah blueprint yang dibutuhkan selesai. Perintahnya mengikuti "Jalankan Langkah [X]" di `Claude.md`.

* **Lokasi Output / Penyatuan File:**
  Semua *script* hasil penggabungan logika **HARUS** disimpan ke dalam folder utama:
  👉 `Survival-Island\Survival Island\LOGIC`

  Struktur sub-folder dan status tiap Langkah ada di `Survival Island/LOGIC/README.md`. Aturan khusus menulis kode ada di `Survival Island/Claude.md`.

* **Standar Penulisan:**

  * Buatlah arsitektur berbasis *Manager* dengan nama yang sama seperti di blueprint.

  * Pastikan sistem bisa saling berkomunikasi lewat pola resmi di [issue #107](https://github.com/Nabiludiin/Survival-Island/issues/107) (contoh: `DayNightDirector` memberi tahu sistem spawn kapan matahari terbit agar zombie terbakar).

  * Beri komentar pada baris kode yang mengimplementasikan fitur spesifik dari *GDD* (contoh: `// Mekanik Region 4: Spawn Zombie Desa`).

### TAHAP 7: Tes di Unity & Perbarui Dokumen

1. Uji sistem di Unity sesuai skenario pada blueprint.
2. Kalau angka atau logikanya berubah saat playtest, perbarui blueprint terkait.
3. Kalau aturan game berubah, perbarui `GDD.md` setelah developer setuju.

## 🗺️ Peta GDD ke Blueprint

Tabel ini menunjukkan tempat setiap aturan GDD dirancang. Semua blueprint tersimpan di `Survival Island/doc/blueprint/`, dan semua issue di tabel ini sudah selesai serta ditutup. Daftar lengkap issue dan file ada di bagian akhir `GLOSSARY.md`.

| Bagian GDD | Blueprint utama | Issue (selesai) |
|---|---|---|
| **3. Fase 1: Kapal** | `ShipTimerManager`, `ShipInteractable`, `ShipPlayerController`, `ShipHazardManager`, `LifeboatExtraction` | #74, #75 |
| **3. Status: Lapar, Haus, dan Infeksi** | `SurvivalStats`, `VitalsRules`, `HydrationSystem`, `InfectionSystem`, `DiseaseManager`, `ItemUseSystem` | #43, #44, #45, #64, #66, #111 |
| **3. Siklus Siang dan Malam** | `DayNightDirector`, `SunlightExposure`, `AIDirector`, `HordeManager`, `AISensory`, `ZombieLair`, `WildlifeAI` | #25, #27, #49, #50, #61, #62, #63, #93, #108, #109, #112 |
| **3. Menang dan Kalah** | `GameManager`, `RunLifecycle`, `WorldStateSave`, `RescueEventManager`, `DeathCinematic` | #14, #53, #80, #82, #116 |
| **4. Aturan Shelter** | `BuildManager`, `BuildMenu`, `BuildingIntegrity`, `BaseNavigation`, `BaseDefense`, `SurfaceDetection`, `TideManager` | #26, #46, #47, #67, #78, #79, #86, #90, #91 |
| **4. Sistem Diary** | `DiaryManager`, `ObjectiveManager`, `LoreCollectibles` | #16, #81, #100 |
| **5. Region 1: Gunung Berapi** | `ZoneManager`, `ResourceNode`, `ResourceRespawn`, `DisasterManager` | #70, #73, #77 |
| **5. Region 2: Hutan Tropis** | `RegionGateSystem`, `EnvironmentProfile`, `InfectionSystem` | #64, #68, #92 |
| **5. Region 3: Es dan Militer** | `WarmthSystem`, `RegionalWeather`, `RescueEventManager` | #14, #65, #103, #115 |
| **5. Region 4: Desa** | `HeavyItemSystem`, `SunlightExposure` | #63, #69 |
| **6. Event dan Bencana** | `DisasterManager`, `BeachSupplyManager`, `WeatherManager`, `EnvironmentalHazard` | #50, #70, #71 |
| **7. Lucky Machine** | `LuckyMachine` | #72 |
| **NPC (bagian 3 dan 4)** | `NPCRoster`, `NPCCompanion` | #76 |
| **Combat (pilar L4D)** | `GunController`, `MeleeCombatSystem`, `DamagePipeline`, `HitboxModifier`, `PlayerPinnedState`, `ThrowableSystem`, `DurabilitySystem`, `ToolRepairSystem` | #58, #59, #60, #95, #96, #110, #113 |

Fitur yang diputuskan tidak dipakai, dan blueprint-nya disimpan sebagai arsip: #48 (Farming), #54 (Supply Drop), #105 (World Map), dan #117 (NPC Infection).

## 🧭 Status Pengerjaan

Tahap blueprint sudah selesai. Semua issue `Add Blueprint` sudah ditutup, dan keputusan desain tercatat di `Survival Island/doc/USULAN_TERBUKA.md`.

Pekerjaan berikutnya adalah menulis *script* C# lewat perintah "Jalankan Langkah [X]", mengikuti tabel TAHAP 7 di `Claude.md`. Status tiap Langkah dicatat di `Survival Island/LOGIC/README.md`.

Untuk sistem baru, ikuti alur di atas: buat issue dulu, lalu tulis blueprint-nya.

## 📝 Format Issue

Judul memakai `Add Blueprint: <Nama Sistem>`. Isinya:

```markdown
Satu kalimat tujuan sistem.

**Prioritas:** Tinggi / Sedang / Rendah

## Kenapa dibutuhkan
- Rujukan ke GDD, blueprint, atau catatan yang menunjukkan kekurangan.

## Batasan
- Issue atau blueprint lain yang menangani bagian di luar pekerjaan ini.
- Tandai kalau idenya usulan dan belum tercantum di GDD.

## Checklist
- [ ] Buat file `Survival Island/doc/blueprint/<folder>/<NamaSistem>.md`.
- [ ] Langkah desain yang bisa diperiksa.
```

Sebelum membuat issue, pastikan topiknya belum ada di issue terbuka, issue tertutup, atau blueprint.

## 📐 Format Blueprint

```markdown
# Blueprint: NamaSistem.cs
**Lokasi:** `Survival Island/doc/blueprint/<folder>/NamaSistem.md`
**Deskripsi Utama:** Fungsi sistem dalam satu sampai dua kalimat.
**Issue:** #<nomor>
**Rujukan GDD:** Bagian <nomor>, atau tulis "Usulan, belum ada di GDD".

## 1. Arsitektur & Variabel Utama
Pola yang dipakai, variabel, data ScriptableObject, dan nilai awal.

## 2. Detail Logika & Fungsi
Fungsi utama dan langkah logikanya.

## 3. Hubungan dengan Sistem Lain
Event yang dikirim dan didengar, serta sistem yang dipanggil.

## 4. Kasus Khusus
Kondisi tepi: pool dipakai ulang, game dijeda, permadeath, dan save atau load.

## 5. Keputusan Desain
Keputusan yang dibuat, dan yang masih menunggu developer.

## 6. Skenario Tes
Langkah menguji di Unity dan hasil yang diharapkan.
```

Blueprint lama memakai bagian 1 dan 2 saja. Lengkapi bagian lainnya saat blueprint tersebut diperbarui.

## ✅ Syarat Blueprint Selesai

- [ ] Sesuai dengan aturan `GDD.md`, atau jelas ditandai sebagai usulan.
- [ ] Semua poin checklist issue sudah dikerjakan.
- [ ] Nama kelas, scene, tag, dan enum mengikuti `Survival Island/doc/blueprint/GLOSSARY.md`.
- [ ] Semua sistem yang dipanggil punya blueprint atau issue.
- [ ] Musuh, peluru, dan objek yang sering muncul memakai `ObjectPooler`.
- [ ] Reset status saat objek dipakai ulang dari pool sudah dijelaskan.
- [ ] Data yang harus disimpan dan yang dihapus saat permadeath sudah jelas.
- [ ] Perilaku saat game dijeda sudah jelas.
- [ ] Angka penyetelan bisa dipindahkan ke `GameBalanceConfig` (issue #84).
- [ ] Ada skenario tes yang bisa diikuti di Unity.

## 🎯 Pesan Konfirmasi

Jika Anda (Claude) telah membaca `README.md` ini, konfirmasikan pemahaman Anda dengan mengatakan: *"Saya telah memahami alur kerja Survival Island. Saya akan merujuk pada GDD.md dan Claude.md, menyerap logika dari 60 Seconds, Raft, dan L4D, lalu menyatukan seluruh kode ke dalam folder \\Survival-Island\\Survival Island\\LOGIC."*
