using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Kumpulan LayerMask bersama, disimpan sekali saat game mulai.
    /// Blueprint: Core_Logic/PhysicsLayers.md (#87). Usulan teknis, belum ada di GDD.
    ///
    /// Nama layer harus sudah dibuat di Project Settings > Tags and Layers, slot 8 sampai 23
    /// (tabel "Daftar layer resmi"). Layer yang belum ada dicatat sebagai kesalahan saat Initialize.
    /// </summary>
    public static class PhysicsMasks
    {
        // ---------------------------------------------------------------- Nama layer resmi (slot 8-23)
        public const string PlayerLayer = "Player";
        public const string PlayerViewmodelLayer = "PlayerViewmodel";
        public const string EnemyLayer = "Enemy";
        public const string EnemyHitboxLayer = "EnemyHitbox";
        public const string NPCLayer = "NPC";
        public const string InteractableLayer = "Interactable";
        public const string WorldItemLayer = "WorldItem";
        public const string TerrainLayer = "Terrain";
        public const string StructureLayer = "Structure";
        public const string StructureGhostLayer = "StructureGhost";
        public const string ObstacleLayer = "Obstacle";
        public const string WaterLayer = "Water";
        public const string ZoneTriggerLayer = "ZoneTrigger";
        public const string ProjectileLayer = "Projectile";
        public const string RagdollLayer = "Ragdoll";
        public const string HeavyItemLayer = "HeavyItem";

        private static readonly string[] AllLayerNames =
        {
            PlayerLayer, PlayerViewmodelLayer, EnemyLayer, EnemyHitboxLayer, NPCLayer, InteractableLayer,
            WorldItemLayer, TerrainLayer, StructureLayer, StructureGhostLayer, ObstacleLayer, WaterLayer,
            ZoneTriggerLayer, ProjectileLayer, RagdollLayer, HeavyItemLayer
        };

        // ---------------------------------------------------------------- Mask bersama (tabel "Kumpulan LayerMask bersama")
        public static int Shoot { get; private set; }
        public static int Melee { get; private set; }
        public static int Interact { get; private set; }
        public static int InteractBlock { get; private set; }
        public static int BuildSurface { get; private set; }
        public static int BuildOverlap { get; private set; }
        public static int AIVision { get; private set; }
        public static int AIAttack { get; private set; }
        public static int Explosion { get; private set; }
        public static int Ground { get; private set; }
        public static int Sunlight { get; private set; }
        public static int EnemyBody { get; private set; }
        public static int CastLanding { get; private set; }
        public static int RegrowBlock { get; private set; }
        public static int Water { get; private set; }
        public static int FireReach { get; private set; }
        public static int Occupant { get; private set; }

        public static bool IsInitialized { get; private set; }

        /// <summary>Isi semua mask. Dipanggil otomatis sebelum scene pertama dimuat.</summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            for (int i = 0; i < AllLayerNames.Length; i++)
            {
                if (LayerMask.NameToLayer(AllLayerNames[i]) < 0)
                    Debug.LogError($"[PhysicsMasks] Layer '{AllLayerNames[i]}' belum dibuat di Tags and Layers (PhysicsLayers.md).");
            }

            Shoot = LayerMask.GetMask(EnemyHitboxLayer, TerrainLayer, StructureLayer, ObstacleLayer, NPCLayer, InteractableLayer, HeavyItemLayer);
            Melee = LayerMask.GetMask(EnemyHitboxLayer, ObstacleLayer, StructureLayer, NPCLayer);
            Interact = LayerMask.GetMask(InteractableLayer, WorldItemLayer, HeavyItemLayer, NPCLayer);
            InteractBlock = LayerMask.GetMask(TerrainLayer, StructureLayer, ObstacleLayer);
            BuildSurface = LayerMask.GetMask(TerrainLayer, StructureLayer);
            BuildOverlap = LayerMask.GetMask(ObstacleLayer, StructureLayer, PlayerLayer, EnemyLayer, NPCLayer, HeavyItemLayer);
            AIVision = LayerMask.GetMask(TerrainLayer, StructureLayer, ObstacleLayer);
            AIAttack = LayerMask.GetMask(PlayerLayer, NPCLayer, StructureLayer);
            Explosion = LayerMask.GetMask(PlayerLayer, EnemyLayer, NPCLayer, StructureLayer);
            Ground = LayerMask.GetMask(TerrainLayer, StructureLayer, ObstacleLayer);
            Sunlight = LayerMask.GetMask(TerrainLayer, StructureLayer, ObstacleLayer);
            EnemyBody = LayerMask.GetMask(EnemyLayer);
            CastLanding = LayerMask.GetMask(WaterLayer, TerrainLayer, StructureLayer, ObstacleLayer);
            RegrowBlock = LayerMask.GetMask(StructureLayer, PlayerLayer, NPCLayer, HeavyItemLayer);
            Water = LayerMask.GetMask(WaterLayer);
            FireReach = LayerMask.GetMask(StructureLayer, ObstacleLayer, EnemyLayer, PlayerLayer, NPCLayer);
            Occupant = LayerMask.GetMask(PlayerLayer, NPCLayer, EnemyLayer);

            IsInitialized = true;
        }
    }
}
