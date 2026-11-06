using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Lapisan slot dalam satu titik grid.</summary>
    public enum GridLayer : byte
    {
        Floor = 0,      // Lantai dan atap (Cell dengan GroundOrWall atau WallBelow)
        Object = 1,     // Perabot, tangga, jebakan, api unggun (Cell dengan FloorOrGround)
        Edge = 2,       // Dinding dan pintu
        MountA = 3,     // WallMount di sisi positif dinding
        MountB = 4,     // WallMount di sisi negatif dinding
    }

    /// <summary>
    /// Kunci slot grid. Koordinat digandakan, sehingga tengah sel = (2cx, 2cz) dan tengah sisi = (2cx, 2cz±1) atau (2cx±1, 2cz).
    /// Dengan begitu sisi utara sel A dan sisi selatan sel tetangganya adalah kunci yang sama (BuildMenu.md "Rotasi dan snap").
    /// </summary>
    [Serializable]
    public struct GridKey : IEquatable<GridKey>
    {
        public int ix, iz, level;
        public GridLayer layer;

        public GridKey(int ix, int iz, int level, GridLayer layer)
        {
            this.ix = ix; this.iz = iz; this.level = level; this.layer = layer;
        }

        public bool IsCell => (ix & 1) == 0 && (iz & 1) == 0;
        public GridKey WithLayer(GridLayer l) => new GridKey(ix, iz, level, l);
        public GridKey WithLevel(int lv) => new GridKey(ix, iz, lv, layer);
        public GridKey Offset(int dx, int dz) => new GridKey(ix + dx, iz + dz, level, layer);

        public bool Equals(GridKey o) => ix == o.ix && iz == o.iz && level == o.level && layer == o.layer;
        public override bool Equals(object obj) => obj is GridKey k && Equals(k);
        public override int GetHashCode()
        {
            unchecked { return ((ix * 73856093) ^ (iz * 19349663) ^ (level * 83492791)) * 31 + (int)layer; }
        }
        public override string ToString() => $"({ix},{iz},L{level},{layer})";
    }

    /// <summary>
    /// Posisi grid dan tingkat satu struktur. Dipasang di prefab struktur. Blueprint: raft/BuildMenu.md (#79).
    /// Tetangga tidak disimpan: dihitung dari grid StructureRegistry, jadi grafik langsung ada setelah load.
    /// </summary>
    [DisallowMultipleComponent]
    public class StructureNode : MonoBehaviour
    {
        public BuildableData Data { get; private set; }
        public GridKey Key { get; private set; }
        public int Level => Key.level;
        public float BaseY { get; private set; }

        /// <summary>Lantai, dinding, dan perabot tingkat 0 yang menapak tanah. Selalu tegak.</summary>
        public bool IsGrounded { get; private set; }

        /// <summary>Biaya terkecil menuju struktur IsGrounded. int.MaxValue = tanpa jalur.</summary>
        public int SupportDistance { get; internal set; } = int.MaxValue;

        public IStructureHealth Health { get; private set; }
        public string StructureId => Health != null ? Health.StructureId : string.Empty;

        internal void Setup(BuildableData data, GridKey key, float baseY, bool grounded)
        {
            Data = data;
            Key = key;
            BaseY = baseY;
            IsGrounded = grounded;
            SupportDistance = grounded ? 0 : int.MaxValue;
            Health = GetComponent<IStructureHealth>();
        }
    }
}
