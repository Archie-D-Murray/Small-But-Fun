using UnityEngine;

using Utilities;

namespace Entity.Player {
    public class RegenerationController : MonoBehaviour {
        [SerializeField] private float _regenPercent = 0.02f;
        [SerializeField] private float _multiplier = 0f;
        [SerializeField] private float _regenTickSpeed = 0.20f;

        private Health _health;
        private CountDownTimer _regenTimer = new CountDownTimer(0f);

        private void Start() {
            _health = GetComponent<Health>();
        }

        private void FixedUpdate() {
            _regenTimer.Update(Time.fixedDeltaTime);
            if (_regenTimer.IsFinished) {
                _health.Heal(_health.MaxHealth * _regenPercent * _multiplier);
                _regenTimer.Reset(_regenTickSpeed);
            }
        }

        public void Increase() {
            _multiplier += 1f;
        }
    }
}