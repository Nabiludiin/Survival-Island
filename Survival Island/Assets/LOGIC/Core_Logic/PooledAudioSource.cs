using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace SurvivalIsland
{
    /// <summary>
    /// Suara one-shot 3D dari pool (tag pool "Audio_OneShot3D"). Dipakai AmbientAudio (#52) dan CombatMusicDirector (#114).
    /// Blueprint: Core_Logic/AmbientAudio.md bagian 4 "Pakai ulang dari pool", GLOSSARY.md (PooledAudioSource).
    /// Usulan teknis, belum ada di GDD.
    ///
    /// Kembali ke pool sendiri setelah klip selesai, kecuali saat AudioListener.pause aktif (jeda #83).
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class PooledAudioSource : MonoBehaviour, IPoolable
    {
        public const string PoolTag = "Audio_OneShot3D";

        /// <summary>Sumber yang sedang keluar dari pool. Dipakai StopAll saat run berakhir.</summary>
        private static readonly List<PooledAudioSource> active = new List<PooledAudioSource>(16);

        private AudioSource source;
        private bool started;
        private int activeIndex = -1;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 1f;
        }

        /// <summary>Ambil satu sumber dari pool lalu putar klip di posisi itu. Mengembalikan null kalau pool tidak ada atau penuh.</summary>
        public static PooledAudioSource PlayAt(AudioClip clip, Vector3 position, AudioMixerGroup group, float volume = 1f)
        {
            if (clip == null || ObjectPooler.Instance == null || !ObjectPooler.Instance.HasPool(PoolTag)) return null;
            var s = ObjectPooler.Instance.SpawnFromPool<PooledAudioSource>(PoolTag, position, Quaternion.identity);
            if (s != null) s.Play(clip, group, volume);
            return s;
        }

        public void Play(AudioClip clip, AudioMixerGroup group, float volume = 1f)
        {
            if (clip == null) return;
            source.clip = clip;
            source.outputAudioMixerGroup = group;
            source.volume = Mathf.Clamp01(volume);
            source.Play();
            started = true;
        }

        public void OnSpawned()
        {
            if (source == null) source = GetComponent<AudioSource>();
            // AmbientAudio.md bagian 4: klip, pitch, volume, dan grup mixer diatur ulang.
            source.clip = null;
            source.pitch = 1f;
            source.volume = 1f;
            source.outputAudioMixerGroup = null;
            started = false;
            if (activeIndex < 0)
            {
                activeIndex = active.Count;
                active.Add(this);
            }
        }

        public void OnReturned()
        {
            if (source != null)
            {
                source.Stop();
                source.clip = null;
            }
            started = false;
            RemoveFromActive();
        }

        private void RemoveFromActive()
        {
            if (activeIndex < 0 || activeIndex >= active.Count || active[activeIndex] != this) { activeIndex = -1; return; }
            int last = active.Count - 1;
            var moved = active[last];
            active[activeIndex] = moved;
            moved.activeIndex = activeIndex;
            active.RemoveAt(last);
            activeIndex = -1;
        }

        private void Update()
        {
            // Saat jeda klip ditahan (isPlaying false), jadi jangan dikembalikan.
            if (AudioListener.pause) return;
            if (started && source.isPlaying) return;
            ReturnSelf();
        }

        private void ReturnSelf()
        {
            if (ObjectPooler.Instance != null) ObjectPooler.Instance.ReturnToPool(gameObject);
            else gameObject.SetActive(false);
        }

        /// <summary>Kembalikan semua sumber aktif ke pool. Dipanggil AudioManager.StopWorldAudio.</summary>
        public static void StopAll()
        {
            for (int i = active.Count - 1; i >= 0; i--)
            {
                if (i >= active.Count) continue;
                var s = active[i];
                if (s != null) s.ReturnSelf();
                else active.RemoveAt(i);
            }
        }
    }
}
