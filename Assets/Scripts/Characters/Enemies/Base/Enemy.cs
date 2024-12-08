using Characters.Base;
using UnityEngine;

namespace Characters.Enemies.Base
{
    public abstract class Enemy : Character
    {
        public int attackPower;
        public int occupiedPositionCount = 1;

        protected override void Start()
        {
            base.Start();
            Debug.Log($"{gameObject.name} has spawned with {currentHealth} health.");
        }

        public void Attack(Character target)
        {
            Debug.Log($"{gameObject.name} attacks {target.gameObject.name} for {attackPower} damage!");
            target.TakeDamage(attackPower);
        }

        protected override void OnDeath()
        {
            Debug.Log($"{gameObject.name} has been defeated!");
            Destroy(gameObject); // Destroy the enemy GameObject on death
        }

        public override void Heal(float amount)
        {
            Debug.Log($"{gameObject.name} cannot heal!");
        }
    }
}