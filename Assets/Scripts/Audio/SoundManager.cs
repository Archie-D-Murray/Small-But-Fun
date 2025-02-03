using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

using Utilities;

namespace Audio {

    [Serializable]
    public class SoundEffect {
        public AudioClip Clip;
        public SoundEffectType Type;
    }

    [DefaultExecutionOrder(-99)]
    public class SoundManager : Singleton<SoundManager> {

        public AudioMixer Mixer;

        protected override void Awake() {
            base.Awake();
            SoundManager.StartSingleton();
        }
    }
}