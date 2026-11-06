using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Komponen pembantu yang ditambahkan otomatis oleh ObjectPooler ke setiap objek pool.
    /// JANGAN ditambahkan manual ke prefab. Blueprint: ObjectPooler.md bagian "PooledObject".
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class PooledObject : MonoBehaviour
    {
        /// <summary>Tag pool asal. Dipakai ReturnToPool.</summary>
        public string PoolTag { get; internal set; }

        /// <summary>Penanda cepat untuk menolak pengembalian ganda.</summary>
        public bool IsInPool { get; internal set; } = true;

        /// <summary>Bertambah setiap keluar dari pool. Pemilik boleh memakainya sebagai generasi.</summary>
        public int SpawnCount { get; internal set; }

        /// <summary>Indeks di daftar aktif untuk swap-remove. -1 saat di pool.</summary>
        internal int activeIndex = -1;

        /// <summary>Hasil GetComponentsInChildren&lt;IPoolable&gt;(true), diisi sekali saat dibuat.</summary>
        internal IPoolable[] poolables;

        /// <summary>Skala prefab, disimpan saat dibuat. Dikembalikan saat masuk pool.</summary>
        internal Vector3 prefabScale = Vector3.one;
    }
}
