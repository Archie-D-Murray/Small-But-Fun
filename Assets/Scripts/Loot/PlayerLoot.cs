using Entity;
using Entity.Player;

using UnityEngine;

namespace Loot {

    public enum LootType { Health, Regen, Damage }

    public class PlayerLoot : MonoBehaviour {

        [SerializeField] private LootType _type;
        [SerializeField] private float _magnitude;

        private void OnTriggerEnter2D(Collider2D player) {
            if (!player.TryGetComponent(out Health health)) {
                return;
            }
            health.Heal(health.MissingHealth);
            switch (_type) {
                case LootType.Health:
                    health.UpdateMaxHealth(health.MaxHealth * _magnitude);
                    break;

                case LootType.Damage:
                    if (player.TryGetComponent(out WeaponController weaponController)) {
                        weaponController.DamageModifier(_magnitude);
                    }
                    break;

                case LootType.Regen:
                    if (player.gameObject.Has<RegenerationController>()) {
                        return;
                    }
                    player.gameObject.AddComponent<RegenerationController>();
                    break;
            }
            Destroy(gameObject);
        }
    }
}