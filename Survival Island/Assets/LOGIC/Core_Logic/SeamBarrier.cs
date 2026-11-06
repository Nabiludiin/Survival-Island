using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Dinding penahan di batas region (BoxCollider layer Obstacle) selama salah satu sisi seam belum Loaded.
    /// Blueprint: Core_Logic/RegionStreaming.md bagian "Pengaman kalau pemain tiba lebih cepat", langkah 2 (#88).
    /// Berbeda dengan GateBarrier (#68), yang menahan region yang belum dibuka.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public sealed class SeamBarrier : MonoBehaviour
    {
        [SerializeField] private BoxCollider box;

        public bool IsBlocking => box != null && box.enabled;
        public Bounds Bounds => box != null ? box.bounds : new Bounds(transform.position, Vector3.zero);

        private void Awake()
        {
            if (box == null) box = GetComponent<BoxCollider>();
            box.isTrigger = false;
        }

        public void SetBlocking(bool value)
        {
            if (box == null) box = GetComponent<BoxCollider>();
            if (box != null) box.enabled = value;
        }
    }
}
