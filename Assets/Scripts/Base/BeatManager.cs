using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Base
{
    public class BeatManager : MonoBehaviour
    {
        // Events for beat synchronization
        public  event Action OnBeat;              // Fired on every beat
        public  event Action<int> OnBeatCount;    // Includes the beat count since the start

        // Beat timing configuration
        [FormerlySerializedAs("beatInterval")] [SerializeField, Tooltip("Time in seconds between beats")] 
        private float spb = 0.5f;

        [SerializeField, Tooltip("Optional: AudioSource for beat synchronization")] 
        private AudioSource audioSource;

        private float _beatTimer;        // Tracks time since the last beat
        private int _beatCount;          // Total number of beats since start
        private bool _isRunning = true;  // Allows pausing the beat manager


        #region Public Properties
        /// <summary>
        ///  Seconds per beat (SPB) - the time interval between beats.
        /// </summary>
        public float SPB
        {
            get => spb;
            set
            {
                if (!(value > 0)) return;
                spb = value;
                SyncToAudio(); // Re-sync if interval changes
            }
        } 
    
        /// <summary>
        ///  Beats per minute (BPM) - the number of beats in a minute.
        /// </summary>
        public float BPM => 60 / SPB; 

        public bool IsRunning => _isRunning;

        #endregion 

        private void Start()
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                SyncToAudio();
            } 
            OnBeat?.Invoke(); 
        }

        private void Update()
        {
            if (!_isRunning) return;

            UpdateBeatTimer(Time.deltaTime);
        }

        private void UpdateBeatTimer(float deltaTime)
        {
            _beatTimer += deltaTime;

            while (_beatTimer >= spb)
            {
                _beatTimer -= spb;
                TriggerBeat();
            }
        }

        private void TriggerBeat()
        {
            _beatCount++; 
            OnBeat?.Invoke();
            OnBeatCount?.Invoke(_beatCount);
        }

        private void SyncToAudio()
        {
            if (audioSource != null)
            {
                _beatTimer = audioSource.time % spb;
            }
        }

        public void PauseBeats()
        {
            _isRunning = false;
        }

        public void ResumeBeats()
        {
            _isRunning = true;
        }

        public void ResetBeats()
        {
            _beatTimer = 0;
            _beatCount = 0;
            Debug.Log("Beats reset");
        }

        public void SyncToAudioSource(AudioSource newAudioSource)
        {
            audioSource = newAudioSource;
            SyncToAudio();
        }
    }
}
