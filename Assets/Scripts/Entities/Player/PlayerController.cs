using UnityEngine;

using Utilities;

namespace Entity.Player {
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour {

        private readonly int _speedID = Animator.StringToHash("speed");

        private Rigidbody2D _rb2D;
        private Animator _animator;
        private SizeController _sizeController;

        [Header("Movement")]
        [SerializeField] private float _speed = 0f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _acceleration = 3f;
        [SerializeField] private float _drag = 2f;

        private void Start() {
            _rb2D = GetComponent<Rigidbody2D>();
            _animator = GetComponentInChildren<Animator>();
            _sizeController = GetComponent<SizeController>();
        }

        private void FixedUpdate() {
            Vector2 input = Vector2.ClampMagnitude(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")), 1f);
            bool movePressed = input.sqrMagnitude >= 0.01f;
            _speed = Mathf.MoveTowards(_speed, movePressed ? _maxSpeed * _sizeController.SpeedModifier : 0f, Time.fixedDeltaTime * (movePressed ? _acceleration : _drag));
            if (movePressed) {
                _rb2D.velocity = input * _speed;
            } else {
                _rb2D.velocity = Vector3.MoveTowards(_rb2D.velocity, Vector3.zero, Time.fixedDeltaTime * _drag);
            }
            _rb2D.velocity = Vector2.ClampMagnitude(_rb2D.velocity, _maxSpeed);
            _animator.SetFloat(_speedID, _rb2D.velocity.sqrMagnitude);
        }
    }
}