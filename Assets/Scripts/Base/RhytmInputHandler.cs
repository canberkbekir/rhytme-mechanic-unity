using System;
using Managers;
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
        // Event for input feedback
        public event Action<InputFeedbackType> OnInputFeedback; // Feedback messages: Perfect, Early, Late, Miss

        [SerializeField, Tooltip("Allowed time window (seconds) for early/late input")] 
        private float inputTolerance = 0.1f;
        
        private BeatManager _beatManager;
        private MovementHandler _movementHandler;
        

        private float _lastBeatTime;    // Tracks the time of the last beat
        private float _nextBeatTime;    // Tracks the time of the next beat
        private float _beatInterval;   // Duration between beats
        private bool _hasBeatStarted;  // Flag to check if a beat cycle has started

        private void OnEnable()
        {
            _beatManager.OnBeat += HandleOnBeat;
        }

        private void OnDisable()
        {
            _beatManager.OnBeat -= HandleOnBeat;
        }
        
        private void Awake()
        {
            _beatManager = GameManager.Instance.BeatManager;
            _movementHandler = GameManager.Instance.Player.MovementHandler;
        }

        private void Update()
        {
            // Example input detection (Space key for testing)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ValidateInput(Time.time);
            }else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("Right Arrow");
                _movementHandler.MoveRight();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _movementHandler.MoveLeft();
            }
            
                
        }

        private void HandleOnBeat()
        {
            _beatInterval = GameManager.Instance.BeatManager.SPB; 
            _lastBeatTime = Time.time;
            _nextBeatTime = _lastBeatTime + _beatInterval;
            _hasBeatStarted = true;
        }

        private void ValidateInput(float inputTime)
        {
            if (!_hasBeatStarted)
            { 
                OnInputFeedback?.Invoke(InputFeedbackType.Miss);
                return;
            }

            var timeDifference = _nextBeatTime - inputTime;

            if (Mathf.Abs(timeDifference) <= inputTolerance)
            {
                // Player input is within the tolerance window
                OnInputFeedback?.Invoke(InputFeedbackType.Perfect);
            } 
            else
            {
                // Player input missed completely
                OnInputFeedback?.Invoke(InputFeedbackType.Miss);
            }
        }


        public void SetInputTolerance(float tolerance)
        {
            inputTolerance = Mathf.Max(0, tolerance); 
        }
    }
}
