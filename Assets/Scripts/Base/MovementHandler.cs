using UnityEngine;
using DG.Tweening;
using Managers;
using Player; // Make sure to include DOTween

namespace Base
{
    public class MovementHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int startWaypointIndex = 1;
        [SerializeField] private float changeWaypointTime = 0.3f; // Time to reach the next waypoint
        [SerializeField] private float heightOffset = 0.5f; // Height of the arc during the movement
        
        [Header("Debug")]
        [SerializeField] private int _currentWaypointIndex;
        
        [Header("References")]
        [SerializeField] private Transform[] waypoints; // Array of waypoints 
        
        private PlayerManager _player;
        private void Awake()
        { 
            _player = GameManager.Instance.Player;
            
            // Set the initial position to the starting waypoint
            _currentWaypointIndex = startWaypointIndex;
            transform.position = waypoints[_currentWaypointIndex].position;
            
        }

        public void MoveRight()
        {
            if (_currentWaypointIndex < waypoints.Length - 1)
            {
                _currentWaypointIndex++;
                // Call the animation method to move to the next waypoint
                MoveTransformAnimation(waypoints[_currentWaypointIndex]);
            }
        }
        
        public void MoveLeft()
        {
            if (_currentWaypointIndex > 0)
            {
                _currentWaypointIndex--;
                // Call the animation method to move to the previous waypoint
                MoveTransformAnimation(waypoints[_currentWaypointIndex]);
            }
        }
        
        private void MoveTransformAnimation(Transform targetTransform)
        {
            // Get the current position and the target position
            var startPosition = _player.transform.position;
            var targetPosition = targetTransform.position;

            // Calculate the middle position for the arc (parabolic movement)
            var arcPeak = new Vector3(
                (startPosition.x + targetPosition.x) / 2, // Middle point on X
                Mathf.Max(startPosition.y, targetPosition.y) + heightOffset, // Higher point on Y
                (startPosition.z + targetPosition.z) / 2  // Middle point on Z
            );

            // Use DOTween to create the arc-like motion
            var moveSequence = DOTween.Sequence();

            moveSequence.Append(_player.transform.DOPath(new[] { startPosition, arcPeak, targetPosition }, changeWaypointTime, PathType.CatmullRom)
                .SetEase(Ease.OutQuad));
        }
    }
}
