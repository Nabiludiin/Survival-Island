# 🏝️ Survival Island - Project Repository & AI Developer Workflow

Selamat datang di repositori proyek **Survival Island**. File `README.md` ini adalah gerbang utama dan panduan alur kerja (workflow) khusus yang dirancang untuk AI Assistant (Claude) dalam merakit, menyatukan, dan menulis kode untuk game ini.

Jika Anda adalah Claude, **baca dokumen ini dengan sangat teliti sebelum mengeksekusi atau menulis kode apapun.**

## 📍 Struktur Direktori Utama

Direktori ini memuat file-file desain dokumen dan folder sumber (source) yang memecah logika game berdasarkan referensi game populer.

## ⚙️ AI Developer Workflow (Alur Kerja Claude)

Untuk merakit game ini dengan benar tanpa kehilangan konteks naratif dan mekanik, Anda (Claude) **DIWAJIBKAN** mengikuti urutan kerja berikut secara berurutan:

### TAHAP 1: Inisialisasi & Pemahaman Konteks (Baca File Dasar)

Sebelum menyentuh folder kode atau logika, Anda harus membaca tiga file utama berikut agar memahami dunia game dan batasan-batasannya:

1. **`README.md` (File ini):** Memahami alur kerja, struktur folder, dan instruksi penulisan kode.

2. **`GDD.md` (Game Design Document):** Dokumen paling krusial. Berisi seluruh aturan game, narasi (insiden kapal, diary), mekanik 4 Region (Gunung Berapi, Amazon, Es, Desa), mekanik siang/malam (Hewan vs Zombie terbakar), dan *Win Condition* (merakit Helikopter di Hari ke-60).

3. **`Claude.md`:** Berisi *prompt* spesifik, perintah eksekusi, dan instruksi *workflow* tambahan dari developer (muham) untuk Anda patuhi selama proses *generate* kode.

### TAHAP 2: Ekstraksi Logika dari Referensi (Baca Folder Sumber)

Game ini menggabungkan tiga pilar mekanik utama. Anda harus masuk ke dalam sub-folder referensi (di dalam folder LOGIC awal, jika ada) dan mengadopsi logika berikut ke dalam *script* C# (Unity):

* **A. Logika "60 Seconds!" (Fase Awal/Scavenging)**

  * **Yang harus diekstrak:** Sistem *timer* (hitung mundur 60 detik di kapal), logika *looting* cepat (ambil peluru, makanan), pembatasan slot *inventory* awal, dan interaksi memilih/menyelamatkan NPC.

  * **Tujuan:** Membuat transisi *state* yang sempurna dari kapal tenggelam menuju pantai pulau dengan membawa barang *default* (Kail & Pistol) beserta barang ekstra hasil *looting*.

* **B. Logika "Raft" (Fase Survival, Status, & Crafting)**

  * **Yang harus diekstrak:** Sistem degradasi bar *Hunger* (lapar) dan *Thirst/Infection*, sistem *gathering* (menebang kayu, menambang batu), memancing menggunakan kail, dan sistem *Base-Building* modular (penempatan lantai, dinding, meja *crafting*).

  * **Aturan Khusus:** Implementasikan restriksi penempatan *collider* di mana pemain tidak bisa membangun struktur *base* di area pasir pantai.

* **C. Logika "Left 4 Dead 2" (Fase Combat & Horde)**

  * **Yang harus diekstrak:** Mekanik FPS *gunplay* (menembak, *recoil*, *reload*), sistem AI musuh (Pathfinding, *aggro*), dan sistem *spawn manager* dinamis.

  * **Aturan Khusus:** Implementasikan siklus Siang & Malam.

    * *Siang:* *Spawn* Hewan Buas (Serigala, Jaguar, dll). Hancurkan (bakar) *GameObject* zombie jika terkena cahaya matahari.

    * *Malam:* Hentikan *spawn* hewan, picu *Horde System* di mana gerombolan zombie dengan *class* berbeda (*Special Infected* per region) akan memburu pemain.

### TAHAP 3: Sintesis & Penyatuan Kode (Output Eksekusi)

Setelah memahami *GDD* dan mengambil sari pati logika dari ketiga referensi game di atas, tugas utama Anda adalah menyatukan semuanya ke dalam satu arsitektur kode (*Script* Unity) yang saling terhubung.

* **Lokasi Output / Penyatuan File:**
  Semua *script* hasil rakitan dan penggabungan logika dari Tahap 2 **HARUS** disatukan dan di-*generate* untuk disimpan ke dalam folder utama:
  👉 `C:\Users\muham\Downloads\Survival Island\Survival Island\LOGIC`

* **Standar Penulisan:**

  * Buatlah arsitektur berbasis *Manager* (misal: `GameManager.cs`, `SurvivalManager.cs`, `DayNightManager.cs`, `HordeManager.cs`).

  * Pastikan variabel antar skrip bisa saling berkomunikasi (contoh: `DayNightManager` harus memberitahu `HordeManager` kapan matahari terbit agar zombie terbakar).

  * Beri komentar pada baris kode yang mengimplementasikan fitur spesifik dari *GDD* (contoh: `// Mekanik Region 4: Spawn Zombie Lapar`).

## 🎯 Pesan Konfirmasi

Jika Anda (Claude) telah membaca `README.md` ini, konfirmasikan pemahaman Anda dengan mengatakan: *"Saya telah memahami alur kerja Survival Island. Saya akan merujuk pada GDD.md dan Claude.md, menyerap logika dari 60 Seconds, Raft, dan L4D2, lalu menyatukan seluruh kode ke dalam folder \\Survival Island\\Survival Island\\LOGIC."*