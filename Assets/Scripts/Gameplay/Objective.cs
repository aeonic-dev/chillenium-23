using UnityEngine;

namespace Gameplay {
    [CreateAssetMenu(fileName = "Objective", menuName = "Add Objective Definition", order = 0)]
    public class Objective : ScriptableObject {
        [Tooltip("The name of the objective")]
        public string objectiveName;
        [Tooltip("Time the player is given for completion, in seconds")]
        public int timer;
        [Tooltip("The platformer scene for this objectggive")]
        public SceneReference scene;
    }
}