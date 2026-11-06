# 🤖 CLAUDE - PERINTAH KHUSUS FOLDER KODE (Survival Island/)

File ini berlaku saat Claude bekerja di folder `Survival Island/`, terutama saat menulis *script* C# di `Survival Island/Assets/LOGIC/` (Mode 3: "Jalankan Langkah [X]").

**`Claude.md` di root tetap berlaku penuh.** File ini hanya menambah aturan teknis untuk menulis kode. Kalau keduanya berbeda, ikuti urutan prioritas di `README.md` root: `GDD.md`, lalu `Claude.md` root, lalu blueprint.

---

## 📌 1. Wajib Dibaca Sebelum Menulis Kode

1. `Claude.md` root: aturan dasar, Mode 3, aturan kode TAHAP 6, dan tabel prasyarat TAHAP 7.
2. `Survival Island/doc/GDD.md`: aturan game yang diimplementasikan.
3. Blueprint utama Langkah itu di `Survival Island/doc/blueprint/`, beserta blueprint yang disebut di bagian 3 ("Hubungan dengan Sistem Lain").
4. Blueprint fondasi yang dipakai hampir semua *script*:
   - `Survival Island/doc/blueprint/GLOSSARY.md`: nama resmi kelas, enum, scene, tag, dan layer.
   - `Survival Island/doc/blueprint/Core_Logic/GameEventBus.md`: cara sistem saling memberi kabar.
   - `Survival Island/doc/blueprint/Core_Logic/PhysicsLayers.md`: layer, mask, dan `PhysicsQuery`.
   - `Survival Island/doc/blueprint/Core_Logic/GameBalanceConfig.md`: tempat semua angka penyetelan.
   - `Survival Island/doc/blueprint/Core_Logic/ObjectPooler.md`: kontrak `IPoolable` dan reset objek pool.
   - `Survival Island/doc/blueprint/Core_Logic/WorldStateSave.md`: kontrak `ISaveable` untuk data yang disimpan.
5. `Survival Island/doc/USULAN_TERBUKA.md`: keputusan developer, dan keputusan yang masih terbuka.
6. `Survival Island/Assets/LOGIC/README.md`: struktur folder dan status tiap Langkah.

---

## 🧭 2. Alur "Jalankan Langkah [X]"

1. Cek tabel prasyarat di TAHAP 7 `Claude.md` root. Pastikan blueprint utama dan blueprint prasyaratnya ada di `Survival Island/doc/blueprint/`.
2. Kalau ada keputusan di "Masih terbuka" (`Survival Island/doc/USULAN_TERBUKA.md`) yang menghalangi Langkah itu, laporkan dan tanyakan dulu sebelum menulis kode.
3. Tulis *script* di sub-folder `Survival Island/Assets/LOGIC/` yang sesuai dengan folder blueprint-nya (lihat `Survival Island/Assets/LOGIC/README.md`).
4. Satu kelas utama per file. Nama file dan nama kelas sama dengan nama blueprint.
5. Perbarui kolom **Status** Langkah itu di `Survival Island/Assets/LOGIC/README.md`.
6. Laporkan *script* yang ditulis, bagian blueprint yang belum diimplementasikan, dan apa yang belum diuji di Unity.

---

## ⚙️ 3. Aturan Kode

Aturan TAHAP 6 di `Claude.md` root tetap berlaku. Berikut cara menerapkannya sesuai blueprint:

| Aturan | Cara di kode |
|---|---|
| **Komunikasi antar-sistem** | Pakai `EventChannel<T>.Publish(...)` dan `EventChannel<T>.Raised` dari `GameEventBus.md`. Daftar di `OnEnable`, lepas di `OnDisable`. Jangan memanggil sistem lain yang bukan dependensinya |
| **Raycast dan overlap** | Pakai `PhysicsQuery.Raycast` dan `PhysicsQuery.OverlapSphere` dengan mask dari `PhysicsMasks`. Keduanya selalu memakai `QueryTriggerInteraction.Ignore` |
| **Object pooling** | Musuh, peluru, efek, dan objek yang sering muncul diambil dari `ObjectPooler` dan dikembalikan lewat `ReturnToPool`. Jangan `Instantiate` atau `Destroy` berulang |
| **Reset pool** | Terapkan `IPoolable`. `OnSpawned` mengembalikan HP, state, skala, dan komponen ke nilai awal, sesuai bagian "Kasus Khusus" di blueprint |
| **Angka penyetelan** | Baca dari `GameBalanceConfig.<bagian>.<nama>`. Jangan menulis angka penyetelan langsung di logika |
| **Save dan permadeath** | Data yang disimpan memakai `ISaveable` (`WorldStateSave.md`). Permadeath mereset data lewat `RunLifecycle` |
| **Jeda game** | Ikuti bagian "Kasus Khusus" di blueprint. Timer gameplay memakai `Time.deltaTime`, sedangkan UI saat jeda memakai waktu tak terskala |

### Komentar

- Beri komentar pada kode yang mengimplementasikan aturan GDD, dengan bagian GDD-nya. Contoh: `// GDD bagian 5, Region 3: frostbite menguras stamina dulu, lalu HP`.
- Beri komentar pada kode yang mengikuti keputusan developer atau usulan. Contoh: `// Keputusan developer (USULAN_TERBUKA): obor tetap menyala saat Blizzard`.
- Komentar ditulis dalam Bahasa Indonesia. Nama kelas, fungsi, dan istilah Unity tetap dalam bahasa aslinya.

### Kalau kode harus berbeda dari blueprint

Jangan diam-diam menyimpang. Perbarui blueprint-nya, jelaskan alasannya di laporan, dan perbarui blueprint lain yang kontraknya ikut berubah.

---

## 🚫 4. Larangan

- Jangan menyalin kode atau asset dari `LOGIC/` di root repo. Tulis ulang logikanya dengan kata-kata sendiri.
- Jangan menulis *script* untuk sistem yang belum punya blueprint.
- Jangan memutuskan desain sendiri. Catat di bagian "Masih terbuka" pada `Survival Island/doc/USULAN_TERBUKA.md`, lalu tanyakan ke developer.
- Jangan commit, push, membuat Pull Request, atau menutup issue tanpa perintah developer.
- Jangan menyebut *script* berhasil kalau belum dijalankan di Unity.

---

*Siap menulis kode. Menunggu perintah "Jalankan Langkah [X]".*
