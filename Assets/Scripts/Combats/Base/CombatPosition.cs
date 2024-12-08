using Characters.Base;
using UnityEngine;

namespace Combats.Base
{
    public class CombatPosition : MonoBehaviour
    {
        [SerializeField] private GameObject _highlightObject; 
        [SerializeField] private Character _character;
    
        public void HighlightPosition(bool isHighlighted)
        {
            _highlightObject.SetActive(isHighlighted);
        }
        
        public void SetCharacter(Character character)
        {
            _character = character;
        }
        
        public void SetCharacterPosition()
        {
            _character.transform.position = transform.position;
        }
        
        public void RemoveCharacter()
        {
            _character = null;
        }
        
        public Character GetCharacter()
        {
            return _character;
        }
        
        public bool IsOccupied()
        {
            return _character != null;
        }
    
    }
}