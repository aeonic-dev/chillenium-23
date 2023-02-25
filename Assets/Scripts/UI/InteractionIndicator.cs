using UnityEngine;

namespace UI {
    public class InteractionIndicator : MonoBehaviour {
        public static InteractionIndicator Instance { get; private set; }
        private static Vector3 _target;
        
        public Vector3 offset = new Vector3(0, 0.5f, 0);
        public float speed = .5f;
        
        
        private void Awake() {
            if (Instance != null) Destroy(Instance);
            Instance = this;
        }

        private void Update() {
            if (Instance.gameObject.activeSelf) {
                Instance.transform.position = Vector3.Lerp(Instance.transform.position, _target + offset, speed * Time.deltaTime);
            }
        }
        
        public static void Show(Vector3 position) {
            Instance.gameObject.SetActive(true);
            _target = position;
            Instance.transform.position = _target + Instance.offset;
        }

        public static void Hide() {
            Instance.gameObject.SetActive(false);
        }
    }
}