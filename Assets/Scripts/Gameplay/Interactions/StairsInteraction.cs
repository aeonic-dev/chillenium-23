using UnityEngine;

namespace Gameplay.Interactions {
    public class StairsInteraction : Interaction {
        public Vector3 position;
        
        public override void Interact() {
            Player.Get().transform.position = transform.position + position;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position + position, Vector3.one * 1f);
        }
    }
}