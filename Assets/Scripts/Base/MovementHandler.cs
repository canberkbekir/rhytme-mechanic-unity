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
        //Public events
        public event Action<int> OnWaypointChanged; 
        
        [Header("Settings")]
        [SerializeField] private int startWaypointIndex = 1;
        [SerializeField] private float changeWaypointTime = 0.3f;
        [SerializeField] private float heightOffset = 0.5f;
        [SerializeField] private float attackOffset = 3f;

        [Header("References")]
        [SerializeField] private CombatPosition[] waypoints;

        [Header("Debug")]
        [SerializeField] private int currentWaypointIndex;

        private PlayerManager _player;
        
        //Public Variables
        public int CurrentWaypointIndex => currentWaypointIndex;

        private void Awake()
        {
            _player = GameManager.Instance.Player;

            if (_player == null)
            {
                Debug.LogError("PlayerManager is not set up in GameManager!");
                return;
            }

            currentWaypointIndex = startWaypointIndex;
            transform.position = waypoints[currentWaypointIndex].transform.position;
        }

        public void MoveRight()
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                MoveToWaypoint(++currentWaypointIndex);
            }
        }

        public void MoveLeft()
        {
            if (currentWaypointIndex > 0)
            {
                MoveToWaypoint(--currentWaypointIndex);
            }
        }

        public void MoveAttack()
        {
            PerformAttackAnimation();
        }

        private void MoveToWaypoint(int waypointIndex)
        {
            if (waypointIndex < 0 || waypointIndex >= waypoints.Length) return;
            OnWaypointChanged?.Invoke(waypointIndex);
            AnimateMovement(waypoints[waypointIndex].transform.position);
        }

        private void AnimateMovement(Vector3 targetPosition)
        {
            var startPosition = _player.transform.position;
            var arcPeak = CalculateArcPeak(startPosition, targetPosition);

            DOTween.Sequence()
                .Append(_player.transform.DOPath(
                    new[] { startPosition, arcPeak, targetPosition },
                    changeWaypointTime,
                    PathType.CatmullRom
                ).SetEase(Ease.OutQuad));
        }

        private void PerformAttackAnimation()
        {
            var startPosition = _player.transform.position;
            var targetPosition = startPosition + _player.transform.forward * -attackOffset;

            var forwardArcPeak = CalculateArcPeak(startPosition, targetPosition);
            var returnArcPeak = CalculateArcPeak(targetPosition, startPosition);

            DOTween.Sequence()
                .Append(_player.transform.DOPath(
                    new[] { startPosition, forwardArcPeak, targetPosition },
                    changeWaypointTime / 2,
                    PathType.CatmullRom
                ).SetEase(Ease.OutQuad))
                .Append(_player.transform.DOPath(
                    new[] { targetPosition, returnArcPeak, startPosition },
                    changeWaypointTime / 2,
                    PathType.CatmullRom
                ).SetEase(Ease.InQuad))
                .OnComplete(() => Debug.Log("Attack animation complete"));
        }

        private Vector3 CalculateArcPeak(Vector3 start, Vector3 target)
        {
            return new Vector3(
                (start.x + target.x) / 2,
                Mathf.Max(start.y, target.y) + heightOffset,
                (start.z + target.z) / 2
            );
        }
    }
}
