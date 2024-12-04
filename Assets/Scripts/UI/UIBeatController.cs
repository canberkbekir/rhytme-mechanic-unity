using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

namespace UI
{
    public class UIBeatController : MonoBehaviour
    {
        [SerializeField] private Image beatImage;
         [SerializeField] private float beatFadeTargetAlpha = 0.5f; // Target alpha value for the heartbeat effect

        private BeatManager _beatManager;
        private float _beatInterval;

        private void OnEnable()
        {
            BeatManager.OnBeat += HandleOnBeat;
        }

        private void OnDisable()
        {
            BeatManager.OnBeat -= HandleOnBeat;
        } 

        private void Start()
        {
            _beatManager = GameManager.Instance.BeatManager;
            _beatInterval = _beatManager.SPB; // Seconds Per Beat
        }

        private void HandleOnBeat()
        {
            _beatInterval = GameManager.Instance.BeatManager.SPB;

            // Animate the opacity like a heartbeat
            // 1. Reset to full alpha
            beatImage.DOFade(1f, _beatInterval/2)
                .OnComplete(() =>
                {
                    // 2. Fade back to a lower alpha value to simulate the heartbeat effect
                    beatImage.DOFade(beatFadeTargetAlpha, _beatInterval/2); // Scale duration with beat interval
                });
        }
    }
}