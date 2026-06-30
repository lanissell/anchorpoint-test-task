using System;
using UnityEngine;

namespace Anchorpoint.Infrastructure.Services.PoolEntries
{
    /// <summary>
    /// A clip with random pitch and volume so it sounds varied.
    /// </summary>
    [Serializable]
    public struct SoundEffect
    {
        /// <summary>
        /// The clip to play.
        /// </summary>
        [field: SerializeField]
        public AudioClip Clip { get; private set; }

        [SerializeField]
        private Vector2 pitch;

        [SerializeField]
        private Vector2 volume;

        /// <summary>
        /// A random pitch in range, or 1 if unset.
        /// </summary>
        public float RandomPitch => pitch == Vector2.zero ? 1f : UnityEngine.Random.Range(pitch.x, pitch.y);

        /// <summary>
        /// A random volume in range, or 1 if unset.
        /// </summary>
        public float RandomVolume => volume == Vector2.zero ? 1f : UnityEngine.Random.Range(volume.x, volume.y);
    }
}
