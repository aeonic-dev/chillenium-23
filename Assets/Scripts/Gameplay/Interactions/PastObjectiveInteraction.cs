using Core;
using UnityEngine;

namespace Gameplay.Interactions {
    public class PastObjectiveInteraction : Interaction {
        [Tooltip("Time to wait before switching back to the hub scene, after completing the level.")]
        public float delay;
        
        private bool _completed;
        
        public override void Interact() {
            if (GameManager.GameState == GameState.Level) {
                _completed = true;
                Invoke(nameof(CompleteLevel), delay);
            }
        }
        
        public override bool IsInteractable() {
            return !_completed;
        }
        
        private void CompleteLevel() {
            GameManager.CompleteLevel();
        }
    }
}