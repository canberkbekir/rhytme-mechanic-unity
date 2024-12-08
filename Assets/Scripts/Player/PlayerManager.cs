using Base;
using UnityEngine;

namespace Player
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private MovementHandler movementHandler;
        [SerializeField] private PlayerHandler playerHandler;
        public MovementHandler MovementHandler => movementHandler;
        public PlayerHandler PlayerHandler => playerHandler;
    }
}
