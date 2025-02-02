using UnityEngine;

using Utilities;
namespace Loot {
    public class LootAnimator : MonoBehaviour {
        [SerializeField] private float _speed = 2f;

        private void FixedUpdate() {
            for (int i = 0; i < transform.childCount; i++) {
                float multiplier = i % 2 == 0 ? 1f : -1f;
                transform.GetChild(i).localPosition = Helpers.FromRadians(multiplier * Mathf.PI * 2f * _speed * Time.fixedTime) * (0.5f + 0.25f * Mathf.Sin(multiplier * Time.fixedTime));
            }
        }
    }
}