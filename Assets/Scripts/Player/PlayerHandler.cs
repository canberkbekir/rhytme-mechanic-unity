using Characters.Base;
using UnityEngine;

namespace Player
{
    public class PlayerHandler : Character
    {  
        protected override void Start()
        {
            base.Start();
            Debug.Log($"{gameObject.name} has joined the game with {currentHealth} health.");
        } 

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            Debug.Log($"{gameObject.name} is a player and reacts differently to damage!");
        }

        protected override void OnDeath()
        {
            Debug.Log($"{gameObject.name} has died heroically!"); 
        }
    }
}