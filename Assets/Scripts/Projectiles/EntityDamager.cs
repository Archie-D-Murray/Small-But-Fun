using Entity;

using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Projectiles {
    public class EntityDamager : MonoBehaviour {
        private float damage;

        public void Init(float damage) {
            this.damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            if (collider.TryGetComponent(out Health health)) {
                health.Damage(damage, transform.position);
            }
            Destroy(gameObject);
        }
    }
}