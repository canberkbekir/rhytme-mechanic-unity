using System;
using Base;
using Characters.Enemies.Base;
using Combats.Base;
using UnityEngine;

namespace Managers
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private Enemy[] enemies;
        [SerializeField] private CombatPosition[] combatPositions;
        
        //private variables
        private MovementHandler _movementHandler;
        
        private void Awake()
        {
            _movementHandler = GameManager.Instance.Player.MovementHandler;
            _movementHandler.OnWaypointChanged += OnPlayerWaypointChanged;
        }

        private void Start()
        { 
            AssignEnemiesToPositions();
        }
        
        private void AssignEnemiesToPositions()
        {
            if (enemies.Length == 1)
            {
                foreach (var combatPosition in combatPositions)
                {
                    combatPosition.SetCharacter(enemies[0]); 
                } 
                combatPositions[1].SetCharacterPosition();
            }else if (enemies.Length == 3)
            {
                foreach (var combatPosition in combatPositions)
                {
                    combatPosition.SetCharacter(enemies[0]); 
                    combatPosition.SetCharacterPosition();
                }  
            }
        } 

        private void OnPlayerWaypointChanged(int obj)
        {
            //throw new System.NotImplementedException();
        }
        
        public Enemy GetCurrentEnemy()
        {
            return combatPositions[_movementHandler.CurrentWaypointIndex].GetCharacter() as Enemy;
        }
    }
}
