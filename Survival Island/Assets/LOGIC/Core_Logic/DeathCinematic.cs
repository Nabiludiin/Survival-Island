using System;
using System.Collections;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Gaya adegan mati (DeathCinematic.md bagian 1).</summary>
    public enum DeathStyle { Ragdoll, Collapse, Sink, Zombify, Blackout }

    /// <summary>Gaya, teks, dan warna layar per penyebab kematian.</summary>
    [Serializable]
    public class DeathProfile
    {
        public DeathReason reason;
        public DeathStyle style;
        public string messageKey;     // Teks lewat LocalizationManager (#104)
        public Color screenTint = Color.clear;
        public AudioClip sting;
    }

    /// <summary>
    /// Transisi visual saat pemain mati: ragdoll, kamera lepas dari tubuh dan mengorbit, lalu fade ke hitam.
    /// Blueprint: Core_Logic/DeathCinematic.md (#53). GDD bagian 1 dan 3 (Permadeath): setelah adegan ini,
    /// RunLifecycle menghapus save dan menampilkan GameOver. Kelas ini tidak menyentuh file save.
    ///
    /// Komponen di prefab pemain Fase 1 dan Fase 2. Instance diisi di OnEnable dan dikosongkan di OnDisable;
    /// kalau null, RunLifecycle melewati langkah ini. Semua penantian memakai waktu nyata, karena panel Cinematic
    /// tidak menjeda dunia dan gerak lambat mengubah skala waktu gameplay.
    /// </summary>
    public class DeathCinematic : MonoBehaviour
    {
        public static DeathCinematic Instance { get; private set; }

        [SerializeField] private DeathProfile[] profiles = Array.Empty<DeathProfile>();

        [Tooltip("PlayerBodyRig: nonaktif selama hidup. Kosong berarti semua gaya menjadi Blackout.")]
        [SerializeField] private GameObject bodyRig;
        [SerializeField] private Animator bodyAnimator;
        [Tooltip("Tulang dada, titik pandang kamera.")]
        [SerializeField] private Transform focusBone;
        [Tooltip("CameraRig yang sama dengan FirstPersonCamera.cameraRig.")]
        [SerializeField] private Transform cameraRig;
        [Tooltip("Komponen audio langkah (#21) yang dimatikan saat mati. Boleh kosong.")]
        [SerializeField] private Behaviour[] disableOnDeath = Array.Empty<Behaviour>();

        private Rigidbody[] bones = Array.Empty<Rigidbody>();
        private Vector3 lastHitDir;
        private Vector3 lastHitPoint;
        private bool isPlaying;
        private bool skipRequested;

        public bool IsPlaying => isPlaying;

        private DeathCinematicBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.deathCinematic : null;

        private void Awake()
        {
            if (bodyRig != null) bones = bodyRig.GetComponentsInChildren<Rigidbody>(true);
        }

        private void OnEnable()
        {
            Instance = this;
            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
        }

        private void OnDisable()
        {
            if (Instance == this) Instance = null;
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
        }

        private void HandlePlayerDamaged(PlayerDamagedEvent e)
        {
            // Arah dan titik pukulan terakhir, untuk gaya Ragdoll.
            lastHitDir = e.info.hitDirection;
            lastHitPoint = e.info.hitPoint;
        }

        private DeathProfile GetProfile(DeathReason reason)
        {
            for (int i = 0; i < profiles.Length; i++)
                if (profiles[i] != null && profiles[i].reason == reason) return profiles[i];
            return new DeathProfile { reason = reason, style = DefaultStyle(reason) };
        }

        /// <summary>Tabel "Penyebab dan gaya" di DeathCinematic.md, dipakai kalau profil belum diisi di Inspector.</summary>
        private static DeathStyle DefaultStyle(DeathReason reason)
        {
            switch (reason)
            {
                case DeathReason.Starvation:
                case DeathReason.Dehydration:
                case DeathReason.Frostbite: return DeathStyle.Collapse;
                case DeathReason.Drowned: return DeathStyle.Sink;
                case DeathReason.Zombified: return DeathStyle.Zombify;
                case DeathReason.LeftOnShip: return DeathStyle.Blackout;
                default: return DeathStyle.Ragdoll;
            }
        }

        // ---------------------------------------------------------------- Pemutar

        /// <summary>Dipanggil RunLifecycle.RunEndSequence langkah 4. Batas waktunya dipegang RunLifecycle.</summary>
        public IEnumerator Play(DeathReason reason)
        {
            if (isPlaying) yield break;
            isPlaying = true;
            skipRequested = false;
            EventChannel<CinematicStateChangedEvent>.Publish(new CinematicStateChangedEvent(true, false));
            try
            {
                // 2. Satu frame, supaya semua pendengar PlayerDiedEvent selesai (Heavy Item dijatuhkan, pin dilepas, dst.).
                yield return null;

                var bal = Bal;
                var profile = GetProfile(reason);
                var style = profile.style;
                // PlayerBodyRig belum tersedia: gaya ragdoll menjadi Blackout. Kegagalan sinematik tidak menghentikan permadeath.
                if (bodyRig == null && (style == DeathStyle.Ragdoll || style == DeathStyle.Collapse || style == DeathStyle.Sink))
                    style = DeathStyle.Blackout;

                // 3. TODO #23/#114: tampilkan profile.screenTint lewat PostProcessController, mainkan profile.sting, turunkan musik.

                // 4.
                DisableController();

                // 5.
                float slowMoLeft = 0f;
                if (style == DeathStyle.Ragdoll || style == DeathStyle.Collapse || style == DeathStyle.Sink)
                {
                    EnableRagdoll(style, reason, bal);
                    // Gerak lambat hanya untuk kematian oleh pukulan (keputusan 3).
                    if (style == DeathStyle.Ragdoll && bal != null && UIFocusStack.Instance != null)
                    {
                        UIFocusStack.Instance.SetGameplayTimeScale(bal.slowMoScale);
                        slowMoLeft = bal.slowMoSeconds;
                    }
                }

                // 6. Kamera lepas dan mengorbit, kecuali Zombify dan Blackout.
                bool orbit = style != DeathStyle.Zombify && style != DeathStyle.Blackout && cameraRig != null && focusBone != null;
                if (orbit && FirstPersonCamera.Instance != null) FirstPersonCamera.Instance.DetachForDeath();

                // 7. Tunggu holdSeconds (atau collapseHoldSeconds / zombifyCinematicSeconds) dengan waktu nyata.
                float hold = bal == null ? 0f
                    : style == DeathStyle.Collapse ? bal.collapseHoldSeconds
                    : style == DeathStyle.Zombify && GameManager.CurrentBalance != null ? GameManager.CurrentBalance.infection.zombifyCinematicSeconds
                    : style == DeathStyle.Blackout ? 0f
                    : bal.holdSeconds;
                float skipAfter = bal != null ? bal.skipAfterSeconds : 2f;
                float elapsed = 0f;
                var orbitState = orbit ? BeginOrbit() : default;
                while (elapsed < hold)
                {
                    float dt = Time.unscaledDeltaTime;
                    elapsed += dt;
                    if (slowMoLeft > 0f)
                    {
                        slowMoLeft -= dt;
                        if (slowMoLeft <= 0f && UIFocusStack.Instance != null) UIFocusStack.Instance.SetGameplayTimeScale(1f);
                    }
                    if (orbit) TickOrbit(ref orbitState, bal, dt);
                    // Usulan 2: Skip boleh setelah skipAfterSeconds.
                    if (elapsed >= skipAfter && InputManager.Instance != null && InputManager.Instance.SkipPressed) skipRequested = true;
                    if (skipRequested) break;
                    yield return null;
                }

                // 8. Fade hitam, teks penyebab, lalu selesai. Layar tetap hitam sampai GameOver tampil.
                var panel = UIManager.Instance != null ? UIManager.Instance.cinematicPanel : null;
                if (panel != null && bal != null)
                {
                    yield return panel.FadeToBlack(bal.fadeSeconds);
                    if (!string.IsNullOrEmpty(profile.messageKey))
                    {
                        panel.ShowCaption(profile.messageKey);
                        yield return new WaitForSecondsRealtime(bal.messageSeconds);
                    }
                }
            }
            finally
            {
                // 9. Pengaman: skala waktu selalu kembali ke 1.
                if (UIFocusStack.Instance != null) UIFocusStack.Instance.SetGameplayTimeScale(1f);
                isPlaying = false;
                EventChannel<CinematicStateChangedEvent>.Publish(new CinematicStateChangedEvent(false, false));
            }
        }

        private void DisableController()
        {
            var traversal = GetComponent<PlayerTraversal>();
            if (traversal != null) traversal.SetLocked(true);
            var island = GetComponent<IslandPlayerController>();
            if (island != null) island.enabled = false;
            var ship = GetComponent<ShipPlayerController>();
            if (ship != null) ship.enabled = false;
            // Tanpa langkah ini, kapsul tetap menabrak tulang dan tubuh terlempar.
            var cc = GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            for (int i = 0; i < disableOnDeath.Length; i++)
                if (disableOnDeath[i] != null) disableOnDeath[i].enabled = false;
        }

        private void EnableRagdoll(DeathStyle style, DeathReason reason, DeathCinematicBalance bal)
        {
            if (bodyRig == null) return;
            bodyRig.transform.SetPositionAndRotation(transform.position, transform.rotation);
            bodyRig.SetActive(true);
            if (bodyAnimator != null) bodyAnimator.enabled = false;

            int ragdollLayer = LayerMask.NameToLayer(PhysicsMasks.RagdollLayer);
            Rigidbody nearest = null;
            float nearestSq = float.MaxValue;
            for (int i = 0; i < bones.Length; i++)
            {
                var rb = bones[i];
                if (rb == null) continue;
                if (ragdollLayer >= 0) rb.gameObject.layer = ragdollLayer;
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                if (style == DeathStyle.Sink && bal != null) rb.linearDamping = bal.sinkDrag;
                float sq = (rb.position - lastHitPoint).sqrMagnitude;
                if (sq < nearestSq) { nearestSq = sq; nearest = rb; }
            }

            // Tanpa PlayerDamagedEvent sebelumnya (mis. kill konsol), lastHitDir nol dan tubuh roboh tanpa gaya.
            if (style == DeathStyle.Ragdoll && nearest != null && bal != null && lastHitDir.sqrMagnitude > 0.0001f)
            {
                float force = bal.ragdollForce * (reason == DeathReason.Explosion ? bal.explosionForceMultiplier : 1f);
                nearest.AddForceAtPosition(lastHitDir.normalized * force, lastHitPoint, ForceMode.Impulse);
            }
        }

        // ---------------------------------------------------------------- Kamera mengambang

        private struct OrbitState
        {
            public Vector3 startPos;
            public float angle;
            public float elapsed;
        }

        private OrbitState BeginOrbit()
        {
            // Sudut awal dari arah pandang terakhir, supaya tidak terasa melompat.
            Vector3 back = -cameraRig.forward;
            back.y = 0f;
            float angle = back.sqrMagnitude > 0.0001f ? Mathf.Atan2(back.z, back.x) * Mathf.Rad2Deg : 0f;
            return new OrbitState { startPos = cameraRig.position, angle = angle, elapsed = 0f };
        }

        private void TickOrbit(ref OrbitState s, DeathCinematicBalance bal, float dt)
        {
            if (bal == null || cameraRig == null || focusBone == null) return;
            // unscaledDeltaTime, jadi tetap halus saat gerak lambat.
            s.angle += bal.orbitDegreesPerSecond * dt;
            s.elapsed += dt;
            float rad = s.angle * Mathf.Deg2Rad;
            Vector3 focus = focusBone.position;
            Vector3 goal = focus + new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad)) * bal.cameraDistance + Vector3.up * bal.cameraHeight;

            // Halangan: kamera berhenti di depan dinding, tidak menembusnya.
            Vector3 dir = goal - focus;
            float dist = dir.magnitude;
            if (dist > 0.01f && PhysicsQuery.Raycast(new Ray(focus, dir / dist), out var hit, dist, PhysicsMasks.Ground))
                goal = hit.point + hit.normal * 0.2f;

            float t = bal.cameraRiseSeconds > 0f ? Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(s.elapsed / bal.cameraRiseSeconds)) : 1f;
            cameraRig.position = Vector3.Lerp(s.startPos, goal, t);
            cameraRig.LookAt(focus);
        }
    }
}
