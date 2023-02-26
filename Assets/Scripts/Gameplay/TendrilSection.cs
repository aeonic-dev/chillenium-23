using Core;
using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(Collider2D))]
    public class TendrilSection : MonoBehaviour {
        private void OnCollisionEnter2D(Collision2D col) {
            var player = col.gameObject.GetComponent<Player>();
            if (player != null) {
                GameManager.StartLevel(GameManager.CurrentObjective);
            }
        }
    }
}