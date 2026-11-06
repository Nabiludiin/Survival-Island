using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Jenis target musuh (GLOSSARY.md, ThreatSystem.md #108).</summary>
    public enum TargetKind { Player, NPC, BaseAnchor, Lure }

    /// <summary>
    /// Kontrak target musuh. Blueprint: left-4-dead/ThreatSystem.md bagian "Pola" (#108).
    /// Dinding dan pintu bukan ITargetable; saat jalur terhalang, EnemyAI memakai sasaran bangunan sementara (#78).
    /// </summary>
    public interface ITargetable
    {
        int TargetId { get; }
        TargetKind Kind { get; }
        Vector3 Position { get; }
        Vector3 AimPoint { get; }       // Titik dada, untuk garis pandang dan tembakan NPC
        float ThreatWeight { get; }     // 0 sampai 1
        bool IsTargetable { get; }      // false saat mati, nonaktif, atau disembunyikan sistem
        IDamageable Damageable { get; } // null untuk BaseAnchor dan Lure
    }
}
