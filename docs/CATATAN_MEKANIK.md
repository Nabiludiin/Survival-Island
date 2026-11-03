# Catatan Mekanik Referensi

Rangkuman cara kerja mekanik di 60 Seconds! Reatomized, Raft, dan Left 4 Dead, ditulis ulang dengan kata-kata sendiri sebagai bahan desain Survival Island. Catatan ini tidak berisi kode atau asset dari game aslinya.

## Cara membaca catatan ini

| Label | Artinya |
|---|---|
| **Dari kode** | Dibaca langsung dari hasil dekompilasi |
| **Default kode** | Nilai awal di script. Nilai saat bermain bisa berbeda karena diatur di prefab atau asset |
| **Dari analisis lokal** | Dari catatan `BEDAH_*.md` di `LOGIC/` yang belum diuji di dalam game |
| **Usulan** | Saran untuk Survival Island, bukan aturan dari game referensi atau GDD |

Nilai L4D memakai unit Source. Sebagai perkiraan kasar, 1 unit sekitar 2 cm, jadi perlu disetel ulang di Unity.

---

## 1. 60 Seconds! (Fase 1: Kapal Tenggelam)

### Alur satu ronde (dari kode)

1. **Pra-game:** gerakan pemain dibatasi, ambil dan taruh barang dimatikan, lalu jam mulai berdetak.
2. **Mulai:** sirene berbunyi, musik masuk, kontrol dibuka, dan pintu shelter terbuka serta ditandai.
3. **Scavenge:** pemain bolak-balik mengambil barang dan menaruhnya di shelter.
4. **Waktu habis:** input dimatikan, lalu game memeriksa apakah pemain berada dekat shelter.
5. **Transfer:** barang yang terkumpul dipindahkan ke fase survival.

Hasil ronde ditentukan oleh **posisi pemain saat waktu habis**. Kalau tidak dekat shelter, cabang gagal yang dijalankan. Statistiknya mencatat pemain tewas dan jurnal survival tidak ditampilkan, jadi kemungkinan ronde berakhir di situ.

### Timer (dari kode)

- Total waktu adalah waktu main ditambah waktu persiapan, dan keduanya diatur di Inspector.
- Timer berjalan di coroutine dengan detak per satu detik, bukan pengurangan `Time.deltaTime`.
- Saat hitungan selesai, jam memanggil akhir level pada pengontrol alur game.

### Inventori tangan (dari kode)

Pemain tidak punya tas, hanya beberapa slot tangan.

- Setiap barang punya **berat**, yaitu jumlah slot yang dipakai.
- Barang muat kalau `slot terpakai + berat - 1` masih di dalam batas slot.
- Saat tangan penuh, muncul teks yang mengarahkan pemain ke shelter.
- Menaruh barang di shelter mengosongkan semua slot.
- NPC diperlakukan sebagai barang yang punya data karakter, sehingga aturan slotnya sama.
- NPC yang tidak terbawa dihitung sebagai korban saat ronde berakhir.

### Cara mengambil barang (dari kode)

Game ini **tidak** memakai raycast dari crosshair. Untuk setiap barang, game memeriksa:

1. Apakah barang berada dalam jarak jangkau. Jaraknya diambil dari radius `SphereCollider` barang, dengan default 1,5.
2. Apakah garis dari barang ke pemain tidak terhalang (`Physics.Linecast`).
3. Barang terdekat yang lolos menjadi target.

Warna highlight target berubah saat tangan penuh, jadi pemain langsung tahu barang itu tidak muat.

### Untuk Survival Island (usulan)

- **Titik aman:** GDD belum punya padanan shelter. Sekoci bisa dipakai: barang dibawa ke sana, dan pemain harus ada di dekatnya saat waktu habis.
- **Pemain gagal:** GDD belum mengatur pemain yang tidak sampai ke titik aman. Pilihannya permadeath langsung, atau tetap sampai ke pulau tanpa barang tambahan.
- **Berat barang:** NPC bisa berberat 3 atau 4, sehingga menyelamatkan NPC mengorbankan banyak barang.
- **Interaksi:** raycast crosshair sesuai `Claude.md` tetap cocok untuk first-person. Yang layak ditiru adalah umpan balik warna saat tangan penuh.

