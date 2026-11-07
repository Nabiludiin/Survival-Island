using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>Isi save benda kecil yang tumbuh lagi: data IslandInteractable ditambah jam tumbuh (#73, #80).</summary>
    [Serializable]
    public class RegrowablePickupSaveData : IslandInteractableSaveData
    {
        public float regrowAtGameHour = -1f;
    }

    /// <summary>
    /// Dasar bersama CoconutPickup dan StonePickup: PickupInteractable yang dijadwalkan tumbuh lagi oleh ResourceRespawn.
    /// Blueprint: raft/ResourceRespawn.md bagian "Tumbuh kembali" (Kelapa, Batu kecil dan ranting, #43, #73, #116).
    /// GDD bagian 3: melewati hari ke-60 bukan Game Over, jadi resource tidak boleh habis permanen.
    ///
    /// Pola sama dengan ResourceNode: saat diambil, jam tumbuh = jam absolut + hari tumbuh * 24 + jitter dari aliran
    /// Gameplay (#89), lalu mendaftar ke ResourceRespawn.MarkDepleted. Regrow() mengosongkan isConsumed dan menyalakan objek.
    /// Kelas dasar ini tidak disebut blueprint; dipakai supaya aturan dua jenis benda tidak ditulis dua kali.
    /// </summary>
    public abstract class RegrowablePickup : PickupInteractable, IRegrowable
    {
        private static RandomStream fallbackStream = new RandomStream(0x5EED5UL);

        public float RegrowAtGameHour { get; private set; } = -1f;
        public bool IsDepleted => isConsumed;
        public Vector3 Position => transform.position;

        /// <summary>Hari sampai tumbuh lagi, dari GameBalanceConfig.resource. 0 atau kurang berarti tidak tumbuh lagi.</summary>
        protected abstract float RespawnDays { get; }

        protected static ResourceBalance Resource => GameManager.CurrentBalance != null ? GameManager.CurrentBalance.resource : null;

        protected override bool OnInteract(in InteractContext ctx)
        {
            bool ok = base.OnInteract(ctx);
            // Habis diambil: jadwalkan tumbuh lagi (ResourceRespawn.md "Kelapa" dan "Batu kecil").
            if (ok && isConsumed) ScheduleRegrow();
            return ok;
        }

        private void ScheduleRegrow()
        {
            float days = RespawnDays;
            if (days <= 0f) { RegrowAtGameHour = -1f; return; }
            // Jitter dari aliran Gameplay (#89), seperti ResourceNode. Tanpa RunSeedManager (scene uji) memakai cadangan.
            ref var stream = ref (RunSeedManager.Instance != null
                ? ref RunSeedManager.Instance.GetStream(RandomStreamId.Gameplay)
                : ref fallbackStream);
            var res = Resource;
            float jitter = res != null ? res.regrowJitterHours : 6f;
            RegrowAtGameHour = ItemUseSystem.CurrentTotalGameHours() + days * 24f + stream.Range(0f, jitter);
            if (ResourceRespawn.Instance != null) ResourceRespawn.Instance.MarkDepleted(this);
        }

        /// <summary>Dipanggil ResourceRespawn saat jamnya tiba, atau ForceRegrowStones (#116).</summary>
        public override void Regrow()
        {
            base.Regrow();
            RegrowAtGameHour = -1f;
        }

        protected override void OnDestroy()
        {
            if (ResourceRespawn.Instance != null) ResourceRespawn.Instance.Forget(this);
            base.OnDestroy();
        }

        // ---------------------------------------------------------------- Save (#80, #88)

        public override string CaptureState()
        {
            return JsonUtility.ToJson(new RegrowablePickupSaveData
            {
                saveId = saveId, isConsumed = isConsumed, isOpened = isOpened, amount = SavedAmount,
                regrowAtGameHour = isConsumed ? RegrowAtGameHour : -1f,
            });
        }

        public override void RestoreState(string json)
        {
            base.RestoreState(json);
            if (string.IsNullOrEmpty(json)) return;
            var d = JsonUtility.FromJson<RegrowablePickupSaveData>(json);
            RegrowAtGameHour = d != null && isConsumed ? d.regrowAtGameHour : -1f;
        }

        public override void OnRestoreFinished()
        {
            base.OnRestoreFinished();
            // Benda yang sudah diambil ikut jadwal lagi. Waktu yang sudah lewat langsung diperiksa ResourceRespawn.
            if (isConsumed && RegrowAtGameHour >= 0f && ResourceRespawn.Instance != null) ResourceRespawn.Instance.MarkDepleted(this);
        }
    }
}
