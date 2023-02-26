using Core;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay {
    
/*
        (\,/)
      oo   '''//,        _
    ,/_;~,        \,    / '
    "'   \    (    \    !
          ',|  \    |__.'
          '~  '~----''
*/
    
    public class Tendrils : MonoBehaviour {
        public GameObject tendrilSectionPrefab;
        
        private static Tendrils _instance;

        private Objective _objective;
        private Vector3 _start;
        private Vector3 _end;
        private float _timer = -1;
        
        public static Tendrils Get() {
            if (_instance == null || _instance.IsDestroyed()) return _instance = FindObjectOfType<Tendrils>();
            return _instance;
        }

        public void Setup(Objective objective) {
            _objective = objective;
            _start = LevelBounds.Get().bottomLeft;
            _end = new Vector3(LevelBounds.Get().topRight.x + 1, _start.y);
            _timer = 0;

            int count = Mathf.FloorToInt((LevelBounds.Get().topRight.y - _start.y) / 2) + 1;
            for (int i = 0; i < count; i++) {
                var section = Instantiate(tendrilSectionPrefab, transform);
                section.transform.localPosition = Vector3.up * i * 2; //new Vector3(0, i * 2, 0);
            }

            if (_objective.direction == Direction.RightToLeft) {
                transform.RotateAround(_start, Vector3.forward, 180);
                _start += Vector3.up * count * 2;
                _end += Vector3.up * count * 2;
            }
            
            transform.position = _start;
        }
        
        private void Awake() {
            _instance = this;
        }

        private void Start() {
            Setup(GameManager.CurrentObjective);
        }
        
        private void FixedUpdate() {
            if (_timer < 0) return;
            _timer = Mathf.Min(_timer + Time.deltaTime, _objective.timer);
            
            float progress = Mathf.Max((_timer - _objective.delay) / _objective.timer, 0);
            if (_objective.direction == Direction.LeftToRight) {
                transform.position = Vector3.Lerp(_start, _end, progress);
            } else {
                transform.position = Vector3.Lerp(_end, _start, progress);
            }
        }
    
        public enum Direction {
            LeftToRight,
            RightToLeft
        }
    }
}