# Referensi Desain Musuh dari Left 4 Dead

Rangkuman perilaku dan angka musuh sebagai bahan penyetelan musuh Survival Island, ditulis ulang dengan kata-kata sendiri. Dokumen ini melengkapi siklus Director, mob, dan event panik di [CATATAN_MEKANIK.md](CATATAN_MEKANIK.md).

**Status data:** semua angka adalah nilai bawaan dari analisis lokal dan belum diuji di dalam game. Nilai saat bermain bisa berbeda karena konfigurasi. Bagian bertanda "(perkiraan)" adalah kesimpulan yang belum pasti.

**Satuan:** jarak dan kecepatan memakai unit Source. Sebagai perkiraan kasar, 1 unit sekitar 2 cm, jadi perlu disetel ulang di Unity.

---

## 1. Perbandingan cepat

| Musuh | Darah | Kecepatan | Serangan utama |
|---|---|---|---|
| Zombie biasa | 50 | 250 | Pukulan jarak dekat (jarak 20) |
| Hunter | 250 | 300 | Menerkam, lalu 5 damage per 0,5 dtk |
| Smoker | Belum terbaca | Belum terbaca | Lidah jarak 750, cekik 10 damage per dtk |
| Boomer | 50 | 175 | Muntah jarak 300, meledak saat mati |
| Witch | 1000 | 300 | Cakar, damage tergantung kesulitan |
| Tank | 4000 | 210 | Pukulan dan lempar batu |

## 2. Tempo munculnya musuh

- **Special:** jeda awal acak 30-60 dtk, lalu muncul lagi tiap 45 dtk. Dalam kondisi tertentu jedanya hanya 10 dtk.
- **Tank:** setelah satu Tank mati, Tank berikutnya berjeda acak 350-500 dtk.
- **Mob saat ada Tank:** mob ditahan selama Tank hidup, kecuali pemain sudah maju lebih dari 1500 unit sejak Tank muncul. Jadi pemain tidak bisa aman hanya dengan berlari menjauh.

### Urutan finale

1. Serangan horde pertama
2. Boss pertama
3. Serangan horde kedua
4. Boss terakhir
5. Horde saat kabur

Ada jeda sekitar 5 dtk antartahap. Kalau boss terakhir gagal muncul, game tetap lanjut ke tahap kabur, sehingga permainan tidak macet. Urutan ini disusun dari teks log (perkiraan).

## 3. Tank

### Kesulitan

| Kesulitan | Pengali darah | Darah |
|---|---|---|
| Easy | 0,75 | 3000 |
| Normal | 1,0 | 4000 |
| Hard | 2,0 | 8000 |
| Expert | 2,0 | 8000 |

### Lempar batu

- Hanya kalau target lebih jauh dari 250 unit, dengan jeda minimal 8 dtk.
- Sudut lemparan naik seiring jarak, sampai batas maksimum.
- Di Easy, bidikan sengaja dibuat meleset. Di kesulitan lain, bidikan mengarah ke perkiraan posisi target berikutnya.
- Batu bisa meledak saat berada dekat pemain, dengan radius 100 (dikali 0,75 di Easy).

### Pukulan

- Jeda 1,5 dtk setelah kena, dan 1,0 dtk setelah meleset.
- Jeda 0,6 dtk saat membersihkan zombie di jalannya.
- Jangkauan 56 unit.

### Melambat saat ditembak

Kecepatan Tank turun saat terkena tembakan. Efeknya penuh sampai jarak 200, lalu melemah sampai hilang di jarak 400. Akibatnya, menembak dari dekat lebih efektif untuk memperlambat, tapi juga lebih berbahaya.

### Pengaman

| Kondisi | Akibat |
|---|---|
| Macet lebih dari 2 dtk dan target tak terlihat minimal 5 dtk | Ganti target |
| Macet lebih dari 10 dtk dan target tak terlihat lebih dari 15 dtk | Bunuh diri |
| Target tak terlihat lebih dari 60 dtk | Bunuh diri |

Makna persis "macet" dan "tak terlihat" masih perkiraan.

### Terbakar

Tank terbakar selama 30 dtk di Normal, 35 dtk di Hard, dan 40 dtk di Expert.

## 4. Hunter

