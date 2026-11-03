# Blueprint: ObjectPooler.cs
**Lokasi:** `docs/blueprint/Core_Logic/ObjectPooler.md`
**Deskripsi Utama:** Penyelamat FPS dan RAM. Zombie dan peluru tidak boleh di-`Instantiate` berulang. Skrip ini membuat gudang objek pasif yang bisa dipanggil dan dimatikan secara instan.
**Issue:** #82
**Rujukan GDD:** Usulan teknis, belum ada di GDD. Mengikuti aturan Object Pooling di `Claude.md`.

## 1. Arsitektur & Variabel Utama

`ObjectPooler` adalah manager persisten (`IBootstrapService`, #82). Semua objek pool berada di bawah `PersistentRoot`, jadi tidak ikut hilang saat scene berganti.

| Variabel | Tipe | Fungsi |
|---|---|---|
| `Instance` | `ObjectPooler` | Singleton |
| `pools` | `List<Pool>` | `Pool { string tag; GameObject prefab; int size; }` |
| `poolDictionary` | `Dictionary<string, Queue<GameObject>>` | Hanya berisi objek yang sedang tidak aktif |
| `activeByTag` | `Dictionary<string, List<GameObject>>` | Objek aktif per tag |
| `IPoolable` | Interface | `OnSpawned()` dan `OnReturned()` untuk mengembalikan HP, state, skala, dan komponen ke nilai awal |

## 2. Detail Logika & Fungsi

**`public void Initialize()`**, menggantikan `Start()`:

1. Inisialisasi `poolDictionary` dan `activeByTag`.
2. Untuk tiap `Pool`, `Instantiate` prefab sebanyak `size` di bawah `PersistentRoot`, set `SetActive(false)`, lalu `Enqueue`.

**`public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)`**:

1. Kalau tag tidak ada, kembalikan `null`.
2. Kalau antrean kosong, buat satu objek baru dan catat peringatan, atau kembalikan `null` untuk pool yang punya batas keras. Objek yang masih hidup tidak pernah dipakai ulang.
3. `Dequeue`, atur posisi dan rotasi, lalu `SetActive(true)`.
4. Catat ke `activeByTag[tag]`.
5. Panggil `OnSpawned()` pada semua `IPoolable`, lalu kembalikan objeknya.

**`public void ReturnToPool(string tag, GameObject obj)`**:

1. Abaikan kalau objek sudah ada di antrean. Objek tidak boleh masuk dua kali.
2. Panggil `OnReturned()` pada semua `IPoolable`.
3. `SetActive(false)`, lepas dari `activeByTag[tag]`, pindahkan ke bawah `PersistentRoot`, lalu `Enqueue`.

**`public void ReturnAllActive()`** memanggil `ReturnToPool` untuk semua objek aktif, berjalan mundur per daftar supaya penghapusan aman. `RunLifecycle` memanggilnya sebelum scene diganti.

**`public void ResetForNewRun()`** memastikan semua objek sudah kembali. Kalau masih ada yang aktif, jalankan `ReturnAllActive()` dan catat peringatan.

## 3. Hubungan dengan Sistem Lain

**Event:** tidak mengirim dan tidak mendengar event.

| Sistem | Hubungan |
|---|---|
| `RunLifecycle` (#82) | Memanggil `Initialize`, `ReturnAllActive`, dan `ResetForNewRun` |
| `VFXManager` (#15) | Berjalan setelah `ObjectPooler` pada urutan bootstrap |
| `SpawnManager` (#62), `WorldItemManager` (#66), #58, #96, #97, dan lainnya | Memanggil `SpawnFromPool` dan `ReturnToPool` |

## 4. Kasus Khusus

- **Reset objek pool:** setiap objek wajib mengembalikan HP, state, skala, dan komponen lewat `OnReturned` dan `OnSpawned`.
- **Game dijeda:** `SpawnFromPool` tetap bisa dipanggil, tetapi pemanggilnya berhenti karena waktu berhenti.
- **Permadeath:** pool tidak dibuat ulang. Objek hanya dikembalikan, sehingga jumlah objek tetap.
- **Save dan load:** isi pool tidak disimpan.
- **Objek ikut scene region:** pengembalian dilakukan pemilik objek saat `Unloading` (#88), sebelum scene lepas.
- **Objek dihancurkan dari luar:** daftar aktif dibersihkan dari referensi `null` saat `ReturnAllActive`, disertai peringatan.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** pool di `PersistentRoot`, dan `ReturnAllActive` sebelum scene diganti (`RunLifecycle.md`).

**Menunggu developer:** tidak ada yang menghalangi.

## 6. Skenario Tes

1. Boot game. **Hasil:** jumlah objek pool sesuai `size`, semuanya nonaktif.
2. Munculkan 10 zombie, lalu mati. **Hasil:** semua zombie kembali sebelum scene berganti.
3. Ulangi mati dan run baru lima kali. **Hasil:** jumlah objek pool tetap.
4. Kembalikan objek yang sama dua kali. **Hasil:** antrean hanya berisi satu.
5. Pakai ulang zombie yang tadi mati. **Hasil:** HP, state, dan skala kembali ke nilai awal.
6. Habiskan pool berbatas keras. **Hasil:** fungsi mengembalikan `null`, dan objek hidup tidak dipakai ulang.
