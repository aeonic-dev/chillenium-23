using Core;
using UnityEngine;

namespace Gameplay {
    [RequireComponent(typeof(Camera))]
    public class PlayerCamera : MonoBehaviour {
        [Range(0, 1)] public float speed = .2f;

        private Player _player;
        private Camera _camera;
        private Transform _cameraTransform;
        private Vector2 _minPosition;
        private Vector2 _maxPosition;

        private void Start() {
            _player = FindObjectOfType<Player>();
            _camera = GetComponent<Camera>();
            _cameraTransform = _camera.transform;

            LevelBounds bounds = LevelBounds.Get();
            float orthographicSize = _camera.orthographicSize;
            float aspect = _camera.aspect;

            _minPosition = bounds.bottomLeft + new Vector2(orthographicSize * aspect, orthographicSize);
            _maxPosition = bounds.topRight - new Vector2(orthographicSize * aspect, orthographicSize);
        }

        private void LateUpdate() {
            Vector2 targetPosition = _player.transform.position;
            targetPosition.x = Mathf.Clamp(targetPosition.x, _minPosition.x, _maxPosition.x);
            targetPosition.y = Mathf.Clamp(targetPosition.y, _minPosition.y, _maxPosition.y);

            var target = new Vector3(targetPosition.x, targetPosition.y, _camera.transform.position.z);
            _cameraTransform.position = Vector3.Lerp(_cameraTransform.position, target, speed * Time.deltaTime * 20);
        }
    }
}