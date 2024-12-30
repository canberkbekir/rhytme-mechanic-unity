using Base;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class UIBeatController : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image beatImage;

        [Header("Beat Sync Settings")]
        [SerializeField, Range(0f, 0.2f)] private float minOpacity = 0.0f; // Minimum opacity
        [SerializeField, Range(0f, 0.5f)] private float maxOpacity = 0.5f;   // Maximum opacity
        [SerializeField] private Ease fadeEase = Ease.InOutSine;        // Easing type for animation

        private BeatManager _beatManager;
        private Tween _fadeTween;

        private void Awake()
        {
            _beatManager = GameManager.Instance?.BeatManager;

            if (_beatManager == null)
            {
                Debug.LogError("Required components are missing. Please ensure GameManager is properly set up.");
                return;
            }

            if (beatImage == null)
            {
                Debug.LogError("No Image component assigned to the Beat Image field!");
                return;
            }

            // Subscribe to music events
            _beatManager.OnMusicStarted += StartFading;
            _beatManager.OnMusicStopped += StopFading;
        }

        private void OnDestroy()
        {
            if (_beatManager != null)
            {
                _beatManager.OnMusicStarted -= StartFading;
                _beatManager.OnMusicStopped -= StopFading;
            }

            _fadeTween?.Kill();
        }

        private void StartFading()
        {
            // Start continuous fade animation
            _fadeTween = beatImage.DOFade(minOpacity, _beatManager.SPB / 2)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(fadeEase);
 
        }

        private void StopFading()
        {
            // Stop fade animation and reset opacity
            _fadeTween?.Kill();
            beatImage.color = new Color(beatImage.color.r, beatImage.color.g, beatImage.color.b, maxOpacity);

            Debug.Log("Fading stopped with music.");
        }
    }
}
