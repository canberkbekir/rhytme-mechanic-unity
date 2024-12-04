using System;
using Base;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

namespace UI
{
    public class UIBeatController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image beatImage;
        
        [Header("Animation Settings")]
        [SerializeField] private float beatScaleUp = 1.2f;  
        [SerializeField] private Ease easeType = Ease.Linear;

        private BeatManager _beatManager;
        private float _beatInterval;

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
            _beatInterval = _beatManager.SPB; // Seconds Per Beat
            
            beatImage.rectTransform.localScale = Vector3.one * beatScaleUp;

        }

        private void HandleOnBeat()
        {
            _beatInterval = _beatManager.SPB; 
 
            beatImage.rectTransform.DOScale(1, _beatInterval / 2).SetEase(easeType)
                .OnComplete(() =>
                {
                    beatImage.rectTransform.DOScale(beatScaleUp, _beatInterval / 2);
                });
        } 

    }
}