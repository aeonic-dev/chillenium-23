using System;
using UnityEngine;
using Util;

namespace Core {
    [RequireComponent(typeof(BoxCollider2D))]
        public class OldPlayer : MonoBehaviour {
            [Header("Lateral Movement")] [Tooltip("Minimum input magnitude to start moving")] [Range(0, 1)]
            public float moveDeadzone = .2f;

            [Tooltip("Top lateral speed in units per second")]
            public float moveSpeed = 5f;

            [Tooltip(
                "Magnitude of lateral acceleration when the player is entering movement input (units per second squared)")]
            public float moveAcceleration = 5f;

            [Tooltip("Multiplier for x velocity when the player is not entering movement input")]
            public float moveDecelerationMultiplier = .9f;

            [Header("Jumping")] [Tooltip("Height at the peak of the player's jump")]
            public float jumpHeight = 2f;

            [Tooltip("Time in seconds the player takes to reach the peak of their jump")]
            public float jumpTime = .8f;

            [Tooltip("Curve describing the ascending part of the player's jump")]
            public AnimationCurve jumpCurve;

            [Header("Physics")] [Tooltip("Acceleration due to gravity")]
            public float gravity = -9.8f;

            [Tooltip("Maximum fall speed")] public float terminalVelocity = -5f;

            [Tooltip("Time in seconds the player can jump after leaving the ground")]
            public float jumpGracePeriod = .1f;

            [Tooltip("Distance from the ground at which the player is considered grounded")]
            public float groundDistance = .025f;

            private Transform _transform;
            private BoxCollider2D _collider;
            
            [Header("Internal")]
            [ReadOnly] public float _xVelocity;
            [ReadOnly] public float _yVelocity;
            // Hacky good-feeling jumping: completely ignore velocity and use the jump curve instead
            [ReadOnly] public bool _isJumping;
            [ReadOnly] public float _jumpTimer;
            [ReadOnly] public float _jumpGraceTimer;

        public void TryJump() {
            if (CanJump()) Jump();
        }

        public void Jump() {
            _isJumping = true;
            _jumpTimer = 0;
        }

        public bool CanJump() {
            if (_jumpGraceTimer < jumpGracePeriod) return true;
            return IsGrounded();
        }

        public bool IsGrounded() {
            return GetGroundDistance() < groundDistance;
        }

        public float GetGroundDistance() {
            Bounds bounds = _collider.bounds;
            RaycastHit2D hit =
                Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.down, terminalVelocity + groundDistance);
            if (hit.collider == null) return float.PositiveInfinity;
            return hit.distance;
        }
        
        public float GetCeilingDistance() {
            Bounds bounds = _collider.bounds;
            RaycastHit2D hit =
                Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.up, terminalVelocity + groundDistance);
            if (hit.collider == null) return float.PositiveInfinity;
            return hit.distance;
        }

        public float GetRightDistance() {
            Bounds bounds = _collider.bounds;
            RaycastHit2D hit =
                Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.right, _xVelocity + groundDistance + .5f);
            if (hit.collider == null) return float.PositiveInfinity;
            return hit.distance;
        }

        public float GetLeftDistance() {
            Bounds bounds = _collider.bounds;
            RaycastHit2D hit =
                Physics2D.BoxCast(bounds.center, bounds.size, 0, Vector2.left, -_xVelocity + groundDistance + .5f);
            if (hit.collider == null) return float.PositiveInfinity;
            return hit.distance;
        }

        private void Start() {
            _transform = transform;
            _collider = GetComponent<BoxCollider2D>();
        }

        private void FixedUpdate() {
            if (_isJumping) {
                float jumpProgress = _jumpTimer / jumpTime;
                if (jumpProgress >= 1) {
                    _isJumping = false;
                }
                else {
                    float distance =
                        (jumpCurve.Evaluate(_jumpTimer + Time.deltaTime) - jumpCurve.Evaluate(_jumpTimer)) * jumpHeight;
                    _transform.position += Vector3.up * distance;
                    _jumpTimer += Time.deltaTime;
                }
            } else {
                float distanceToGround = GetGroundDistance();
                if (distanceToGround < groundDistance) {
                    _jumpGraceTimer = 0;
                }
                else {
                    _jumpGraceTimer += Time.deltaTime;
                    _yVelocity = Mathf.Max(_yVelocity + gravity * Time.deltaTime, terminalVelocity);

                    float fallDistance = Mathf.Min(distanceToGround - groundDistance, -_yVelocity * Time.deltaTime);
                    _transform.position += Vector3.down * fallDistance;
                }
            }

            if (_xVelocity > 0) {
                float moveDistance = Mathf.Min(GetRightDistance(), _xVelocity * Time.deltaTime);
                _transform.position += Vector3.right * moveDistance;
            }
            else if (_xVelocity < 0) {
                float moveDistance = Mathf.Min(GetLeftDistance(), -_xVelocity * Time.deltaTime);
                _transform.position += Vector3.left * moveDistance;
            }
        }

        private void Update() {
            float xInput = Input.GetAxis("Horizontal");
            if (Mathf.Abs(xInput) > moveDeadzone) {
                _xVelocity = Mathf.Min(_xVelocity + moveAcceleration * Time.deltaTime * xInput, moveSpeed);
            }
            else {
                _xVelocity *= moveDecelerationMultiplier;
                if (Mathf.Abs(_xVelocity) < 0.01f) {
                    _xVelocity = 0;
                }
            }

            if (Input.GetButtonDown("Jump")) TryJump();
        }
    }
}