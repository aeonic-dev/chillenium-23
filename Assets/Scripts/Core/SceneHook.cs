using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Core {
    public class SceneHook : MonoBehaviour {
        private static SceneHook _instance;
        private Objective _queuedObjective;

        public static SceneHook Get() {
            return _instance;
        }
        
        public void QueueStartLevel(Objective objective) {
            _queuedObjective = objective;
            objective.scene.LoadScene(LoadSceneMode.Additive);
        }

        private void Awake() {
            _instance = this;
        }
        
        private void Update() {
            if (_queuedObjective != null) {
                GameManager.OnLevelLoaded(_queuedObjective);
                _queuedObjective = null;
            }
        }
    }
}