using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Core {
    public class SceneHook : MonoBehaviour {
        public SceneReference hubScene;
        
        private Objective _queuedObjective;
        
        public void QueueStartLevel(Objective objective) {
            _queuedObjective = objective;
            objective.scene.LoadScene(LoadSceneMode.Additive);
        }

        public static SceneHook Get() {
            return FindObjectOfType<SceneHook>();
        }
        
        private void Update() {
            if (_queuedObjective != null) {
                GameManager.OnLevelLoaded(_queuedObjective);
                _queuedObjective = null;
            }
        }
    }
}