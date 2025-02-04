using UnityEngine;

namespace Entity.Player {
    public class PlayerAnimationHandler : MonoBehaviour {
        private WeaponController _weaponController;

        private void Start() {
            _weaponController = GetComponentInParent<WeaponController>();
        }

        public void AttackFrame() {
            _weaponController.Attack();
        }
    }
}