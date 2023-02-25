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
        
        private static Objective _currentObjective;
        

        public static void CompleteLevel() {
            PresentObjectiveInteraction presentObjective = (from obj in Object.FindObjectsOfType<PresentObjectiveInteraction>()
                where obj.objective == _currentObjective select obj).FirstOrDefault();
            if (presentObjective != null) presentObjective.Complete();
        }
        
        public static void StartLevel(Objective objective) {
            if (GameState == GameState.Level) return;
            
            _currentObjective = objective;
            GameState = GameState.Level;
            SceneHook.Get().QueueStartLevel(objective);
        }

        public static void OnLevelLoaded(Objective objective) {
            objective.scene.SetActiveScene();
        }
    }
}