# 🧟‍♂️ Left 4 Dead Logic - Combat, Horde & AI Director

## 📌 Deskripsi Modul
Folder ini berisi injeksi adrenalin dan aksi ke dalam game, mengambil referensi mekanik pertarungan dan *horde* dari **Left 4 Dead (L4D)**. 

Modul ini mengubah game *survival-crafting* biasa menjadi *Action-Horror* yang menegangkan, mengelola siklus ancaman siang/malam, AI musuh yang agresif, dan *gunplay* FPS yang solid.

## ⚙️ Mekanik Inti yang Diekstrak (Unity C#)

### 1. Dual Threat Day/Night Cycle (AI Director)
*   Ini adalah pengatur *spawn* dinamis (*Spawner Manager*).
*   **☀️ Siang Hari (06:00 - 18:00):** 
    *   *Spawn:* Hewan Buas (Serigala, Babi Hutan, Jaguar).
    *   *Behavior:* *Territorial*. Melakukan *Wander* (berkeliling acak). Hanya menyerang jika pemain masuk ke radius *Aggro* mereka.
    *   *Pembersihan Zombie:* Saat matahari terbit (06:00), zombie terbakar sesuai `SunlightExposure.md`. `FindGameObjectsWithTag` dilarang (#87).
*   **🌙 Malam Hari (18:00 - 06:00):**
    *   *Spawn:* *Horde Zombie* (Zombie Biasa, Zombie Armor, Zombie Lumut sesuai Region).
    *   *Behavior:* *Hyper-Aggressive*. Secara konstan melakukan pencarian path (*Pathfinding*) menuju lokasi pemain. Jika pemain berada di dalam *Base*, Zombie akan mengubah target untuk menyerang dan menghancurkan Dinding *Base*.

### 2. NavMesh Pathfinding
*   **Logika:** Sistem AI musuh TIDAK BOLEH menggunakan pergerakan *Translate* biasa (nanti mereka menembus tembok). 
*   SEMUA musuh (hewan dan zombie) harus memiliki komponen `NavMeshAgent`. Peta (pulau) harus di-*Bake* menggunakan Unity NavMesh agar musuh bisa menghindari batu, pohon, dan mengejar pemain melewati pintu dengan cerdas.

### 3. FPS Gunplay (Pistol)
*   **Logika Hitscan:** Menggunakan Pistol (*Base Item*). Saat pemain menembak, gunakan `Physics.Raycast` dari tengah kamera.
*   **Feedback:** Instansiasi partikel *Muzzle Flash* di moncong pistol, suara tembakan, dan partikel darah (*Blood Splatter*) di titik jatuh *Raycast* pada musuh.
*   **Hitbox Damage:** Buat *Collider* terpisah untuk kepala musuh. Jika tembakan mengenai *Collider* Kepala, berikan `Damage * 2` (Kritis/Headshot).
*   **Ammo Management:** Peluru sangat terbatas. Harus di-*reload* jika *magazine* habis.

## 📂 Rekomendasi Script (Untuk Claude)
Saat mengerjakan folder ini, Claude harus fokus membuat *script* berikut:
1.  `SpawnManager.cs`: Mengontrol transisi waktu, memanggil *Wave* zombie malam, dan menghapus zombie siang hari.
2.  `EnemyAI.cs`: State machine (Wander, Chase, AttackBuilding, Die) menggunakan `NavMeshAgent`.
3.  `GunController.cs`: Mengatur peluru, *fire rate*, *recoil* kamera, dan hitungan *damage raycast*.
4.  `ObjectPooler.cs`: (Sangat Wajib) Untuk mendaurnya peluru dan zombie agar game tidak mengalami penurunan FPS.