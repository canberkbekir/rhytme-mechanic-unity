using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class BeatPoolManager : MonoBehaviour
    {
        [FormerlySerializedAs("arrowPrefab")] [SerializeField] private RectTransform beatPrefab; // Arrow prefab
        [SerializeField] private int poolSize = 10; // Number of arrows in the pool

        private Queue<RectTransform> _beatPool;

        private void Awake()
        {
            _beatPool = new Queue<RectTransform>();

            // Preload arrows into the pool
            for (int i = 0; i < poolSize; i++)
            {
                var arrow = Instantiate(beatPrefab, transform);
                arrow.gameObject.SetActive(false);
                _beatPool.Enqueue(arrow);
            }
        }

        /// <summary>
        /// Gets an arrow from the pool.
        /// </summary>
        public RectTransform GetArrow()
        {
            if (_beatPool.Count > 0)
            {
                var arrow = _beatPool.Dequeue();
                arrow.gameObject.SetActive(true);
                return arrow;
            }
            else
            {
                // Optionally expand the pool if needed
                var arrow = Instantiate(beatPrefab, transform);
                return arrow;
            }
        }

        /// <summary>
        /// Returns an arrow back to the pool.
        /// </summary>
        public void ReturnArrow(RectTransform arrow)
        {
            arrow.gameObject.SetActive(false);
            _beatPool.Enqueue(arrow);
        }
    }
}