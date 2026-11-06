using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Catatan semua EventChannel yang pernah dipakai, untuk reset run dan pemeriksaan kebocoran pendengar.
    /// Blueprint: Core_Logic/GameEventBus.md (#107) dan Core_Logic/RunLifecycle.md langkah 7-8 reset (#82).
    /// </summary>
    public static class GameEventRegistry
    {
        private struct ChannelEntry
        {
            public string name;
            public Action clear;
            public Func<int> count;
        }

        private static readonly List<ChannelEntry> Channels = new List<ChannelEntry>(128);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        /// <summary>
        /// Dipasang DevConsole (#56) untuk perintah eventlog. Menerima nama channel setiap kali event terkirim.
        /// Nilai awal null. Hanya ada di build pengembangan.
        /// </summary>
        public static Action<string> LogHook;
#endif

        /// <summary>Dipanggil konstruktor statis EventChannel&lt;T&gt; saat channel pertama kali dipakai.</summary>
        internal static void RegisterChannel(string name, Action clear, Func<int> count)
        {
            Channels.Add(new ChannelEntry { name = name, clear = clear, count = count });
        }

        /// <summary>Total pendengar seluruh channel (GameEventBus.md, untuk pemeriksaan kebocoran #82).</summary>
        public static int ListenerCount
        {
            get
            {
                int total = 0;
                for (int i = 0; i < Channels.Count; i++) total += Channels[i].count();
                return total;
            }
        }

        /// <summary>
        /// Hapus semua pendengar dan nilai sticky di semua channel.
        /// Dipanggil RunLifecycle (#82) saat reset, diapit managersRoot.SetActive(false/true).
        /// </summary>
        public static void ClearAll()
        {
            for (int i = 0; i < Channels.Count; i++) Channels[i].clear();
        }

        /// <summary>
        /// Catat tiap channel yang masih punya pendengar sebagai peringatan kebocoran.
        /// RunLifecycle.md langkah 7 reset: pada titik itu jumlahnya seharusnya 0.
        /// </summary>
        public static void LogRemainingListeners()
        {
            for (int i = 0; i < Channels.Count; i++)
            {
                int count = Channels[i].count();
                if (count > 0)
                    Debug.LogWarning($"[GameEventRegistry] Kemungkinan kebocoran: {Channels[i].name} masih punya {count} pendengar saat reset.");
            }
        }

        /// <summary>
        /// GameEventBus.md bagian 4: kalau Domain Reload dimatikan di Editor, langganan lama tidak boleh terbawa ke Play berikutnya.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetOnEnterPlayMode()
        {
            ClearAll();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            LogHook = null;
#endif
        }
    }
}
