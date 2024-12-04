using System;
using Managers;
using UnityEngine;

public class RhythmInputManager : MonoBehaviour
{
    // Event for input feedback
    public static event Action<string> OnInputFeedback; // Feedback messages: Perfect, Early, Late, Miss

    [SerializeField, Tooltip("Allowed time window (seconds) for early/late input")] 
    private float inputTolerance = 0.1f;

    private float _lastBeatTime;    // Tracks the time of the last beat
    private float _beatInterval;   // Duration between beats
    private bool _hasBeatStarted;  // Flag to check if a beat cycle has started

    private void OnEnable()
    {
        BeatManager.OnBeat += HandleOnBeat;
    }

    private void OnDisable()
    {
        BeatManager.OnBeat -= HandleOnBeat;
    }

    private void Update()
    {
        // Example input detection (Space key for testing)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ValidateInput(Time.time);
        }
    }

    private void HandleOnBeat()
    {
        _lastBeatTime = Time.time;
        _beatInterval = GameManager.Instance.BeatManager.SPB;
        _hasBeatStarted = true;
    }

    private void ValidateInput(float inputTime)
    {
        if (!_hasBeatStarted)
        {
            Debug.Log("Beat hasn't started yet.");
            OnInputFeedback?.Invoke("Miss");
            return;
        }

        float timeDifference = inputTime - _lastBeatTime;

        if (Mathf.Abs(timeDifference) <= inputTolerance)
        {
            // Player input is within the tolerance window
            OnInputFeedback?.Invoke("Perfect");
            Debug.Log("Perfect!");
        }
        else if (timeDifference > inputTolerance && timeDifference <= _beatInterval)
        {
            // Player input was late but within the interval
            OnInputFeedback?.Invoke("Late");
            Debug.Log("Late!");
        }
        else if (timeDifference < -inputTolerance)
        {
            // Player input was early
            OnInputFeedback?.Invoke("Early");
            Debug.Log("Early!");
        }
        else
        {
            // Player input missed completely
            OnInputFeedback?.Invoke("Miss");
            Debug.Log("Miss!");
        }
    }

    public void SetInputTolerance(float tolerance)
    {
        inputTolerance = Mathf.Max(0, tolerance); 
    }
}
