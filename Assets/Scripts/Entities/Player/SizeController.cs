using UnityEngine;

using System;
using System.Collections.Generic;
using Utilities;

namespace Entity.Player {
    public class SizeController : MonoBehaviour {
        [SerializeField]
        private SizeData[] _sizeData = {
            new SizeData(0.33f, 0.5f, 1.5f, 2f),
            new SizeData(0.66f, 0.75f, 1.25f, 1.5f),
            new SizeData(1f, 1f, 1f, 1f),
            new SizeData(1.33f, 1.25f, 0.75f, 0.75f),
            new SizeData(1.66f, 1.5f, 0.5f, 0.5f)
        };
        [SerializeField] private int _sizeIndex = 2;
        [SerializeField] private float _colliderDefaultSize;
        [SerializeField] private Vector3 _rendererDefaultScale;
        [SerializeField] private CountDownTimer _sizeTimer = new CountDownTimer(0f);
        [SerializeField] private float _sizeTime = 3f;

        private CircleCollider2D _collider;
        private SpriteRenderer _spriteRenderer;
        private Health _health;

        public float SizeModifier => _sizeData[_sizeIndex].SizeModifier;
        public float DamageModifier => _sizeData[_sizeIndex].DamageModifier;
        public float SpeedModifier => _sizeData[_sizeIndex].SpeedModifier;
        public float DamageTakenModifier => _sizeData[_sizeIndex].DamageTakenModifier;

        private void Start() {
            _collider = GetComponent<CircleCollider2D>();
            _health = GetComponent<Health>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _colliderDefaultSize = _collider.radius;
            _rendererDefaultScale = _spriteRenderer.transform.localScale;
        }

        private void Update() {
            _sizeTimer.Update(Time.deltaTime);
            if (_sizeTimer.IsFinished) {
                if (Input.GetKeyDown(KeyCode.Q)) {
                    SetSize(_sizeIndex - 1);
                    _sizeTimer.Reset(_sizeTime);
                }
                if (Input.GetKeyDown(KeyCode.E)) {
                    SetSize(_sizeIndex + 1);
                    _sizeTimer.Reset(_sizeTime);
                }
            }
        }

        private void SetSize(int size) {
            _sizeIndex = Mathf.Clamp(size, 0, _sizeData.Length - 1);
            _spriteRenderer.transform.localScale = _rendererDefaultScale * _sizeData[_sizeIndex].SizeModifier;
            _collider.radius = _colliderDefaultSize * _sizeData[_sizeIndex].SizeModifier;
            _health.DamageModifier = _sizeData[_sizeIndex].DamageTakenModifier;
        }
    }
}