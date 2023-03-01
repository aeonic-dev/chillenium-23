using System;
using UI;
using Unity.VisualScripting;
using UnityEngine;

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
        [Tooltip("Time in seconds the player can jump after leaving the ground or before touching it.")]
        public float jumpGracePeriod = .05f;
        [Tooltip("A cooldown for jumping to avoid cheaty double jumps")]
        public float jumpCooldown = .05f;
        [Tooltip("Distance from the player's origin to the ground at which the player is considered grounded")]
        public float groundDistance = .5f;
        [Tooltip("Threshold for the ground distance at which the player is considered grounded")]
        public float groundThreshold = .1f;
        
        private static Player _instance;

        private Transform _transform;
        private Rigidbody2D _rigidbody;
        private CapsuleCollider2D _capsuleCollider;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Interaction _hoveredInteraction;
        private float _groundTimer;
        private float _jumpTimer;
        private float _jumpQueueTimer;
        private bool _jumpQueued;

        public static Player Get() {
            if (_instance.IsDestroyed()) return FindObjectOfType<Player>();
            return _instance;
        }

        private bool CanJump() {
            return _groundTimer <= jumpGracePeriod;
        }

        private bool IsGrounded() {
            RaycastHit2D hit = Physics2D.Raycast(_transform.position - Vector3.up * groundDistance, Vector2.down, groundThreshold);
            return hit.collider != null;
        }

        private void OnTriggerEnter2D(Collider2D col) {
            Interaction interaction = col.gameObject.GetComponent<Interaction>();
            if (interaction != null && interaction.IsInteractable()) {
                _hoveredInteraction = interaction;
                InteractionIndicator.Show(_hoveredInteraction.transform.position);
            }
        }

        private void OnTriggerExit2D(Collider2D col) {
            Interaction interaction = col.gameObject.GetComponent<Interaction>();
            if (interaction != null && interaction == _hoveredInteraction) {
                _hoveredInteraction = null;
                InteractionIndicator.Hide();
            }
        }

        private void Awake() {
            _instance = this;
        }

        private void Start() {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody2D>();
            _capsuleCollider = GetComponent<CapsuleCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate() {
            float xInput = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(xInput) > moveDeadzone) {
                if (Mathf.Abs(_rigidbody.velocity.x) < moveSpeed)
                    _rigidbody.AddForce(Vector2.right * (xInput * moveAcceleration), ForceMode2D.Impulse);
            } else {
                _rigidbody.velocity *= new Vector2(moveDecelerationMultiplier, 1);
            }
            
            if (xInput != 0) _spriteRenderer.flipX = xInput > 0;
        }

        private void Update() {
            if (IsGrounded()) _groundTimer = 0;
            _groundTimer += Time.deltaTime;
            _jumpTimer += Time.deltaTime;
            _jumpQueueTimer += Time.deltaTime;

            if (_jumpQueued) {
                if (_jumpQueueTimer > jumpGracePeriod) _jumpQueued = false;
                else if (CanJump()) {
                    _jumpQueued = false;
                    _jumpTimer = 0;
                    _groundTimer = jumpGracePeriod + 1;
                    _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                }
            } else if (Input.GetButtonDown("Jump")) {
                if (CanJump()) {
                    if (_jumpTimer > jumpCooldown) {
                        _jumpQueued = false;
                        _jumpTimer = 0;
                        _groundTimer = jumpGracePeriod + 1;
                        _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    }
                } else {
                    _jumpQueued = true;
                    _jumpQueueTimer = 0;
                }
            }

            if (_hoveredInteraction != null && !_hoveredInteraction.IsInteractable()) _hoveredInteraction = null;
            if (Input.GetButtonDown("Interact") && _hoveredInteraction != null && _hoveredInteraction.IsInteractable()) {
                _hoveredInteraction.Interact();
                _animator.SetTrigger("Interact");
                Invoke(nameof(ResetInteractTrigger), .5f);
            }
        }

        private void ResetInteractTrigger() {
            _animator.ResetTrigger("Interact");
        }

        private void LateUpdate() {
            float xVel = _rigidbody.velocity.x;
            _animator.SetFloat("Speed", Mathf.Abs(xVel / moveSpeed));
            _animator.SetBool("Jumping", !IsGrounded() && Mathf.Abs(_rigidbody.velocity.y) > .1f);
        }
    }
}