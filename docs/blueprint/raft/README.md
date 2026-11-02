# 🪵 Raft Logic - Island Survival & Base Building

## 📌 Deskripsi Modul
Folder ini berisi adaptasi mekanik dari game **Raft**. Modul ini adalah tulang punggung dari **Fase 2 (Island Survival)** di *Survival Island*. 

Fokus utamanya adalah manajemen sumber daya tubuh (Lapar, Haus, Infeksi), mengumpulkan material dari alam, dan membangun tempat berlindung (*Base-Building*) yang modular.

## ⚙️ Mekanik Inti yang Diekstrak (Unity C#)

### 1. Vitals / Status Management
*   **Hunger (Lapar):** Berkurang secara pasif setiap detik via `Update()` atau `Coroutine`. Jika mencapai 0, HP pemain akan mulai terkuras (*starving damage*).
*   **Infection (Infeksi):** Berbeda dengan Raft, ini adalah mekanik khusus. Infeksi naik jika terkena *hit* dari zombie. Membutuhkan *Vaksin* (dari Region 2) untuk menurunkannya.
*   **Pemulihan:** Pemain harus memakan makanan kaleng (hasil *looting* kapal) atau ikan matang untuk memulihkan *Hunger*.

### 2. Modular Base-Building (Grid Snapping)
*   **Logika:** Pemain memegang "Palu Bangunan" (*Build Hammer*). Saat menekan klik kanan, UI menu *crafting* roda (*Radial Menu*) muncul (Pilih: Lantai, Dinding, Pintu).
*   **Grid Placement:** Gunakan *Mathf.Round* atau pembagian *Vector3* untuk membuat *Grid Snapping* (bangunan menempel satu sama lain dengan presisi, tidak berantakan).
*   **RESTRIKSI PANTAI (Sangat Penting):** 
    Sebelum objek di-*Instantiate*, tembakkan `Physics.Raycast` ke arah bawah *preview* bangunan. Jika mengenai *Collider* dengan tag/layer `Sand` atau `Beach`, ubah material *preview* menjadi merah muda/transparan muda, dan blokir input klik kiri. Pemain hanya boleh membangun di tanah keras (rumput/batu).

### 3. Gathering & Fishing
*   **Gathering:** Menyorot pohon/batu dengan kapak/beliung dan menahan tombol klik kiri untuk menambah `Kayu` atau `Batu` ke dalam `InventoryManager`.
*   **Fishing (Kail Pancing):** Menggunakan item dasar *Kail*. Saat dilempar ke badan air (laut/sungai), jalankan jeda *Random.Range* sebelum pelampung tenggelam. Pemain harus menekan tombol tepat waktu untuk mendapatkan *Ikan Mentah*.

## 📂 Rekomendasi Script (Untuk Claude)
Saat mengerjakan folder ini, Claude harus fokus membuat *script* berikut:
1.  `SurvivalStats.cs`: Mengelola bar Lapar, Infeksi, dan HP.
2.  `BuildManager.cs` & `BuildableObject.cs`: Mengatur rotasi, *snapping*, dan validasi penempatan (restriksi pantai).
3.  `ResourceNode.cs`: Ditempelkan pada Pohon/Batu agar memiliki HP dan bisa dihancurkan/di-*farm*.