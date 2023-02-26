using System;
using System.Linq;
using Gameplay;
using Gameplay.Interactions;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Core {
    public class GameManager {
        public static GameState GameState = GameState.Hub;
        public static Objective CurrentObjective => GameState == GameState.Level ? _currentObjective : null;

        public static ControlType ControlType {
            get => _controlType;
            set {
                OnControlTypeChange.Invoke(value);
                _controlType = value;
            }
        }

        public static Action<ControlType> OnControlTypeChange = (type) => { };

        private static string _returnScene;
        private static Objective _currentObjective;
        private static HubState _hubState = HubState.Empty;
        private static ControlType _controlType = ControlType.Keyboard;
        private static bool _initialized;

        public static void Bootstrap() {
            if (_initialized) return;
            _initialized = true;
            
            InputSystem.onEvent += OnInputEvent;
        }

        private static void OnInputEvent(InputEventPtr ptr, InputDevice device) {
            string name = device.displayName.ToLower();
            Debug.Log(device.description.deviceClass.ToLower());

            if (name.Contains("xbox") || name.Contains("stadia") || device.description.deviceClass.ToLower().Contains("gamepad")) ControlType = ControlType.XboxLike;   
            else ControlType = ControlType.Keyboard;
        }

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