---

## 2. Raft (Fase 2: Survival dan Building)

### Status lapar dan haus (dari kode)

- Nilainya turun terus setiap frame. Default kodenya 0,11 per detik, dikali pengali mode permainan.
- Makanan tidak menambah nilai secara instan. Nilai target naik lebih dulu, lalu nilai yang tampil mengejarnya selama sekitar 20 detik (default kode).
- Hasil makanan mengikuti kurva, dan kurva mentah berbeda dari kurva matang. Nilai pengalinya bergantung pada seberapa penuh status saat makan.

### Pengaruh ke HP (dari kode)

Setiap frame, hanya satu kondisi pertama yang cocok yang berlaku:

| Urutan | Kondisi | Akibat |
|---|---|---|
| 1 | Oksigen habis | HP berkurang (tenggelam) |
| 2 | Lapar atau haus nol | HP berkurang |
| 3 | Lapar **dan** haus di bawah batas sehat | HP berkurang |
| 4 | Kondisi tubuh tidak buruk | HP pulih |

Kerusakan ini mengurangi HP bonus lebih dulu, baru HP utama. Layar juga memberi peringatan lewat vignette, aberasi warna, saturasi, dan suara.

### Building (dari kode)

- **Grid:** posisi dibulatkan ke kelipatan ukuran blok, yaitu 0,75 untuk X dan Z, serta 1,21 untuk Y.
- **Rotasi:** dibulatkan ke kelipatan sudut milik tiap blok.
- **Hologram:** dicek setiap frame. Hijau berarti bisa dibangun, merah berarti ada masalah, dan tersembunyi saat menu terbuka atau kondisinya tidak diketahui.

Validasi berhenti pada error pertama, dengan urutan:

1. Blok kosong, atau menu sedang terbuka
2. Posisi terlalu rendah (di dalam air)
3. Bertabrakan dengan blok lain
4. Tidak stabil
5. Bahan kurang

Bangunan hanya bisa ditempatkan kalau hasilnya tanpa error.

### Memancing (dari kode)

1. Kail dilempar, lalu penantian dimulai saat pelampung menyentuh air.
2. Setelah waktu tunggu acak, ikan menggigit dan pelampung ikut terseret.
3. Timer acak kedua mulai berjalan. Kalau pemain terlambat menarik, ikan lolos dan umpan habis.
4. Kalau pemain menarik tepat waktu, hasil diambil acak dari daftar sesuai umpan, umpan habis, dan durabilitas alat berkurang.

Rentang kedua waktu acak itu diatur di prefab, jadi nilainya tidak terbaca dari kode.

### AI hewan (dari kode)

- Saat mulai mengejar, AI menyimpan **titik awal pengejaran**.
- Kalau hewan terlalu jauh dari titik itu (default kode 10), ia pindah ke state cadangan. Inilah yang membuatnya terasa teritorial.
- Target dipetakan ke titik NavMesh terdekat.
- Ada state terpisah untuk target yang tidak terjangkau.

### Untuk Survival Island (usulan)

- **Restriksi pantai:** tambahkan sebagai satu error baru di urutan validasi, sehingga hologram merah dan pembatalan bangunan memakai jalur yang sama.
- **Infeksi:** masukkan ke tabel kondisi HP sebagai sumber kerusakan, supaya urutan prioritasnya jelas.
- **Hewan buas:** jarak batas pengejaran bisa dibedakan per spesies, misalnya jaguar pendek tapi serigala panjang.
- **Memancing:** dua timer acak membuat memancing butuh perhatian, dan ini lebih menarik daripada satu jeda RNG seperti di `Claude.md`.

---

## 3. Left 4 Dead (Fase 2: Horde dan AI Director)

Semua nilai di bagian ini **dari analisis lokal**, dan belum diuji di dalam game.

### Siklus Director

Director mengatur tempo lewat empat tahap:

| Tahap | Fungsi | Keluar ke tahap berikutnya |
|---|---|---|
| Build Up | Tekanan naik, mob boleh muncul (minimal 15 dtk) | Intensitas mencapai 0,9 |
| Sustain Peak | Puncak ketegangan dipertahankan | Timer 3-5 dtk habis |
| Peak Fade | Menunggu situasi reda | Tidak ada Tank dan intensitas di bawah 0,9 |
| Relax | Istirahat, mob berhenti | Timer 30-45 dtk habis, atau pemain maju lebih dari 3000 unit |

