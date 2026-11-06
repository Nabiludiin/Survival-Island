using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Perilaku saat pool habis.</summary>
    public enum PoolOverflow
    {
        Fixed,  // Pool habis: kembalikan null. Dipakai musuh, efek, dan UI
        Grow    // Pool habis: buat objek baru sampai maxSize, lalu kembalikan null
    }

    /// <summary>Definisi satu pool. Ukuran pool adalah kapasitas teknis di prefab PersistentRoot, bukan GameBalanceConfig.</summary>
    [Serializable]
    public class Pool
    {
        public string tag;                              // Kunci unik. Musuh memakai EnemyData.enemyId
        public GameObject prefab;
        public int size;                                // Jumlah objek yang dibuat saat Initialize (pre-warm)
        public PoolOverflow overflow = PoolOverflow.Fixed;
        public int maxSize;                             // Hanya untuk Grow. 0 berarti sama dengan size
    }

    /// <summary>
    /// Gudang objek pasif: zombie, peluru, efek, dan objek yang sering muncul tidak di-Instantiate berulang.
    /// Blueprint: Core_Logic/ObjectPooler.md (#82). Claude.md Tahap 6 poin 1 dan 5.
    ///
    /// Urutan bootstrap ke-12. Semua objek pool tinggal di bawah PersistentRoot (PoolStorage), jadi tidak ikut hilang
    /// saat scene berganti. Tidak mengirim atau mendengar event.
    /// </summary>
    public class ObjectPooler : MonoBehaviour, IBootstrapService
    {
        public static ObjectPooler Instance { get; private set; }

        [Tooltip("Daftar pool. Nilai awal ada di tabel ObjectPooler.md bagian 1.")]
        public List<Pool> pools = new List<Pool>();

        private readonly Dictionary<string, Stack<PooledObject>> poolDictionary = new Dictionary<string, Stack<PooledObject>>();
        private readonly Dictionary<string, List<PooledObject>> activeByTag = new Dictionary<string, List<PooledObject>>();
        private readonly Dictionary<string, Pool> poolByTag = new Dictionary<string, Pool>();
        private readonly Dictionary<string, int> createdCount = new Dictionary<string, int>();
        private readonly HashSet<string> warnedTags = new HashSet<string>();
        private readonly List<PooledObject> returnBuffer = new List<PooledObject>(256);

        private Transform poolStaging;
        private Transform poolStorage;
        private bool isQuitting;

        public bool IsInitialized { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("[ObjectPooler] Instance ganda. Periksa prefab PersistentRoot.");
                return;
            }
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        private void OnApplicationQuit()
        {
            // Panggilan ReturnToPool dari OnDisable/OnDestroy objek lain saat game ditutup diabaikan tanpa error.
            isQuitting = true;
        }

        // ---------------------------------------------------------------- Initialize

        public void Initialize()
        {
            if (IsInitialized) return;

            // 1. PoolStaging selalu nonaktif, PoolStorage aktif (anak-anaknya nonaktif).
            poolStaging = GetOrCreateChild("PoolStaging", false);
            poolStorage = GetOrCreateChild("PoolStorage", true);

            int totalObjects = 0;
            for (int i = 0; i < pools.Count; i++)
            {
                var p = pools[i];
                // 2. Pool yang gagal dilewati. Boot tetap berlanjut.
                if (!ValidatePool(p)) continue;

                // 3.
                poolByTag[p.tag] = p;
                poolDictionary[p.tag] = new Stack<PooledObject>(p.size);
                activeByTag[p.tag] = new List<PooledObject>(p.size);
                createdCount[p.tag] = 0;

                // 4. Pre-warm.
                for (int n = 0; n < p.size; n++) CreateObject(p);
                totalObjects += p.size;
            }

            IsInitialized = true;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[ObjectPooler] Siap: {poolByTag.Count} tag, {totalObjects} objek.");
#endif
        }

        private Transform GetOrCreateChild(string childName, bool active)
        {
            var child = transform.Find(childName);
            if (child == null)
            {
                var go = new GameObject(childName);
                child = go.transform;
                child.SetParent(transform, false);
            }
            child.gameObject.SetActive(active);
            return child;
        }

        private bool ValidatePool(Pool p)
        {
            if (p == null) return false;
            if (string.IsNullOrEmpty(p.tag))
            {
                Debug.LogError("[ObjectPooler] Pool dengan tag kosong dilewati.");
                return false;
            }
            if (poolByTag.ContainsKey(p.tag))
            {
                Debug.LogError($"[ObjectPooler] Tag '{p.tag}' dipakai lebih dari satu pool. Pool kedua dilewati.");
                return false;
            }
            if (p.prefab == null)
            {
                Debug.LogError($"[ObjectPooler] Pool '{p.tag}' tidak punya prefab.");
                return false;
            }
            if (p.size < 1)
            {
                Debug.LogError($"[ObjectPooler] Pool '{p.tag}' punya size < 1.");
                return false;
            }
            if (p.overflow == PoolOverflow.Grow)
            {
                if (p.maxSize == 0) p.maxSize = p.size;
                else if (p.maxSize < p.size)
                {
                    Debug.LogError($"[ObjectPooler] Pool '{p.tag}' (Grow) punya maxSize lebih kecil dari size.");
                    return false;
                }
            }
            if (p.prefab.GetComponentInChildren<IPoolable>(true) == null)
                Debug.LogWarning($"[ObjectPooler] Prefab pool '{p.tag}' tidak punya IPoolable, jadi tidak bisa mereset dirinya.");
            return true;
        }

        private PooledObject CreateObject(Pool p)
        {
            // 1. Induk nonaktif: Awake dan OnEnable prefab BELUM berjalan. Mencegah NavMeshAgent mencari NavMesh
            //    di scene Bootstrap dan mencegah langganan event saat boot.
            var go = Instantiate(p.prefab, poolStaging);

            // 2.
            go.SetActive(false);
            int number = createdCount.TryGetValue(p.tag, out int c) ? c : 0;
            go.name = p.tag + " (" + number.ToString("00") + ")";

            // 3.
            var pooled = go.AddComponent<PooledObject>();
            pooled.PoolTag = p.tag;
            pooled.IsInPool = true;
            pooled.activeIndex = -1;
            pooled.poolables = go.GetComponentsInChildren<IPoolable>(true);
            pooled.prefabScale = p.prefab.transform.localScale;

            // 4. Objek tetap nonaktif karena activeSelf = false.
            go.transform.SetParent(poolStorage, false);

            // 5.
            poolDictionary[p.tag].Push(pooled);
            createdCount[p.tag] = number + 1;
            return pooled;
        }

        // ---------------------------------------------------------------- Spawn

        /// <summary>
        /// Ambil objek dari pool. Mengembalikan null kalau pool habis (Fixed), tag tidak ada, atau OnSpawned
        /// mengembalikan objek ke pool. Pemanggil wajib menerima null tanpa error.
        /// </summary>
        public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
        {
            // 1-3.
            if (isQuitting) return null;
            if (!IsInitialized)
            {
                Debug.LogError("[ObjectPooler] SpawnFromPool dipanggil sebelum Initialize.");
                return null;
            }
            if (tag == null || !poolByTag.TryGetValue(tag, out var p))
            {
                Debug.LogWarning($"[ObjectPooler] Tag pool '{tag}' tidak terdaftar.");
                return null;
            }

            // 4-5.
            var pooled = TakeInactive(p);
            if (pooled == null) return null;

            // 6. Skala tidak diubah di sini; OnSpawned yang mengembalikannya.
            var go = pooled.gameObject;
            go.transform.SetPositionAndRotation(position, rotation);

            // 7.
            pooled.IsInPool = false;
            pooled.SpawnCount++;
            var active = activeByTag[p.tag];
            pooled.activeIndex = active.Count;
            active.Add(pooled);

            // 8.
            go.SetActive(true);

            // 9. Kesalahan satu komponen tidak menghentikan komponen lain.
            var poolables = pooled.poolables;
            for (int i = 0; i < poolables.Length; i++)
            {
                try { poolables[i].OnSpawned(); }
                catch (Exception ex) { Debug.LogException(ex, go); }
            }

            // 10. OnSpawned sendiri mengembalikan objek (mis. EnemyAI tanpa NavMesh): pemanggil menerima null.
            if (pooled.IsInPool) return null;

            // 11.
            return go;
        }

        /// <summary>SpawnFromPool lalu GetComponent. Kalau komponen tidak ada, objek dikembalikan dan hasilnya null.</summary>
        public T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
        {
            var go = SpawnFromPool(tag, position, rotation);
            if (go == null) return null;
            var component = go.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"[ObjectPooler] Objek pool '{tag}' tidak punya komponen {typeof(T).Name}.");
                ReturnToPool(go);
            }
            return component;
        }

        private PooledObject TakeInactive(Pool p)
        {
            var stack = poolDictionary[p.tag];
            while (true)
            {
                if (stack.Count == 0)
                {
                    if (p.overflow == PoolOverflow.Grow && createdCount[p.tag] < p.maxSize)
                    {
                        WarnOnce(p.tag, $"[ObjectPooler] Pool '{p.tag}' tumbuh melewati size {p.size}.", true);
                        CreateObject(p);
                    }
                    else
                    {
                        // Objek aktif tidak pernah diambil paksa. Log hanya di build pengembangan, karena
                        // SpawnManager dan VFXManager memang mengandalkan null sebagai batas.
                        WarnOnce(p.tag, $"[ObjectPooler] Pool '{p.tag}' habis.", false);
                        return null;
                    }
                }

                var pooled = stack.Pop();
                if (pooled != null) return pooled;

                // 5. Objek dihancurkan dari luar.
                createdCount[p.tag] = Mathf.Max(0, createdCount[p.tag] - 1);
                Debug.LogWarning($"[ObjectPooler] Objek pool '{p.tag}' dihancurkan dari luar.");
            }
        }

        private void WarnOnce(string tag, string message, bool alwaysLog)
        {
            if (warnedTags.Contains(tag)) return;
            warnedTags.Add(tag);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.LogWarning(message);
#else
            if (alwaysLog) Debug.LogWarning(message);
#endif
        }

        // ---------------------------------------------------------------- Return

        /// <summary>Kembalikan objek ke pool. Tag pemanggil hanya dipakai untuk pengecekan; yang dipakai PoolTag.</summary>
        public void ReturnToPool(string tag, GameObject obj)
        {
            if (isQuitting || obj == null) return;

            var pooled = obj.GetComponent<PooledObject>();
            if (pooled == null)
            {
                // Bukan objek pool. Tidak dihancurkan supaya kesalahan mudah dilihat di Hierarchy.
                Debug.LogError($"[ObjectPooler] '{obj.name}' bukan objek pool. Objek dimatikan.", obj);
                obj.SetActive(false);
                return;
            }

            if (pooled.IsInPool) return;   // Tidak boleh masuk dua kali

            if (tag != null && tag != pooled.PoolTag)
                Debug.LogWarning($"[ObjectPooler] ReturnToPool dengan tag '{tag}', padahal objek berasal dari '{pooled.PoolTag}'. Memakai '{pooled.PoolTag}'.", obj);

            ReturnInternal(pooled);
        }

        /// <summary>Kembalikan objek ke pool memakai PoolTag-nya. Disarankan untuk pemanggil baru.</summary>
        public void ReturnToPool(GameObject obj)
        {
            ReturnToPool(null, obj);
        }

        private void ReturnInternal(PooledObject pooled)
        {
            // 5. Tanda diisi lebih dulu, supaya panggilan ulang dari dalam OnReturned diabaikan.
            pooled.IsInPool = true;

            // 6.
            var go = pooled.gameObject;
            var poolables = pooled.poolables;
            for (int i = 0; i < poolables.Length; i++)
            {
                try { poolables[i].OnReturned(); }
                catch (Exception ex) { Debug.LogException(ex, go); }
            }

            // 7.
            go.SetActive(false);

            // 8. Swap-remove dari daftar aktif.
            RemoveFromActive(pooled);

            // 9.
            go.transform.SetParent(poolStorage, false);
            go.transform.localScale = pooled.prefabScale;

            // 10.
            if (poolDictionary.TryGetValue(pooled.PoolTag, out var stack)) stack.Push(pooled);
        }

        private void RemoveFromActive(PooledObject pooled)
        {
            if (!activeByTag.TryGetValue(pooled.PoolTag, out var active)) return;
            int index = pooled.activeIndex;
            if (index < 0 || index >= active.Count || active[index] != pooled)
            {
                index = active.IndexOf(pooled);
                if (index < 0) { pooled.activeIndex = -1; return; }
            }

            int last = active.Count - 1;
            if (index != last)
            {
                var moved = active[last];
                active[index] = moved;
                if (moved != null) moved.activeIndex = index;
            }
            active.RemoveAt(last);
            pooled.activeIndex = -1;
        }

        /// <summary>
        /// Kembalikan semua objek aktif. Dipanggil RunLifecycle sebelum scene diganti (reset langkah 4 dan
        /// EnterIslandPhase langkah 1).
        /// </summary>
        public void ReturnAllActive()
        {
            if (!IsInitialized) return;

            // 1. Salinan, karena OnReturned satu objek bisa mengembalikan objek lain.
            returnBuffer.Clear();
            foreach (var pair in activeByTag)
            {
                var active = pair.Value;
                for (int i = active.Count - 1; i >= 0; i--)
                {
                    var pooled = active[i];
                    if (pooled == null)
                    {
                        // Objek dihancurkan dari luar: hapus dari daftar aktif.
                        Debug.LogWarning($"[ObjectPooler] Objek pool '{pair.Key}' dihancurkan dari luar.");
                        createdCount[pair.Key] = Mathf.Max(0, createdCount[pair.Key] - 1);
                        int last = active.Count - 1;
                        if (i != last)
                        {
                            active[i] = active[last];
                            if (active[i] != null) active[i].activeIndex = i;
                        }
                        active.RemoveAt(last);
                        continue;
                    }
                    returnBuffer.Add(pooled);
                }
            }

            // 2.
            for (int i = 0; i < returnBuffer.Count; i++)
            {
                var pooled = returnBuffer[i];
                if (pooled != null && !pooled.IsInPool) ReturnInternal(pooled);
            }

            // 3.
            returnBuffer.Clear();
        }

        /// <summary>Dipanggil RunLifecycle langkah 10 reset, setelah ReturnAllActive dan pergantian scene.</summary>
        public void ResetForNewRun()
        {
            if (!IsInitialized) return;

            // 1.
            if (TotalActive > 0)
            {
                foreach (var pair in activeByTag)
                    if (pair.Value.Count > 0)
                        Debug.LogWarning($"[ObjectPooler] Masih ada {pair.Value.Count} objek aktif '{pair.Key}' saat reset.");
                ReturnAllActive();
            }

            foreach (var pair in poolByTag)
            {
                var p = pair.Value;
                var stack = poolDictionary[p.tag];

                // Buang rujukan null yang tersisa di stack (objek dihancurkan dari luar saat di pool).
                if (ContainsNull(stack)) RebuildWithoutNulls(p.tag, stack);

                // 2. Pool Grow dipangkas kembali ke size. Aman karena layar sedang Loading.
                while (createdCount[p.tag] > p.size && stack.Count > 0)
                {
                    var extra = stack.Pop();
                    createdCount[p.tag]--;
                    if (extra != null) Destroy(extra.gameObject);
                }

                // 3. Pool yang kekurangan objek dibuat ulang sampai size.
                while (createdCount[p.tag] < p.size) CreateObject(p);
            }

            // 4.
            warnedTags.Clear();
        }

        private static bool ContainsNull(Stack<PooledObject> stack)
        {
            foreach (var item in stack) if (item == null) return true;
            return false;
        }

        private void RebuildWithoutNulls(string tag, Stack<PooledObject> stack)
        {
            var survivors = new List<PooledObject>(stack.Count);
            int removed = 0;
            while (stack.Count > 0)
            {
                var item = stack.Pop();
                if (item != null) survivors.Add(item);
                else removed++;
            }
            for (int i = survivors.Count - 1; i >= 0; i--) stack.Push(survivors[i]);
            if (removed > 0)
            {
                createdCount[tag] = Mathf.Max(0, createdCount[tag] - removed);
                Debug.LogWarning($"[ObjectPooler] {removed} objek pool '{tag}' dihancurkan dari luar saat di pool.");
            }
        }

        // ---------------------------------------------------------------- Fungsi baca

        public bool HasPool(string tag) => tag != null && poolByTag.ContainsKey(tag);

        public int GetPoolSize(string tag) => tag != null && poolByTag.TryGetValue(tag, out var p) ? p.size : 0;

        public int CountInactive(string tag) => tag != null && poolDictionary.TryGetValue(tag, out var s) ? s.Count : 0;

        public int CountActive(string tag) => tag != null && activeByTag.TryGetValue(tag, out var a) ? a.Count : 0;

        public int TotalActive
        {
            get
            {
                int total = 0;
                foreach (var pair in activeByTag) total += pair.Value.Count;
                return total;
            }
        }

#if UNITY_EDITOR
        // ---------------------------------------------------------------- Pemeriksaan Editor

        private void OnValidate()
        {
            var seen = new HashSet<string>();
            for (int i = 0; i < pools.Count; i++)
            {
                var p = pools[i];
                if (p == null) continue;
                if (string.IsNullOrEmpty(p.tag)) Debug.LogWarning($"[ObjectPooler] Pool #{i} punya tag kosong.", this);
                else if (!seen.Add(p.tag)) Debug.LogWarning($"[ObjectPooler] Tag pool ganda: '{p.tag}'.", this);
                if (p.prefab == null) Debug.LogWarning($"[ObjectPooler] Pool '{p.tag}' tidak punya prefab.", this);
                else if (p.prefab.GetComponentInChildren<PooledObject>(true) != null)
                    Debug.LogWarning($"[ObjectPooler] Prefab '{p.prefab.name}' punya PooledObject manual. Hapus; komponen ini ditambahkan otomatis.", this);
                if (p.size < 1) Debug.LogWarning($"[ObjectPooler] Pool '{p.tag}' punya size < 1.", this);
            }
        }
#endif
    }
}
