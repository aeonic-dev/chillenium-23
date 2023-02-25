using System.Linq;
using Gameplay;
using Gameplay.Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core {
    public class GameManager {
        public static GameState GameState = GameState.Menu;
        public static Objective CurrentObjective => GameState == GameState.Level ? _currentObjective : null;

        private static string _returnScene;
        private static Objective _currentObjective;
        private static HubState _hubState = HubState.Empty;

        public static void CompleteLevel() {
            SceneManager.LoadScene(_returnScene);
            SceneManager.sceneLoaded += OnHubLoad;
        }

        private static void OnHubLoad(Scene scene, LoadSceneMode mode) {
            if (_hubState.presentObjectives == null) return;
            
            SceneManager.sceneLoaded -= OnHubLoad;
            HubState.Restore(_hubState);
            _hubState = HubState.Empty;
            GameState = GameState.Hub;
            
            PresentObjectiveInteraction presentObjective = (from obj in Object.FindObjectsOfType<PresentObjectiveInteraction>()
                where obj.objective == _currentObjective select obj).FirstOrDefault();
            if (presentObjective != null) presentObjective.Complete();
        }
        
        public static void StartLevel(Objective objective) {
            if (GameState == GameState.Level) return;
            
            _currentObjective = objective;
            GameState = GameState.Level;
            _returnScene = SceneManager.GetActiveScene().path;
            _hubState = HubState.Collect();
            
            SceneHook.Get().QueueStartLevel(objective);
        }
    }
}