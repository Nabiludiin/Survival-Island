# Blueprint: ResourceNode.cs
**Lokasi:** `docs/blueprint/raft/ResourceNode.md`
**Deskripsi Utama:** Komponen panen pada pohon, batu, dan tanaman. Mengatur sisa pukulan, validasi alat, item jatuh, dan status habis. Tabel alat, jadwal tumbuh, dan kepadatan lengkap ada di `ResourceRespawn.md`.
**Issue:** #15, #16, #64, #73, #88, #89
**Rujukan GDD:** Bagian 5 (kayu dan batu Region 1, material medis Region 2). Tabel alat dan jadwal tumbuh adalah usulan, belum ada di GDD.

## 1. Arsitektur & Variabel Utama

`ResourceNode : MonoBehaviour, ISaveable, IRegrowable` berada di scene region, memakai layer `Obstacle`, dan tidak memakai pool. `SaveId` stabil berasal dari `SaveIdTag` (#80).

```csharp
public enum NodeType { Tree, Rock, MedicalHerb }
public enum NodeStatus { Standing, Depleted }
```

`ResourceNodeData`, `ToolClass`, `ToolRule`, dan `IRegrowable` didefinisikan di `ResourceRespawn.md`.

| Variabel | Tipe | Nilai awal | Fungsi |
|---|---|---|---|
| `data` | `ResourceNodeData` | Inspector | Data node |
| `Status` | `NodeStatus` | `Standing` | Status node |
| `currentHits` | `int` | `maxHits` | Sisa pukulan |
| `RegrowAtGameHour` | `float` | -1 | Waktu tumbuh kembali, dalam `TotalGameHours` |
| `standingVisual`, `depletedVisual` | `GameObject` | Dari prefab | Bentuk utuh dan bentuk habis |
| `hitSound`, `breakSound` | `AudioClip` | Inspector | Suara |

`hitParticles`, `dropPrefabs`, dan `dropAmountRange` dihapus.

### Varian `MedicalHerb` (#64)

| Node | Hasil | Alat | Pukulan | Tumbuh kembali | Region |
|---|---|---|---|---|---|
| `Node_Mushroom` | 1 sampai 2 `Mat_Mushroom` | Tangan | 1 | 1 hari game | Hanya Region 2 |
| `Node_SapTree` | 1 sampai 2 `Mat_TreeSap` | `Base_Pisau` | 2, tidak ditebang | 2 hari game | Hanya Region 2 |

### Tag efek pukulan (#15)

| `NodeType` | Tag |
|---|---|
| `Tree` | `VFX_WoodDust` |
| `Rock` | `VFX_StoneDust` |
| `MedicalHerb` | `VFX_LeafBurst` |

## 2. Detail Logika & Fungsi

**`public HitResult ReceiveHit(ToolClass tool, Vector3 hitPoint)`** menggantikan `ReceiveHit(int, string)`:

1. Kalau `Status == Depleted`, kembalikan `Rejected`.
2. Cari damage alat di `toolRules`. Kalau 0, tampilkan `Butuh alat lain`, lalu kembalikan `Rejected`.
3. `currentHits -= damage`.
4. `VFXManager.Instance.Play(tag, hitPoint, rot)`, suara pukulan, dan goyangan singkat.
5. Terbitkan `ResourceNodeHitEvent { nodeType, isDepleted, byPlayer }` (#16).
6. Kalau `currentHits <= 0`, panggil `Deplete()`.
7. Kembalikan `Hit`.

**`private void Deplete()`** menggantikan `ShatterNode()` dan `Destroy(gameObject)`:

1. `ref var s = ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay)`, lalu `s.Range(dropMin, dropMax + 1)` (#89).
2. `SpawnDrops(total)`, memakai `WorldItemManager.Spawn`, bukan `Instantiate`.
3. `Status = Depleted`, dan tukar visual. Objek tetap ada, supaya statusnya tercatat di `RegionStateStore` (#88).
4. Isi `RegrowAtGameHour`, lalu `ResourceRespawn.Instance.MarkDepleted(this)`.
5. Mainkan `breakSound`.

**`public void ForceDeplete(bool withDrops)`** dipakai #70 dan #97.

**`public void Regrow()`** mengembalikan `Standing`, `currentHits`, visual, dan collider.

**`CaptureState()` dan `RestoreState()`** menyimpan `status`, `currentHits`, dan `regrowAtGameHour`.

## 3. Hubungan dengan Sistem Lain

**Event yang dikirim:** `ResourceNodeHitEvent`.

**Event yang didengar:** tidak ada.

| Sistem | Hubungan |
|---|---|
| `ResourceRespawn` (#73) | `MarkDepleted`, jadwal, dan `SpawnDrops` |
| `MeleeCombatSystem` | Memanggil `ReceiveHit` |
| `VFXManager` (#15) | Efek pukulan |
| `RunSeedManager` (#89) | Aliran `Gameplay` |
| `WorldItemManager` (#66) | Item jatuh |
| `RegionStreaming`, `RegionStateStore` (#88), `WorldStateSave` (#80) | Status node |
| `FireSystem`, `Flammable` (#97) | Menghabiskan node lewat `ForceDeplete(false)` |
| `DisasterManager` (#70) | `ForceDeplete` |
| `InfectionSystem` (#64) | Bahan medis |
| `AudioManager` (#16) | Mendengar `ResourceNodeHitEvent` |

## 4. Kasus Khusus

- **Objek pool:** node tidak memakai pool. Item jatuh memakai pool (`ResourceRespawn.md`).
- **Game dijeda:** `ReceiveHit` tidak terpanggil, karena input `Gameplay` mati.
- **Permadeath:** `RegionStateStore` terhapus bersama save run (#82). Semua node kembali `Standing`.
- **Save dan load:** `currentHits` pohon yang dipukul sebagian tetap tersimpan. Node habis mendaftar lagi lewat `MarkDepleted`.
- **Node dipukul setelah habis:** `Rejected`, tanpa pengurangan durabilitas.
- **`Node_SapTree` saat habis:** pohon tetap berdiri, dan batang tampak kering.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** tabel alat, jadwal tumbuh, dan kepadatan (`ResourceRespawn.md`), serta hasil `Node_Mushroom` dan `Node_SapTree` (`InfectionSystem.md`).

**Keputusan teknis di blueprint ini:**

- Node habis hanya berganti status, dan tidak dinonaktifkan, sesuai `ResourceRespawn.md`. Dengan begitu, tunggul tetap punya collider.
- Blueprint ini merangkum kontrak. Kalau ada perbedaan rincian, `ResourceRespawn.md` yang dipakai.

**Usulan yang disetujui developer sebagai nilai awal:** tabel alat dan jadwal tumbuh perlu playtest.

## 6. Skenario Tes

1. Pukul pohon dengan tangan sebanyak 5 kali. **Hasil:** habis, tunggul muncul, dan item jatuh.
2. Pukul pohon dengan kapak. **Hasil:** habis dalam 2 ayunan.
3. Pukul batu dengan tangan. **Hasil:** ditolak, tanpa pengurangan durabilitas.
4. Panen `Node_SapTree` dengan `Base_Pisau`. **Hasil:** 1 sampai 2 `Mat_TreeSap`, dan pohon tetap ada.
5. Panen `Node_Mushroom` dengan tangan. **Hasil:** 1 sampai 2 `Mat_Mushroom`, lalu tumbuh setelah 1 hari game.
6. Jalankan dua run dengan seed sama. **Hasil:** jumlah drop sama.
7. Pukul pohon 3 kali, simpan, lalu lanjutkan. **Hasil:** sisa pukulan sama.
8. Habiskan node, jauhi region sampai dilepas, lalu kembali. **Hasil:** status tetap, dan node tumbuh kalau waktunya sudah lewat.
9. Pukul node apa pun. **Hasil:** efek muncul sesuai `NodeType`, dan `ResourceNodeHitEvent` terkirim.

Belum diuji di Unity, karena blueprint ini baru rancangan.
