using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Tong Merah (Explosive Barrel): meledak saat ditembak, terkena ledakan, atau terbakar.
    /// Blueprint: Island_Logic/EnvironmentalHazard.md (#59, #96). Usulan bergaya L4D, belum ada di GDD
    /// (GDD bagian 3 hanya menyebut barang lempar untuk kerumunan zombie). Disetujui developer sebagai nilai awal (#85).
    ///
    /// Prefab: collider di layer Obstacle (kena PhysicsMasks.Shoot, tidak ada di PhysicsMasks.Explosion), SaveIdTag,
    /// NoiseEmitter, dan Flammable dengan profil Flame_Barrel. Status disimpan lewat RegionStateStore (objek region, LoadOrder 0).
    /// </summary>
    public class EnvironmentalHazard : MonoBehaviour, IDamageable, ISaveable
    {
        private static readonly List<EnvironmentalHazard> active = new List<EnvironmentalHazard>(16);
        private static EnvironmentalHazard[] chainBuffer = new EnvironmentalHazard[16];

        [SerializeField] private string hazardId = "Barrel";
        [SerializeField] private string explosionVFXPoolTag = "VFX_Explosion";
        [SerializeField] private AudioClip explosionSFX;
        [Tooltip("Model dan collider yang dimatikan setelah meledak. Objek akar tetap ada supaya status bisa dilaporkan.")]
        [SerializeField] private GameObject visualRoot;
        [SerializeField] private NoiseEmitter noiseEmitter;
        [SerializeField] private Flammable flammable;
        [SerializeField] private SaveIdTag saveTag;

        private bool hasExploded;
        private bool isArmed;
        private bool armedByPlayer;
        private bool registeredForSave;

        public bool IsAlive => !hasExploded;
        public bool HasExploded => hasExploded;

        private static HazardBalance B => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.hazard : null;

        // ---------------------------------------------------------------- Siklus hidup

        private void Awake()
        {
            if (saveTag == null) saveTag = GetComponent<SaveIdTag>();
            if (noiseEmitter == null) noiseEmitter = GetComponent<NoiseEmitter>();
            if (flammable == null) flammable = GetComponentInChildren<Flammable>(true);
            // Objek region: RegionStateStore (#88). Tanpa RegionStreaming (scene uji): jalur cadangan WorldStateSave.
            registeredForSave = RegionSaveRouting.Register(this, this);
        }

        private void OnDestroy()
        {
            RegionSaveRouting.Unregister(this, ref registeredForSave);
        }

        private void OnEnable()
        {
            if (!hasExploded && !active.Contains(this)) active.Add(this);
        }

        private void OnDisable()
        {
            active.Remove(this);
            // Kasus khusus: region dilepas saat sumbu berjalan. Coroutine berhenti, isArmed tidak disimpan (usulan 7).
            isArmed = false;
        }

        // ---------------------------------------------------------------- Pemicu

        public void TakeDamage(in DamageInfo info)
        {
            // 1-2.
            if (hasExploded || isArmed || info.amount <= 0f) return;
            var b = B;
            // 3. Usulan 2: peluru, ledakan, dan api memicu tong; melee dan zombie tidak.
            switch (info.type)
            {
                case DamageType.Bullet:
                case DamageType.Explosion:
                    Arm(info.byPlayer, 0f);
                    break;
                case DamageType.Fire:
                    // Usulan 3: sumbu api 2 detik.
                    Arm(info.byPlayer, b != null ? b.fireFuseSeconds : 2f);
                    break;
            }
        }

        private void Arm(bool byPlayer, float delay)
        {
            if (!isActiveAndEnabled) return;
            isArmed = true;
            armedByPlayer = byPlayer;
            StartCoroutine(FuseRoutine(delay));
        }

        private IEnumerator FuseRoutine(float delay)
        {
            // Selalu menunggu minimal satu frame: ExplosionDamage memakai buffer statis, jadi ledakan tidak boleh
            // terjadi di dalam TakeDamage yang dipanggil ExplosionDamage.Apply (Kasus khusus "Rekursi buffer").
            if (delay <= 0f) yield return null;
            else yield return new WaitForSeconds(delay);   // Ikut Time.timeScale, berhenti saat jeda
            Detonate();
        }

        // ---------------------------------------------------------------- Ledakan

        private void Detonate()
        {
            // 1.
            if (hasExploded) return;
            hasExploded = true;
            isArmed = false;
            active.Remove(this);

            var b = B;
            Vector3 pos = transform.position;
            // 2-3.
            if (VFXManager.Instance != null && !string.IsNullOrEmpty(explosionVFXPoolTag))
                VFXManager.Instance.Play(explosionVFXPoolTag, pos, Quaternion.identity);
            if (AudioManager.Instance != null && explosionSFX != null) AudioManager.Instance.PlaySFX(explosionSFX);
            CameraShake.PlayAt(CameraShakeKind.Explosion, pos);

            // 4-5. Falloff, target unik, dan friendly fire diatur ExplosionDamage.
            var info = new DamageInfo
            {
                amount = b != null ? b.barrelDamage : 100f,
                type = DamageType.Explosion,
                source = gameObject,
                sourceId = hazardId,
                hitPoint = pos,
                bodyPart = BodyPart.Torso,
                byPlayer = armedByPlayer
            };
            float radius = b != null ? b.barrelRadius : 5f;
            ExplosionDamage.Apply(pos, radius, info);
            // 8. Rantai ledakan: ExplosionDamage.Apply memanggil NotifyExplosion untuk tong lain di dalam radius.

            // 6. Usulan 6: belum ada NoiseSourceKind khusus ledakan.
            float noise = b != null ? b.noiseRadius : 40f;
            if (noiseEmitter != null) noiseEmitter.Emit(noise, NoiseSourceKind.Throwable);
            else NoiseEmitter.Emit(pos, noise, NoiseSourceKind.Throwable);

            // 7. Matikan tampilan, bukan Destroy: RegionStreaming.CaptureRegion memakai daftar ISaveable yang dikumpulkan.
            ApplyExplodedVisual();
        }

        private void ApplyExplodedVisual()
        {
            if (visualRoot != null) visualRoot.SetActive(!hasExploded);
            if (flammable != null)
            {
                if (hasExploded) flammable.Extinguish(true);
                flammable.enabled = !hasExploded;
            }
        }

        // ---------------------------------------------------------------- Ledakan berantai

        /// <summary>
        /// Tong di dalam radius ikut bersumbu chainDelaySeconds. Dipanggil tong sendiri, dan sebaiknya oleh ExplosionDamage.Apply
        /// supaya Bom Rakitan (#96) dan Fireball (#70) ikut meledakkan tong. Tanpa query fisika dan tanpa alokasi.
        /// </summary>
        public static void NotifyExplosion(Vector3 center, float radius, bool byPlayer)
        {
            int count = active.Count;
            if (count == 0) return;
            // 3. Salin ke buffer, karena Arm bisa mengubah active.
            if (chainBuffer.Length < count) chainBuffer = new EnvironmentalHazard[Mathf.NextPowerOfTwo(count)];
            active.CopyTo(chainBuffer);
            var b = B;
            float delay = b != null ? b.chainDelaySeconds : 0.15f;
            float r2 = radius * radius;
            for (int i = 0; i < count; i++)
            {
                var h = chainBuffer[i];
                chainBuffer[i] = null;
                if (h == null || h.hasExploded || h.isArmed) continue;
                if ((h.transform.position - center).sqrMagnitude <= r2) h.Arm(byPlayer, delay);
            }
        }

        // ---------------------------------------------------------------- Save (lewat RegionStateStore)

        [Serializable]
        private class SaveData { public bool hasExploded; }

        public string SaveId => saveTag != null ? saveTag.SaveId : string.Empty;
        public int LoadOrder => 0;

        public string CaptureState() => JsonUtility.ToJson(new SaveData { hasExploded = hasExploded });

        public void RestoreState(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            SaveData d = null;
            try { d = JsonUtility.FromJson<SaveData>(json); }
            catch (Exception e) { Debug.LogWarning($"[EnvironmentalHazard] Save tidak terbaca: {e.Message}"); }
            if (d == null) return;
            hasExploded = d.hasExploded;
            isArmed = false;
            if (hasExploded) active.Remove(this);
            else if (isActiveAndEnabled && !active.Contains(this)) active.Add(this);
            ApplyExplodedVisual();
        }

        public void OnRestoreFinished() { }   // Tidak mengirim event
    }
}
