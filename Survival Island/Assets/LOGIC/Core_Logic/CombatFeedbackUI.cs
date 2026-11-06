using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Umpan balik pertempuran di HUD: indikator arah damage, penanda serangan dari luar pandangan, Hit Marker, dan Kill Marker.
    /// Blueprint: Core_Logic/CombatFeedbackUI.md (#51). GDD bagian 3 secara tidak langsung; bentuk tampilan adalah usulan
    /// yang disetujui developer sebagai nilai awal.
    ///
    /// Komponen di canvas HUD (anak HUDManager), bukan singleton. Hanya mendengar event, tidak mengubah gameplay,
    /// tidak mengirim event, dan tidak menyimpan data.
    ///
    /// Belum ada (TODO): pilihan SettingsManager.showHitMarkers (usulan 4 blueprint). SettingsManager (#19) sudah ditulis
    /// tanpa field itu, jadi marker selalu aktif.
    /// </summary>
    public class CombatFeedbackUI : MonoBehaviour
    {
        [Header("Susunan HUD")]
        [Tooltip("RectTransform di tengah layar, induk indikator damage.")]
        [SerializeField] private RectTransform damageRing;
        [Tooltip("Anak CrosshairRoot milik HUDManager.")]
        [SerializeField] private GameObject markerRoot;
        [SerializeField] private RectTransform hitMarkerImage;
        [SerializeField] private RectTransform killMarkerImage;

        [Header("Acuan")]
        [Tooltip("Kosong = diambil dari FirstPersonCamera.Instance.WorldCamera (#102).")]
        [SerializeField] private Transform playerCamera;

        [Header("Suara")]
        [SerializeField] private AudioClip hitMarkerClip;
        [SerializeField] private AudioClip killTinkClip;

        private DamageIndicator[] active = new DamageIndicator[0];
        private Camera cameraComponent;
        private float markerTimer;
        private float killTimer;
        private float lastTinkTime = -999f;
        private bool feedbackEnabled = true;
        private bool runOver;   // Pemain mati atau run berakhir, sampai scene diganti

        private static CombatFeedbackBalance B
        {
            get
            {
                var cfg = GameManager.CurrentBalance;
                return cfg != null ? cfg.combatFeedback : null;
            }
        }

        // ---------------------------------------------------------------- Siklus hidup

        private void OnEnable()
        {
            var b = B;
            int size = b != null ? Mathf.Max(1, b.maxIndicators) : 1;
            if (active.Length != size) active = new DamageIndicator[size];

            EventChannel<PlayerDamagedEvent>.Raised += HandlePlayerDamaged;
            EventChannel<EnemyAttackTelegraphEvent>.Raised += HandleTelegraph;
            EventChannel<EnemyHitEvent>.Raised += HandleEnemyHit;
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocusChanged;
            EventChannel<CinematicStateChangedEvent>.Raised += HandleCinematic;
            EventChannel<PlayerDiedEvent>.Raised += HandlePlayerDied;
            EventChannel<RunEndedEvent>.Raised += HandleRunEnded;

            feedbackEnabled = true;
            runOver = false;
            HideMarkers();

            // CombatFeedbackUI.md bagian 4: baca nilai sticky yang sudah terkirim.
            if (EventChannel<UIFocusChangedEvent>.HasLast) HandleFocusChanged(EventChannel<UIFocusChangedEvent>.Last);
            if (EventChannel<CinematicStateChangedEvent>.HasLast) HandleCinematic(EventChannel<CinematicStateChangedEvent>.Last);
        }

        private void OnDisable()
        {
            EventChannel<PlayerDamagedEvent>.Raised -= HandlePlayerDamaged;
            EventChannel<EnemyAttackTelegraphEvent>.Raised -= HandleTelegraph;
            EventChannel<EnemyHitEvent>.Raised -= HandleEnemyHit;
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocusChanged;
            EventChannel<CinematicStateChangedEvent>.Raised -= HandleCinematic;
            EventChannel<PlayerDiedEvent>.Raised -= HandlePlayerDied;
            EventChannel<RunEndedEvent>.Raised -= HandleRunEnded;

            // Indikator adalah anak canvas scene. Dikembalikan sebelum scene hilang, supaya pool tidak kehilangan objek
            // (IPoolable: objek pool tidak boleh tetap menjadi anak objek scene fase).
            ClearAll();
        }

        // ---------------------------------------------------------------- Directional Damage

        private void HandlePlayerDamaged(PlayerDamagedEvent e)
        {
            // 1.
            if (!feedbackEnabled) return;
            var b = B;
            if (b == null || !ResolveCamera()) return;

            // 2. Damage tanpa arah (mis. jatuh) tidak punya indikator. Damage kondisi tidak mengirim event ini (#59).
            if (!TryResolveSource(e.info, b, out Vector3 sourcePos)) return;

            // 3.
            float strength = Mathf.Lerp(b.minAlpha, 1f, b.fullAlphaDamage > 0f ? e.info.amount / b.fullAlphaDamage : 1f);

            // 4. Gabung dengan indikator damage yang arahnya hampir sama.
            float angle = AngleTo(sourcePos);
            for (int i = 0; i < active.Length; i++)
            {
                var ind = active[i];
                if (ind == null || ind.isTelegraph) continue;
                if (Mathf.Abs(Mathf.DeltaAngle(ind.lastAngle, angle)) < b.indicatorMergeAngle)
                {
                    ind.sourcePosition = sourcePos;
                    ind.duration = b.indicatorSeconds;
                    ind.timer = b.indicatorSeconds;
                    ind.strength = Mathf.Max(ind.strength, strength);
                    ind.lastAngle = angle;
                    return;
                }
            }

            // 5. Indikator baru. Kalau penuh, pakai ulang indikator dengan timer terkecil.
            var target = AcquireIndicator(true);
            if (target == null) return;
            target.Setup(sourcePos, b.indicatorSeconds, strength, false);
            target.lastAngle = angle;
        }

        private bool TryResolveSource(in DamageInfo info, CombatFeedbackBalance b, out Vector3 sourcePos)
        {
            Vector3 playerPos = playerCamera.position;
            if (info.hitDirection.sqrMagnitude > 0.0001f)
            {
                // hitDirection = arah dari musuh ke target (EnemyAttack.md), jadi sumbernya di arah sebaliknya.
                sourcePos = playerPos - info.hitDirection.normalized * b.sourceOffsetMeters;
                return true;
            }
            if (info.source != null)
            {
                sourcePos = info.source.transform.position;
                return true;
            }
            sourcePos = default;
            return false;
        }

        private void HandleTelegraph(EnemyAttackTelegraphEvent e)
        {
            if (!feedbackEnabled) return;
            var b = B;
            if (b == null || !ResolveCamera()) return;

            // Serangan yang terlihat tidak perlu penanda (#109).
            if (IsPositionInView(e.position)) return;

            // Penanda mengikuti batas maxIndicators dan tidak menggantikan busur damage.
            var target = AcquireIndicator(false);
            if (target == null) return;
            target.Setup(e.position, b.telegraphSeconds, 1f, true);
            target.lastAngle = AngleTo(e.position);
        }

        /// <summary>Ambil slot kosong. allowReplace = boleh memakai ulang indikator dengan timer terkecil.</summary>
        private DamageIndicator AcquireIndicator(bool allowReplace)
        {
            int free = -1;
            int smallest = -1;
            for (int i = 0; i < active.Length; i++)
            {
                if (active[i] == null) { free = i; break; }
                if (smallest < 0 || active[i].timer < active[smallest].timer) smallest = i;
            }

            if (free < 0)
            {
                if (!allowReplace || smallest < 0) return null;
                return active[smallest];
            }

            var pooler = ObjectPooler.Instance;
            if (pooler == null || damageRing == null) return null;
            var ind = pooler.SpawnFromPool<DamageIndicator>(DamageIndicator.PoolTag, damageRing.position, Quaternion.identity);
            if (ind == null) return null;

            ind.transform.SetParent(damageRing, false);
            ind.Rect.anchoredPosition = Vector2.zero;
            ind.owner = this;
            ind.slotIndex = free;
            active[free] = ind;
            return ind;
        }

        /// <summary>Dipanggil DamageIndicator.OnReturned, supaya slot tidak menyimpan rujukan lama.</summary>
        internal void ReleaseSlot(DamageIndicator ind)
        {
            int i = ind.slotIndex;
            if (i >= 0 && i < active.Length && active[i] == ind) { active[i] = null; return; }
            for (int k = 0; k < active.Length; k++) if (active[k] == ind) active[k] = null;
        }

        // ---------------------------------------------------------------- Per frame

        private void LateUpdate()
        {
            float dt = Time.deltaTime; // Game dijeda: indikator membeku (bagian 4).
            bool hasCamera = ResolveCamera();

            for (int i = 0; i < active.Length; i++)
            {
                var ind = active[i];
                if (ind == null) continue;

                // 1-3.
                if (hasCamera) ind.lastAngle = AngleTo(ind.sourcePosition, ind.lastAngle);
                ind.transform.localEulerAngles = new Vector3(0f, 0f, -ind.lastAngle);

                // 4.
                ind.timer -= dt;
                float fadeWindow = Mathf.Max(0.0001f, ind.duration * 0.5f);
                ind.SetAlpha(ind.strength * Mathf.Clamp01(ind.timer / fadeWindow));

                // 5.
                if (ind.timer <= 0f) ReturnIndicator(ind);
            }

            UpdateMarkers(dt);
        }

        private float AngleTo(Vector3 source) => AngleTo(source, 0f);

        /// <summary>Sudut bertanda dari arah pandang horizontal ke sumber. Sumber di bawah 0,1 m memakai sudut terakhir.</summary>
        private float AngleTo(Vector3 source, float fallback)
        {
            if (playerCamera == null) return fallback;
            Vector3 toSource = source - playerCamera.position;
            toSource.y = 0f;
            if (toSource.sqrMagnitude < 0.01f) return fallback;
            Vector3 forward = playerCamera.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude < 0.0001f) forward = playerCamera.up; // Melihat lurus ke atas atau bawah
            forward.y = 0f;
            return Vector3.SignedAngle(forward, toSource, Vector3.up);
        }

        private bool IsPositionInView(Vector3 position)
        {
            if (!ResolveCamera() || cameraComponent == null) return false;
            Vector3 v = cameraComponent.WorldToViewportPoint(position);
            return v.z > 0f && v.x >= 0f && v.x <= 1f && v.y >= 0f && v.y <= 1f;
        }

        private bool ResolveCamera()
        {
            if (playerCamera != null)
            {
                if (cameraComponent == null) cameraComponent = playerCamera.GetComponent<Camera>();
                return true;
            }
            var fpc = FirstPersonCamera.Instance;
            if (fpc == null || fpc.WorldCamera == null) return false;
            cameraComponent = fpc.WorldCamera;
            playerCamera = cameraComponent.transform;
            return true;
        }

        // ---------------------------------------------------------------- Hit Marker dan Kill Marker

        private void HandleEnemyHit(EnemyHitEvent e)
        {
            // 1. Tembakan NPC tidak memberi marker.
            if (!feedbackEnabled || !e.byPlayer) return;
            var b = B;
            if (b == null) return;

            // 2. Usulan 2: jebakan di luar pandangan tidak memberi marker.
            if (!IsPositionInView(e.position)) return;

            // 3.
            if (e.isKill)
            {
                PlayKillMarker(b);
                return;
            }

            // Kill Marker selalu mengalahkan Hit Marker yang sedang tampil.
            if (killTimer > 0f) return;

            // 4. Usulan 1: berlaku untuk semua serangan pemain. Banyak kena dalam satu frame hanya menyetel ulang timer.
            markerTimer = b.hitMarkerSeconds;
            if (hitMarkerImage != null)
            {
                hitMarkerImage.gameObject.SetActive(true);
                // Usulan 3: marker headshot lebih besar, karena headshot 2x damage (Claude.md Pilar 3).
                float scale = e.bodyPart == BodyPart.Head ? b.headshotMarkerScale : 1f;
                hitMarkerImage.localScale = new Vector3(scale, scale, 1f);
            }

            // 5.
            var audio = AudioManager.Instance;
            if (audio != null) audio.PlaySFX(hitMarkerClip);
        }

        private void PlayKillMarker(CombatFeedbackBalance b)
        {
            // 1.
            killTimer = b.killBlinkSeconds;
            markerTimer = 0f;
            if (hitMarkerImage != null) hitMarkerImage.gameObject.SetActive(false);
            if (killMarkerImage != null) killMarkerImage.gameObject.SetActive(true);

            // 3.
            if (Time.time - lastTinkTime >= b.tinkMinInterval)
            {
                var audio = AudioManager.Instance;
                if (audio != null) audio.PlaySFX(killTinkClip);
                lastTinkTime = Time.time;
            }
        }

        private void UpdateMarkers(float dt)
        {
            if (markerTimer > 0f)
            {
                markerTimer -= dt;
                if (markerTimer <= 0f && hitMarkerImage != null) hitMarkerImage.gameObject.SetActive(false);
            }

            if (killTimer > 0f)
            {
                killTimer -= dt;
                var b = B;
                if (killTimer <= 0f || b == null)
                {
                    killTimer = 0f;
                    if (killMarkerImage != null) killMarkerImage.gameObject.SetActive(false);
                    return;
                }

                // 2. Kedipan: tampil saat Floor(progress * count * 2) genap.
                float progress = 1f - killTimer / Mathf.Max(0.0001f, b.killBlinkSeconds);
                bool show = Mathf.FloorToInt(progress * b.killBlinkCount * 2) % 2 == 0;
                if (killMarkerImage != null) killMarkerImage.gameObject.SetActive(show);
            }
        }

        private void HideMarkers()
        {
            markerTimer = 0f;
            killTimer = 0f;
            if (hitMarkerImage != null) hitMarkerImage.gameObject.SetActive(false);
            if (killMarkerImage != null) killMarkerImage.gameObject.SetActive(false);
        }

        // ---------------------------------------------------------------- Status

        private void HandleFocusChanged(UIFocusChangedEvent e)
        {
            // Panel tanpa jeda (Inventori): busur tetap tampil, MarkerRoot disembunyikan bersama crosshair.
            if (markerRoot != null) markerRoot.SetActive(!e.isGameplayBlocked);
        }

        private void HandleCinematic(CinematicStateChangedEvent e)
        {
            // #114: umpan balik mati selama adegan.
            feedbackEnabled = !e.isPlaying && !runOver;
            if (e.isPlaying) ClearAll();
        }

        private void HandlePlayerDied(PlayerDiedEvent e)
        {
            runOver = true;
            feedbackEnabled = false;
            ClearAll();
        }

        private void HandleRunEnded(RunEndedEvent e)
        {
            runOver = true;
            feedbackEnabled = false;
            ClearAll();
        }

        /// <summary>Kembalikan semua indikator ke pool dan matikan marker (supaya DeathCinematic #53 bersih).</summary>
        public void ClearAll()
        {
            for (int i = 0; i < active.Length; i++)
            {
                var ind = active[i];
                if (ind != null) ReturnIndicator(ind);
                active[i] = null;
            }
            HideMarkers();
        }

        private void ReturnIndicator(DamageIndicator ind)
        {
            var pooler = ObjectPooler.Instance;
            if (pooler != null) pooler.ReturnToPool(ind.gameObject);
            else
            {
                ReleaseSlot(ind);
                ind.gameObject.SetActive(false);
            }
        }
    }
}
