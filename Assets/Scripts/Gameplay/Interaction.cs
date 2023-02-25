using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(Collider2D))]
    public abstract class Interaction : MonoBehaviour {
        public abstract void Interact();

        public virtual bool IsInteractable() {
            return true;
        }

        private void Awake() {
            // Ensure the attached collider is a trigger to avoid physical collissions
            GetComponent<Collider2D>().isTrigger = true;
        }
    }
}