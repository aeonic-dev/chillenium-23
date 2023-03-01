using Unity.VisualScripting;
using UnityEngine;

namespace UI {
    public class ObjectivesUI : MonoBehaviour {
        public ObjectiveHeart[] hearts;

        private static ObjectivesUI _instance;
        private int _index = -1;

        public static ObjectivesUI Get() {
            if (_instance == null || _instance.IsDestroyed()) return _instance = FindObjectOfType<ObjectivesUI>();
            return _instance;
        }

        public void PutLeft() {
            _index++;
            hearts[_index].ShowLeft();
        }

        public void PutRight() {
            hearts[_index].ShowRight();
        }

        private void Awake() {
            if (_instance != null && _instance != this) Destroy(gameObject);
            else {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}