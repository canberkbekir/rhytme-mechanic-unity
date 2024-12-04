using System;
using System.Collections;
using Base;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UITemporaryBeatText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        private RhythmInputHandler _rhythmInputHandler;
        private BeatManager _beatManager;

        private void Awake()
        {
            _rhythmInputHandler = GameManager.Instance.RhythmInputHandler;
            _beatManager = GameManager.Instance.BeatManager;
            
            // Subscribe to the event
            _rhythmInputHandler.OnInputFeedback += HandleOnInputFeedback;
            
        } 

        private void HandleOnInputFeedback(InputFeedbackType obj)
        {
            
            StartCoroutine(ShowTemporaryText(obj));
            
        }

        private IEnumerator ShowTemporaryText(InputFeedbackType inputFeedbackType)
        {
            _text.gameObject.SetActive(true);
            _text.color = inputFeedbackType switch
            {
                InputFeedbackType.Perfect => Color.green, 
                InputFeedbackType.Miss => Color.red,
                _ => throw new ArgumentOutOfRangeException(nameof(inputFeedbackType), inputFeedbackType, null)
            };
            _text.text = inputFeedbackType.ToString() ; 
            yield return  new WaitForSeconds(0.4f);
            _text.gameObject.SetActive(false);
        } 
    }
}
