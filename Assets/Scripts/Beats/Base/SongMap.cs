using System.Linq;
using UnityEngine;

namespace Beats.Base
{
    [CreateAssetMenu(fileName = "SongMap", menuName = "Song Map")]
    public class SongMap : ScriptableObject
    {
        [Header("Song Settings")]
        public string songName;
        public AudioClip song;

        [Header("Beat Maps")]
        public BeatMap[] beats;

        public float SongLength => beats.Length == 0 ? 0 : beats.Sum(beat => beat.BeatCount * beat.BeatInterval);
    }
}