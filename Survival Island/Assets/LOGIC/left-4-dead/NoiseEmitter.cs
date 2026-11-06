using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Pengirim Noise Sphere. Blueprint: left-4-dead/AISensory.md bagian "Noise Sphere" (#49).
    /// GDD bagian 3: zombie mendeteksi pemain lewat suara, sehingga pemain bisa mengendap.
    /// Pada pemain (isPlayerMovement = true), suara gerak dikirim setiap noiseTickSeconds sesuai cara bergerak.
    /// </summary>
    public class NoiseEmitter : MonoBehaviour
    {
        [SerializeField] private bool isPlayerMovement;

        /// <summary>Radius gerak terakhir, untuk DevConsole dan hewan (#61).</summary>
        public float CurrentRadius { get; private set; }

        /// <summary>NoiseEmitter pemain, kalau ada. Dipakai GunController dan MeleeCombatSystem.</summary>
        public static NoiseEmitter Player { get; private set; }

        private float tickTimer;

        private void OnEnable()
        {
            if (isPlayerMovement) Player = this;
        }

        private void OnDisable()
        {
            if (Player == this) Player = null;
        }

        public void Emit(float radius, NoiseSourceKind kind)
        {
            Emit(transform.position, radius, kind);
        }

        public static void Emit(Vector3 position, float radius, NoiseSourceKind kind)
        {
            if (radius <= 0f) return;
            EventChannel<NoiseEmittedEvent>.Publish(new NoiseEmittedEvent(position, radius, kind));
        }

        /// <summary>Suara mendarat (#101). impact01 = 1 memakai noiseLandingMax.</summary>
        public void EmitLanding(float impact01)
        {
            var bal = GameManager.CurrentBalance;
            if (bal == null) return;
            Emit(bal.stealth.noiseLandingMax * Mathf.Clamp01(impact01), NoiseSourceKind.Landing);
        }

        private void Update()
        {
            if (!isPlayerMovement || Time.deltaTime <= 0f) return;
            var bal = GameManager.CurrentBalance;
            if (bal == null) return;
            tickTimer -= Time.deltaTime;
            if (tickTimer > 0f) return;
            tickTimer = bal.stealth.noiseTickSeconds;

            var s = bal.stealth;
            var pc = IslandPlayerController.Instance;
            var trav = pc != null ? pc.GetComponent<PlayerTraversal>() : null;
            float speed = 0f;
            if (pc != null) { var v = pc.Velocity; v.y = 0f; speed = v.magnitude; }
            bool moving = speed > (bal.gun.movingSpeedThreshold);

            float radius;
            if (trav != null && trav.Mode == TraversalMode.Swimming) radius = s.noiseSwim;
            else if (!moving) radius = s.noiseIdle;
            else if (trav != null && trav.IsCrouching) radius = s.noiseCrouchMove;
            else if (pc != null && pc.IsSprinting) radius = s.noiseSprint;
            else radius = s.noiseWalk;
            CurrentRadius = radius;

            // AISensory.md: saat diam, event tidak dikirim supaya tidak ada beban tanpa guna.
            if (!moving) return;
            Emit(radius, NoiseSourceKind.Movement);
            // TODO #67: berhenti saat tidur lewat SleepStateChangedEvent setelah SleepSystem ditulis.
        }
    }
}
