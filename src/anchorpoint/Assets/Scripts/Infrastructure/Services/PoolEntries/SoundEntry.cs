using System;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.PoolEntries
{
    /// <summary>
    /// A pooled sound that returns itself when it finishes.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public sealed class SoundEntry : MonoBehaviour
    {
        [SerializeField]
        private AudioSource source;

        /// <summary>
        /// True once the sound has stopped.
        /// </summary>
        public bool Finished { get; private set; }

        private Action onFinished;
        private Action onNaturalFinish;

        /// <summary>
        /// Sets the release callback.
        /// </summary>
        public void Initialize(Action onFinished)
        {
            this.onFinished = onFinished;

            if (source == null)
            {
                source = GetComponent<AudioSource>();
            }
        }

        /// <summary>
        /// Plays the effect; onNaturalFinish fires on natural end, not on Stop.
        /// </summary>
        public void Play(SoundEffect effect, Action onNaturalFinish = null)
        {
            this.onNaturalFinish = onNaturalFinish;
            Finished = false;

            if (effect.Clip == null)
            {
                Finish();
                return;
            }

            source.clip = effect.Clip;
            source.pitch = effect.RandomPitch;
            source.volume = effect.RandomVolume;
            source.Play();

            CancelInvoke();

            float pitch = Mathf.Abs(source.pitch);
            float duration = pitch > 0.01f ? effect.Clip.length / pitch : effect.Clip.length;

            Invoke(nameof(Finish), duration);
        }

        /// <summary>
        /// Stops the sound early and returns it to the pool.
        /// </summary>
        public void Stop()
        {
            if (Finished)
            {
                return;
            }

            CancelInvoke();
            Finished = true;
            onNaturalFinish = null;
            source.Stop();
            onFinished?.Invoke();
        }

        private void Finish()
        {
            Finished = true;
            var cb = onNaturalFinish;
            onNaturalFinish = null;
            cb?.Invoke();
            onFinished?.Invoke();
        }
    }
}
