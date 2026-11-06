using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Tampilan riwayat pesan di dalam panel Pause, dibuka lewat tombol Log. Bukan UIPanel sendiri.
    /// Blueprint: Core_Logic/NotificationManager.md bagian "Log event" (#22). Usulan disetujui developer (#22 poin 2).
    /// Isi per baris: "Hari 12, 21:00  Base terbakar"; di Fase 1 jam ditulis lewat kunci ui.log.ship ("Kapal").
    /// Teks panjang memakai ScrollRect di prefab.
    /// </summary>
    public class NotificationLogView : MonoBehaviour
    {
        [SerializeField] private TMP_Text content;

        private readonly List<NotificationLogEntry> buffer = new List<NotificationLogEntry>(100);
        private readonly StringBuilder builder = new StringBuilder(2048);

        private void OnEnable()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (content == null) return;
            builder.Clear();
            var nm = NotificationManager.Instance;
            var loc = LocalizationManager.Instance;
            if (nm != null) nm.GetLog(buffer);
            else buffer.Clear();

            for (int i = 0; i < buffer.Count; i++)
            {
                var e = buffer[i];
                string day = loc != null ? loc.FormatDay(e.day) : e.day.ToString();
                string time = e.hour < 0
                    ? (loc != null ? loc.Get("ui.log.ship") : "Kapal")
                    : (loc != null ? loc.FormatClock(e.hour) : e.hour.ToString("00") + ":00");
                builder.Append(day).Append(", ").Append(time).Append("  ").Append(e.text).Append('\n');
            }
            content.text = builder.ToString();
        }
    }
}
