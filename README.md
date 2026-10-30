# Survival Island 🏝️🧟‍♂️

**Survival Island** adalah proyek game *First-Person Survival Action* yang menggabungkan elemen *scavenging* berbatas waktu, *base-building*, dan pertempuran intens melawan *horde* zombie. Pemain harus bertahan hidup selama 60 hari di sebuah pulau misterius yang terbagi menjadi 4 region, mengumpulkan *resource*, menghadapi hewan buas di siang hari dan zombie di malam hari, serta merakit helikopter untuk melarikan diri.

Repositori ini disusun secara modular untuk memudahkan AI (seperti Claude) dalam membaca dan merakit kode. Logika utama game dipecah ke dalam folder `LOGIC` yang mereferensikan mekanik dari tiga game populer.

## 📂 Struktur Folder `LOGIC`

Arsitektur kode dalam proyek ini dipisahkan berdasarkan tiga pilar mekanik utama. AI diharapkan membaca ketiga folder ini untuk menggabungkan logika permainan secara utuh.

### 1. `LOGIC/60_Seconds_Logic/` (Fase Scavenging)
Diadaptasi dari logika game *60 Seconds!*. Folder ini menangani fase awal permainan di mana pemain berada di kapal yang sedang tenggelam.
*   **Mekanik:** *Timer* hitung mundur 60 detik.
*   **Sistem Sistem:** Pengambilan barang (*looting* peluru, makanan), manajemen kapasitas inventori awal, dan interaksi untuk menyelamatkan NPC sebelum waktu habis.
*   **Transisi:** Membawa *state* barang bawaan dan NPC yang diselamatkan ke dalam fase pulau utama (Fase 2).

### 2. `LOGIC/Raft_Logic/` (Fase Survival & Crafting)
Diadaptasi dari logika game *Raft*. Folder ini mengatur bagaimana pemain bertahan hidup dari nol di pulau.
*   **Manajemen Status:** Pengurangan bar Lapar (*Hunger*), Haus, dan Infeksi (*Infection*) seiring waktu.
*   **Gathering & Crafting:** Sistem menebang pohon, menambang, memancing menggunakan kail (*Base Item*), dan resep *crafting* untuk membuat dinding, lantai, atau stasiun kerja (*workbench*).
*   **Base Building:** Restriksi penempatan objek (tidak boleh di pasir pantai) dan sistem ketahanan bangunan terhadap bencana alam atau serangan.

### 3. `LOGIC/L4D2_Logic/` (Fase Combat & Horde)
Diadaptasi dari logika game *Left 4 Dead 2*. Folder ini mengendalikan sistem pertempuran, AI musuh, dan *gunplay*.
*   **Siklus Siang/Malam (Dual Threat):** 
    *   *Siang:* *Spawn* hewan buas teritorial (Serigala, Jaguar, Beruang). Zombie tidak *spawn* (mati terbakar matahari).
    *   *Malam:* Mekanik *Horde*. Zombie muncul dalam jumlah banyak, mengejar pemain, dan menyerang markas.
*   **Gunplay & Senjata:** Mekanik menembak menggunakan Pistol (*Base Item*), peluru, *recoil*, dan *damage* area.
*   **Special Infected:** Pengaturan *behavior* zombie khusus di setiap region (Zombie Terbakar, Zombie Lumut, Zombie Militer Berarmor, Zombie Lapar).

## 🚀 Alur Kerja untuk AI (Claude)

Jika Anda adalah AI yang bertugas men-generate *script* untuk game ini, ikuti langkah berikut:
1.  Baca spesifikasi lengkap dunia, region, dan *lore* pada file `GDD.md`.
2.  Analisis *script* pergerakan dan interaksi awal di `LOGIC/60_Seconds_Logic/` untuk membangun transisi *scene* dari Kapal ke Pulau.
3.  Implementasikan sistem inventori dan *crafting* dari `LOGIC/Raft_Logic/` sebagai fondasi utama di *scene* Pulau.
4.  Integrasikan sistem *combat*, AI musuh siang/malam, dan *Sudden Events* (bencana alam) dari `LOGIC/L4D2_Logic/` untuk memberikan tekanan (hambatan) pada pemain saat mereka melakukan *grinding*.
5.  Pastikan sistem *quest* logistik (memindahkan *parts* helikopter dari Region 4 ke Region 3) menjadi kondisi penyelesaian (*Win Condition*) pada Hari ke-60.