using UnityEngine;
using Tags;

namespace Projectiles {

    public class LinearProjectileMover : MonoBehaviour {
        private Rigidbody2D rb;
        bool destroyObject;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Init(float speed, bool destroyObject = true) {
            if (!destroyObject) {
                rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
            } else {
                rb.velocity = transform.up * speed;
            }
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            if (!destroyObject) { return; }
            if (collider.gameObject.Has<Wall>()) {
                Destroy(gameObject);
            }
        }
    }
}