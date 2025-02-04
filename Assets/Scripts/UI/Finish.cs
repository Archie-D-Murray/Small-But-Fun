using UnityEngine;

using Entity.Player;
using Audio;

namespace UI {
    public class Finish : MonoBehaviour {

        [SerializeField] private SFXEmitter _emitter;

        private void Start() {
            _emitter = GetComponent<SFXEmitter>();
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            if (collider.gameObject.Has<PlayerController>()) {
                PlayerUI.Instance.PlayerFinish();
            }
        }

        public void PlayUnlock() {
            _emitter.Play(SoundEffectType.Key);
        }
    }
}