using UnityEngine;

namespace UI {
    public abstract class SpriteButton : MonoBehaviour {
        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit hit;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray.origin, ray.direction, out hit)) {
                    Click();
                }
            }
        }

        public abstract void Click();
    }
}