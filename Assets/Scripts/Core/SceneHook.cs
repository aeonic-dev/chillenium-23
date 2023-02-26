using Gameplay;
using UnityEngine;
using Util;

namespace Core {
    /// <summary>
    /// Ended up not needing the implementation that was here but I'm leaving it in case we need to do something before
    /// level loading later.
    /// </summary>
    public class SceneHook : MonoBehaviour {
        private static SceneHook _instance;

        public static SceneHook Get() {
            return _instance;
        }
        
        public void QueueStartLevel(Objective objective) {
            objective.scene.LoadScene();
        }

        private void Awake() {
            _instance = this;
            GameManager.Bootstrap();
        }
    }
}