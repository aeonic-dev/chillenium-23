using UnityEngine;

namespace Gameplay {
    [CreateAssetMenu(fileName = "Objective", menuName = "Add Objective Definition", order = 0)]
    public class Objective : ScriptableObject {
        [Tooltip("The name of the objective")]
        public string objectiveName;
        [Tooltip("Time the player is given for completion, in seconds")]
        public int timer;
        [Tooltip("Time in seconds before the timer begins")]
        public float delay;
        [Tooltip("The direction the tendrils will move")]
        public Tendrils.Direction direction;
        [Tooltip("The platformer scene for this objectggive")]
        public SceneReference scene;
    }
}