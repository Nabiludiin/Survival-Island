# 🏝️ Survival Island - Project Repository & AI Developer Workflow

Selamat datang di repositori proyek **Survival Island**. File `README.md` ini adalah gerbang utama dan panduan alur kerja untuk developer dan AI Assistant (Claude) dalam merancang, menyatukan, dan menulis kode game ini.

Jika Anda adalah Claude, **baca dokumen ini dengan sangat teliti sebelum mengeksekusi atau menulis kode apapun.**

**Status proyek:** tahap desain. Logika game ditulis dulu sebagai blueprint `.md` mengikuti `GDD.md`. Proyek Unity dan *script* C# belum dibuat. Nama resmi tercatat di `docs/blueprint/GLOSSARY.md`.

## 📍 Struktur Direktori Utama

```text
Survival-Island/
├── README.md                     # Alur kerja proyek (file ini)
├── GDD.md                        # Aturan game dan sumber utama desain
├── Claude.md                     # Cara kerja dan standar teknis Claude
├── docs/
│   ├── CATATAN_MEKANIK.md        # Catatan mekanik 60 Seconds!, Raft, dan L4D
│   ├── REFERENSI_MUSUH_L4D.md    # Referensi perilaku dan angka musuh
│   └── blueprint/
│       ├── 60-seconds/           # Fase 1: kapal tenggelam
│       ├── raft/                 # Status, gathering, dan base building
│       ├── left-4-dead/          # Siklus ancaman, AI musuh, spawn, dan gunplay
│       ├── Core_Logic/           # Sistem yang dipakai lintas fase
│       └── Island_Logic/         # Sistem khusus Fase 2 di pulau
├── LOGIC/                        # Bahan referensi game asli, bukan kode proyek
└── Survival Island/
    ├── README.md                 # Cerita, feel, dan gambaran game
    └── LOGIC/                    # Lokasi output script C# nantinya
```

### Tempat menaruh blueprint

| Folder | Isi | Contoh |
|---|---|---|
| `docs/blueprint/60-seconds/` | Logika yang hanya berjalan di Fase 1 | `ShipTimerManager.md` |
| `docs/blueprint/raft/` | Status tubuh, resource, dan bangunan | `SurvivalStats.md`, `BuildManager.md` |
| `docs/blueprint/left-4-dead/` | Siklus siang dan malam, AI musuh, spawn, dan gunplay | `EnemyAI.md`, `DayNightDirector.md` |
| `docs/blueprint/Core_Logic/` | Sistem umum yang dipakai beberapa fase | `GameManager.md`, `SaveSystem.md` |
| `docs/blueprint/Island_Logic/` | Mekanik khusus pulau dan region | `ZoneManager.md`, `LuckyMachine.md` |

## 📚 Urutan Prioritas Dokumen

Kalau dua dokumen berbeda, ikuti urutan ini:

| Urutan | Dokumen | Peran |
|---|---|---|
| 1 | `GDD.md` | Sumber utama aturan game |
| 2 | `Claude.md` | Standar teknis dan cara kerja |
| 3 | `docs/blueprint/` | Rancangan logika tiap sistem, harus mengikuti GDD |
| 4 | Issue GitHub | Daftar pekerjaan dan batas tiap pekerjaan |
| 5 | `docs/CATATAN_MEKANIK.md`, `docs/REFERENSI_MUSUH_L4D.md` | Bahan referensi dan usulan, bukan aturan |
| 6 | `LOGIC/` | Bahan analisis |