- **Damage terkaman:** 5 per 0,5 dtk, dikali 2 di Hard dan dikali 3 di Expert.
- **Dekat:** kalau target lebih dekat dari 75 unit, Hunter langsung menyerang.
- **Jauh:** kalau target lebih jauh dari 1000 unit, Hunter membatalkan lompatan menjauh.
- **Membatalkan terkaman:** 50 damage kemungkinan menjadi batas pembatalan (perkiraan).
- **Bonus jarak:** tersedia di kode tapi mati secara bawaan. Kalau dinyalakan, bonusnya naik dari jarak 500 sampai penuh 24 damage di jarak 1000.

## 5. Smoker

- **Target:** dicari dalam jarak 750 unit.
- **Damage:** 3 per detik saat ditarik, dan 10 per detik saat dicekik. Cekikan dikali 2 di Hard dan dikali 3 di Expert.
- **Melepas korban:** kalau Smoker menerima lebih dari 50 damage sejak mulai menggenggam.
- **Tarikan macet:** kalau korban hampir tidak bergerak, cekikan dimulai lebih awal.

## 6. Boomer

- **Muntah:** target harus dalam jarak 300 unit.
- **Jeda sebelum muntah:** 2 dtk di Easy, 1 dtk di Normal, 0,2 dtk di Hard, dan tanpa jeda di Expert.
- **Mundur:** Boomer menjauh saat terbuka dan muntahnya masih mengisi ulang, atau saat target terlalu jauh.
- **Patuh pada Director:** Boomer menahan serangan saat Director berada di tahap istirahat.
- **Ledakan:** radius dalam 130 dan radius luar 200.

## 7. Witch

Witch diam sampai diganggu, jadi pemain bisa memilih untuk menghindarinya.

- **Pemicu amarah:** senter menyala dalam jarak 400 dan terlihat olehnya, atau pemain berada dalam jarak 100.
- **Laju amarah:** naik sekitar 0,4 per detik saat ada ancaman langsung.
- **Terbakar:** mati setelah 15 dtk.
- **Mundur:** minimal 10 dtk, dan selesai kalau sudah jauh 1000 unit atau tidak terlihat selama 10 dtk.

| Target (perkiraan) | Easy | Normal | Hard | Expert |
|---|---|---|---|---|
| Pemain berdiri | 24 | 24 | 33 | 100 |
| Pemain sudah jatuh | 75 | 75 | 75 | 150 |

---

## 8. Penerapan di Survival Island (usulan)

Bagian ini saran, bukan aturan GDD. Angkanya perlu disetel lewat playtest.

| Musuh GDD | Pola referensi | Yang bisa ditiru |
|---|---|---|
| Zombie Militer (Region 3) | Tank | Darah tebal, melambat saat ditembak, dan pengaman saat macet |
| Zombie Lumut (Region 2) | Witch | Diam menyamar sampai pemain terlalu dekat atau menyorotkan cahaya |
| Zombie Desa (Region 4) | Hunter | Cepat dan menyerang dari jarak jauh saat naik dari air |
| Jaguar (Region 2) | Hunter | Menerkam dari atas pohon, dan terkaman batal kalau cukup banyak menerima damage |
| Zombie Tribal (Region 2) | Smoker | Serangan jarak jauh yang menahan pemain, dan terlepas kalau zombie ditembak |
| Zombie Beku (Region 3) | Boomer | Meledak saat mati dengan efek ke area sekitar |

### Pola yang layak ditiru

- **Kesulitan mengubah perilaku:** selain menaikkan damage, ubah juga jeda dan ketepatan bidikan.
- **Serangan yang bisa dilawan:** beri cara melepaskan diri lewat damage, supaya pemain tetap bisa bertindak.
- **Musuh yang bisa dihindari:** beri satu tipe yang tidak agresif sampai diganggu, supaya pemain punya pilihan.
- **Pengaman macet:** pasang di semua musuh besar. Di Survival Island, mereka bisa kembali ke pool alih-alih bunuh diri.
- **Satu pengatur tempo:** semua spawn mengikuti Director, sehingga tekanan pada pemain tetap terkendali.

---

## 9. Yang tidak disertakan

Catatan analisis aslinya tetap di komputer lokal, karena memuat bagian yang tidak perlu dan tidak layak masuk repo:

- Daftar lengkap pengaturan dan teks dari DLL game
- Alamat fungsi dan rujukan ke hasil dekompilasi
- Catatan modifikasi file game
- Analisis HUD, glow, musik, dan kamera, yang tidak dipakai di Survival Island

**Belum tercakup:** gunplay, senjata, dan darah Smoker.
