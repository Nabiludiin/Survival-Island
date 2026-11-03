# Game Design Document (GDD): Survival Island

## 1. Game Overview
**Logika Inti:** Percampuran mekanik *scavenging* berbatas waktu, *survival base-building* dengan restriksi area, aksi melawan ancaman ganda (hewan buas di siang hari, zombie di malam hari), dan sistem *grinding* logistik antar-region.
**Genre:** First-Person Survival, Action Horror, Base-Building, Resource Management, Roguelike/Permadeath.
**Tujuan Utama:** Bertahan hidup dengan target 60 hari, mengelola status lapar dan infeksi, memindahkan suku cadang dari desa (Region 4) ke pangkalan militer (Region 3), dan merakit helikopter untuk kabur. **Game menggunakan sistem Permadeath: jika pemain mati, game akan diulang sepenuhnya dari awal.**

## 2. Narrative & Latar Belakang
**Insiden Kapal:** Karakter Utama (MC) adalah penumpang kapal pesiar raksasa. Kekacauan terjadi ketika seorang zombie (korban wabah dari daratan yang menyusup) menggigit penumpang. Wabah menyebar cepat, kepanikan membuat kru kehilangan kendali, dan kapal menabrak gunung es hingga karam di dekat pulau misterius. 

**Misteri Pulau:** Pulau ini bukan pulau biasa, melainkan bekas area eksperimen dan observasi yang ditinggalkan. Ada jejak militer, desa yang hancur, dan sistem pengiriman suplai darurat yang masih aktif secara otomatis.

## 3. Core Gameplay Loop & Status Karakter
Game dibagi menjadi dua fase utama yang saling berkesinambungan.

**Fase 1: The 60-Second Scavenge (Kapal Sinking)**
-   **Mekanik:** MC memiliki waktu persis 60 detik di dalam kapal yang miring dan dipenuhi air/zombie untuk mengambil *loot*.
-   **Base Item:** MC otomatis memulai dengan membawa **Kail (seperti di game Raft)** dan **Pistol** dan **Pisau**.
-   **Prioritas:** Mencari peluru, makanan kaleng, dan menyelamatkan NPC (yang masing-masing punya *skill* khusus, misal: mempercepat *crafting* atau ahli medis).

**Fase 2: The Island Survival (60 Days Target)**
-   **Manajemen Status:** 
    *   **Lapar:** Harus diisi dengan memancing (pakai Kail), berburu hewan, atau *looting*.
    *   **Haus:** Harus diisi dengan Air Minum Bersih, kelapa, atau air hujan yang ditampung. Air kotor bisa diminum dengan risiko sakit.
    *   **Infeksi:** Terkena serangan zombie akan meningkatkan meteran infeksi. MC memulai dengan formula vaksin dan stok vaksin yang sangat sedikit. Vaksin baru hanya bisa dirakit menggunakan material dari Region 2.
-   **Siklus Siang & Malam (Ancaman Ganda):**
    *   **Siang Hari:** **Zombie akan terbakar dan mati jika terkena sinar matahari.** Oleh karena itu, tidak ada zombie yang berkeliaran di siang hari. Ancaman utama di siang hari murni berasal dari **Hewan Buas** yang agresif menjaga teritori dan *resource*.
    *   **Malam Hari:** Matahari terbenam dan hewan buas kembali ke sarang. Gelapnya malam memicu **Horde Zombie** keluar dari persembunyiannya untuk memburu MC dan menyerang *base*.
-   **Syarat Menang & Game Over (Permadeath):** 
    *   **Menang:** Menyelesaikan rakitan Helikopter di Region 3 dan lepas landas.
    *   **Kalah / Game Over:** Jika MC **mati** (HP habis karena diserang, lapar, haus, atau infeksi), **maka game akan mengulang dari awal (Reset).** Pemain akan kembali ke Fase 1 (berada di dalam kapal pesiar yang tenggelam) dan harus mengambil barang-barang (looting) dari nol lagi.
    *   **Melewati Hari ke-60:** Bukan Game Over. Pemain tetap bisa bertahan dan kabur dengan helikopter, tetapi zombie akan semakin kuat.

## 4. Mekanik Base Building & Sistem Diary (Tutorial)
**Aturan Shelter:**
-   **Restriksi Pantai:** Pemain dilarang keras membangun *base* di atas pasir pantai (akan hancur oleh air pasang atau dilarang oleh sistem). *Base* harus dibangun di daratan luar garis pantai.
-   **Fungsi Base:** Sebagai tempat berlindung dari serangan zombie di malam hari, lokasi meja *crafting*, dan tempat NPC berlindung.

**Sistem Diary (Peninggalan Penghuni Lama):**
MC akan menemukan *shelter* tua yang hancur. Di dalamnya terdapat buku *Diary* yang berfungsi sebagai *guidebook* dan *lore*:
-   **Diary Bab 1 (Survival Dasar):** Berisi peringatan untuk tidak tidur di pantai, fakta bahwa zombie terbakar oleh matahari siang, cara membuat api unggun, dan cara memakai kail untuk menangkap ikan di tebing karang.
-   **Diary Bab 2 (Obat & Infeksi):** Menceritakan bagaimana penulis diary terinfeksi dan berhasil meracik vaksin dari getah pohon dan jamur khusus yang hanya tumbuh di Hutan Tropis (Region 2). Diary ini memberikan *Blueprint Jembatan*.
-   **Diary Bab 3 (Kebenaran & Pelarian):** Penulis mengungkap bahwa ada helipad dan landasan militer di puncak es (Region 3), tetapi suku cadang helikopternya ditinggalkan di desa bawah (Region 4) saat evakuasi gagal. Ini menjadi panduan objektif akhir pemain.

