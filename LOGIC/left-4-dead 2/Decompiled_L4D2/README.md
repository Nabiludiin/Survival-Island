# Decompiled Left 4 Dead 2 (L4D2) Game Logic & Scripts

Folder ini berisi hasil **decompile & ekstraksi logika penting** dari game **Left 4 Dead 2** (left4dead2):

---

## 📁 Isi Folder left4dead2/

1. **scripts/vscripts/**:
   - Skrip **AI Director** (alur pacing kemunculan zombie, ambush, crescendo event, dan holdout wave).
   - Logika evakuasi helikopter (ase_helicopter.nuc).
   - Logika mutasi & skrip entitas AI.

2. **scripts/**:
   - Data statistik senjata (weapon_*.txt): damage, recoil, clip size, cycle time.
   - Pemicu suara zombie dan efek lingkungan (game_sounds_*.txt).
   - Aturan populasi zombie (population.txt).

3. **missions/ & modes/**:
   - Definisi kampanye dan aturan mode permainan (*Survival*, *Versus*, *Scavenge*, *Realism*).

4. **materials/correction/**:
   - File color correction grading (LUT .raw, termasuk garage_alleys.raw).

5. **File Konfigurasi Utama**:
   - gameinfo.txt & steam.inf: Informasi versi resmi dan metadata game.
   - cfg/: Konfigurasi engine dan controller bindings.
