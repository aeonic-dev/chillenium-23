using Core;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Interactions {
    public class PastObjectiveInteraction : Interaction {
        [Tooltip("Time to wait before switching back to the hub scene, after completing the level.")]
        public float delay;
        public UnityEvent onInteract;
        
        private bool _completed;
        
        public override void Interact() {
            if (GameManager.GameState == GameState.Level) {
                ObjectivesUI.Get().PutRight();
                _completed = true;
                onInteract.Invoke();
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