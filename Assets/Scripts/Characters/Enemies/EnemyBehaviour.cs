using System;
using Base;
using Combats.Base;
using Managers;
using UnityEngine;

namespace Characters.Enemies
{
    public enum AttackState
    {
        Idle,
        Left,
        Right,
        Middle,
        LeftMiddle,
        RightMiddle,
        LeftRight,
        All
    }

    public class EnemyBehaviour : MonoBehaviour
    {
        private AttackState _attackState;

        private BeatManager _beatManager;
        private MovementHandler _movementHandler;
        private CombatPosition[] _waypoints;

        private int _beatCounter = 0;
        private bool _isPrepared = false;
        private bool _hasAttacked = false;

        private void Awake()
        {
            _beatManager = GameManager.Instance.BeatManager;
            _movementHandler = GameManager.Instance.Player.MovementHandler;

            _beatManager.OnBeat += OnBeat;
        }

        private void Start()
        {
            _waypoints = _movementHandler.Waypoints;
        }

        private void OnBeat()
        {
            _beatCounter++;

            if (!_isPrepared && !_hasAttacked) // Prepare attack
            {
                PrepareAttack();
            }
            else if (_isPrepared && !_hasAttacked) // Execute attack
            {
                ExecuteAttack();
            }
            else if (_hasAttacked) // Wait one beat after attack
            {
                Debug.Log("Waiting one beat after attack.");
                _hasAttacked = false; // Reset for the next cycle
            }
        }

        private void PrepareAttack()
        {
            ChooseRandomState();
            _isPrepared = true;
            Debug.Log($"Preparing attack: {_attackState}");

            ResetHighlights(); // Reset any previous highlights

            switch (_attackState)
            {
                case AttackState.Left:
                    _waypoints[0].HighlightPosition(true);
                    break;
                case AttackState.Middle:
                    _waypoints[1].HighlightPosition(true);
                    break;
                case AttackState.Right:
                    _waypoints[2].HighlightPosition(true);
                    break;
                case AttackState.LeftMiddle:
                    _waypoints[0].HighlightPosition(true);
                    _waypoints[1].HighlightPosition(true);
                    break;
                case AttackState.RightMiddle:
                    _waypoints[1].HighlightPosition(true);
                    _waypoints[2].HighlightPosition(true);
                    break;
                case AttackState.LeftRight:
                    _waypoints[0].HighlightPosition(true);
                    _waypoints[2].HighlightPosition(true);
                    break;
                case AttackState.All:
                    foreach (var waypoint in _waypoints)
                    {
                        waypoint.HighlightPosition(true);
                    }
                    break;
                case AttackState.Idle:
                default:
                    // No highlights for Idle state
                    break;
            }
        }

        private void ExecuteAttack()
        {
            if (!_isPrepared)
            {
                Debug.LogWarning("Tried to execute an attack without preparation!");
                return;
            }

            Debug.Log($"Executing attack: {_attackState}");

            ResetHighlights(); // Turn off highlights when attacking

            switch (_attackState)
            {
                case AttackState.Left:
                    Debug.Log("Attacking Left");
                    break;
                case AttackState.Middle:
                    Debug.Log("Attacking Middle");
                    break;
                case AttackState.Right:
                    Debug.Log("Attacking Right");
                    break;
                case AttackState.LeftMiddle:
                    Debug.Log("Attacking Left and Middle");
                    break;
                case AttackState.RightMiddle:
                    Debug.Log("Attacking Right and Middle");
                    break;
                case AttackState.LeftRight:
                    Debug.Log("Attacking Left and Right");
                    break;
                case AttackState.All:
                    Debug.Log("Attacking All Positions");
                    break;
                case AttackState.Idle:
                default:
                    Debug.Log("Enemy is idle during attack phase.");
                    break;
            }

            _isPrepared = false;
            _hasAttacked = true;
        }

        private void ResetHighlights()
        {
            foreach (var waypoint in _waypoints)
            {
                waypoint.HighlightPosition(false);
            }
        }

        private void ChooseRandomState()
        {
            // Exclude 'Idle' if it shouldn't be randomly chosen
            Array values = Enum.GetValues(typeof(AttackState));
            _attackState = (AttackState)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        private void OnDestroy()
        {
            if (_beatManager != null)
            {
                _beatManager.OnBeat -= OnBeat;
            }
        }
    }
}