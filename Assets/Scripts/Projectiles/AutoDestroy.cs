using UnityEngine;

namespace Projectiles {
    public class AutoDestroy : MonoBehaviour {
        [SerializeField] private float duration = 2;
        [SerializeField] private bool isManual = false;
        public void Init(float duration) {
            if (isManual) { return; }
            Destroy(gameObject, duration);
        }

        private void Start() {
            if (isManual) { Destroy(gameObject, duration); }
        }
    }
}