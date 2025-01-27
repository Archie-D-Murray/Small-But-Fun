using UnityEngine;

using Utilities;

namespace Entities.Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb2D;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        [SerializeField] private CircleCollider2D _collider;

        [Header("Movement")]
        [SerializeField] private float _speed = 0f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _teleportCooldown = 0.75f;
        [SerializeField] private float _acceleration = 3f;
        [SerializeField] private float _drag = 2f;
        [SerializeField] private CountDownTimer _teleportTimer = new CountDownTimer(0f);

        [Header("Size")]
        [SerializeField] private int _sizeIndex = 2;
        [SerializeField] private float[] _sizes = new float[5] { 0.33f, 0.66f, 1f, 1.33f, 1.66f };
        [SerializeField] private float _colliderDefaultSize;
        [SerializeField] private Vector3 _rendererDefaultScale;

        private void Start() {
            _rb2D = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            _collider = GetComponent<CircleCollider2D>();
            _colliderDefaultSize = _collider.radius;
            _rendererDefaultScale = _spriteRenderer.transform.localScale;
        }

        private void FixedUpdate() {
            Vector2 input = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);
            bool movePressed = input.sqrMagnitude >= 0.01f;
            _speed = Mathf.MoveTowards(_speed, movePressed ? _maxSpeed : 0f, Time.fixedDeltaTime * (movePressed ? _acceleration : _drag));
            if (movePressed) {
                _rb2D.velocity = input * _speed;
            } else {
                _rb2D.velocity = Vector3.MoveTowards(_rb2D.velocity, Vector3.zero, Time.fixedDeltaTime * _drag);
            }
            _rb2D.velocity = Vector2.ClampMagnitude(_rb2D.velocity, _maxSpeed);
            _teleportTimer.Update(Time.fixedDeltaTime);
        }

        private void Update() {
            if (Application.isFocused && Input.GetMouseButtonDown(1) && _teleportTimer.IsFinished) { // Right click
                transform.position = Utilities.Helpers.Instance.WorldMousePosition(); // TODO: Fix this
                _teleportTimer.Reset(_teleportCooldown);
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                SetSize(--_sizeIndex);
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                SetSize(++_sizeIndex);
            }
        }

        private void SetSize(int size) {
            _sizeIndex = Mathf.Clamp(size, 0, _sizes.Length - 1);
            _spriteRenderer.transform.localScale = _rendererDefaultScale * _sizes[_sizeIndex];
            _collider.radius = _colliderDefaultSize * _sizes[_sizeIndex];
        }
    }
}