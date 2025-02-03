using UnityEngine;

using Entity.Player;

namespace UI {
    public class Finish : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D collider) {
            if (collider.gameObject.Has<PlayerController>()) {
                PlayerUI.Instance.PlayerFinish();
            }
        }
    }
}