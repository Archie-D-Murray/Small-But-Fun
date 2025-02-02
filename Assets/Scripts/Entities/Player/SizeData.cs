using System;

namespace Entity.Player {
    [Serializable]
    public class SizeData {
        public float SizeModifier = 1f;
        public float DamageModifier = 1f;
        public float SpeedModifier = 1f;
        public float DamageTakenModifier = 1f;

        public SizeData(float sizeModifier, float damageModifier, float speedModifier, float damageTakenModifier) {
            SizeModifier = sizeModifier;
            DamageModifier = damageModifier;
            SpeedModifier = speedModifier;
            DamageTakenModifier = damageTakenModifier;
        }
    }
}