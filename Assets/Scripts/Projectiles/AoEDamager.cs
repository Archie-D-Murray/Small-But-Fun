using System;

using Entity;

using UnityEngine;

using Utilities;

namespace Projectiles {
    public class AoEDamager : MonoBehaviour {
        private float damage;
        private float radius;
        private LayerMask _mask;

        public Action<Vector3> onHit;

        public AoEDamager Init(float damage, float radius, LayerMask mask) {
            this.damage = damage;
            this.radius = radius;
            _mask = mask;
            return this;
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            foreach (Collider2D hit in Physics2D.OverlapCircleAll(transform.position, radius, _mask)) {
                if (hit.TryGetComponent(out Health health)) {
                    health.Damage(damage);
                }
            }
            onHit?.Invoke(transform.position);
            Destroy(gameObject);
        }
    }
}