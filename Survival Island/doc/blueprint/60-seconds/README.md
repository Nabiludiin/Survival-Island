# ⏱️ 60 Seconds! Logic - Sinking Ship Phase

## 📌 Deskripsi Modul
Folder ini berisi kumpulan *script* dan referensi logika yang diadaptasi dari mekanik inti game **60 Seconds!**. Modul ini bertanggung jawab Penuh atas **Fase 1 (Sinking Ship)** dalam game *Survival Island*. 

Tujuan utama dari modul ini adalah memberikan tekanan waktu (Time Pressure) kepada pemain untuk mengumpulkan sumber daya sebanyak mungkin sebelum kapal benar-benar tenggelam, yang akan menentukan tingkat kesulitan mereka saat tiba di pulau (Fase 2).

## ⚙️ Mekanik Inti yang Diekstrak (Unity C#)

### 1. The 60-Second Countdown Timer
*   **Logika:** Sebuah *timer* presisi yang menghitung mundur dari 60.0 hingga 0.0 menggunakan `Time.deltaTime`.
*   **Efek Lingkungan:** Seiring berjalannya waktu, *script* harus memicu *event* visual/audio:
    *   *T = 45s:* Suara alarm kapal semakin keras.
    *   *T = 30s:* Kamera mulai bergetar (*Camera Shake*).
    *   *T = 15s:* Air menyebar ke lantai utama (*Rising Water Level*). Dek bawah sudah mulai basah pada *T = 30s*, mengikuti `ShipHazardManager.md` (#75).
    *   *T = 0s:* Layar *Fade to Black* seketika, menghentikan input pemain.

### 2. Raycast Looting & Interaction
*   **Logika:** Pemain tidak bisa mengambil barang dengan menyentuhnya. Harus menggunakan `Physics.Raycast` dari tengah layar (*crosshair*).
*   **Interaksi:** Jika *Raycast* mengenai `GameObject` dengan Layer "Interactable" (Peluru, Makanan Kaleng, Vaksin, atau NPC), tampilkan UI *Prompt* (misal: "Tekan [E] untuk Mengambil").

### 3. Limited Starting Inventory
*   **Logika:** Pemain hanya memiliki jumlah *slot* inventori yang sangat terbatas di fase ini (menggambarkan kepanikan dan kapasitas tangan kosong).
*   **Base Item:** Pemain secara *default* sudah membawa **Kail**, **Pistol**, dan **Pisau** terlepas dari apa yang mereka pungut.

## 📂 Rekomendasi Script (Untuk Claude)
Saat mengerjakan folder ini, Claude harus fokus membuat *script* berikut:
1.  `ShipTimerManager.cs`: Mengontrol UI *Timer* dan memicu *event* tenggelam.
2.  `ShipInteractable.cs`: Menempel pada barang/NPC agar bisa di-*raycast*.
3.  `ShipPlayerController.cs`: Pengontrol pemain khusus Fase 1 (tidak perlu sistem *crafting* atau menembak di fase ini, murni lari dan *looting*).

## 🔄 Transisi ke Fase 2
Transisi data dan perpindahan scene mengikuti `LifeboatExtraction.md` (#74): barang tersimpan lewat `GameManager.SaveShipOutcome`, lalu `RunLifecycle.EnterIslandPhase()` memuat pulau.