Batas jarak di Relax mencegah pemain memanfaatkan masa istirahat dengan berlari jauh.

### Intensitas

- Mengukur seberapa tegang situasi pemain.
- Turun dari 1 ke 0 dalam 30 detik kalau tidak ada tambahan.
- Bertambah saat musuh mati di dekat pemain, dan nilainya bergantung pada jarak (150 sampai 500 unit).

### Mob

Mob biasa hanya muncul kalau tahapnya Build Up, tidak ada event panik atau finale, timer sudah habis, dan tidak ada Tank aktif.

- **Timer:** acak 90-180 dtk di Normal, dan 120-240 dtk di Easy.
- **Ukuran:** mulai dari 10 dan maksimal 30. Makin lama menunggu, mob makin besar, dan laju kenaikannya juga makin cepat.
- **Hitungan:** ukuran mencapai 30 setelah sekitar 126 detik.
- **Arah:** peluang 75 kemungkinan berarti mob lebih sering datang dari belakang. Ini masih perkiraan.
- **Batas:** maksimal 30 zombie biasa sekaligus, dan mega mob berisi 50.

### Event panik

Urutannya: jeda awal, mega mob, tunggu pertarungan selesai, lalu selesai atau jeda 5-7 detik dan gelombang berikutnya.

### Perilaku zombie biasa

Zombie memakai state terpisah untuk berkeliaran, waspada, mengejar, memukul, terbakar, terdorong, terhuyung, dan sekarat. State memukul juga bisa mengenai pintu, dalam jarak 20 unit.

### Pengaman Tank

- Kalau macet dan tidak melihat pemain, Tank mengganti target (macet lebih dari 2 dtk).
- Kalau tetap tidak ada kemajuan, Tank bunuh diri (macet lebih dari 10 dtk, atau 60 dtk tanpa melihat pemain).

Darahnya 4000 di Normal, dengan pengali 0,75 di Easy dan 2 di Hard serta Expert.

### Untuk Survival Island (usulan)

- **Malam sebagai Director:** jalankan siklus empat tahap hanya saat malam, sehingga serangan datang bergelombang dan pemain sempat memperbaiki base.
- **Tingkat ancaman:** naikkan ukuran mob berdasarkan hari dan region.
- **Serangan ke base:** state `AttackBuilding` bisa dibuat seperti state memukul biasa yang targetnya dinding.
- **Terbakar saat fajar:** jadikan state tersendiri, bukan penghapusan langsung, supaya animasinya berjalan lalu zombie kembali ke pool.
- **Ukuran pool:** samakan dengan jumlah zombie terbesar yang boleh hidup sekaligus.
- **Pengaman macet:** zombie yang terlalu lama tersangkut dikembalikan ke pool, supaya tidak ada yang tertinggal di NavMesh.

---

## 4. Keputusan sebelum coding

Bagian ini sudah ditutup. Keputusannya ada di `docs/blueprint/GLOSSARY.md` bagian 1, dan usulan yang masih terbuka dicatat di issue #85.

| Topik | Keputusan |
|---|---|
| Item awal | Kail, Pistol, dan Pisau |
| Zombie saat fajar | Terbakar, lalu kembali ke pool |
| Status haus | Ada |
| Frostbite | Stamina dulu, lalu HP |
| Syarat jembatan | Nilai awal 500 kayu, dan bisa disetel |
| Referensi combat | L4D saja |

## 5. Yang belum tercakup

- **Gunplay L4D:** file script senjata tidak ditemukan di folder ekstrak, jadi recoil, spread, reload, dan pengali headshot belum dicatat.
- **Nilai di prefab:** durasi timer 60 Seconds, waktu tunggu ikan, dan berat tiap barang tidak terbaca dari kode.
- **Crafting Raft dan event 60 Seconds:** belum ditelusuri, karena catatan ini fokus pada fase utama.
- **Pengujian:** belum ada mekanik yang diuji langsung di dalam game atau di Unity.
