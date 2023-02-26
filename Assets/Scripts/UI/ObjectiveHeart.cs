using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class ObjectiveHeart : MonoBehaviour {
        public Image leftPiece;
        public Image rightPiece;
        public Image combined;

        public float fadeDelay = .5f;
        public float fadeTime = .5f;

        public void ShowLeft() {
            leftPiece.enabled = true;
        }
        
        public void ShowRight() {
            rightPiece.enabled = true;
            ShowCombined();
        }
        
        public void ShowCombined() {
            combined.color = Color.clear;
            combined.enabled = true;

            StartCoroutine(FadeInCombined(fadeDelay));
        }
        
        private IEnumerator FadeInCombined(float delay) {
            yield return new WaitForSeconds(delay);
            
            var timer = 0f;
            while (timer < fadeTime) {
                timer += Time.deltaTime;
                combined.color = Color.Lerp(Color.clear, Color.white, timer / fadeTime);
                yield return null;
            }
        }

        private void Start() {
            leftPiece.enabled = false;
            rightPiece.enabled = false;
            combined.enabled = false;
        }
    }
}