Jangan memutuskan sendiri kalau aturannya bertentangan. Catat di [issue #85](https://github.com/Nabiludiin/Survival-Island/issues/85), lalu minta keputusan developer.

## ⚙️ AI Developer Workflow (Alur Kerja Claude)

Untuk merakit game ini dengan benar tanpa kehilangan konteks naratif dan mekanik, setiap sistem mengikuti alur berikut secara berurutan:

```text
GDD.md → Issue GitHub → Blueprint .md → Pull Request → Script C# → Tes di Unity
```

### TAHAP 1: Inisialisasi & Pemahaman Konteks (Baca File Dasar)

Sebelum menyentuh folder kode atau logika, baca file berikut agar memahami dunia game dan batasan-batasannya:

1. **`Survival Island/README.md`:** Memahami cerita dan *feel* game.
2. **`GDD.md` (Game Design Document):** Dokumen paling krusial. Berisi seluruh aturan game, narasi (insiden kapal, diary), mekanik 4 Region (Gunung Berapi, Amazon, Es, Desa), mekanik siang/malam (Hewan vs Zombie terbakar), dan *Win Condition* (merakit Helikopter dan kabur sebelum batas Hari ke-60).
3. **`README.md` (File ini):** Memahami alur kerja, struktur folder, dan instruksi penulisan.
4. **`Claude.md`:** Berisi cara kerja, perintah eksekusi, dan standar teknis yang harus dipatuhi.

### TAHAP 2: Pilih Pekerjaan dari Issue

1. Cek daftar [Issues](https://github.com/Nabiludiin/Survival-Island/issues) dan folder `docs/blueprint/`, termasuk issue yang sudah ditutup.
2. Kalau sistemnya belum ada, buat issue baru dengan [format issue](#-format-issue).
3. Baca bagian "Kenapa dibutuhkan", "Batasan", dan "Checklist" pada issue.
4. Baca blueprint dan issue lain yang disebut, supaya pekerjaan tidak tumpang tindih.

Saat diminta membuat issue saja, berhenti setelah issue terbit. Jangan langsung mengerjakan blueprint-nya.

### TAHAP 3: Ekstraksi Logika dari Referensi

Game ini menggabungkan tiga pilar mekanik utama. Baca `docs/CATATAN_MEKANIK.md` dulu, lalu buka sub-folder di `LOGIC/` hanya kalau butuh detail tambahan.

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

* **Standar Penulisan:**

  * Buatlah arsitektur berbasis *Manager* dengan nama yang sama seperti di blueprint.

  * Pastikan sistem bisa saling berkomunikasi lewat pola resmi di [issue #107](https://github.com/Nabiludiin/Survival-Island/issues/107) (contoh: `DayNightDirector` memberi tahu sistem spawn kapan matahari terbit agar zombie terbakar).

  * Beri komentar pada baris kode yang mengimplementasikan fitur spesifik dari *GDD* (contoh: `// Mekanik Region 4: Spawn Zombie Desa`).

### TAHAP 7: Tes di Unity & Perbarui Dokumen

1. Uji sistem di Unity sesuai skenario pada blueprint.
2. Kalau angka atau logikanya berubah saat playtest, perbarui blueprint terkait.
3. Kalau aturan game berubah, perbarui `GDD.md` setelah developer setuju.

## 🗺️ Peta GDD ke Blueprint

Tabel ini menunjukkan tempat setiap aturan GDD dirancang. Kalau issue sudah selesai, blueprint-nya tersimpan di `docs/blueprint/`.

| Bagian GDD | Blueprint yang sudah ada | Issue yang masih terbuka |
|---|---|---|
| **3. Fase 1: Kapal** | `ShipTimerManager`, `ShipInteractable`, `ShipPlayerController`, `LootSpawnerSystem` | #74, #75 |
| **3. Status: Lapar dan Infeksi** | `SurvivalStats`, `ItemData`, `InventoryManager` | #43, #44, #45, #64, #66, #111 |
| **3. Siklus Siang dan Malam** | `DayNightDirector`, `EnemyAI`, `ObjectPooler` | #25, #27, #49, #61, #62, #63, #93, #108, #109, #112 |
| **3. Menang dan Kalah** | `GameManager`, `UIManager`, `SaveSystem` | #14, #53, #80, #82, #116 |
| **4. Aturan Shelter** | `BuildManager` | #26, #46, #47, #67, #78, #79, #86, #90, #91 |
| **4. Sistem Diary** | `DiaryManager` | #16, #81, #100 |
| **5. Region 1: Gunung Berapi** | `ZoneManager`, `ResourceNode` | #70, #73, #77 |
| **5. Region 2: Hutan Tropis** | `ZoneManager` | #64, #68, #92 |
| **5. Region 3: Es dan Militer** | `ZoneManager`, `WeatherManager` | #14, #65, #103, #115 |
| **5. Region 4: Desa** | `IslandPlayerController` | #63, #69 |
| **6. Event dan Bencana** | `WeatherManager`, `EnvironmentalHazard` | #50, #54, #70, #71 |
| **7. Lucky Machine** | `LuckyMachine` | #72 |
| **NPC (bagian 3 dan 4)** | `NPCCompanion` | #76, #117 |
| **Combat (pilar L4D)** | `MeleeCombatSystem`, `HitboxModifier`, `DurabilitySystem` | #58, #59, #60, #95, #96, #110, #113 |

## 🧭 Urutan Pengerjaan Blueprint yang Disarankan

Kerjakan dari atas ke bawah, karena tahap berikutnya bergantung pada keputusan tahap sebelumnya.

| Tahap | Fokus | Issue |
|---|---|---|
| **0** | Keputusan dan fondasi | #85, #107, #87, #84, #82, #83, #18, #88, #89 |
| **1** | Pemain dan item inti | #59, #60, #66, #102, #111, #101, #17, #118, #56 |
| **2** | Fase 1 | #74, #75 |
| **3** | Survival dan base | #64, #43, #44, #45, #20, #77, #73, #113, #55, #48, #106, #86, #79, #78, #47, #46, #67, #80 |
| **4** | Combat dan musuh | #58, #95, #109, #108, #49, #61, #62, #63, #25, #27, #93, #94, #110, #112, #96, #26 |
| **5** | Region dan event | #68, #65, #69, #13, #91, #92, #115, #70, #71, #54, #50, #72, #76, #117, #81, #103, #90, #97, #98 |
| **6** | Akhir run | #14, #116, #53, #57, #99 |
| **7** | Polesan | #12, #15, #16, #19, #21, #22, #23, #24, #51, #52, #114, #100, #104, #105 |

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
- [ ] Buat file `docs/blueprint/<folder>/<NamaSistem>.md`.
- [ ] Langkah desain yang bisa diperiksa.
```

Sebelum membuat issue, pastikan topiknya belum ada di issue terbuka, issue tertutup, atau blueprint.

## 📐 Format Blueprint

```markdown
# Blueprint: NamaSistem.cs
**Lokasi:** `docs/blueprint/<folder>/NamaSistem.md`
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
- [ ] Nama kelas, scene, tag, dan enum mengikuti `docs/blueprint/GLOSSARY.md`.
- [ ] Semua sistem yang dipanggil punya blueprint atau issue.
- [ ] Musuh, peluru, dan objek yang sering muncul memakai `ObjectPooler`.
- [ ] Reset status saat objek dipakai ulang dari pool sudah dijelaskan.
- [ ] Data yang harus disimpan dan yang dihapus saat permadeath sudah jelas.
- [ ] Perilaku saat game dijeda sudah jelas.
- [ ] Angka penyetelan bisa dipindahkan ke `GameBalanceConfig` (issue #84).
- [ ] Ada skenario tes yang bisa diikuti di Unity.

## 🎯 Pesan Konfirmasi

Jika Anda (Claude) telah membaca `README.md` ini, konfirmasikan pemahaman Anda dengan mengatakan: *"Saya telah memahami alur kerja Survival Island. Saya akan merujuk pada GDD.md dan Claude.md, menyerap logika dari 60 Seconds, Raft, dan L4D, lalu menyatukan seluruh kode ke dalam folder \\Survival-Island\\Survival Island\\LOGIC."*
