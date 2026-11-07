using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Trigger di Region3_Snow yang memutar adegan helipad_found saat pemain pertama kali masuk, sekali per run.
    /// GDD bagian 5 Region 3: helipad tempat helikopter. Blueprint: Core_Logic/NarrativeSequencer.md (#99),
    /// bagian "HelipadFoundVolume". Usulan 6 disetujui developer: adegan ini membekukan dunia.
    ///
    /// Collider trigger di layer ZoneTrigger. ISaveable objek region: dikumpulkan RegionSceneRoot (#88) dan disimpan di
    /// RegionStateStore; tanpa RegionStreaming (scene uji) mendaftar ke WorldStateSave lewat RegionSaveRouting.
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class HelipadFoundVolume : MonoBehaviour, ISaveable
    {
        [Serializable]
        private struct SaveData { public bool isConsumed; }

        [SerializeField] private string saveId = "region3_helipad_found";
        [SerializeField] private string sceneId = NarrativeSequencer.HelipadFound;

        private bool isConsumed;
        private bool registeredForSave;

        public string SaveId => saveId;
        public int LoadOrder => 0;   // Objek region

        // Di region scene: RegionStateStore (#88). Scene uji tanpa RegionStreaming: jalur cadangan WorldStateSave.
        private void Awake() { registeredForSave = RegionSaveRouting.Register(this, this); }
        private void OnDestroy() { RegionSaveRouting.Unregister(this, ref registeredForSave); }

        private void OnTriggerEnter(Collider other)
        {
            if (isConsumed || other == null || !other.CompareTag("Player")) return;
            var narrative = NarrativeSequencer.Instance;
            var rl = RunLifecycle.Instance;
            if (narrative == null || narrative.IsPlaying) return;
            if (rl != null && rl.State != RunState.OnIsland) return;

            isConsumed = true;
            // Coroutine berjalan di objek persisten, jadi tetap selesai walau region ini dilepas.
            narrative.StartCoroutine(narrative.Play(sceneId));
        }

        public string CaptureState() => JsonUtility.ToJson(new SaveData { isConsumed = isConsumed });

        public void RestoreState(string json)
        {
            if (string.IsNullOrEmpty(json)) return;
            try { isConsumed = JsonUtility.FromJson<SaveData>(json).isConsumed; }
            catch (Exception ex) { Debug.LogWarning($"[HelipadFoundVolume] Gagal membaca save: {ex.Message}", this); }
        }

        public void OnRestoreFinished() { }
    }
}
