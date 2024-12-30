using System;
using Beats.Base;
using UnityEngine;

namespace Base
{
    public class BeatManager : MonoBehaviour
    {
        public event Action OnMusicStarted; 
        public event Action OnMusicStopped; 
        public event Action OnBeat;         
        public event Action<int> OnBeatCount; 

        [Header("Beat Manager Settings")]
        [SerializeField, Tooltip("The Song Map to process beats from")]
        private SongMap songMap;

        private AudioSource _audioSource;
        private float _currentSPB;
        private float _beatTimer;
        private int _beatCount;
        private int _currentBeatMapIndex;
        private bool _isRunning;

        public float SPB => _currentSPB;
        public int BeatCount => _beatCount;
        public float LastBeatTime { get; private set; }
        public float NextBeatTime => LastBeatTime + _currentSPB;

        private void Start()
        {
            InitializeAudioSource();
            if (!ValidateSongMap()) return;

            Initialize();
            SetMusicState(true);
        }

        private void Update()
        {
            if (!_isRunning) return;
            UpdateBeatTimer();
        }

        private void InitializeAudioSource()
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.clip = songMap?.song;
            _audioSource.playOnAwake = false;
        }

        private bool ValidateSongMap()
        {
            if (songMap == null || songMap.beats.Length == 0)
            {
                Debug.LogError("No SongMap assigned or it has no BeatMaps!");
                return false;
            }
            if (songMap.song == null)
            {
                Debug.LogError("SongMap has no audio clip assigned!");
                return false;
            }
            return true;
        }

        private void Initialize()
        {
            _isRunning = true;
            _currentBeatMapIndex = 0;
            LoadCurrentBeatMap();
            _beatTimer = 0;
        }

        private void SetMusicState(bool isRunning)
        {
            _isRunning = isRunning;

            if (isRunning)
            {
                _audioSource.Play();
                OnMusicStarted?.Invoke();
                Debug.Log("Music started.");
            }
            else
            {
                _audioSource.Stop();
                OnMusicStopped?.Invoke();
                Debug.Log("Music stopped.");
            }
        }

        private void UpdateBeatTimer()
        {
            _beatTimer += Time.deltaTime;

            while (_beatTimer >= _currentSPB)
            {
                _beatTimer -= _currentSPB;
                TriggerBeat();
                if (_beatCount >= GetCurrentBeatMap().BeatCount) LoadNextBeatMap();
            }
        }

        private void TriggerBeat()
        {
            LastBeatTime = _audioSource.time;
            _beatCount++;
            OnBeat?.Invoke();
            OnBeatCount?.Invoke(_beatCount);
            Debug.Log($"Beat {_beatCount}");
        }

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
            if (++_currentBeatMapIndex < songMap.beats.Length)
            {
                LoadCurrentBeatMap();
            }
            else
            {
                SetMusicState(false);
                Debug.Log("Song and beats finished!");
            }
        }

        private BeatMap GetCurrentBeatMap() => songMap.beats[_currentBeatMapIndex];
    }
}
