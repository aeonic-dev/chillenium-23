using System.Linq;
using Gameplay;
using Gameplay.Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace Core {
    public class GameManager {
        public static GameState GameState = GameState.Menu;
        public static Objective CurrentObjective => GameState == GameState.Level ? _currentObjective : null;

        private static Scene _returnScene;
        private static Objective _currentObjective;
        

        public static void CompleteLevel() {
            PresentObjectiveInteraction presentObjective = (from obj in Object.FindObjectsOfType<PresentObjectiveInteraction>()
                where obj.objective == _currentObjective select obj).FirstOrDefault();
            if (presentObjective != null) presentObjective.Complete();
            
            SceneManager.SetActiveScene(_returnScene);
        }
        
        public static void StartLevel(Objective objective) {
            if (GameState == GameState.Level) return;
            
            _currentObjective = objective;
            GameState = GameState.Level;
            SceneHook.Get().QueueStartLevel(objective);
        }

        public static bool OnLevelLoaded(Objective objective) {
            return objective.scene.SetActiveScene();
        }
    }
}