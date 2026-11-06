using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Sumber rotasi dan posisi tambahan kamera (getaran #24, ship sway #75).</summary>
    public interface ICameraOffsetSource
    {
        Vector3 EulerOffset { get; }      // Derajat
        Vector3 PositionOffset { get; }   // Meter, lokal
    }

    public enum ViewmodelMode { Visible, Lowered, Hidden }

    /// <summary>Posisi item di layar, terhadap ViewmodelCamera (FirstPersonCamera.md, tabel "Posisi tiap item").</summary>
    [Serializable]
    public struct ViewmodelPose
    {
        public Vector3 position;
        public Vector3 rotation;
        public float swayMultiplier;
        public float bobMultiplier;

        public ViewmodelPose(Vector3 position, Vector3 rotation, float sway, float bob)
        {
            this.position = position;
            this.rotation = rotation;
            swayMultiplier = sway;
            bobMultiplier = bob;
        }

        /// <summary>Tangan kosong (nilai awal tabel).</summary>
        public static ViewmodelPose EmptyHands => new ViewmodelPose(new Vector3(0.22f, -0.30f, 0.40f), new Vector3(0f, -10f, 0f), 1f, 1f);
    }

    /// <summary>
    /// Pengaturan pemain untuk kamera. Pemilik nilai: SettingsManager (#19). Selama #19 belum ada, nilai awal tabel
    /// "Pengaturan pemain dari #19" di FirstPersonCamera.md dipakai. #19 memanggil FirstPersonCamera.SetViewSettings.
    /// </summary>
    [Serializable]
    public struct PlayerViewSettings
    {
        public float mouseSensitivity;
        public bool invertY;
        public float fieldOfView;
        public bool headBobEnabled;
        public bool weaponSwayEnabled;
        public bool fovKickEnabled;

        public static PlayerViewSettings Default => new PlayerViewSettings
        {
            mouseSensitivity = 0.12f, invertY = false, fieldOfView = 70f,
            headBobEnabled = true, weaponSwayEnabled = true, fovKickEnabled = true
        };
    }

    /// <summary>
    /// Mouse look untuk Fase 1 dan Fase 2, kamera dunia dan kamera tangan, penggabungan rotasi tambahan, dan pose item.
    /// Blueprint: Core_Logic/FirstPersonCamera.md (#102). GDD bagian 1 (First-Person) dan bagian 3 (kapal miring).
    /// Susunan rig, angka, dan "tanpa ADS" adalah usulan yang disetujui developer (#85).
    ///
    /// Susunan: Player (yaw) > CameraRig (tinggi mata) > PitchPivot (pitch) > OffsetPivot (tambahan) > WorldCamera,
    /// ViewmodelCamera > ViewmodelRoot. Rotasi tambahan tidak pernah mencemari yaw dan pitch pemain.
    /// </summary>
    [DefaultExecutionOrder(200)]
    public class FirstPersonCamera : MonoBehaviour
    {
        public static FirstPersonCamera Instance { get; private set; }

        [Header("Rig")]
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform cameraRig;
        [SerializeField] private Transform pitchPivot;
        [SerializeField] private Transform offsetPivot;
        [SerializeField] private Camera worldCamera;
        [SerializeField] private Camera viewmodelCamera;
        [SerializeField] private Transform viewmodelRoot;
        [SerializeField] private float viewmodelLowerDistance = 0.5f;   // Jarak turun viewmodel, data prefab

        private CharacterController controller;
        private IslandPlayerController islandController;

        private float yaw, pitch;
        private readonly List<ICameraOffsetSource> offsetSources = new List<ICameraOffsetSource>(8);
        private Transform forcedLookTarget;
        private float yawLimitCenter, yawLimitRange;
        private float eyeHeightCurrent, eyeHeightTarget;
        private ViewmodelMode viewmodelMode = ViewmodelMode.Visible;
        private int skipLookFrames;
        private bool lookEnabled = true;
        private bool detached;
        private bool carryingHeavy;
        private bool wasGameplayBlocked;

        private Vector3 recoilEuler;
        private Vector3 bobEuler, bobPosition, landingPosition;
        private float bobPhase;
        private Vector2 swayOffset;
        private Vector2 lastLook;
        private float lowerAmount;   // 0 = tampil penuh, 1 = turun penuh
        private ViewmodelPose mainPose = ViewmodelPose.EmptyHands;
        private PlayerViewSettings settings = PlayerViewSettings.Default;

        public Camera WorldCamera => worldCamera;
        public Camera ViewmodelCamera => viewmodelCamera;
        public Transform ViewmodelRoot => viewmodelRoot;
        public float Yaw => yaw;
        public float Pitch => pitch;
        public ViewmodelMode CurrentViewmodelMode => viewmodelMode;

        private static GameBalanceConfig Balance => GameManager.CurrentBalance;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            Instance = this;
            if (playerBody == null) playerBody = transform;
            controller = playerBody.GetComponent<CharacterController>();
            islandController = playerBody.GetComponent<IslandPlayerController>();
            yaw = playerBody.eulerAngles.y;
            eyeHeightCurrent = eyeHeightTarget = Balance != null ? Balance.camera.eyeHeightStand : 1.6f;
            if (worldCamera != null) worldCamera.fieldOfView = settings.fieldOfView;
            if (viewmodelCamera != null) viewmodelCamera.fieldOfView = Balance != null ? Balance.camera.viewmodelFov : 55f;
        }

        private void OnEnable()
        {
            EventChannel<UIFocusChangedEvent>.Raised += HandleFocus;
            EventChannel<HeavyItemChangedEvent>.Raised += HandleHeavyItem;
            carryingHeavy = EventChannel<HeavyItemChangedEvent>.HasLast && EventChannel<HeavyItemChangedEvent>.Last.isCarried;
            wasGameplayBlocked = EventChannel<UIFocusChangedEvent>.HasLast && EventChannel<UIFocusChangedEvent>.Last.isGameplayBlocked;
        }

        private void OnDisable()
        {
            EventChannel<UIFocusChangedEvent>.Raised -= HandleFocus;
            EventChannel<HeavyItemChangedEvent>.Raised -= HandleHeavyItem;
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        // ---------------------------------------------------------------- Mouse look

        private void Update()
        {
            lastLook = Vector2.zero;
            if (detached || playerBody == null) return;

            if (forcedLookTarget != null)
            {
                UpdateForcedLook();
                return;
            }
            if (!lookEnabled) return;
            if (skipLookFrames > 0) { skipLookFrames--; return; }
            var input = InputManager.Instance;
            if (input == null) return;

            float lookScale = carryingHeavy && Balance != null ? Balance.camera.heavyLookScale : 1f;
            // LookDelta TIDAK dikali Time.deltaTime (InputManager.md).
            Vector2 look = input.LookDelta * settings.mouseSensitivity * lookScale;
            lastLook = look;
            float maxLook = Balance != null ? Balance.camera.maxLookAngle : 80f;
            yaw += look.x;
            pitch += settings.invertY ? look.y : -look.y;
            pitch = Mathf.Clamp(pitch, -maxLook, maxLook);
            if (yawLimitRange > 0f)
                yaw = yawLimitCenter + Mathf.Clamp(Mathf.DeltaAngle(yawLimitCenter, yaw), -yawLimitRange, yawLimitRange);
            playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        /// <summary>Diterkam (#110): rotasi menuju penyerang, input mouse mati. yaw/pitch tetap diisi, jadi lepas tanpa lompatan.</summary>
        private void UpdateForcedLook()
        {
            Vector3 eye = cameraRig != null ? cameraRig.position : playerBody.position;
            Vector3 dir = forcedLookTarget.position - eye;
            if (dir.sqrMagnitude < 0.0001f) return;
            var look = Quaternion.LookRotation(dir).eulerAngles;
            float speed = 360f * Time.deltaTime;
            yaw = Mathf.MoveTowardsAngle(yaw, look.y, speed);
            float targetPitch = Mathf.DeltaAngle(0f, look.x);
            pitch = Mathf.MoveTowards(pitch, targetPitch, speed);
            playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);
        }

        private void LateUpdate()
        {
            if (playerBody == null) return;
            var b = Balance != null ? Balance.camera : null;
            float dt = Time.deltaTime;

            // 1. Tinggi mata.
            if (cameraRig != null && !detached)
            {
                eyeHeightCurrent = Mathf.MoveTowards(eyeHeightCurrent, eyeHeightTarget, (b != null ? b.eyeHeightSpeed : 6f) * dt);
                var p = cameraRig.localPosition;
                p.y = eyeHeightCurrent;
                cameraRig.localPosition = p;
            }

            // 2. Pitch dasar.
            if (pitchPivot != null) pitchPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            // 3. Rotasi tambahan.
            UpdateRecoil(dt, b);
            UpdateHeadBob(dt, b);
            UpdateLandingDip(dt, b);
            if (offsetPivot != null)
            {
                Vector3 euler = recoilEuler + bobEuler;
                Vector3 pos = bobPosition + landingPosition;
                for (int i = 0; i < offsetSources.Count; i++)
                {
                    var s = offsetSources[i];
                    if (s == null) continue;
                    euler += s.EulerOffset;
                    pos += s.PositionOffset;
                }
                euler = ClampPerAxis(euler, b != null ? b.maxOffsetDegrees : 15f);
                offsetPivot.localRotation = Quaternion.Euler(euler);
                offsetPivot.localPosition = pos;
            }

            // 4. FOV.
            UpdateFov(dt, b);

            // 5. Sway dan pose viewmodel.
            UpdateViewmodel(dt, b);
        }

        private static Vector3 ClampPerAxis(Vector3 v, float max)
        {
            return new Vector3(Mathf.Clamp(v.x, -max, max), Mathf.Clamp(v.y, -max, max), Mathf.Clamp(v.z, -max, max));
        }

        private void UpdateRecoil(float dt, CameraBalance b)
        {
            float rate = (b != null ? b.recoilRecoverPerSec : 12f) * dt;
            recoilEuler = Vector3.MoveTowards(recoilEuler, Vector3.zero, rate);
        }

        /// <summary>Head bob dari kecepatan horizontal CharacterController, bukan input (diam saat menabrak dinding).</summary>
        private void UpdateHeadBob(float dt, CameraBalance b)
        {
            float speed = 0f;
            bool grounded = false;
            if (controller != null && controller.enabled)
            {
                var v = controller.velocity;
                v.y = 0f;
                speed = v.magnitude;
                grounded = controller.isGrounded;
            }

            float amplitude = b != null ? b.bobAmplitude : 0.025f;
            bool active = settings.headBobEnabled && grounded && speed > 0.5f && !detached;
            if (active)
            {
                float walk = Balance != null ? Balance.traversal.walkSpeed : 5f;
                bobPhase += (b != null ? b.bobFrequency : 9f) * (speed / Mathf.Max(0.1f, walk)) * dt;
                float mult = 1f;
                if (islandController != null && islandController.IsSprinting) mult = b != null ? b.bobSprintMultiplier : 1.5f;
                if (carryingHeavy) mult = b != null ? b.bobHeavyMultiplier : 1.8f;
                bobPosition = new Vector3(0f, Mathf.Sin(bobPhase) * amplitude * mult, 0f);
                bobEuler = new Vector3(0f, 0f, Mathf.Cos(bobPhase * 0.5f) * amplitude * mult * 20f);
            }
            else
            {
                // Kembali ke tengah saat diam.
                bobPosition = Vector3.MoveTowards(bobPosition, Vector3.zero, amplitude * 4f * dt);
                bobEuler = Vector3.MoveTowards(bobEuler, Vector3.zero, 2f * dt);
            }
        }

        private void UpdateLandingDip(float dt, CameraBalance b)
        {
            landingPosition = Vector3.MoveTowards(landingPosition, Vector3.zero, (b != null ? b.landingDipRecoverSpeed : 1.5f) * dt);
        }

        private void UpdateFov(float dt, CameraBalance b)
        {
            if (worldCamera == null) return;
            float target = settings.fieldOfView;
            bool sprinting = islandController != null && islandController.IsSprinting;
            // Heavy Item: tidak ada FOV lari.
            if (settings.fovKickEnabled && sprinting && !carryingHeavy) target += b != null ? b.fovSprintBonus : 8f;
            worldCamera.fieldOfView = Mathf.MoveTowards(worldCamera.fieldOfView, target, (b != null ? b.fovChangeSpeed : 40f) * dt);
        }

        private void UpdateViewmodel(float dt, CameraBalance b)
        {
            if (viewmodelRoot == null) return;
            bool hidden = viewmodelMode == ViewmodelMode.Hidden || detached;
            if (viewmodelRoot.gameObject.activeSelf == hidden) viewmodelRoot.gameObject.SetActive(!hidden);
            if (hidden) return;

            float targetLower = viewmodelMode == ViewmodelMode.Lowered ? 1f : 0f;
            lowerAmount = Mathf.MoveTowards(lowerAmount, targetLower, (b != null ? b.lowerSpeed : 8f) * dt);

            Vector2 swayTarget = Vector2.zero;
            if (settings.weaponSwayEnabled)
            {
                float amount = b != null ? b.swayAmount : 0.02f;
                float max = b != null ? b.swayMax : 0.06f;
                swayTarget = -Vector2.ClampMagnitude(lastLook * amount, max) * mainPose.swayMultiplier;
            }
            swayOffset = Vector2.Lerp(swayOffset, swayTarget, (b != null ? b.swaySmooth : 10f) * dt);

            Vector3 pos = mainPose.position + new Vector3(swayOffset.x, swayOffset.y, 0f)
                          + bobPosition * mainPose.bobMultiplier
                          + Vector3.down * (lowerAmount * viewmodelLowerDistance);
            viewmodelRoot.localPosition = pos;
            viewmodelRoot.localRotation = Quaternion.Euler(mainPose.rotation);
        }

        // ---------------------------------------------------------------- Event

        private void HandleFocus(UIFocusChangedEvent e)
        {
            // Kamera tidak melompat setelah panel tertutup.
            if (wasGameplayBlocked && !e.isGameplayBlocked) skipLookFrames = 1;
            wasGameplayBlocked = e.isGameplayBlocked;
        }

        private void HandleHeavyItem(HeavyItemChangedEvent e)
        {
            carryingHeavy = e.isCarried;
        }

        // ---------------------------------------------------------------- API publik

        /// <summary>Fase 1 (ShipPlayerController, ShipControlLevel.None). LateUpdate tetap menerapkan ship sway dan getaran.</summary>
        public void SetLookEnabled(bool enabled)
        {
            if (enabled && !lookEnabled) skipLookFrames = 1;
            lookEnabled = enabled;
        }

        public void RegisterOffsetSource(ICameraOffsetSource source)
        {
            if (source != null && !offsetSources.Contains(source)) offsetSources.Add(source);
        }

        /// <summary>Aman dipanggil dua kali.</summary>
        public void UnregisterOffsetSource(ICameraOffsetSource source)
        {
            offsetSources.Remove(source);
        }

        /// <summary>Recoil dari GunController (#58). Pulih recoilRecoverPerSec.</summary>
        public void AddRecoil(float pitchUp, float yawSide)
        {
            recoilEuler += new Vector3(-pitchUp, yawSide, 0f);
        }

        /// <summary>Target paksa saat diterkam (#110). null melepas; yaw dan pitch sudah berisi rotasi akhir.</summary>
        public void SetForcedLook(Transform target)
        {
            forcedLookTarget = target;
            if (target == null) skipLookFrames = 1;
        }

        /// <summary>Tinggi mata tujuan. Dipakai jongkok (#101).</summary>
        public void SetEyeHeight(float height)
        {
            eyeHeightTarget = height;
        }

        /// <summary>Tinggi mata berdiri dari config.</summary>
        public void ResetEyeHeight()
        {
            eyeHeightTarget = Balance != null ? Balance.camera.eyeHeightStand : 1.6f;
        }

        /// <summary>Tangga (#101): yaw dibatasi ladderYawRange, viewmodel turun.</summary>
        public void EnterClimbView(Vector3 ladderForward)
        {
            ladderForward.y = 0f;
            if (ladderForward.sqrMagnitude > 0.0001f) yawLimitCenter = Quaternion.LookRotation(ladderForward).eulerAngles.y;
            else yawLimitCenter = yaw;
            yawLimitRange = Balance != null ? Balance.camera.ladderYawRange : 70f;
            SetViewmodelMode(ViewmodelMode.Lowered);
        }

        public void ExitClimbView()
        {
            yawLimitRange = 0f;
            SetViewmodelMode(ViewmodelMode.Visible);
        }

        /// <summary>Dip kamera saat mendarat (#101). impact01 = 0..1.</summary>
        public void PlayLandingDip(float impact01)
        {
            float max = Balance != null ? Balance.camera.landingDipMax : 0.25f;
            landingPosition = Vector3.down * (Mathf.Clamp01(impact01) * max);
        }

        /// <summary>Pose item utama (dipanggil EquipmentManager saat berganti item).</summary>
        public void SetMainPose(ViewmodelPose pose)
        {
            mainPose = pose;
        }

        public void SetViewmodelMode(ViewmodelMode mode)
        {
            viewmodelMode = mode;
        }

        /// <summary>Kematian (#53): rig dilepas dari pemain, input berhenti, offset dinolkan, viewmodel hilang.</summary>
        public void DetachForDeath()
        {
            if (detached) return;
            detached = true;
            recoilEuler = bobEuler = bobPosition = landingPosition = Vector3.zero;
            offsetSources.Clear();
            if (offsetPivot != null)
            {
                offsetPivot.localRotation = Quaternion.identity;
                offsetPivot.localPosition = Vector3.zero;
            }
            if (viewmodelCamera != null) viewmodelCamera.enabled = false;
            if (cameraRig != null) cameraRig.SetParent(null, true);
        }

        /// <summary>Arah pandang langsung (save #80, arah awal di pantai #74).</summary>
        public void SetYawPitch(float newYaw, float newPitch)
        {
            yaw = newYaw;
            float maxLook = Balance != null ? Balance.camera.maxLookAngle : 80f;
            pitch = Mathf.Clamp(newPitch, -maxLook, maxLook);
            if (playerBody != null) playerBody.rotation = Quaternion.Euler(0f, yaw, 0f);
            skipLookFrames = 1;
        }

        /// <summary>Menghadap titik dunia (IslandArrival.lookTarget).</summary>
        public void LookAt(Vector3 worldPoint)
        {
            Vector3 eye = cameraRig != null ? cameraRig.position : playerBody.position;
            Vector3 dir = worldPoint - eye;
            if (dir.sqrMagnitude < 0.0001f) return;
            var e = Quaternion.LookRotation(dir).eulerAngles;
            SetYawPitch(e.y, Mathf.DeltaAngle(0f, e.x));
        }

        /// <summary>Pengaturan pemain dari SettingsManager (#19). Langsung berlaku.</summary>
        public void SetViewSettings(PlayerViewSettings value)
        {
            settings = value;
        }
    }
}
