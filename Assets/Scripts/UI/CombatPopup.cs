using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class CombatPopup : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float moveUpDistance = 1f; // Distance the popup moves upward
        [SerializeField] private float animationDuration = 1f; // Duration of the animation
        [SerializeField] private float fadeOutDuration = 0.5f; // Duration of the fade-out effect

        [Header("References")]
        [SerializeField] private TextMeshPro textMesh; // Text component to display the message
        [SerializeField] private Vector3 initialOffset = Vector3.zero; // Offset position from the start

        private Vector3 _startingPosition; // Stores the popup's starting position
        private Color _originalColor;

        private void Awake()
        {
            if (textMesh == null)
            {
                Debug.LogError("TextMeshPro reference is missing!");
                return;
            }

            _originalColor = textMesh.color;

            // Store the initial starting position plus the offset
            _startingPosition = transform.localPosition + initialOffset;

            // Apply the offset at the beginning
            transform.localPosition = _startingPosition;
        }

        /// <summary>
        /// Sets up the popup message with a value and starts the animation.
        /// </summary>
        /// <param name="value">The damage or value to display.</param>
        /// <param name="color">The color of the text (optional).</param>
        public void ShowMessage(string value, Color? color = null)
        {
            // Activate the popup object
            gameObject.SetActive(true);

            // Reset the position to the initial offset
            transform.localPosition = _startingPosition;

            // Set the text and color
            textMesh.text = value;
            textMesh.color = color ?? _originalColor;

            // Animate the popup
            AnimatePopup();
        }

        private void AnimatePopup()
        {
            // Move the text upward from its current position
            transform.DOLocalMoveY(transform.localPosition.y + moveUpDistance, animationDuration).SetEase(Ease.OutQuad);

            // Fade out the text
            textMesh.DOFade(0, fadeOutDuration).SetDelay(animationDuration - fadeOutDuration)
                .OnComplete(() =>
                {
                    // Reset the text to empty after the animation ends
                    textMesh.text = string.Empty;

                    // Reset the color to its original alpha
                    textMesh.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1);
                });
        }
    }
}
