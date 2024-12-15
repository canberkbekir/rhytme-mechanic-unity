using Characters.Enemies.AI.Base;
using UnityEngine;

namespace Characters.Enemies.AI
{
    public class DefaultEnemyBehavior : EnemyBehaviour
    {
        protected override void OnAttackComplete()
        {
            base.OnAttackComplete();
            HandlePreAttackStyle();
        }

        private void HandlePreAttackStyle()
        { 
            AttackStyle = DetermineNextAttackStyle();
            Debug.Log($"Pre-processed AttackStyle: {AttackStyle}");
        }

        private AttackStyle DetermineNextAttackStyle()
        { 
            return (AttackStyle)Random.Range(0, System.Enum.GetValues(typeof(AttackStyle)).Length);
        }
    }
}