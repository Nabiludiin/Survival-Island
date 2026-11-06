using System;
using UnityEngine;

namespace SurvivalIsland
{
    /// <summary>
    /// Penanda struct event yang menyimpan nilai terakhir (event "kondisi saat ini").
    /// Blueprint: Core_Logic/GameEventBus.md, bagian "Pendengar yang terlambat aktif" (#107).
    /// </summary>
    [AttributeUsage(AttributeTargets.Struct, Inherited = false)]
    public sealed class StickyEventAttribute : Attribute { }

    /// <summary>
    /// Jalur event statis generik, satu channel per tipe struct.
    /// Blueprint: Core_Logic/GameEventBus.md (#107). Usulan arsitektur, bukan aturan GDD.
    ///
    /// Aturan pakai (GameEventBus.md bagian 2):
    /// - Berlangganan di OnEnable, lepas di OnDisable. Jangan di Awake atau Start.
    /// - Jangan memakai lambda yang menangkap variabel sebagai pendengar.
    /// - Isi event selalu struct kecil, supaya Publish tidak menghasilkan garbage.
    /// </summary>
    public static class EventChannel<T> where T : struct
    {
        /// <summary>Pendengar channel ini.</summary>
        public static event Action<T> Raised;

        /// <summary>Ada nilai terakhir yang tersimpan (hanya untuk struct bertanda [StickyEvent]).</summary>
        public static bool HasLast { get; private set; }

        /// <summary>Nilai terakhir yang terkirim (hanya untuk struct bertanda [StickyEvent]).</summary>
        public static T Last { get; private set; }

        private static readonly bool IsSticky;
        private static readonly string ChannelName;

        static EventChannel()
        {
            IsSticky = typeof(T).IsDefined(typeof(StickyEventAttribute), false);
            ChannelName = typeof(T).Name;
            // GameEventRegistry mencatat fungsi Clear tiap channel saat channel pertama kali dipakai.
            GameEventRegistry.RegisterChannel(ChannelName, Clear, CountListeners);
        }

        /// <summary>Kirim event ke semua pendengar. Event sticky menyimpan nilainya di Last lebih dulu.</summary>
        public static void Publish(in T data)
        {
            if (IsSticky)
            {
                Last = data;
                HasLast = true;
            }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            // Perintah eventlog di DevConsole (#56) memasang LogHook. Hanya ada di build pengembangan.
            GameEventRegistry.LogHook?.Invoke(ChannelName);
#endif

            var handlers = Raised;
            if (handlers == null) return;

#if UNITY_EDITOR
            // GameEventBus.md bagian 4: di Editor, exception ditangkap per pendengar supaya pendengar lain tetap jalan.
            // GetInvocationList mengalokasikan array, jadi hanya dipakai di Editor.
            var list = handlers.GetInvocationList();
            for (int i = 0; i < list.Length; i++)
            {
                try
                {
                    ((Action<T>)list[i]).Invoke(data);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
#else
            handlers.Invoke(data);
#endif
        }

        /// <summary>Hapus semua pendengar dan nilai sticky. Dipanggil lewat GameEventRegistry.ClearAll().</summary>
        public static void Clear()
        {
            Raised = null;
            HasLast = false;
            Last = default;
        }

        private static int CountListeners()
        {
            var handlers = Raised;
            return handlers == null ? 0 : handlers.GetInvocationList().Length;
        }
    }
}
