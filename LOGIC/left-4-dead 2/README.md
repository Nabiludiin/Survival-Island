# Modul Logika Game: Left 4 Dead (L4D2) - Survival Island

Modul ini mengimplementasikan sistem **Combat, Gunplay, Dual Threat Enemy AI, dan Horde Mechanics** yang diadaptasi dari mekanik *Left 4 Dead 2* untuk game **Survival Island**.

---

## 📁 File dalam Modul Ini

1. **combatHorde.js**:
   - **First-Person Gunplay**: Mekanik menembak menggunakan Pistol dengan kapasitas magazine 12 peluru, sistem reload ([R]), recoil kamera/senjata, dan muzzle flash dinamis.
   - **Melee Combat**: Senjata jarak dekat menggunakan Kail Pancing (*Hook*) / Kapak dengan animasi tebasan dan deteksi benturan.
   - **Hit Detection & Damage**: Raycasting presisi untuk mendeteksi tembakan pada kepala/badan zombie, armor musuh militer, dan sumber daya alam (pohon, batu).

2. **enemies.js**:
   - **Dual Threat AI (Siklus Siang & Malam)**:
     - **Siang Hari**: Zombie terbakar musnah secara otomatis jika terpapar sinar matahari langsung. Musuh aktif di siang hari murni merupakan **Hewan Buas Teritorial** (Serigala, Babi Hutan, Jaguar Amazon, Beruang Kutub).
     - **Malam Hari**: Hewan buas kembali ke sarang, dan **Horde Zombie** keluar secara masif memburu pemain dan menyerang markas (*shelter*).
   - **Tipe Musuh Khusus**:
     - *Standard Zombie*: Zombie reguler dari penumpang kapal karam.
     - *Camo Moss Zombie*: Zombie hutan tropis (Region 2) yang berkamuflase di semak-semak.
     - *Armored Military Zombie*: Zombie pangkalan militer bersalju (Region 3) dengan rompi anti-peluru (mengurangi 50% damage peluru).
     - *Fast Mud Zombie*: Zombie desa kuno (Region 4) yang bersembunyi di lumpur siang hari dan berlari sangat cepat di malam hari.

3. **events.js**:
   - **Weekly Beach Supply Drop**: Setiap kelipatan hari ke-7 (Hari 7, 14, 21...), arus laut membawa 3 peti kargo kapal ke tepi pantai berisi amunisi, makanan kaleng, dan suplai medis.
   - **Sudden Events (Bencana Alam)**: Erupsi Gunung Berapi Region 1 dengan hujan meteor batu pijar dan pengisian daya statis gratis untuk *The Lucky Machine*.

---

## 🎮 Cara Menjalankan

Modul ini terintegrasi penuh ke dalam game 3D **Survival Island**.
Untuk menjalankan game secara lokal:
1. Buka folder utama Survival Island/
2. Jalankan 
ode server.js atau dobel klik file uka-game.bat.
3. Akses game di browser pada http://localhost:3000.
