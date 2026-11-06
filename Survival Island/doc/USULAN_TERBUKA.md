# Keputusan atas Usulan Blueprint

**Issue:** #85

Developer sudah meninjau usulan dari semua blueprint. Daftar di bawah menjadi nilai awal, dan boleh berubah saat playtest. Kalau berbeda dari blueprint terkait, ikuti blueprint, karena sebagian usulan disesuaikan setelah daftar ini dibuat.

## Masih terbuka

| Topik | Kondisi sekarang |
|---|---|
| Posisi Lucky Machine Region 1, 2, dan 4 | Bukan keputusan desain. Ditentukan saat layout peta #88, dengan syarat di `LuckyMachine.md` |
| Radius suara tembakan Pistol (#58, #49) | `GunController.md` langkah 10 memakai `stealth.noiseGunshot` (40 m), tetapi tabel senjata memberi `noiseRadius` 35 m dan skenario tes 10 menyebut 35 m. Kode Langkah 5 memakai `WeaponData.noiseRadius` per senjata, dengan `noiseGunshot` sebagai cadangan. Perlu dipilih salah satu |

## Keputusan utama

| Topik | Keputusan |
|---|---|
| Zombie setelah hari ke-60 | Per hari: jumlah +2 persen, HP +2 persen, dan damage +1 persen. Batasnya 1,5x, 2x, dan 1,5x |
| Infeksi mencapai 100 | HP turun bertahap, dan tidak mati seketika |
| Diary | Satu buku di shelter tua, dan bab terbuka bertahap |
| Enchant Lucky Machine | Modifier boleh dipasang ke senjata atau alat. Pada alat, damage pukulan dan kecepatan ayun naik, tetapi jumlah pukulan pada resource tetap |
| Hewan menjaga resource | Suara alat di dalam teritori langsung memicu kejaran. Teritori hewan diletakkan dekat resource lewat `AnimalTerritory`, dengan peluang spawn 0,7 |
| Usulan dari pelengkapan blueprint | Semua usulan baru di ShipInteractable, ShipTimerManager, ShipPlayerController, UIManager, CompassSystem, ZoneManager, WeatherManager, HitboxModifier, EnvironmentalHazard, NPCCompanion, dan FlashlightController disetujui sebagai nilai awal |
| Bahan referensi `LOGIC/` di root | Tetap dilacak git. PR #119 ditutup |
| Ambang lepas jerat Tribal | 40 damage dari pemain, yaitu 2 tusukan Pisau. Menggantikan nilai lama 25 |
| Proyek Unity | Folder `Survival Island/` adalah root proyek Unity 6 LTS. Kode di `Survival Island/Assets/LOGIC/` |
| Peluru di kapal | Satu kotak peluru di kapal berisi 12 butir (satu magazine penuh). `Loot_Ship` hanya memilih jenis barang |
| Berat NPC di kapal | Semua NPC berberat 4, jadi membawa satu NPC memenuhi tangan |
| Lucky Machine | Satu mesin disetiap Region. Kuota: Region 1 8 zombie standar atau 3 serigala, Region 2 5 Zombie Tribal atau 2 Jaguar, Region 3 5 Zombie Militer atau 3 Beruang Kutub, Region 4 5 Zombie Desa atau 4 anjing liar. Tabel hadiah sama, hadiah unik sekali per run, dan putaran tidak bisa dipindah antar-mesin |
| Obor saat badai | Obor tetap menyala saat Storm dan Blizzard, sesuai GDD bagian 5 |
| Vaksin di kapal | Tetap ada sebagai loot langka, dan dihitung sebagai stok awal. Setelah Fase 1, vaksin baru hanya dari material Region 2 |
| Fase 1 | 60 detik adalah batas paling lama. Pemain boleh berangkat dengan sekoci sebelum waktu habis |
| Air es Region 3 | Pakaian hangat tetap melindungi di air es, sesuai GDD bagian 5. Obor padam di air, jadi tidak melindungi |
| Diterkam | Pemain hanya bisa melepaskan diri sendiri. NPC tidak menolong, dan hanya damage dari pemain yang bisa membatalkan terkaman Jaguar atau jerat Tribal |
| Fitur tambahan yang dipakai | #96, #106, dan #112 |
| Fitur yang tidak dipakai | #48, #54, #105, dan #117 |
| Angka inti | Hari 15 menit, 8 sampai 12 kayu per pohon, 5 suku cadang, dan frostbite 1 HP per detik |
| Pusat base (Blood Moon, `BaseNavigation.md` usulan 7) | Rata-rata posisi semua struktur, dihitung ulang saat jumlah struktur berubah |
| Zombie terhalang base | `EnemyAI` memakai `BaseNavigation.PollChase`. Posisi tunggu dan respons traversal tidak ditimpa kejaran, dan target dicoret setelah `threat.unreachableSeconds` |
| Bahan bakar Obor Dinding | Tekan `E` pada obor mati yang belum penuh menambah 1 kayu lalu menyala. Tidak ada tahan `E` 0,6 detik |
| Animasi dorong | Trigger `Shove` di rig tangan, dipakai `PlayerShove` (`AnimationController.md`) |
| Keausan Pisau saat bertarung | 1 per tusukan yang kena musuh (`resource.knifeDecayPerCombatHit`) |
| Pukulan tangan kosong | 5 damage (`equipment.fistDamage`) |
| NPC di kapal | `NPCData` dari daftar di Inspector `LifeboatExtraction` dan `NarrativeSequencer`, aset yang sama dengan `NPCRoster` |
| Urutan bootstrap `NarrativeSequencer` | Urutan 19, setelah `DevConsole` |
| Validasi durabilitas | Validasi Editor `ItemData` untuk item non-inti yang tidak bisa diperbaiki (kecuali obor dan senter), dan nama `DurabilitySystem.GetRatio`, disetujui |
| Angka usulan teknis sistem pendukung | Disetujui sebagai nilai awal, boleh diubah saat playtest: `traversal.rockDamage` 10, `rockWindupSeconds` 1, `rockSpeed` 16, `retreatDistance` 15, `rockLifetimeSeconds` 6, `linkAvoidCost` 1000; `throwable.fireTickSeconds` 1; `lair.nestHP` 300, `registryTickSeconds` 1; `interact.containerNoiseRadius` 8; `npc.doorKeepAwayRadius`, `commandBlockEnemyRadius`, `fireAvoidMargin`; `safety.stuckHintMoveThreshold`; `campfire.skyCheckDistance`, `structureRecheckRadius`, `cookedNotifyDistance`; `debris.bobAmplitude`, `bobFrequency`; volume musik Calm saat Alert 0,5; volume kerumunan jumlah musuh dibagi 24 |

## Usulan yang menjadi nilai awal, per issue

### #12

| Topik | Usulan |
|---|---|
| Model pemain: tangan first-person saja atau tubuh penuh | Tangan first-person saja, tanpa tubuh penuh. Mengurangi kebutuhan aset; konsekuensinya pemain tidak punya bayangan tubuh. |
| Aset animasi | Pakai placeholder sampai aset final dipilih. Nama state dan parameter tetap sama saat klip diganti. |

### #13

| Topik | Usulan |
|---|---|
| Menyelam | Tetap ada dengan napas 30 detik, tanpa hadiah pada versi awal. Hadiah bisa ditambah lewat #106. |
| Kecepatan berenang | 2,5 m/s, sama dengan saat membawa Heavy Item. |
| Regenerasi stamina saat berenang | Tidak ada, termasuk saat mengapung, supaya pemain tidak bisa aman dari horde tanpa batas. |
| Musuh di air | Tidak bisa berenang. Hanya Zombie Desa yang masuk air untuk bersembunyi (#63). |
| Bahaya lain di laut | Tidak ada ikan atau hewan laut pada versi awal, karena tidak ada di GDD. |

### #14

| Topik | Usulan |
|---|---|
| Pemanasan mesin sebelum lepas landas | 120 detik bertahan di dekat kokpit. Alternatifnya langsung terbang saat E ditekan. |
| Gelombang saat pemanasan | RequestPanicWave hanya saat malam, karena zombie terbakar di siang hari. |
| Event final hari ke-60 | Pesan kritis dan satu gelombang besar pada malam terakhir, tanpa mengubah syarat menang. |
| Bahan tambahan saat memasang suku cadang | Tidak ada. Pemasangan langsung selesai saat barang tiba. |
| NPC yang ikut terbang | Semua NPC hidup dianggap ikut, dari mana pun posisinya. |
| Save setelah tiap pemasangan suku cadang | Ya, memakai SaveReason.Manual. |
| Tekanan pada hari ke-60 | Pertahankan pesan kritis dan satu gelombang targetNightWaveSize 25 pada malam hari ke-60 sebagai tekanan sebelum zombie menguat, ditambah notifikasi saat hari ke-60 terlewat. Field dan config diganti nama dari final menjadi target. GDD tidak memintanya, jadi boleh dihapus. |

### #15

| Topik | Usulan |
|---|---|
| Warna darah zombie dan hewan | Satu warna pada versi awal, karena EnemyHitEvent tidak membawa jenis musuh. |
| Jumlah efek impact per SurfaceType | Tiga kelompok: VFX_Impact, VFX_ImpactDust, VFX_ImpactWood, bukan satu efek per SurfaceType. |

### #16

| Topik | Usulan |
|---|---|
| Apakah pop-up diulang setelah permadeath | Tidak diulang. Daftar seen disimpan di PlayerPrefs (settings.tutorial.seen), terpisah dari save run. |
| Bentuk tampilan tutorial | Pop-up kecil tanpa menjeda game, bukan layar penuh. |
| Daftar pemicu pop-up | 19 pop-up pada tabel blueprint, berbasis data sehingga bisa dikurangi tanpa mengubah kode. Lucky Machine sengaja tanpa pop-up. |
| Pilihan mematikan tutorial | Tersedia di menu Settings, dengan nilai awal menyala. |

### #17

| Topik | Usulan |
|---|---|
| Peti loot | Isi keluar ke tanah sebagai WorldItem, bukan membuka panel. Panel hanya untuk peti simpanan (#46). |
| Interaksi tahan | Peti loot 1 detik dan kargo 2 detik, batal saat berlari atau diserang. Benda kecil langsung terambil. |
| Interaksi saat membawa Heavy Item | Hanya pintu yang boleh dipakai. |
| Suara membuka peti | Menimbulkan suara kecil lewat NoiseEmitter, supaya looting malam punya risiko. Radius mengikuti #49. |

### #18

| Topik | Usulan |
|---|---|
| Tombol awal selain E dan klik kiri | Mengikuti tabel Action Map: Space, Left Shift, Left Ctrl, R, 1-8, scroll, F, G, Q, Tab, C, B, J, M, dan Esc. |
| Dukungan gamepad | Versi awal hanya KeyboardMouse. Aset disiapkan agar skema Gamepad bisa ditambah nanti. |
| Lari dan jongkok | Ditahan, bukan toggle. Pilihan toggle bisa ditambah lewat #19. |
| Ganti tombol oleh pemain | Semua boleh diganti, termasuk E, kecuali Pause, Cancel, Submit, Point, Click, dan ToggleConsole. E tetap nilai awal. |

### #19

| Topik | Usulan |
|---|---|
| Kapan setting berlaku | Volume dan sensitivitas langsung berlaku, grafis lewat tombol Terapkan karena perubahan tekstur bisa membuat game tersendat. |
| Rentang Brightness | Dibatasi 0,8 sampai 1,2, supaya Region 2 tetap gelap sesuai GDD Bagian 5. |
| Render pipeline | Blueprint mengasumsikan Built-in Render Pipeline memakai QualitySettings. File ProjectVersion dan manifest tidak terbaca saat diperiksa. Kalau URP, Shadows ditulis ke UniversalRenderPipelineAsset dengan API publik yang sama. |
| Versi Unity untuk batas tekstur | Pakai globalTextureMipmapLimit (Unity 2022.2 ke atas), atau masterTextureLimit untuk versi lebih lama. |
| Resolusi, layar penuh, dan VSync | Tidak ikut versi awal karena belum diminta issue. Rekomendasinya ditambah di tab Grafis sebelum rilis. |

### #20

| Topik | Usulan |
|---|---|
| Suplemen Med_Supplement | Kecepatan x1,2 selama 60 detik, sesuai contoh issue. Item belum ada di GDD, dan sumbernya hanya loot (#103). |
| Aturan tumpuk suplemen | Minum ulang hanya mengisi durasi (Refresh), tanpa kecepatan bertumpuk. |
| Batas efek aktif | Maksimum 8 efek. Debuff wajib menggantikan buff yang paling cepat habis. |

### #21

| Topik | Usulan |
|---|---|
| Pemicu langkah | Berbasis jarak (stepDistance 1,75 m, seirama head bob), bukan Animation Event. |
| Nilai awal volume langkah | stepDistance 1,75 m, jongkok 40 persen, dan berlari 130 persen. |
| Suara air dangkal | Tidak ada suara khusus di versi awal. Bisa ditambah bersama #13 dan #91. |
| Suara langkah musuh | Di luar issue ini. Kalau dibutuhkan, zombie memakai komponen 3D terpisah yang di-reset lewat IPoolable. |

### #22

| Topik | Usulan |
|---|---|
| Cara sistem lain mengirim pesan | Memanggil NotificationManager.Instance.Show langsung, seperti AudioManager. Event hanya untuk pesan di tabel blueprint. |
| Bentuk Event Log | Tampilan di dalam panel Pause lewat tombol Log, tanpa UIPanelId baru. Judul issue menyebut Event Log, tetapi isinya tidak dirinci. |
| Posisi notifikasi | Pojok kanan atas, dengan pesan terbaru di bawah. |
| Nilai awal notifikasi | 4 baris tampil, 3 detik untuk pesan biasa, dan 5 detik untuk pesan penting. |
| Barang berkurang | Tidak ditampilkan. Hanya delta positif yang menjadi pesan. |

### #23

| Topik | Usulan |
|---|---|
| Masa hidup PostProcessController | Prefab per scene di ShipPhase dan IslandPhase, bukan manager persisten. |
| Bentuk efek layar | Vignette untuk HP dan napas, Chromatic Aberration ditambah saturasi untuk infeksi, serta DOF Gaussian ditambah warna air untuk menyelam. |
| Batas HP efek layar | Vignette mulai pada 40 persen HP, dan denyut mulai saat tanda LowHP di 25 persen. |
| Pilihan mematikan Chromatic Aberration | Tambahkan chromaticAberrationEnabled di SettingsManager. Infeksi tetap terlihat lewat warna. |
| DiseaseOverlay | Tetap di HUD, tidak dipindah ke Volume. |
| Layer Volume | Memakai layer Default, tanpa layer baru di PhysicsLayers.md. |

### #24

| Topik | Usulan |
|---|---|
| Alat getaran | Cinemachine Impulse hanya sebagai sumber sinyal, tanpa Virtual Camera, lalu diubah menjadi ICameraOffsetSource. Versi paket perlu dipastikan saat proyek Unity dibuat. |
| Getaran tembak dan bidikan | GunFire memakai channel Visual (posisi dan roll saja), supaya akurasi hanya diatur AddRecoil dan spread. |
| Kekuatan getaran per senjata | Field baru WeaponData.cameraShakeScale, dengan nilai awal Base_Pistol 0,6. |
| Damage berkala | Tanpa getaran untuk lapar, haus, infeksi, beku, racun, api, dan tenggelam. |
| Nilai awal preset getaran | Pakai tabel preset di blueprint, lalu setel lewat playtest. |

### #25

| Topik | Usulan |
|---|---|
| Runner, Tank, dan Spitter: digabung atau jenis terpisah | Digabung sebagai pola pada varian GDD: Runner ke Zombie Desa, Tank ke Zombie Militer, jarak jauh ke Zombie Tribal. Tidak ada enemyId baru. |
| Zombie Tribal: jenis serangan, jarak, dan cara lepas | Jerat proyektil jarak 5-14 m yang bisa dihindari, menarik dan menahan lewat PlayerPinnedState. Lepas lewat damage dari pemain ke Tribal (40, yaitu 2 tusukan Pisau, diputuskan developer), aksi #110, jarak di atas 18 m, atau garis pandang terputus. |
| Zombie Lumut: state menyamar dan pemicu bangun | AIState.Dormant dengan rage bergaya Witch: bangun seketika dalam 4 m, saat kena damage, atau kena suara. Rage naik saat pemain berdiri dalam 8 m atau disorot senter dalam 12 m. Tidak bisa kembali menyamar. |
| Zombie Beku: melambatkan atau menurunkan Warmth | Menurunkan Warmth 25 lewat ledakan saat mati, tanpa melambatkan. Mati karena api tidak meledak. |
| Zombie Desa: kecepatan dan serangan | Kecepatan 7,5 m/s dan terkaman 3-10 m tanpa menahan pemain, dengan pemulihan 1,5 detik sebagai celah membalas. |
| Pengaman macet varian besar dan statistik varian | Ambang L4D (2/5 detik ganti target, 10/15 detik atau 60 detik tak terlihat kembali ke pool), tanpa EnemyKilledEvent dan drop. Statistik di bagian 1 sebagai nilai awal playtest. |

### #26

| Topik | Usulan |
|---|---|
| Daftar jebakan | Spike Trap, kawat berduri, dan api unggun |
| Pemain dan NPC menginjak jebakan | Tidak terkena, karena GDD hanya membahas ancaman musuh dan base dipakai NPC berlindung |
| Hewan menginjak jebakan | Ikut terkena |
| Musuh menghindari atau mengincar jebakan | Musuh tidak menghindar lewat NavMesh, dan AttackBuilding tidak mengincar jebakan |
| Keausan jebakan | Spike Trap rusak setelah 12 kali kena dan kawat 40 kali, lewat HP struktur |
| Batas jumlah jebakan | 30 jebakan per region, tanpa menghitung api unggun, lalu disetel lewat playtest |

### #27

| Topik | Usulan |
|---|---|
| Siklus empat tahap Director | Dipakai hanya saat malam: BuildUp, SustainPeak, PeakFade, Relax. |
| Kurva ancaman hari 1 sampai 60 | Tabel threatCurve: mob 6 ke 30, total per malam 24 ke 180, HP 1 ke 1,5 kali, jeda serang 1 ke 0,75 kali. Perlu diuji bersama jumlah peluru. |
| Pengali Region 4 dan batas kecepatan zombie | Region 4 dikali 1,2 sebagai arti 'secara masif' di GDD. Kecepatan zombie dibatasi 7,8 m/s, di bawah lari pemain 8 m/s. |
| Apakah hewan siang ikut diskalakan Director | Tidak. Director hanya untuk zombie malam. |
| Angka zombie semakin kuat setelah hari ke-60 (GDD tidak memberi angka) | Pengali per hari di GameBalanceConfig.aiDirector: jumlah +2 persen sampai 1,5 kali, HP +2 persen sampai 2 kali, damage +1 persen sampai 1,5 kali. Dikalikan pada baris hari 60, sehingga batasnya nightBudget 270, HP 3 kali, dan damage sekitar 1,88 kali. Perlu diuji bersama jumlah peluru. |
| Batas zombie aktif setelah hari ke-60 | maxAlive tetap 30, sama dengan SpawnManager.maxZombiesAlive dan aman untuk HordeCrowdManager, sehingga mobSize yang diminta bisa 45 tetapi yang muncul tetap dibatasi 30. Alternatifnya, naik sampai 40 lewat capOverride setelah uji performa. |

### #43

| Topik | Usulan |
|---|---|
| Wadah air | Tanpa botol kosong: satu item air habis sekali minum. Pilihan lain adalah botol isi ulang, yang menambah aturan slot dan wadah. |
| Air laut | Tidak bisa diminum, karena GDD tidak menyebut laut sebagai sumber air. |
| Air kotor | Tersedia tanpa batas di sungai dan danau, dengan risiko keracunan 35 persen. |
| Merebus air kotor | Bisa direbus menjadi air bersih di api unggun (#44). |
| Angka penampung hujan | Penuh 4 porsi, 0,5 porsi per jam game saat Rain, 0,75 saat Storm, dan tidak mengisi saat Snow. |
| Pengali haus aktivitas lain | Berenang dan memanjat 1,25 kali, selain berlari yang sudah 1,5 kali. |

### #44

| Topik | Usulan |
|---|---|
| Waktu gosong | 30 detik setelah matang, dan hasilnya Food_Burnt dengan lapar 5. |
| Bahan bakar | 1 kayu 60 detik, batas 300 detik, dan menambah kayu otomatis menyalakan api tanpa alat. |
| Jumlah baris masak | Tiga baris per api unggun. |
| Makanan busuk | Tidak bisa dimasak. |
| Merebus air | Air kotor menjadi bersih dalam 20 detik. |
| Pengaruh cuaca pada api | Rain menghabiskan bahan bakar 1,5 kali. Storm dan Blizzard memadamkan api tak beratap. |
| Cara membuat api unggun | Dari Kit_Campfire yang dibuat dengan tangan, 5 kayu dan 3 batu, tanpa palu. |
| Arang | Kayu dibakar menjadi Mat_Charcoal, sebagai bahan Norit (#45). |

### #45

| Topik | Usulan |
|---|---|
| Daftar penyakit | Keracunan makanan, racun ular, pendarahan, dan infeksi luka. |
| Peran Antibiotik | Hanya untuk infeksi luka, karena GDD menjadikan vaksin obat gigitan zombie. Keputusan ini sama dengan #64. |
| Obat racun ular | Norit juga mengobati racun ular, supaya tidak perlu item penawar baru. |
| Peluang pendarahan | 25 persen pada pukulan melee dengan damage minimal 15. |
| Infeksi luka | Tidak mematikan sendiri. Hanya menghentikan pemulihan HP dan menurunkan stamina. |
| Air kotor | Peluang keracunan 35 persen, sama dengan #43. |
| Efek layar sakit | Kedipan hijau dengan alpha sampai 0,18 per 2,5 detik, dengan pilihan aksesibilitas tanpa kedip. |
| Resep obat | Norit dari arang, Perban dari kain, dan Antibiotik dari bahan Region 2 di MedicalBench. |

### #46

| Topik | Usulan |
|---|---|
| Jumlah slot peti | 20 slot per peti (GameBalanceConfig.storage.chestSlotCount), sedikit lebih besar dari 15 slot inventori. |
| Item inti dan item wajib di peti | Tidak boleh masuk peti: item dengan isCoreItem atau isDroppable = false ditolak. |
| Isi peti saat peti hancur | Isi tumpah sebagai WorldItem yang tidak hilang otomatis. Pilihan isi ikut hancur terlalu menghukum saat base diserang. |
| Makanan di peti | Tetap membusuk dengan kecepatan normal, tanpa perlambatan dari peti. |
| Jumlah yang dipindahkan klik kanan | Klik kanan memindahkan seluruh tumpukan. Memindahkan sebagian memakai seret dan pisah di panel Inventory. |
| Zombie dan peti | Zombie mengabaikan peti karena peti tidak bertag BaseWall. Peti hanya rusak karena ledakan, api, bencana, atau runtuhan. |

### #47

| Topik | Usulan |
|---|---|
| Biaya perbaikan | 1 bahan per ayunan, dan tiap ayunan memulihkan 15 persen HP maksimum. |
| Perbaikan saat diserang | Ditolak selama 2 detik setelah menerima damage, supaya satu dinding tidak bisa ditahan hanya dengan menekan klik kiri. |
| Tingkat material | Dua tingkat, kayu dan batu. Dinding batu 900 HP, tahan api, dan tahan gempa. |
| Biaya upgrade | Dinding 10 batu, dan pintu 8 batu. |
| Upgrade pintu | Pintu ikut bisa di-upgrade walau checklist hanya menyebut dinding, supaya base batu tidak punya satu titik lemah kayu. |
| Upgrade lantai, atap, dan tangga | Tidak bisa di-upgrade pada versi awal. |
| HP setelah upgrade | Mengikuti rasio HP sebelum upgrade. |
| Syarat membuka upgrade | Terbuka lewat TechTreeManager, dengan node awal mengikuti #77. Sampai #77 selesai, unlockNodeId dikosongkan. |

### #49

| Topik | Usulan |
|---|---|
| Sistem stealth | Kini aturan GDD bagian 3 (Mengendap). Dipakai, dan stealth.enabled = true. Saklar hanya untuk tes. |
| Aturan Aggro penuh di Claude.md bertentangan dengan stealth | Zombie malam selalu bergerak ke HuntHint di sekitar pemain, tetapi baru mengejar langsung setelah Aware. |
| Angka pandang dan suara | Pakai sightRange 22 m, kerucut 120 derajat, suara jalan 8 m, lari 20 m, tembakan 40 m, lalu setel lewat playtest. |

### #50

| Topik | Usulan |
|---|---|
| Pemakaian Blood Moon | Kini aturan GDD bagian 3 (setiap 10 hari). Dipakai, dan bloodMoon.enabled = true. Saklar hanya untuk tes. |
| Durasi Blood Moon | Satu malam penuh, dari NightfallEvent sampai DaybreakEvent. Zombie tetap terbakar saat fajar sesuai GDD bagian 3. |
| Override cuaca | Hanya visual. Cuaca tetap berjalan, dan hanya warna langit serta kabut yang ditimpa. |
| Ukuran serangan | Empat gelombang, lalu menjaga 20 zombie. Ukuran 12 pada hari ke-10 sampai 32 pada hari ke-60, dengan batas 50. |
| Pusat serbuan saat pemain jauh dari base | Kalau pemain lebih dari 120 meter dari base, horde berpusat pada pemain, dan kerusakan base dihitung lewat #90. |
| Hari ke-60 | Ikut Blood Moon sebagai puncak akhir run. |
| Peringatan Blood Moon | Satu notifikasi jam 12:00 pada hari yang sama, tanpa hitung mundur HUD. |
| Blood Moon setelah hari ke-60 | Jadwal berlanjut tiap 10 hari, ukuran gelombang berhenti di 50, dan RequestBurst menerima pengali HP dan damage dari AIDirector.GetPostTargetModifiers. Ini mengubah kontrak SpawnManager.RequestBurst. Pengali belum diterapkan pada penyergapan kargo, penyergapan tidur, dan sarang zombie, jadi cakupannya perlu dikonfirmasi. |

### #51

| Topik | Usulan |
|---|---|
| Cakupan Hit Marker. Issue hanya menyebut peluru. | Berlaku untuk semua serangan pemain, yaitu pistol, melee, dan barang lempar. |
| Marker dari jebakan, karena jebakan #26 mengirim byPlayer = true. | Tampil hanya kalau titik kena berada dalam pandangan, tanpa mengubah EnemyHitEvent. |
| Marker headshot yang berbeda. | Dipakai dengan skala 1,3x (GameBalanceConfig.combatFeedback.headshotMarkerScale). |
| Pengaturan untuk mematikan marker. | Tambahkan showHitMarkers di SettingsManager saat #19 ditulis, dengan nilai awal aktif. |

### #52

| Topik | Usulan |
|---|---|
| Area di luar Pantai dan Hutan. | Empat profil cadangan per RegionType, supaya tidak ada area yang sunyi. |
| Prioritas saat area Pantai dan Hutan bertumpuk. | Pantai menang (prioritas 20 lawan 10). |
| Lama crossfade dan jeda keluar area. | Crossfade 3 detik dan jeda 1,5 detik sebagai nilai awal, lalu disetel lewat playtest. |
| Suara jangkrik saat Blood Moon dan bencana. | Dihilangkan, supaya suasananya lebih mencekam. |

### #53

| Topik | Usulan |
|---|---|
| Model tubuh pemain untuk ragdoll | PlayerBodyRig di prefab pemain, nonaktif selama hidup. |
| Lewati adegan kematian | Skip boleh setelah 2 detik. |
| Gerak lambat saat mati | 1,2 detik pada skala 0,35, hanya untuk kematian oleh pukulan. |
| Gaya adegan per penyebab | Lima gaya: Ragdoll, Collapse, Sink, Zombify, dan Blackout. |
| Teks penyebab kematian | Teks singkat di layar hitam sebelum GameOver tampil. |

### #54

| Topik | Usulan |
|---|---|
| Pemakaian Supply Drop (belum ada di GDD sebagai mekanik) | Diputuskan developer: tidak dipakai. Blueprint disimpan sebagai arsip. |
| Jam jatuh peti | Hanya siang, jam 08:00 sampai 14:00, supaya pemain sempat pulang sebelum malam. |
| Lokasi titik jatuh | Cincin 120 sampai 260 meter dari pemain, karena hanya region yang dimuat yang punya Terrain dan NavMesh (#88). |
| Penjaga peti | Tanpa musuh tambahan. Risiko berasal dari jarak dan hewan teritorial. Tambahkan RequestBurst kecil kalau playtest terasa terlalu aman. |
| Kedaluwarsa peti | 24 jam game, supaya peti bertahan satu malam. |
| Isi peti | Tanpa Med_Vaccine, mengikuti GDD bagian 3. |
| Bentrok dengan Weekly Beach Supply (#71) | Hari kelipatan 7 dilewati. |

### #55

| Topik | Usulan |
|---|---|
| Lokasi memancing | Boleh di semua WaterBody (laut, sungai, danau). Tebing karang lebih cepat dengan biteDelayMultiplier 0,75, sesuai Diary Bab 1. |
| Akibat benang putus | Ikan hilang dan jeda 5 detik (lineBreakRecoverSeconds). Kail tidak rusak dan tidak butuh bahan, karena item inti tidak boleh hilang. |
| Jenis hasil tangkapan | Hanya Food_RawFish pada versi awal, tanpa jenis ikan dan sampah. |
| Serangan saat memancing | Serangan langsung (Melee, Bullet, Fire, Explosion) membatalkan sesi. Damage berkala (lapar, haus, infeksi, frostbite, racun) tidak membatalkan. |
| Nilai Food_RawFish | Hunger +12, busuk dalam 12 jam game, poisonChance 0,5 mengikuti DiseaseManager. |

### #56

| Topik | Usulan |
|---|---|
| Konsol di build rilis | Tidak ada, supaya pemain tidak bisa mengaktifkannya dan permadeath terjaga. |
| Perilaku setday | Tidak mengejar hari yang terlewati, sehingga kargo, bencana, dan pembusukan tidak berjalan pada hari itu. |
| Run yang memakai cheat | Ditandai [CHEAT] dan tidak masuk statistik #57. |
| Alias item | Nama pendek seperti wood diterima, sesuai contoh issue. |

### #57

| Topik | Usulan |
|---|---|
| Pemakaian sistem statistik | Dipakai, karena RunLifecycle.md sudah memakainya, walaupun belum ada di GDD. |
| Arti Telemetry | Lokal saja, tanpa pengiriman data. |
| Run yang ditinggalkan lewat Run Baru | Angkanya masuk total, tetapi tidak dihitung sebagai run atau kematian. |
| Tombol hapus statistik | Ada, dengan ConfirmDialog. |
| Metrik tambahan di luar checklist | animalsKilled, headshots, npcsRescued, dan playSeconds. |

### #58

| Topik | Usulan |
|---|---|
| Pistol sebagai item inti terhadap DurabilitySystem | Bisa rusak (Broken, tidak bisa menembak) tetapi tidak pernah hancur, dengan perbaikan Hand. Selaras dengan ToolRepairSystem.md. |
| Gangguan reload | Batal saat ganti senjata, mulai berlari, atau tertahan. Tetap lanjut saat kena serangan. Peluru baru berkurang di akhir reload. |
| Angka Base_Pistol | Damage 30, magazine 12, jeda tembak 0,25 detik, reload 1,6 detik, jarak 60 meter, suara 35 meter. Disetel lewat playtest. |
| Isi awal Pistol | 12 peluru penuh dan cadangan 0, supaya pemain yang gagal mengambil peluru di kapal masih bisa bertahan. |
| Laser Pistol | Tanpa cadangan, 8 tembakan, isi daya 3 detik, damage 60, dan tanpa keausan. GDD hanya menyebut damage tinggi. |
| Peluru NPC | Tak terbatas dan tidak memakai inventori pemain, dengan damage rendah 15. |

### #59

| Topik | Usulan |
|---|---|
| Friendly fire | Peluru dan melee pemain tidak melukai NPC. Ledakan buatan pemain melukai pemain dan NPC 50 persen. Ledakan lingkungan melukai semua pihak. |
| Armor Zombie Militer | Badan 0,35, kaki 0,5, kepala tanpa pengurangan, sehingga headshot menjadi cara utama melawannya. Angka final di #25. |
| Pemain merusak bangunan sendiri | Peluru dan melee pemain tidak merusak bangunan. Bongkar bangunan lewat #79. |
| Falloff ledakan | Berkurang linier sampai 25 persen di tepi radius. |
| DeathReason.None | Tambahkan ke GLOSSARY sebagai nilai pertama, untuk RunEndedEvent saat menang (usulan #82). |

### #60

| Topik | Usulan |
|---|---|
| Jumlah dan bentuk hotbar | 8 slot yang merujuk ke slot inventori lewat instanceId, sehingga tidak mengurangi 15 slot inventori. |
| Tangan kiri | Satu slot untuk Senter atau Obor. Bisa bersama Pistol, Pisau, dan Palu, tetapi tidak dengan Kail, Kapak, dan Beliung. |
| Jeda ganti item | 0,35 detik total (0,15 turun dan 0,2 naik). |
| Item aktif hancur | Beralih ke slot terisi berikutnya, lalu tangan kosong kalau tidak ada. |
| Konsumsi di hotbar dan tangan kosong | Konsumsi boleh masuk hotbar dan dipakai dengan klik kiri. Menekan tombol slot aktif menyimpan item, dan klik kiri menjadi pukulan lemah. |

### #61

| Topik | Usulan |
|---|---|
| Cara Gagak terbang dengan aturan wajib NavMeshAgent | NavMeshAgent tetap di NavMesh tanah dengan baseOffset 4 m, lalu baseOffset turun ke 1,2 m saat menukik. |
| Hubungan hewan dan zombie saat fajar atau senja | Saling mengabaikan: tidak saling menargetkan dan tidak saling memberi damage. |
| Syarat menguliti bangkai | Base_Pisau harus sedang dipegang, dan durabilitasnya tidak berkurang karena Pisau item inti. |
| Angka per spesies (HP, kecepatan, damage, aggro, jarak batas, hasil buruan) | Pakai tabel nilai awal di bagian 1 WildlifeAI.md, lalu setel lewat playtest. |

### #62

| Topik | Usulan |
|---|---|
| Bobot RegionSpawnTable per region | Pakai bobot di bagian 1 SpawnManager.md, misalnya Region 2 malam: standar 50, Tribal 30, Lumut 20. |
| Batas musuh hidup dan target hewan | 30 zombie, 50 saat override, batas 8 hewan, dan target 4 hewan. |
| Arah kemunculan dari belakang pemain | behindChance 0,75 mengikuti CATATAN_MEKANIK, dengan catatan angka sumbernya masih perkiraan. |
| Musuh saat pemain berpindah region | Musuh yang sedang mengejar tetap mengejar selama regionnya dimuat, sisanya kembali ke pool setelah 70 m. Region dilepas mengembalikan semuanya. |
| Jarak spawn dan pengaman macet | Spawn 20 sampai 45 m. Macet 20 detik di luar pandangan, atau 45 detik kalau terlihat, lalu kembali ke pool. |

### #63

| Topik | Usulan |
|---|---|
| Aturan fajar: semua terbakar atau hanya yang terkena sinar | Raycast ke arah matahari, dengan batas berlindung maxShelteredSeconds 60 detik, supaya tetap tidak ada zombie berkeliaran siang hari sesuai GDD. |
| Kanopi Region 2, bangunan, dan badai | Kanopi dan bangunan hanya menunda paling lama 60 detik. Badai tidak melindungi. InteriorZone mengikuti #112. |
| Akibat Zombie Desa terlambat sembunyi | Terbakar seperti zombie lain. |
| Zombie yang sembunyi bisa dilihat atau diserang | Tidak terlihat dan tidak bisa diserang, tetapi ada tanda gelembung atau lumpur bergerak. |
| Kapasitas HidingSpot dan jam mulai sembunyi | Air 8, lumpur 4, total minimal 30, dan mulai pukul 05:00. |

### #64

| Topik | Usulan |
|---|---|
| Kenaikan infeksi: naik sendiri atau hanya saat diserang | Hanya saat diserang. selfProgressPerGameHour bernilai 0 dan bisa dinaikkan tanpa mengubah kode. |
| Tahap infeksi dan penaltinya | Empat tahap (0-25, 25-50, 50-75, 75-100). Batas stamina x1; 0,9; 0,75; 0,5. Regenerasi stamina x1; 1; 0,75; 0,5. Intensitas penglihatan 0; 0,15; 0,4; 0,8. |
| Kenaikan infeksi per jenis musuh | Standar 5, Tribal 6, Lumut 8, Militer 10, Beku 5, Desa 6, hewan 0. Runner, Tank, dan Spitter mengikuti #25. |
| Stok vaksin awal dan vaksin dari kapal | 2 dosis, dan vaksin dari kapal menambah stok itu. Dikonfirmasi developer: vaksin tetap ada di loot kapal. |
| Efek dan pemakaian vaksin | Menurunkan infeksi 50, suntik 4 detik, dan batal saat diserang tanpa kehilangan vaksin. |
| Resep vaksin | rec_vaccine terbuka sejak awal, di MedicalBench, 10 detik, dengan 2 Mat_TreeSap dan 2 Mat_Mushroom. |
| Node MedicalHerb | Jamur dipanen dengan tangan dan getah dengan Base_Pisau, hanya di Region 2. Jamur tumbuh kembali 1 hari game, getah 2 hari game. |
| Pengaruh NPC medis | Vaksin 25 persen lebih kuat selama NPC medis hidup, tanpa syarat jarak. |
| Peran Vaksin dan Antibiotik (juga di #45) | Vaksin satu-satunya obat Infection zombie. Antibiotik hanya untuk WoundInfection, dan infectionDelta-nya 0. Checklist #45 menulis 'Antibiotik untuk infeksi', jadi developer perlu mengonfirmasi tafsir ini. |

### #65

| Topik | Usulan |
|---|---|
| Bentuk dan pemilik Warmth | Nilai 0 sampai 100 dengan pemilik WarmthSystem. SurvivalStats hanya membaca isFreezing. |
| Hujan dan air di luar Region 3 | Hanya menurunkan Warmth sampai batas aman (hujan 40, badai 30, air 30), sehingga frostbite tetap khusus Region 3. |
| Durasi dan padamnya obor | Bakar 300 detik. Rain memadamkan setelah 20 detik. Diputuskan developer: Storm dan Blizzard tidak memadamkan obor, sesuai GDD bagian 5. |
| Slot pakaian hangat dan resep | Satu slot di luar inventori tanpa durabilitas pada versi awal. Resep 4 Mat_Hide dan 2 Mat_Rope di WoodBench. |
| Obor saat membawa Heavy Item | Tidak boleh (equipment.offHandAllowedWhileCarrying = false), karena barang dipegang dua tangan. Obor disimpan otomatis dan durasinya tetap berjalan. |
| Ambang Warmth untuk membeku | Masuk membeku pada 20 dan keluar pada 30, sehingga stamina habis sekitar detik ke-24. Ini sedikit lebih lambat dari perhitungan 20 detik di GameBalanceConfig.md. |

### #66

| Topik | Usulan |
|---|---|
| Pemulihan bertahap | Makanan dan minuman naik selama 20 detik mengikuti Raft, tanpa kurva mentah dan matang. HP dan vaksin tetap langsung. |
| Pembatalan pemakaian item | Terkena serangan, berganti item, atau mulai lari membatalkan pemakaian, dan item tidak hilang. |
| Umur makanan | Daging mentah 12 jam game, ikan matang 36, daging matang 48, kelapa 96. Makanan kaleng dan air tidak busuk. |
| Akibat makanan busuk | Tiga tingkat: Segar, Basi (hasil 75 persen), dan Busuk (hasil 25 persen dan peluang keracunan 80 persen, dihubungkan ke #45). |
| Penghapusan WorldItem | Item yang dibuang hilang setelah 48 jam game, dengan batas 64 item lepas. Loot dan barang dari kapal tidak hilang. |
| Isi item konsumsi | Mengikuti tabel contoh di blueprint, selaras dengan target 33 lapar dan 25 haus per porsi di GameBalanceConfig. |

### #67

| Topik | Usulan |
|---|---|
| Syarat tidur | Hanya malam, tanpa musuh aktif dalam 25 meter, tanpa bencana, bukan Blood Moon, tidak membeku, serta lapar dan haus minimal 10. |
| Apakah tidur harus di dalam base | Tidak wajib, supaya pemain baru tetap bisa tidur. Base tetap lebih aman lewat peluang gangguan yang lebih kecil dan pemulihan HP. |
| Lama tidur | Satu malam penuh selesai dalam 20 detik nyata (GameBalanceConfig.sleep.timeMultiplierAsleep = 2160). |
| Lapar dan haus selama tidur | Berkurang setengah dari laju biasa pada jam game yang sama (drainRatioAsleep = 0,5). Satu malam mengurangi sekitar 25 lapar dan 34 haus. |
| Apakah tidur memulihkan HP dan stamina | Ya. HP 2 per jam game hanya di dalam base, dengan lapar dan haus tidak nol serta infeksi di bawah 50. Stamina penuh kalau tidur minimal 4 jam dan tidak terbangun paksa. |
| Definisi dan hukuman tidur di pantai | Tidur di pantai berarti kasur dalam 8 meter dari pasir. Selalu ada penyergapan, air pasang membangunkan dan mengurangi 30 stamina, dan tidak ada pemulihan. |
| Spawner selama tidur | Spawner berhenti dan diganti gangguan yang dihitung sekali saat mulai tidur: 25 persen di dalam base, 60 persen di luar, 100 persen di pantai. |
| Tidur saat hari ke-60 | Diizinkan, dengan ConfirmDialog bahwa batas waktu akan terlewati. |

### #68

| Topik | Usulan |
|---|---|
| Syarat akses Region 4 (seam S2) | Diary Bab 3 ditemukan, karena bab itu mengungkap lokasi suku cadang. ObjectiveManager sudah menaruh Bab 3 di Region 2. |
| Syarat akses Region 3 (seam S3) | Terbuka bersama S2 tanpa syarat item. Dingin sudah menjadi rintangan alami, dan pemain tidak terkunci karena gagal menemukan satu benda. |
| Kerusakan jembatan | Tidak bisa dirusak zombie, hewan, atau bencana, karena kerugiannya terlalu besar dalam game permadeath. Alternatifnya, jembatan punya HP dan bisa diperbaiki lewat #47 dan #116. |
| Bentuk Blueprint Jembatan | Penanda hasBridgeBlueprint di RegionGateSystem, diisi saat Diary Bab 2 terbuka dan tersimpan di world_gates, bukan item inventori dan bukan node TechTreeManager. Sama dengan RegionGateSystem.md dan ObjectiveManager.md. Pembaca lain memakai properti HasBridgeBlueprint. |
| Pencegahan berenang melewati jurang | Dinding tak terlihat GateBarrier di air. Arus laut lebih alami, tetapi lebih sulit dijamin. |

### #69

| Topik | Usulan |
|---|---|
| Dua suku cadang tambahan untuk mengisi 'dll' di GDD | Baling-baling Ekor (Part_TailRotor) dan Panel Kendali (Part_Avionics), sehingga totalnya 5. |
| Pengali kecepatan per barang | Semua 0,5 pada versi awal. speedMultiplierOverride disediakan, dan weightKg tidak memengaruhi angka permainan. |
| Pemicu jatuhnya barang | Damage 25 ke atas, ledakan, dan diterkam. Zombie biasa tidak menjatuhkan barang, supaya perjalanan tidak terasa acak. |
| Barang masuk air dalam | Langsung kembali ke posisi valid terakhir, bukan tenggelam dan diselami. |
| Penanda suku cadang | Hanya barang yang sudah terlihat dalam 40 meter. Alternatifnya, semua barang ditandai setelah Diary Bab 3. |

### #70

| Topik | Usulan |
|---|---|
| Cakupan erupsi | Fireball hanya di Region 1, sedangkan langit merah dan gemuruh terasa di semua region. Pakai, karena GDD menempatkan erupsi di Region 1 dan pemain di mana pun tetap tahu ada peluang spin gratis. |
| Prioritas dengan Blood Moon dan malam | Blood Moon menang, tidak ada bencana pada hari Blood Moon, dan bencana mulai pukul 08:00 sampai 14:00. Pakai, supaya pemain tidak menerima horde dan bencana sekaligus. |
| Definisi bencana besar | Erupsi selalu besar. Gempa dan badai hanya besar pada skala 2. |
| Bencana berjalan saat keluar game | Ikut disimpan dan dilanjutkan, supaya bencana tidak bisa dihindari dengan keluar game. Pakai. |
| Peluang, durasi, dan damage bencana | Pakai tabel di blueprint sebagai nilai awal, lalu setel lewat playtest. |
| Bencana setelah hari ke-60 | Peluang dan damage bencana tetap sama, karena GDD hanya menyebut zombie yang semakin kuat. |

### #71

| Topik | Usulan |
|---|---|
| Lokasi kargo | Hanya pantai selatan Region 1 (#88), karena region ini selalu terbuka. Pakai. |
| Jumlah kargo | 2 sampai 4 kargo, dengan 3 isi per kargo. |
| Batas waktu kargo | Muncul pukul 06:00 dan hilang pukul 00:00 pada puncak pasang, supaya 'turun sebentar' di GDD dan #91 saling menguatkan. Pakai. |
| Risiko pantai | Hewan pada siang dan zombie pada malam, tanpa Zombie Desa, karena pantai berada di Region 1. Pakai. |
| Isi kargo berubah menurut hari | Tiga tier di tabel blueprint: air dan makanan lebih dulu pada pekan awal, peluru naik pada pekan tengah. Pakai. |
| Barang langka | Mat_Scrap, Seed_Potato, dan Med_Antibiotic, tanpa vaksin. Daftar bisa diperpanjang setelah #103 selesai. |
| SaveId kargo | world_supply_beach, terpisah dari #54, karena Register menolak SaveId ganda. |
| Kargo mingguan dan ketersediaan bekal setelah hari ke-60 | Kiriman terus berjalan tiap 7 hari dengan 4 kargo dan tier Hari 42+. Titik loot tetap tidak terisi ulang sepanjang run, sehingga run panjang mengandalkan panen, kargo, dan kotak suplai. Jumlah peluru perlu diuji bersama pengali HP. |

### #72

| Topik | Usulan |
|---|---|
| Nasib AddEnergy | Dihapus, karena GDD meminta musuh tertentu dijebak. |
| Arti permanen | Selama satu run. Buff dan skin hilang saat permadeath, sesuai GDD bagian 3. |
| Lokasi mesin | Diganti GDD bagian 7: satu mesin di setiap region. Mesin Region 3 tetap di (350, 2050). Posisi tiga mesin lainnya ditentukan saat layout #88. |
| Batas putaran gratis | 3 putaran per jendela 30 detik. |
| Tabungan putaran | Maksimum 1, dan gerbang menutup setelah kuota penuh. |
| Kuota per musuh | Tiap mesin memakai musuh regionnya. Region 1: 8 zombie_standar atau 3 serigala. Region 2: 5 zombie_tribal atau 2 jaguar. Region 3: 5 Zombie Militer atau 3 Beruang Kutub (GDD). Region 4: 5 zombie_desa atau 4 anjing liar. Tiap baris memberi 1 putaran. |
| Hadiah antar-mesin | Keempat mesin memakai tabel hadiah yang sama. Batas hadiah unik dihitung bersama lewat LuckyRewardLedger, dan putaran tersimpan tidak bisa dipindah antar-mesin. |
| Peluang tier | S 5 persen, A 25 persen, B 70 persen, dari blueprint lama. |
| Aturan hadiah ganda | Item unik tidak berulang, buff bertumpuk sampai 3 kali, dan satu modifier per senjata. |
| Hadiah tier B berupa barang biasa | Tetap ada SpawnItem peluru dan kayu, karena issue meminta jenis ini walau GDD tidak menyebutnya. |
| Blood Moon memicu mode gratis | Tidak. GDD hanya menyebut Sudden Event. |

### #73

| Topik | Usulan |
|---|---|
| Tabel alat dan node | Pohon: tangan 1, kapak 3, lainnya 1. Batu: kapak 1, beliung 3, lainnya ditolak. Jamur semua 1. Pohon getah hanya pisau. |
| Alat minimum | Batu besar butuh Kapak atau Beliung, pohon getah butuh Pisau, dan pohon biasa boleh dipukul tangan. |
| Jadwal tumbuh kembali | Pohon 5 sampai 10 hari, dan batu 10 sampai 12 hari per region. Jamur 1 hari dan pohon getah 2 hari mengikuti InfectionSystem.md. |
| Syarat tumbuh kembali | Minimal 25 meter dari pemain, dan radius 3 meter bebas bangunan, pemain, NPC, serta Heavy Item. |
| Kepadatan node per region | Region 1 minimal 120 pohon dan 40 batu, dengan 80 pohon dalam 200 meter dari titik mulai. Region lain sesuai tabel. |
| Item jatuh masuk air atau tertimbun | Item kembali ke pangkal node, tanpa hilang. |

### #74

| Topik | Usulan |
|---|---|
| Sekoci | Sekoci di dek atas menjadi titik aman dan titik setor. Menyetor mengosongkan seluruh tangan. |
| Gagal mencapai sekoci | Permadeath langsung dengan DeathReason.LeftOnShip. Pilihan lain adalah tiba di pulau tanpa barang tambahan. |
| Barang di tangan saat waktu habis | Ikut kalau pemain berada di area aman. |
| Berangkat lebih awal | Diputuskan developer: boleh, dengan menahan E 1,5 detik saat tangan kosong. 60 detik di GDD bagian 3 adalah batas paling lama. |
| Jeda pra-game | 3 detik di luar 60 detik. Pemain hanya bisa menengok, sirene di detik ke-1,5, lalu kontrol dibuka. |
| Kapasitas tangan dan NPC | Kapasitas 4 berat. Diputuskan developer: semua NPC berberat 4, termasuk Koki, disembunyikan saat dibawa dan tampil sebagai ikon. NPC tertinggal menjadi korban. |
| Kedatangan di pantai | Pemain di pantai selatan Region 1 pada jam 06:00. Barang langsung di inventori, sisanya di samping bangkai sekoci, dan NPC di dekat pemain. |

### #75

| Topik | Usulan |
|---|---|
| Pistol dan Pisau di Fase 1 | Tidak bisa dipakai. Pemain hanya menghindar dan mendorong, mengikuti 60-seconds/README.md. |
| Dorongan | Klik kiri mendorong zombie 3 meter, terhuyung 1,5 detik, tanpa damage, dengan jeda 1,5 detik. |
| Jumlah dan kekuatan zombie kapal | Maksimal 8 sekaligus, total sekitar 10, kecepatan 3,5 m/s, gigitan damage 15 dan infeksi 8. |
| Infeksi dan HP terbawa ke pulau | Keduanya terbawa lewat ShipOutcomeData. |
| HP habis di kapal | Permadeath penuh, kembali ke ShipPhase lewat alur #82. |
| Air naik | Mulai dari dek bawah pada T=30, kepala terendam di dek bawah sekitar T=13, mencapai dek tengah pada T=5, dan dek atas kering sampai T=0. |
| Kemiringan kapal memengaruhi gerakan | Ya, kecil: kecepatan berubah 12 persen menanjak atau menurun, dan geser 0,4 m/s pada 15 detik terakhir. Ship sway hanya efek kamera. |
| Koridor runtuh | Dua titik (T=30 dan T=15), dengan tanda 3 detik dan tanpa damage langsung. Selalu ada satu rute ke sekoci, diperiksa ShipRouteAudit. |

### #76

| Topik | Usulan |
|---|---|
| Daftar NPC | Medis, Tukang, dan Koki. |
| Skill Tukang | Mempercepat crafting saat Work, dan perbaikan 1,25 kali saat di base. Tukang tidak memperbaiki sendiri. |
| Skill Koki | Masak 1,25 kali lebih cepat saat Work. |
| NPC sebagai target | Ya untuk zombie dan hewan, dengan bobot lebih rendah dari pemain. |
| Akibat NPC mati | Hilang permanen selama run, dan skillnya hilang. |
| Kebutuhan makan NPC | Tidak butuh makan. |
| Pemulihan HP NPC | Hanya di base, 0,5 HP per detik, tanpa memakai item. |
| Cara memberi perintah | Tombol E berganti antara ikuti, tunggu, dan kembali ke base, tanpa panel baru. |
| Perilaku NPC saat malam | NPC yang diperintah kembali ke base otomatis berlindung dan pulang kerja saat fajar. NPC Follow tetap ikut pemain. |

### #77

| Topik | Usulan |
|---|---|
| Jenis dan biaya stasiun | Hand, WoodBench (15 Mat_Wood), MedicalBench (15 Mat_Wood, 4 Mat_Stone), dan Campfire. |
| Model proses crafting | Antrean berurutan sampai 5 job. Bahan dipotong saat masuk antrean, dan kembali penuh saat dibatalkan. |
| Pengaruh NPC pada crafting | NPC tukang mempercepat 1,5 kali. GDD hanya menyebut mempercepat. |
| Pemain menjauh atau diserang saat crafting | Job meja berhenti di atas 6 meter, job tangan tetap jalan, dan semua job berhenti 1,5 detik saat kena serangan. Tidak ada yang batal otomatis. |
| Bahan, waktu, dan status terbuka resep inti | Sesuai tabel resep inti. Alat dasar dibuat dengan tangan, dan semua resep inti terbuka sejak awal. Pakaian hangat menunggu #65. |
| Tampilan resep terkunci | Redup dengan nama ??? kalau node prasyarat sudah terbuka, dan disembunyikan kalau belum. |

### #78

| Topik | Usulan |
|---|---|
| Carving atau bake ulang NavMesh | NavMeshObstacle dengan carving, bukan bake ulang per area. |
| Pintu terkunci | Dikunci lewat gerendel dengan E, menahan NPC, dan menerima 75 persen damage. Zombie tidak bisa membuka pintu yang tidak terkunci, tetapi tetap menyerangnya. |
| NPC lewat pintu tertutup | Boleh kalau tidak terkunci, dan pintu menutup sendiri setelah 1,5 detik. |
| Definisi pemain di dalam base | Minimal 3 dari 4 arah terhalang dinding atau pintu dalam 6 meter. Atap tidak wajib. |
| Jumlah penyerang per struktur | 4 per dinding dan 3 per pintu. Sisanya menunggu pada waitRingRadius. |
| Zombie di lantai atas | Zombie tidak bisa naik. Bangunan bertingkat aman dari gigitan langsung, tetapi bisa runtuh kalau penopangnya hancur. |

### #79

| Topik | Usulan |
|---|---|
| Bentuk menu bangun | Menu radial dua cincin, dibuka dengan klik kanan atau tombol B. |
| Mode palu | Empat mode: Perbaiki, Bangun, Upgrade, dan Bongkar. Mode awal Perbaiki, supaya klik kiri tidak membuang bahan tanpa sengaja. |
| Biaya, HP, dan daftar struktur | Ikuti tabel "Daftar struktur awal" di BuildMenu.md. Stairs_Wood adalah tambahan untuk bangunan bertingkat. |
| Batas bangunan | 3 tingkat, 2 sel menjorok, dan 400 struktur per run. |
| Pengembalian bahan saat bongkar | 50 persen dikali rasio HP, dibulatkan ke bawah, dan ditolak selama 5 detik setelah struktur diserang. |
| Area larangan bangun | Seam antar-region, lokasi jembatan, dan helipad memakai NoBuildVolume. |
| Teks GDD bagian 4 tentang pantai | Ganti dengan: "Pemain dilarang membangun di atas pasir pantai. Sistem menolak penempatan, dan Diary menjelaskan bahwa air pasang akan menghanyutkan bangunan di sana." Satu aturan lebih jelas daripada dua kemungkinan. |

### #80

| Topik | Usulan |
|---|---|
| Pemicu autosave | Tiba di pulau, pagi hari, bangun tidur, dan keluar game. Pergantian hari diartikan pagi pukul 06:00, karena pukul 24:00 berada di tengah serangan. |
| Aturan autosave saat pertempuran | Ditunda saat ada musuh aktif dalam 30 meter, 10 detik setelah diserang, saat bencana, diterkam, tenggelam, membeku, lapar atau haus nol, dan HP di bawah 10 persen. Permintaan dibuang setelah 120 detik. |
| Keluar game saat pertempuran | Tetap disimpan. Saat dilanjutkan pada malam hari, musuh muncul lagi di sekitar pemain, paling banyak 10, supaya keluar game tidak menjadi cara kabur. |
| Mematikan proses sebelum mati | Kembali ke autosave terakhir dan tidak dicegah lebih jauh, supaya pemain yang terkena crash tidak kehilangan run. Pilihan menghapus save setiap kali dibuka tidak disarankan. |
| Cadangan .bak | Dipakai hanya saat save rusak. Pemain bisa kehilangan progres satu autosave, tetapi tidak kehilangan seluruh run. |
| Save rusak total atau versi tidak cocok | Run tidak bisa dilanjutkan, dengan pesan jelas, dan file tidak dihapus sampai Run Baru dimulai. |

### #81

| Topik | Usulan |
|---|---|
| Hadiah api unggun dari Bab 1 | Beri satu Kit_Campfire, dan rec_campfire tetap terbuka sejak awal. |
| Bentuk Blueprint Jembatan | Penanda hasBridgeBlueprint di RegionGateSystem, bukan item, agar tidak bisa hilang dan tidak memakai slot. |
| Tujuan sebelum babnya ditemukan | Disembunyikan dari HUD, tetapi progresnya tetap dicatat, supaya Region 3 dan suku cadang tetap menjadi kejutan Bab 3. |
| Syarat selesai obj_base | 4 dinding dan 1 pintu (objective.baseWallCount 4). |

### #82

| Topik | Usulan |
|---|---|
| Pembagian manager persisten dan per scene | Layanan pada tabel urutan bootstrap, RunLifecycle, NarrativeSequencer, VFXManager, dan EventSystem persisten. Sistem dunia berada di ShipPhase atau IslandPhase. Region scene tidak boleh memuat manager. InventoryManager punya instance per scene. |
| Tombol layar kalah | Dua tombol: Ulangi dari Kapal dan Menu Utama. GDD hanya meminta kembali ke Fase 1, jadi Menu Utama adalah tambahan. |
| Alur setelah menang | Save dihapus, WinScreen tampil, lalu kembali ke MainMenu. Run yang sudah menang tidak bisa dilanjutkan. |
| Save di Fase 1 | Tidak ada save di kapal. Save pertama dibuat setelah tiba di pulau, dan menutup game saat di kapal membuang run. |
| Keluar saat bermain | Panel Pause memiliki Simpan dan Keluar ke Menu, mengikuti Save & Quit di SaveSystem.md. |
| Penutup gagal kabur di hari ke-60 | Fade hitam 3 detik dan teks, sampai #99 menyediakan adegan. |
| Nilai DeathReason saat menang | Tambahkan DeathReason.None sebagai nilai pertama enum, supaya RunEndedEvent saat menang tidak terbaca sebagai Starvation. |

### #83

| Topik | Usulan |
|---|---|
| Panel yang menjeda game | Pause, Settings, ConfirmDialog, RunSummary, Diary, LoreReader, GameOver, WinScreen, Loading, dan DevConsole menjeda. Inventori, crafting, peti, api unggun, menu bangun, peta, dan Lucky Machine tidak menjeda. |
| Gerakan saat panel tanpa jeda terbuka | Pemain berhenti bergerak, karena seluruh Action Map Gameplay dimatikan. |
| Audio saat dijeda | SFX dunia berhenti, musik turun 12 dB, dan suara tombol UI tetap bunyi. |
| Jeda otomatis saat jendela kehilangan fokus | Buka panel Pause kalau stack kosong dan pemain sedang bermain. |
| Serangan saat inventori terbuka | Panel tetap terbuka, kecuali pemain diterkam (PlayerPinnedEvent), yang menutup panel non-locked. |

### #85

| Topik | Usulan |
|---|---|
| Pencatatan usulan di issue #85 | Keempat file menulis "Usulan yang menunggu developer (dicatat di #85)", tetapi agen ini tidak memposting apa pun ke GitHub. Posting daftar usulan ini ke #85 supaya pernyataan di file benar. |

### #86

| Topik | Usulan |
|---|---|
| Sumber deteksi permukaan | Alphamap Terrain yang di-bake di Editor menjadi byte[], bukan volume trigger khusus pantai. |
| Campuran tekstur di garis pantai | Nilai dominan untuk suara dan gerak. Untuk bangunan, titik dilarang kalau bobot pasir mencapai 35 persen pada salah satu dari lima titik tapak. |
| Masa hidup manager SurfaceDetection | Persisten, karena dipakai Fase 1 dan Fase 2. |

### #88

| Topik | Usulan |
|---|---|
| Struktur scene pulau | Additive scene per region dengan IslandPhase sebagai scene dasar. Setelah disetujui, tanda usulan pada GLOSSARY bagian 3 dihapus. |
| Peta konsep | Rantai Region 1, Region 2, Region 4, Region 3 dengan tiga seam, tiap region 1000 x 1000 meter, dan jalur angkut 400 meter. Syarat akses tiap seam tetap milik #68. |
| Musuh saat region dilepas | Dikembalikan ke ObjectPooler tanpa menyimpan HP atau state, dan muncul lagi lewat SpawnManager. |
| NPC di region yang dilepas | NPC tinggal di IslandPhase. Objeknya dimatikan dengan status tercatat, dan nasibnya dihitung #90. |
| Bangunan di region yang dilepas | Tetap ada di akar IslandPhase tetapi nonaktif. Data dan BaseHealth tetap bisa dipakai #90. |
| Batas perangkat dan anggaran | RAM 8 GB, VRAM 4 GB, 1080p, dan 60 FPS. Batas 1.000 MB dan 300 batch untuk dasar, serta 1.200 MB dan 600 batch per region, dengan maksimal 2 region dimuat. |
| Pemain tertahan karena region belum siap | SeamBarrier menahan pemain, pesan muncul setelah 2 detik, dan panel Loading menjeda waktu setelah 10 detik. |

### #89

| Topik | Usulan |
|---|---|
| Seed dari pemain | Tidak diizinkan pada versi awal. Seed tetap ditampilkan untuk laporan bug, dan hanya DevConsole yang bisa memasukkannya. |
| Daftar yang berubah tiap run | Berubah: loot kapal, NPC kapal, lokasi suku cadang, jadwal bencana, cuaca, isi kargo, dan loot pulau. Tetap: peta, Diary, syarat menang, item awal, Blood Moon, jadwal kargo, dan node wajib. |
| Lokasi suku cadang | Acak dari titik buatan tangan di Region 4, minimal 10 titik untuk 5 suku cadang. Ini juga menjawab pilihan di #69. |
| NPC kapal tiap run | Tidak selalu sama. Jumlah dan daftarnya mengikuti #76. |
| Jaminan bekal kapal | Minimal 3 makanan, 2 peluru, dan 1 minuman di seluruh kapal. |
| Batas bencana agar run bisa dimenangkan | Bencana besar tidak sebelum hari ke-3, dan berjarak minimal 4 hari. |
| Jadwal bencana tanpa batas | Jadwal dibuat per blok 60 hari saat dibutuhkan lewat RunSeedManager.EnsureDisasterScheduleThrough dan Fork(Disaster, nomorBlok), lalu disimpan di RunLayout. Nilai baru: disaster.scheduleBlockDays 60. |

### #90

| Topik | Usulan |
|---|---|
| Aturan dasar base yang ditinggal | Diserang secara simulasi. Base aman membuat pertahanan sia-sia, dan serangan penuh terlalu berat untuk performa. |
| Rumus dan angka serangan | Seperti bagian 2 blueprint: kekuatan dari hari dan ukuran base, mitigasi dari dinding, jebakan, dan NPC, dengan serangan pertama hari ke-3. |
| Peluang NPC mati saat base ditinggal | Hanya bisa mati saat dinding jebol. Tanpa jebol, NPC paling parah hanya cedera. |
| Banyak base dan pos sementara | Boleh. Paling banyak 2 cluster diserang per malam, dan pos di bawah 4 struktur tidak diserang. |
| Bencana saat base ditinggal | Tetap kena. Besarnya dihitung #70, dan diterapkan lewat QueueDisasterDamage. |
| Notifikasi serangan tanpa alasan cerita (tanpa radio) | Pemain selalu diberi tahu, karena lebih adil untuk game permadeath. |
| Serangan simulasi pada base yang ditinggal setelah hari ke-60 | Kekuatan tetap naik 20 per hari sampai remoteBase.maxPower 2550 (setara hari ke-121), supaya base masih bisa bertahan pada run yang sangat panjang. |

### #91

| Topik | Usulan |
|---|---|
| Siklus pasang | Dua kali pasang per hari (00:00 dan 12:00), surut 06:00 dan 18:00, dengan puncak malam lebih tinggi. Pakai, karena GDD mengaitkan pantai dengan bahaya tidur. |
| Bahaya saat pasang | Stamina berkurang dan gerak melambat, tanpa dorongan dan tanpa item hanyut dari inventori. Pakai, karena dorongan melawan CharacterController dan terasa tidak adil pada game permadeath. |
| Nasib WorldItem dan kargo yang terkena air | Barang biasa hanyut, barang penting pindah ke TideSafePoint, dan kargo hilang saat puncak pasang malam (#71). |
| Efek pasang pada NPC dan musuh | Tanpa damage. Mereka hanya menghindari Area NavMesh Tidal lewat SetAreaCost. |
| Area khusus saat surut | Ada LowTideSpot kecil tanpa barang wajib, sebagai hadiah kecil. Memancing di tebing karang tetap bisa kapan saja. |
| Badai menaikkan air | Ya, tambahan level 0,25 saat cuaca Storm di Region 1. |

### #92

| Topik | Usulan |
|---|---|
| Urutan gabungan suasana | Region, jam, cuaca, event (bencana atau Blood Moon), lalu interior. Pakai. |
| Region aktif saat zona bertumpuk | Hitungan zona untuk keabsahan, dan zona terakhir untuk memilih, lewat ZoneTracker. Pakai. |
| Nilai fog dan warna tiap region | Pakai tabel di blueprint sebagai nilai awal, lalu setel lewat playtest. |
| Target luminansi senter di area gelap | Mulai dari minAmbientLuminance 0,035, lalu setel di Unity pada monitor rata-rata. |
| Penulis rotasi matahari | EnvironmentDirector menulis rotasi yang dihitung DayNightDirector, supaya Light hanya punya satu penulis. Pakai. |

### #93

| Topik | Usulan |
|---|---|
| Jalur bersama untuk kelompok zombie | Pakai titik tujuan bersama di atas 15 m, bukan satu NavMeshPath. Hidupkan hanya kalau uji stres belum memenuhi target. |
| Target performa dan spesifikasi minimum | PC menengah 1080p Medium. 30 zombie: rata-rata 60 FPS, minimum 45. 50 zombie: rata-rata 45 FPS, minimum 30. Developer perlu menetapkan spesifikasi minimum sebenarnya. |

### #94

| Topik | Usulan |
|---|---|
| Kemampuan traversal per jenis | Standar melompat dan turun sampai 4 m. Tribal memanjat sampai 3 m. Desa memanjat sampai 4 m dan turun sampai 8 m. Militer hanya turun sampai 2 m. Hewan diatur #61. |
| Respons saat pemain tak terjangkau | Bertahap: serang struktur kalau ada, mengepung pada 3 detik, melempar batu pada 10 detik dengan batas 3 pelempar, dan zombie lain mundur dari pandangan pada 20 detik. GDD tidak menyebut lempar batu. |
| Atap bangunan pemain bisa dinaiki zombie | Tidak bisa, sesuai BaseNavigation.md. Zombie menyerang dinding di bawahnya sampai atap runtuh. |
| Hukuman berkemah | Tier 1 pada 45 detik menggandakan pelempar batu. Tier 2 pada 90 detik memanggil 2 varian pembalas per region, berulang tiap 60 detik. Waktu 2x lebih cepat setelah menembak. Tidak berlaku di dalam base. |

### #95

| Topik | Usulan |
|---|---|
| Drop zombie | Ada, dengan peluang kecil per varian: standar 8 persen, Militer 40 persen. Batas 10 per malam, dan tanpa drop dari matahari atau tumbal. |
| Aksi Shove | Tombol V, tanpa damage, biaya 10 stamina, jeda 0,8 detik, dan area 120 derajat di depan pemain. |
| Efek kaki kena | Kecepatan 60 persen selama 2 detik, diperpanjang tanpa bertumpuk. |
| Aturan mayat | Diam 6 detik, memudar 1,5 detik, maksimal 12 mayat dan 4 ragdoll aktif. Mayat tertua memudar lebih dulu. |
| Musuh yang tidak bisa didorong | Zombie Militer, Tank, dan Beruang Kutub kebal dorongan dan Shove, tetapi tetap bisa terhuyung lewat poise tinggi. |

### #96

| Topik | Usulan |
|---|---|
| Sistem barang lempar dipakai atau tidak | Dipakai. throwable.enabled tetap false sampai disetujui |
| Daftar barang lempar | Molotov, Bom Rakitan, Batu, dan Suar |
| Garis perkiraan dan tombol | Ketuk G memilih jenis, tahan G membidik, lepas G melempar, dan klik kanan membatalkan |
| Friendly fire api Molotov | Mengikuti baris ledakan buatan pemain: pemain dan NPC dikali 0,5, bangunan dan musuh penuh |
| Reaksi per varian | Pengali Fire: Lumut 2, Militer 0,5, Beku 1,5. Hewan tidak terpancing. Lumut dan Zombie Desa tidak terpancing selama Dormant atau Submerged |
| Resep dan item baru Mat_Scrap | Pakai tabel resep di blueprint, dengan Mat_Scrap dari reruntuhan Region 3 dan 4 |

### #97

| Topik | Usulan |
|---|---|
| Cara pemain memadamkan api | Tahan E selama fire.extinguishHoldSeconds (1 detik) dan pakai 1 air, Drink_DirtyWater lebih dulu, lalu Drink_CleanWater. Air kotor mendapat kegunaan baru tanpa alat tambahan. |
| Apakah api unggun dan obor bisa menjalar | Tidak menjalar, supaya pemain tidak kehilangan base karena alat dasar yang diajarkan Diary Bab 1. Hanya jebakan api unggun (#26) yang bisa menyalakan musuh. |
| Apakah pemain dan NPC bisa ikut menyala | Tidak. Keduanya hanya menerima damage selama berada di dalam FireArea, sehingga tidak butuh status baru. |
| Efek area api ke NavMesh | Zombie menerobos Molotov seperti L4D. Hanya Fireball yang memotong NavMesh, dibatasi fire.maxCarvedAreas (4), untuk menghemat biaya carving. |
| Batas lompatan api antar-vegetasi | fire.maxVegetationGeneration = 3, supaya kayu untuk jembatan (500) tidak habis oleh satu percikan. |
| Semua angka GameBalanceConfig.fire | Pakai nilai awal di tabel blueprint, lalu setel lewat playtest. |

### #98

| Topik | Usulan |
|---|---|
| Apakah cahaya base menarik zombie | Ya, secara ringan. Hanya zombie Unaware yang terpengaruh, dengan light.attractionChancePerLight 0,05 per lampu dan light.attractionChanceMax 0,35. |
| Cara mengisi baterai senter | Baterai temuan (Item_Battery) tanpa generator. Generator menambah struktur, bahan bakar, dan suara baru pada issue berprioritas rendah, dan bisa ditambah kemudian. |
| Bahan bakar Obor Dinding dan Lampu | Obor Dinding memakai Mat_Wood, Lampu memakai Item_Battery, supaya baterai punya dua kegunaan yang saling bersaing. |
| Perilaku lampu saat badai | Lampu tak beratap padam saat Storm dan Blizzard. Lampu menyala kembali otomatis, sedangkan Obor Dinding harus dinyalakan lagi dengan E. |
| Laju kuras senter | flashlight.drainPerSec = 0,2 menggantikan drainRate = 1 yang habis dalam 100 detik, atau kurang dari seperempat malam. |
| Slot pemasangan Obor Dinding | Tambah slot baru WallMount, sehingga Obor Dinding menempel di sisi dinding yang sudah ada. |
| Semua angka GameBalanceConfig.light dan flashlight | Pakai nilai awal di tabel blueprint, lalu setel lewat playtest. |

### #99

| Topik | Usulan |
|---|---|
| Alat pemutar adegan | Rangkaian teks dan gambar untuk versi awal. Timeline bisa menyusul tanpa mengubah pemanggil. |
| Daftar adegan | Delapan adegan: intro_ship, lifeboat_to_beach, helipad_found, takeoff, tiga ending, dan fail_time_expired. |
| Ending berbeda | Tiga ending berdasarkan NPC: sendiri, dengan sebagian NPC, dan semua selamat. Tidak memengaruhi hadiah. |
| Aturan lewati | Hanya setelah pernah ditonton, sesuai checklist. |
| Penyimpanan adegan yang sudah ditonton | PlayerPrefs dengan kunci narrative.seen.<sceneId>. |
| Adegan helipad_found membekukan dunia | Ya, lewat SetGameplayTimeScale(0), supaya pemain tidak diserang saat kontrol terkunci. |

### #100

| Topik | Usulan |
|---|---|
| Sebaran temuan lore per region | 20 temuan: Volcano 4, Amazon 4, Snow 5, Logistic 7, sehingga Region 4 paling banyak sesuai GDD bagian 5. |
| Apakah temuan memberi petunjuk gameplay | Ya, terbatas: 2 log militer Region 4 masing-masing menandai 1 suku cadang (GameBalanceConfig.lore.maxPartHintsPerRun = 2). Temuan lain murni cerita. |
| Koleksi lore setelah permadeath | Teks tetap terbuka di file terpisah lore_archive.json seperti stats.json #57 dan bisa dibaca dari menu utama. Temuan di dunia dan petunjuk gameplay direset tiap run. |
| Arti ucapan 'lapar' karena NPCRoster.md menyatakan NPC tidak punya Hunger | Kategori Hungry mengomentari lapar pemain lewat VitalWarningFlags HungerLow atau Starving. Alternatifnya menambah kebutuhan makan NPC di #76. |
| Suara ucapan NPC | Versi awal hanya teks lewat subtitle. Klip suara bersifat opsional. |
| Nilai jeda ucapan NPC | Global 6 detik, per NPC 20 detik, per kategori 90 detik, dengan ZombieIncoming melewati jeda global dan per NPC. |

### #101

| Topik | Usulan |
|---|---|
| Vault dan memanjat tepian | Tidak ada di versi awal, karena menambah animasi, membuka celah masuk base, dan mengacaukan rute jurang. |
| Tangga dan tali | Naik dengan E, senjata tersimpan, dan stamina habis berarti lambat, bukan jatuh. |
| Damage jatuh | Aman sampai sekitar 4 meter, mematikan sekitar 20 meter. Salju dan lumpur mengurangi 25 persen, air meniadakan damage. |
| Lereng curam dan permukaan | Di atas 45 derajat pemain meluncur tanpa damage. Salju 0,8, lumpur 0,7, pasir 0,9, sisanya 1. |
| Tinggi lompat | Turunkan jumpHeight dari 2 ke 1,1 supaya pemain tidak bisa melompat ke atas dinding base. |
| Batas saat membawa Heavy Item | Tidak bisa jongkok atau memanjat, dan jalur Region 4 ke Region 3 harus bebas tangga. Salju membuat kecepatan 2 m/s, sehingga HP berkurang sekitar 55, bukan 40 seperti asumsi GameBalanceConfig. Perlu disetel lewat playtest. |

### #102

| Topik | Usulan |
|---|---|
| Aim down sights | Tidak ada di versi awal, mengikuti L4D. SetMainPose cukup untuk menambahkannya nanti. |
| Nilai awal kamera | FOV 70, FOV tangan 55, tambahan FOV lari 8, dan head bob 2,5 cm. Head bob, sway, dan FOV lari bisa dimatikan di #19. |
| Kamera di tangga dan saat membawa Heavy Item | Di tangga yaw dibatasi 70 derajat dan senjata turun. Saat membawa Heavy Item barang tampil di layar dengan sensitivitas 80 persen. |
| Pipeline render | URP dengan Camera Stack. Perlu dipastikan saat proyek Unity dibuat. |

### #103

| Topik | Usulan |
|---|---|
| Senjata dan peluru baru | Tambah Wpn_Machete, Wpn_SMG, Wpn_Rifle, dan Ammo_556, karena Pistol, Pisau, dan Kapak saja tidak memberi alasan menjelajah. |
| Armor pemain dan pengaruhnya pada infeksi | Tidak ada armor pemain di versi awal, dan infeksi tidak berubah. |
| Isi ulang titik loot selama 60 hari | Tidak terisi lagi. Sumber berulang sudah ada di #73 dan #71. |
| Skala loot terhadap hari dan jarak | Pengali per lokasi (Region 1 x1, Region 2 x1,1, Region 4 x1,25, Region 3 x1,4), tambahan jumlah tiap 10 hari, dan bobot item langka naik mengikuti hari. |
| Barang unik | Tiga titik dengan isi tetap: Chest_AuthorCamp, Warehouse_VillageHall, dan Chest_Armory, bukan diacak. |

### #104

| Topik | Usulan |
|---|---|
| Bahasa awal | Indonesia sebagai bahasa awal dan cadangan, dengan struktur siap untuk Inggris. Tabel Inggris diisi setelah teks stabil. |
| Alat lokalisasi | Tabel kunci sendiri yang diimpor dari CSV, bukan paket Unity Localization: hanya dua bahasa, panggilan sinkron cocok dengan GetPrompt(), dan tidak menambah Addressables. |
| Rentang pilihan aksesibilitas | textScale 0,85 sampai 1,5 dan cameraShakeScale 0 sampai 1. Head bob tetap memakai SettingsManager.headBobEnabled. |
| Peringatan kilatan cahaya | Tampil sekali saat peluncuran pertama lewat ConfirmDialog dengan pilihan Lanjut atau Kurangi kilatan, dan batas 3 kilatan per detik. |
| Subtitle peringatan bencana saat subtitle dimatikan | Peringatan bencana tetap tampil karena menyangkut keselamatan pemain. |
| Tempat data reduceFlashing | Di LocalizationManager.Accessibility, seperti yang sudah dirujuk DiseaseManager. Alternatifnya memindahkannya ke SettingsManager. |

### #106

| Topik | Usulan |
|---|---|
| Fungsi tarik barang dipakai atau tidak ('seperti di game Raft') | Dipakai dengan prioritas rendah, setelah #55 dan #71 selesai. Kalau ditolak, issue ditutup dan FishingSystem tidak perlu berubah. |
| Isi barang hanyut | Bobot Mat_Wood 55, Mat_Cloth 30, Drink_CleanWater 7, Food_Can 5, Ammo_9mm 3, supaya kargo mingguan tetap berharga. |
| Pengaruh cuaca dan air pasang | Pengali jeda muncul: Storm 0,5, Rain 0,75, pasang 0,75. |
| Cara menarik | Tahan klik kiri tanpa Tension Bar. |
| Save barang hanyut | Barang tidak disimpan. Hanya nextSpawnGameHour yang disimpan. |
| Laju kemunculan | 8 barang per hari game, paling banyak 4 aktif sekaligus. |

### #108

| Topik | Usulan |
|---|---|
| Urutan override target | Baru ditembak, lalu umpan, lalu Blood Moon, lalu skor biasa. |
| NPC sebagai target | Zombie dan hewan boleh mengincar NPC, dengan bobot lebih rendah dari pemain. Perlu selaras dengan #76. |
| Aggro penuh Claude.md terhadap Stealth #49 | Zombie yang kehilangan jejak tetap memburu pemain, tetapi tujuannya tidak tepat, dalam radius 8 meter. |
| Frekuensi pemilihan target | Penilaian tiap 1 detik untuk musuh dekat dan 2,5 detik untuk musuh jauh, target tetap minimal 3 detik, dan selisih skor minimal 15. |
| Batas pengincar NPC | 3 pengincar per NPC, ditambah pengurang skor 8 per pengincar lain supaya kelompok terbagi. |

### #109

| Topik | Usulan |
|---|---|
| Batas penyerang sekaligus | 3 zombie untuk pemain dan 2 untuk tiap NPC. Sisanya menunggu di lingkaran 3,5 meter. |
| Waktu serangan zombie standar | Windup 0,5 detik, Active 0,15 detik, Recovery 1,2 detik setelah kena dan 0,8 detik setelah meleset. |
| Pembatalan serangan oleh stagger | Stagger membatalkan Windup dan Active pada zombie biasa. Musuh besar hanya dibatalkan ledakan. |
| Tanda arah serangan | Penanda HUD hanya untuk serangan dari luar pandangan, dibatasi 0,5 detik sekali. |

### #110

| Topik | Usulan |
|---|---|
| Status sistem | Kini aturan GDD bagian 3 (Diterkam). Untuk Jaguar dan Zombie Tribal, dan pinned.enabled = true |
| Cara lepas | Menekan Struggle berulang menjadi bawaan, menahan tombol menjadi pilihan aksesibilitas, dan Pisau menjadi cara cepat |
| Pengaruh stamina, HP, dan infeksi pada kesulitan lepas | Pengali gabungan stamina 0,5, HP 0,75, infeksi 0,75, dengan batas bawah 0,35. Terburuk sekitar 6,3 detik lewat menekan berulang |
| Peran NPC | Diputuskan developer: tidak ada bantuan NPC. Usulan lama: NPC Follow atau Attack dalam 8 meter menyerang penyerang dan menambah kemajuan 0,25 per detik. NPC StayAtBase, Work, dan TakeCover tidak menolong |
| Musuh lain saat pemain tertahan | Paling banyak 1 penyerang lain dengan damage 50 persen, dan batas tertahan 8 detik |
| Durabilitas Pisau saat dipakai untuk lepas | Berkurang 5 tiap tusukan. Pisau Broken tidak bisa dipakai sampai diperbaiki |
| Heavy Item saat diterkam | Barang langsung jatuh di dekat kaki pemain |
| Waktu kebal setelah lepas | 1,5 detik dari damage musuh, dan 4 detik dari penahanan baru |

### #111

| Topik | Usulan |
|---|---|
| Kerusakan bertumpuk atau tidak | Tidak bertumpuk. Hanya kondisi tertinggi yang berlaku, mengikuti pola Raft. |
| Urutan prioritas | Tenggelam, frostbite, kelaparan, dehidrasi, infeksi, lalu pemulihan. |
| Pemulihan HP alami | 0,25 per detik, dengan syarat lapar dan haus minimal 50, infeksi di bawah 50, dan 12 detik tanpa diserang. |
| Damage infeksi | 0,3 per detik mulai Infection 75, selain mati di 100. Disetel bersama #64. |
| Biaya stamina dan kelelahan | Lari 10 per detik, lompat 8, melee 15, berenang 6 per detik, memanjat 5 per detik. Setelah habis, lari dan lompat terkunci sampai stamina 25. Melee tetap bisa dengan damage 70 persen dan jeda 1,5x. |
| Penalti lapar dan haus rendah | Di bawah 25, batas stamina menjadi 75 persen (lapar) atau regenerasi stamina setengah (haus). |
| Status awal di pulau | Hunger, Thirst, dan Stamina penuh. HP dan Infection dibawa dari kapal. |

### #112

| Topik | Usulan |
|---|---|
| Status sistem dan zombie di dalam ruangan saat siang | Boleh, karena tidak berkeliaran dan tidak terkena sinar. lair.enabled tetap false sampai disetujui |
| Lokasi sarang | Gua Region 1 dan 2, bunker Region 3, serta gudang dan rumah runtuh Region 4 |
| Zombie standar di interior Region 4, padahal GDD hanya menyebut Zombie Desa sebagai ancaman malam region itu | Pakai zombie standar dalam jumlah kecil, karena Zombie Desa harus bersembunyi di air atau lumpur |
| Menghancurkan sarang mengurangi horde | 10 persen per sarang, paling banyak 30 persen per region |
| Sarang menyumbang horde malam | Separuh isi sarang keluar saat malam, lalu sarang terisi lagi saat fajar kecuali isCleared |
| Batas pengejaran saat siang | Zombie berhenti di ambang pintu lewat NavMesh Area Interior, menunggu 6 detik, lalu kembali |
| Senter membangunkan zombie Dormant | Bangun setelah disorot 2 detik dalam 8 meter |

### #113

| Topik | Usulan |
|---|---|
| Pemakaian sistem perbaikan dan upgrade | Dipakai. GDD tidak menyebutnya, tetapi alat dan senjata perlu bertahan selama 60 hari. |
| Bahan dan stasiun perbaikan | Sesuai tabel biaya, dengan biaya mengikuti tingkat kerusakan. Perbaikan selalu penuh terhadap maksimum efektif. |
| Batas durabilitas maksimum saat diperbaiki | Turun 10 persen per perbaikan sampai 6 kali untuk item non-inti, sehingga Fire Axe tetap bernilai. |
| Alat rusak dan hancur | Durabilitas 0 berarti rusak dan bisa diperbaiki. Item hancur hanya kalau sudah melewati batas perbaikan. |
| Item inti, selaras dengan #58 | Pistol, Kail, dan Pisau bisa rusak tetapi tidak pernah hancur, maksimumnya tidak turun, dan perbaikannya murah di Hand. |
| Upgrade kecil | 4 jenis, sekali per jenis, maksimum 2 per item. Damage dan kecepatan +10 persen, magazine +2, dan durabilitas +25 persen. |
| Urutan gabung modifier | Nilai dasar, lalu upgrade, lalu perkalian Lucky Machine, dibatasi 4 kali damage dasar. Armor dan headshot dihitung sesudahnya. |

### #114

| Topik | Usulan |
|---|---|
| Bentuk lapisan musik. | Empat stem selaras dengan tempo dan panjang sama, bukan empat lagu terpisah. Aset musik harus mengikuti syarat ini. |
| Rumus sumber intensitas. | Nilai terbesar dari musuh dekat, damage, AIDirector.Intensity, dan tahap Director, supaya siang hari juga punya musik pertempuran dari hewan. |
| Batas suara musuh serentak. | 8 suara total, 3 per varian, ditambah satu suara kerumunan 3D untuk horde besar. |
| Pemicu suara peringatan Blood Moon. | HordeManager mengirim BloodMoonWarningEvent baru, supaya aturan hari Blood Moon tidak dihitung dua kali. |
| Tanda suara Zombie Lumut. | Tetap hanya napas dalam 6 meter saat Dormant sesuai #25, lalu jeritan saat marah. |
| Musik Fase 1. | Tetap milik ShipTimerManager. CombatMusicDirector hanya aktif saat IslandSurvival. |

### #115

| Topik | Usulan |
|---|---|
| Cuaca per region atau hanya region pemain | Semua region selalu punya cuaca dari fungsi murni, tetapi hanya region aktif yang ditampilkan. Pakai, karena penampung hujan dan tanaman butuh cuaca di tempatnya. |
| Tabel peluang cuaca per region | Pakai tabel di blueprint sebagai nilai awal. |
| Batas cuaca berat | Cuaca berat tidak berurutan dan tidak muncul pada hari pertama. Pakai, karena game ini permadeath. |
| Efek Blizzard | Kecepatan x0,75, Warmth -2 per detik, dan jarak pandang sekitar 25 m. Teks blueprint masih menulis skala Warmth menunggu #65, padahal WarmthSystem.md sudah ada dengan blizzardCold 2. Lihat contractChanges untuk WarmthSystem. |
| Pengaruh cuaca pada deteksi musuh | Cuaca mengurangi jarak pandang dan pendengaran musuh, supaya badai bisa dimanfaatkan untuk lewat diam-diam. Pakai. |
| Efek cuaca di dalam ruangan | Efek pada pemain hilang di dalam InteriorZone. Bangunan pemain mengikuti #65. |

### #116

| Topik | Usulan |
|---|---|
| Jembatan bisa rusak atau tidak | Tidak bisa dirusak, mengikuti usulan RegionGateSystem.md. Alternatifnya punya HP dan diperbaiki lewat #47. |
| Pengaman Diary dan Blueprint Jembatan | Buku tidak bisa rusak, dan Blueprint berupa status, bukan item. |
| Batu kecil tumbuh lagi | Jadwal normal di #73, ditambah ForceRegrowStones sebagai pengaman. |
| Fungsi keluar dari posisi tersangkut | Tombol di Pause, hitung mundur 5 detik yang batal saat kena damage, cooldown 180 detik, dan barang berat dijatuhkan. |
| Waktu pemeriksaan kondisi run | Setiap pergantian hari dan sekali setelah masuk atau load ke pulau. |
| Peringatan waktu tidak cukup | Sekali per hari, dengan margin 1,5, dan tidak pernah memaksa game berakhir. |
| Peringatan time_budget | Diubah menjadi peringatan zombie akan menguat, sekali per hari, dan hanya sampai hari ke-60. Setelah itu time_budget selalu Ok dan tidak pernah mengakhiri run. |

### #118

| Topik | Usulan |
|---|---|
| Tingkatan Backpack | Dipakai, dengan tiga tingkat +5, +10, dan +15 slot. Syarat menang tidak bergantung pada tas, sehingga aman ditolak. Konversi barang kapal tetap berlaku. |
| Melepas tas saat slot terisi | Ditolak kalau slot tambahan masih terisi. Kalau tas lepas karena sistem lain, sisanya jatuh sebagai WorldItem. |
| Slot tas | Kotak khusus di panel Inventory, bukan di hotbar. |
| Beban memperlambat pemain | Tidak. Slot sudah menjadi batas, dan perlambatan hanya ada pada Heavy Item. |
| Kayu dan batu | Tumpukan 100 per slot, ditambah setoran bertahap di BridgeSite (#68). |
| Hasil barang kapal | Satu objek peluru menjadi 12, makanan kaleng dan air menjadi 2, kain atau tali menjadi 4. Barang sisa di pantai tidak hilang otomatis. |
