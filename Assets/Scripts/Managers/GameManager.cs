using Base;
using Player;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
     
        [SerializeField] private BeatManager beatManager;
        [SerializeField] private RhythmInputHandler rhythmInputHandler;
        [SerializeField] private PlayerManager player;
        [SerializeField] private CombatManager combatManager;
        public BeatManager BeatManager => beatManager;
        public RhythmInputHandler RhythmInputHandler => rhythmInputHandler;
         public PlayerManager Player => player;
        public CombatManager CombatManager => combatManager;
        
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
