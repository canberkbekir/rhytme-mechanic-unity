using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
     
        [SerializeField] private BeatManager beatManager;
        [SerializeField] private RhythmInputManager rhythmInputManager;
        
        public BeatManager BeatManager => beatManager;
        public RhythmInputManager RhythmInputManager => rhythmInputManager;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
