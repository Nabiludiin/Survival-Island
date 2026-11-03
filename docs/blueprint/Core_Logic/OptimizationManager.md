# Blueprint: OptimizationManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/OptimizationManager.md`
**Deskripsi Utama:** Mengatur performa game dengan mematikan AI dan animasi musuh yang berada jauh dari pemain (culling).
**Issue:** #62, #90, #93
**Rujukan GDD:** Usulan teknis, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `player` | `Transform` | Dari scene | Pusat jarak |
| `cullingDistance` | `float` | 50 | Jarak culling |

**Dihapus:** `activeEnemies` milik sendiri. Daftar musuh dibaca dari `SpawnManager.Instance.ActiveEnemies`, dan `MaxAlive` dipakai untuk kapasitas (#62). Agen dan tingkatannya dibaca lewat `HordeCrowdManager` (#93).

## 2. Detail Logika & Fungsi

**`OnEnable` dan `OnDisable`** memulai dan menghentikan coroutine `CullingLoop()`.

**`IEnumerator CullingLoop()`**, setiap 1 detik, untuk tiap musuh di `ActiveEnemies`:

1. Lewati agen yang `IsDormantCounted`.
2. Kalau `EnemyAI` berstate `AttackBuilding` dan masih dalam `remoteBase.remoteDistance` (80), jangan di-culling (#90). Tanpa aturan ini, zombie yang menyerang base nyata membeku di jarak 50 sampai 80 m.
3. Kalau jarak > `cullingDistance`, nonaktifkan `Animator` dan `NavMeshAgent`, lalu panggil `ICrowdAgent.SetTier(CrowdTier.Culled)`. Musuh ini tetap dihitung hidup.
4. Kalau jarak <= `cullingDistance` dan sedang di-culling, nyalakan kembali komponennya, perbarui tujuan, lalu kembalikan tingkatan lewat `HordeCrowdManager.UpdateTiers`.

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event.

| Sistem | Hubungan |
|---|---|
| `SpawnManager` (#62) | Sumber `ActiveEnemies` dan `MaxAlive` |
| `HordeCrowdManager` (#93) | `ICrowdAgent.SetTier` dan `UpdateTiers` |
| `RemoteBaseSimulation` (#90) | Sumber `remoteDistance`, dan menangani base di luar jarak itu |
| `EnemyAttack` (#109), `AnimationController` (#12) | Tetap berfungsi saat Animator dimatikan |

## 4. Kasus Khusus

- **Objek pool:** musuh yang kembali ke pool hilang dari `ActiveEnemies`. `OnSpawned` musuh menyalakan komponennya kembali.
- **Game dijeda:** coroutine memakai waktu berskala, jadi berhenti.
- **Permadeath:** ikut hilang bersama scene.
- **Save dan load:** tidak ada data yang disimpan.
- **Musuh lebih dari kapasitas:** tidak terjadi, karena batasnya `MaxAlive`.
- **`maxDistance` spawn:** 45 m sengaja di bawah `cullingDistance` 50 m (#62).

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** sumber daftar dari `SpawnManager` (`SpawnManager.md`). Peran culling jarak jauh (`HordeCrowdManager.md`). Pengecualian base (`RemoteBaseSimulation.md`).

**Keputusan yang perlu dicek developer:** #62 meminta daftar musuh dari `SpawnManager`, dan #93 meminta daftar agen dari `HordeCrowdManager`. Blueprint ini memakai `SpawnManager` sebagai daftar musuh, dan `HordeCrowdManager` hanya untuk tingkatan.

## 6. Skenario Tes

1. Jauhi zombie lebih dari 50 m. **Hasil:** Animator dan agen mati, tier `Culled`, dan zombie tetap terhitung hidup.
2. Dekati kembali. **Hasil:** zombie aktif lagi dengan tujuan baru.
3. Biarkan zombie menyerang base, lalu berjalan 70 m dari base. **Hasil:** zombie tetap menyerang.
4. Jauhi lebih dari 80 m. **Hasil:** alur `RemoteBaseSimulation` mengambil alih.
5. Musuh `IsDormantCounted`. **Hasil:** dilewati.
