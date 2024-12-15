using System;
using Combats.Base;
using UnityEngine;
using DG.Tweening;
using Managers;
using Player;

namespace Base
{
    public class MovementHandler : MonoBehaviour
    {
        // Public events
        public event Action<int> OnWaypointChanged;

        [Header("Settings")]
        [SerializeField] private int startWaypointIndex = 1;
        [SerializeField] private float waypointTransitionTime = 0.3f;
        [SerializeField] private float arcHeight = 0.5f;
        [SerializeField] private float attackRange = 3f;

        [Header("References")]
        [SerializeField] private CombatPosition[] waypoints;

        [Header("Debug")]
        [SerializeField] private int currentWaypointIndex;

        private PlayerManager _player;

        // Public Properties
        public int CurrentWaypointIndex => currentWaypointIndex;
        public CombatPosition[] Waypoints => waypoints;

        private void Awake()
        {
            _player = GameManager.Instance.Player;

            if (_player == null)
            {
                Debug.LogError("PlayerManager is not set up in GameManager!");
                return;
            }

            // Initialize starting position
            currentWaypointIndex = startWaypointIndex;
            transform.position = waypoints[currentWaypointIndex].transform.position;
        }

        private void Start()
        {
            waypoints[currentWaypointIndex].SetCharacter(_player.PlayerHandler);
        }

        public void MoveRight()
        {
            MoveToWaypoint(currentWaypointIndex + 1);
        }

        public void MoveLeft()
        {
            MoveToWaypoint(currentWaypointIndex - 1);
        }

        public void PerformAttack()
        {
            AnimateAttack();
        }

        private void MoveToWaypoint(int waypointIndex)
        {
            if (IsWaypointValid(waypointIndex))
            {
                waypoints[currentWaypointIndex].RemoveCharacter();
                currentWaypointIndex = waypointIndex;
                OnWaypointChanged?.Invoke(waypointIndex);
                waypoints[waypointIndex].SetCharacter(_player.PlayerHandler);
                AnimateMovement(waypoints[waypointIndex].transform.position);
            }
        }

        private bool IsWaypointValid(int waypointIndex)
        {
            return waypointIndex >= 0 && waypointIndex < waypoints.Length;
        }

        private void AnimateMovement(Vector3 targetPosition)
        {
            Vector3 arcPeak = CalculateArc(transform.position, targetPosition);

            DOTween.Sequence()
                .Append(_player.transform.DOPath(
                    new[] { transform.position, arcPeak, targetPosition },
                    waypointTransitionTime,
                    PathType.CatmullRom
                ).SetEase(Ease.OutQuad));
        }

        private void AnimateAttack()
        {
            Vector3 startPosition = _player.transform.position;
            Vector3 attackTarget = startPosition + _player.transform.forward * -attackRange;

            Vector3 forwardArcPeak = CalculateArc(startPosition, attackTarget);
            Vector3 returnArcPeak = CalculateArc(attackTarget, startPosition);

            DOTween.Sequence()
                .Append(_player.transform.DOPath(
                    new[] { startPosition, forwardArcPeak, attackTarget },
                    waypointTransitionTime / 2,
                    PathType.CatmullRom
                ).SetEase(Ease.OutQuad))
                .Append(_player.transform.DOPath(
                    new[] { attackTarget, returnArcPeak, startPosition },
                    waypointTransitionTime / 2,
                    PathType.CatmullRom
                ).SetEase(Ease.InQuad));
        }

        private Vector3 CalculateArc(Vector3 start, Vector3 target)
        {
            return new Vector3(
                (start.x + target.x) / 2,
                Mathf.Max(start.y, target.y) + arcHeight,
                (start.z + target.z) / 2
            );
        }
    }
}
