using Core;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay.Interactions {
    public class PastObjectiveInteraction : Interaction {
        [Tooltip("Time to wait before switching back to the hub scene, after completing the level.")]
        public float delay;
        public UnityAction onLevelComplete;
        
        public override void Interact() {
            if (GameManager.GameState == GameState.Level) {
                Invoke(nameof(CompleteLevel), delay);
            }
        }
        
        private void CompleteLevel() {
            GameManager.CompleteLevel();
        }
    }
}