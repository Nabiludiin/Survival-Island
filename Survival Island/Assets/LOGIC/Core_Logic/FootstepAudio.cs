using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Suara langkah dan mendarat pemain sesuai permukaan yang diinjak, di kapal dan di pulau.
    /// Blueprint: Core_Logic/FootstepAudio.md (#21). Mendukung GDD bagian 3 (Fase 1 di kapal) dan bagian 5 (region);
    /// sistem suara langkah sendiri adalah usulan teknis.
    ///
    /// Komponen scene di prefab pemain Fase 1 dan Fase 2. Hanya membaca CharacterController (dan PlayerTraversal kalau ada).
    /// Permukaan dibaca lewat SurfaceDetection (#86) sekali per langkah; komponen ini tidak menembakkan raycast sendiri.
    /// Tidak mengirim NoiseEmittedEvent: noise untuk musuh milik #49.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class FootstepAudio : MonoBehaviour
    {
        [SerializeField] private SurfaceAudioSet audioSet;
        [Tooltip("AudioSource 2D di objek kaki, keluar ke grup SFX.")]
        [SerializeField] private AudioSource source;

        private CharacterController controller;
        private PlayerTraversal traversal;

        private SurfaceAudioEntry[] lookup;   // Indeks (int)SurfaceType, tanpa Dictionary
        private float distanceSinceStep;
        private int lastClipIndex = -1;
        private SurfaceType lastSurface = SurfaceType.Tanah;
        private float airTime;
        private float lastFallSpeed;
        private bool isCarryingHeavy;

        private static FootstepBalance Bal => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.footstep : null;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            traversal = GetComponent<PlayerTraversal>();   // null di kapal
            if (source == null) source = GetComponentInChildren<AudioSource>();
            if (source != null)
            {
                source.spatialBlend = 0f;
                source.playOnAwake = false;
                // Klip ditahan saat jeda dan lanjut setelahnya (#83).
                source.ignoreListenerPause = false;
            }
            BuildLookup();
        }

        private void BuildLookup()
        {
            int n = System.Enum.GetValues(typeof(SurfaceType)).Length;
            lookup = new SurfaceAudioEntry[n];
            if (audioSet != null && audioSet.entries != null)
            {
                for (int i = 0; i < audioSet.entries.Length; i++)
                {
                    var e = audioSet.entries[i];
                    int idx = (int)e.type;
                    if (idx >= 0 && idx < n) lookup[idx] = e;
                }
            }
            // Baris kosong diisi dari Tanah (FootstepAudio.md bagian 2).
            var fallback = lookup[(int)SurfaceType.Tanah];
            for (int i = 0; i < n; i++)
            {
                if (lookup[i].steps != null && lookup[i].steps.Length > 0) continue;
#if UNITY_EDITOR
                if (i != (int)SurfaceType.Tanah)
                    Debug.LogWarning($"[FootstepAudio] SurfaceAudioSet tidak punya klip untuk {(SurfaceType)i}. Memakai Tanah.", this);
#endif
                lookup[i] = fallback;
                lookup[i].type = (SurfaceType)i;
            }
        }

        private void OnEnable()
        {
            EventChannel<HeavyItemChangedEvent>.Raised += OnHeavyItemChanged;
            // Sticky: dibaca saat aktif.
            if (EventChannel<HeavyItemChangedEvent>.HasLast) isCarryingHeavy = EventChannel<HeavyItemChangedEvent>.Last.isCarried;
        }

        private void OnDisable()
        {
            EventChannel<HeavyItemChangedEvent>.Raised -= OnHeavyItemChanged;
        }

        private void OnHeavyItemChanged(HeavyItemChangedEvent e) => isCarryingHeavy = e.isCarried;

        private void Update()
        {
            // Pemain mati: CharacterController dimatikan #53.
            if (controller == null || !controller.enabled) return;
            var b = Bal;
            if (b == null) return;

            Vector3 v = controller.velocity;
            float speed = new Vector2(v.x, v.z).magnitude;
            bool grounded = controller.isGrounded;

            UpdateLanding(grounded, v.y, b);
            if (!CanStep(grounded, speed, b)) { distanceSinceStep = 0f; return; }

            // Langkah dihitung dari jarak (usulan disetujui developer), jadi lari otomatis lebih rapat dan seirama head bob (#102).
            distanceSinceStep += speed * Time.deltaTime;
            if (b.stepDistance > 0f && distanceSinceStep >= b.stepDistance)
            {
                distanceSinceStep -= b.stepDistance;
                PlayStep(speed, b);
            }
        }

        private bool CanStep(bool grounded, float speed, FootstepBalance b)
        {
            if (!grounded || speed < b.minSpeed) return false;
            // Climbing, Sliding, Swimming, dan Locked tidak menghasilkan langkah. Suara air milik #13.
            if (traversal == null) return true;
            var mode = traversal.Mode;
            return mode == TraversalMode.Ground || mode == TraversalMode.Crouching;
        }

        private SurfaceType ReadSurface()
        {
            // Titik dasar CharacterController. Raycast gagal (region belum dimuat): pakai lastSurface.
            var sd = SurfaceDetection.Instance;
            if (sd != null && sd.TryGetSurfaceAt(transform.position, out var sample)) lastSurface = sample.type;
            return lastSurface;
        }

        private void PlayStep(float speed, FootstepBalance b)
        {
            if (source == null) return;
            var entry = lookup[(int)ReadSurface()];
            var clip = PickClip(entry.steps);
            if (clip == null) return;

            // Suara tampilan saja: UnityEngine.Random biasa, tanpa RunSeedManager (#89).
            float vol = b.baseVolume * EntryVolume(entry) * Random.Range(1f - b.volumeJitter, 1f);
            if (traversal != null && traversal.IsCrouching) vol *= b.crouchVolumeMultiplier;
            else if (speed >= b.sprintSpeedThreshold) vol *= b.sprintVolumeMultiplier;
            if (isCarryingHeavy) vol *= b.heavyVolumeMultiplier;

            source.pitch = Random.Range(b.pitchMin, b.pitchMax);
            source.PlayOneShot(clip, vol);
        }

        private void UpdateLanding(bool grounded, float velocityY, FootstepBalance b)
        {
            if (!grounded)
            {
                airTime += Time.deltaTime;
                lastFallSpeed = Mathf.Max(lastFallSpeed, -velocityY);
                return;
            }
            if (airTime >= b.minLandingAirSeconds)
                PlayLanding(b.landingFullFallSpeed > 0f ? Mathf.Clamp01(lastFallSpeed / b.landingFullFallSpeed) : 1f, b);
            airTime = 0f;
            lastFallSpeed = 0f;
        }

        private void PlayLanding(float impact01, FootstepBalance b)
        {
            // Langkah pertama setelah mendarat tidak menumpuk.
            distanceSinceStep = 0f;
            if (source == null) return;
            var entry = lookup[(int)ReadSurface()];
            var clips = entry.landings != null && entry.landings.Length > 0 ? entry.landings : entry.steps;
            var clip = PickClip(clips);
            if (clip == null) return;
            source.pitch = Random.Range(b.pitchMin, b.pitchMax);
            source.PlayOneShot(clip, b.baseVolume * EntryVolume(entry) * Mathf.Lerp(b.landingVolumeMin, b.landingVolumeMax, impact01));
        }

        // Baris yang belum diisi di Inspector punya volume 0; dianggap nilai awal 1.
        private static float EntryVolume(in SurfaceAudioEntry e) => e.volume > 0f ? e.volume : 1f;

        /// <summary>Klip acak, tidak sama dengan klip sebelumnya.</summary>
        private AudioClip PickClip(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0) return null;
            int i;
            if (clips.Length == 1) i = 0;
            else
            {
                i = Random.Range(0, clips.Length - 1);
                if (i >= lastClipIndex && lastClipIndex >= 0 && lastClipIndex < clips.Length) i++;
            }
            lastClipIndex = i;
            return clips[i];
        }
    }
}
