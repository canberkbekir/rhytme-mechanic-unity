using System;
using System.Collections;
using System.Collections.Generic;
using Base;
using Characters.Enemies.Base;
using Combats.Base;
using Managers;
using UnityEngine;

namespace Characters.Enemies.AI.Base
{
    public enum AttackStyle
    { 
        Left,
        Right,
        Middle,
        LeftMiddle,
        RightMiddle,
        LeftRight,
        All
    }

    public enum AttackState
    {
        Idle,
        PrepareAttack,
        Attack
    }

    public abstract class EnemyBehaviour : MonoBehaviour
    {
        protected AttackStyle AttackStyle;
        protected AttackState AttackState;

        // Observable pattern: Events for state changes
        public event Action<AttackState> OnStateChange;
        public event Action<AttackStyle> OnAttackStyleChange;

        // References
        protected BeatManager BeatManager;
        protected MovementHandler MovementHandler;
        protected RhythmInputHandler RhythmInputHandler;
        protected CombatPosition[] Waypoints;
        protected Enemy Enemy; 
        
        //booleans
        protected bool isCombatStarted = true;

        // Mapping of attack styles to waypoints
        private readonly Dictionary<AttackStyle, int[]> _styleToWaypoints = new()
        {
            { AttackStyle.Left, new[] { 0 } },
            { AttackStyle.Right, new[] { 2 } },
            { AttackStyle.Middle, new[] { 1 } },
            { AttackStyle.LeftMiddle, new[] { 0, 1 } },
            { AttackStyle.RightMiddle, new[] { 1, 1 } },
            { AttackStyle.LeftRight, new[] { 0, 2 } },
            { AttackStyle.All, new[] { 0, 1, 2 } }
        };

        protected virtual void Awake()
        {
            BeatManager = GameManager.Instance.BeatManager;
            MovementHandler = GameManager.Instance.Player.MovementHandler;
            RhythmInputHandler = GameManager.Instance.RhythmInputHandler;
            Enemy = gameObject.GetComponent<Enemy>();

            BeatManager.OnBeat += OnBeat;
        }

        protected virtual void Start()
        {
            Waypoints = MovementHandler.Waypoints;
            SetAttackState(AttackState.Idle); 
        }

        protected virtual void OnBeat()
        {
            if (!isCombatStarted) return;
            switch (AttackState)
            {
                case AttackState.Idle:
                    HandleIdle();
                    break;
                case AttackState.PrepareAttack:
                    HandlePrepareAttack();
                    break;
                case AttackState.Attack:
                    HandleAttack();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void HandleAttack()
        {
            HighlightTargets(_styleToWaypoints[AttackStyle], true, true);
            StartCoroutine(WaitForAttack(RhythmInputHandler.InputTolerance));
        }

        private IEnumerator WaitForAttack(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            ExecuteAttack();
            OnAttackComplete();
            SetAttackState(AttackState.Idle);
        }

        protected virtual void ExecuteAttack()
        {
            foreach (var position in _styleToWaypoints[AttackStyle])
            {
                var character = Waypoints[position].GetCharacter();
                if (character != null)
                {
                    character.TakeDamage(Enemy.attackPower);
                }
            }
        }

        protected virtual void OnAttackComplete()
        { 
        }
        protected virtual void HandlePrepareAttack()
        {
            HighlightTargets(_styleToWaypoints[AttackStyle], true, false);
            SetAttackState(AttackState.Attack);
        }

        protected virtual void HandleIdle()
        {
            ResetHighlights();
            SetAttackState(AttackState.PrepareAttack);
        }

        private void HighlightTargets(int[] positions, bool highlight, bool isAttack)
        {
            foreach (var position in positions)
            {
                if (isAttack)
                    Waypoints[position].EnemyAttackHighlight(highlight);
                else
                    Waypoints[position].Highlight(highlight);
            }
        }

        private void ResetHighlights()
        {
            foreach (var waypoint in Waypoints)
            {
                waypoint.Highlight(false);
                waypoint.EnemyAttackHighlight(false);
            }
        }

        protected void SetAttackState(AttackState newState)
        {
            AttackState = newState;
            OnStateChange?.Invoke(newState);
        }

        protected void SetAttackStyle(AttackStyle newStyle)
        {
            AttackStyle = newStyle;
            OnAttackStyleChange?.Invoke(newStyle);
        }
    }
}
