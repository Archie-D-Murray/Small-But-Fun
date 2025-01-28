using Entity;

using UnityEngine;

namespace Projectiles {
    public class HomingProjectileMover : MonoBehaviour {
        private float speed;
        private Rigidbody2D rb2D;
        Transform target;
        Vector2 position;
        float maxTilt = 20;
        bool tilt = false;

        public void Init(float speed, Vector3 position, bool tilt = false) {
            this.speed = speed;
            this.position = position;
            rb2D = GetComponent<Rigidbody2D>();
            this.tilt = tilt;
        }

        public void Init(float speed, Transform target, bool tilt = false) {
            this.speed = speed;
            this.target = target;
            this.position = target.position;
            rb2D = GetComponent<Rigidbody2D>();
            this.tilt = tilt;
        }

        private void FixedUpdate() {
            if (target) {
                position = target.position;
            }

            rb2D.velocity = (position - rb2D.position).normalized * (speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Euler(0, 0, -rb2D.velocity.x * 50 / speed * maxTilt);
        }
    }
}