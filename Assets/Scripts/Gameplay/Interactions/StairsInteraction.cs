using UnityEngine;

namespace Gameplay.Interactions {
    public class StairsInteraction : Interaction {
        public Vector2 position;
        
        public override void Interact() {
            Player.Get().transform.position = transform.position + (Vector3) position;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(position, Vector3.one * .5f);
        }
    }
}