using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Ship sway sebagai sumber offset kamera. Blueprint: 60-seconds/ShipHazardManager.md bagian "Kemiringan dan gerakan" (#75).
    /// Menggantikan rumus zSway lama di ShipPlayerController. Hanya kamera, tidak pernah menyentuh NavMesh.
    /// Memakai Time.time, jadi berhenti saat game dijeda.
    /// </summary>
    public class ShipSwaySource : MonoBehaviour, ICameraOffsetSource
    {
        private static readonly ShipHazardBalance FallbackHazard = new ShipHazardBalance();

        private float currentSway;
        private float targetSway;
        private bool registered;

        public int Stage { get; private set; }

        private static ShipHazardBalance Hazard
        {
            get
            {
                var b = GameManager.CurrentBalance;
                return b != null ? b.shipHazard : FallbackHazard;
            }
        }

        public Vector3 EulerOffset
        {
            get
            {
                float speed = Hazard.swaySpeed;
                return new Vector3(
                    Mathf.Sin(Time.time * speed * 0.7f) * currentSway * 0.2f,
                    0f,
                    Mathf.Sin(Time.time * speed) * currentSway);
            }
        }

        public Vector3 PositionOffset => Vector3.zero;

        /// <summary>Dipanggil ShipHazardManager saat entri SetSwayStage.</summary>
        public void SetStage(int stage)
        {
            var degrees = Hazard.swayDegreesByStage;
            Stage = stage;
            if (degrees == null || degrees.Length == 0) { targetSway = 0f; return; }
            targetSway = degrees[Mathf.Clamp(stage, 0, degrees.Length - 1)];
        }

        private void OnEnable() { TryRegister(); }
        private void Start() { TryRegister(); }

        private void OnDisable()
        {
            if (registered && FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.UnregisterOffsetSource(this);
            registered = false;
        }

        private void TryRegister()
        {
            if (registered || FirstPersonCamera.Instance == null) return;
            FirstPersonCamera.Instance.RegisterOffsetSource(this);
            registered = true;
        }

        private void Update()
        {
            if (!registered) TryRegister();
            // currentSway mengejar nilai tahap dengan MoveTowards, jadi tidak melompat.
            // Laju kejar memakai swaySpeed derajat per detik (usulan Langkah 2, blueprint tidak menyebut lajunya).
            currentSway = Mathf.MoveTowards(currentSway, targetSway, Hazard.swaySpeed * Time.deltaTime);
        }
    }
}
