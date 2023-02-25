using System.Collections;
using UnityEngine;

namespace UI {
    public class InteractionIndicator : MonoBehaviour {
        public static InteractionIndicator Instance { get; private set; }

        public Vector3 offset = new(0, 0.5f, 0);
        public float easingTime = .1f;
        public AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private SpriteRenderer _renderer;
        private Vector3 _target;
        private float _targetOpacity;

        private void Start() {
            Instance = this;
            _renderer = GetComponent<SpriteRenderer>();
            _renderer.color = Color.clear;
        }

        public static void Show(Vector3 position) {
            Instance.ShowInstance(position);
        }

        public static void Hide() {
            Instance.HideInstance();
        }

        private void ShowInstance(Vector3 position) {
            _target = position + offset;
            transform.position = _target + offset;
            StartCoroutine(FadeIn());
        }

        private void HideInstance() {
            StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn() {
            _renderer.color = Color.clear;
            return Fade(Color.white);
        }

        private IEnumerator FadeOut() {
            return Fade(Color.clear, false);
        }

        private IEnumerator Fade(Color fadeTo, bool doPosition = true) {
            Color fadeFrom = _renderer.color;
            Vector3 startPosition = transform.position;
            
            float time = 0f;
            while (time < easingTime) {
                time += Time.deltaTime;
                float fraction = easingCurve.Evaluate(time / easingTime);
                
                _renderer.color = Color.Lerp(fadeFrom, fadeTo, fraction);
                if (doPosition) transform.position = Vector3.Lerp(startPosition, _target, fraction);
                yield return null;
            }
        }
    }
}