using Characters.Base;
using UnityEngine;

namespace Player
{
    public class PlayerHandler : Character
    {  
        public float attackPower = 10f;
        
        protected override void Start()
        {
            base.Start();
            Debug.Log($"{gameObject.name} has joined the game with {currentHealth} health.");
        } 

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            Debug.Log($"{gameObject.name} is a player and reacts differently to damage!");
        }
        
        public void Attack(Character target)
        {
            Debug.Log($"{gameObject.name} attacks {target.gameObject.name} for {attackPower} damage!");
            target.TakeDamage(attackPower);
        }

        protected override void OnDeath()
        {
            Debug.Log($"{gameObject.name} has died heroically!"); 
        }
    }
}