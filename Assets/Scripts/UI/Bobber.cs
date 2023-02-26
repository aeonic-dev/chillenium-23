using UnityEngine;

namespace UI {
    public class Bobber : MonoBehaviour {
        public float speed = 1f;
        public float amplitude = 1f;
        public float exponent = 1f;

        private Vector3 _originalPosition;
        private Vector3 _lastPosition;

        private void Start() {
            _originalPosition = transform.localPosition;
        }

        private void Update() {
            if (transform.position != _lastPosition) _originalPosition = transform.localPosition;
            float sin = Mathf.Sin(Time.time * speed), sign = Mathf.Sign(sin);
            _lastPosition = transform.localPosition =
                _originalPosition + Vector3.up * (Mathf.Pow(Mathf.Abs(sin), exponent) * amplitude * sign);
        }
    }
}