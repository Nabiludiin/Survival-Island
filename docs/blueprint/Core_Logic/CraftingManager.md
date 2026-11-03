# Blueprint: CraftingManager.cs
**Lokasi:** `docs/blueprint/Core_Logic/CraftingManager.md`
**Deskripsi Utama:** Catatan peralihan. Isi lama digantikan `Core_Logic/CraftingStation.md`. `CraftingManager` tetap menjadi manager antrean crafting, dengan isi baru.
**Issue:** #77, #80
**Rujukan GDD:** Bagian 3 dan 4 (crafting). Rincian antrean adalah usulan di `CraftingStation.md`.

## 1. Arsitektur & Variabel Utama

**Digantikan:** `unlockedRecipes`, `craftingUIPanel`, dan `TryCraft`. Yang berlaku adalah `RequestCraft`, antrean `CraftJob`, dan `IsRecipeUnlocked`, seperti di `CraftingStation.md`.

`CraftingManager` juga `ISaveable`, dengan `SaveId = world_recipes` dan `LoadOrder = 110`.

## 2. Detail Logika & Fungsi

Semua logika ada di `CraftingStation.md`. `CaptureState` menyimpan node dan resep yang terbuka bersama `TechTreeManager`, ditambah antrean.

## 3. Hubungan dengan Sistem Lain

Lihat `CraftingStation.md`. `TechTreeManager` menyimpan status node lewat data `world_recipes` milik manager ini.

## 4. Kasus Khusus

Lihat `CraftingStation.md`: pool, game dijeda, permadeath, serta save dan load.

## 5. Keputusan Desain

**Sudah ditetapkan dokumen lain:** penggantian isi (`CraftingStation.md`), dan tabel save (`WorldStateSave.md`).

## 6. Skenario Tes

Ikuti bagian 6 di `CraftingStation.md`. Pastikan juga node dan resep terbuka tetap sama setelah keluar dan lanjut.
