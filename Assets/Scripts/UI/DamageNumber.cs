using UnityEngine;

using TMPro;

using Utilities;

namespace UI {
    public class DamageNumber : MonoBehaviour {
        [SerializeField] private TMP_Text _damageNumber;
        [SerializeField] private CountDownTimer _duration = new CountDownTimer(2f);

        public void Init(float damage, Vector2 position) {
            transform.position = position;
            _damageNumber.text = damage.ToString("0");
            _duration.Start();
        }

        private void FixedUpdate() {
            _duration.Update(Time.fixedDeltaTime);
            if (_duration.IsFinished) {
                Destroy(gameObject);
            } else {
                transform.position += Vector3.up * Time.fixedDeltaTime;
                _damageNumber.alpha = 1f - _duration.Progress();
            }
        }
    }
}