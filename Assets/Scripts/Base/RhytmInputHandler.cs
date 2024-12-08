using System;
using Managers;
using Player;
using UnityEngine;

namespace Base
{
    public enum InputFeedbackType
    {
        Perfect,
        Miss
    }

    public class RhythmInputHandler : MonoBehaviour
    {
        // Public event for input feedback
        public event Action<InputFeedbackType> OnInputFeedback;

        [SerializeField, Tooltip("Allowed time window (percentage of beat interval) for early/late input")] 
        private float inputTolerancePercentage = 0.1f;

        private BeatManager _beatManager;
        private MovementHandler _movementHandler;
        private PlayerHandler _playerHandler;
        private CombatManager _combatManager;
        
        private float _nextBeatTime;    // Time of the next beat
        private float _beatInterval;   // Duration between beats
        private bool _inputProcessed;  // Flag to track if input is already processed for the current beat

        private void OnEnable()
        {
            if (_beatManager != null)
                _beatManager.OnBeat += HandleOnBeat;
        }

        private void OnDisable()
        {
            if (_beatManager != null)
                _beatManager.OnBeat -= HandleOnBeat;
        }

        private void Awake()
        {
            _beatManager = GameManager.Instance?.BeatManager;
            _movementHandler = GameManager.Instance?.Player?.MovementHandler;
            _playerHandler = GameManager.Instance?.Player?.PlayerHandler;
            _combatManager = GameManager.Instance?.CombatManager;

            if (_beatManager == null || _movementHandler == null)
            {
                Debug.LogError("Required components are missing. Please ensure GameManager is properly set up.");
            }
        }

        private void Update()
        {
            if (_inputProcessed) return;

            HandlePlayerInput();
        }

        private void HandleOnBeat()
        {
            _beatInterval = _beatManager.SPB; // Seconds per beat
            _nextBeatTime = Time.time + _beatInterval;
            _inputProcessed = false; // Allow input for the new beat
        }

        private void HandlePlayerInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ProcessInput(Time.time);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (ProcessInput(Time.time))
                {
                    _movementHandler.MoveRight();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (ProcessInput(Time.time))
                {
                    _movementHandler.MoveLeft();
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (ProcessInput(Time.time))
                {
                    _playerHandler.Attack(_combatManager.GetCurrentEnemy());
                    _movementHandler.MoveAttack();
                }
            }
        }

        private bool ProcessInput(float inputTime)
        {
            _inputProcessed = true; // Block further input for this beat

            if (IsInputInTolerance(inputTime))
            {
                OnInputFeedback?.Invoke(InputFeedbackType.Perfect);
                return true;
            }

            OnInputFeedback?.Invoke(InputFeedbackType.Miss);
            return false;
        }

        private bool IsInputInTolerance(float inputTime)
        {
            if (_beatInterval <= 0) return false;

            var timeDifference = Mathf.Abs(_nextBeatTime - inputTime);
            return timeDifference <= inputTolerancePercentage * _beatInterval;
        }

        public void SetInputTolerance(float tolerance)
        {
            inputTolerancePercentage = Mathf.Max(0, tolerance);
        }
    }
}
