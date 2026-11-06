using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pengganti Physics.Raycast dan kawan-kawan. Semua query memakai LayerMask dan QueryTriggerInteraction.Ignore,
    /// supaya trigger raksasa ZoneManager tidak menghentikan raycast.
    /// Blueprint: Core_Logic/PhysicsLayers.md bagian 2 "Aturan query fisika" (#87). Claude.md Tahap 6 poin 7.
    ///
    /// Belum ditulis di Langkah 1: CollectUniqueDamageables, karena butuh IDamageable milik DamagePipeline (#59, Langkah 5).
    /// </summary>
    public static class PhysicsQuery
    {
        /// <summary>Raycast dengan mask, selalu mengabaikan trigger.</summary>
        public static bool Raycast(Ray ray, out RaycastHit hit, float distance, int mask)
        {
            return Physics.Raycast(ray, out hit, distance, mask, QueryTriggerInteraction.Ignore);
        }

        /// <summary>SphereCast dengan mask, selalu mengabaikan trigger (#17).</summary>
        public static bool SphereCast(Ray ray, float radius, out RaycastHit hit, float distance, int mask)
        {
            return Physics.SphereCast(ray, radius, out hit, distance, mask, QueryTriggerInteraction.Ignore);
        }

        /// <summary>Cek kapsul, selalu mengabaikan trigger. Dipakai PlayerTraversal.CanStandUp (#101).</summary>
        public static bool CheckCapsule(Vector3 bottom, Vector3 top, float radius, int mask)
        {
            return Physics.CheckCapsule(bottom, top, radius, mask, QueryTriggerInteraction.Ignore);
        }

        /// <summary>Overlap bola tanpa garbage, memakai buffer milik pemanggil.</summary>
        public static int OverlapSphere(Vector3 position, float radius, int mask, Collider[] buffer)
        {
            return Physics.OverlapSphereNonAlloc(position, radius, buffer, mask, QueryTriggerInteraction.Ignore);
        }

        /// <summary>Overlap kotak tanpa garbage. Dipakai validasi BuildManager (#79).</summary>
        public static int OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion rotation, int mask, Collider[] buffer)
        {
            return Physics.OverlapBoxNonAlloc(center, halfExtents, buffer, rotation, mask, QueryTriggerInteraction.Ignore);
        }

        // ---------------------------------------------------------------- Pengecualian tertulis untuk air
        // PhysicsLayers.md: tiga fungsi ini sengaja memakai QueryTriggerInteraction.Collide,
        // karena collider Water adalah trigger.

        /// <summary>Raycast yang ikut mengenai trigger. Khusus query yang memang mencari collider Water (#55).</summary>
        public static bool RaycastIncludingTriggers(Ray ray, out RaycastHit hit, float distance, int mask)
        {
            return Physics.Raycast(ray, out hit, distance, mask, QueryTriggerInteraction.Collide);
        }

        /// <summary>Raycast ke permukaan air dengan mask Water (#13).</summary>
        public static bool RaycastWater(Vector3 origin, Vector3 direction, float distance, out RaycastHit hit)
        {
            return Physics.Raycast(origin, direction, out hit, distance, PhysicsMasks.Water, QueryTriggerInteraction.Collide);
        }

        /// <summary>Overlap air dengan mask Water (#13), tanpa garbage.</summary>
        public static int OverlapWater(Vector3 position, float radius, Collider[] buffer)
        {
            return Physics.OverlapSphereNonAlloc(position, radius, buffer, PhysicsMasks.Water, QueryTriggerInteraction.Collide);
        }
    }
}
