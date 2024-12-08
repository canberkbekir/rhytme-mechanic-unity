using System;
using System.Globalization;
using DG.Tweening; // Import DoTween
using UnityEngine;
using Characters.Enemies.Base;
using UI;

namespace Characters.Enemies
{
    public class DummyEnemy : Enemy
    {
        [Header("Renderer Settings")]
        [SerializeField] private Renderer enemyRenderer; // Reference to the enemy's renderer
        [SerializeField] private CombatPopup combatPopup; // Reference to the combat popup

        [Header("Animation Settings")]
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private Color healColor = Color.green;
        [Tooltip("Duration of the color change and shake animation.")]
        [SerializeField] private float animationDuration = 0.2f;

        [Header("Shake Settings")]
        [Tooltip("The strength of the shake animation.")]
        [SerializeField] private float shakeStrength = 0.2f;
        [Tooltip("The number of vibrations in the shake animation.")]
        [SerializeField] private int shakeVibrato = 10;

        private Color _originalColor; // Store the original color

        private void Awake()
        { 
            if (enemyRenderer == null)
            {
                Debug.LogError("Enemy Renderer is not assigned!");
                return;
            }

            _originalColor = enemyRenderer.material.color; 
            
            OnDamageEvent += OnDamage;
            OnHealEvent += OnHeal;
        }

        private void OnDamage(float damageAmount)
        { 
            transform.DOShakePosition(animationDuration, shakeStrength, shakeVibrato);
 
            enemyRenderer.material.DOColor(damageColor, animationDuration / 2)
                .OnComplete(() => enemyRenderer.material.DOColor(_originalColor, animationDuration / 2));
            
            
            combatPopup.ShowMessage(damageAmount.ToString());
        }

        private void OnHeal(float healAmount)
        { 
            enemyRenderer.material.DOColor(healColor, animationDuration / 2)
                .OnComplete(() => enemyRenderer.material.DOColor(_originalColor, animationDuration / 2));
        }

        private void OnDestroy()
        {
            // Unsubscribe from events to prevent memory leaks
            OnDamageEvent -= OnDamage;
            OnHealEvent -= OnHeal;
        }
    }
}
