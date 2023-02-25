using System;
using UnityEngine;
using Util;

namespace Gameplay {
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour {
        [Header("Lateral Movement")]
        [Tooltip("Minimum input magnitude to start moving")] [Range(0, 1)]
        public float moveDeadzone = .2f;
        [Tooltip("Top lateral speed in units per second")]
        public float moveSpeed = 5f;
        [Tooltip("Magnitude of lateral acceleration when the player is entering movement input (units per second squared)")]
        public float moveAcceleration = 5f;
        [Tooltip("Multiplier for x velocity when the player is not entering movement input")]
        public float moveDecelerationMultiplier = .9f;

        [Header("Jumping")]
        [Tooltip("Magnitude of jumping force")]
        public float jumpForce = 2f;
        [Tooltip("Time in seconds the player can jump after leaving the ground")]
        public float jumpGracePeriod = .05f;
        [Tooltip("Distance from the player's origin to the ground at which the player is considered grounded")]
        public float groundDistance = .5f;

        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private float _groundTimer;

        private void Start() {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        
        private void Update() {
            if (IsGrounded()) _groundTimer = 0;
            _groundTimer += Time.deltaTime;

            if (CanJump() && Input.GetButtonDown("Jump")) {
                _groundTimer = jumpGracePeriod + 1;
                _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            
            float xInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(xInput) > moveDeadzone) {
                if (Mathf.Abs(_rigidbody.velocity.x) < moveSpeed)
                    _rigidbody.AddForce(Vector2.right * xInput * moveAcceleration, ForceMode2D.Impulse);
            } else {
                _rigidbody.velocity *= new Vector2(moveDecelerationMultiplier, 1);
            }
        }

        private bool CanJump() {
            return _groundTimer <= jumpGracePeriod;
        }

        private bool IsGrounded() {
            RaycastHit2D hit = Physics2D.BoxCast(_transform.position, Vector2.one, 0, Vector2.down, groundDistance);
            return hit.collider != null;
        }
    }
}   