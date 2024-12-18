using Base;
using Managers;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace UI
{
    public class UIBeatController : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image beatImage; 
        [SerializeField] private RectTransform leftBeatSpawnPoint;
        
        [Header("UI Settings")]
        [SerializeField] private int beatCount = 10;
        
        private BeatManager _beatManager;
        
        private void Awake()
        {
            _beatManager = GameManager.Instance?.BeatManager;
            if (_beatManager == null)
            {
                Debug.LogError("Required components are missing. Please ensure GameManager is properly set up.");
            } 
        }
        
      

    }
}
