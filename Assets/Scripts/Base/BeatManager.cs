using System;
using Beats.Base;
using UnityEngine;

namespace Base
{
    public class BeatManager : MonoBehaviour
    {
        // Events for beat synchronization
        public event Action OnBeat;             // Fired on every beat
        public event Action<int> OnBeatCount;   // Includes the beat count since the start

        [Header("Beat Manager Settings")]
        [SerializeField, Tooltip("The Song Map to process beats from")]
        private SongMap songMap;

        private float _currentSPB;       // Seconds per beat
        private float _beatTimer;        // Accumulated time for the current beat
        private int _beatCount;          // Total beats in current BeatMap
        private int _currentBeatMapIndex;
        private bool _isRunning;         // Tracks whether the beat manager is active


        #region Public Properties
        public float SPB => _currentSPB; 
        public int BeatCount => _beatCount; 
        
        #endregion
        private void Start()
        {
            if (!ValidateSongMap()) return;

            Initialize();
            Debug.Log("BeatManager initialized successfully.");
        }

        private void Update()
        {
            if (!_isRunning) return;

            UpdateBeatTimer();
        }

        #region Initialization and Validation

        private bool ValidateSongMap()
        {
            if (songMap == null || songMap.beats.Length == 0)
            {
                Debug.LogError("No SongMap assigned or it has no BeatMaps!");
                return false;
            }
            return true;
        }

        private void Initialize()
        {
            _isRunning = true;
            _currentBeatMapIndex = 0;
            LoadCurrentBeatMap();
        }

        #endregion

        #region Timer and Beat Updates

        private void UpdateBeatTimer()
        {
            _beatTimer += Time.deltaTime;

            while (_beatTimer >= _currentSPB)
            {
                _beatTimer -= _currentSPB;
                TriggerBeat();

                if (_beatCount >= GetCurrentBeatMap().BeatCount)
                {
                    LoadNextBeatMap();
                }
            }
        }

        private void TriggerBeat()
        {
            _beatCount++;
            OnBeat?.Invoke();
            OnBeatCount?.Invoke(_beatCount);

            Debug.Log($"Beat {_beatCount}");
        }

        #endregion

        #region BeatMap Handling

        private void LoadCurrentBeatMap()
        {
            var beatMap = GetCurrentBeatMap();
            _currentSPB = beatMap.BeatInterval;
            _beatCount = 0;
            _beatTimer = 0;

            Debug.Log($"Loaded BeatMap {_currentBeatMapIndex + 1}: {beatMap.BeatCount} beats at {_currentSPB} seconds per beat");
        }

        private void LoadNextBeatMap()
        {
            _currentBeatMapIndex++;

            if (_currentBeatMapIndex < songMap.beats.Length)
            {
                LoadCurrentBeatMap();
            }
            else
            {
                FinishSong();
            }
        }

        private BeatMap GetCurrentBeatMap() => songMap.beats[_currentBeatMapIndex];

        private void FinishSong()
        {
            _isRunning = false;
            Debug.Log("Song finished!");
        }

        #endregion

        #region Public Controls

        public void PauseBeats()
        {
            _isRunning = false;
            Debug.Log("Beats paused.");
        }

        public void ResumeBeats()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                Debug.Log("Beats resumed.");
            }
        }

        public void ResetBeats()
        {
            Initialize();
            Debug.Log("Beats reset.");
        }

        #endregion
    }
}
