using System;
using Characters.Base;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerHandler : Character
    {   
        [Header("References")]
        [SerializeField] private CombatPopup combatPopup;
        [SerializeField] private Renderer playerRenderer; // Reference to the enemy's renderer 

        [Header("Player Stats")]
        public float attackPower = 10f;
        
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
            _originalColor = playerRenderer.material.color; 

            OnDamageEvent += OnDamage;
            OnHealEvent += OnHeal;
        }

        protected override void Start()
        {
            base.Start();
            Debug.Log($"{gameObject.name} has joined the game with {currentHealth} health.");
        } 

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            combatPopup.ShowMessage(damage.ToString()); 
        }
        
        public void Attack(Character target)
        { 
            target.TakeDamage(attackPower);
        }

        protected override void OnDeath()
        { 
        }

        private void OnDamage(float damageAmount)
        { 
            
            playerRenderer.gameObject.transform.DOShakePosition(animationDuration, shakeStrength, shakeVibrato);
 
            playerRenderer.material.DOColor(damageColor, animationDuration / 2)
                .OnComplete(() => playerRenderer.material.DOColor(_originalColor, animationDuration / 2));
            
            
            combatPopup.ShowMessage(damageAmount.ToString());
        }

        private void OnHeal(float healAmount)
        { 
            playerRenderer.material.DOColor(healColor, animationDuration / 2)
                .OnComplete(() => playerRenderer.material.DOColor(_originalColor, animationDuration / 2));
        }
        
    }
}