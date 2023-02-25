using Core;
using UnityEngine;

namespace Gameplay.Interactions {
    [RequireComponent(typeof(SpriteRenderer))]
    public class PresentObjectiveInteraction : Interaction {
        public Objective objective;
        public Sprite completedSprite;

        private SpriteRenderer _renderer;
        private bool _isComplete;
        
        public void Complete() {
            _isComplete = true;
            _renderer.sprite = completedSprite;
        }
        
        public override void Interact() {
            GameManager.StartLevel(objective);
        }
        
        public override bool IsInteractable() {
            return !_isComplete;
        }
    }
}