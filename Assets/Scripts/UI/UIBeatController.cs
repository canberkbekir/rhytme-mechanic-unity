using Base;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class UIBeatController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image beatImage;
        [SerializeField] private BeatPoolManager beatPoolManager;
        [SerializeField] private RectTransform leftArrowParent;
        [SerializeField] private RectTransform rightArrowParent;
        [SerializeField] private RectTransform centerTransform;

        [Header("Arrow Settings")]
        [SerializeField] private int arrowCount = 5;
        [SerializeField] private float arrowSpacing = 20f;
        [SerializeField] private float arrowMergeDistance = 50f;

        [Header("Animation Settings")]
        [SerializeField] private float beatScaleUp = 1.2f;
        [SerializeField] private Ease easeType = Ease.Linear;
        [SerializeField] private bool animateCenter = true;
        [SerializeField] private bool animateArrows = false;

        private BeatManager _beatManager;
        private float _beatInterval;
        private int _currentArrowIndex = 0;

        private void OnEnable()
        {
            _beatManager.OnBeat += HandleOnBeat;
            _beatManager.OnBeatCount += UpdateSPB;
        }

        private void OnDisable()
        {
            _beatManager.OnBeat -= HandleOnBeat;
            _beatManager.OnBeatCount -= UpdateSPB;
        }

        private void Awake()
        {
            _beatManager = GameManager.Instance.BeatManager;
            _beatInterval = _beatManager.SPB; 
        }

        private void HandleOnBeat()
        {
            AnimateHeart();
            SpawnAndAnimateArrows();
        }

        private void AnimateHeart()
        {
            if (!animateCenter) return; 
            
            beatImage.rectTransform.DOScale(1, _beatInterval / 2).SetEase(easeType)
                .OnComplete(() =>
                {
                    beatImage.rectTransform.DOScale(beatScaleUp, _beatInterval / 2);
                });
        }

        private void SpawnAndAnimateArrows()
        {
            if (!animateArrows) return;
            AnimateArrow(CreateArrow(leftArrowParent, -_currentArrowIndex * arrowSpacing), true);
            AnimateArrow(CreateArrow(rightArrowParent, _currentArrowIndex * arrowSpacing), false);
            _currentArrowIndex++;
        }

        private RectTransform CreateArrow(RectTransform parent, float offset)
        {
            var arrow = beatPoolManager.GetArrow();
            arrow.SetParent(parent, false);
            arrow.anchoredPosition = new Vector2(offset, 0);
            return arrow;
        }

        private void AnimateArrow(RectTransform arrow, bool isLeft)
        {
            Vector3 direction = isLeft ? Vector3.left : Vector3.right;
            Vector3 targetPosition = centerTransform.position + (direction * arrowMergeDistance);

            arrow.DOMove(targetPosition, _beatInterval / 2)
                .SetEase(easeType)
                .OnComplete(() => beatPoolManager.ReturnArrow(arrow));
        } 
        private void UpdateSPB(int beatCount)
        {
            _beatInterval = _beatManager.SPB;
        }
    }
}
