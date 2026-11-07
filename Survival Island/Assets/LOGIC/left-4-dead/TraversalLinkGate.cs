using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pembatas pengguna satu NavMeshLink. Blueprint: left-4-dead/AITraversal.md (#94). Dipasang di objek NavMeshLink.
    /// Usulan disetujui developer (#85): musuh bergantian melewati link (linkUsersDefault), sisanya menunggu lalu
    /// mencari jalur lain.
    /// </summary>
    public class TraversalLinkGate : MonoBehaviour
    {
        [Tooltip("0 = GameBalanceConfig.traversal.linkUsersDefault.")]
        [SerializeField] private int maxUsers;

        private readonly int[] users = new int[8];
        private int count;

        private int Capacity
        {
            get
            {
                var b = GameManager.CurrentBalance;
                int cap = maxUsers > 0 ? maxUsers : (b != null ? b.traversal.linkUsersDefault : 1);
                return Mathf.Clamp(cap, 1, users.Length);
            }
        }

        public bool TryEnter(int enemyId)
        {
            for (int i = 0; i < count; i++) if (users[i] == enemyId) return true;
            if (count >= Capacity) return false;
            users[count++] = enemyId;
            return true;
        }

        public void Exit(int enemyId)
        {
            for (int i = 0; i < count; i++)
            {
                if (users[i] != enemyId) continue;
                users[i] = users[--count];
                return;
            }
        }

        // Link pada region yang dilepas: musuhnya sudah dikembalikan (#88), gate dikosongkan.
        private void OnDisable() => count = 0;
    }
}
