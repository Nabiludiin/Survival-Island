# Blueprint: FlashlightController.cs
**Lokasi:** `docs/blueprint/Island_Logic/FlashlightController.md`
**Deskripsi Utama:** Mengontrol senter, konsumsi baterai, dan pengisian daya.

## 1. Arsitektur & Variabel
*   `public Light flashlight;`
*   `public float maxBattery = 100f, currentBattery;`
*   `public float drainRate = 1f;`
*   `private bool isOn = false;`

## 2. Detail Logika & Fungsi
*   **`void Update()`**
    *   Toggle hidup/mati dengan tombol (misal: 'F').
    *   Jika `isOn` dan `currentBattery > 0`: `currentBattery -= drainRate * Time.deltaTime;`
    *   Jika baterai habis, paksa senter mati dan kedipkan cahaya sebelum mati (efek *flicker*).