## 5. World Design & Spesifikasi Region
Pulau terbagi menjadi 4 Region. Pemain harus membuka aksesnya secara bertahap.

**Region 1: Hutan Terbuka & Gunung Berapi (Area Awal)**
-   **Deskripsi:** Hutan pinus yang tidak terlalu lebat dengan gunung berapi aktif di ujungnya. Tempat pemain memulai *survival* dasar.
-   **Ancaman Siang:** Serigala lapar dan Babi Hutan.
-   **Ancaman Malam:** Zombie standar (berpakaian penumpang kapal/penduduk biasa).
-   **Aktivitas Utama:** Menebang kayu dasar, menambang batu, dan membangun *base* awal.
-   **Sudden Event:** Erupsi Gunung Berapi. Langit menjadi merah dan batu pijar jatuh menghancurkan area sekitar, memaksa MC berlindung.

**Region 2: Hutan Tropis Lebat (Amazon-style)**
-   **Syarat Akses:** MC harus mengumpulkan ratusan kayu (nilai awal 500, bisa disetel) dari Region 1 untuk membangun jembatan gantung panjang melewati jurang.
-   **Deskripsi:** Hutan sangat rapat, gelap meski di siang hari, dan penuh tumbuhan rambat.
-   **Ancaman Siang:** Macan Tutul (Jaguar) yang menyergap dari pohon, dan Ular Berbisa.
-   **Ancaman Malam:** Zombie Suku/Tribal, Zombie Lumut (bisa berkamuflase dengan semak-semak) dan zombie standar.
-   **Aktivitas Utama:** *Grinding* material medis (jamur, getah) untuk merakit **Vaksin** agar infeksi dari gigitan zombie bisa diobati.

**Region 3: Pegunungan Es Salju & Pangkalan Militer**
-   **Deskripsi:** Area bersalju dengan suhu ekstrem. Di puncaknya terdapat Pangkalan Militer dan **Landasan Helikopter (Helipad)** beserta kerangka helikopter yang belum selesai.
-   **Ancaman Siang:** Beruang Kutub dan Serigala Salju.
-   **Ancaman Malam:** Zombie Militer (mengenakan armor anti-peluru, sangat tebal), Zombie Beku dan zombie standar.
-   **Mekanik Khusus:** MC harus menggunakan pakaian hangat atau membawa obor, jika tidak stamina akan terkuras lebih dulu, lalu HP, karena *Frostbite*.
-   **Aktivitas Utama:** Area ini adalah tempat MC harus merakit helikopter, namun *parts*-nya tidak ada di sini.

**Region 4: Reruntuhan Desa (Pusat Lore & Resource Heli)**
-   **Deskripsi:** Reruntuhan desa kuno di lembah gelap yang lembab. Tempat eksperimen awal wabah terjadi.
-   **Ancaman Siang:** Anjing liar buas dan Gagak yang menyerang dalam kawanan.
-   **Ancaman Malam:** "Zombie Desa". Karena zombie akan hancur terbakar sinar matahari, di siang hari mereka bertahan hidup dengan cara berendam di dasar air (sungai/danau desa) atau terkubur di dalam lumpur. Saat malam tiba, mereka naik ke darat secara masif dan bergerak sangat cepat.
-   **Aktivitas Utama (Logistik):** Semua suku cadang Helikopter (Baling-baling, Mesin, Tangki Bahan Bakar, dll) tersebar di desa ini.
-   **Tantangan Dinamis:** MC tidak bisa membawa semua *parts* sekaligus karena berat. MC harus **bolak-balik dari Region 4 (mengambil barang) naik menuju Region 3 (memasang ke kerangka helikopter)**, sambil bertahan dari ancaman cuaca, hewan buas, dan zombie di perjalanan.

## 6. Event Mingguan & Bencana Alam (Sudden Events)
-   **Weekly Beach Supply:** Setiap hari ke-7, ke-14, dst., arus laut akan membawa kargo dari sisa-sisa kapal pesiar yang tenggelam ke pinggir pantai. MC bisa turun sebentar ke pantai untuk memungut kargo berisi Air Minum Bersih, Makanan Kaleng, Peluru, atau *Resource* langka.
-   **Bencana Acak:** Badai Tropis, Gempa Bumi (dapat merusak *base* yang tidak di-upgrade dindingnya), dan Erupsi Gunung. Bencana ini memaksa pemain selalu menyiapkan cadangan material untuk perbaikan.

## 7. Unique Mechanic: "The Lucky Machine"
Sebuah mesin eksperimen aneh (gacha/RNG) peninggalan peneliti terdahulu yang bisa memberikan *skin* senjata, *buff* HP/Stamina permanen, atau senjata ajaib ber-damage tinggi (seperti *Laser Pistol* atau *Fire Axe* tak bisa rusak).
-   **Cara Mengaktifkan:**
    1.  **Tumbal Zombie:** Mesin ini memiliki area vakum/kandang. MC harus memancing dan menjebak musuh tertentu (misal: 5 Zombie Militer dari Region 3, atau 3 Beruang Kutub) ke dalam mesin.
    2.  **Energi Alam (Gratis):** Mesin akan otomatis menyala dan gratis diputar selama 30 detik saat terjadi *Sudden Event* skala besar, seperti saat Gunung Berapi di Region 1 meletus dan menyalurkan listrik statis. MC harus berani keluar *base* di tengah bencana untuk memutar mesin ini.