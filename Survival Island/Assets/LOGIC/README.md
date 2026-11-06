# 🧩 Survival Island - Kode Gabungan (LOGIC)

Folder ini adalah tempat **kode gabungan C#** Survival Island. Semua *script* yang ditulis dari blueprint di `Survival Island/doc/blueprint/` disimpan di sini, sesuai `README.md` di root (TAHAP 6).

**Status:** belum ada *script*. Folder `Survival Island/` sudah menjadi proyek Unity 6 LTS (keputusan developer), tetapi belum pernah dibuka di Unity, dan belum ada Langkah yang dijalankan.

> Jangan tertukar dengan `LOGIC/` di root repo. Folder itu hanya bahan analisis dari game referensi (60 Seconds!, Raft, dan Zombie). Kode dari sana tidak boleh disalin ke folder ini.

## 📍 Struktur Folder

Usulan struktur: sub-folder di sini mengikuti folder blueprint, supaya setiap *script* mudah dicari blueprint-nya.

```text
Survival Island/Assets/LOGIC/
├── README.md          # File ini
├── Core_Logic/        # Dari Survival Island/doc/blueprint/Core_Logic/ (GameManager, ObjectPooler, UIManager, dll.)
├── 60-seconds/        # Dari Survival Island/doc/blueprint/60-seconds/ (Fase 1: kapal tenggelam)
├── raft/              # Dari Survival Island/doc/blueprint/raft/ (status, gathering, base building)
├── left-4-dead/       # Dari Survival Island/doc/blueprint/left-4-dead/ (siang-malam, AI musuh, spawn, gunplay)
└── Island_Logic/      # Dari Survival Island/doc/blueprint/Island_Logic/ (region, cuaca, Diary, Lucky Machine)
```

Sub-folder baru dibuat saat Langkah pertama yang membutuhkannya dijalankan. Folder kosong tidak perlu dibuat lebih dulu.

### Aturan penamaan

- Satu kelas utama per file. Nama file sama dengan nama kelas dan nama blueprint, misalnya `Survival Island/doc/blueprint/Core_Logic/GameManager.md` menjadi `Core_Logic/GameManager.cs`.
- Kelas kecil yang hanya dipakai satu sistem boleh satu file dengan pemiliknya, kalau blueprint-nya menulis begitu.
- Nama kelas, enum, tag, layer, dan scene mengikuti `Survival Island/doc/blueprint/GLOSSARY.md`.

## 🚀 Status Langkah

Langkah mengikuti tabel di `Claude.md` root (TAHAP 7). Perintahnya "Jalankan Langkah [X]".

| Langkah | Isi | Folder utama | Status |
|---|---|---|---|
| **1** | Kerangka `GameManager`, `UIManager`, `ObjectPooler`, `SaveSystem`, dan pola *State Management* inti | `Core_Logic/` | Belum dimulai |
| **2** | Fase 1: `ShipTimerManager`, `ShipInteractable`, `ShipPlayerController` | `60-seconds/` | Belum dimulai |
| **3** | `IslandPlayerController`, `SurvivalStats`, `InventoryManager`, `ItemData` | `Island_Logic/`, `raft/`, `Core_Logic/` | Belum dimulai |
| **4** | `DayNightDirector`, lapar dan infeksi, `BuildManager`, `ResourceNode`, `CraftingManager` | `left-4-dead/`, `raft/`, `Core_Logic/` | Belum dimulai |
| **5** | `EnemyAI`, spawn siang dan malam, gunplay, `MeleeCombatSystem`, `HitboxModifier` | `left-4-dead/`, `Island_Logic/` | Belum dimulai |
| **6** | `ZoneManager`, frostbite, jembatan kayu, barang berat, `WeatherManager` | `Island_Logic/` | Belum dimulai |
| **7** | *Sudden Event*, `LuckyMachine`, `DiaryManager`, alur menang dan kalah | `Island_Logic/`, `Core_Logic/` | Belum dimulai |

Isi kolom **Status** dengan salah satu: `Belum dimulai`, `Sedang ditulis`, `Ditulis, belum diuji di Unity`, atau `Diuji di Unity`. Jangan menulis `Diuji di Unity` sebelum *script* benar-benar dijalankan di Unity.

## 🔗 Hubungan dengan Proyek Unity

**Keputusan developer:** folder `Survival Island/` adalah root proyek Unity, dengan Unity 6 LTS. Folder ini (`Assets/LOGIC/`) adalah tempat semua *script*. `Survival Island/doc/` berada di luar `Assets/`, jadi tidak diimpor Unity.

### Membuka proyek pertama kali

1. Buka Unity Hub, pilih **Add project from disk**, lalu pilih folder `Survival Island/`.
2. Pilih editor Unity 6 LTS. Unity membuat `Packages/`, `ProjectSettings/`, dan `Library/` sendiri. `Library/` dan folder buatan lain diabaikan lewat `Survival Island/.gitignore`.
3. Lewat **Window > Package Manager**, pasang paket yang dipakai blueprint:

| Paket | Dipakai oleh |
|---|---|
| Input System | `InputManager` (#18) |
| AI Navigation | `NavMeshAgent`, `NavMeshSurface`, dan `NavMeshLink` di semua AI musuh, NPC, dan `BaseNavigation` |
| Universal RP | `PostProcessController`, `EnvironmentProfile` |
| Cinemachine | Impulse di `CameraShake`. Blueprint memakai nama kelas Cinemachine 2.x. Kalau yang terpasang 3.x, sesuaikan kodenya dan catat di `CameraShake.md` |
| TextMeshPro | Teks UI. Di Unity 6 sudah termasuk paket uGUI |

4. Commit `Packages/manifest.json`, `Packages/packages-lock.json`, dan `ProjectSettings/` setelah proyek pertama kali dibuka, supaya versi paket sama untuk semua orang.

**Catatan OneDrive:** proyek berada di folder OneDrive. Sinkronisasi OneDrive bisa mengunci file di `Library/` saat Unity menulis. Kalau muncul error akses file, jeda sinkronisasi OneDrive selama Unity terbuka.

## ✅ Sebelum Menambah Script

1. Baca `Survival Island/Claude.md` untuk aturan menulis kode.
2. Pastikan blueprint utama dan prasyarat Langkah itu sudah ada di `Survival Island/doc/blueprint/`.
3. Setelah menulis, perbarui tabel **Status Langkah** di atas.
