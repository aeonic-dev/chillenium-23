using System.Collections;
using UnityEngine;

namespace Gameplay.Interactions {
    public class DummyInteraction : Interaction {
        public override void Interact() {
            StartCoroutine(Spin(360, .5f));
        }

        private IEnumerator Spin(float degrees, float duration) {
            float elapsed = 0;
            while (elapsed < duration) {
                elapsed += Time.deltaTime;
                transform.Rotate(0, 0, degrees * Time.deltaTime / duration);
                yield return null;
            }
        }
    }
}