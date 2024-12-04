using Base;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private MovementHandler movementHandler;
        
        public MovementHandler MovementHandler => movementHandler;
    }
}
