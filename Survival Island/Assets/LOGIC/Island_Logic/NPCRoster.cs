using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalIsland
{
    /// <summary>Satu baris NPC di pulau. Isi yang disimpan di world_npcs (NPCRoster.md, #76).</summary>
    [Serializable]
    public class NPCRecord
    {
        public string npcId;
        public bool isAlive = true;
        public float hp;
        public NPCState state = NPCState.Follow;
        public NPCCommand command = NPCCommand.Follow;
        public Vector3 position;
        public float yaw;

        [NonSerialized] public NPCCompanion companion;   // null kalau objeknya sedang dimatikan atau sudah dihapus
        [NonSerialized] public NPCData data;
    }

    /// <summary>
    /// Daftar NPC hidup di pulau, skill aktif, dan penyimpanan world_npcs. Blueprint: Island_Logic/NPCRoster.md (#76).
    /// GDD bagian 3: NPC punya skill khusus (mempercepat crafting, ahli medis). GDD bagian 4: base tempat NPC berlindung.
    /// Daftar NPC, angka skill, perintah, dan kebutuhan NPC adalah usulan disetujui developer.
    /// Keputusan developer: NPC tidak bisa terinfeksi (#117 tidak dipakai), dan NPC tidak menolong pemain yang diterkam.
    ///
    /// Manager scene IslandPhase (dibuat ulang tiap run, jadi permadeath otomatis mengosongkan members).
    ///
    /// Catatan implementasi (sistem pendukung, juga di blueprint):
    /// - GetSkillMultiplier(Crafter) adalah pengali crafting (butuh Work). Pengali perbaikan tukang (1,25 di base) memakai
    ///   GetBuilderRepairMultiplier(), karena satu fungsi tidak bisa mengembalikan dua angka berbeda untuk skill yang sama.
    /// - RegionStreaming (#88) belum ada: NPC base tidak dimatikan saat region dilepas (TODO di HandleRegionUnloading).
    /// </summary>
    public class NPCRoster : MonoBehaviour, ISaveable
    {
        public const string SaveIdConst = "world_npcs";

        public static NPCRoster Instance { get; private set; }

        [Tooltip("Semua NPCData. Pencarian lewat npcId.")]
        [SerializeField] private NPCData[] database = Array.Empty<NPCData>();

        private readonly List<NPCRecord> members = new List<NPCRecord>(4);
        private readonly List<string> aliveIds = new List<string>(4);
        private bool registeredForSave;
        private float lastCrafterMultiplier = -1f;

        public IReadOnlyList<NPCRecord> Members => members;
        public bool IsPlayerInsideBase { get; private set; }

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[NPCRoster] Instance ganda di scene IslandPhase.");
                enabled = false;
                return;
            }
            Instance = this;
        }

        private void OnEnable()
        {
            EventChannel<NightfallEvent>.Raised += HandleNightfall;
            EventChannel<DaybreakEvent>.Raised += HandleDaybreak;
            EventChannel<PlayerBaseStateChangedEvent>.Raised += HandlePlayerBaseState;
            EventChannel<StructureDestroyedEvent>.Raised += HandleStructureDestroyed;
            // TODO(RegionStreaming): EventChannel<RegionLoadStateChangedEvent>.Raised += HandleRegionUnloading (#88).
            if (EventChannel<PlayerBaseStateChangedEvent>.HasLast) IsPlayerInsideBase = EventChannel<PlayerBaseStateChangedEvent>.Last.isInside;
            if (WorldStateSave.Instance != null)
            {
                WorldStateSave.Instance.Register(this);
                registeredForSave = true;
            }
        }

        private void OnDisable()
        {
            EventChannel<NightfallEvent>.Raised -= HandleNightfall;
            EventChannel<DaybreakEvent>.Raised -= HandleDaybreak;
            EventChannel<PlayerBaseStateChangedEvent>.Raised -= HandlePlayerBaseState;
            EventChannel<StructureDestroyedEvent>.Raised -= HandleStructureDestroyed;
            if (registeredForSave && WorldStateSave.Instance != null) WorldStateSave.Instance.Unregister(this);
            registeredForSave = false;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Data

        public NPCData GetData(string npcId)
        {
            if (string.IsNullOrEmpty(npcId) || database == null) return null;
            for (int i = 0; i < database.Length; i++)
                if (database[i] != null && string.Equals(database[i].npcId, npcId, StringComparison.Ordinal)) return database[i];
            return null;
        }

        public NPCRecord FindRecord(string npcId)
        {
            for (int i = 0; i < members.Count; i++)
                if (string.Equals(members[i].npcId, npcId, StringComparison.Ordinal)) return members[i];
            return null;
        }

        /// <summary>Dipakai RescueEventManager.BuildEndingContext (#14). Membaca NPCRecord.isAlive. List dipakai ulang.</summary>
        public IReadOnlyList<string> GetAliveNpcIds()
        {
            aliveIds.Clear();
            for (int i = 0; i < members.Count; i++)
                if (members[i].isAlive) aliveIds.Add(members[i].npcId);
            return aliveIds;
        }

        // ---------------------------------------------------------------- Kedatangan

        /// <summary>
        /// NPC yang disetor ke sekoci muncul di pantai. Dipanggil IslandArrival / LifeboatExtraction (#74).
        /// NPC tidak memakai ObjectPooler: jumlahnya kecil dan dibuat sekali per run.
        /// </summary>
        public void SpawnRescued(IReadOnlyList<string> ids, Vector3 origin)
        {
            if (ids == null) return;
            var bal = Balance;
            float spacing = bal != null ? bal.ship.arrivalNpcSpacing : 2f;
            int index = 0;
            for (int i = 0; i < ids.Count; i++)
            {
                string id = ids[i];
                // 1.
                var data = GetData(id);
                if (data == null || data.prefab == null)
                {
                    Debug.LogWarning($"[NPCRoster] NPCData atau prefab untuk '{id}' tidak ada. Dilewati.");
                    continue;
                }
                if (FindRecord(id) != null) continue;   // Sudah ada di pulau
                // 4.
                var rec = new NPCRecord
                {
                    npcId = id, isAlive = true, hp = data.maxHP, state = NPCState.Follow, command = NPCCommand.Follow,
                    // 3. origin + indeks * ship.arrivalNpcSpacing
                    position = origin + Vector3.right * (index * spacing), yaw = 0f, data = data,
                };
                members.Add(rec);
                // 2.
                SpawnObject(rec);
                index++;
            }
            // 5.
            RefreshSkills();
        }

        private void SpawnObject(NPCRecord rec)
        {
            var go = Instantiate(rec.data.prefab, rec.position, Quaternion.Euler(0f, rec.yaw, 0f));
            // Objek NPC hidup di scene IslandPhase, jadi hilang bersama scene saat run berakhir.
            if (go.scene != gameObject.scene) SceneManager.MoveGameObjectToScene(go, gameObject.scene);
            var c = go.GetComponent<NPCCompanion>();
            if (c == null)
            {
                Debug.LogError($"[NPCRoster] Prefab NPC '{rec.npcId}' tidak punya NPCCompanion.");
                Destroy(go);
                return;
            }
            rec.companion = c;
            c.Initialize(rec.data, rec);
        }

        // ---------------------------------------------------------------- Skill

        /// <summary>Pengali skill kalau syarat terpenuhi, 1 kalau tidak. Membaca NPCRecord, jadi tetap benar saat objek mati.</summary>
        public float GetSkillMultiplier(NPCSkill skill)
        {
            var bal = Balance;
            if (bal == null || skill == NPCSkill.None) return 1f;
            for (int i = 0; i < members.Count; i++)
            {
                var r = members[i];
                if (!r.isAlive || r.data == null || r.data.skill != skill) continue;
                switch (skill)
                {
                    // GDD bagian 3: NPC ahli medis. Syarat: hidup. Dosis vaksin x medicVaccineMultiplier (#64).
                    case NPCSkill.Medic: return bal.infection.medicVaccineMultiplier;
                    // GDD bagian 3: NPC mempercepat crafting. Syarat: hidup dan Work (#77).
                    case NPCSkill.Crafter: if (r.state == NPCState.Work) return bal.crafting.npcCrafterSpeedMultiplier; break;
                    // Usulan disetujui 3: Koki masak 1,25 kali lebih cepat saat Work (#44).
                    case NPCSkill.Cook: if (r.state == NPCState.Work) return bal.npc.cookSpeedMultiplier; break;
                }
            }
            return 1f;
        }

        /// <summary>
        /// Usulan disetujui 2: Tukang hidup dan berada di base membuat perbaikan 1,25 kali (#47). Tukang tidak memperbaiki sendiri.
        /// </summary>
        public float GetBuilderRepairMultiplier()
        {
            var bal = Balance;
            if (bal == null) return 1f;
            for (int i = 0; i < members.Count; i++)
            {
                var r = members[i];
                if (r.isAlive && r.data != null && r.data.skill == NPCSkill.Crafter && NPCCompanion.IsBaseState(r.state))
                    return bal.npc.builderRepairMultiplier;
            }
            return 1f;
        }

        /// <summary>Dipanggil saat state NPC berubah, NPC mati, atau load selesai. Hanya tukang yang dikirim ke CraftingManager.</summary>
        public void RefreshSkills()
        {
            float crafter = GetSkillMultiplier(NPCSkill.Crafter);
            if (Mathf.Approximately(crafter, lastCrafterMultiplier)) return;
            lastCrafterMultiplier = crafter;
            if (CraftingManager.Instance != null) CraftingManager.Instance.SetNpcSpeedMultiplier(crafter);
        }

        // ---------------------------------------------------------------- Fungsi lewat data

        /// <summary>Damage jarak jauh dari RemoteBaseSimulation (#90). Bekerja juga saat objek NPC nonaktif.</summary>
        public void ApplyRemoteDamage(string npcId, float amount)
        {
            var r = FindRecord(npcId);
            if (r == null || !r.isAlive || amount <= 0f) return;
            r.hp -= amount;
            if (r.hp <= 0f) KillNpc(npcId, DeathReason.KilledByEnemy);
        }

        /// <summary>Kematian lewat data. NPCDiedEvent terkirim sekali, baik objek aktif maupun nonaktif.</summary>
        public void KillNpc(string npcId, DeathReason reason)
        {
            var r = FindRecord(npcId);
            // 1.
            if (r == null || !r.isAlive) return;
            // 2.
            if (r.companion != null && r.companion.isActiveAndEnabled)
            {
                r.companion.DieFromRoster(reason);
                return;
            }
            // 3.
            r.isAlive = false;
            r.hp = 0f;
            EventChannel<NPCDiedEvent>.Publish(new NPCDiedEvent(npcId, reason));
            RefreshSkills();
        }

        // ---------------------------------------------------------------- Event

        private void HandleNightfall(NightfallEvent e)
        {
            for (int i = 0; i < members.Count; i++) if (members[i].companion != null) members[i].companion.HandleNightfall();
        }

        private void HandleDaybreak(DaybreakEvent e)
        {
            for (int i = 0; i < members.Count; i++) if (members[i].companion != null) members[i].companion.HandleDaybreak();
        }

        private void HandlePlayerBaseState(PlayerBaseStateChangedEvent e) => IsPlayerInsideBase = e.isInside;

        private void HandleStructureDestroyed(StructureDestroyedEvent e)
        {
            // Objek struktur dimatikan pada frame yang sama atau sesudahnya. NPCCompanion juga memeriksa ulang di Think.
            for (int i = 0; i < members.Count; i++) if (members[i].companion != null) members[i].companion.HandleStructureDestroyed();
        }

        // TODO(RegionStreaming): HandleRegionUnloading (#88, RegionStreaming.md):
        //   - Follow: Warp ke titik NavMesh valid di belakang pemain.
        //   - StayAtBase/Work/TakeCover di region yang dilepas: companion.WriteRecord(), lalu gameObject.SetActive(false).
        //     Saat region kembali: SetActive(true); OnEnable memanggil Warp ke record.position.

        // ---------------------------------------------------------------- Save (#80)

        [Serializable]
        private class RosterSaveData
        {
            public NPCRecord[] records = Array.Empty<NPCRecord>();
        }

        public string SaveId => SaveIdConst;
        public int LoadOrder => 120;

        public string CaptureState()
        {
            for (int i = 0; i < members.Count; i++)
                if (members[i].companion != null && members[i].isAlive) members[i].companion.WriteRecord();
            return JsonUtility.ToJson(new RosterSaveData { records = members.ToArray() });
        }

        public void RestoreState(string json)
        {
            // Hanya mengisi data. Objek dibuat di OnRestoreFinished. Target dan jalur tidak disimpan.
            for (int i = 0; i < members.Count; i++)
                if (members[i].companion != null) Destroy(members[i].companion.gameObject);
            members.Clear();
            lastCrafterMultiplier = -1f;
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<RosterSaveData>(json);
            if (d?.records == null) return;
            for (int i = 0; i < d.records.Length; i++)
            {
                var r = d.records[i];
                if (r == null || string.IsNullOrEmpty(r.npcId)) continue;
                r.data = GetData(r.npcId);
                if (r.data == null)
                {
                    Debug.LogWarning($"[NPCRoster] NPCData '{r.npcId}' dari save tidak ada. Dilewati.");
                    continue;
                }
                members.Add(r);
            }
        }

        public void OnRestoreFinished()
        {
            // NPC mati tidak dibuat ulang.
            for (int i = 0; i < members.Count; i++)
                if (members[i].isAlive && members[i].companion == null) SpawnObject(members[i]);
            RefreshSkills();
        }
    }
}
