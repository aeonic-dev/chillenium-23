using UnityEngine;

namespace Core {
    /// <summary>
    /// Simple camera bounding box that exists in every level.
    /// </summary>
    public class LevelBounds : MonoBehaviour {
        [Tooltip("The point furthest to the left and down that the camera will ever see.")]
        public Vector2 bottomLeft;
        [Tooltip("The point furthest to the right and up that the camera will ever see.")]
        public Vector2 topRight;

        public static LevelBounds Get() {
            return FindObjectOfType<LevelBounds>();
        }

        private void OnDrawGizmos() {
            Vector2 topLeft = new(bottomLeft.x, topRight.y);
            Vector2 bottomRight = new(topRight.x, bottomLeft.y);
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(bottomLeft, topLeft);
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
        }
    }
}