using Core;
using UnityEngine;
using Util;

namespace Gameplay.Interactions {
    [RequireComponent(typeof(SpriteRenderer))]
    public class PresentObjectiveInteraction : Interaction {
        public Objective objective;
        public Sprite completedSprite;

        private SpriteRenderer _renderer;
        [SerializeField] [ReadOnly] private bool isComplete;
        
        public void Complete() {
            isComplete = true;
        }
        
        public override void Interact() {
            GameManager.StartLevel(objective);
        }
        
        public override bool IsInteractable() {
            return !isComplete;
        }

        private void Start() {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update() {
            if (isComplete && _renderer.sprite != completedSprite) _renderer.sprite = completedSprite;
        }
    }
}