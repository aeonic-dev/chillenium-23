using Core;
using UI;
using UnityEngine;
using UnityEngine.Events;
using Util;

/*
         ____
        |    |
        |____|
       _|____|_
        /  ee`.
      .<     __O
     /\ \.-.' \
    J  `.|`.\/ \
    | |_.|  | | |
     \__.'`.|-' /
     L   /|o`--'\ 
     |  /\/\/\   \           
     J /      `.__\
     |/         /  \     
      \\      .'`.  `.                                            .'
    ____)_/\_(____`.  `-._______________________________________.'/
   (___._/  \_.___) `-.________________________________________.-'
 */

namespace Gameplay.Interactions {
    [RequireComponent(typeof(SpriteRenderer))]
    public class PresentObjectiveInteraction : Interaction {
        public Objective objective;
        public Sprite completedSprite;
        public float delay;
        public UnityEvent onInteract;

        private SpriteRenderer _renderer;
        [SerializeField] [ReadOnly] private bool isComplete;
        
        public void Complete() {
            isComplete = true;
        }
        
        public override void Interact() {
            ObjectivesUI.Get().PutLeft();
            onInteract.Invoke();
            Invoke(nameof(StartLevel), delay);
        }

        private void StartLevel() {
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